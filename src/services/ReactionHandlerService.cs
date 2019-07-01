using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.services
{
    public static class ReactionHandlerService
    {
        public static async Task ReactionControllerAsync(Cacheable<IUserMessage, ulong> messageId, ISocketMessageChannel channel, SocketReaction reaction)
        {
            //dont look at bot reactions
            if (reaction.User.Value.IsBot)
                return;

            if (reaction.MessageId == QuoteManagamentService.QuoteConfirmationMessage.Id && QuoteManagamentService.Quoter.Guild.Name == "Bot Test")
            {
                if (reaction.Emote.ToString() == "✅")
                    await QuoteManagamentService.QuoteConfirmedAsync();

                if (reaction.Emote.ToString() == "❎")
                    await QuoteManagamentService.QuoteDeniedAsync();
            }
        }
    }
}
