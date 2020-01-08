using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Reflection;

namespace Blyatmir_Putin_Bot.Modules
{
	public class ShowVersion : ModuleBase<SocketCommandContext>
	{
		[Command("info")]
		public async Task DisplayVersion()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			EmbedBuilder embed = new EmbedBuilder
			{
				Title = "Current Status",
				ThumbnailUrl = @"https://cdn.discordapp.com/attachments/266903379534413826/664476722313297940/1ghjca.png",
				Fields = new List<EmbedFieldBuilder>
				{
					new EmbedFieldBuilder
					{
						Name = "OS Version",
						Value = $"{Environment.OSVersion}"
					},
					new EmbedFieldBuilder
					{
						Name = "Runtime Version",
						Value = $"{assembly.ImageRuntimeVersion}"
					},
					new EmbedFieldBuilder
					{
						Name = "Bot Version",
						Value = $"v{assembly.GetName().Version}"
					},
					new EmbedFieldBuilder
					{
						Name = "Uptime",
						Value = $"{GetUptime()}"
					}
				},

				Footer = new EmbedFooterBuilder
				{
					IconUrl = BotConfig.Client.CurrentUser.GetAvatarUrl(),
					Text = "The version number may or may not be wrong, but it's better than nothing"
				},
				Color = Color.Purple,
			};

			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}

		private string GetUptime()
		{
			TimeSpan difference = DateTime.Now.Subtract(BotConfig.StartTime);
			if (difference.TotalSeconds < 60)
				return (int)difference.TotalSeconds + " Seconds";

			else if (difference.TotalMinutes < 60)
				return (int)difference.TotalMinutes + " Minutes";

			else if (difference.TotalHours < 24)
				return (int)difference.TotalHours + " Hours";

			else if (difference.TotalDays < 1)
				return (int)difference.TotalDays + " Days";

			else if (difference.TotalDays > 365)
				return (int)(difference.Days / 365) + " Years";

			else
				return "Unkown";
		}
	}
}
