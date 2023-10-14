using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBotsApi.Common.Extensions;
using ChatBotsApi.Common.Handlers;
using ChatBotsApi.Core.Data;
using ChatBotsApi.Core.Messages;
using ChatBotsApi.Core.Messages.Data;

namespace ChatBotsApi.Core
{
    [Serializable]
    public abstract class Bot
    {
        public string Token { get; }
        public MemoryData Memory { get; private set; }
        public string Name { get; }
        public string Id { get; }
        
        
        /// <summary>
        /// Invokes when message of bot was forwarded
        /// MessageData: CurrentMessage; MessageData: ForwardedMessage
        /// </summary>
        public event Action<MessageData, MessageData> OnBotMessageForwarded; 
        public event Action<MessageData> OnMessageSend;
        public event Action<MessageData> OnMessageReceived;

        private Dictionary<Type, HashSet<object>>_messageReceivers = new();

        public Bot(string token, string name, string id)
        {
            Token += token;
            
            Name = name;
            Id = id;
        }

        protected void BindMessageReceivers<T>()
        {
            Type[] receiversTypes = typeof(T).GetAllConcreteChildTypes();
            foreach (var messageReceiver in receiversTypes.ActivateAllTypes<T>())
            {
                if (_messageReceivers.ContainsKey(typeof(T)))
                {
                    _messageReceivers[typeof(T)].Add(messageReceiver);
                }
                else
                {
                    _messageReceivers.Add(typeof(T), new HashSet<object>(){messageReceiver});
                }
            }
        }

        protected IReadOnlyCollection<T> GetBindReceivers<T>()
        {
            if (!_messageReceivers.ContainsKey(typeof(T)))
                return Array.Empty<T>();

            HashSet<T> buffer = new HashSet<T>();
            foreach (var obj in _messageReceivers[typeof(T)])
                buffer.Add((T) obj);

            return buffer;
        }

        protected void InitializeBotData()
        {
            Memory = SaveDataHandler.TryLoadData(GetType().Name, out MemoryData data) ? data : new MemoryData();
        }

        public async Task<MessageData> SendTextMessage(string message, ChatData chatData)
        {
            MessageData result = await SendTextMessageInternal(message, chatData);
            chatData.AddMessage(result);
            return result;
        }

        protected void MessageReceived(MessageData message)
        {
            OnMessageReceived?.Invoke(message);
        }

        protected void BotMessageForwarded(MessageData currentMessage, MessageData forwardedMessage)
        {
            OnBotMessageForwarded?.Invoke(currentMessage, forwardedMessage);
        }

        protected void MessageSend(MessageData message) => OnMessageSend?.Invoke(message);

        protected abstract Task<MessageData> SendTextMessageInternal(string message, ChatData chatData);

        public void Save()
        {
            SaveDataHandler.SaveData(GetType().Name, Memory);
        }
    }
}