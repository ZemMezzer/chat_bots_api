using System;
using System.Threading;
using System.Threading.Tasks;
using ChatBotsApi.Bots.TelegramBot.Interfaces;
using ChatBotsApi.Bots.TelegramBot.Messages;
using ChatBotsApi.Core;
using ChatBotsApi.Core.Data;
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

        public TelegramBot(string token) : base(token)
        {
            _messageProvider = new TelegramMessageProvider();
            _client = new TelegramBotClient(token);
            _client.StartReceiving(OnMessageReceivedHandler, OnError);
            
            InitializeBotData();
            BindMessageReceivers<ITelegramMessageReceiver>();
        }

        private async Task OnMessageReceivedHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Message != null)
            {
                MemoryController.UpdateMemoryByMessage(update.Message, Memory, _messageProvider);
                var message = MessageHandler.AddMessageInChat(update.Message, Memory, _messageProvider);
                MessageReceived(message);
            }

            foreach (var messageReceiver in GetBindReceivers<ITelegramMessageReceiver>())
                await messageReceiver.OnMessageReceived(client, update, token);
        }
        
        private Task OnError(ITelegramBotClient client, Exception exception, CancellationToken token) => throw exception;

        protected override async Task<MessageData> SendTextMessageInternal(string message, ChatData chatData)
        {
            var sentMessage = await _client.SendTextMessageAsync(chatData.ChatId, message);
            MemoryController.UpdateMemoryByMessage(sentMessage, Memory, _messageProvider);
            
            MessageData result = MessageHandler.Convert.ToMessageData(sentMessage, Memory, _messageProvider);
            
            if (Memory.GetChats().ContainsKey(chatData.ChatId))
            {
                MessageSend(result);
            }

            return result;
        }
    }
}