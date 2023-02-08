﻿using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBotsApi.Bots.TelegramBot.Interfaces
{
    public interface ITelegramMessageReceiver
    {
        public Task OnMessageReceived(ITelegramBotClient client, Update update, CancellationToken token);
    }
}