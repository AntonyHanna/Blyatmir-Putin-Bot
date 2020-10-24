using Blyatmir_Putin_Bot.Model;
using Blyatmir_Putin_Bot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot
{
	class Startup
	{
		public static IAppSettings AppConfig;
		public static DiscordSocketClient Client;
		public static CommandService Commands;
		private static CommandHandler commandHandler;

		public static DateTime StartTime { get; private set; }

		private static void Main()
			=> new Startup().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			StartTime = DateTime.Now;
			await StartBotAsync();
		}

		/// <summary>
		/// Apply the bots configuration and start him asyncronously
		/// </summary>
		/// <returns></returns>
		public async static Task StartBotAsync()
		{
			Client = new DiscordSocketClient(new DiscordSocketConfig
			{
				LogLevel = LogSeverity.Debug
			});

			Commands = new CommandService();
			commandHandler = new CommandHandler(Client, Commands);

			AppConfig = SettingsFactory.Create();

			if(string.IsNullOrWhiteSpace(AppConfig.Token))
			{
				Console.WriteLine("Failed to start... Bot Token was missing.\n\n" +
					"Troubleshooting:\n" +
					"If running on Windows or Linux make sure to fill in the Settings.xml file (generated after first launch)\n" +
					"If running on Docker make sure to pass in the BOT_TOKEN variable");
				Environment.Exit(-1);
			}

			AttachEventHandlers();

			await Client.SetGameAsync(AppConfig.Activity);
			await Client.LoginAsync(TokenType.Bot, AppConfig.Token);
			await commandHandler.InstallCommandsAsync();

			await Client.StartAsync();

			//wait infinitely I think?
			await Task.Delay(-1);
		}

		/// <summary>
		/// Attaches the required event handlers to the bot
		/// </summary>
		private static void AttachEventHandlers()
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

			Client.Ready += Container.GenerateMissingContiners;

			//Update serverdata when the bot joins a new guild
			Client.JoinedGuild += Guild.GenerateGuildData;

			Client.UserVoiceStateUpdated += IntroMusicService.PlayIntroMusic;

			//log messages in the console
			Client.Log += Log;

			AppDomain.CurrentDomain.ProcessExit += OnExitAsync;
		}

		private static async void OnExitAsync(object sender, EventArgs e)
		{
			await Client.StopAsync();
			SshController.SshClient.Disconnect();
			SshController.SshClient.Dispose();

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
			Task.Run(() => Console.WriteLine(message.ToString()));
			return Task.CompletedTask;
		}
	}
}
