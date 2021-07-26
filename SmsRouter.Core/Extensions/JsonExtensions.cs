using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmsRouter.Core.Extensions
{
    public static class JsonExtensions
    {
        public static bool TryParseJson<T>(this string obj, out T result, out string parseError)
        {
            try
            {
                // Validate missing fields of object
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.MissingMemberHandling = MissingMemberHandling.Error;

                result = JsonConvert.DeserializeObject<T>(obj, settings);
                parseError = "";
                return true;
            }
            catch (Exception ex)
            {
                result = default(T);
                parseError = ex.Message;
                return false;
            }
        }
    }
}
