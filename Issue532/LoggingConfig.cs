using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue532
{
    public class LoggingConfig
    {


        public void Configure(string sqlSvrConString)
        {
            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn
                        {ColumnName = "MethodName", DataType = SqlDbType.NVarChar, DataLength = 100, NonClusteredIndex = true},

                    new SqlColumn
                        {ColumnName = "CorrelationId", DataType = SqlDbType.NVarChar, DataLength = 50, NonClusteredIndex = true},

                }
            };

            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Override("Microsoft.Azure", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo
                            .ApplicationInsights(TelemetryConfiguration.Active, TelemetryConverter.Traces)
                        .WriteTo.Console()
                        .WriteTo.MSSqlServer(
                            logEventFormatter: new RenderedCompactJsonFormatter(),
                            restrictedToMinimumLevel: LogEventLevel.Debug,
                            connectionString: sqlSvrConString,
                            sinkOptions: new MSSqlServerSinkOptions
                            {
                                TableName = "LogEvents",
                                AutoCreateSqlTable = true,
                            },
                            columnOptions: columnOptions)
                        .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(Console.Error);
        }

    }
}
