using ApplicationCore;
using Infrastructure.Services.EventPostServices.QueryObject;
using Infrastructure.Services.EventServices;
using Infrastructure.Services.FirebaseServices.NotificationService;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventPostServices.Implementation
{
    public class EventPostService
    {
        private readonly ClubEventManagementContext _context;
        private readonly NotificationService _notificationService;

        public EventPostService(ClubEventManagementContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public IQueryable<EventPost> GetEventPosts(bool isAdmin, List<int> clubIds)
        {
            if (isAdmin)
                return _context.EventPosts;
            var eventIds = _context.EventClubProfile.AsQueryable().Where(link => clubIds.Contains(link.ClubProfileId)).Select(link => link.EventId).Distinct().ToList();
            return _context.EventPosts.BusinessFilter(eventIds);
        }

        public EventPost CreateEventPost(CreateEventPostRequest createEventPostRequest)
        {
            var owningEvent = _context.Events.Include(ev => ev.EventStatus).AsQueryable().Where(ev => ev.Id == createEventPostRequest.EventId).FirstOrDefault();
            if (owningEvent == null || (owningEvent.EventStatus.EventStatusName != EventStatusEnum.PUBLISHED.ToString() && owningEvent.EventStatus.EventStatusName != EventStatusEnum.PAST.ToString()))
                return null;
            var newPost = new EventPost
            {
                Content = createEventPostRequest.Content,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Picture = createEventPostRequest.Picture,
                EventId = createEventPostRequest.EventId,
                ClubProfileId = createEventPostRequest.ClubProfileId
            };
            _context.Add(newPost);
            _context.SaveChanges();
            //generate notification
            var notification = new NotificationDto
            {
                ActionType = ActionType.CREATE.ToString(),
                EventId = owningEvent.Id,
                Eventname = owningEvent.EventName,
                SubjectId = newPost.EventPostId,
                SubjectType = SubjectType.POST.ToString()
            };
            _notificationService.PublishNotification(notification);
            return newPost;

        }

        public IStatusGeneric<EventPost> UpdateEventPost(UpdateEventPostRequest updateEventPostRequest, int postId)
        {
            var result = new StatusGenericHandler<EventPost>();

            var oldPost = _context.EventPosts.Find(postId);
            if (oldPost == null)
            {
                return result.AddError("Could not find post", nameof(postId));
            }
            if (oldPost.EventId != updateEventPostRequest.EventId)
            {
                return result.AddError("Could not change event id", nameof(updateEventPostRequest.EventId));
            }

            oldPost.Content = updateEventPostRequest.Content;
            oldPost.Picture = updateEventPostRequest.Picture;
            oldPost.UpdatedDate = DateTime.Now;

           _context.SaveChanges();
            return result.SetResult(oldPost);

        }

        public IStatusGeneric<int> DeleteEventPost(int postId, bool isAdmin, List<int> owniningClubIds)
        {
            var result = new StatusGenericHandler<int>();

            var oldPost = _context.EventPosts.Find(postId);
            if (oldPost == null)
            {
                return result.AddError("Could not find post", nameof(postId));
            }

            if(!CanModifyPost(isAdmin, owniningClubIds, oldPost.EventId))
            {
                return result.SetResult(-1);
            }
            _context.EventPosts.Remove(oldPost);
            _context.SaveChanges();
            return result.SetResult(postId);
        }

        public bool CanModifyPost(bool isAdmin, List<int> owniningClubIds, int eventId)
        {
            if (isAdmin)
                return true;
            //Any author memeber of event's clubs
            return _context.EventClubProfile.Any(link => link.EventId == eventId && owniningClubIds.Contains(link.ClubProfileId));
        }
    }
}
