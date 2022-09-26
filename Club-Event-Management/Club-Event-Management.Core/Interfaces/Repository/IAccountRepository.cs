using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repository
{
    public interface IAccountRepository
    {
        public Task AddNewAccount(UserIdentity user);
        public Task AddNewStudentAccount(UserIdentity user, StudentAccount student);

        public Task<StudentAccount> GetStudentAccount(string email);
        public AdminAccount GetAdminAccount(string email);

        public Task<IEnumerable<StudentAccount>> GetStudentList();
        public Task<IEnumerable<AdminAccount>> GetAdminList();
          
    }
}
