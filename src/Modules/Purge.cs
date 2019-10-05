using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	[Name("purge")]
	[Summary("Set fire to the rain (chat)")]
	[Remarks("`purge [int number] - Removes traces of your weird searches, you specify how many messages`")]
	public class purge : ModuleBase<SocketCommandContext>
	{
		/// <summary>
		/// Purge evidence of our comrades secrets
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		[Command("purge")]
		[Summary("Set fire to the chat")]
		public async Task PurgeChatAsync(int num)
		{
			//if true set count to 100
			//if false set count to the number specified + 1
			//+ 1 because we want to remove the message the user just sent to initiate this command
			//on top of the messages they originally wanted to delete
			var count = ((num > 100) ? 100 : num + 1);

			//just so it looks a bit smorter use the plural if num if more than one :)
			var endText = ((num > 1) ? "Messages." : "Message.");

			//gets the messages and deletes them
			var messages = await this.Context.Channel.GetMessagesAsync(count).FlattenAsync();
			await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

			//message embed format
			var easyEmbed = new EasyEmbed()
			{
				AuthorName = $"Purged {count} {endText}",
				AuthorIcon = $"https://cdn.betterttv.net/emote/55028cd2135896936880fdd7/1x",
				EmbedColor = Color.Red
			};

			//send the message
			await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
		}
	}
}
