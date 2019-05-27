using System;

namespace Barista.Contracts.Commands.AssignmentToPointOfSale
{
    public interface ICreateAssignmentToPointOfSale : ICommand
    {
        Guid Id { get; }
        Guid MeansId { get; }
        DateTimeOffset ValidSince { get; }
        DateTimeOffset? ValidUntil { get; }
        Guid PointOfSaleId { get; }
    }
}
