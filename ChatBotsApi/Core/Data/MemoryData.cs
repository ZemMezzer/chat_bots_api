using System;
using System.Collections.Generic;
using ChatBotsApi.Core.Messages.Data;

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

        public UserData GetUser(long id)
        {
            if (_users.ContainsKey(id))
                return _users[id];

            throw new KeyNotFoundException();
        }
    }
}