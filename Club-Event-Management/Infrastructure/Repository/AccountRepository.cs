using ApplicationCore;
using ApplicationCore.Interfaces.Repository;
using Infrastructure.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public Task AddNewAccount(UserIdentity user) => AccountDAO.Instance.AddNew(user);

        public Task AddNewStudentAccount(UserIdentity user, StudentAccount student) 
            => AccountDAO.Instance.AddNewStudentAccount(user, student);

        public AdminAccount GetAdminAccount(string email)
            => AccountDAO.Instance.GetAdminAccount(email);

        public Task<StudentAccount> GetStudentAccount(string email)
            => AccountDAO.Instance.GetStudentAccount(email);
    }
}
