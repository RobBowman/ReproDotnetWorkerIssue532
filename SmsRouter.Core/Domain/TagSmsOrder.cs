using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core.Domain
{
    public class TagSmsOrder 
    {
        public int TagId { get; set; }
        public int SmsOrderId { get; set; }
        public Tag Tag { get; set; } = new Tag();
        public SmsOrder SmsOrder { get; set; } = new SmsOrder();

    }
}
