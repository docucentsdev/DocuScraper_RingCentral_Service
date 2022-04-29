using System;
using System.Linq;
using RingCentral;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DocuScraper.Services.RingCentral.Data.Entities;
using DocuScraper.Services.RingCentral.Models;

namespace DocuScraper.Services.RingCentral.Components
{
    public class MessageServiceComponent : IMessageServiceComponent
    {
        private readonly RingCentralSettings ringCentralSettings;
        private RestClient restClient;

        public MessageServiceComponent(IConfiguration configuration)
        {
            ringCentralSettings = configuration.GetSection("RingCentralSettings").Get<RingCentralSettings>();
        }

        public async Task<bool> SendMessage(SmsRequestDTO smsRequest)
        {
            try
            {
                TokenInfo token = await CreateClientAndAuthenticate();

                if (token != null)
                {
                    string fromPhoneNumber = await GetFromPhoneNumber();

                    if (fromPhoneNumber != null)
                    {
                        var smsMessage = new CreateSMSMessage()
                        {
                            from = new MessageStoreCallerInfoRequest { phoneNumber = fromPhoneNumber },
                            to = new MessageStoreCallerInfoRequest[] { new MessageStoreCallerInfoRequest { phoneNumber = smsRequest.RecipientNumber } },                               
                            text = smsRequest.Message
                        };                        
                        var response = await restClient.Restapi().Account().Extension().Sms().Post(smsMessage);

                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region ..... Private Methods .....

        private async Task<TokenInfo> CreateClientAndAuthenticate()
        {
            restClient = new RestClient(ringCentralSettings.ClientId, ringCentralSettings.ClientSecret, ringCentralSettings.ServerUrl);

            return await restClient.Authorize(ringCentralSettings.Username, ringCentralSettings.Extension, ringCentralSettings.Password);
        }

        private async Task<string> GetFromPhoneNumber()
        {
            var phoneNumberResponse = await restClient.Restapi().Account().Extension().PhoneNumber().Get();
            
            if (phoneNumberResponse.records != null && phoneNumberResponse.records.Length > 0)
            {
                return phoneNumberResponse.records.Where(r => r.features.Contains("SmsSender")).FirstOrDefault()?.phoneNumber;                
            }
            return null;
        }

        #endregion

    }// class ends
}
