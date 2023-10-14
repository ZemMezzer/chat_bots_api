﻿using System;
using System.Collections.Generic;

namespace ChatBotsApi.Core.Messages.Data
{
    [Serializable]
    public class ChatData
    {
        private readonly Dictionary<long, UserData> _users = new();
        private readonly Dictionary<long, MessageData> _messages = new();
        
        public long ChatId { get; }
        public string ChatName { get; }
        
        public IReadOnlyDictionary<long, MessageData> Messages => _messages;

        /// <summary>
        /// Warning! Not thread safe
        /// </summary>
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
            if(!_messages.ContainsKey(message.Id)) 
                _messages.Add(message.Id, message);
            
            OnMessageReceived?.Invoke(message);
        }
    }
}