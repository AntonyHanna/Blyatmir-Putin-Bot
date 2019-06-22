using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.modules
{
    [Summary("Take a risk and call Blyatmir a bad communist comrade")]
    [Remarks("`badbot [none] - Tell the bot he is bad with his vodka`")]
    public class badbot : ModuleBase<SocketCommandContext>
    {
        [Command("badbot")]
        [Alias("bad")]
        [Summary("Tell the bot he's a bad bot.")]
        public async Task BadBot()
        {
            PointManager.points--;
            PointManager.PointCalculations();

            var embed = new EmbedBuilder();
            var embedAuthor = new EmbedAuthorBuilder();
            var embedFooter = new EmbedFooterBuilder();

            embed.Color = Color.Red;
            embedAuthor.Name = "Blyat Boy Putin";
            embedAuthor.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/thumbs-down-sign_1f44e.png";

            embedFooter.Text = $"The freedom man kobes' a single stick away from Blyatmir, Blyatmir only has {PointManager.points} stick(s) remaining";

            embed.Author = embedAuthor;
            embed.Footer = embedFooter;

            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }
    }
}
