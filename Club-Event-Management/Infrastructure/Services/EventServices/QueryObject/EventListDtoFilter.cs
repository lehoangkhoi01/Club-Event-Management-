using ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventServices.QueryObject
{
    public static class EventListDtoFilter
    {
        public static IQueryable<EventClubProfile> FilterEvents(this IQueryable<EventClubProfile> eventClubLink, FilterPagingRequest filterPagingRequest)
        {
            var result = eventClubLink.Where(link =>
                link.Event.EventName.Contains(filterPagingRequest.EventName) ||
                link.Event.EventStatus.EventStatusName.Equals(filterPagingRequest.EventStatus) ||
                link.Event.EventCategory.EventCategoryName.Equals(filterPagingRequest.CategoryName) ||
                link.ClubProfile.ClubName.Equals(filterPagingRequest.ClubName));

            if(filterPagingRequest.EventStartTime.HasValue)
                result = result.Where(link => link.Event.EventStartTime.CompareTo(filterPagingRequest.EventStartTime.Value) < 0);

            if (filterPagingRequest.EventEndTime.HasValue)
                result = result.Where(link => link.Event.EventEndTime.CompareTo(filterPagingRequest.EventEndTime.Value) > 0);

            return result.Distinct();
        }
    }
}
