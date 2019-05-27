using System;

namespace Barista.AccountingGroups.Dto
{
    public class UserAuthorizationDto
    {
        public Guid AccountingGroupId { get; set; }
        public Guid UserId { get; set; }
        public string Level { get; set; }
    }
}
