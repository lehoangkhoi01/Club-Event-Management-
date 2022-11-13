using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ClubProfileServices
{
    public class CreateClubProfileRequest
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
        public Dictionary<string , bool> MemeberInforMap { get; set; } = new Dictionary<string, bool>();
    }
}
