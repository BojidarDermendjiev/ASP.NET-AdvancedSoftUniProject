namespace ServerAspNetCoreAPIMakePC.Application.Mappings
{
    using AutoMapper;

    using Utilities;
    using DTOs.User;
    using DTOs.Order;
    using DTOs.Review;
    using DTOs.Category;
    using DTOs.ShoppingCart;
    using Domain.Entities;


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

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            CreateMap<CreateOrderItemDto, OrderItem>();
            CreateMap<UpdateOrderDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
