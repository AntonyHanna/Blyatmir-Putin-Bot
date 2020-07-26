using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	[Name("Configuration")]
	[Group("config")]
	[Alias("cfg")]
	[Summary("Change configuration settings for your server")]
	[Remarks("`cfg quotechannel [ITextChannel textChannel] - Set a specific quote channel for your server`\n" +
		"`cfg announcmentchannel [ITextChannel textChannel] - Set a specific announcment channel for your server`\n" +
		"`cfg list [bool option] - Opt in or out of being listed in server leadboards`")]
	public class Configuration : ModuleBase<SocketCommandContext>
	{
		[Command("quotechannel")]
		[Alias(new string[] { "quote", "qc" })]
		[Summary("Set a specific quote channel for your server")]
		public async Task AssignQuoteChannelAsync([Remainder] ITextChannel textChannel)
		{
			Guild guildData = Guild.GetGuildData(context: Context);
			guildData.SetQuoteChannel(textChannel);
			Guild.Write(Guild.GuildDataList);

			await Context.Channel.SendMessageAsync($"Quote channel has been assigned with id: `{textChannel.Id}` for the guild: `{guildData.GuildName}`");
		}

		[Command("announcmentchannel")]
		[Alias(new string[] { "announcement", "ac" })]
		[Summary("Set a specific announcment channel for your server")]
		public async Task AssignAnnouncmentChannelAsync([Remainder] ITextChannel textChannel)
		{
			Guild guildData = Guild.GetGuildData(context: Context);
			guildData.SetAnnouncmentChannel(textChannel);
			Guild.Write(Guild.GuildDataList);

			await Context.Channel.SendMessageAsync($"Announcment channel has been assigned with id: `{textChannel.Id}` for the guild: `{guildData.GuildName}`");
		}

		[Command("ftrigger")]
		[Alias("ftr")]
		public async Task SetFTriggerValue([Remainder] int value)
		{
			Guild guild = Guild.GetGuildData(Context);
			guild.SetFTriggerCount(value);
			Guild.Write(Guild.GuildDataList);

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
			Guild.Write(Guild.GuildDataList);

			await Context.Channel.SendMessageAsync($"The F trigger cooldown time for `{Context.Guild.Name}` has been updated to `{value}` seconds");
		}

		[Command("intromusic")]
		public async Task SetIntroMusic([Remainder] bool status)
		{
			Guild guildData = Guild.GetGuildData(Context);
			guildData.EnableIntroMusic = status;
			Guild.Write(Guild.GuildDataList);

			await Context.Channel.SendMessageAsync($"Intro Music has been set to `{status}` for `{guildData.GuildName}`");
		}
	}
}
