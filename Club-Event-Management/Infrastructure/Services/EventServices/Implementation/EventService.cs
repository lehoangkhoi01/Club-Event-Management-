using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Services.EventServices.QueryObject;

namespace Infrastructure.Services.EventServices.Implementation
{
    public class EventService
    {
        private readonly ClubEventManagementContext _context;

        public EventService(ClubEventManagementContext context)
        {
            _context = context;
        }

        public List<EventListDto> GetEvents(FilterPagingRequest filterPagingRequest, bool isAdmin, List<int> clubIds)
        {
            return _context.EventClubProfiles
                .AsNoTracking()
                .FilterEvents(filterPagingRequest)
                .FilterEventsByBusinessRule(isAdmin, clubIds)
                .MapEventsToEventDtos()
                .Page(filterPagingRequest.PageIndex, filterPagingRequest.PageSize)
                .ToList();
        }
    }
}
