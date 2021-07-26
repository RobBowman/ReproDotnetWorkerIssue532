using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core.Exceptions
{
    public class HttpResponseEx : Exception
    {

        public HttpResponseEx(string responseCode, string reason)
            : base($"{responseCode} - {reason}")
        {
        }

        public HttpResponseEx(string responseCode, string reason, string responseMsg)
            : base($"{responseCode} - {reason} - {responseMsg}")
        {
        }
    }
}
