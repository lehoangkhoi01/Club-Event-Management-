using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public static class GenericPagingQuery
    {
        public static PagingResult<T> Page<T>( this IQueryable<T> query, int pageNumOneStart, int pageSize)
        {
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize), "pageSize cannot be less than zero.");

            var count = query.Count();

            if (pageNumOneStart > 1)
                query = query
                .Skip((pageNumOneStart-1) * pageSize);


            var totalPage = (int)Math.Ceiling(
                (double)count / pageSize);
            var values = query.Take(pageSize).ToList();
            return new PagingResult<T>(values, pageNumOneStart, totalPage, pageSize);
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
