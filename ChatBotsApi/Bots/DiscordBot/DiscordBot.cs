using System.Threading.Tasks;
using ChatBotsApi.Bots.DiscordBot.Interfaces;
using ChatBotsApi.Bots.DiscordBot.Messages;
using ChatBotsApi.Core;
using ChatBotsApi.Core.Data;
using ChatBotsApi.Core.Messages;
using ChatBotsApi.Core.Messages.Data;
using ChatBotsApi.Core.Messages.Interfaces;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace ChatBotsApi.Bots.DiscordBot
{
    public class DiscordBot : Bot
    {
        private readonly DiscordClient _client;
        private IMessageProvider _messageProvider;
        
        public DiscordBot(string token, string name, string id) : base(token, name, id)
        {
            DiscordConfiguration configuration = new DiscordConfiguration
            {
                Token = token,
                Intents = DiscordIntents.All
            };

            _client = new DiscordClient(configuration);
            Task.Run(InitializeClient).Wait();
        }

        private async Task InitializeClient()
        {
            await _client.ConnectAsync();
            InitializeBotData();
            _client.MessageCreated += OnMessageReceivedHandler;
            
            BindMessageReceivers<IDiscordMessageReceiver>();
            _messageProvider = new DiscordMessageProvider(Memory);
        }

        private async Task OnMessageReceivedHandler(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Message.Content) && e.Author != _client.CurrentUser)
            {
                MemoryController.UpdateMemoryByMessage(e.Message, Memory, _messageProvider);
                var message = MessageHandler.AddMessageInChat(e.Message, _messageProvider);
                MessageReceived(message);
            }

            var receivers = GetBindReceivers<IDiscordMessageReceiver>();
            foreach (var messageReceiver in receivers)
                await messageReceiver.OnMessageReceived(sender, e);
        }

        protected override async Task<MessageData> SendTextMessageInternal(string message, ChatData chatData)
        {
            var channel = await _client.GetChannelAsync((ulong) chatData.ChatId);
            var sentMessage = await channel!.SendMessageAsync(message);

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