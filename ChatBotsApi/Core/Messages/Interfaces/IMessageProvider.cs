using ChatBotsApi.Core.Data;
using ChatBotsApi.Core.Messages.Data;

namespace ChatBotsApi.Core.Messages.Interfaces
{
    public interface IMessageProvider
    {
        public MessageData ToMessageData(object source, MemoryData data);
        public ChatData GetChatData(object source, MemoryData data);
        public UserData GetUserData(object source, MemoryData data);
    }
}