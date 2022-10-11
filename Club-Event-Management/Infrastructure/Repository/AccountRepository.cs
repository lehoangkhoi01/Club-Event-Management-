using ApplicationCore;
using ApplicationCore.Interfaces.Repository;
using Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
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

        public StudentAccount GetStudentAccountByEmail(string email)
        {
            var dbContext = new ClubEventManagementContext();
            var student = dbContext.StudentAccounts
                .Include(s => s.UserIdentity)
                .Include(s => s.ClubProfiles)
                .FirstOrDefault(s => s.UserIdentity.Email == email);
            return student;
        }

        public async Task<StudentAccount> GetStudentAccountById(int id)
        {
            var dbContext = new ClubEventManagementContext();
            var student = await dbContext.StudentAccounts
                .Include(s => s.UserIdentity)
                .Include(s => s.ClubProfiles)
                .FirstOrDefaultAsync(s => s.StudentAccountId == id);
            return student;
        }

        public async Task<StudentAccount> GetStudentAccountBySchoolId(string id)
        {
            var dbContext = new ClubEventManagementContext();
            var student = await dbContext.StudentAccounts
                .Include(s => s.UserIdentity)
                .Include(s => s.ClubProfiles)
                .FirstOrDefaultAsync(s => s.StudentId == id);
            return student;
        }
    }
}
