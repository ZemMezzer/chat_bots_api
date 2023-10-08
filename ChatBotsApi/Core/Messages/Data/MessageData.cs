using System;

namespace ChatBotsApi.Core.Messages.Data
{
    [Serializable]
    public readonly struct MessageData
    {
        public long Id { get; }
        public string Message { get; }
        public UserData Sender { get; }
        public ChatData ChatData { get; }
        public bool IsPrivateMessage { get; }

        public MessageData(string message, long id, UserData sender, ChatData chatData, bool isPrivateMessage)
        {
            Message = message;
            Sender = sender;
            ChatData = chatData;
            Id = id;
            IsPrivateMessage = isPrivateMessage;
        }
    }
}