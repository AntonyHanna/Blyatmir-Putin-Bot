using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	/// <summary>
	/// Displays a list of command modules available
	/// </summary>
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
			var fieldList = new List<EmbedFieldBuilder>();

			//foreach availble module
			//set the module description to "No Description" if there is no available description
			//create fields based on the module name and result of the above condition
			foreach (ModuleInfo module in BotConfig.Commands.Modules)
			{
				var result = (module.Summary == null) ? "No Description" : module.Summary;
				var field = new EmbedFieldBuilder { Name = module.Name, Value = result };
				fieldList.Add(field);
			}

			//embed template
			var easyEmbed = new EasyEmbed()
			{
				AuthorName = "List of Commands",
				AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/scroll_1f4dc.png",
				EmbedColor = Color.Blue,
				EmbedDescription = $"If you'd like more information on how to use a command a certain command then use `/help [command name]` " +
				$"to get more detailed information.",
				EmbedFieldList = fieldList
			};

			//send the message
			await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
		}

		/// <summary>
		/// Displays more detailed info on a specified command
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		[Command("help")]
		[Alias("?")]
		[Summary("Get more detailed info on an individual command")]
		public async Task Help([Remainder] string message)
		{

			ModuleInfo moduleInfo = default;

			//loop through the commands modules available
			//and check for which one has the same name as what the user typed
			//assign moduleInfo the command if there is a match
			foreach (ModuleInfo command in BotConfig.Commands.Modules)
				if (message == command.Name)
					moduleInfo = command;

			//might be a pointless check
			if (message == moduleInfo.Name)
			{
				var field = new EmbedFieldBuilder { Name = "Module Parameters", Value = moduleInfo.Remarks };

				//embed template
				var easyEmbed = new EasyEmbed()
				{
					AuthorName = $"Module Name: {moduleInfo.Name}",
					AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/209/thinking-face_1f914.png",
					EmbedColor = Color.Purple,
					EmbedField = field
				};

				//send the message
				await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
			}
		}
	}
}
