using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core
{
    public class SmsResponse
    {
        public string Body { get; set; } = "";
        public string FromNumber { get; set; } = "";
    }
}
