using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventServices
{
    public class UpdateEventRequest : IValidatableObject
    {
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string EventName { get; set; }
        [Required]
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string Description { get; set; }
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string Place { get; set; }
        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        [Required]
        public string EventStatus { get; set; }
        public String EventCategory { get; set; }
        public int OwningClubProfileId { get; set; }
        public List<int> ClubProfileIds { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EventStartTime.CompareTo(EventEndTime) > 0 || EventStartTime.CompareTo(DateTime.Now) < 0)
            {
                yield return new ValidationResult(
                    $"Event start time/ end time is invalid",
                    new[] { nameof(EventStartTime), nameof(EventEndTime) });
            }
            if (EventStatus != EventStatusEnum.CANCELLED.ToString() && EventStatus != EventStatusEnum.PUBLISHED.ToString()
                && EventStatus != EventStatusEnum.DELETED.ToString())
            {
                yield return new ValidationResult(
                    $"EventStatus can only be CANCELLED or PUBLISHED or DELETED",
                    new[] { nameof(EventStatus) });
            }
        }
    }
}
