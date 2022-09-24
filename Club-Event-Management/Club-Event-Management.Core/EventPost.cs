﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class EventPost
    {
        [Key]
        public int EventPostId { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [ForeignKey("StudentAccountId")]
        public int StudentAccountId { get; set; }
        public virtual StudentAccount StudentAccount { get; set; }

        [ForeignKey("EventId")]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}