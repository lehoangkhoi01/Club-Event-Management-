using ApplicationCore;
using Infrastructure.Services.EventServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventPostServices.QueryObject
{
    public static class EventPostBusinessFilter
    {
        public static IQueryable<EventPost> BusinessFilter(this IQueryable<EventPost> posts, List<int> eventIds)
        {
            return posts.Where(p => eventIds.Contains(p.Event.Id) || !p.Event.IsInternal &&
                            p.Event.EventStatus.EventStatusName != EventStatusEnum.DELETED.ToString() &&
                            p.Event.EventStatus.EventStatusName != EventStatusEnum.DRAFT.ToString());
        }
    }
}
