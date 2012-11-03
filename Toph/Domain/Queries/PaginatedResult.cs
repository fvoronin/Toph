using System;
using System.Collections.Generic;
using Toph.Common;

namespace Toph.Domain.Queries
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(IEnumerable<T> items, int pageNumber = 1, int pageSize = 0, int totalItems = 0)
        {
            Items = items.AsReadOnly();

            PageNumber = Math.Max(pageNumber, 1);
            PageSize = Math.Max(pageSize, Items.Count);
            TotalItems = Math.Max(totalItems, Items.Count);

            TotalPages = (int)Math.Ceiling((double)TotalItems / Math.Max(PageSize, 1));
        }

        public IReadOnlyList<T> Items { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalItems { get; private set; }
        public int TotalPages { get; private set; }
    }
}
