using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.modules
{
    [Name("purge")]
    [Summary("Set fire to the rain (chat)")]
    [Remarks("`purge [int number] - Removes traces of your weird searches, you specify how many messages`")]
    public class purge : ModuleBase<SocketCommandContext>
    {
        [Command("purge")]
        [Summary("Set fire to the chat")]
        public async Task PurgeChatAsync(int num)
        {
            var count = ((num > 100) ? 100 : num);
            var endText = ((num > 1) ? "Messages." : "Message.");

            var messages = await this.Context.Channel.GetMessagesAsync(count + 1).FlattenAsync();
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

            var embed = new EmbedBuilder();
            var embedAuthor = new EmbedAuthorBuilder();

            embedAuthor.Name = $"Purged {count} {endText}";

            embed.Author = embedAuthor;
            embedAuthor.IconUrl = "https://cdn.betterttv.net/emote/55028cd2135896936880fdd7/1x";
            embed.Color = Color.Red;

            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }
    }
}
