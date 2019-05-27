using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barista.Contracts.Commands.PointOfSaleUserAuthorization;

namespace Barista.Consistency.Commands
{
    public class DeletePointOfSaleUserAuthorization : IDeletePointOfSaleUserAuthorization
    {
        public DeletePointOfSaleUserAuthorization(Guid pointOfSaleId, Guid userId)
        {
            PointOfSaleId = pointOfSaleId;
            UserId = userId;
        }

        public Guid PointOfSaleId { get; }
        public Guid UserId { get; }
    }
}
