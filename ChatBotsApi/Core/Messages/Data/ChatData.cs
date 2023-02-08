using System;
using System.Collections.Generic;

namespace ChatBotsApi.Core.Messages.Data
{
    [Serializable]
    public class ChatData
    {
        private readonly Dictionary<long, UserData> _users = new();
        private readonly List<MessageData> _messages = new();
        
        public long ChatId { get; }
        public string ChatName { get; }
        
        public IReadOnlyList<MessageData> Messages => _messages;

        [NonSerialized] public Action<MessageData> OnMessageReceived; 

        public ChatData(long id, string name)
        {
            ChatId = id;
            ChatName = name;
        }
        
        public bool ContainsUser(long userId)
        {
            return _users.ContainsKey(userId);
        }

        public UserData GetUser(long userId)
        {
            if (!ContainsUser(userId))
                return null;

            return _users[userId];
        }

        public void AddUser(UserData data) => _users.Add(data.UserId, data);

        public void AddMessage(MessageData message)
        {
            _messages.Add(message);
            OnMessageReceived?.Invoke(message);
        }
    }
}