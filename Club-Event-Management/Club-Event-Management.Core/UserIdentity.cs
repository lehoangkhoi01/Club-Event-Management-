using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class UserIdentity
    {
        [Key]
        public string Email { get; set; }
        public bool IsLocked { get; set; }

        [ForeignKey("Id")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
