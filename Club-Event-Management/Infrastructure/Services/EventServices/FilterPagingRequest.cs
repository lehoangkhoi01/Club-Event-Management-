using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventServices
{
    public class FilterPagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string EventName { get; set; }
        public DateTime? EventStartTime { get; set; }
        public DateTime? EventEndTime { get; set; }
        public String CategoryName { get; set; }
        public String EventStatus { get; set; }
        public String ClubName { get; set; }
    }
}
