using Newtonsoft.Json;
using SmsRouter.Core;
using SmsRouter.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core.OpenApiDef.Operations
{
    public class SendTemplateSmsRequest
    {
        [JsonProperty("requestConfig")]
        public RequestConfig RequestConfig { get; set; } = new RequestConfig();

        [JsonProperty("templateId")]
        public string TemplateId { get; set; } = "";

        [JsonProperty("templateParameters")]
        public Dictionary<string, string> TemplateParameters { get; set; } = new Dictionary<string, string>();

        public SmsOrder ConvertToSmsOrder(string functionId)
        {
            SmsOrder ret = new SmsOrder();

            ret.Tags = this.RequestConfig.ConvertTags();
            ret.BusinessArea = RequestConfig.BusinessArea;
            ret.CallingSystem = RequestConfig.CallingSystem;
            ret.CallingSystemRef = RequestConfig.CallingSystemRef;
            ret.TargetPhoneNumber = RequestConfig.TargetPhoneNumber;
            ret.TemplateId = TemplateId;
            ret.TemplateParameters = ConvertTemplateParameters(TemplateParameters);
            ret.CorrelationId = functionId;

            return ret;
        }

        private List<TemplateParameter> ConvertTemplateParameters(Dictionary<string, string> apiTemplateParameters)
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
