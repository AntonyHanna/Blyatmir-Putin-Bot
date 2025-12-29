using BlyatmirPutin.Logic.Events;
using BlyatmirPutin.Logic.Modules;
using BlyatmirPutin.Logic.Services;
using BlyatmirPutin.Models.Interfaces;
using NetCord;
using NetCord.Gateway;
using NetCord.Gateway.ReconnectStrategies;
using NetCord.Gateway.Voice;
using NetCord.Logging;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace BlyatmirPutin.Logic.Discord
{
	public class DiscordManager : IDisposable
	{
		/// <summary>
		/// Represents the discord bot client
		/// </summary>
		/// <remarks>Will be null until <see cref="Setup(IConfiguration?)"/> is called</remarks>
        public GatewayClient GatewayClient { get; private set; }

        public GatewayClientConfiguration GatewayClientConfiguration { get; private set; }

		private ConsoleLogger _consoleLogger;

        public async Task Setup(IConfiguration config)
		{
			this._consoleLogger = new ConsoleLogger(LogLevel.Trace);

			this.GatewayClientConfiguration = new GatewayClientConfiguration
			{
				Logger = this._consoleLogger
			};

			this.GatewayClient = new GatewayClient(new BotToken(config.Token), this.GatewayClientConfiguration);
		}

		public async Task Start()
		{
			await InstallCommandsAsync();
			await GatewayClient.StartAsync();
		}

		public async Task InstallCommandsAsync()
		{
			ApplicationCommandService<ApplicationCommandContext> applicationCommandService = new ApplicationCommandService<ApplicationCommandContext>();

			applicationCommandService.AddModule<IntroMusicModule>();

			CustomVoiceStateUpdate.VoiceStateUpdateTriggered += IntroMusicService.PlayIntroMusic;

			GatewayClient.VoiceStateUpdate += async (VoiceState voice) =>
			{
				VoiceState cachedVoiceState = GatewayClient.Cache.Guilds[voice.GuildId].VoiceStates[voice.UserId];

				if (voice.User.IsBot)
				{
					return;
				}

				// Disconnect
				if (voice.ChannelId == null)
				{
					return;
				}

				if (cachedVoiceState.ChannelId == voice.ChannelId)
				{
					return;
				}

				CustomVoiceStateUpdateEventArgs args = new CustomVoiceStateUpdateEventArgs()
				{
					Client = this.GatewayClient,
					VoiceState = voice
				};

				CustomVoiceStateUpdate.OnVoiceStateUpdateTriggered(args);
			};

			GatewayClient.InteractionCreate += async (Interaction interaction) =>
			{
				if (interaction is not ApplicationCommandInteraction commandInteraction)
				{
					return;
				}

				IExecutionResult result = await applicationCommandService.ExecuteAsync(new ApplicationCommandContext(commandInteraction, this.GatewayClient));

				if (result is not IFailResult failResult)
				{
					return;
				}

				try
				{
					await interaction.SendResponseAsync(InteractionCallback.Message(failResult.Message));
				}
				catch
				{

				}

			};

			await applicationCommandService.RegisterCommandsAsync(GatewayClient.Rest, GatewayClient.Id);
		}

		public async Task UpdatePresenceAsync(PresenceProperties presence)
		{
			await this.GatewayClient.UpdatePresenceAsync(presence);
		}

		public void Dispose()
		{
			GatewayClient?.Dispose();
		}
	}
}
