using System;
using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class AccountingGroupDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public Guid SaleStrategyId { get; set; }
    }
}
