using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Blyatmir_Putin_Bot.Services
{
	public class FInChatService
	{
		private static bool onCooldown = false;

		/// <summary>
		/// Checks if the message should recieve an F
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static async Task CheckForLoss(SocketMessage message)
		{
			if (message.Author.IsBot)
				return;

			string[] terms = new string[] { "F", "in", "chat", "respect", "respects", "assignment", "due", "fucked", "can", "get", "bois" };
			int fCounter = 0;
			bool skipCheck = false;

			Timer timer = new Timer
			{
				AutoReset = true,
				Interval = 30000,
			};

			timer.Elapsed += ResetCooldown;

			string[] words = message.Content.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

			for(int i = 0; i < terms.Length; i++)
			{
				for(int x = 0; x <words.Length; x++)
				{
					if(words[x].Equals("F"))
					{
						fCounter += 3;
					}

					if(words[x].Equals(terms[i], System.StringComparison.OrdinalIgnoreCase))
					{
						fCounter++;
					}
				}
			}

			if ((fCounter >= 3 && !onCooldown))
			{
				fCounter = 0;
				onCooldown = true;
				timer.Enabled = true;

				await message.Channel.SendMessageAsync(text: "F");
			}
		}

		private static void ResetCooldown(object sender, ElapsedEventArgs e) => onCooldown = false;
	}
}
