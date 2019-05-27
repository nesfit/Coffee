using System;
using System.Threading.Tasks;
using Barista.Api.Authorization;
using Barista.Api.Commands.Offer;
using Barista.Api.Dto;
using Barista.Api.Models.Offers;
using Barista.Api.Queries;
using Barista.Api.ResourceAuthorization;
using Barista.Api.ResourceAuthorization.Policies;
using Barista.Api.Services;
using Barista.Common;
using Barista.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Api.Controllers
{
    [ApiController]
    [Route("api/offers")]
    public class OffersController : BaseController
    {
        private readonly IOffersService _offersService;
        private readonly IPointOfSaleAuthorizationLoader _posAuthLoader;

        public OffersController(IBusPublisher busPublisher, IOffersService offersService, IPointOfSaleAuthorizationLoader posAuthLoader)
            : base(busPublisher)
        {
            _offersService = offersService ?? throw new ArgumentNullException(nameof(offersService));
            _posAuthLoader = posAuthLoader ?? throw new ArgumentNullException(nameof(posAuthLoader));
        }

        [HttpGet]
        public async Task<ActionResult<IPagedResult<Offer>>> BrowseOffers([FromQuery] BrowseOffers query)
            => Collection(await _offersService.BrowseOffers(query));

        [HttpGet("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(Offer))]
        public async Task<ActionResult<Offer>> GetOffer(Guid id)
            => Single(await _offersService.GetOffer(id));

        [HttpPost]
        [ProducesResponseType(401)]
        [ProducesResponseType(201)]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult> CreateOffer(OfferDto offer)
        {
            await _posAuthLoader.AssertResourceAccessAsync(User, offer.PointOfSaleId, IsAuthorizedUserPolicy.Instance);

            var cmd = new CreateOffer(Guid.NewGuid(), offer.PointOfSaleId, offer.ProductId, offer.RecommendedPrice, offer.StockItemId, offer.ValidSince, offer.ValidUntil);
            return await SendAndHandleIdentifierResultCommand(cmd, nameof(GetOffer));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult> UpdateOffer(Guid id, OfferDto offerDto)
        {
            var offer = await _offersService.GetOffer(id);
            if (offer is null)
                return NotFound();

            await _posAuthLoader.AssertResourceAccessAsync(User, offer.PointOfSaleId, IsAuthorizedUserPolicy.Instance);

            if (offer.PointOfSaleId != offerDto.PointOfSaleId)
                await _posAuthLoader.AssertResourceAccessAsync(User, offerDto.PointOfSaleId, IsAuthorizedUserPolicy.Instance);

            return await SendAndHandleOperationCommand(
                new UpdateOffer(id, offerDto.PointOfSaleId, offerDto.ProductId, offerDto.RecommendedPrice, offerDto.StockItemId, offerDto.ValidSince, offerDto.ValidUntil)
            );
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [Authorize(Policies.IsUser)]
        public async Task<ActionResult> DeleteOffer(Guid id)
        {
            var offer = await _offersService.GetOffer(id);
            if (offer is null)
                return NotFound();

            await _posAuthLoader.AssertResourceAccessAsync(User, offer.PointOfSaleId, IsAuthorizedUserPolicy.Instance);
            return await SendAndHandleOperationCommand(new DeleteOffer(id));
        }
    }
}
