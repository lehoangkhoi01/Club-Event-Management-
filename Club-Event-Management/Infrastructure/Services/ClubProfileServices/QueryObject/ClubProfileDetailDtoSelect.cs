using ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ClubProfileServices.QueryObject
{
    public static class ClubProfileDetailDtoSelect
    {
        public static IQueryable<ClubProfileDetailDto> MapClubProfileToDetailDto(this IQueryable<ClubProfile> clubProfiles, int profileId)
        {
            return clubProfiles.Where(club => club.ClubProfileId == profileId).Select(club => new ClubProfileDetailDto
            {
                ClubProfileId = club.ClubProfileId,
                Avatar = club.Avatar,
                ClubName = club.ClubName,
                FoundationDate = club.FoundationDate.ToString(),
                CreatedDate = club.CreatedDate.ToString(),
                SocialAddress = club.SocialAddress,
                Summary = club.Summary,
                TotalMember = club.StudentAccountsLink.Count(),
                MemeberInfos = club.StudentAccountsLink.Select(link => new ClubProfileDetailDto_MemeberInfo
                {
                    CanModify = link.CanModify,
                    FullName = link.StudentAccount.FullName,
                    StudentProfileId = link.StudentAccountId
                }).ToList(),
                UpdatedDate = club.UpdatedDate.ToString()
            });
        }

    }
}
