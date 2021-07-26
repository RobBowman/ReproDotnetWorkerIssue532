using SmsRouter.Core.Domain;
using SmsRouter.Core.OpenApiDef.NSwag;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core.Extensions
{
    public static class SimpleSmsConversion
    {
        public static SmsOrder ToOrder(this SimpleSms nswagObject, string correlationId)
        {
            return new SmsOrder
            {
                Tags = Helper.ConvertTags(nswagObject.RequestConfig.Tags),
                BusinessArea = nswagObject.RequestConfig.BusinessArea,
                CallingSystem = nswagObject.RequestConfig.CallingSystem,
                CallingSystemRef = nswagObject.RequestConfig.CallingSystemRef,
                TargetPhoneNumber = nswagObject.RequestConfig.TargetPhoneNumber,
                CorrelationId = correlationId,
                TemplateParameters = new List<TemplateParameter> { new TemplateParameter { Key = "Message", Value = nswagObject.MessageBody } }
            };
        }
    }
}
