using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChatBotsApi.Bots.TelegramBot.Interfaces;
using ChatBotsApi.Common.Extensions;
using ChatBotsApi.Core;
using ChatBotsApi.Core.Messages;
using ChatBotsApi.Core.Messages.Data;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBotsApi.Bots.TelegramBot
{
    public class TelegramBot : Bot
    {
        private readonly TelegramBotClient _client;
        private readonly HashSet<ITelegramMessageReceiver> _messageReceivers = new();
        

        public TelegramBot(string name, string token) : base(name, token)
        {
            _client = new TelegramBotClient(token);
            _client.StartReceiving(OnMessageReceivedHandler, OnError);
            
            InitializeBotData((long)_client.BotId);

            Type[] receiversTypes = typeof(ITelegramMessageReceiver).GetAllConcreteChildTypes();

            foreach (var messageReceiver in receiversTypes.ActivateAllTypes<ITelegramMessageReceiver>())
                _messageReceivers.Add(messageReceiver);
        }

        private async Task OnMessageReceivedHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Message != null)
            {
                string messageChatName = update.Message.Chat.Username ?? update.Message.Chat.Title;
                if (!Data.Chats.ContainsKey(messageChatName))
                {
                    ChatData chat = new ChatData(update.Message.Chat.Id, messageChatName);
                    Data.Chats.Add(messageChatName, chat);
                }

                var message = MessageHandler.AddMessageInChat(update.Message, Data.Chats[messageChatName]);
                MessageReceived(message);
            }

            foreach (var messageReceiver in _messageReceivers)
                await messageReceiver.OnMessageReceived(client, update, token);
        }
        
        private Task OnError(ITelegramBotClient client, Exception exception, CancellationToken token) => throw exception;

        protected override async Task SendMessageInternal(MessageData message, ChatData chat)
        {
            if (Data.Chats.ContainsKey(chat.ChatName))
            {
                MessageSend(message);
            }
            
            await _client.SendTextMessageAsync(chat.ChatId, message.Message);
        }
    }
}