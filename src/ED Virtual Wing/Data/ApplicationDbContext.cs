using Duende.IdentityServer.EntityFramework.Options;
using ED_Virtual_Wing.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ED_Virtual_Wing.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Environment.GetEnvironmentVariable("EDVW_MARIADB_CONNECTIONSTRING") ?? "server=localhost;user=root;password=1234;database=ef",
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
    }
}