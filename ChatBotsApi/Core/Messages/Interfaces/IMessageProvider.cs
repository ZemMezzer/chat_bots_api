using ChatBotsApi.Core.Messages.Data;

namespace ChatBotsApi.Core.Messages.Interfaces
{
    internal interface IMessageProvider
    {
        public MessageData ToMessageData(object source);
        public ChatData GetChatData(object source);
        public UserData GetUserData(object source);
    }
}