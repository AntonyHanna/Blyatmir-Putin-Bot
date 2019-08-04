using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
    [Name("highscore")]
    [Summary("Get the Blyatmirs stick history")]
    [Remarks("`highscore [none] - Get the some highscores from Blyatmirs history`")]
    public class Highscore : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Displays a Guilds highest points and lowest points
        /// </summary>
        /// <returns></returns>
        [Command("highscore")]
        [Alias("hs")]
        [Summary("Look at how many sticks I, err I mean we had...")]
        public async Task Highscores()
        {
            string lessThanText = "";

            GuildData guildData = PersistantStorage.GetServerData(context: Context);

            //sets the less than text to different values based on the point scenario
            if (guildData.Points == guildData.HighestPoints)
                lessThanText = ($"There are no bad times in the motherland");

            else if (guildData.Points < guildData.HighestPoints)
                lessThanText = $"I said I had like {guildData.HighestPoints} stick(s) but then " +
                    $"they became our stick(s) and now I only have {guildData.LowestPoints} stick :(";

            //list of embeds for the embed template
            var fieldList = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder { Name = "Stick Highs", Value = $"I had like so many sticks, at least {guildData.HighestPoints} of them" },
                new EmbedFieldBuilder { Name = "Stick Lows", Value = $"{lessThanText}" }
            };

            //embed template
            var easyEmbed = new EasyEmbed()
            {
                AuthorName = "Our good commie boy putin",
                AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/thumbs-up-sign_1f44d.png",
                EmbedColor = Color.Green,
                EmbedFieldList = fieldList
            };

            //send the message
            await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
        }
    }
}
