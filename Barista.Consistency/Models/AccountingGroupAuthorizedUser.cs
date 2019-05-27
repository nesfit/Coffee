using System;

namespace Barista.Consistency.Models
{
    public class AccountingGroupAuthorizedUser
    {
        public Guid AccountingGroupId { get; set; }
        public Guid UserId { get; set; }
    }
}
