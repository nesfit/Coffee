using System;

namespace Barista.Contracts.Commands.PointOfSaleUserAuthorization
{
    public interface IUpdatePointOfSaleUserAuthorization : ICommand
    {
        Guid PointOfSaleId { get; }
        Guid UserId { get; }
        string Level { get; }
    }
}
