using System;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;

namespace ChatBotsApi.Core.Messages
{
    internal static class MessageHandler
    {
        public static class Convert
        {
            public static MessageData ToMessageData(object message, ChatData chatData, IMessageProvider provider)
            {
                UserData user = provider.GetUserData(message, chatData);

                if (!chatData.ContainsUser(user.UserId))
                {
                    user.UserColor = GenerateColor();
                    chatData.AddUser(user);
                }

                return provider.ToMessageData(message, chatData);
            }
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

        public static MessageData AddMessageInChat(object message, ChatData chatData, IMessageProvider provider)
        {
            MessageData messageData = Convert.ToMessageData(message, chatData, provider);
            chatData.AddMessage(messageData);
            return messageData;
        }
        
        public static MessageData AddMessageInChat(MessageData message, ChatData chatData)
        {
            chatData.AddMessage(message);
            return message;
        }
    }
}