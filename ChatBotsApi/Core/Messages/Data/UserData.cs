using System;

namespace ChatBotsApi.Core.Messages.Data
{
    [Serializable]
    public class UserData
    {
        public long UserId { get; }
        public ColorData UserColor { get; }
        public string UserName { get; }

        public UserData(long id, ColorData color, string name)
        {
            UserId = id;
            UserColor = color;
            UserName = name;
        }
    }
}