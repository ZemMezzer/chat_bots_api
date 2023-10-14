using System;
using System.Collections.Generic;
using ChatBotsApi.Core.Messages.Data;
using Telegram.Bot.Types;

namespace ChatBotsApi.Core.Data
{
    [Serializable]
    public class MemoryData
    {
        private Dictionary<long, ChatData> _chats = new();
        private Dictionary<long, UserData> _users = new();

        public IReadOnlyDictionary<long, ChatData> GetChats() => _chats;
        public IReadOnlyDictionary<long, UserData> GetUsers() => _users;

        public void AddChat(ChatData data)
        {
            if(!_chats.ContainsKey(data.ChatId))
                _chats.Add(data.ChatId, data);
        }
        
        public void RememberUser(UserData data)
        {
            if(!_users.ContainsKey(data.UserId))
                _users.Add(data.UserId, data);
        }

        public UserData GetUser(long id, string userName)
        {
            if (_users.TryGetValue(id, out var result))
                return result;

            UserData user = new UserData(id, userName);
            _users.Add(id, user);
            return user;
        }
    }
}