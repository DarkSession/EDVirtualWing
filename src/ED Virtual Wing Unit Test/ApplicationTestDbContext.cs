using Duende.IdentityServer.EntityFramework.Options;
using ED_Virtual_Wing.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace ED_Virtual_Wing_Unit_Test
{
    public class ApplicationTestDbContext : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:"; // Filename=:memory:
        private readonly SqliteConnection _connection;

        public ApplicationDbContext DbContext { get; }

        public ApplicationTestDbContext()
        {
            OperationalStoreOptions appSettings = new() {
            
            };
            // IOptions<OperationalStoreOptions> options = Options.Create(appSettings);

            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(_connection)
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine)
                    .Options;
            DbContext = new ApplicationDbContext(options, Options.Create(appSettings));
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
