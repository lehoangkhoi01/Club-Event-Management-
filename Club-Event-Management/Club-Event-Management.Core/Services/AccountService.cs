﻿using ApplicationCore.Interfaces.Repository;
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
            await _repository.AddNewAccount(user);
        }

        public AdminAccount GetAdminAccount(UserIdentity user)
        {
            return _repository.GetAdminAccount(user.Email);
        }

        public async Task<StudentAccount> GetStudentAccount(UserIdentity user)
        {
            return await _repository.GetStudentAccount(user.Email);
        }

        public StudentAccount GetStudentAccountByEmail(string email)
        {
            return _repository.GetStudentAccountByEmail(email);
        }

        public async Task<StudentAccount> GetStudentAccountById(int id)
        {
            return await _repository.GetStudentAccountById(id);
        }

        public async Task<StudentAccount> GetStudentAccountBySchoolId(string id)
        {
            return await _repository.GetStudentAccountBySchoolId(id);
        }
    }
}
