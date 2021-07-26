using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Issue532
{
    public class Helper
    {
        public static HttpResponseData CreateResponse(HttpRequestData req, HttpStatusCode statusCode, string body)
        {
            HttpResponseData ret = req.CreateResponse(statusCode);
            ret.WriteString(body);
            return ret;
        }
    }
}
