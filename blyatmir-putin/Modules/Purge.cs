using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	public class purge : ModuleBase<SocketCommandContext>
	{
		/// <summary>
		/// Purge evidence of our comrades secrets
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		[Command("purge")]
		public async Task PurgeChatAsync(int num)
		{
			var count = CalculateAmount(num);

			await DeleteMessages(num);

			var embed = new EmbedBuilder
			{
				Color = Color.Red,
				Author = new EmbedAuthorBuilder
				{
					Name = $"Purged {count} Message(s).",
					IconUrl = $"https://cdn.betterttv.net/emote/55028cd2135896936880fdd7/1x"
				},
			};

			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}


		/// <summary>
		/// Clamps the value so that it isn't too large or too small
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		internal int CalculateAmount(int count)
		{
			if (count > 100) return 101;
			else if (count < 0) return 0;
			return count + 1;
		}

		internal async Task DeleteMessages(int count)
		{
			try
			{
				var messages = await this.Context.Channel.GetMessagesAsync(count).FlattenAsync();
				await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
			}
			catch (Exception ex)
			{
				System.Console.WriteLine(ex.Message);
			}
		}
	}
}
