using System.Linq;
using Barista.Common;
using Barista.Users.Domain;
using Barista.Users.Dto;

namespace Barista.Users.Queries
{
    public class BrowseUsers : PagedQuery<UserDto>, IPagedQueryImpl<User>
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string EmailAddressExact { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsActive { get; set; }

        public IQueryable<User> Apply(IQueryable<User> q)
        {
            q = q.ApplySort(SortBy);

            if (FullName != null)
                q = q.Where(u => u.FullName.Contains(FullName));

            if (EmailAddress != null)
                q = q.Where(u => u.EmailAddress.Contains(EmailAddress));

            if (EmailAddressExact is string emailAddressExact)
                q = q.Where(u => u.EmailAddress == emailAddressExact);

            if (IsAdministrator is bool isAdminVal)
                q = q.Where(u => u.IsAdministrator == isAdminVal);

            if (IsActive is bool isActiveVal)
                q = q.Where(u => u.IsActive == isActiveVal);

            return q;
        }
    }
}
