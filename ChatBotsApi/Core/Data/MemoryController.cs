using System;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;

namespace ChatBotsApi.Core.Data
{
    public static class MemoryController
    {
        public static void UpdateMemoryByMessage(object message, MemoryData memory, IMessageProvider provider)
        {
            UserData user = provider.GetUserData(message, memory);
            ChatData chat = provider.GetChatData(message, memory);

            if (!memory.GetUsers().ContainsKey(user.UserId))
            {
                user.UserColor = GenerateColor();
                memory.RememberUser(user);
            }

            if(!memory.GetChats().ContainsKey(chat.ChatId))
                memory.AddChat(chat);
        }
        
        private static ColorData GenerateColor()
        {
            int[] colors = new int[3];
            for (int i = 0; i < 3; i++)
            {
                Random random = new Random(i);
                colors[i] = random.Next(0, 255);
            }

            ColorData color = new ColorData(colors[0], colors[1], colors[2]);
            return color;
        }
    }
}