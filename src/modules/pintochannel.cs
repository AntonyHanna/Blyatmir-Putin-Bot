using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;

namespace Blyatmir_Putin_Bot.modules
{
    /// <summary>
    /// Transfers a text channels pinned messages to another text channels chat
    /// </summary>
    [Name("pintochannel")]
    [Summary("Gets channel pins and transfers to them to another channel as text")]
    [Remarks("`ptc [ITextChannel sourceChannel] - Gets channel pins and transfers to them to another channel as text`")]
    public class quotet : ModuleBase<SocketCommandContext>
    {
        [Command("pintochannel")]
        [Alias("ptc")]
        public async Task PinToTextChannel(ITextChannel sourceChannel)
        {
            //get the pinned messages of the specified sourceChannel
            var pinnedMessages = await sourceChannel.GetPinnedMessagesAsync();

            //foreach message, write the messages to another channel in text form
            foreach (var msg in pinnedMessages.Reverse())
                await Context.Channel.SendMessageAsync(msg.Content);
        }
    }
}
