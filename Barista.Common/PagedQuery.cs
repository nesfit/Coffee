using Barista.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Barista.Common
{
    public class PagedQuery : IPagedQuery
    {
        public int CurrentPage { get; set; } = 1;
        public int ResultsPerPage { get; set; } = 10;
        [ModelBinder(BinderType = typeof(StringArrayModelBinder))]
        public string[] SortBy { get; set; } = new string[0];
    }
}
