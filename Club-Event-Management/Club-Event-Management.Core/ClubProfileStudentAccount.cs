using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class ClubProfileStudentAccount
    {
        public int ClubProfileId { get; set; }
        public int StudentAccountId { get; set; }
        public bool CanModify { get; set; }
        public ClubProfile ClubProfile { get; set; }
        public StudentAccount StudentAccount { get; set; }

    }
}
