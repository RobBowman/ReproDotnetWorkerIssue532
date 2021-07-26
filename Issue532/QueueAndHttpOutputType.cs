using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Issue532
{
    public class QueueAndHttpOutputType
    {
        [QueueOutput("%SendSmsQueueName%")]
        public string QueueMessage { get; set; } = "";

        public HttpResponseData HttpResponse { get; set; }
    }
}
