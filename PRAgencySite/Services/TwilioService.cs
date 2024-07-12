using Microsoft.Extensions.Options;
using PRAgencySite.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PRAgencySite.Services
{
    public class TwilioService
    {
        private readonly TwilioSettings _twilioSettings;

        public TwilioService(IOptions<TwilioSettings> twilioSettings)
        {
            _twilioSettings = twilioSettings.Value;
            TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
        }

        public void SendWhatsAppMessage(string to, string message)
        {
            var messageOptions = new CreateMessageOptions(new PhoneNumber($"whatsapp:{to}"))
            {
                From = new PhoneNumber(_twilioSettings.FromNumber),
                Body = message
            };

            var messageResource = MessageResource.Create(messageOptions);
            // Handle the message resource as needed
        }
    }
}
