using ApplicationCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventServices.QueryObject
{
    public static class EventListDtoFilter
    {
        public static IQueryable<Event> FilterEvents(this IQueryable<Event> events, EventFilterPagingRequest filterPagingRequest)
        {
            var result = events
                .OrderByDescending(ev => filterPagingRequest.SortByCreatedDate.Value ? ev.CreatedDate : ev.EventStartTime)
                .Where(ev =>
                ev.EventName.Contains(filterPagingRequest.EventName) &&
                (ev.EventStatus.EventStatusName.Equals(filterPagingRequest.EventStatus.ToString()) || filterPagingRequest.EventStatus == EventStatusEnum.UNDEFINED) &&
                (ev.EventCategory.EventCategoryName.Equals(filterPagingRequest.CategoryName) || filterPagingRequest.CategoryName.Equals("")));

            if (filterPagingRequest.ClubId.HasValue)
            {
                result = result.Where(ev => ev.ClubProfilesLinks.Any(link => link.ClubProfileId == filterPagingRequest.ClubId.Value));
            }

            if(filterPagingRequest.EventStartTime.HasValue)
                result = result.Where(ev => ev.EventStartTime.CompareTo(filterPagingRequest.EventStartTime.Value) < 0);

            if (filterPagingRequest.EventEndTime.HasValue)
                result = result.Where(ev => ev.EventEndTime.CompareTo(filterPagingRequest.EventEndTime.Value) > 0);

            return result;
        }
    }
}
