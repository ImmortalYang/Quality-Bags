using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace QualityBags
{
    public class PaginatedList<T>: List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        /// <summary>
        /// Initialize an instance of PaginatedList
        /// </summary>
        /// <param name="items">Original list of items</param>
        /// <param name="count">Total number of elements in data source</param>
        /// <param name="pageIndex">The page number</param>
        /// <param name="pageSize">How many items in one page</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        /// <summary>
        /// Generate a paginated list asynchronously from a queryable data source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
