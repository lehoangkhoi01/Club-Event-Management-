using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PagingRequest
    {
        public virtual int? PageIndex { get; set; } = 0;
        public virtual int? PageSize { get; set; } = 5;

        public PagingRequest() { }

        public PagingRequest(int? pageIndex, int? pageSize)
        {
            if (pageIndex.HasValue && pageSize.Value > 0)
                PageIndex = pageIndex.Value;

            if (pageSize.HasValue && pageSize.Value > 0)
                PageSize = pageSize.Value;
        }
    }
}
