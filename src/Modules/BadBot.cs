using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	/// <summary>
	/// Downvote the bot
	/// </summary>
	[Name("BadBot")]
	[Summary("Take a risk and call Blyatmir a bad communist comrade")]
	[Remarks("`badbot [none] - Tell the bot he is bad with his vodka`")]
	public class Badbot : ModuleBase<SocketCommandContext>
	{
		[Command("badbot")]
		[Alias("bad")]
		public async Task BadBot()
		{
			//get server sepcific GuildData
			Guild contextSpecificData = Guild.GetServerData(context: Context);

			//decrement points
			contextSpecificData.Points--;

			//calculate new point statistics
			Guild.PointCalculations(contextSpecificData);

			//some embed field
			var field = new EmbedFieldBuilder
			{
				Name = $"Slavenski was taken by the freedom man",
				Value = $"{contextSpecificData.Points} Slavenski(s) lost to the freedom man",
				IsInline = true
			};

			//embed template
			var easyEmbed = new EasyEmbed()
			{
				AuthorName = "Bad Boy Blyat",
				AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/thumbs-down-sign_1f44e.png",
				EmbedColor = Color.Red,
				EmbedField = field,
				FooterText = $"You lose a Slavenski, Blyatmir looks disgusted and takes your bottle of vodka. " +
				$"Slavensk now has a population of {contextSpecificData.Points}."
			};

			//send the message
			await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
		}
	}
}
