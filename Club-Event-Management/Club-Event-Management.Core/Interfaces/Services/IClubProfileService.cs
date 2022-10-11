using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IClubProfileService
    {
        public ClubProfile GetClubProfileById(int id);
        public IEnumerable<ClubProfile> GetAllClub();
        public IEnumerable<ClubProfile> SearchClub(string searchValue);
        public Task AddNewClub(ClubModel club);
        public Task UpdateClub(ClubModel club);
    }
}
