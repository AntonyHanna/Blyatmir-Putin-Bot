using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	/// <summary>
	/// Transfers a channels recent messages from one channel to another
	/// </summary>
	public class messagetransfer : ModuleBase<SocketCommandContext>
	{
		[Command("messagetransfer")]
		[Alias("mt")]
		public async Task MessageTransfer(ITextChannel sourceChannel)
		{
			//get all presently "cached" messages in the channel
			var sourceMessages = await sourceChannel.GetMessagesAsync().FlattenAsync();

			//send the cached messages in another channel
			foreach (var msg in sourceMessages.Reverse())
				await Context.Channel.SendMessageAsync(msg.Content);
		}
	}
}
