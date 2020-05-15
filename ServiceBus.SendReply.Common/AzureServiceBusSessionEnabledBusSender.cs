using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ServiceBus.SendReply.Common
{
    public class AzureServiceBusSessionEnabledBusSender : ISessionEnabledBusSender
    {
        private QueueClient QueueClient { get; }

        public AzureServiceBusSessionEnabledBusSender(string serviceBusConnectionString, string queue)
        {
            QueueClient = new QueueClient(serviceBusConnectionString, queue);
        }

        public async Task SendMessage<T>(T message, string sessionId)
        {
            await QueueClient.SendAsync(new Message
            {
                ContentType = "application/json",
                SessionId = sessionId,
                Body = Helpers.SerializeBody(JsonConvert.SerializeObject(message))
            });
        }
    }
}
