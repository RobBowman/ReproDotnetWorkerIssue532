using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core.OpenApiDef.Operations
{
    public class RequestConfig
    {
        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonProperty("businessArea")]
        public string BusinessArea { get; set; } = "";

        [JsonProperty("callingSystem")]
        public string CallingSystem { get; set; } = "";

        [JsonProperty("callingSystemRef")]
        public string CallingSystemRef { get; set; } = "";


        [JsonProperty("targetPhoneNumber")]
        public string TargetPhoneNumber { get; set; } = "";


       public List<Domain.Tag> ConvertTags()
        {
            List<Domain.Tag> ret = new List<Domain.Tag>();

            foreach (var item in Tags)
            {
                ret.Add(new Domain.Tag 
                { 
                    Description = item
                });
            }

            return ret;
        }
    }
}
