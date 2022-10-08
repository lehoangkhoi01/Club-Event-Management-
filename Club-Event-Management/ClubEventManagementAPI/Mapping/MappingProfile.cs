using ApplicationCore;
using AutoMapper;
using ClubEventManagementAPI.ViewModels;

namespace ClubEventManagementAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventPost, EventPostViewModel>().ReverseMap();
        }
    }
}
