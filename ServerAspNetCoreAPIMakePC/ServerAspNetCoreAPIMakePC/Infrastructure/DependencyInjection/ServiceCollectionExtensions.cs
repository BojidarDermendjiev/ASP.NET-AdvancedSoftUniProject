namespace ServerAspNetCoreAPIMakePC.Infrastructure.DependencyInjection
{
    using Services;
    using Repositories;
    using Domain.Interfaces;
    using Application.Services;
    using Application.Interfaces;
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IPlatformFeedbackService, PlatformFeedbackService>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IPlatformFeedbackRepository, PlatformFeedbackRepository>();
            return services;
        }
    }
}