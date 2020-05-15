using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus.SendReply.Common
{
    public class AzureSerivceBusSessionEnabledBusReceiver : ISessionEnabledBusReceiver
    {
        private SessionClient SessionClient { get; set; }
        private const int Retries = 5;

        public AzureSerivceBusSessionEnabledBusReceiver(string serviceBusConnectionString, string queue)
        {
            SessionClient = new SessionClient(serviceBusConnectionString, queue);
        }

        public async Task<T> ReceiveMessage<T>(string sessionId)
        {
            Message message = null;

            var retryCount = 0;

            do
            {
                try
                {
                    var messageSession = await SessionClient.AcceptMessageSessionAsync(sessionId);
                    message = await messageSession.ReceiveAsync();
                }
                catch (Exception)
                {
                    retryCount++;
                    Thread.Sleep(1000);

                    if (retryCount > Retries) throw new InvalidOperationException($"Receiver did not receive a response for session Id {sessionId}");
                }
            }
            while (message == null);

            return JsonConvert.DeserializeObject<T>(Helpers.DeserializeBody(message.Body));
        }
    }
}
