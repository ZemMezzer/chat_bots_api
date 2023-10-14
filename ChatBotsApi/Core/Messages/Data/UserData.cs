using System;

namespace ChatBotsApi.Core.Messages.Data
{
    [Serializable]
    public class UserData
    {
        public long UserId { get; }
        public string UserName { get; }

        public UserData(long id, string name)
        {
            UserId = id;
            UserName = name;
        }

        public override int GetHashCode()
        {
            return UserId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is not UserData userData)
                return false;

            return userData.UserId == UserId && userData.UserName == UserName;
        }
    }
}