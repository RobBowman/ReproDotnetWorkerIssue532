using Newtonsoft.Json;
using SmsRouter.Core;
using SmsRouter.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core.OpenApiDef.Operations
{
    public class SendSimpleSmsRequest
    {
        [JsonProperty("requestConfig")]
        public RequestConfig RequestConfig { get; set; } = new RequestConfig();

        [JsonProperty("messageBody")]
        public string MessageBody { get; set; } = "";

        public SmsOrder ConvertToSmsOrder(string templateId, string functionId)
        {
            SmsOrder ret = new SmsOrder();

            ret.Tags = this.RequestConfig.ConvertTags();
            ret.BusinessArea = RequestConfig.BusinessArea;
            ret.CallingSystem = RequestConfig.CallingSystem;
            ret.CallingSystemRef = RequestConfig.CallingSystemRef;
            ret.TargetPhoneNumber = RequestConfig.TargetPhoneNumber;
            ret.TemplateId = templateId;
            ret.TemplateParameters = new List<TemplateParameter> { new TemplateParameter { Key = "Message", Value = this.MessageBody } };
            ret.CorrelationId = functionId;
            return ret;
        }
    }

    
}
