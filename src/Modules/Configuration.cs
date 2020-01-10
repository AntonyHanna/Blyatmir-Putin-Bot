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
			PersistantStorage<Guild>.Write(Guild.GuildDataList);

			await Context.Channel.SendMessageAsync($"Quote channel has been assigned with id: `{textChannel.Id}` for the guild: `{guildData.GuildName}`");
		}

		[Command("announcmentchannel")]
		[Alias(new string[] { "announcement", "ac" })]
		[Summary("Set a specific announcment channel for your server")]
		public async Task AssignAnnouncmentChannelAsync([Remainder] ITextChannel textChannel)
		{
			Guild guildData = Guild.GetGuildData(context: Context);
			guildData.SetAnnouncmentChannel(textChannel);
			PersistantStorage<Guild>.Write(Guild.GuildDataList);

			await Context.Channel.SendMessageAsync($"Announcment channel has been assigned with id: `{textChannel.Id}` for the guild: `{guildData.GuildName}`");
		}

		[Command("list")]
		[Alias("ls")]
		[Summary("Opt in or out of being listed in server leadboards")]
		public async Task DontListServer([Remainder] bool selection)
		{
			Guild guildData = Guild.GetGuildData(context: Context);

			guildData.IsListed = selection;
			PersistantStorage<Guild>.Write(Guild.GuildDataList);

			if (selection)
				await Context.Channel.SendMessageAsync($"`{guildData.GuildName}` has been opted out of being listed in scoreboards");

			if (!selection)
				await Context.Channel.SendMessageAsync($"`{guildData.GuildName}` has been opted into being listed in scoreboards");
		}

		[Command("ftrigger")]
		[Alias("ftr")]
		public async Task SetFTriggerValue([Remainder] int value)
		{
			Guild guild = Guild.GetGuildData(Context);
			guild.SetFTriggerCount(value);
			PersistantStorage<Guild>.Write(Guild.GuildDataList);

			await Context.Channel.SendMessageAsync($"The F trigger value for `{Context.Guild.Name}` has been updated to `{value}`");
		}
	}
}
