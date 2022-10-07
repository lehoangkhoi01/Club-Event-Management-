using System;
using System.ComponentModel.DataAnnotations;

namespace ClubEventManagementAPI.ViewModels
{
    public class EventPostViewModel
    {
        public int EventPostId { get; set; }
        [Required]
        public string Content { get; set; }
        
        public string Picture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int StudentAccountId { get; set; }

        public int EventId { get; set; }
    }
}
