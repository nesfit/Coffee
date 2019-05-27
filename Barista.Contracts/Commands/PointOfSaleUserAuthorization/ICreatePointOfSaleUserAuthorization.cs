using System;

namespace Barista.Contracts.Commands.PointOfSaleUserAuthorization
{
    public interface ICreatePointOfSaleUserAuthorization : ICommand
    {
        Guid PointOfSaleId { get; }
        Guid UserId { get; }
        string Level { get; }
    }

}
