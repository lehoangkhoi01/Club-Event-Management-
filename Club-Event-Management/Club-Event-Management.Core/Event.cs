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
        public int TotalFollow { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public List<ClubProfile> ClubProfiles { get; set; }

        public int CreatedClubId { get; set; }
        public string CreatedClub { get; set; }


        [ForeignKey("EventTypeId")]
        public int EventTypeId { get; set; }
        public virtual EventType EventType { get; set; }

        [ForeignKey("EventCategoryId")]
        public int EventCategoryId { get; set; }
        public virtual EventCategory EventCategory { get; set; }
    }
}
