using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using SmsRouter.Core;
using SmsRouter.Core.Domain;
using SmsRouter.Core.Extensions;
using SmsRouter.Core.OpenApiDef.NSwag;
using System;
//using SmsRouter.Core;
//using SmsRouter.Core.Domain;
//using SmsRouter.Core.Extensions;
//using SmsRouter.Core.OpenApiDef.NSwag;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Issue532
{
    public class HttpSms
    {
        //private readonly IUtrnSvc utrnSvc;
        private readonly ILogger<HttpSms> logger;

        //public HttpSms(IUtrnSvc utrnSvc, ILogger<HttpSms> logger)
        public HttpSms(ILogger<HttpSms> logger)
        {
            //this.utrnSvc = utrnSvc;
            this.logger = logger;
        }

        [Function(nameof(SendSimpleSms))]
        public async Task<QueueAndHttpOutputType> SendSimpleSms([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1.0/simple-sms")] HttpRequestData req,
            FunctionContext executionContext)
        {
            HttpResponseData httpResponse;
            SmsOrder coreSmsOrder = new SmsOrder();

            string correlationId = executionContext.FunctionId;
            string methodName = nameof(SendSimpleSms);

            try
            {
                logger.LogInformation("Started {MethodName}. {CorrelationId}", methodName, correlationId);

                QueueAndHttpOutputType ret = new QueueAndHttpOutputType();
                string? reqBody = req.ReadAsString();
                if (!IsValidated<SimpleSms>(reqBody, req, correlationId, out ret))
                    return ret;

                SimpleSms sendSimpleSmsRequest = JsonConvert.DeserializeObject<SimpleSms>(reqBody);

                coreSmsOrder = sendSimpleSmsRequest.ToOrder(correlationId);

                //coreSmsOrder.UtrnServiceRef = await utrnSvc.GetUtrn(correlationId);
                Random rnd = new Random();
                coreSmsOrder.UtrnServiceRef = rnd.Next(1, 7).ToString();
                ServiceRefResponse serviceRefResponse = new ServiceRefResponse { ServiceRef = coreSmsOrder.UtrnServiceRef };
                httpResponse = Helper.CreateResponse(req, HttpStatusCode.OK, JsonConvert.SerializeObject(serviceRefResponse));

                logger.LogInformation("Completed {MethodName}. {CorrelationId}", methodName, correlationId);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Exception {MethodName}. {CorrelationId}", methodName, correlationId);
                httpResponse = Helper.CreateResponse(req, HttpStatusCode.InternalServerError,
                    $"Sorry an error has occurred. Please quote the following correlation id for support:[{correlationId}]");
            }

            return new QueueAndHttpOutputType
            {
                HttpResponse = httpResponse,
                QueueMessage = JsonConvert.SerializeObject(coreSmsOrder)
            };
        }

        [Function(nameof(SendTemplateSms))]
        public async Task<QueueAndHttpOutputType> SendTemplateSms([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1.0/template-sms")] HttpRequestData req,
            FunctionContext executionContext)
        {
            HttpResponseData httpResponse;
            SmsOrder coreSmsOrder = new SmsOrder();

            string correlationId = executionContext.FunctionId;
            string methodName = nameof(SendTemplateSms);

            try
            {
                logger.LogInformation("Started {MethodName}. {CorrelationId}", methodName, correlationId);

                QueueAndHttpOutputType ret = new QueueAndHttpOutputType();
                string? reqBody = req.ReadAsString();
                if (!IsValidated<TemplateSms>(reqBody, req, correlationId, out ret))
                    return ret;

                TemplateSms sendTemplateSmsRequest = JsonConvert.DeserializeObject<TemplateSms>(reqBody);

                coreSmsOrder = sendTemplateSmsRequest.ToOrder(correlationId);

                //coreSmsOrder.UtrnServiceRef = await utrnSvc.GetUtrn(correlationId);
                Random rnd = new Random();
                coreSmsOrder.UtrnServiceRef = rnd.Next(1, 7).ToString();
                ServiceRefResponse serviceRefResponse = new ServiceRefResponse { ServiceRef = coreSmsOrder.UtrnServiceRef };
                httpResponse = Helper.CreateResponse(req, HttpStatusCode.OK, JsonConvert.SerializeObject(serviceRefResponse)); ;

                logger.LogInformation("Completed {MethodName}. {CorrelationId}", methodName, correlationId);
            }
            catch (System.Exception ex)
            {
                Log.Error(ex, "{MethodName}. {CorrelationId}", methodName, correlationId);
                httpResponse = Helper.CreateResponse(req, HttpStatusCode.InternalServerError,
                    $"Sorry an error has occurred. Please quote the following correlation id for support:[{executionContext.FunctionId}]");
            }

            return new QueueAndHttpOutputType
            {
                HttpResponse = httpResponse,
                QueueMessage = JsonConvert.SerializeObject(coreSmsOrder)
            };
        }



        private bool IsValidated<T>(string? reqBody, HttpRequestData req, string correlationId, out QueueAndHttpOutputType ret)
        {
            string methodName = nameof(IsValidated);
            logger.LogInformation("Started {MethodName}. {CorrelationId}", methodName, correlationId);
            HttpResponseData httpResponse;
            ret = new QueueAndHttpOutputType();
            if (reqBody == null)
            {
                httpResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                httpResponse.WriteString("The request is missing a message body");
                ret.HttpResponse = httpResponse;
                return false;
            }
            logger.LogInformation("Processing {MethodName}. {CorrelationId}.Received request body:{reqBody}", methodName, correlationId, reqBody);

            if (!reqBody.TryParseJson<T>(out T? smsRequest, out string parseError))
            {
                httpResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                httpResponse.WriteString(parseError);
                ret.HttpResponse = httpResponse;
                return false;
            }

            return true;
        }
    }
}
