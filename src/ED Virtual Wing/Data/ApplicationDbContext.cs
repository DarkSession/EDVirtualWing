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
        public DbSet<FDevApiAuthCode> FDevApiAuthCodes { get; set; }
        public DbSet<StarSystem> StarSystems { get; set; }
        public DbSet<StarSystemBody> StarSystemBodies { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<TranslationsPending> TranslationsPendings { get; set; }
        public DbSet<Wing> Wings { get; set; }
        public DbSet<WingMember> WingMembers { get; set; }
        public DbSet<WingInvite> WingInvites { get; set; }

        private IConfiguration Configuration { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions, IConfiguration configuration)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options, operationalStoreOptions)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Configuration["EDVW:ConnectionString"],
                new MariaDbServerVersion(new Version(10, 6, 0)),
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasOne(a => a.Commander)
                .WithOne(c => c.User);

            builder.Entity<Commander>()
                .HasOne(c => c.User)
                .WithOne(u => u.Commander);

            builder.Entity<Commander>()
                .HasOne(c => c.Location)
                .WithOne(l => l.Commander);

            builder.Entity<CommanderLocation>()
                .HasOne(l => l.Commander)
                .WithOne(c => c.Location);

            builder.Entity<Commander>()
                .HasOne(c => c.Target)
                .WithOne(t => t.Commander);

            builder.Entity<CommanderLocation>()
                .HasOne(l => l.Commander)
                .WithOne(c => c.Location);
        }
    }
}