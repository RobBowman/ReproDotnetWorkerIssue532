using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core.OpenApiDef.Operations
{
    public class UtrnResponse
    {
        [JsonProperty("serviceRef")]
        public string ServiceRef { get; set; } = "";
    }
}
