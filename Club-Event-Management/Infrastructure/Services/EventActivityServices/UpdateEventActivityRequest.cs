using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EventActivityServices
{
    public class UpdateEventActivityRequest : IValidatableObject
    {
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string EventActivityName { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EventId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime.CompareTo(EndTime) > 0 || StartTime.CompareTo(DateTime.Now) < 0)
            {
                yield return new ValidationResult(
                    $"Event start time/ end time is invalid",
                    new[] { nameof(StartTime), nameof(EndTime) });
            }
        }
    }

}
