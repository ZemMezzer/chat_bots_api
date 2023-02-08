using System;
using System.Collections.Generic;
using ChatBotsApi.Core.Messages.Data;

namespace ChatBotsApi.Core.Data
{
    [Serializable]
    public class BotData
    {
        public Dictionary<string, ChatData> Chats = new();
        public UserData BotUser;

        public BotData(long botId, string name, ColorData userColor)
        {
            BotUser = new UserData(botId, userColor, name);
        }
    }
}