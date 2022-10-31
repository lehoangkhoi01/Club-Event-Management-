using ApplicationCore;
using Infrastructure.Services.EventServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventPostServices.QueryObject
{
    public static class EventActivityBusinessFilter
    {
        public static IQueryable<EventActivity> BusinessFilter(this IQueryable<EventActivity> activities, List<int> eventIds)
        {
            return activities.Where(activity => eventIds.Contains(activity.Event.Id) || !activity.Event.IsInternal &&
                            activity.Event.EventStatus.EventStatusName != EventStatusEnum.DELETED.ToString() &&
                            activity.Event.EventStatus.EventStatusName != EventStatusEnum.DRAFT.ToString());
        }
    }
}
