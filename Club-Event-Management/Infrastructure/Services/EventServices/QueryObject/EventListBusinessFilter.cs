using ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventServices.QueryObject
{
    public static class EventListDtoBusinessFilter
    {
        public static IQueryable<Event> FilterEventsByBusinessRule(this IQueryable<EventClubProfile> eventClubLink, bool isAdmin, List<int> clubIds)
        {
            if (isAdmin)
                return eventClubLink.Select(link => link.Event).Distinct();
            if (clubIds == null || clubIds.Count == 0)
            {
                return eventClubLink.Select(link => link.Event).Where(ev => !ev.IsInternal).Distinct();
            }
            return eventClubLink.Where(link => !link.Event.IsInternal 
                || clubIds.Contains(link.ClubProfile.ClubProfileId)).Select(link => link.Event).Distinct();
        }

    }
}
