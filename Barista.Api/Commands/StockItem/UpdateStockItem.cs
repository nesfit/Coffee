﻿using System;
using Barista.Contracts.Commands.StockItem;

namespace Barista.Api.Commands.StockItem
{
    public class UpdateStockItem : IUpdateStockItem
    {
        public UpdateStockItem(Guid id, string displayName, Guid pointOfSaleId)
        {
            Id = id;
            DisplayName = displayName;
            PointOfSaleId = pointOfSaleId;
        }

        public Guid Id { get; }
        public string DisplayName { get; }
        public Guid PointOfSaleId { get; }
    }
}
