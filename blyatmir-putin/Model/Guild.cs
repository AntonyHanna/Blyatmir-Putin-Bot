using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Blyatmir_Putin_Bot.Model
{
	public class Guild : PersistantStorage<Guild>
	{
		public static List<Guild> GuildDataList = new List<Guild>(PersistantStorage<Guild>.Read());

		public ulong GuildId { get; set; }
		public string GuildName { get; set; }
		public ulong QuoteChannelId { get; set; }
		public ulong AnnouncmentChannelId { get; set; }
		public int FTriggerCount { get; set; }
		public double FTriggerCoolDown { get; set; }
		public bool EnableIntroMusic { get; set; }

		public Guild()
		{
			this.GuildName = default;
			this.GuildId = default;
			this.QuoteChannelId = default;
			this.AnnouncmentChannelId = default;
			this.FTriggerCount = 3;
			this.FTriggerCoolDown = 20.0;
			this.EnableIntroMusic = false;
		}
		public Guild(IGuild guild)
		{
			this.GuildName = guild.Name;
			this.GuildId = guild.Id;
			this.QuoteChannelId = default;
			this.AnnouncmentChannelId = default;
			this.FTriggerCount = 3;
			this.FTriggerCoolDown = 20.0;
			this.EnableIntroMusic = false;
		}

		/// <summary>
		/// Sets a value for the QuoteChannel property
		/// </summary>
		/// <param name="channel"></param>
		public void SetQuoteChannel(ITextChannel channel)
		{
			this.QuoteChannelId = (channel as SocketTextChannel).Id;
		}

		/// <summary>
		/// Sets a value for the AnnouncmentChannel property
		/// </summary>
		/// <param name="channel"></param>
		public void SetAnnouncmentChannel(ITextChannel channel)
		{
			this.AnnouncmentChannelId = (channel as SocketTextChannel).Id;
		}

		/// <summary>
		/// Sets the trigger value for when F should be sent
		/// </summary>
		/// <param name="value"></param>
		public void SetFTriggerCount(int value)
		{
			this.FTriggerCount = value;
		}

		/// <summary>
		/// Sets the F trigger cooldown timer
		/// </summary>
		/// <param name="value">Value in seconds</param>
		public void SetFTriggerCooldown(double value)
		{
			this.FTriggerCoolDown = value;
		}

		/// <summary>
		/// Generates guild data for every server that the bot is present in
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void GenerateGuildData(object sender, ElapsedEventArgs e)
		{
			//Create the config directory if it doesn't exist
			if (!Directory.Exists(AppEnvironment.ConfigLocation))
				Directory.CreateDirectory(AppEnvironment.ConfigLocation);

			//loop through all the guilds
			for (int j = 0; j < BotConfig.Client.Guilds.Count; j++)
			{
				//indexing for readonly collections
				var guild = BotConfig.Client.Guilds.ElementAt(j);
				bool isPresent = false;

				//dont run if there is no guild data 
				//otherwise compare the guild ids and only add the ones that are different
				if (GuildDataList.Count() > 0)
					foreach (var gld in GuildDataList)
						if (guild.Id == gld.GuildId)
							isPresent = true;

				//for the ones not present add them to data
				if (!isPresent)
				{
					GuildDataList.Add(new Guild(guild));
					PersistantStorage<Guild>.Write(GuildDataList);
					Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Create      Default data has been written for Guild: {guild.Name}");
				}
			}
		}

		/// <summary>
		/// For use with the GuildAvailable event
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public static Task GenerateGuildData(SocketGuild arg)
		{
			//Create the config directory if it doesn't exist
			if (!Directory.Exists(AppEnvironment.ConfigLocation))
				Directory.CreateDirectory(AppEnvironment.ConfigLocation);

			//loop through all the guilds
			for (int j = 0; j <= BotConfig.Client.Guilds.Count; j++)
			{
				//indexing for readonly collections
				var guild = BotConfig.Client.Guilds.ElementAt(j);
				bool isPresent = false;

				//dont run if there is no guild data 
				//otherwise compare the guild ids and only add the ones that are different
				if (GuildDataList.Count() > 0)
					foreach (var gld in GuildDataList)
						if (guild.Id == gld.GuildId)
							isPresent = true;

				//for the ones not present add them to data
				if (!isPresent)
				{
					GuildDataList.Add(new Guild(guild));
					PersistantStorage<Guild>.Write(GuildDataList);
					Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Create      Default data has been written for Guild: {guild.Name}");
				}
			}

			return Task.CompletedTask;
		}

		public static Task GenerateMissingGuilds()
		{
			//Create the config directory if it doesn't exist
			if (!Directory.Exists(AppEnvironment.ConfigLocation))
				Directory.CreateDirectory(AppEnvironment.ConfigLocation);

			//loop through all the guilds
			for (int j = 0; j < BotConfig.Client.Guilds.Count; j++)
			{
				//indexing for readonly collections
				var guild = BotConfig.Client.Guilds.ElementAt(j);
				bool isPresent = false;

				//dont run if there is no guild data 
				//otherwise compare the guild ids and only add the ones that are different
				if (GuildDataList.Count() > 0)
					foreach (var gld in GuildDataList)
						if (guild.Id == gld.GuildId)
							isPresent = true;

				//for the ones not present add them to data
				if (!isPresent)
				{
					GuildDataList.Add(new Guild(guild));
					PersistantStorage<Guild>.Write(GuildDataList);
					Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Create      Default data has been written for Guild: {guild.Name}");
				}
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Gets a specific guilds GuildData
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Guild GetGuildData(SocketCommandContext context)
		{
			foreach (Guild data in GuildDataList)
				if (data.GuildId == context.Guild.Id)
					return data;

			return default;
		}

		/// <summary>
		/// Gets a specific guilds GuildData
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Guild GetGuildData(IGuild guild)
		{
			foreach (Guild data in GuildDataList)
				if (data.GuildId == guild.Id)
					return data;

			return default;
		}
	}
}

