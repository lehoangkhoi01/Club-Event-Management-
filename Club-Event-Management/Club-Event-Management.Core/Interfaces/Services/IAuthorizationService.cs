using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IAuthorizationService
    {
        public Task<UserIdentity> Login(UserLogin userLogin);

        public Task<string> GenerateToken(UserIdentity user); 
    }
}
