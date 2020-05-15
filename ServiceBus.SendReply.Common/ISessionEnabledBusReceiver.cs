using System.Threading.Tasks;

namespace ServiceBus.SendReply.Common
{
    public interface ISessionEnabledBusReceiver
    {
        Task<T> ReceiveMessage<T>(string sessionId);
    }
}
