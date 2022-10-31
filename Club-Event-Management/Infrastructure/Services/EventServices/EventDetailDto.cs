using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventServices
{
    public class EventDetailDto
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public string EventStartTime { get; set; }
        public string EventEndTime { get; set; }
        public bool IsInternal { get; set; }
        public string EventStatus { get; set; }
        public string EventCategory { get; set; }
        public List<EventDetail_EventActivityDto> EventActivities { get; set; }
        public List<EventDetail_EventPostDto> EventPosts { get; set; }
        public List<EventListDto_ClubInfo> ClubInfos { get; set; }
        public PagingStatus PagingStatus { get; set; }

    }

    public class EventDetail_EventActivityDto
    {
        public int Id { get; set; }
        public string EventActivityName { get; set; }
        public string Content { get; set; }
        public string Location { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class EventDetail_EventPostDto
    {
        public int EventPostId { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public string CreatedDate { get; set; }
    }

    public class PagingStatus
    {
        public int totalPost;
        public int totalActivity;
        public int currentPostSize;
        public int currentActivitySize;
    }
}

