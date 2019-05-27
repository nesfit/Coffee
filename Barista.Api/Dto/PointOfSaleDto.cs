using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class PointOfSaleDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public Guid ParentAccountingGroupId { get; set; }

        public Guid? SaleStrategyId { get; set; }
        public string[] Features { get; set; } = new string[0];
    }
}
