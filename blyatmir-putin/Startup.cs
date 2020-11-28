using Blyatmir_Putin_Bot.Core;
using Blyatmir_Putin_Bot.Core.Attributes;
using Blyatmir_Putin_Bot.Core.Database;
using Blyatmir_Putin_Bot.Core.Factories;
using Blyatmir_Putin_Bot.Core.Interfaces;
using Blyatmir_Putin_Bot.Core.Models;
using Blyatmir_Putin_Bot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot
{
	[DirectoryRequired("config")]
	class Startup
	{
		public static IAppSettings AppConfig;
		public static DiscordSocketClient Client;
		public static DataContext context;
		public static CommandService Commands;
		private static CommandHandler commandHandler;

		private static void Main()
			=> new Startup().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			await StartBotAsync();
		}

		/// <summary>
		/// Apply the bots configuration and start him asyncronously
		/// </summary>
		/// <returns></returns>
		public async static Task StartBotAsync()
		{
			AttributeLoader.LoadCustomAttributes();

			Client = new DiscordSocketClient(new DiscordSocketConfig
			{
				LogLevel = LogSeverity.Debug
			});

			Commands = new CommandService();
			commandHandler = new CommandHandler(Client, Commands);

			AppConfig = await SettingsFactory.CreateAsync();

			context = new DataContext();
			context.Database.Migrate(); /* ensure the db exists */

			if (string.IsNullOrWhiteSpace(AppConfig.Token))
			{
				Console.WriteLine("Failed to start... Bot Token was missing.\n\n" +
					"Troubleshooting:\n" +
					"If running on Windows or Linux make sure to fill in the Settings.xml file (generated after first launch)\n" +
					"If running on Docker make sure to pass in the BOT_TOKEN variable");
				Environment.Exit(-1);
			}

			await AttachEventHandlers();

			await Client.SetGameAsync(AppConfig.Activity);
			await Client.LoginAsync(TokenType.Bot, AppConfig.Token);
			await commandHandler.InstallCommandsAsync();

			await Client.StartAsync();

			Logger.Debug("Delaying the startup of the Game notifier service");

			new Thread(() =>
			{
				Thread.CurrentThread.IsBackground = true;
				Thread.Sleep(10000);
				GameNotifierService.QueryService.StartService();
				Logger.Debug("Game notifier service has been started");
			}).Start();

			//wait infinitely I think?
			await Task.Delay(-1);
		}

		/// <summary>
		/// Attaches the required event handlers to the bot
		/// </summary>
		private static async Task AttachEventHandlers()
		{
			await Task.Run(() =>
			{
				//responds with f's in chat
				Client.MessageReceived += FInChatService.CheckForLoss;

				//check messages for potential quotes
				Client.MessageReceived += QuoteManagamentService.QuoteIntentProcessorAsync;

				//Control how reactions should affect messages
				Client.ReactionAdded += ReactionHandlerService.ReactionAddedAsync;

				// Handle how services that require reactions should respond to the clearing of reactions
				Client.ReactionsCleared += ReactionHandlerService.ReactionsCleared;

				//Generate GuildData once Ready is fired
				Client.Ready += Guild.GenerateMissingGuilds;

				//Update serverdata when the bot joins a new guild
				Client.JoinedGuild += Guild.GenerateGuildData;

				Client.UserVoiceStateUpdated += IntroMusicService.PlayIntroMusic;

				//log messages in the console
				Client.Log += Log;

				AppDomain.CurrentDomain.ProcessExit += OnExitAsync;
			});
		}

		private static async void OnExitAsync(object sender, EventArgs e)
		{
			await Client.StopAsync();

			// Disconnect from all voice chats
			foreach (AudioService audioService in AudioService.AudioServices)
			{
				await audioService.DisconnectFromVoiceAsync();
			}

			Environment.Exit(1);
		}

		/// <summary>
		/// Dedicated logging method
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static Task Log(LogMessage message)
		{
			Task.Run(() => Logger.Debug(message.Message));
			return Task.CompletedTask;
		}
	}
}
