using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.modules
{
    [Name("help")]
    [Summary("Get some info on these darn commmands")]
    [Remarks("`help [none] - Gets a list of all commands\n" +
        "help [string command name] - Gets detailed info on a command module`")]
    public class help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Alias("?")]
        [Remarks("List all available commands")]
        public async Task Help()
        {
            var embedBuilder = new EmbedBuilder();
            var embedAuthor = new EmbedAuthorBuilder();

            embedBuilder.Description = "If you'd like more information on how to use a command a certain command then use `/help [command name]` to get more detailed information.";

            embedAuthor.Name = "List of Commands";
            embedAuthor.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/scroll_1f4dc.png";

            embedBuilder.Color = Color.Blue;

            foreach (ModuleInfo module in BotConfig.Commands.Modules)
            {
                var result = ((module.Summary == null) ? "No Description" : module.Summary);
                embedBuilder.AddField(module.Name, result);
            }

            embedBuilder.Author = embedAuthor;

            await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("help")]
        [Alias("?")]
        [Summary("Get more detailed info on an individual command")]
        public async Task Help([Remainder] string message)
        {
            var embedBuilder = new EmbedBuilder();
            var embedAuthor = new EmbedAuthorBuilder();

            ModuleInfo moduleInfo = default;
            
            foreach (ModuleInfo command in BotConfig.Commands.Modules)
                if (message == command.Name)
                    moduleInfo = command;

            if (message == moduleInfo.Name)
            {
                embedAuthor.Name = $"Module Name: {moduleInfo.Name}";
                embedAuthor.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/209/thinking-face_1f914.png";

                embedBuilder.AddField("Module Parameters", moduleInfo.Remarks, false);
                embedBuilder.Color = Color.Purple;
            }

            embedBuilder.Author = embedAuthor;

            await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
        }
    }
}
