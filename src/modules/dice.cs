using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.modules
{
    [Name("roll")]
    [Summary("Roll a dice, you should know how to use this")]
    [Remarks("`roll [none] - Roll a regular die\n" +
    "roll [int number1] - Roll a dice starting from 1 to number1\n" +
    "roll [int number1, int number2] Roll a die starting from number1 to number2`")]
    public class dice : ModuleBase<SocketCommandContext>
    {
        [Command("roll")]
        [Alias("dice")]
        [Summary("Roll a regular imaginary dice")]
        public async Task DiceRollAsync()
        {
            var embed = new EmbedBuilder();
            EmbedFooterBuilder embedf = new EmbedFooterBuilder();
            var embedAuthor = new EmbedAuthorBuilder();

            embed.Color = Color.Green;
            embed.Title = "Dice Roll Results";
            embedf.Text = $"Rolled on some mystical dice from bum fuck nowhere seems to have 6 possible value(s)... BLYAT!";
            embedAuthor.Name = "(╯°□°）╯︵ ┻━┻";
            embedAuthor.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png";
            embed.AddField("Roll", new Random().Next(1, 6));

            embed.Author = embedAuthor;
            embed.Footer = embedf;

            await Context.Channel.SendMessageAsync("", false, embed.Build(), null);
        }

        [Command("roll")]
        [Alias("dice")]
        [Summary("Roll an imaginary dice with a specified number of sides")]
        public async Task DiceRollAsync(int num1)
        {
            var embed = new EmbedBuilder();
            EmbedFooterBuilder embedf = new EmbedFooterBuilder();
            var embedAuthor = new EmbedAuthorBuilder();

            embed.Color = Color.Green;
            embed.Title = "Dice Roll Results";
            embedf.Text = $"Rolled on some mystical dice from bum fuck nowhere seems to have {num1} possible value(s)... BLYAT!";
            embedAuthor.Name = "(╯°□°）╯︵ ┻━┻";
            embedAuthor.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png";
            embed.AddField("Roll", new Random().Next(1, num1));

            embed.Footer = embedf;
            embed.Author = embedAuthor;

            await Context.Channel.SendMessageAsync("", false, embed.Build(), null);
        }

        [Command("roll")]
        [Alias("dice")]
        [Summary("Roll an imaginary dice using two numbers specifying a min and max")]
        public async Task DiceRollAsync(int num1, int num2)
        {

            if (num1 > num2)
            {
                var embedAlt = new EmbedBuilder();
                EmbedFooterBuilder embedFooterAlt = new EmbedFooterBuilder();
                var embedAuthorAlt = new EmbedAuthorBuilder();

                embedAlt.Color = Color.Red;
                embedAlt.Title = "Dice Roll Failed";
                embedFooterAlt.Text = $"Your first number should be smaller than your second number";
                embedAuthorAlt.Name = "(╯°□°）╯︵ ┻━┻";
                embedAuthorAlt.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png";

                embedAlt.Author = embedAuthorAlt;
                embedAlt.Footer = embedFooterAlt;

                await Context.Channel.SendMessageAsync("", false, embedAlt.Build(), null);
                return;
            }

            var embed = new EmbedBuilder();
            EmbedFooterBuilder embedf = new EmbedFooterBuilder();
            var embedAuthor = new EmbedAuthorBuilder();

            embed.Color = Color.Green;
            embed.Title = "Dice Roll Results";
            embedf.Text = $"Rolled on some mystical dice from bum fuck nowhere seems to have {num2 - num1 + 1} possible value(s)... BLYAT!";
            embedAuthor.Name = "(╯°□°）╯︵ ┻━┻";
            embedAuthor.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png";
            embed.AddField("Roll", new Random().Next(num1, num2));

            embed.Author = embedAuthor;
            embed.Footer = embedf;

            await Context.Channel.SendMessageAsync("", false, embed.Build(), null);
        }
    }
}
