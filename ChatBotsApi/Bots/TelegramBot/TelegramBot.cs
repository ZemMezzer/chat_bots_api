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

        public TelegramBot(string token, string name, string id) : base(token, name, id)
        {
            _client = new TelegramBotClient(token);
            _client.StartReceiving(OnMessageReceivedHandler, OnError);
            
            InitializeBotData();
            BindMessageReceivers<ITelegramMessageReceiver>();
            
            _messageProvider = new TelegramMessageProvider(Memory);
        }

        private async Task OnMessageReceivedHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Message != null)
            {
                MemoryController.UpdateMemoryByMessage(update.Message, Memory, _messageProvider);
                var message = MessageHandler.AddMessageInChat(update.Message, _messageProvider);

                if (update.Message.ReplyToMessage != null && update.Message.ReplyToMessage.From.Username == Id)
                {
                    var forwardedMessage = MessageHandler.Convert.ToMessageData(update.Message.ReplyToMessage, _messageProvider);
                    BotMessageForwarded(message, forwardedMessage);
                }
                else
                {
                    MessageReceived(message);
                }
            }
            
            foreach (var messageReceiver in GetBindReceivers<ITelegramMessageReceiver>())
                await messageReceiver.OnMessageReceived(client, update, token);
        }
        
        private Task OnError(ITelegramBotClient client, Exception exception, CancellationToken token) => throw exception;

        protected override async Task<MessageData> SendTextMessageInternal(string message, ChatData chatData)
        {
            var sentMessage = await _client.SendTextMessageAsync(chatData.ChatId, message);
            MemoryController.UpdateMemoryByMessage(sentMessage, Memory, _messageProvider);
            
            MessageData result = MessageHandler.Convert.ToMessageData(sentMessage, _messageProvider);
            
            if (Memory.GetChats().ContainsKey(chatData.ChatId))
            {
                MessageSend(result);
            }

            return result;
        }
    }
}