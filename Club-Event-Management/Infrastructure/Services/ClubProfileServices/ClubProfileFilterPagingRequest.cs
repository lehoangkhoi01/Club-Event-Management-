using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ClubProfileServices
{
    public class ClubProfileFilterPagingRequest : PagingRequest
    {
        public string ClubName { get; set; } = String.Empty;
        public int? EventSize { get; set; } = 2;
        public int? StudentProfileSize { get; set; } = 5;

        public ClubProfileFilterPagingRequest() { }
        public ClubProfileFilterPagingRequest(int? pageIndex, int? pageSize) { }

    }
}
