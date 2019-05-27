using System;
using System.ComponentModel.DataAnnotations;
using Barista.Contracts.Commands.Payment;

namespace Barista.Api.Commands.Payment
{
    public class UpdatePayment : IUpdatePayment
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Source { get; set; }

        public string ExternalId { get; set; }
    }
}
