using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Services.EventServices.QueryObject;
using ApplicationCore;
using static Infrastructure.Services.GenericPagingQuery;
using StatusGeneric;
using Infrastructure.Services.FirebaseServices.NotificationService;

namespace Infrastructure.Services.EventServices.Implementation
{
    public class EventService
    {
        private readonly ClubEventManagementContext _context;
        private readonly NotificationService _notificationService;

        public EventService(ClubEventManagementContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        //Get all events available for student in clubIds
        public List<int> GetEventIdsFromClubIds(List<int> clubIds)
        {
            return _context.EventClubProfile.AsNoTracking().Where(link => !link.Event.IsInternal
                || clubIds.Contains(link.ClubProfileId)).Select(link => link.Event.Id).Distinct().ToList();
        }

        public async Task<PagingResult<EventListDto>> GetFollowEvents(PagingRequest pagingRequest, int studentId, List<int> clubIds)
        {
            var followEventIdsFuture = _notificationService.GetFollowEventIds(studentId);
            var availableEventIds = GetEventIdsFromClubIds(clubIds);
            var followEventIdsList = (await followEventIdsFuture).ToHashSet();
            followEventIdsList.RemoveWhere(id => !availableEventIds.Contains(Int16.Parse(id)));
            return _context.Events.AsNoTracking()
                .Where(ev => followEventIdsList.Contains(ev.Id.ToString()) && (ev.EventStatus.EventStatusName.Equals(EventStatusEnum.PUBLISHED.ToString()) || ev.EventStatus.EventStatusName.Equals(EventStatusEnum.PAST.ToString())))
                .OrderBy(ev => ev.Id)
                .MapEventsToEventDtos()
                .Page(pagingRequest.PageIndex.Value, pagingRequest.PageSize.Value);
            
        }

        public PagingResult<EventListDto> GetDraftEvent(EventFilterPagingRequest filterPagingRequest, bool isAdmin, List<int> ownedClubIds)
        {
            IQueryable<Event> queryContext = null;
            if (ownedClubIds != null && ownedClubIds.Count != 0)
            {
                var ownedEventIds = _context.EventClubProfile.AsNoTracking().Where(link => 
                ownedClubIds.Contains(link.ClubProfileId)).Select(link => link.Event.Id).Distinct().ToList();

                queryContext = _context.Events.AsNoTracking().Where(ev => ev.EventStatus.EventStatusName.Equals(EventStatusEnum.DRAFT.ToString()) && ownedEventIds.Contains(ev.Id));
            }else if (isAdmin)
            {
                queryContext = _context.Events.AsNoTracking().Where(ev => ev.EventStatus.EventStatusName.Equals(EventStatusEnum.DRAFT.ToString()));
            }
            return queryContext
                .FilterEvents(filterPagingRequest)
                .MapEventsToEventDtos()
                .Page(filterPagingRequest.PageIndex.Value, filterPagingRequest.PageSize.Value);
        }

        public PagingResult<EventListDto> GetEvents(EventFilterPagingRequest filterPagingRequest, bool isAdmin, List<int> clubIds)
        {
            IQueryable<Event> queryContext = null;
            if (!isAdmin && (filterPagingRequest.EventStatus == EventStatusEnum.DELETED || filterPagingRequest.EventStatus == EventStatusEnum.DRAFT))
            {
                filterPagingRequest.EventStatus = EventStatusEnum.UNDEFINED;
            }

            if(clubIds != null && clubIds.Count != 0)
            {
                var eventIds = GetEventIdsFromClubIds(clubIds);

                queryContext = _context.Events.AsNoTracking().Where(ev => !ev.IsInternal || eventIds.Contains(ev.Id));
            }
            else if (isAdmin)
            {
                queryContext = _context.Events.AsNoTracking();
            }
            else
            {
                queryContext = _context.Events.AsNoTracking().Where(ev => !ev.IsInternal 
                && (ev.EventStatus.EventStatusName.Equals(EventStatusEnum.PUBLISHED.ToString()) || ev.EventStatus.EventStatusName.Equals(EventStatusEnum.PAST.ToString())));
                filterPagingRequest.EventStatus = EventStatusEnum.UNDEFINED;
            }
            return queryContext
                .FilterEvents(filterPagingRequest)
                .MapEventsToEventDtos()
                .Page(filterPagingRequest.PageIndex.Value, filterPagingRequest.PageSize.Value);
        }

        public EventDetailDto GetEvent(int postSize, int activitySize, int eventId)
        {
            var resultEventDetail = _context.Events.AsNoTracking()
                .Where(ev => ev.Id == eventId)
                .MapEventToEventDetailDto(activitySize, postSize)
                .FirstOrDefault();

            return resultEventDetail;
        }

        public async Task<IEnumerable<EventCategory>> GetEventCategories()
        {
            return await _context.EventCategories.AsQueryable().ToListAsync();
        }

        public IStatusGeneric<Event> CreateEvent(CreateEventRequest createEventRequest, int owningClubProfileId)
        {
            var status = new StatusGenericHandler<Event>();

            //check category
            var eventCategory = _context.EventCategories.AsQueryable().Where(cat => cat.EventCategoryName == createEventRequest.EventCategory).FirstOrDefault();
            if(eventCategory == null)
            {
                return status.AddError("Event category is invalid", nameof(createEventRequest.EventCategory));
            }

            //check club profiles
            var clubProfilesIds = _context.ClubProfiles.AsQueryable().Where(club => createEventRequest.ClubProfileIds.Contains(club.ClubProfileId)).ToList();
            if(clubProfilesIds.Count() != createEventRequest.ClubProfileIds.Count())
            {
                return status.AddError("Club profile ids are invalid", nameof(createEventRequest.ClubProfileIds));
            }

            var owningClubProfile = _context.ClubProfiles.Find(owningClubProfileId);
            if(owningClubProfile == null)
            {
                return status.AddError("Owning club profile id is invalid", nameof(owningClubProfileId));
            }

            var newEvent = new Event()
            {
                EventName = createEventRequest.EventName,
                EventStatus = _context.EventStatuses.AsQueryable().Where(status => status.EventStatusName == createEventRequest.EventStatus).First(),
                CreatedDate = DateTime.Now,
                Description = createEventRequest.Description,
                EventCategory = eventCategory,
                EventEndTime = createEventRequest.EventEndTime,
                EventStartTime = createEventRequest.EventStartTime,
                IsInternal = createEventRequest.IsInternal,
                UpdatedDate = DateTime.Now,
                Place = createEventRequest.Place,
                Image = createEventRequest.Images
            };

            var clubProfileLink = new List<EventClubProfile>();
            clubProfilesIds.ForEach(club => clubProfileLink.Add(new EventClubProfile { ClubProfile = club, Event = newEvent, IsOwner = false }));
            clubProfileLink.Add(new EventClubProfile { ClubProfile = owningClubProfile, Event = newEvent, IsOwner = true });

            newEvent.ClubProfilesLinks = clubProfileLink;

            _context.Events.Add(newEvent);
            _context.SaveChanges();
            if(newEvent.EventStatus.EventStatusName == EventStatusEnum.PUBLISHED.ToString())
            {
                //generate notification
                newEvent.ClubProfilesLinks.ForEach(link =>
                {
                    var newNoti = new NotificationDto
                    {
                        ActionType = ActionType.CREATE.ToString(),
                        ClubId = link.ClubProfileId,
                        SubjectId = newEvent.Id,
                        SubjectName = newEvent.EventName,
                        SubjectType = SubjectType.EVENT.ToString()
                    };
                    _notificationService.PublishNotification(newNoti);
                });
            }
            return status.SetResult(newEvent);


        }

        public int GetOwningClubProfileId(int eventId)
        {
            return _context.EventClubProfile.AsQueryable().Where(link => link.IsOwner && link.EventId == eventId).Select(link => link.ClubProfileId).First();
        }

        public IStatusGeneric<Event> UpdateEvent(UpdateEventRequest updateEventRequest, int eventId, int owningClubProfileId)
        {
            var status = new StatusGenericHandler<Event>();

            var eventToUpdate = _context.Events
                .Include(ev => ev.ClubProfilesLinks)
                .Where(ev => ev.Id == eventId)
                .FirstOrDefault();

            if(eventToUpdate == null)
            {
                return status.AddError("Event does not exist", nameof(eventId));
            }

            //check status transistion
            var currentEventStatus = eventToUpdate.EventStatus.EventStatusName;
            var futureEventStatus = updateEventRequest.EventStatus;
            if(currentEventStatus == EventStatusEnum.DELETED.ToString())
            {
                return status.AddError("Event has been deleted", nameof(eventId));
            }
            if( futureEventStatus != currentEventStatus
                && ((futureEventStatus == EventStatusEnum.CANCELLED.ToString() && currentEventStatus != EventStatusEnum.PUBLISHED.ToString())
                || (futureEventStatus == EventStatusEnum.PUBLISHED.ToString() && currentEventStatus != EventStatusEnum.DRAFT.ToString())
                || (futureEventStatus == EventStatusEnum.DELETED.ToString() && currentEventStatus != EventStatusEnum.DRAFT.ToString()))
            )
            {
                return status.AddError("Invalid event status transition", nameof(updateEventRequest.EventStatus));
            };


            //update club link
            var currentClubProfileIds = eventToUpdate.ClubProfilesLinks.ToList();
            if (currentClubProfileIds.Count != updateEventRequest.ClubProfileIds.Count 
                || currentClubProfileIds.Where(link => !updateEventRequest.ClubProfileIds.Contains(link.ClubProfileId)).Any()){

                //check club profiles
                var clubProfilesIds = _context.ClubProfiles.AsQueryable().Select(clubProfile => clubProfile.ClubProfileId).Where(id => updateEventRequest.ClubProfileIds.Contains(id)).ToList();
                if (clubProfilesIds.Count != updateEventRequest.ClubProfileIds.Count)
                {
                    return status.AddError("Club profile ids are invalid", nameof(updateEventRequest.ClubProfileIds));
                }

                //get owner
                var ownerClubLink = currentClubProfileIds.Where(link => link.IsOwner).First();

                var newClubProfileLinks = new List<EventClubProfile>();
                clubProfilesIds.ForEach(id => newClubProfileLinks.Add(new EventClubProfile { ClubProfileId = id, EventId = eventId, IsOwner = false }));
                newClubProfileLinks.Add(new EventClubProfile  {ClubProfileId = ownerClubLink.ClubProfileId, EventId = eventId, IsOwner = true });

                eventToUpdate.ClubProfilesLinks = newClubProfileLinks;
            }

            //update other info
            eventToUpdate.EventName = updateEventRequest.EventName;
            eventToUpdate.Description = updateEventRequest.Description;
            eventToUpdate.Place = updateEventRequest.Place;
            eventToUpdate.EventStartTime = updateEventRequest.EventStartTime;
            eventToUpdate.EventEndTime = updateEventRequest.EventEndTime;
            eventToUpdate.EventStatus = _context.EventStatuses.AsQueryable().Where(status => status.EventStatusName == futureEventStatus).First();

            if (eventToUpdate.EventCategory.EventCategoryName != updateEventRequest.EventCategory)
            {
                //check category
                var newEventCategory = _context.EventCategories.AsQueryable().Where(cat => cat.EventCategoryName == updateEventRequest.EventCategory).FirstOrDefault();
                if (newEventCategory == null)
                {
                    return status.AddError("Event category is invalid", nameof(updateEventRequest.EventCategory));
                }

                eventToUpdate.EventCategory = newEventCategory;
            }

            _context.SaveChanges();
            return status.SetResult(eventToUpdate);
        }
    
        public int GetTotalEvents()
        {
            return _context.Events.Count();
        }    

        public int GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling(GetTotalEvents() / (double)pageSize);
        }

    }
}
