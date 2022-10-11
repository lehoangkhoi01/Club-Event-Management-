using ApplicationCore.Interfaces.Services;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ClubProfileService : IClubProfileService
    {
        public Task AddNewClub(ClubModel club)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ClubProfile> GetAllClub()
        {
            throw new NotImplementedException();
        }

        public ClubProfile GetClubProfileById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ClubProfile> SearchClub(string searchValue)
        {
            throw new NotImplementedException();
        }

        public Task UpdateClub(ClubModel club)
        {
            throw new NotImplementedException();
        }
    }
}
