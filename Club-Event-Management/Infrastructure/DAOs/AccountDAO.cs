﻿using ApplicationCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAOs
{
    public class AccountDAO
    {
        private readonly ClubEventManagementContext _context;
        private static AccountDAO instance;
        private static readonly object instanceLock = new object();

        private AccountDAO()
        {
        }

        public static AccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AccountDAO();
                    }
                }
                return instance;
            }
        }
        //-------------------------------

        public async Task AddNew(UserIdentity user)
        {
            var dbContext = new ClubEventManagementContext();
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddNewStudentAccount(UserIdentity user, StudentAccount studentAccount)
        {
            var dbContext = new ClubEventManagementContext();
            await dbContext.Users.AddAsync(user);
            await dbContext.StudentAccounts.AddAsync(studentAccount);
            await dbContext.SaveChangesAsync();
        }

        public async Task<StudentAccount> GetStudentAccount(string email)
        {
            var dbContext = new ClubEventManagementContext();
            var student = await dbContext.StudentAccounts.Include(u => u.ClubProfiles)
                                                .FirstOrDefaultAsync(u => u.UserIdentity.Email == email);
            return student;
        }

        public AdminAccount GetAdminAccount(string email)
        {
            var dbContext = new ClubEventManagementContext();
            var admin = dbContext.AdminAccounts.FirstOrDefault(u => u.UserIdentity.Email == email);
            return admin;
        }

        public async Task<IEnumerable<StudentAccount>> GetStudentAccountList()
        {
            var dbContext = new ClubEventManagementContext();
            var studentList = await dbContext.StudentAccounts
                            .Include(s => s.UserIdentity)
                            .Include(s => s.ClubProfiles)
                            .ToListAsync();
            return studentList;
        }

        public async Task<IEnumerable<AdminAccount>> GetAdminList()
        {
            var dbContext = new ClubEventManagementContext();
            var adminAccount = await dbContext.AdminAccounts
                                .Include(a => a.UserIdentity)
                                .ToListAsync();
            return adminAccount;
        }
    }
}
