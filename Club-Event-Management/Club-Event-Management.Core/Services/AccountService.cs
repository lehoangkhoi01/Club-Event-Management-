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
    }
}
