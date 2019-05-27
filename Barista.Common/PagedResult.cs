using Barista.Contracts;

namespace Barista.Common
{
    public class PagedResult : IPagedResult
    {
        public int CurrentPage { get; }
        public int ResultsPerPage { get; }
        public int TotalPages { get; }
        public int TotalResults { get; }

        public PagedResult(int currentPage, int resultsPerPage, int totalPages, int totalResults)
        {
            CurrentPage = currentPage;
            ResultsPerPage = resultsPerPage;
            TotalPages = totalPages;
            TotalResults = totalResults;
        }
    }
}
