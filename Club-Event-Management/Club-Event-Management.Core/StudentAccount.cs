using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public class StudentAccount
    {
        [Key]
        public int StudentAccountId { get; set; }
        public string FullName { get; set; }
        public string StudentId { get; set; }

        public List<ClubProfileStudentAccount> ClubLinks { get; set; }
        public UserIdentity UserIdentity { get; set; }

    }
}
