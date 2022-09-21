using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class EventType
    {
        [Key]
        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; }
    }
}
