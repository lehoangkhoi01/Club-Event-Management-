using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventServices
{
    public class EventListDto
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public string Image { get; set; }
        public string EventStartTime { get; set; }
        public string EventEndTime { get; set; }
        public string CreatedDate { get; set; }
        public bool IsInternal { get; set; }
        public string EventStatus { get; set; }
        public string Category { get; set; }
        public EventListDto_ClubInfo[] ClubInfos { get; set; }

    }

    public class EventListDto_ClubInfo
    {
        public String ClubName { get; set; }
        public int ClubProfileId { get; set; }
        public bool IsOwner { get; set; }
        public String Avatar { get; set; }
    }
}
