namespace ChatBotsApi.Core.Messages.Interfaces
{
    public interface IMessageProvider
    {
        public string ToString(object source);
        public long GetSenderId(object source);
        public string GetSenderNickname(object source);
    }
}