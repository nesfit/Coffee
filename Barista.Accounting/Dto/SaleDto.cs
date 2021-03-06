﻿using System;

namespace Barista.Accounting.Dto
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public decimal Cost { get; set; }
        public decimal Quantity { get; set; }
        public Guid AccountingGroupId { get; set; }
        public Guid UserId { get; set; }
        public Guid AuthenticationMeansId { get; set; }
        public Guid PointOfSaleId { get; set; }
        public string State { get; set; }
        public Guid OfferId { get; set; }
        public Guid ProductId { get; set; }
        public SaleStateChangeDto[] StateChanges { get; set; }
    }
}
