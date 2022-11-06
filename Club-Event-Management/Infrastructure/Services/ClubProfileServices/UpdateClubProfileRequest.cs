using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ClubProfileServices
{
    public class UpdateClubProfileRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string ClubName { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Summary { get; set; }
        [Required]
        public string Avatar { get; set; }
        public DateTime FoundationDate { get; set; }
        [Required]
        public string SocialAddress { get; set; }
        public Dictionary<string, UpdateMemeberRequest> UpdateMemeberRequestsMap { get; set; } = new Dictionary<string, UpdateMemeberRequest>();
    }

    public class UpdateMemeberRequest
    {
        public bool CanModify { get; set; }

        public bool Remove { get; set; } = false;
    }
}
