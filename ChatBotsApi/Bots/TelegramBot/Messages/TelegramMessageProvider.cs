using System;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;
using Telegram.Bot.Types;

namespace ChatBotsApi.Bots.TelegramBot.Messages
{
    public class TelegramMessageProvider : IMessageProvider
    {
        public MessageData ToMessageData(object source, ChatData data)
        {
            if (source is not Message message)
                throw new InvalidCastException();

            return new MessageData(ToString(message), message.MessageId, data.GetUser(message.From.Id), data);
        }

        public UserData GetUserData(object source, ChatData data)
        {
            if (source is not Message message)
                throw new InvalidCastException();

            long userId = message.From.Id;

            if (data.ContainsUser(userId))
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