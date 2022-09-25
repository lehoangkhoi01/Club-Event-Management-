using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IAccountService
    {
        public Task Add(UserIdentity user);
        public Task<StudentAccount> GetStudentAccount(UserIdentity user);
        public AdminAccount GetAdminAccount(UserIdentity user);
    }
}
