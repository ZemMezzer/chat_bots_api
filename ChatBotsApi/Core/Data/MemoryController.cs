using System;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;

namespace ChatBotsApi.Core.Data
{
    internal static class MemoryController
    {
        public static void UpdateMemoryByMessage(object message, MemoryData memory, IMessageProvider provider)
        {
            ChatData chat = provider.GetChatData(message);

            if(!memory.GetChats().ContainsKey(chat.ChatId))
                memory.AddChat(chat);
        }
    }
}