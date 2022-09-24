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

namespace ApplicationCore.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _repo;
        private readonly IConfiguration _config;
        public AuthorizationService(IAuthorizationRepository repo, IConfiguration configuration)
        {
            _repo = repo;
            _config = configuration;
        }

        public string GenerateToken(UserIdentity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserIdentity> Login(string email)
        {
            return await _repo.GetIdentityUserByEmail(email);
        }
    }
}
