using blyatmir_putin.Core.Database;
using blyatmir_putin.Core.Models;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace blyatmir_putin.Modules
{
	[Group("config")]
	[Alias("cfg")]
	public class Configuration : ModuleBase<SocketCommandContext>
	{
		private DataContext DbContext => Startup.context;

		[Command("quotechannel")]
		[Alias(new string[] { "quote", "qc" })]
		public async Task AssignQuoteChannelAsync([Remainder] ITextChannel textChannel)
		{
			Guild guildData = Guild.GetGuildData(context: Context);
			guildData.SetQuoteChannel(textChannel);
			await DbContext.SaveChangesAsync();

			await Context.Channel.SendMessageAsync($"Quote channel has been assigned with id: `{textChannel.Id}` for the guild: `{guildData.GuildName}`");
		}

		[Command("announcmentchannel")]
		[Alias(new string[] { "announcement", "ac" })]
		public async Task AssignAnnouncmentChannelAsync([Remainder] ITextChannel textChannel)
		{
			Guild guildData = Guild.GetGuildData(context: Context);
			guildData.SetAnnouncmentChannel(textChannel);
			await DbContext.SaveChangesAsync();

			await Context.Channel.SendMessageAsync($"Announcment channel has been assigned with id: `{textChannel.Id}` for the guild: `{guildData.GuildName}`");
		}

		[Command("ftrigger")]
		[Alias("ftr")]
		public async Task SetFTriggerValue([Remainder] int value)
		{
			Guild guild = Guild.GetGuildData(Context);
			guild.SetFTriggerCount(value);
			await DbContext.SaveChangesAsync();

			await Context.Channel.SendMessageAsync($"The F trigger value for `{Context.Guild.Name}` has been updated to `{value}`");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">Value in seconds</param>
		/// <returns></returns>
		[Command("ftrigger cd")]
		public async Task SetFTriggerCooldownValue([Remainder] double value)
		{
			Guild guild = Guild.GetGuildData(Context);
			guild.SetFTriggerCooldown(value);
			await DbContext.SaveChangesAsync();

			await Context.Channel.SendMessageAsync($"The F trigger cooldown time for `{Context.Guild.Name}` has been updated to `{value}` seconds");
		}

		[Command("intromusic")]
		public async Task SetIntroMusic([Remainder] bool status)
		{
			Guild guildData = Guild.GetGuildData(Context);
			guildData.EnableIntroMusic = status;
			await DbContext.SaveChangesAsync();

			await Context.Channel.SendMessageAsync($"Intro Music has been set to `{status}` for `{guildData.GuildName}`");
		}
	}
}
