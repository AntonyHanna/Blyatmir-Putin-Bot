using Discord.WebSocket;
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
			string[] terms = new string[] { "F", "in", "chat", "respect", "respects", "assignment", "due", "fucked", "can", "get", "bois" };
			int fCounter = 0;
			bool skipCheck = false;

			Timer timer = new Timer
			{
				AutoReset = true,
				Interval = 30000,
			};

			timer.Elapsed += ResetCooldown;
			timer.Enabled = true;

			foreach (string str in terms)
			{
				if(message.Content.Contains("F", System.StringComparison.OrdinalIgnoreCase))
					skipCheck = true;

				else if (message.Content.Contains(str, System.StringComparison.OrdinalIgnoreCase))
					fCounter++;
			}

			if ((fCounter >= 2 && !message.Author.IsBot && !onCooldown) || (skipCheck && !onCooldown))
			{
				fCounter = 0;
				onCooldown = true;
				await message.Channel.SendMessageAsync(text: "F");
			}
		}

		private static void ResetCooldown(object sender, ElapsedEventArgs e) => onCooldown = false;
	}
}
