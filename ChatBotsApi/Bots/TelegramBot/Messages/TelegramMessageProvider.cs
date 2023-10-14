using System;
using ChatBotsApi.Core.Data;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatBotsApi.Bots.TelegramBot.Messages
{
    internal class TelegramMessageProvider : IMessageProvider
    {
        private readonly MemoryData _memory;
        
        public TelegramMessageProvider(MemoryData memory)
        {
            _memory = memory;
        }
        
        public MessageData ToMessageData(object source)
        {
            if (source is not Message message)
                throw new InvalidCastException();
            
            return new MessageData(ToString(message), message.MessageId, _memory.GetUser(message.From.Id, message.From.Username), _memory.GetChats()[message.Chat.Id], message.Chat.Type == ChatType.Private);
        }

        public ChatData GetChatData(object source)
        {
            if (source is not Message message)
                throw new InvalidCastException();
            
            string messageChatName = message.Chat.Username ?? message.Chat.Title;
            if (!_memory.GetChats().ContainsKey(message.Chat.Id))
                return new ChatData(message.Chat.Id, messageChatName);

            return _memory.GetChats()[message.Chat.Id];
        }

        public UserData GetUserData(object source)
        {
            if (source is not Message message)
                throw new InvalidCastException();

            long userId = message.From.Id;
            return _memory.GetUser(userId, message.From.Username);
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