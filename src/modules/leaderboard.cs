using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
    [Name("Leaderboard")]
    [Summary("Display a list of servers ordered by their points")]
    [Remarks("`leaderboard [none] - Display a list of servers ordered by their points`")]
    public class Leaderboard : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Shows a leaderboard with all listed guilds
        /// </summary>
        /// <returns></returns>
        [Command("leaderboard")]
        [Alias("lb")]
        [Summary("Display a list of servers ordered by their points")]
        public async Task ShowLeaderboard()
        {
            var fieldList = new List<EmbedFieldBuilder>();

            //get all guilds that wish to be listed, by default is true
            var listedGuilds = from GuildData guilds in PersistantStorage.ServerDataList
                               where guilds.IsListed == true
                               select guilds;

            //order the listed guilds from largest to smallest in order to mimic a leaderboard
            var orderAndListedGuilds = from GuildData guilds in listedGuilds
                                       orderby guilds.Points descending
                                       select guilds;

            //just some suffixes for our top 3 guilds
            string[] places = new string[] { "First", "Second", "Third" };

            //foreach listed guild add a field with their info
            for (int i = 0; i < listedGuilds.Count(); i++)
                fieldList.Add(new EmbedFieldBuilder { Name = $"**{places[i]} Place**", Value = $"{orderAndListedGuilds.ElementAt(i).GuildName} | `{orderAndListedGuilds.ElementAt(i).Points}`" });

            //embed template
            var easyEmbed = new EasyEmbed()
            {
                AuthorName = "Leaders of the USSR",
                AuthorIcon = $"https://cdn.discordapp.com/attachments/559700127275679762/595131707490041886/1200px-Adidas_Logo.svg.png",
                EmbedColor = Color.Gold,
                EmbedDescription = $"The current leaders of the USSR are as follows:",
                EmbedFieldList = fieldList,
                FooterIcon = $"https://cdn.discordapp.com/attachments/559700127275679762/595117013203025950/902023-1.png",
                FooterText = $"This was an automated message, don't even at me comrade"
            };

            //send the message
            await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
        }
    }
}
