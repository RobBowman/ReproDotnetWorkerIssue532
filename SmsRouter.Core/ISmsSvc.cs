using SmsRouter.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core
{
    public interface ISmsSvc
    {
        public SmsResponse SendSms(SmsOrder smsOrder, string correlationId);
        string GetSimpleTemplateId();
    }
}
