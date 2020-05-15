using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServiceBus.SendReply.Common;
using System;

namespace ServiceBus.SendReply.Trigger
{
    /// <summary>
    /// This Azure Function is an HTTP trigger you can use to test a send/reply service
    /// bus pattern on Azure.
    /// 
    /// POST to http://localhost:7071/api/MessageTrigger when running this project and it should
    /// display the message from ServiceBusConsumer
    /// </summary>
    public static class MessageTrigger
    {
        private static string ServiceBusConnectionString { get; set; }
        private static string ReceivingQueue { get; set; }
        private static string SendingQueue { get; set; }

        static MessageTrigger()
        {
            SendingQueue = Environment.GetEnvironmentVariable("SendingQueue");
            ReceivingQueue = Environment.GetEnvironmentVariable("ReceivingQueue");
            ServiceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
        }

        [FunctionName("MessageTrigger")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("MessageTrigger processed a request");

            ISessionEnabledBusSender sender = new AzureServiceBusSessionEnabledBusSender(ServiceBusConnectionString, SendingQueue);

            var sessionId = Guid.NewGuid().ToString(); //This should be unique for each message sent that a reply is expected for

            await sender.SendMessage("Hello there", sessionId);

            ISessionEnabledBusReceiver receiver = new AzureSerivceBusSessionEnabledBusReceiver(ServiceBusConnectionString, ReceivingQueue);

            var reply = await receiver.ReceiveMessage<string>(sessionId); //This will only receive messages for the session ID above

            log.LogInformation($"Got reply: {reply}");

            return new OkObjectResult("Send/Reply flow worked! Great job!");
        }
    }
}
