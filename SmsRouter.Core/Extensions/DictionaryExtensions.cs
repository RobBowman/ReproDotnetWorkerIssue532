using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmsRouter.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, dynamic> ToDynamicDict(this Dictionary<string, string> paramList)
        {
            Dictionary<string, dynamic> ret = new Dictionary<string, dynamic>();

            foreach (var item in paramList)
            {
                ret.Add(item.Key, item.Value);
            }

            return ret;
        }

        public static string ToString(this Dictionary<string, dynamic> dict)
        {
            return "{" + string.Join(",", dict.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }
    }
}
