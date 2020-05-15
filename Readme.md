# Azure Service Bus Send/Reply Example

This repo is a very simple demonstration of how to implement Send/Reply with Azure Service Bus using Azure Functions Service Bus Triggers and Output Binding.

The code in this repo is not intended for production use, but to demonstrate how to correctly use Session Ids to correlate messages for Send/Reply

## Running Locally:

1. Clone this repo locally
1. In Azure portal, create two session enabled queues in a service bus namespace [per this documentation](https://docs.microsoft.com/en-us/azure/service-bus-messaging/message-sessions)
1. Copy `local.dist.settings.json` to `local.settings.json`
1. In `local.settings.json` copy the `Primary Connection String` from the service bus namespace created in Azure to `ServiceBusConnectionString`
1. Put the entity path (aka name) of one queue as the value of the `ReceivingQueue` setting
1. Put the entity path (aka name) of the other queue as the value of the `SendingQueue` setting
1. Start the `ServiceBus.SendReply.Trigger` project
1. POST anything to http://localhost:7071/api/MessageTrigger and look at console output to see if its working.
