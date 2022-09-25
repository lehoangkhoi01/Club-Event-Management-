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
    public class AuthorizationRepository : IAuthorizationRepository
    {
        public Task<UserIdentity> GetIdentityUserByEmail(string email) => IdentityUserDAO.Instance.GetUserByEmail(email);
    }
}
