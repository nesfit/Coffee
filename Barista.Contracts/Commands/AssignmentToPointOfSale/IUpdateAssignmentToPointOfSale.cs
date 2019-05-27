using System;

namespace Barista.Contracts.Commands.AssignmentToPointOfSale
{
    public interface IUpdateAssignmentToPointOfSale : ICommand
    {
        Guid Id { get; }
        Guid MeansId { get; }
        Guid PointOfSaleId { get; }
        DateTimeOffset ValidSince { get; }
        DateTimeOffset? ValidUntil { get; }
    }
}
