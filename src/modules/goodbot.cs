using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
    /// <summary>
    /// Upvote the bot
    /// </summary>
    [Name("GoodBot")]
    [Summary("Tell Blyatmir that he is good communist comrade")]
    [Remarks("`goodbot [none] - Tell the bot he is good with his vodka`")]
    public class Goodbot : ModuleBase<SocketCommandContext>
    {
        [Command("goodbot")]
        [Alias("good")]
        [Summary("Tell the bot he's not shit")]
        public async Task GoodBot()
        {
            //pull up specific server data
            var contextSpecificData = PersistantStorage.GetServerData(context: Context);

            //increment points
            contextSpecificData.Points++;

            //calculate the servers differnt score
            PersistantStorage.PointCalculations(contextSpecificData);

            //some embed field
            var field = new EmbedFieldBuilder
            {
                Name = $"Stick saved from a non-communist life",
                Value = $"{contextSpecificData.Points} stick(s) reclaimed from the freedom man",
                IsInline = true
            };

            //embed template
            var easyEmbed = new EasyEmbed()
            {
                AuthorName = "Good Boy Putin",
                AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/thumbs-up-sign_1f44d.png",
                EmbedColor = Color.Green,
                EmbedField = field,
                FooterText = $"You free a poor communist stick, putin sees this and attacks you, you drop the stick, " +
                $"putin runs off with the stick. Putin now has: {contextSpecificData.Points} communist stick(s)."
            };

            //send the message
            await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
        }
    }
}
