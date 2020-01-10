using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Reflection;
using System.Linq;

namespace Blyatmir_Putin_Bot.Modules
{
	public class ShowInfo : ModuleBase<SocketCommandContext>
	{
		[Command("info")]
		public async Task DisplayInfo()
		{
			TimeSpan difference = DateTime.Now.Subtract(BotConfig.StartTime);
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
						Value = $"v{assembly.GetName().Version.ToString().Remove(assembly.GetName().Version.ToString().ToCharArray().Count() - 2)}"
					},
					new EmbedFieldBuilder
					{
						Name = "Bot Uptime",
						Value = $"{difference.Days} Days | {difference.Hours} Hours | {difference.Minutes} Minutes | {difference.Seconds} Seconds"
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
	}
}
