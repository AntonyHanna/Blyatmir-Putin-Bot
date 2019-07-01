using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.services
{
    public static class MessageDeletedService
    {
        public static async Task MessageDeleted(Cacheable<IMessage, ulong> messageId, ISocketMessageChannel channel)
        {
            //check if message id is same as Quote Message ID
            //if it is stop quote message stuff
            //if not don't do anything
        }
    }
}
