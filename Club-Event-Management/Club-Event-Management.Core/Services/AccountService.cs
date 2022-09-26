using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task Add(UserIdentity user)
        {
            // Hash password
            // Check existing user
            // Do other business rules, use case

            await _repository.AddNewAccount(user);
        }

        public AdminAccount GetAdminAccount(UserIdentity user)
        {
            return _repository.GetAdminAccount(user.Email);
        }

        public async Task<IEnumerable<AdminAccount>> GetAdminList()
        {
            return await _repository.GetAdminList();
        }

        public async Task<StudentAccount> GetStudentAccount(UserIdentity user)
        {
            return await _repository.GetStudentAccount(user.Email);
        }

        public async Task<IEnumerable<StudentAccount>> GetStudentList()
        {
            return await _repository.GetStudentList();
        }
    }
}
