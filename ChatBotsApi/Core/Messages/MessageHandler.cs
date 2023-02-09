using ChatBotsApi.Core.Data;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;

namespace ChatBotsApi.Core.Messages
{
    internal static class MessageHandler
    {
        public static class Convert
        {
            public static MessageData ToMessageData(object message, MemoryData memory, IMessageProvider provider)
            {
                var messageData = provider.ToMessageData(message, memory);
                return messageData;
            }
        }

        public static MessageData AddMessageInChat(object message, MemoryData memory, IMessageProvider provider)
        {
            MessageData messageData = Convert.ToMessageData(message, memory, provider);
            messageData.ChatData.AddMessage(messageData);
            return messageData;
        }
        
        public static MessageData AddMessageInChat(MessageData message, ChatData chatData)
        {
            chatData.AddMessage(message);
            return message;
        }
    }
}