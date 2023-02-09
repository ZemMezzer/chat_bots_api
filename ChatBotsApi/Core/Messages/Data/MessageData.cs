using System;

namespace ChatBotsApi.Core.Messages.Data
{
    [Serializable]
    public readonly struct MessageData
    {
        public long Id { get; }
        public string Message { get; }
        public UserData Sender { get; }
        public ChatData Chat { get; }

        public MessageData(string message, long id, UserData sender, ChatData chat)
        {
            Message = message;
            Sender = sender;
            Chat = chat;
            Id = id;
        }
    }
}