using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmsRouter.Core.Domain
{
    public class TemplateParameter
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
        public SmsOrder SmsOrder { get; set; } = new SmsOrder();
        public int SmsOrderId { get; set; }

    }
}
