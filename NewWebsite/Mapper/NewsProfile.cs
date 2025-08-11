using AutoMapper;
using NewWebsite.Extension;
using NewWebsite.Models;

namespace NewWebsite.Mapper;
public class NewsProfile : Profile
{
    public NewsProfile()
    {
        CreateMap<News, SimpleNews>()
            .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => src.NewsStatus == NewsStatus.Published))
            .ForMember(dest => dest.NewsCategory, opt => opt.MapFrom(src => src.NewsCategory.Name))
            .ForMember(dest => dest.CreatedDateString, opt => opt.MapFrom(src => src.CreatedDate.ToLocalTime().ToString("MMMM dd, HH:mm:ss tt zz") + " " + src.CreatedDate.ToHumanAgoString()));
            
        CreateMap<NewsRequest, News>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Title.Slugify()))
            .ForMember(dest => dest.TotalLikes, opt => opt.MapFrom(src=> 0));
    }
}