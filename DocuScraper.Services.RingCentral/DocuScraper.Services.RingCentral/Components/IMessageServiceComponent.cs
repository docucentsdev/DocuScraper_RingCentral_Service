using System.Threading.Tasks;
using DocuScraper.Services.RingCentral.Models;

namespace DocuScraper.Services.RingCentral.Components
{
    public interface IMessageServiceComponent
    {
        Task<bool> SendMessage(SmsRequestDTO smsRequest);
    }
}
