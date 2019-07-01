using Blyatmir_Putin_Bot.model;
using Blyatmir_Putin_Bot.services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Blyatmir_Putin_Bot
{
    class BotConfig
    {
        private static void Main(string[] args)
            => new BotConfig().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            await StartBotAsync();
        }

        public static DiscordSocketClient Client;
        public static CommandService Commands;

        private static CommandHandler commandHandler;

        public async static Task StartBotAsync()
        {

            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            Commands = new CommandService();

            commandHandler = new CommandHandler(Client, Commands);
            Env.LoadVariables();

            Client.MessageReceived += RestrictedWordService.ScanMessage;
            Client.MessageReceived += FInChatService.Respond;
            Client.MessageReceived += QuoteManagamentService.QuoteIntentAsync;

            Client.ReactionAdded += ReactionHandlerService.ReactionControllerAsync;

            //Update serverdata when the bot joins a new guild
            Client.JoinedGuild += PersistantStorage.GenerateGuildData;

            //Client.MessageDeleted += MessageDeletedService.MessageDeleted;

            Client.Log += Log;

            await Client.SetGameAsync("Rebuilding the USSR");
            await Client.LoginAsync(TokenType.Bot, Env.BotToken);
            await commandHandler.InstallCommandsAsync();


            await Client.StartAsync();

            PersistantStorage.DelayGuildDataGeneration();

            await Task.Delay(-1);

        }

        /// <summary>
        /// Stop the bot and exit the application asynchronously
        /// </summary>
        /// <returns></returns>
        public async static Task StopBotAsync()
        {
            await Client.StopAsync();
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
