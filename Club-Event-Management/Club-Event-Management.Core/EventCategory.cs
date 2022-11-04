using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class EventCategory
    {
        [Key]
        public int EventCategoryId { get; set; }
        public string EventCategoryName { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
