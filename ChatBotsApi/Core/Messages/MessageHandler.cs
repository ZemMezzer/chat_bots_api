using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;

namespace ChatBotsApi.Core.Messages
{
    internal static class MessageHandler
    {
        public static class Convert
        {
            public static MessageData ToMessageData(object message, IMessageProvider provider)
            {
                var messageData = provider.ToMessageData(message);
                return messageData;
            }
        }

        public static MessageData AddMessageInChat(object message, IMessageProvider provider)
        {
            MessageData messageData = Convert.ToMessageData(message, provider);
            messageData.ChatData.AddMessage(messageData);
            return messageData;
        }
    }
}