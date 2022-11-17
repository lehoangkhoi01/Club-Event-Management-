using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventPostServices
{
    public class CreateEventPostRequest
    {
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public string Picture { get; set; }
        public int EventId { get; set; }
        public int ClubProfileId { get; set; }
    }
}
