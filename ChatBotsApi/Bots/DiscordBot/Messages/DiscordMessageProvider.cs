using System;
using ChatBotsApi.Core.Data;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;
using DSharpPlus.Entities;

namespace ChatBotsApi.Bots.DiscordBot.Messages
{
    public class DiscordMessageProvider : IMessageProvider
    {
        public MessageData ToMessageData(object source, MemoryData data)
        {
            if (source is not DiscordMessage message)
                throw new InvalidCastException();

            return new MessageData(message.Content, (long)message.Id, data.GetUser((long)message.Author.Id), data.GetChats()[(long)message.ChannelId]);
        }

        public ChatData GetChatData(object source, MemoryData data)
        {
            if (source is not DiscordMessage message)
                throw new InvalidCastException();
            
            string messageChatName = message.Channel.Name;
            if (!data.GetChats().ContainsKey((long)message.ChannelId))
                return new ChatData((long)message.ChannelId, messageChatName);

            return data.GetChats()[(long)message.ChannelId];
        }

        public UserData GetUserData(object source, MemoryData data)
        {
            if (source is not DiscordMessage message)
                throw new InvalidCastException();
            
            long userId = (long)message.Author.Id;

            if (data.GetUsers().ContainsKey(userId))
                return data.GetUser(userId);
            
            return new UserData((long)message.Author.Id, message.Author.Username);
        }
    }
}