using ApplicationCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAOs
{
    public class IdentityUserDAO
    {
        private static IdentityUserDAO instance;
        private static readonly object instanceLock = new object();

        private IdentityUserDAO()
        {
        }

        public static IdentityUserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new IdentityUserDAO();
                    }
                }
                return instance;
            }
        }

        public async Task<UserIdentity> GetUserByEmail(string email)
        {
            var dbContext = new ClubEventManagementContext();
            return await dbContext.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
            
        }
    }
}
