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

        CreateMap<FeedItem, FeedItemDto>()
            .ForMember(dest => dest.IsRead,
                opt => opt.MapFrom((src, _, _, ctx) =>
                    src.UserFeedItems.Any(uf => uf.UserId == (int)ctx.Items["UserId"] && uf.IsRead)))
            .ForMember(dest => dest.IsFavorite,
                opt => opt.MapFrom((src, _, _, ctx) =>
                    src.UserFeedItems.Any(uf => uf.UserId == (int)ctx.Items["UserId"] && uf.IsFavorite)));

        CreateMap<CreateFeedItemDto, FeedItem>();

        CreateMap<UpdateFeedItemDto, FeedItem>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
