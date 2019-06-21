using Discord.WebSocket;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.services
{
    public class FInChatService
    {
        public static async Task Respond(SocketMessage message)
        {
            //if(message.Content.Equals("F", StringComparison.OrdinalIgnoreCase) && !message.Author.IsBot)
            //{
            //    await message.Channel.SendMessageAsync(text: "F");
            //}

            string[] terms = new string[] { "F", "in", "chat", "respect", "respects", "assignment", "due", "fucked", "can", "get", "bois" };
            int fCounter = 0;

            foreach (string str in terms)
            {
                if (message.Content.Contains(str))
                {
                    fCounter++;
                }
            }

            if (fCounter >= 2 && !message.Author.IsBot)
            {
                fCounter = 0;
                await message.Channel.SendMessageAsync(text: "F");
            }
        }
    }
}
