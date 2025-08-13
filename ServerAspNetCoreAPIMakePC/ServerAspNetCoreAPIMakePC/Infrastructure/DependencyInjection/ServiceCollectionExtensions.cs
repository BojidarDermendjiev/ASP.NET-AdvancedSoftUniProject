namespace ServerAspNetCoreAPIMakePC.Infrastructure.DependencyInjection
{
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Authentication.JwtBearer;

    using Services;
    using External;
    using Repositories;
    using API.Middleware;
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
            services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
            services.AddScoped<IPlatformFeedbackService, PlatformFeedbackService>();
            services.AddHttpClient<IPaymentGatewayService, PaymentGatewayService>();
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
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Secret"] ?? string.Empty))
                    };
                });

            services.AddAuthorization();
            return services;
        }
        public static IApplicationBuilder UseMakePcMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseMiddleware<RateLimitingMiddleware>();
            app.UseMiddleware<CustomHeaderMiddleware>();
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<MaintenanceModeMiddleware>();
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RoleAuthorizationMiddleware>();
            app.UseMiddleware<AdminAuthorizationMiddleware>();
            app.UseMiddleware<CustomerAuthorizationMiddleware>();
            return app;
        }

    }
}