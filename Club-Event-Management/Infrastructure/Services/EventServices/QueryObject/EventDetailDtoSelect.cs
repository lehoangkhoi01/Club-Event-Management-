using ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventServices.QueryObject
{
    public static class EventDetailDtoSelect
    {
        public static IQueryable<EventDetailDto> MapEventToEventDetailDto(this IQueryable<Event> events, int activitySize, int postSize)
        {
            return events.Select(ev => new EventDetailDto
            {
                Id = ev.Id,
                EventName = ev.EventName,
                Description = ev.Description,
                Image = ev.Image,
                Place = ev.Place,
                EventStartTime = ev.EventStartTime.ToString(),
                EventEndTime = ev.EventEndTime.ToString(),
                IsInternal = ev.IsInternal,
                EventStatus = ev.EventStatus.EventStatusName,
                EventCategory = ev.EventCategory.EventCategoryName,
                EventActivities = ev.EventActivities.OrderByDescending(activity => activity.CreatedDate).Select(activity => new EventDetail_EventActivityDto
                {
                    Id = activity.EventActivityId,
                    EventActivityName = activity.EventActivityName,
                    Content = activity.Content,
                    Location = activity.Location,
                    StartTime = activity.StartTime.ToLongDateString(),
                    EndTime = activity.EndTime.ToLongDateString()
                }).Take(activitySize).ToList(),
                EventPosts = ev.EventPosts.OrderByDescending(activity => activity.CreatedDate).Select(post => new EventDetail_EventPostDto
                {
                    EventPostId = post.EventPostId,
                    Content = post.Content,
                    CreatedDate = post.CreatedDate.ToLongDateString(),
                    Picture = post.Picture
                }).Take(postSize).ToList(),
                ClubInfos = ev.ClubProfilesLinks.Select(link => new EventListDto_ClubInfo
                {
                    ClubName = link.ClubProfile.ClubName,
                    ClubProfileId = link.ClubProfileId,
                    IsOwner = link.IsOwner,
                    Avatar = link.ClubProfile.Avatar
                }).ToList(),
                PagingStatus = new PagingStatus
                {
                    totalActivity = ev.EventActivities.Count,
                    totalPost = ev.EventPosts.Count,
                    currentActivitySize = activitySize,
                    currentPostSize = postSize
                }

            });
        }
    }
}
