using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	/// <summary>
	/// Upvote the bot
	/// </summary>
	[Name("GoodBot")]
	[Summary("Praise Blyatmir for leading UBR into a new era")]
	[Remarks("`goodbot [none] - Tell the bot he is good with his vodka`")]
	public class Goodbot : ModuleBase<SocketCommandContext>
	{
		[Command("goodbot")]
		[Alias("good")]
		public async Task GoodBot()
		{
			//get server sepcific GuildData
			GuildData contextSpecificData = GuildData.GetServerData(context: Context);

			//increment points
			contextSpecificData.Points++;

			//calculate new point statistics
			GuildData.PointCalculations(contextSpecificData);

			//some embed field
			var field = new EmbedFieldBuilder
			{
				Name = $"Slavenski was rescued from the freedom man",
				Value = $"{contextSpecificData.Points} Slavenski(s) saved from the freedom man",
				IsInline = true
			};

			//embed template
			var easyEmbed = new EasyEmbed()
			{
				AuthorName = "Good Boy Blyat",
				AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/thumbs-up-sign_1f44d.png",
				EmbedColor = Color.Green,
				EmbedField = field,
				FooterText = $"You rescue a Slavenski, Blyatmir sees this and rewards you with a bottle of vodka" +
				$"Slavensk now has a population of {contextSpecificData.Points}."
			};

			//send the message
			await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
		}
	}
}
