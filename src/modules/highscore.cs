using Blyatmir_Putin_Bot.model;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.modules
{
    [Name("highscore")]
    [Summary("Get the Blyatmirs stick history")]
    [Remarks("`highscore [none] - Get the some highscores from Blyatmirs history`")]
    public class Highscore : ModuleBase<SocketCommandContext>
    {
        [Command("highscore")]
        [Alias("hs")]
        [Summary("Look at how many sticks I, err I mean we had...")]
        public async Task Highscores()
        {
            var embed = new EmbedBuilder();
            var embedAuthor = new EmbedAuthorBuilder();
            var embedFooter = new EmbedFooterBuilder();

            embed.Color = Color.Green;
            embedAuthor.Name = "Our good commie boy putin";
            embedAuthor.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/thumbs-up-sign_1f44d.png";

            string lessThanText = "";

            GuildData guildData = PersistantStorage.GetServerData(context: Context);

            if (guildData.Points == guildData.HighestPoints)
                lessThanText = ($"There are no bad times in the motherland");

            else if (guildData.Points < guildData.HighestPoints)
                lessThanText = $"I said I had like {guildData.HighestPoints} stick(s) but then " +
                    $"they became our stick(s) and now I only have {guildData.LowestPoints} stick :(";

            embed.AddField("Stick Highs", $"I had like so many sticks, at " +
                $"least {guildData.HighestPoints} of them", false);

            embed.AddField($"Stick Lows", $"{lessThanText}", true);

            embed.Author = embedAuthor;
            embed.Footer = embedFooter;

            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }
    }
}
