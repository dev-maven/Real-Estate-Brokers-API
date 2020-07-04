using System.Linq;
using AutoMapper;
using Urban.ng.Dtos;
using Urban.ng.Models;
using Urban.ng.ViewModels;

namespace Urban.ng.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            
                CreateMap<Photo, PhotoReturnDto>();
                CreateMap<PhotoCreateDto, Photo>();
                CreateMap<Video, VideoReturnDto>();
                CreateMap<VideoCreateDto, Video>();
                CreateMap<Plan, PlanReturnDto>();
                CreateMap<PlanCreateDto, Plan>();
                CreateMap<PropertyCreateDto, Property>();
                CreateMap<PropertyEditDto, Property>();
                CreateMap<Property, PropertyReturnDto>();
                CreateMap<Property, PropertyForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault
                    (p => p.IsMain).PhotoUrl);
                });
                CreateMap<RegisterViewModel, User>();
                CreateMap<User, UserForListDto>();
                CreateMap<User, UserLoginViewModel>().ReverseMap();
                CreateMap<ProfilePhoto, ProfilePhotoReturnDto>();
                CreateMap<ProfilePhotoCreateDto, ProfilePhoto>();

        }
    }
}