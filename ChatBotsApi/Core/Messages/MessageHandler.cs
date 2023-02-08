using System;
using ChatBotsApi.Core.Messages.Data;
using Telegram.Bot.Types;

namespace ChatBotsApi.Core.Messages
{
    internal static class MessageHandler
    {
        public static class Convert
        {
            public static MessageData ToMessageData(Message message, ChatData chat)
            {
                long userId = message.From.Id;
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
                    user = new UserData(userId, color, message.From.Username);
                    chat.AddUser(user);
                }
                else
                {
                    user = chat.GetUser(userId);
                }

                return new MessageData(ToString(message), user, chat);
            }


            public static string ToString(Message message)
            {
                if (message.Text != null)
                    return message.Text;

                if (message.LeftChatMember != null)
                    return "Left";

                return "[File]";
            }
        }

        public static MessageData AddMessageInChat(Message message, ChatData chat)
        {
            MessageData messageData = Convert.ToMessageData(message, chat);
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