using ApplicationCore;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Services.GenericPagingQuery;

namespace Infrastructure.Services.AccountService.Implementation
{
    public class AccountService
    {
        private readonly ClubEventManagementContext _db;

        public AccountService(ClubEventManagementContext db)
        {
            _db = db;
        }

        public IStatusGeneric<StudentAccount> CreateStudenAccount(CreateStudentAccountRequest request, string email)
        {
            var result = new StatusGenericHandler<StudentAccount>();
            if (_db.StudentAccounts.Any(acc => acc.UserIdentity.Email == email))
                return result.AddError("Account has been already created");
            var studentAccount = new StudentAccount()
            {
                FullName = request.FullName,
                StudentId = request.StudentId
            };
            _db.StudentAccounts.Add(studentAccount);
            _db.SaveChanges();
            return result.SetResult(studentAccount);
        }

        public IStatusGeneric<StudentAccount> UpdateStudenAccount(CreateStudentAccountRequest request, int studentAccountId, string email)
        {
            var result = new StatusGenericHandler<StudentAccount>();
            var oldStudentAccount = _db.StudentAccounts.Include(acc => acc.UserIdentity)
                .Where(acc => acc.StudentAccountId == studentAccountId).FirstOrDefault();
            if(oldStudentAccount != null)
            {
                if(oldStudentAccount.UserIdentity.Email != email)
                {
                    return result.AddError("Not matching email", nameof(email));
                }
                oldStudentAccount.StudentId = request.StudentId;
                oldStudentAccount.FullName = request.FullName;
                _db.SaveChanges();
                return result.SetResult(oldStudentAccount);
            }
            else
            {
                return result.AddError("StudentAccountId does not exist", nameof(studentAccountId));
            }
        }

        public IStatusGeneric<AdminAccount> CreateAdminAccount(CreateAdminAccountRequest request, string email)
        {
            var admin = new AdminAccount()
            {
                FullName = request.FullName
            };
            _db.AdminAccounts.Add(admin);
            _db.SaveChanges();
            return new StatusGenericHandler<AdminAccount>().SetResult(admin);
        }

        public IStatusGeneric<AdminAccount> UpdateAdminAccount(CreateAdminAccountRequest request, int adminAccountId, string email)
        {
            var result = new StatusGenericHandler<AdminAccount>();
            var oldAdminAccount = _db.AdminAccounts.Include(acc => acc.UserIdentity)
                .Where(acc => acc.AdminAccountId == adminAccountId).FirstOrDefault();
            if (oldAdminAccount != null)
            {
                if (oldAdminAccount.UserIdentity.Email != email)
                {
                    return result.AddError("Not matching email", nameof(email));
                }
                oldAdminAccount.FullName = request.FullName;
                _db.SaveChanges();
                return result.SetResult(oldAdminAccount);
            }
            else
            {
                return result.AddError("AdminAccountId does not exist", nameof(adminAccountId));
            }
        }

        public StudentAccount GetStudentAccountFromEmail(string email)
        {
            return _db.StudentAccounts.FirstOrDefault(s => s.UserIdentity.Email == email);
        }

        public IEnumerable<ClubProfileStudentAccount> GetClubProfileStudentAccountsByStudentId(int id)
        {
            return _db.ClubProfileStudentAccount.AsQueryable().Where(s => s.StudentAccountId == id && s.CanModify == true);
        }


    }
}
