using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class PostReaction
    {
        public int PostReactionId { get; set; }
        public bool IsLike { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("EventPostId")]
        public int EventPostId { get; set; }
        public virtual EventPost EventPost { get; set; }
    }
}
