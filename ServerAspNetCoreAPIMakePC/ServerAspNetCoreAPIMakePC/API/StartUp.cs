namespace ServerAspNetCoreAPIMakePC.API
{
    using Microsoft.EntityFrameworkCore;

    using Infrastructure.Data;
    using Application.Mappings;
    using Application.Interfaces;
    using Infrastructure.Services;
    using Infrastructure.DependencyInjection;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<MakePCDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddApplicationServices();
            builder.Services.AddRepositories();

            builder.Services.AddMemoryCache();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
