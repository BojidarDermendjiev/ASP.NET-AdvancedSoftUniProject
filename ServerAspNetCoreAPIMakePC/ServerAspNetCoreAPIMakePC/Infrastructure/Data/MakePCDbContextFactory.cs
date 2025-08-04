namespace ServerAspNetCoreAPIMakePC.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    public class MakePCDbContextFactory : IDesignTimeDbContextFactory<MakePCDbContext>
    {
        public MakePCDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MakePCDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new MakePCDbContext(optionsBuilder.Options);
        }
    }
}
