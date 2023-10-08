using System;
using ChatBotsApi.Core.Data;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;
using DSharpPlus.Entities;

namespace ChatBotsApi.Bots.DiscordBot.Messages
{
    public class DiscordMessageProvider : IMessageProvider
    {
        private readonly MemoryData _memory;

        public DiscordMessageProvider(MemoryData memory)
        {
            _memory = memory;
        }
        
        public MessageData ToMessageData(object source)
        {
            if (source is not DiscordMessage message)
                throw new InvalidCastException();

            return new MessageData(message.Content, (long)message.Id, _memory.GetUser((long)message.Author.Id), _memory.GetChats()[(long)message.ChannelId], message.Channel.IsPrivate);
        }

        public ChatData GetChatData(object source)
        {
            if (source is not DiscordMessage message)
                throw new InvalidCastException();
            
            string messageChatName = message.Channel.Name;
            if (!_memory.GetChats().ContainsKey((long)message.ChannelId))
                return new ChatData((long)message.ChannelId, messageChatName);

            return _memory.GetChats()[(long)message.ChannelId];
        }

        public UserData GetUserData(object source)
        {
            if (source is not DiscordMessage message)
                throw new InvalidCastException();
            
            long userId = (long)message.Author.Id;

            if (_memory.GetUsers().ContainsKey(userId))
                return _memory.GetUser(userId);
            
            return new UserData((long)message.Author.Id, message.Author.Username);
        }
    }
}