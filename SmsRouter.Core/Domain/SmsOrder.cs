using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmsRouter.Core.Domain
{
    public class SmsOrder
    {
        public SmsOrder()
        {
            Timestamp = DateTime.Now;
            Tags = new List<Tag>();
        }

        public int Id { get; set; }
        [NotMapped]
        public string CorrelationId { get; set; } = ""; 
        public DateTime Timestamp { get; set; }
        [MaxLength(50)]
        public string TemplateId { get; set; } = "";
        public List<TemplateParameter> TemplateParameters { get; set; } = new List<TemplateParameter>();
        public SmsOrderStatesEnum State { get; set; } = SmsOrderStatesEnum.Received;
        [MaxLength(20)]
        public string UtrnServiceRef { get; set; } = "";
        public string SmsServiceResponsePayload { get; set; } = "";
        public string? ExceptionPostingToSmsService { get; set; }
        [MaxLength(30)]
        public string BusinessArea { get; set; } = "";
        [MaxLength(30)]
        public string CallingSystem { get; set; } = "";
        [MaxLength(30)]
        public string CallingSystemRef { get; set; } = "";
        [MaxLength(30)]
        public string TargetPhoneNumber { get; set; } = "";
        public List<Tag> Tags { get; set; }

        public Dictionary<string, dynamic> ConvertTemplateParametersToDynamicDict()
        {
            Dictionary<string, dynamic> ret = new Dictionary<string, dynamic>();

            foreach (var item in TemplateParameters)
            {
                ret.Add(item.Key, item.Value);
            }

            return ret;
        }

    }
}
