using System;
using System.Linq;
using System.Threading.Tasks;
using Barista.Common;
using Barista.Common.OperationResults;
using Barista.Contracts.Commands.AuthenticationMeans;
using Barista.Contracts.Commands.Sale;
using Barista.Contracts.Commands.SaleStrategy;
using Barista.Contracts.Commands.Swipe;
using Barista.Swipe.Commands;
using Barista.Swipe.Models;
using Barista.Swipe.Services;

namespace Barista.Swipe.Handlers.Swipe
{
    public class ProcessSwipeHandler : ICommandHandler<IProcessSwipe, IIdentifierResult>
    {
        private readonly IAccountingService _acSvc;
        private readonly IAccountingGroupsService _agSvc;
        private readonly IIdentityService _idSvc;
        private readonly IOffersService _offSvc;
        private readonly IPointsOfSaleService _posSvc;
        private readonly IProductsService _prodSvc;
        private readonly IBusPublisher _busPublisher;

        public ProcessSwipeHandler(IProductsService prodSvc, IPointsOfSaleService posSvc, IOffersService offSvc, IIdentityService idSvc, IAccountingGroupsService agSvc, IBusPublisher busPublisher, IAccountingService acSvc)
        {
            _prodSvc = prodSvc;
            _posSvc = posSvc;
            _offSvc = offSvc;
            _idSvc = idSvc;
            _agSvc = agSvc;
            _busPublisher = busPublisher;
            _acSvc = acSvc;
        }

        protected async Task<(Product, decimal)> RetrieveProductAndCost(Guid offerId, Guid pointOfSaleId, decimal quantity)
        {
            var offer = await _offSvc.GetOffer(offerId);
            if (offer is null)
                throw new BaristaException("invalid_offer_id", $"The offer with ID '{offerId}' was not found.");
            else if (offer.PointOfSaleId != pointOfSaleId)
                throw new BaristaException("invalid_offer_id", $"The offer with ID '{offerId}' is not associated with PoS with ID '{pointOfSaleId}'");

            var product = await _prodSvc.GetProduct(offer.ProductId);
            if (product is null)
                throw new BaristaException("invalid_offer_product_id", $"The offer with ID '{offerId}' offers product with ID '{offer.ProductId}' which was not found.");

            var saleCostNullable = product.RecommendedPrice ?? offer.RecommendedPrice;
            if (saleCostNullable is null)
                throw new BaristaException("invalid_offer_settings", $"The offer with ID '{offerId}' does not specify the recommended cost and neither does product '{offer.ProductId}'");

            return (product, saleCostNullable.Value * quantity);
        }

        protected async Task<(Guid, Guid)> RetrieveMeansIdAndUserId(string authMeansMethod, string authMeansValue)
        {
            var meansIdResolution = await _busPublisher.SendRequest<IResolveAuthenticationMeans, IIdentifierResult>(new ResolveAuthenticationMeans(authMeansMethod, authMeansValue));
            if (!meansIdResolution.Successful)
                throw meansIdResolution.ToException();
            else if (!meansIdResolution.Id.HasValue)
                throw new BaristaException("means_id_resolution_failed", "Could not resolve ID of located authentication means");

            var meansId = meansIdResolution.Id.Value;

            var userIdResolution = await _busPublisher.SendRequest<IResolveAuthenticationMeansUserId, IUserIdResolutionResult>(new ResolveAuthenticationMeansUserId(meansId, false));
            if (!userIdResolution.Successful)
                throw new BaristaException("invalid_means", $"Authentication means with ID '{meansId}' is not currently assigned to an user.");
            else if (!userIdResolution.AssignmentId.HasValue)
                throw new BaristaException("assignment_to_user_resolution_failed", $"Could not resolve ID of assignment to user from means '{meansId}'.");
            else if (!userIdResolution.UserId.HasValue)
                throw new BaristaException("user_id_resolution_failed", $"Could not resolve user ID from means '{meansId}'.");

            var assignment = await _idSvc.GetAssignmentToUser(userIdResolution.AssignmentId.Value);
            if (assignment is null)
                throw new BaristaException("invalid_means_assignment_id", $"The user assignment with ID '{userIdResolution.AssignmentId}' (resolved from means with ID '{meansId}') was not found.");

            var now = DateTimeOffset.UtcNow;
            var spendingLimitChecks = assignment.SpendingLimits.ToDictionary(
                sl => sl,
                sl => _acSvc.GetSpendingOfMeans(assignment.Means, sl.Interval == TimeSpan.Zero ? DateTimeOffset.MinValue : now.Subtract(sl.Interval))
            );

            await Task.WhenAll(spendingLimitChecks.Values);

            foreach (var (spendingLimit, actualSpendingTask) in spendingLimitChecks)
            {
                var actualSpending = actualSpendingTask.Result;

                if (actualSpending > spendingLimit.Value)
                    throw new BaristaException("spending_limit_exceeded", $"The swipe was forbidden by the spending limit '{assignment.Id}/{spendingLimit.Id}' with value {spendingLimit.Value} per {spendingLimit.Interval}, current spending in this interval is {actualSpending}");
            }

            return (meansId, userIdResolution.UserId.Value);
        }

        protected async Task<Guid> RetrieveAccountingGroupAndApplySaleStrategy(Guid userId, Guid pointOfSaleId, decimal saleCost)
        {
            var pointOfSale = await _posSvc.GetPointOfSale(pointOfSaleId);
            if (pointOfSale is null)
                throw new BaristaException("invalid_point_of_sale", $"The point of sale with ID '{pointOfSaleId}' was not found.");

            var accountingGroup = await _agSvc.GetAccountingGroup(pointOfSale.ParentAccountingGroupId);
            if (accountingGroup is null)
                throw new BaristaException("invalid_accounting_group", $"The accounting group with ID '{pointOfSale.ParentAccountingGroupId}' to which PoS '{pointOfSaleId}' belongs was not found.");

            var saleStrategyId = pointOfSale.SaleStrategyId ?? accountingGroup.SaleStrategyId;

            var saleStrategyApplicationResult = await _busPublisher.SendRequest<IApplySaleStrategy, IOperationResult>(
                new ApplySaleStrategy(userId, saleStrategyId, saleCost)
            );

            if (!saleStrategyApplicationResult.Successful)
                throw new BaristaException("sale_strategy_application_failed", $"The sale strategy with ID '{saleStrategyId}' rejected the purchase: {saleStrategyApplicationResult.ErrorCode}");

            return accountingGroup.Id;
        }

        public async Task<IIdentifierResult> HandleAsync(IProcessSwipe command, ICorrelationContext correlationContext)
        {
            if (command.Quantity <= 0)
                throw new BaristaException("invalid_quantity", $"Sale cannot be made on less than or zero quantity");

            var productAndCostTask = Task.Run(() => RetrieveProductAndCost(command.OfferId, command.PointOfSaleId, command.Quantity));
            var meansAndUserIdTask = Task.Run(() => RetrieveMeansIdAndUserId(command.AuthenticationMeansMethod, command.AuthenticationMeansValue));
            await Task.WhenAll(productAndCostTask, meansAndUserIdTask);

            var (product, saleCost) = productAndCostTask.Result;
            var (meansId, userId) = meansAndUserIdTask.Result;

            var accountingGroupId = await RetrieveAccountingGroupAndApplySaleStrategy(userId, command.PointOfSaleId, saleCost);
            var saleCreationResult = await _busPublisher.SendRequest<ICreateSale, IIdentifierResult>(new CreateSale(
                Guid.NewGuid(),
                saleCost,
                command.Quantity,
                accountingGroupId,
                userId,
                meansId,
                command.PointOfSaleId,
                product.Id,
                command.OfferId
            ));

            if (saleCreationResult.Successful)
                return new IdentifierResult(saleCreationResult.Id.Value);
            else
                throw saleCreationResult.ToException();
        }
    }
}
