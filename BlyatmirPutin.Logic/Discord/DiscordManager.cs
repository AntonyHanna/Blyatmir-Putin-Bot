using BlyatmirPutin.Common.Logging;
using BlyatmirPutin.Models.Interfaces;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

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

		public async Task ConnectAsync(IConfiguration? config)
		{
			Client = new DiscordSocketClient(new DiscordSocketConfig
			{
				GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildVoiceStates | GatewayIntents.GuildMessages,
				LogLevel = LogSeverity.Debug,
				UseSystemClock = true
			});

			CommandService = new CommandService(new CommandServiceConfig
			{ 
				LogLevel = LogSeverity.Debug,
				CaseSensitiveCommands = true
			});

			Client.Log += Client_Log;
			CommandService.Log += Client_Log;

			CommandHandler handler = new CommandHandler(Client, CommandService);

			await handler.InstallCommandsAsync();
			await Client.LoginAsync(TokenType.Bot, config?.Token);
			await Client.SetGameAsync(config?.Activity);
			await Client.StartAsync();
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
