using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class EventClubProfile
    {
        public int EventId { get; set; }
        public int ClubProfileId { get; set; }
        public Event Event { get; set; }
        public ClubProfile ClubProfile { get; set; }
        public bool IsOwner { get; set; }

    }
}
