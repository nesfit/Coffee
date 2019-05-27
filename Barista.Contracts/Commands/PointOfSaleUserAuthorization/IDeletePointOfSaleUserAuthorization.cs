using System;

namespace Barista.Contracts.Commands.PointOfSaleUserAuthorization
{
    public interface IDeletePointOfSaleUserAuthorization : ICommand
    {
        Guid PointOfSaleId { get; }
        Guid UserId { get; }
    }
}
