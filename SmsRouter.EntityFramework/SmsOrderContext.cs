using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmsRouter.Core.Domain;
using SmsRouter.EntityFramework.Options;

namespace SmsRouter.EntityFramework
{
    public class SmsOrderContext : DbContext
    {
        private readonly DbOptions dbOptions;
        private readonly ILogger<SmsOrderContext> logger;

        public SmsOrderContext(DbContextOptions<SmsOrderContext> options, IOptions<DbOptions> dbOptions, ILogger<SmsOrderContext> logger) : base(options)
        {
            this.dbOptions = dbOptions.Value;
            this.logger = logger;
            RelationalDatabaseCreator databaseCreator =
                        (RelationalDatabaseCreator)this.Database.GetService<IDatabaseCreator>();
            databaseCreator.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<SmsOrder>? SmsOrder { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmsOrder>()
                .HasIndex(b => b.UtrnServiceRef)
                .IsUnique();
        }
    }
}
