using System;

namespace ChatBotsApi.Core.Messages.Data
{
    [Serializable]
    public readonly struct MessageData
    {
        public string Message { get; }
        public UserData Sender { get; }
        public ChatData Chat { get; }

        public MessageData(string message, UserData sender, ChatData chat)
        {
            Message = message;
            Sender = sender;
            Chat = chat;
        }
    }
}