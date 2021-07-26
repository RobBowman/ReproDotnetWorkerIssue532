using SmsRouter.Core.Domain;
using SmsRouter.Core.OpenApiDef.NSwag;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core.Extensions
{
    public static class ResponseConversions
    {
        public static StatusResponseStatus ToNSwagResponseState(this SmsOrderStatesEnum inputState)
        {
            switch (inputState)
            {
                case SmsOrderStatesEnum.Received:
                    return StatusResponseStatus.Received;
                case SmsOrderStatesEnum.Sent:
                    return StatusResponseStatus.Sent;
                case SmsOrderStatesEnum.Faulted:
                    return StatusResponseStatus.Faulted;
                default:
                    throw new ApplicationException($"Unable to match case in: {nameof(ToNSwagResponseState)}");
            }
        }

    }
}
