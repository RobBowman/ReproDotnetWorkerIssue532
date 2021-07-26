using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmsRouter.Core;
using SmsRouter.EntityFramework.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.EntityFramework
{
    public class SmsOrderContextFactory : IDesignTimeDbContextFactory<SmsOrderContext>
    {
        private readonly IOptions<DbOptions> dbOptions;
        private readonly ILogger<SmsOrderContext> logger;

        public SmsOrderContextFactory(IOptions<DbOptions> dbOptions, ILogger<SmsOrderContext> logger)
        {
            this.dbOptions = dbOptions;
            this.logger = logger;
        }

        public SmsOrderContext CreateDbContext(string[] args)
        {
            string designTimeConString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SmsRouter";

            var optionsBuilder = new DbContextOptionsBuilder<SmsOrderContext>();
            optionsBuilder.UseSqlServer(designTimeConString);

            return new SmsOrderContext(optionsBuilder.Options, dbOptions, logger);
        }
    }
}
