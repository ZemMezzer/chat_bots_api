using System;
using ChatBotsApi.Core.Data;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;
using Telegram.Bot.Types;

namespace ChatBotsApi.Bots.TelegramBot.Messages
{
    internal class TelegramMessageProvider : IMessageProvider
    {
        public MessageData ToMessageData(object source, MemoryData data)
        {
            if (source is not Message message)
                throw new InvalidCastException();

            return new MessageData(ToString(message), message.MessageId, data.GetUser(message.From.Id), data.GetChats()[message.Chat.Id]);
        }

        public ChatData GetChatData(object source, MemoryData data)
        {
            if (source is not Message message)
                throw new InvalidCastException();
            
            string messageChatName = message.Chat.Username ?? message.Chat.Title;
            if (!data.GetChats().ContainsKey(message.Chat.Id))
                return new ChatData(message.Chat.Id, messageChatName);

            return data.GetChats()[message.Chat.Id];
        }

        public UserData GetUserData(object source, MemoryData data)
        {
            if (source is not Message message)
                throw new InvalidCastException();

            long userId = message.From.Id;

            if (data.GetUsers().ContainsKey(userId))
                return data.GetUser(userId);
            
            return new UserData(message.From.Id, message.From.Username);
        }

        private string ToString(Message message)
        {
            if (message.Text != null)
                return message.Text;

            if (message.LeftChatMember != null)
                return "Left";

            return "[File]";
        }
    }
}