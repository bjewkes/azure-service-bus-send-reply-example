using System.Threading.Tasks;

namespace ServiceBus.SendReply.Common
{
    public interface ISessionEnabledBusSender
    {
        Task SendMessage<T>(T message, string sessionId);
    }
}
