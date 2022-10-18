using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class ClubProfile
    {
        [Key]
        public int ClubProfileId { get; set; }
        public string ClubName { get; set; }
        public string Summary { get; set; }
        public string Avatar { get; set; }
        public int TotalMember { get; set; }
        public DateTime FoundationDate { get; set; }
        public string SocialAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<ClubProfileStudentAccount> StudentAccountsLink { get; set; }
        public List<EventClubProfile> EventsLink { get; set; }
    }
}
