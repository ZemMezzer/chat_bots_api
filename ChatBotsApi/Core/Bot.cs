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
        private static Dictionary<Type, Bot> _bots = new();

        public string Token { get; }
        public MemoryData Memory { get; private set; }

        public event Action<MessageData> OnMessageReceived;
        public event Action<MessageData> OnMessageSend;

        private Dictionary<Type, HashSet<object>>_messageReceivers = new();

        public Bot(string token)
        {
            Token += token;
            _bots.Add(GetType(), this);
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
            MessageHandler.AddMessageInChat(result, chatData);
            return result;
        }

        protected void MessageReceived(MessageData message)
        {
            OnMessageReceived?.Invoke(message);
        }

        protected void MessageSend(MessageData message) => OnMessageSend?.Invoke(message);

        protected abstract Task<MessageData> SendTextMessageInternal(string message, ChatData chatData);

        public void Save()
        {
            SaveDataHandler.SaveData(GetType().Name, Memory);
        }

        public static T GetBot<T>() where T : Bot
        {
            if (_bots.ContainsKey(typeof(T)))
                return (T)_bots[typeof(T)];

            return null;
        }
    }
}