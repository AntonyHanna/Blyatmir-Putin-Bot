using Blyatmir_Putin_Bot.model;
using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.modules
{
    [Name("Leaderboard")]
    [Summary("Display a list of servers ordered by their points")]
    [Remarks("`leaderboard [none] - Display a list of servers ordered by their points`")]
    public class Leaderboard : ModuleBase<SocketCommandContext>
    {
        private EmbedAuthorBuilder author;
        private EmbedBuilder embed;
        private EmbedFooterBuilder footer;

        [Command("leaderboard")]
        [Alias("lb")]
        [Summary("Display a list of servers ordered by their points")]
        public async Task ShowLeaderboard()
        {
            PrepareEmbed();

            author.Name = "Leaders of the USSR";
            author.IconUrl = "https://cdn.discordapp.com/attachments/559700127275679762/595131707490041886/1200px-Adidas_Logo.svg.png";

            embed.Color = Color.Gold;
            embed.Description = "The current leaders of the USSR are as follows:";

            footer.IconUrl = "https://cdn.discordapp.com/attachments/559700127275679762/595117013203025950/902023-1.png";
            footer.Text = "This was an automated message, don't even at me comrade";

            var listedGuilds = from GuildData guilds in PersistantStorage.ServerDataList
                          where guilds.IsListed == true
                          select guilds;

            var orderAndListedGuilds = from GuildData guilds in listedGuilds
                          orderby guilds.Points descending
                          select guilds;

            string[] places = new string[] { "First", "Second", "Third" };

            for (int i = 0; i < listedGuilds.Count(); i++)
                embed.AddField($"**{places[i]} Place**", $"{orderAndListedGuilds.ElementAt(i).GuildName} | `{orderAndListedGuilds.ElementAt(i).Points}`");

            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        /// <summary>
        /// Resets the embeds, in preperation for re-use
        /// </summary>
        private void PrepareEmbed()
        {
            author = new EmbedAuthorBuilder();
            embed = new EmbedBuilder();
            footer = new EmbedFooterBuilder();

            embed.Author = author;
            embed.Footer = footer;
        }
    }
}
