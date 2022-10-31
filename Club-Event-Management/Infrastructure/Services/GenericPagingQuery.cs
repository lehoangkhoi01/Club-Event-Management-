using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public static class GenericPagingQuery
    {
        public static PagingResult<T> Page<T>( this IQueryable<T> query, int pageNumZeroStart, int pageSize)
        {
            if (pageSize < 0) throw new ArgumentOutOfRangeException(nameof(pageSize), "pageSize cannot be zero.");

            if (pageNumZeroStart != 0)
                query = query
                .Skip(pageNumZeroStart * pageSize);

            var totalPage = (int)Math.Ceiling(
                (double)query.Count() / pageSize);
            var values = query.Take(pageSize).ToList();
            return new PagingResult<T>(values, pageNumZeroStart, totalPage, pageSize);
        }

        public class PagingResult<T>
        {
            public List<T> Data { get; set; }
            public int TotalPage { get; set; }
            public int PageSize { get; set; }
            public int PageIndex { get; set; }
            public PagingResult(List<T> values, int pageIndex, int totalPage, int pageSize)
            {
                Data = values;
                TotalPage = totalPage;
                PageSize = pageSize;
                PageIndex = pageIndex;
            }
        }
    }
}
