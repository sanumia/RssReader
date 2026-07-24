using AutoMapper;
using RssReader.DTOs.FeedItem;
using RssReader.Models;

namespace RssReader.Mapping;

public class FeedItemMappingProfile : Profile
{
    public FeedItemMappingProfile()
    {
        CreateMap<FeedItem, FeedItemDto>()
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.IsFavorite, opt => opt.MapFrom(src => false));
    }
}
