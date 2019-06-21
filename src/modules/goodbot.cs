using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.modules
{
    [Summary("Tell Blyatmir that he is good communist comrade")]
    [Remarks("`[none] - Tell the bot he is good with his vodka`")]
    public class goodbot : ModuleBase<SocketCommandContext>
    {
        [Command("goodbot")]
        [Alias("good")]
        [Summary("Tell the bot he's not shit")]
        public async Task GoodBot()
        {
            Env.points++;
            Env.PointCalculations();

            var embed = new EmbedBuilder();
            var embedAuthor = new EmbedAuthorBuilder();
            var embedFooter = new EmbedFooterBuilder();

            embedAuthor.Name = "Good Boy Putin";
            embedAuthor.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/thumbs-up-sign_1f44d.png";

            embedFooter.Text = $"You free a poor communist stick, putin sees this and attacks you, you drop the stick, " +
                $"putin runs off with the stick. Putin now has: {Env.points} communist stick(s).";

            embed.Color = Color.Green;
            embed.AddField("Stick saved from a non-communist life", $"{Env.points} stick(s) reclaimed from the freedom man", true);
            embed.Author = embedAuthor;
            embed.Footer = embedFooter;

            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }
    }
}
