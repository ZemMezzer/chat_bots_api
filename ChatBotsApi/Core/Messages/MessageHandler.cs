using System;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;

namespace ChatBotsApi.Core.Messages
{
    internal static class MessageHandler
    {
        public static class Convert
        {
            public static MessageData ToMessageData(object message, ChatData chat, IMessageProvider provider)
            {
                UserData user = provider.GetUserData(message, chat);

                if (!chat.ContainsUser(user.UserId))
                {
                    user.UserColor = GenerateColor();
                    chat.AddUser(user);
                }

                return provider.ToMessageData(message, chat);
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

        public static MessageData AddMessageInChat(object message, ChatData chat, IMessageProvider provider)
        {
            MessageData messageData = Convert.ToMessageData(message, chat, provider);
            chat.AddMessage(messageData);
            return messageData;
        }
        
        public static MessageData AddMessageInChat(MessageData message, ChatData chat)
        {
            chat.AddMessage(message);
            return message;
        }
    }
}