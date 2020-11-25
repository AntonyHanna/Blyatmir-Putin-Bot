using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.WebSocket;
using ElCheapo.Managers;
using ElCheapo.Stores;
using System.Collections.Generic;
using System.Linq;

namespace Blyatmir_Putin_Bot.Services
{
	public static class GameNotifierService
	{
		private static StoreQueryService _queryService;

		public static StoreQueryService QueryService
		{
			get
			{
				if (_queryService == null)
				{
					_queryService = new StoreQueryService
					{
						TimeInterval = 14400000  /* 4 hours in milliseconds */
					};

					_queryService.QueryCompleted += PostMessage;
					_queryService.RegisterStore(new EpicGamesStore("Epic Store",
						"https://cdn2.unrealengine.com/Diesel%2Flogo%2FLogo_EpicGames_Black-1360x1360-f15ee5845c95eacd424199bdf326047631b4bc69.png"));

					_queryService.QueryProviders();
				}

				return _queryService;
			}
		}

		private static void PostMessage(IEnumerable<ElCheapo.Generics.Game> games)
		{
			PersistantStorage<LocalGame>.InitializeStorage();

			AddNewGames(games);
			RemoveOldGames(games);

			IEnumerable<SocketGuild> guilds = Startup.Client.Guilds;
			IEnumerable<LocalGame> recordedGames = PersistantStorage<LocalGame>.Read();

			for (int guildIdx = 0; guildIdx < guilds.Count(); guildIdx++)
			{
				Guild lGuild = Guild.GetGuildData(guilds.ElementAt(guildIdx));

				if(lGuild.AnnouncmentChannelId == 0)
				{
					Logger.Warning($"No announcment channel was set for guild [{guilds.ElementAt(guildIdx).Name}]");
					continue;
				}

				if(!lGuild.EnableGameNotifier)
				{
					continue;
				}

				for (int gameIdx = 0; gameIdx < recordedGames.Count(); gameIdx++)
				{
					if (recordedGames.ElementAt(gameIdx).Posted)
					{
						continue;
					}

					guilds.ElementAt(guildIdx).GetTextChannel(lGuild.AnnouncmentChannelId).SendMessageAsync(embed: GameEmbed(recordedGames.ElementAt(gameIdx)));
					recordedGames.ElementAt(gameIdx).Posted = true;
				}
			}

			PersistantStorage<LocalGame>.Write((recordedGames as List<LocalGame>));
		}

		private static Embed GameEmbed(LocalGame game)
		{
			EmbedBuilder builder = new EmbedBuilder
			{
				Title = game.Name,
				Url = "https://www.epicgames.com/store/en-US/product/" + game.Name.Replace(" ", "-").ToLower(),
				Color = new Color(r: 163, g: 255, b: 0),
				Author = new EmbedAuthorBuilder
				{
					IconUrl = "https://i.pinimg.com/originals/21/2d/1b/212d1b4d8d3f8ec990eb405e735b3f8d.png",
					Name = "Epic Games Store",
					Url = "https://www.epicgames.com/store/en-US/"
				},

				Description = game.Description,
				Fields = new List<EmbedFieldBuilder>
				{
					new EmbedFieldBuilder
					{
						Name = "Developer",
						Value = game.Developer,
						IsInline = true
					},
					new EmbedFieldBuilder
					{
						Name = "Publisher",
						Value = game.Publisher,
						IsInline = true
					},
					new EmbedFieldBuilder
					{
						Name = "Availability (UTC)",
						Value = game.EffectiveDate
					},
				}
			};



			return builder.Build();
		}

		private static void AddNewGames(IEnumerable<ElCheapo.Generics.Game> newGames)
		{
			bool isGameNew = true;
			IEnumerable<LocalGame> recordedGames = PersistantStorage<LocalGame>.Read();

			foreach (ElCheapo.Generics.Game game in newGames)
			{
				isGameNew = !(recordedGames as List<LocalGame>).Exists(s => s.Name == game.Name);

				if (isGameNew)
				{
					(recordedGames as List<LocalGame>).Add(new LocalGame(game));
				}
			}

			PersistantStorage<LocalGame>.Write((recordedGames as List<LocalGame>));
		}

		private static void RemoveOldGames(IEnumerable<ElCheapo.Generics.Game> newGames)
		{
			bool isGameStillFree = false;
			IEnumerable<LocalGame> recordedGames = PersistantStorage<LocalGame>.Read();

			foreach (ElCheapo.Generics.Game game in newGames)
			{
				isGameStillFree = (recordedGames as List<LocalGame>).Exists(s => s.Name == game.Name);

				if (!isGameStillFree)
				{
					(recordedGames as List<LocalGame>).Remove(new LocalGame(game));
				}
			}

			PersistantStorage<LocalGame>.Write((recordedGames as List<LocalGame>));
		}
	}
}
