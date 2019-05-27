using System.ComponentModel.DataAnnotations;

namespace Barista.Api.Dto
{
    public class CancelSaleByUser
    {
        [Required]
        public string Reason { get; set; }
    }
}
