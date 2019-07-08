using Blyatmir_Putin_Bot.model;
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
    public class Dice : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Roll a regular dice from 1-6
        /// </summary>
        /// <returns></returns>
        [Command("roll")]
        [Alias("dice")]
        [Summary("Roll a regular imaginary dice")]
        public async Task DiceRollAsync()
        {
            //new field woo hoo
            EmbedFieldBuilder field = new EmbedFieldBuilder()
            {
                Name = "Roll",
                Value = $"`{new Random().Next(1, 6)}`",
                IsInline = false
            };

            //embed template
            var easyEmbed = new EasyEmbed()
            {
                AuthorName = "(╯°□°）╯︵ ┻━┻",
                AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png",
                EmbedColor = Color.Green,
                EmbedTitle = $"Dice Roll Results",
                EmbedField = field,
                FooterText = $"Rolled on some mystical dice from bum fuck nowhere seems to have 6 possible value(s)... BLYAT!"
            };

            //send the message
            await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
        }

        /// <summary>
        /// Roll a dice from 1 to the number specified
        /// </summary>
        /// <param name="num1"></param>
        /// <returns></returns>
        [Command("roll")]
        [Alias("dice")]
        [Summary("Roll an imaginary dice with a specified number of sides")]
        public async Task DiceRollAsync(int num1)
        {
            //new field woo hoo
            EmbedFieldBuilder field = new EmbedFieldBuilder()
            {
                Name = "Roll",
                Value = $"`{new Random().Next(1, num1)}`",
                IsInline = false
            };

            //embed template
            var easyEmbed = new EasyEmbed()
            {
                AuthorName = "(╯°□°）╯︵ ┻━┻",
                AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png",
                EmbedColor = Color.Green,
                EmbedTitle = $"Dice Roll Results",
                EmbedField = field,
                FooterText = $"Rolled on some mystical dice from bum fuck nowhere seems to have {num1} possible value(s)... BLYAT!"
            };

            //send the message
            await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
        }

        /// <summary>
        /// Roll a dice from num1 to num2
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        [Command("roll")]
        [Alias("dice")]
        [Summary("Roll an imaginary dice using two numbers specifying a min and max")]
        public async Task DiceRollAsync(int num1, int num2)
        {
            //invalid number check
            if (num1 > num2)
            {
                //you get the point
                var failedEmbed = new EasyEmbed()
                {
                    AuthorName = "(╯°□°）╯︵ ┻━┻",
                    AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png",
                    EmbedColor = Color.Red,
                    EmbedTitle = $"Dice Roll Failed",
                    FooterText = $"Your first number should be smaller than your second number"
                };

                //same thing as always
                await Context.Channel.SendMessageAsync("", false, failedEmbed.Build(), null);
                return;
            }

            //ya its the thing you've seen like 100 times already
            var field = new EmbedFieldBuilder
            {
                Name = $"Roll",
                Value = $"`{new Random().Next(num1, num2)}`",
            };

            //wow templates, cool
            var successfulEmbed = new EasyEmbed()
            {
                AuthorName = "(╯°□°）╯︵ ┻━┻",
                AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png",
                EmbedColor = Color.Green,
                EmbedTitle = $"Dice Roll Results",
                EmbedField = field,
                FooterText = $"Rolled on some mystical dice from bum fuck nowhere seems to have {num2 - num1 + 1} possible value(s)... BLYAT!"
            };

            //sending the damn message
            await Context.Channel.SendMessageAsync("", false, successfulEmbed.Build(), null);
        }
    }
}
