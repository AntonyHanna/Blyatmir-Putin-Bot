using Discord.WebSocket;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Services
{
    public class FInChatService
    {
        /// <summary>
        /// Checks if the message should recieve an F
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task CheckForLoss(SocketMessage message)
        {
            string[] terms = new string[] { "F", "in", "chat", "respect", "respects", "assignment", "due", "fucked", "can", "get", "bois" };
            int fCounter = 0;

            foreach (string str in terms)
                if (message.Content.Contains(str))
                    fCounter++;

            if (fCounter >= 2 && !message.Author.IsBot)
            {
                fCounter = 0;
                await message.Channel.SendMessageAsync(text: "F");
            }
        }
    }
}
