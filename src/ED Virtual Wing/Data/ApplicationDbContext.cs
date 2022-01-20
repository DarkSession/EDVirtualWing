using Duende.IdentityServer.EntityFramework.Options;
using ED_Virtual_Wing.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ED_Virtual_Wing.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Commander> Commanders { get; set; }
        public DbSet<StarSystem> StarSystems { get; set; }
        public DbSet<StarSystemBody> StarSystemBodies { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<TranslationsPending> TranslationsPendings { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options, operationalStoreOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Environment.GetEnvironmentVariable("EDVW_MARIADB_CONNECTIONSTRING") ?? "server=localhost;user=root;password=1234;database=edvw",
                new MariaDbServerVersion(new Version(10, 3, 25)),
                options =>
                {
                    options.EnableRetryOnFailure();
                    options.CommandTimeout(60 * 10 * 1000);
                })
#if DEBUG
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine)
#endif
                ;
        }
        /*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        */
    }
}