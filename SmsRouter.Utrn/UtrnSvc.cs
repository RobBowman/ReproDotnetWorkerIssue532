using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmsRouter.Core;
using SmsRouter.Core.Extensions;
using SmsRouter.Utrn.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmsRouter.Utrn
{
    public class UtrnSvc : IUtrnSvc
    {
        private readonly UtrnOptions utrnOptions;
        private readonly IHttpClientFactory factory;
        private readonly ILogger<UtrnSvc> logger;

        public UtrnSvc(IOptions<UtrnOptions> utrnOptions, IHttpClientFactory factory, ILogger<UtrnSvc> logger)
        {
            this.utrnOptions = utrnOptions.Value;
            this.factory = factory;
            this.logger = logger;
        }

        public async Task<string> GetUtrn(string correlationId)
        {
            string methodName = nameof(GetUtrn);
            logger.LogInformation("Started {MethodName}. {CorrelationId}", methodName, correlationId);
            HttpResponseMessage httpResponse;

            string responseBody = "";
            try
            {
                string addressSuffix = utrnOptions.AddressSuffix;

                using (var httpClient = factory.CreateClient(nameof(UtrnSvc)))
                {
                    httpResponse = await httpClient.GetAsync(addressSuffix);
                    responseBody = httpResponse.CheckResponseWithPayload(logger);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception at {MethodName}. {CorrelationId}", methodName, correlationId);
                throw;
            }

            return responseBody;

        }
    }
}
