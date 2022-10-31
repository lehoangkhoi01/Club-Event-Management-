using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ClubProfileServices
{
    public class ClubProfileDetailDto
    {
        public int ClubProfileId { get; set; }
        public string ClubName { get; set; }
        public string Summary { get; set; }
        public string Avatar { get; set; }

        public int TotalMember { get; set; }
        public string FoundationDate { get; set; }
        public string SocialAddress { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public List<ClubProfileDetailDto_MemeberInfo> MemeberInfos { get; set; }
    }

    public class ClubProfileDetailDto_MemeberInfo
    {
        public int StudentProfileId { get; set; }
        public string FullName { get; set; }
        public bool CanModify { get; set; }
    }
}
