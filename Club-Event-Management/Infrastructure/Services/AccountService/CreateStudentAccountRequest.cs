using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.AccountService
{
    public class CreateStudentAccountRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string FullName { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string StudentId { get; set; }
    }
}
