using SmsRouter.Core;
using System;
using System.Threading.Tasks;

namespace SmsRouter.Utrn
{
    public class UtrnSvcMock : IUtrnSvc
    {
        public async Task<string> GetUtrn(string correlationId)
        {
            var rand = new Random();
            string utrn = $"UTRN_{rand.Next(147)}";

            return await Task.FromResult(utrn);
        }
    }
}
