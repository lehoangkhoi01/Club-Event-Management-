using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class EventActivity
    {
        public int EventActivityId { get; set; }
        public string EventActivityName { get; set; }
        public string Content { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Event Event { get; set; }
        public int EventId { get; set; }
    }
}
