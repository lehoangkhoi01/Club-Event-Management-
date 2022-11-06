using ApplicationCore;
using Infrastructure.Services.EventActivityServices;
using Infrastructure.Services.EventPostServices.QueryObject;
using Infrastructure.Services.EventServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventPostServices.Implementation
{
    public class EventActivityService
    {
        private readonly ClubEventManagementContext _context;

        public EventActivityService(ClubEventManagementContext context)
        {
            _context = context;
        }

        public IQueryable<EventActivity> GetEventActivities(bool isAdmin, List<int> clubIds)
        {
            if (isAdmin)
                return _context.EventActivities;
            var eventIds = _context.EventClubProfile.AsQueryable().Where(link => clubIds.Contains(link.ClubProfileId)).Select(link => link.EventId).Distinct().ToList();
            return _context.EventActivities.BusinessFilter(eventIds);
        }

        public EventActivity CreateEventActivity(CreateEventActivityRequest createEventActivityRequest)
        {
            var owningEvent = _context.Events.Find(createEventActivityRequest.EventId);
            if (owningEvent == null || owningEvent.EventStatus.EventStatusName != EventStatusEnum.PUBLISHED.ToString() || owningEvent.EventStatus.EventStatusName != EventStatusEnum.PAST.ToString())
                return null;
            var newActivity = new EventActivity
            {
                Content = createEventActivityRequest.Content,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                EndTime = createEventActivityRequest.EndTime,
                StartTime = createEventActivityRequest.StartTime,
                EventId = createEventActivityRequest.EventId,
                Location = createEventActivityRequest.Location,
                EventActivityName = createEventActivityRequest.EventActivityName
            };
            _context.Add(newActivity);
            _context.SaveChanges();
            return newActivity;

        }

        public IStatusGeneric<EventActivity> UpdateEventActivity(UpdateEventActivityRequest updateEventActivityRequest, int activityId)
        {
            var result = new StatusGenericHandler<EventActivity>();

            var oldActivity= _context.EventActivities.Find(activityId);
            if (oldActivity == null)
            {
                return result.AddError("Could not find activity", nameof(activityId));
            }
            if (oldActivity.EventId != updateEventActivityRequest.EventId)
            {
                return result.AddError("Could not change event id", nameof(updateEventActivityRequest.EventId));
            }

            oldActivity.Content = updateEventActivityRequest.Content;
            oldActivity.EndTime = updateEventActivityRequest.EndTime;
            oldActivity.StartTime = updateEventActivityRequest.StartTime;
            oldActivity.EventActivityName = updateEventActivityRequest.EventActivityName;
            oldActivity.Location = updateEventActivityRequest.Location;
            oldActivity.UpdatedDate = DateTime.Now;

           _context.SaveChanges();
            return result.SetResult(oldActivity);

        }

        public IStatusGeneric<int> DeleteEventActivity(int activityId, bool isAdmin, List<int> owningClubIds)
        {
            var result = new StatusGenericHandler<int>();

            var oldActivity = _context.EventActivities.Find(activityId);
            if (oldActivity == null)
            {
                return result.AddError("Could not find post", nameof(activityId));
            }

            if(!CanModifyActivity(isAdmin, owningClubIds, oldActivity.EventId))
            {
                return result.SetResult(-1);
            }
            _context.EventActivities.Remove(oldActivity);
            _context.SaveChanges();
            return result.SetResult(activityId);
        }

        public bool CanModifyActivity(bool isAdmin, List<int> owningClubIds, int eventId)
        {
            if (isAdmin)
                return true;
            return _context.EventClubProfile.Any(link => link.EventId == eventId && owningClubIds.Contains(link.ClubProfileId));
        }
    }
}
