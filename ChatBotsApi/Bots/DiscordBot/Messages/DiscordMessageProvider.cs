using System;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;
using DSharpPlus.Entities;

namespace ChatBotsApi.Bots.DiscordBot.Messages
{
    public class DiscordMessageProvider : IMessageProvider
    {
        public MessageData ToMessageData(object source, ChatData data)
        {
            if (source is not DiscordMessage message)
                throw new InvalidCastException();

            return new MessageData(message.Content, (long)message.Id, data.GetUser((long)message.Author.Id), data);
        }

        public UserData GetUserData(object source, ChatData data)
        {
            if (source is not DiscordMessage message)
                throw new InvalidCastException();
            
            long userId = (long)message.Author.Id;

            if (data.ContainsUser(userId))
                return data.GetUser(userId);
            
            return new UserData((long)message.Author.Id, message.Author.Username);
        }
    }
}