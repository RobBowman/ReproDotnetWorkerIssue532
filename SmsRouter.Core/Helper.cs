using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core
{
    public class Helper
    {
        public static List<Domain.Tag> ConvertTags(List<string> tags)
        {
            List<Domain.Tag> ret = new List<Domain.Tag>();

            foreach (var item in tags)
            {
                ret.Add(new Domain.Tag
                {
                    Description = item
                });
            }

            return ret;
        }
    }
}
