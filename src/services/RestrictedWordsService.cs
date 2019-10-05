using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Services
{
	public class RestrictedWordService
	{
		public static async Task ScanMessage(SocketMessage message)
		{
			string[] bannedTerms = new string[] { "UwU", "OwO", "Yiff" };
			string[] insults = new string[] { $"Hey {message.Author.Username}, if I wanted to kill myself, I would climb up your ego and jump down to your IQ level.",
			$"{message.Author.Username}, your mother was a hamster and your father smelt of elderberries!",
			$"{message.Author.Username}, you are about as useful as a knitted condom.",
			$"I'm jealous of people that don’t know {message.Author.Username}!",
			$"If ignorance ever goes up to $5 a barrel, I want drilling rights to {message.Author.Username}'s head."};

			foreach (string str in bannedTerms)
			{
				if (message.Content.Equals(str, StringComparison.OrdinalIgnoreCase))
				{
					var random = new Random();
					var embed = new EmbedBuilder();
					EmbedFooterBuilder embedf = new EmbedFooterBuilder();
					var embedAuthor = new EmbedAuthorBuilder();

					embed.Color = Color.LightOrange;
					embedAuthor.Name = "Warning to Fuck Right Off";
					embedAuthor.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/octagonal-sign_1f6d1.png";
					embed.Title = $"Warning to the prick that triggered this warning, i'm looking at you, {message.Author.Username}";
					embedf.Text = insults[random.Next(0, insults.Length)];

					embed.Footer = embedf;
					embed.Author = embedAuthor;

					await message.DeleteAsync();
					await message.Channel.SendMessageAsync(message.Author.Mention);
					await message.Channel.SendMessageAsync(embed: embed.Build());
				}
			}
		}
	}
}
