using System;
using ChatBotsApi.Core.Messages.Interfaces;
using Telegram.Bot.Types;

namespace ChatBotsApi.Bots.TelegramBot.Messages
{
    public class TelegramMessageProvider : IMessageProvider
    {
        public string ToString(object source)
        {
            if (source is not Message message)
                throw new InvalidCastException();
            
            if (message.Text != null)
                return message.Text;

            if (message.LeftChatMember != null)
                return "Left";

            return "[File]";
        }

        public long GetSenderId(object source)
        {
            if (source is not Message message)
                throw new InvalidCastException();

            return message.From.Id;
        }

        public string GetSenderNickname(object source)
        {
            if (source is not Message message)
                throw new InvalidCastException();

            return message.From.Username;
        }
    }
}