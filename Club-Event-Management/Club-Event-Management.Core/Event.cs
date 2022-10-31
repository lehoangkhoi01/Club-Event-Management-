using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsInternal { get; set; }
        public EventStatus EventStatus { get; set; }
        public EventCategory EventCategory { get; set; }
        public List<EventActivity> EventActivities { get; set; }
        public List<EventPost> EventPosts { get; set; }
        public List<EventClubProfile> ClubProfilesLinks { get; set; }
    }
}
