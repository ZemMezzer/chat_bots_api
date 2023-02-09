using System.Threading.Tasks;
using ChatBotsApi.Bots.DiscordBot.Interfaces;
using ChatBotsApi.Bots.DiscordBot.Messages;
using ChatBotsApi.Core;
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
        
        public DiscordBot(string name, string token) : base(name, token)
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
            InitializeBotData((long)_client.CurrentUser.Id);
            _client.MessageCreated += OnMessageReceivedHandler;
            
            BindMessageReceivers<IDiscordMessageReceiver>();
            _messageProvider = new DiscordMessageProvider();
        }

        private async Task OnMessageReceivedHandler(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Message.Content) && e.Author != _client.CurrentUser)
            {
                string messageChatName = e.Channel.Name;
                if (!Data.Chats.ContainsKey(messageChatName))
                {
                    ChatData chatData = new ChatData((long)e.Channel.Id, messageChatName);
                    Data.Chats.Add(messageChatName, chatData);
                }

                var message = MessageHandler.AddMessageInChat(e.Message, Data.Chats[messageChatName], _messageProvider);
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

            MessageData result = MessageHandler.Convert.ToMessageData(sentMessage, chatData, _messageProvider);
            
            if (Data.Chats.ContainsKey(chatData.ChatName))
            {
                MessageSend(result);
            }

            return result;
        }
    }
}