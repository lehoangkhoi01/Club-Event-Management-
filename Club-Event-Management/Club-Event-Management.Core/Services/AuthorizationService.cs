using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _repo;
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _config;
        public AuthorizationService(IAuthorizationRepository repo, 
                                    IAccountRepository accountRepository,
                                    IConfiguration configuration)
        {
            _repo = repo;
            _config = configuration;
            _accountRepository = accountRepository;
        }

        public async Task<string> GenerateToken(UserIdentity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            string fullname = "";
            if(user.RoleId == 1)
            {
                AdminAccount adminAccount = _accountRepository.GetAdminAccount(user.Email);
                if (adminAccount != null)
                    fullname = adminAccount.FullName;
            }
            else if(user.RoleId == 2)
            {
                StudentAccount studentAccount = await _accountRepository.GetStudentAccount(user.Email);
                if (studentAccount != null)
                    fullname = studentAccount.FullName;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim(ClaimTypes.Name, fullname),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserIdentity> Login(UserLogin userLogin)
        {
            var user = await _repo.GetIdentityUserByEmail(userLogin.Email);

            if(user == null)
            {
                // User's email is not existed -> create new one
                UserIdentity newUserIdentity = new UserIdentity
                {
                    Email = userLogin.Email,
                    IsLocked = false,
                    RoleId = 2 // Role 2 for Student
                };
                StudentAccount newStudentAccount = new StudentAccount
                {
                    FullName = userLogin.FullName,
                    UserIdentity = newUserIdentity,
                };
                await _accountRepository.AddNewStudentAccount(newUserIdentity, newStudentAccount);
                user = await _repo.GetIdentityUserByEmail(newUserIdentity.Email);
            }


            if (user.IsLocked)
            {
                return null;
            }
            return user;


        }
    }
}
