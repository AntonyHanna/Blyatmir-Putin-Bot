using BlyatmirPutin.Common.Logging;
using BlyatmirPutin.DataAccess.Database;
using BlyatmirPutin.Logic.Factories;
using BlyatmirPutin.Logic.Services;
using BlyatmirPutin.Models.Common;
using BlyatmirPutin.Models.Interfaces;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BlyatmirPutin.Logic.Discord
{
	public class DiscordManager : IDisposable
	{
		/// <summary>
		/// Represents the discord bot client
		/// </summary>
		/// <remarks>Will be null until <see cref="ConnectAsync(IConfiguration?)"/> is called</remarks>
		public DiscordSocketClient? Client { get; private set; }

		public CommandService? CommandService{ get; private set; }

		public InteractionService? InteractionService { get; private set; }

		public async Task ConnectAsync(IConfiguration? config)
		{
	
			Client = new DiscordSocketClient(new DiscordSocketConfig
			{
				GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildVoiceStates | GatewayIntents.GuildMessages,
				LogLevel = LogSeverity.Debug,
				UseSystemClock = true,
				MaxWaitBetweenGuildAvailablesBeforeReady = 500
			});

			CommandService = new CommandService(new CommandServiceConfig
			{
				LogLevel = LogSeverity.Debug,
				CaseSensitiveCommands = true
			});

			InteractionService = new InteractionService(Client.Rest, new InteractionServiceConfig
			{
				LogLevel = LogSeverity.Debug,
			});

			Client.Log += Client_Log;
			Client.Ready += Client_Ready;

			CommandService.Log += Client_Log;

			Client.UserVoiceStateUpdated += IntroMusicService.PlayIntroMusic;

			CommandHandler handler = new CommandHandler(Client, CommandService, InteractionService);

			

			await handler.InstallCommandsAsync();
			await Client.LoginAsync(TokenType.Bot, config?.Token);
			await Client.SetGameAsync(config?.Activity);
			await Client.StartAsync();
		}

		private async Task Client_Ready()
		{
			await InteractionService?.RegisterCommandsGloballyAsync();
		}

		public async Task DisconnectAsync()
		{
			if(Client != null)
				await Client.StopAsync();
		}

		private Task Client_Log(LogMessage arg)
		{
			switch(arg.Severity)
			{
				case LogSeverity.Info:
				case LogSeverity.Verbose:
					Logger.LogInfo(arg.Message, arg.Source);
					break;

				case LogSeverity.Debug:
					Logger.LogDebug(arg.Message, arg.Source);
					break;

				case LogSeverity.Warning:
					Logger.LogWarning(arg.Message, arg.Source);
					break;

				case LogSeverity.Error:
					Logger.LogError(arg.Message, arg.Source);
					break;

				case LogSeverity.Critical:
					Logger.LogCritical(arg.Message, arg.Source);
					break;
			}
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			Client?.Dispose();
		}
	}
}
