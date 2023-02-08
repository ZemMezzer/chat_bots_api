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
                long userId = provider.GetSenderId(message);
                UserData user;
                
                if (!chat.ContainsUser(userId))
                {
                    int[] colors = new int[3];
                    for (int i = 0; i < 3; i++)
                    {
                        Random random = new Random(i);
                        colors[i] = random.Next(0, 255);
                    }

                    ColorData color = new ColorData(colors[0], colors[1], colors[2]);
                    user = new UserData(userId, color, provider.GetSenderNickname(message));
                    chat.AddUser(user);
                }
                else
                {
                    user = chat.GetUser(userId);
                }

                return new MessageData(provider.ToString(message), user, chat);
            }
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