using Blyatmir_Putin_Bot.Core.Database;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Core.Models
{
	public class Guild
	{
		private static DataContext DbContext => Startup.context;

		public ulong GuildId { get; set; }
		public string GuildName { get; set; }
		public ulong QuoteChannelId { get; set; }
		public ulong AnnouncmentChannelId { get; set; }
		public int FTriggerCount { get; set; }
		public double FTriggerCoolDown { get; set; }
		public bool EnableIntroMusic { get; set; }
		public bool EnableGameNotifier { get; set; }

		public Guild()
		{
			this.GuildName = default;
			this.GuildId = default;
			this.QuoteChannelId = default;
			this.AnnouncmentChannelId = default;
			this.FTriggerCount = 3;
			this.FTriggerCoolDown = 20.0;
			this.EnableIntroMusic = false;
			this.EnableGameNotifier = false;
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
			this.EnableGameNotifier = false;
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
		/// For use with the GuildAvailable event
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public static async Task GenerateGuildData(SocketGuild arg)
		{
			//loop through all the guilds
			for (int j = 0; j <= Startup.Client.Guilds.Count; j++)
			{
				//indexing for readonly collections
				var guild = Startup.Client.Guilds.ElementAt(j);
				bool isPresent = false;

				//dont run if there is no guild data 
				//otherwise compare the guild ids and only add the ones that are different
				if (DbContext.Guilds.Count() > 0)
				{
					foreach (var gld in DbContext.Guilds)
					{
						if (guild.Id == gld.GuildId)
						{
							isPresent = true;
						}
					}	
				}

				//for the ones not present add them to data
				if (!isPresent)
				{
					DbContext.Guilds.Add(new Guild(guild));
					await DbContext.SaveChangesAsync();
					Logger.Debug($"Creating a default guild data for [{guild.Name}]");
				}
			}
		}

		public static async Task GenerateMissingGuilds()
		{
			//loop through all the guilds
			for (int j = 0; j < Startup.Client.Guilds.Count; j++)
			{
				//indexing for readonly collections
				var guild = Startup.Client.Guilds.ElementAt(j);
				bool isPresent = false;

				//dont run if there is no guild data 
				//otherwise compare the guild ids and only add the ones that are different
				if (DbContext.Guilds.Count() > 0)
				{
					foreach (var gld in DbContext.Guilds)
					{
						if (guild.Id == gld.GuildId)
						{
							isPresent = true;
						}
					}
				}

				//for the ones not present add them to data
				if (!isPresent)
				{
					DbContext.Guilds.Add(new Guild(guild));
					await DbContext.SaveChangesAsync();
					Logger.Debug($"Creating a default guild data for [{guild.Name}]");
				}
			}
		}

		/// <summary>
		/// Gets a specific guilds GuildData
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Guild GetGuildData(SocketCommandContext context)
		{
			foreach (Guild data in DbContext.Guilds)
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
			foreach (Guild data in DbContext.Guilds)
				if (data.GuildId == guild.Id)
					return data;

			return default;
		}
	}
}

