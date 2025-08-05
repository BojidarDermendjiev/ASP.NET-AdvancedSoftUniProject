namespace ServerAspNetCoreAPIMakePC.API
{
    using Microsoft.EntityFrameworkCore;

    using Application.Mappings;
    using ModelBinders;
    using Infrastructure.Data;
    using Infrastructure.Data.DbSeed;
    using Infrastructure.DependencyInjection;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
            });

            builder.Services.AddDbContext<MakePCDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddApplicationServices();
            builder.Services.AddRepositories();

            builder.Services.AddMemoryCache();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddMakePcMiddlewares();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MakePCDbContext>();
                DbSeed.Seed(db);
            }

            app.UseHttpsRedirection();

            app.UseMakePcMiddlewares();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
