using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Core
{
	public class CommandHandler
	{
		private DiscordSocketClient _client;
		private CommandService Commands;

		public CommandHandler(DiscordSocketClient client, CommandService commands)
		{
			this._client = client;
			this.Commands = commands;
		}

		public async Task InstallCommandsAsync()
		{
			//attach the command handler
			_client.MessageReceived += HandleCommandAsync;

			//add modules
			await Commands.AddModulesAsync(
				assembly: Assembly.GetEntryAssembly(),
				services: null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public async Task HandleCommandAsync(SocketMessage message)
		{
			// Don't process the command if it was a system message
			var userMessage = message as SocketUserMessage;

			//dont run if message = null
			if (userMessage == null)
				return;

			// Create a number to track where the prefix ends and the command begins
			int argPos = 0;

			// Determine if the message is a command based on the prefix and make sure no bots trigger commands
			if (!(userMessage.HasStringPrefix(Startup.AppConfig.Prefix, ref argPos) ||
				userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
				userMessage.Author.IsBot)
			{
				return;
			}

			// Create a WebSocket-based command context based on the message
			var context = new SocketCommandContext(_client, userMessage);

			// Execute the command with the command context we just
			// created, along with the service provider for precondition checks.

			// Keep in mind that result does not indicate a return value
			// rather an object stating if the command executed successfully.
			var result = await Commands.ExecuteAsync(
				context: context,
				argPos: argPos,
				services: null);

			// Optionally, we may inform the user if the command fails
			// to be executed; however, this may not always be desired,
			// as it may clog up the request queue should a user spam a
			// command.
#if DEBUG
			if (!result.IsSuccess)
				await context.Channel.SendMessageAsync(result.ErrorReason);
#endif
		}
	}
}
