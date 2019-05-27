using System;
using System.Collections.Generic;
using Barista.Contracts;

namespace Barista.Common.Dto
{
    public class ResultPage<T> : IPagedResult<T>
    {
        public int CurrentPage { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public List<T> Items { get; set; } = new List<T>();
        IEnumerable<T> IPagedResult<T>.Items => Items;
    }
}
