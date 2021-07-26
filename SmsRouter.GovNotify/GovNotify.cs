using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notify.Client;
using Notify.Models.Responses;
using Serilog;
using SmsRouter.Core;
using SmsRouter.Core.Domain;
using SmsRouter.GovNotify.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace SmsRouter.GovNotify
{
    public class GovNotifySvc : ISmsSvc
    {
        private readonly GovNotifyOptions options;
        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger<GovNotifySvc> logger;

        public GovNotifySvc(IOptions<GovNotifyOptions> options, IHttpClientFactory clientFactory, ILogger<GovNotifySvc> logger)
        {
            this.options = options.Value;
            this.clientFactory = clientFactory;
            this.logger = logger;
        }

        public string GetSimpleTemplateId()
        {
            return options.TemplateIdForSimple;
        }

        public SmsResponse SendSms(SmsOrder smsOrder, string correlationId)
        {
            string methodName = nameof(SendSms);
            logger.LogInformation("Started {MethodName}. {CorrelationId}", methodName, correlationId);

            var httpClient = clientFactory.CreateClient(nameof(GovNotifySvc));
            var notifyClient = new NotificationClient(new HttpClientWrapper(httpClient), options.ApiKey);

            Dictionary<string, dynamic> templateParams = smsOrder.ConvertTemplateParametersToDynamicDict();
            string templateParamsAsString = ConvertToString(templateParams);
            string logMsg = $@"Going to call service. TargetPhoneNumber=[{smsOrder.TargetPhoneNumber}].
                            TemplateId=[{smsOrder.TemplateId}]
                            TemplateParameters=[{templateParamsAsString}]";

            logger.LogInformation("Processing {MethodName}. {CorrelationId}. {logMsg}", methodName, correlationId, logMsg);

            SmsNotificationResponse govNotifyResponse = notifyClient.SendSms(
                                                  mobileNumber: smsOrder.TargetPhoneNumber,
                                                  templateId: smsOrder.TemplateId,
                                                  personalisation: templateParams
                                                  );

            logger.LogInformation("Completed {MethodName}. {CorrelationId}", methodName, correlationId);
            return new SmsResponse
            {
                Body = govNotifyResponse.content.body,
                FromNumber = govNotifyResponse.content.fromNumber
            };

        }

        private string ConvertToString(Dictionary<string, dynamic> templateParams)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in templateParams)
            {
                sb.Append(item.Key + "=" + item.Value + "|");
            }
            return sb.ToString();
        }
    }
}
