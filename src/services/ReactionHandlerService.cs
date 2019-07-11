using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.services
{
    public static class ReactionHandlerService
    {
        public static async Task ReactionAddedAsync(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            //dont look at bot reactions
            if (reaction.User.Value.IsBot)
                return;

            await ReactionControlsAddedAsync(reaction);
        }

        /// <summary>
        /// Handle reaction controls for Quotes
        /// </summary>
        /// <param name="reaction"></param>
        private static async Task ReactionControlsAddedAsync(SocketReaction reaction)
        {
            if (reaction.MessageId == QuoteManagamentService.QuoteConfirmationMessage.Id)
            {
                if (reaction.Emote.ToString() == "✅")
                    await QuoteManagamentService.QuoteConfirmedAsync();

                if (reaction.Emote.ToString() == "❎")
                    await QuoteManagamentService.QuoteDeniedAsync();
            }
        }

        public static async Task ReactionsCleared(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel)
        {
            await ReactionControlsClearedAsync(message);
        }

        /// <summary>
        /// Handle the clearing of reactions for services
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static async Task ReactionControlsClearedAsync(Cacheable<IUserMessage, ulong> message)
        {
            if (message.Id == QuoteManagamentService.QuoteConfirmationMessage.Id)
                await QuoteManagamentService.AddReactionsAsync();
        }
    }
}
