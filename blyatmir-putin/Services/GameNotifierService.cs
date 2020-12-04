using Discord;
using System;
using Discord.WebSocket;
using ElCheapo.Managers;
using ElCheapo.Stores;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blyatmir_putin.Core.Database;
using blyatmir_putin.Core.Models;

namespace blyatmir_putin.Services
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

		private static DataContext DbContext => Startup.context;

		private static void PostMessage(IEnumerable<ElCheapo.Generics.Game> games)
		{
			Task.Run(async () => 
			{
				await ImportNewGames(games);
				await RemoveOldGames(games);

				IEnumerable<SocketGuild> guilds = Startup.Client.Guilds;
				DbSet<LocalGame> recordedGames = DbContext.Games;

				for (int guildIdx = 0; guildIdx < guilds.Count(); guildIdx++)
				{
					Guild lGuild = Guild.GetGuildData(guilds.ElementAt(guildIdx));

					if (lGuild.AnnouncmentChannelId == 0)
					{
						Logger.Warning($"No announcment channel was set for guild [{guilds.ElementAt(guildIdx).Name}]");
						continue;
					}

					if (!lGuild.EnableGameNotifier)
					{
						continue;
					}

					for (int gameIdx = 0; gameIdx < recordedGames.Count(); gameIdx++)
					{
						if (recordedGames.AsEnumerable().ElementAt(gameIdx).Posted)
						{
							continue;
						}

						await guilds.ElementAt(guildIdx).GetTextChannel(lGuild.AnnouncmentChannelId).SendMessageAsync(embed: GameEmbed(recordedGames.AsEnumerable().ElementAt(gameIdx)));
						recordedGames.AsEnumerable().ElementAt(gameIdx).Posted = true;
					}
				}

				await DbContext.SaveChangesAsync();
			});

		}

		private static Embed GameEmbed(LocalGame game)
		{
			EmbedBuilder builder = new EmbedBuilder
			{
				Title = game.Name,
				Description = game.Description,
				ImageUrl = game.BannerUri,
				ThumbnailUrl = game.PosterUri,
				Color = new Color(r: 163, g: 255, b: 0),
				Url = "https://www.epicgames.com/store/en-US/product/" + game.Name.Replace(" ", "-").ToLower(),
				Author = new EmbedAuthorBuilder
				{
					IconUrl = "https://cdn.discordapp.com/attachments/598416605185310729/781731261525000232/epicgames_logo.png",
					Name = "Epic Games Store",
					Url = "https://www.epicgames.com/store/en-US/"
				},

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
						Value = game.StartDate + " - " + game.EndDate
					},
				}
			};



			return builder.Build();
		}

		private static async Task ImportNewGames(IEnumerable<ElCheapo.Generics.Game> newGames)
		{
			IQueryable<LocalGame> gamesDb = DbContext.Games.AsQueryable();
			bool isGameNew = true;

			foreach (ElCheapo.Generics.Game game in newGames)
			{
				isGameNew = gamesDb.Where(g => g.Name == game.Name).Count() == 0;

				if (isGameNew)
				{
					Logger.Debug($"Adding a new game to DB: [{game.Name}]");
					DbContext.Games.Add(new LocalGame(game));
				}
			}

			await DbContext.SaveChangesAsync();
		}

		private static async Task RemoveOldGames(IEnumerable<ElCheapo.Generics.Game> newGames)
		{
			DbSet<LocalGame> storedGames = DbContext.Games;

			foreach (LocalGame oldGame in storedGames)
			{
				bool gameExists = false;

				foreach (ElCheapo.Generics.Game newGame in newGames)
				{
					if (newGame.Name == oldGame.Name)
					{
						gameExists = true;
					}
				}

				if (!gameExists)
				{
					LocalGame tmp = storedGames.AsQueryable().First(g => g.Name == oldGame.Name);
					Logger.Debug($"Removing an old game from the DB: [{tmp.Name}]");
					storedGames.Remove(tmp);
				}
			}

			await DbContext.SaveChangesAsync();
		}
	}
}
