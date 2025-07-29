namespace ServerAspNetCoreAPIMakePC.Application.Mappings
{

    
    using Utilities;
    using DTOs.User;
    using AutoMapper;
    using DTOs.Review;
    using Domain.Entities;
    using DTOs.ShoppingCart;


    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"))
                .AfterMap((src, dest) =>
                {
                    byte[] salt;
                    dest.PasswordHash = PasswordHasher.HashPassword(src.Password, out salt);
                    dest.PasswordSalt = salt;
                });

            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            CreateMap<BasketItem, BasketItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<AddBasketItemDto, ShoppingCart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => new List<BasketItem>
                {
                    new BasketItem
                    {
                        ProductId = src.ProductId,
                        Quantity = src.Quantity
                    }
                }));
           
            CreateMap<AddBasketItemDto, BasketItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Basket, opt => opt.Ignore()) 
                .ForMember(dest => dest.BasketId, opt => opt.Ignore()) 
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<Review, ReviewDto>();
            CreateMap<CreateReviewDto, Review>();
            CreateMap<UpdateReviewDto, Review>();
            CreateMap<Review, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore());
        }
    }
}
