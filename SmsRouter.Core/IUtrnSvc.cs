using System.Threading.Tasks;

namespace SmsRouter.Core
{
    public interface IUtrnSvc
    {
        Task<string> GetUtrn(string correlationId);
    }
}
