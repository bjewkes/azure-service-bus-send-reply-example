using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ServiceBus.SendReply.Common;

namespace ServiceBus.SendReply.Trigger
{
    public static class ServiceBusConsumer
    {
        /// <summary>
        /// This function is triggered by service bus messages and sends them back to a separate queue, following the send/reply pattern.
        /// 
        /// The return value must be a Message entity so that the session Id can be returned indicating that these messages are related
        /// in Azure Service Bus
        /// </summary>
        [FunctionName("ServiceBusConsumer")]
        [return: ServiceBus("%ReceivingQueue%", Connection = "ServiceBusConnectionString")]
        public static Message Run([ServiceBusTrigger("%SendingQueue%", Connection = "ServiceBusConnectionString", IsSessionsEnabled = true)] Message message, ILogger log)
        {
            log.LogInformation($"ServiceBusConsumer function processed message: {Helpers.DeserializeBody(message.Body)}");

            return new Message
            {
                CorrelationId = message.MessageId,
                ContentType = "application/json",
                SessionId = message.SessionId,
                Body = Helpers.SerializeBody("General Kenobi")
            };
        }
    }
}
