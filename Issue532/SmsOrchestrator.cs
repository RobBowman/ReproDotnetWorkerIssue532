using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using SmsRouter.Core;
using SmsRouter.Core.Domain;
using SmsRouter.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Issue532
{
    public class SmsOrchestrator
    {
        private readonly SmsOrderContext smsOrderContext;
        private readonly ISmsSvc smsSvc;
        private readonly ILogger<SmsOrchestrator> logger;

        public SmsOrchestrator(SmsOrderContext smsOrderContext, ISmsSvc smsSvc, ILogger<SmsOrchestrator> logger)
        {
            this.smsOrderContext = smsOrderContext;
            this.smsSvc = smsSvc;
            this.logger = logger;
        }

        [Function(nameof(QueueOrchestration))]
        public async Task QueueOrchestration([QueueTrigger("%SendSmsQueueName%")] SmsOrder smsOrder, FunctionContext executionContext)
        {
            string correlationId = executionContext.FunctionId;
            string methodName = nameof(QueueOrchestration);

            if (smsOrder.TemplateId.Length == 0)
                smsOrder.TemplateId = smsSvc.GetSimpleTemplateId();

            //1. Save request to database
            logger.LogInformation("Started {MethodName}. {CorrelationId}", methodName, correlationId);
            smsOrderContext.Add(smsOrder);
            await smsOrderContext.SaveChangesAsync();

            //2. Call the SMS service
            CallSmsSvc(smsOrder, executionContext.FunctionId);

            //3. Update db with response from SMS service
            smsOrderContext.Update(smsOrder);
            await smsOrderContext.SaveChangesAsync();

            logger.LogInformation("Completed {MethodName}. {CorrelationId}", methodName, correlationId);
        }

        private void CallSmsSvc(SmsOrder smsOrder, string correlationId)
        {
            string methodName = nameof(CallSmsSvc);
            logger.LogInformation("Started {MethodName}. {CorrelationId}", methodName, correlationId);
            SmsResponse smsResponse;
            try
            {
                smsResponse = smsSvc.SendSms(smsOrder, correlationId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception {MethodName}. {CorrelationId}", methodName, correlationId);
                smsOrder.State = SmsOrderStatesEnum.Faulted;
                smsOrder.ExceptionPostingToSmsService = ex.Message;
                return;
            }
            smsOrder.State = SmsOrderStatesEnum.Sent;
            smsOrder.SmsServiceResponsePayload = JsonConvert.SerializeObject(smsResponse);
            logger.LogInformation("Completed {MethodName}. {CorrelationId}", methodName, correlationId);
            return;
        }
    }
}
