using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Blyatmir_Putin_Bot.Modules
{
    /// <summary>
    /// Transfers a channels recent messages from one channel to another
    /// </summary>
    [Name("messagetransfer")]
    [Summary("Gets the channels message and transfers to them to another channel")]
    [Remarks("`mt [ITextChannel sourceChannel] - Gets the channels message and transfers to them to another channel`")]
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
