using Blyatmir_Putin_Bot.model;
using Blyatmir_Putin_Bot.services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot
{
    class BotConfig
    {
        public static DiscordSocketClient Client;
        public static CommandService Commands;
        private static CommandHandler commandHandler;

        private static void Main(string[] args)
            => new BotConfig().MainAsync().GetAwaiter().GetResult();
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
            //set the bots log level
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            //instantiate the command service
            Commands = new CommandService();

            //instantiate the command handler with a client and commands
            commandHandler = new CommandHandler(Client, Commands);

            //Load in some environment variables
            AppEnvironment.LoadVariables();

            //attach the bots event handlers
            AttachEventHandlers();

            //tell people what edgy thing the bot is doing
            await Client.SetGameAsync("Rebuilding the USSR");

            //login to discords servers as a bot
            await Client.LoginAsync(TokenType.Bot, AppEnvironment.BotToken);

            //check for available commands
            await commandHandler.InstallCommandsAsync();

            //start the bot
            await Client.StartAsync();

            //wait infinitely I think?
            await Task.Delay(-1);
        }

        /// <summary>
        /// Attaches the required event handlers to the bot
        /// </summary>
        private static void AttachEventHandlers()
        {
            //remove messages that are fucked
            Client.MessageReceived += RestrictedWordService.ScanMessage;

            //responds with f's in chat
            Client.MessageReceived += FInChatService.Respond;

            //check messages for potential quotes
            Client.MessageReceived += QuoteManagamentService.QuoteIntentProcessorAsync;

            //Control how reactions should affect messages
            Client.ReactionAdded += ReactionHandlerService.ReactionAddedAsync;

            // Handle how services that require reactions should respond to the clearing of reactions
            Client.ReactionsCleared += ReactionHandlerService.ReactionsCleared;

            //Generate GuildData once Ready is fired
            Client.Ready += PersistantStorage.GenerateGuildData;

            //Update serverdata when the bot joins a new guild
            Client.JoinedGuild += PersistantStorage.GenerateGuildData;

            //log messages in the console
            Client.Log += Log;
        }


        /// <summary>
        /// Stop the bot and exit the application asynchronously
        /// </summary>
        /// <returns></returns>
        public async static Task StopBotAsync()
        {
            //stop the bot correctly
            await Client.StopAsync();

            //kill the program
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
