using System;
using System.Linq;
using Barista.Common;
using Barista.Contracts;
using Barista.Identity.Domain;
using Barista.Identity.Dto;

namespace Barista.Identity.Queries
{
    public class BrowseAssignedMeansToUser : IPagedQuery<AuthenticationMeansWithValueDto>, IPagedQueryImpl<AuthenticationMeans>
    {
        private readonly BrowseAssignedMeans _parent;

        public BrowseAssignedMeansToUser(BrowseAssignedMeans parent, Guid userId)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            UserId = userId;
        }

        public Guid UserId { get; }
        public IQueryable<AuthenticationMeans> Apply(IQueryable<AuthenticationMeans> q) => _parent.Apply(q);
        public int CurrentPage => _parent.CurrentPage;
        public int ResultsPerPage => _parent.ResultsPerPage;
        public string[] SortBy => _parent.SortBy;
    }
}
