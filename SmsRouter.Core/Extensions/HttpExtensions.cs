using Microsoft.Extensions.Logging;
using SmsRouter.Core.Exceptions;
using System.Linq;
using System.Net.Http;

namespace SmsRouter.Core.Extensions
{
    public static class HttpExtensions
    {
        public static string CheckResponseWithPayload(this HttpResponseMessage response, ILogger logger)
        {
            response.Headers.ToList().ForEach(x => logger.LogDebug($"Response Header:[{x.Key}] : [{x.Value.ToList().First()}]"));
            string responsePayload = response.Content.ReadAsStringAsync().Result;
            string statusCode = response.StatusCode.ToString();
            string reasonPhrase = response.ReasonPhrase;
            logger.LogInformation($"StatusCode:[{statusCode}]. ReasonPhrase[{reasonPhrase}]");
            logger.LogDebug($"ResponseMsg:[{responsePayload}].");

            if (response.IsSuccessStatusCode)
            {
                return responsePayload;
            }

            HttpResponseEx ex = new HttpResponseEx(statusCode,
                                                reasonPhrase, responsePayload);
            logger.LogError("http response error", ex);

            throw ex;
        }
    }
}
