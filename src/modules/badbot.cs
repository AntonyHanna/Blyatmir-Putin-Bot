using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	[Summary("Take a risk and call Blyatmir a bad communist comrade")]
	[Remarks("`badbot [none] - Tell the bot he is bad with his vodka`")]
	public class Badbot : ModuleBase<SocketCommandContext>
	{
		/// <summary>
		/// Tell the bot he's not good
		/// </summary>
		[Command("badbot")]
		[Alias("bad")]
		[Summary("Tell the bot he's a bad bot.")]
		public async Task BadBot()
		{
			//get server sepcific GuildData
			Guild contextSpecificData = Guild.GetServerData(context: Context);

			//increment their points
			contextSpecificData.Points--;

			//calculate new point statistics
			Guild.PointCalculations(contextSpecificData);

			//embed template
			var easyEmbed = new EasyEmbed()
			{
				AuthorName = "Blyat Boy Putin",
				AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/thumbs-down-sign_1f44e.png",
				EmbedColor = Color.Red,
				FooterText = $"The freedom man kobes' a single stick away from Blyatmir, Blyatmir only has {contextSpecificData.Points} stick(s) remaining"
			};

			//send the message
			await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
		}
	}
}
