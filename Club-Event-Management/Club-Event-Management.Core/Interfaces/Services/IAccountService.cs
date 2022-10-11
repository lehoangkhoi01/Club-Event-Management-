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
        public StudentAccount GetStudentAccountByEmail(string email);
        public Task<StudentAccount> GetStudentAccountById(int id);
        public Task<StudentAccount> GetStudentAccountBySchoolId(string id);
    }
}
