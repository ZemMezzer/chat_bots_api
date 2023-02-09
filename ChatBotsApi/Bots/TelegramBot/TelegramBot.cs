using System;
using System.Threading;
using System.Threading.Tasks;
using ChatBotsApi.Bots.TelegramBot.Interfaces;
using ChatBotsApi.Bots.TelegramBot.Messages;
using ChatBotsApi.Core;
using ChatBotsApi.Core.Messages;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBotsApi.Bots.TelegramBot
{
    public class TelegramBot : Bot
    {
        private readonly TelegramBotClient _client;
        private readonly IMessageProvider _messageProvider;

        public TelegramBot(string name, string token) : base(name, token)
        {
            _messageProvider = new TelegramMessageProvider();
            _client = new TelegramBotClient(token);
            _client.StartReceiving(OnMessageReceivedHandler, OnError);
            
            InitializeBotData((long)_client.BotId);
            BindMessageReceivers<ITelegramMessageReceiver>();
        }

        private async Task OnMessageReceivedHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Message != null)
            {
                string messageChatName = update.Message.Chat.Username ?? update.Message.Chat.Title;
                if (!Data.Chats.ContainsKey(messageChatName))
                {
                    ChatData chatData = new ChatData(update.Message.Chat.Id, messageChatName);
                    Data.Chats.Add(messageChatName, chatData);
                }

                var message = MessageHandler.AddMessageInChat(update.Message, Data.Chats[messageChatName], _messageProvider);
                MessageReceived(message);
            }

            foreach (var messageReceiver in GetBindReceivers<ITelegramMessageReceiver>())
                await messageReceiver.OnMessageReceived(client, update, token);
        }
        
        private Task OnError(ITelegramBotClient client, Exception exception, CancellationToken token) => throw exception;

        protected override async Task<MessageData> SendTextMessageInternal(string message, ChatData chatData)
        {
            var sentMessage = await _client.SendTextMessageAsync(chatData.ChatId, message);
            MessageData result = MessageHandler.Convert.ToMessageData(sentMessage, chatData, _messageProvider);
            
            if (Data.Chats.ContainsKey(chatData.ChatName))
            {
                MessageSend(result);
            }

            return result;
        }
    }
}