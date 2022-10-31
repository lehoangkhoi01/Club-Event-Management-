using ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventServices.QueryObject
{
    public static class EventListDtoSelect
    {
        public static IQueryable<EventListDto> MapEventsToEventDtos(this IQueryable<Event> events)
        {
            return events.Select(
                ev => new EventListDto
                {
                    Id = ev.Id,
                    EventName = ev.EventName,
                    Description = ev.Description,
                    Place = ev.Place,
                    EventStartTime = ev.EventStartTime.ToString(),
                    EventEndTime = ev.EventEndTime.ToString(),
                    CreatedDate = ev.CreatedDate.ToString(),
                    IsInternal = ev.IsInternal,
                    EventStatus = ev.EventStatus.EventStatusName,
                    Category = ev.EventCategory.EventCategoryName,
                    ClubInfos = ev.ClubProfilesLinks.Select(link => new EventListDto_ClubInfo
                    {
                        ClubName = link.ClubProfile.ClubName,
                        ClubProfileId = link.ClubProfileId,
                        IsOwner = link.IsOwner
                    }).ToArray(),
                });
        }
    }
}
