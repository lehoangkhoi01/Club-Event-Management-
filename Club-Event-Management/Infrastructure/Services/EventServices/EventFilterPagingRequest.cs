using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventServices
{
    public class EventFilterPagingRequest : PagingRequest
    {
        public int? ClubId { get; set; }
        public string EventName { get; set; } = String.Empty;
        [DataType(DataType.Date)]
        public DateTime? EventStartTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EventEndTime { get; set; }
        public String CategoryName { get; set; } = String.Empty;
        public EventStatusEnum EventStatus { get; set; } = EventStatusEnum.UNDEFINED;
        public bool? SortByCreatedDate { get; set; } = false;

        public EventFilterPagingRequest() { }

        public EventFilterPagingRequest(int? pageIndex, int? pageSize) : base(pageIndex, pageSize) { }
    }

    public enum EventStatusEnum
    {
        PUBLISHED, DRAFT, CANCELLED, DELETED, PAST, UNDEFINED
    }
}
