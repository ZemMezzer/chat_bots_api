using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public BotData Data { get; private set; }
        
        
        protected readonly ColorData Color;
        protected string Name { get; }

        public event Action<MessageData> OnMessageReceived;
        public event Action<MessageData> OnMessageSend;

        public Bot(string name, string token) : this(name, token, new ColorData(1,1,1,1))
        {
            
        }

        public Bot(string name, string token, ColorData color)
        {
            Token += token;
            Name = name;
            _bots.Add(GetType(), this);
            Color = color;
        }

        protected void InitializeBotData(long clientBotId)
        {
            Data = SaveDataHandler.TryLoadData(Name, out BotData data) ? data : new BotData(clientBotId, Name, Color);
        }

        public async Task SendMessage(MessageData message, ChatData chat)
        {
            MessageHandler.AddMessageInChat(message, chat);
            await SendMessageInternal(message, chat);
        }

        protected void MessageReceived(MessageData message) => OnMessageReceived?.Invoke(message);
        protected void MessageSend(MessageData message) => OnMessageSend?.Invoke(message);

        protected abstract Task SendMessageInternal(MessageData message, ChatData chat);

        public void Save()
        {
            SaveDataHandler.SaveData(Name, Data);
        }

        public static T GetBot<T>() where T : Bot
        {
            if (_bots.ContainsKey(typeof(T)))
                return (T)_bots[typeof(T)];

            return null;
        }
    }
}