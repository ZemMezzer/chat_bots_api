using System;

namespace ChatBotsApi.Core.Messages.Data
{
    [Serializable]
    public class UserData
    {
        public long UserId { get; }
        public ColorData UserColor { get; set; }
        public string UserName { get; }

        public UserData(long id, string name)
        {
            UserId = id;
            UserName = name;
        }
    }
}