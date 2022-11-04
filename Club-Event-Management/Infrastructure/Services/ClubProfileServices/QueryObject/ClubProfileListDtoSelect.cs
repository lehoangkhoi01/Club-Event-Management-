using ApplicationCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ClubProfileServices.QueryObject
{
    public static class ClubProfileListDtoSelect
    {
        public static IQueryable<ClubProfileListDto> MapClubProfileToDto(this IQueryable<ClubProfile> clubProfiles, int eventSize, int studentAccountSize)
        {
            return clubProfiles.Select(profile => new ClubProfileListDto
            {
                ClubProfileId = profile.ClubProfileId,
                Avatar = profile.Avatar,
                FoundationDate = profile.FoundationDate.ToLongDateString(),
                ClubName = profile.ClubName,
                SocialAddress = profile.SocialAddress,
                Summary = profile.Summary,
                TotalMember = profile.StudentAccountsLink.Count(),
                Events = profile.EventsLink.OrderByDescending(link => link.Event.EventStartTime)
                .Where(link => !link.Event.IsInternal)
                .Select(link => new ClubProfileListDto_EventInfo
                {
                    EventId = link.EventId,
                    EventName = link.Event.EventName,
                    EndTime = link.Event.EventStartTime.ToLongDateString(),
                    StartTime = link.Event.EventEndTime.ToLongTimeString()
                }).Take(eventSize).ToList(),
                StudentAccounts = profile.StudentAccountsLink.OrderByDescending(link => link.CanModify).Select(link => new ClubProfileListDto_MemberInfo
                {
                    MemberId = link.StudentAccountId,
                    IsEditor = link.CanModify,
                    MemberName = link.StudentAccount.FullName
                }).ToList(),
            });
        }
    }
}
