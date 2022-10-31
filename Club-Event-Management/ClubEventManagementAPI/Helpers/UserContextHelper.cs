using ApplicationCore;
using Infrastructure.Services.AccountService.Implementation;
using Infrastructure.Services.ClubProfileServices.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace ClubEventManagementAPI.Helpers
{
    public class UserContextService
    {
        private readonly ClubProfileService _clubService;
        private readonly AccountService _accountService;

        public UserContextService(ClubProfileService clubService, AccountService accountService)
        {
            _clubService = clubService;
            _accountService = accountService;
        }

        public AccountInfo GetIdentity(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new AccountInfo
                {
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };

            }
            return null;
        }

        public UserContext GetUserContext(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;
                //return if admin
                if (userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value == "Admin")
                    return new UserContext { IsAdmin = true };

                //collect other info
                var studenAccId = _accountService.GetStudentAccountFromEmail(userClaims.First(o => o.Type == ClaimTypes.Email).Value).StudentAccountId;
                var clubIds = _clubService.GetClubLinkByStudentAccountId(studenAccId);

                return new UserContext
                {
                    IsAdmin = false,
                    ClubIds = clubIds.Select(tuple => tuple.Item1).ToList(),
                    OwningClubIds = clubIds.Where(tuple => tuple.Item2).Select(tuple => tuple.Item1).ToList(),
                    StudentAccountId = studenAccId
                };
            }
            return null;
        }
    }

    public class UserContext
    {
        public bool IsAdmin { get; set; }
        public int? StudentAccountId { get; set; } = null;
        public List<int> ClubIds { get; set; } = new List<int>();
        public List<int> OwningClubIds { get; set; } = new List<int>();
    }

}

    public class AccountInfo
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public int? StudentAccountId { get; set; } = null;
        public int? AdminAccountId { get; set; } = null;

}