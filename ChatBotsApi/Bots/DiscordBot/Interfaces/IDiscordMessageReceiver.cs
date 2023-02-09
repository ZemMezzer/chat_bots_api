using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace ChatBotsApi.Bots.DiscordBot.Interfaces
{
    public interface IDiscordMessageReceiver
    {
        public Task OnMessageReceived(DiscordClient sender, MessageCreateEventArgs e);
    }
}