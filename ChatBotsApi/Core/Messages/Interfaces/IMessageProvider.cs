using ChatBotsApi.Core.Messages.Data;

namespace ChatBotsApi.Core.Messages.Interfaces
{
    public interface IMessageProvider
    {
        public MessageData ToMessageData(object source, ChatData data);
        public UserData GetUserData(object source, ChatData data);
    }
}