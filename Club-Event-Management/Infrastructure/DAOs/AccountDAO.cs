using ApplicationCore;
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



    }
}
