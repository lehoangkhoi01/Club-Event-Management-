using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ClubProfileServices
{
    public class ClubProfileListDto
    {
        public int ClubProfileId { get; set; }
        public string ClubName { get; set; }
        public string Summary { get; set; }
        public string Avatar { get; set; }
        public int TotalMember { get; set; }
        public string FoundationDate { get; set; }
        public string SocialAddress { get; set; }
        public List<ClubProfileListDto_MemberInfo> StudentAccounts { get; set; }
        public List<ClubProfileListDto_EventInfo> Events{ get; set; }
    }

    public class ClubProfileListDto_EventInfo
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class ClubProfileListDto_MemberInfo
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public bool IsEditor { get; set; }
    }
}
