using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class AdminAccount
    {
        [Key]
        public int AdminAccountId { get; set; }
        public string FullName { get; set; }

        public virtual UserIdentity UserIdentity { get; set; }
    }
}
