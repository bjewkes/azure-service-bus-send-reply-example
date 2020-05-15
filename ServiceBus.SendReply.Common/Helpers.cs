using System.Text;

namespace ServiceBus.SendReply.Common
{
    public static class Helpers
    {
        public static byte[] SerializeBody(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

        public static string DeserializeBody(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
