using SmsRouter.Core.Domain;
using SmsRouter.Core.OpenApiDef.NSwag;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core.Extensions
{
    public static class TemplateSmsConversion
    {
        public static SmsOrder ToOrder(this TemplateSms nswagObject, string correlationId)
        {
            return new SmsOrder
            {
                Tags = Helper.ConvertTags(nswagObject.RequestConfig.Tags),
                BusinessArea = nswagObject.RequestConfig.BusinessArea,
                CallingSystem = nswagObject.RequestConfig.CallingSystem,
                CallingSystemRef = nswagObject.RequestConfig.CallingSystemRef,
                TargetPhoneNumber = nswagObject.RequestConfig.TargetPhoneNumber,
                TemplateId = nswagObject.TemplateId,
                CorrelationId = correlationId,
                TemplateParameters = ConvertTemplateParameters(nswagObject.TemplateParameters)
            };
            
        }

        private static List<TemplateParameter> ConvertTemplateParameters(IDictionary<string, string> apiTemplateParameters)
        {
            List<TemplateParameter> ret = new List<TemplateParameter>();

            foreach (var item in apiTemplateParameters)
            {
                ret.Add(new TemplateParameter() { Key = item.Key, Value = item.Value });
            }
            return ret;
        }
    }
}
