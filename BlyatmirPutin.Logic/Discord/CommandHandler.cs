using BlyatmirPutin.Logic.Factories;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;

namespace BlyatmirPutin.Logic.Discord
{
	internal class CommandHandler
	{
		private DiscordSocketClient Client { get; set; }

		private CommandService Commands { get; set; }

        private InteractionService Interactions { get; set; }

		public CommandHandler(DiscordSocketClient client, CommandService commands, InteractionService interactions)
		{
            this.Client = client;
            this.Commands = commands;
            this.Interactions = interactions;
		}

        public async Task InstallCommandsAsync()
		{
            Client.MessageReceived += HandleCommandAsync;

            Assembly assembly = Assembly.GetExecutingAssembly();

            await Commands.AddModulesAsync(
                assembly: assembly,
                services: ServiceFactory.ServiceProvider
            );
            await Interactions.AddModulesAsync(
                    assembly: assembly,
                    services: ServiceFactory.ServiceProvider
            );

            Client.InteractionCreated += HandleInteractionAsync;
        }

		private async Task HandleInteractionAsync(SocketInteraction interaction)
		{
            SocketInteractionContext context = new SocketInteractionContext(Client, interaction);
            await Interactions.ExecuteCommandAsync(context, ServiceFactory.ServiceProvider);
        }

		public async Task HandleCommandAsync(SocketMessage arg)
        {
            // Bail out if it's a System Message.
            var msg = arg as SocketUserMessage;
            if (msg == null) return;

            // We don't want the bot to respond to itself or other bots.
            if (msg.Author.Id == Client?.CurrentUser.Id || msg.Author.IsBot) return;

            // Create a number to track where the prefix ends and the command begins
            int pos = 0;

            // get the guild based prefix
            if (msg.HasCharPrefix('/', ref pos))
            {
                // Create a Command Context.
                var context = new SocketCommandContext(Client, msg);

                // Execute the command. (result does not indicate a return value, 
                // rather an object stating if the command executed successfully).
                var result = await Commands.ExecuteAsync(context: context, argPos: pos, null);

#if DEBUG
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
					await msg.Channel.SendMessageAsync(result.ErrorReason);
#endif
            }
        }
    }
}
