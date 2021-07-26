using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsRouter.GovNotify.Options
{
    public class GovNotifyOptions
    {
        public string ApiKey { get; set; } = "";
        public string TemplateIdForSimple { get; set; } = "";
        public bool IsProxyRequired { get; set; }
        public string ProxyUrl { get; set; } = "";
        public string ProxyUsername { get; set; } = "";
        public string ProxyPassword { get; set; } = "";
    }
}
