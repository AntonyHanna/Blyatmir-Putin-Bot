using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	public class ContainerManager : ModuleBase<SocketCommandContext>
	{
		private Container _container;
		private User _user;

		[Command("gs")]
		public async Task StartConnection(string function, [Remainder] string containerName)
		{
			if (SshController.IsSshEnabled)
			{
				int result = RunCommand(function, containerName).Result;
				string functionText = default;

				if (result == 1)
				{
					if (function == "start")
						functionText = "started";

					else if (function == "stop")
						functionText = "stopped";

					else if (function == "restart")
						functionText = "restarted";

					await Context.Channel.SendMessageAsync($"The `{containerName}` container has been `{functionText}`");
				}
			}
		}


		/// <summary>
		/// Checks if the container in it's current status can execute the commmand
		/// </summary>
		/// <param name="cont">The container that will be affected by a state changing command</param>
		/// <param name="function">A state changing function</param>
		/// <returns></returns>
		private static bool CanExecuteStateChange(Container cont, string function)
		{
			string state = Container.GetContainerCurrentRunState(cont.ContainerId).Contains("up", System.StringComparison.OrdinalIgnoreCase) ? "running" : "stopped";
			if (state == "running" && function == "start")
				return false;
			if (state == "exited" && function == "stop")
				return false;
			return true;
		}
		private static bool IsValidCommand(string function)
		{
			foreach (var validCommand in new string[] { "start", "stop", "restart" })
			{
				if (function == validCommand)
					return true;
			}
			return false;
		}


		[Command("gs ucp")]
		public async Task UpdateContainerPermission(string containerName, [Remainder] Container.ContainerPermissions permissions)
		{
			if (User.GetUser(Context.User.Id).ContainerAccessLevel == Container.ContainerPermissions.root)
			{
				_container = Container.GetContainerByName(containerName);

				_container.ContainerPermissionLevel = permissions;
				Container.Write(Container.ContainerList);
			}
			await Context.Channel.SendMessageAsync($"Container: `{_container.ContainerName}'s` permissions have been changed to {permissions}");
		}

		[Command("gs uup")]
		public async Task UpdateUserPermission(ulong userId, [Remainder] Container.ContainerPermissions permissions)
		{
			if (User.GetUser(Context.User.Id).ContainerAccessLevel == Container.ContainerPermissions.root)
			{
				_user = User.GetUser(userId);

				if (this._user != null)
				{
					_user.ContainerAccessLevel = permissions;
					User.Write(User.UserList);
				}
				else
				{
					new User(Context.Guild.GetUser(userId));
					this._user = User.GetUser(userId);
					this._user.ContainerAccessLevel = permissions;
					User.Write(User.UserList);
				}

				EmbedBuilder embed = new EmbedBuilder
				{
					Title = "A Users permissions has been updated",
					Color = Color.Green,
					Footer = new EmbedFooterBuilder
					{
						IconUrl = Context.Message.Author.GetAvatarUrl(),
						Text = $"Permission was updated by {Context.Message.Author} at {DateTime.Now}"
					},
					Description = $"User with id: `{_user.UserId}'s` permissions have been changed to `{permissions}`",
				};

				await Context.Channel.SendMessageAsync(embed: embed.Build());
			}
		}

		[Command("gs uup")]
		public async Task UpdateUserPermission(IGuildUser user, [Remainder] Container.ContainerPermissions permissions)
		{
			if (User.GetUser(Context.User.Id).ContainerAccessLevel == Container.ContainerPermissions.root)
			{
				_user = User.GetUser(user.Id);

				if (this._user != null)
				{
					_user.ContainerAccessLevel = permissions;
					User.Write(User.UserList);
				}
				else
				{
					new User(Context.Guild.GetUser(user.Id));
					this._user = User.GetUser(user.Id);
					this._user.ContainerAccessLevel = permissions;
					User.Write(User.UserList);
				}

				EmbedBuilder embed = new EmbedBuilder
				{
					Title = "A Users permissions has been updated",
					Color = Color.Green,
					Footer = new EmbedFooterBuilder
					{
						IconUrl = Context.Message.Author.GetAvatarUrl(),
						Text = $"Permission was updated by {Context.Message.Author} at {DateTime.Now}"
					},
					Description = $"User: `{Context.Guild.GetUser(this._user.UserId)}'s` permissions have been changed to `{permissions}`",
				};

				await Context.Channel.SendMessageAsync(embed: embed.Build());
			}
		}

		private async Task<int> RunCommand(string function, string containerName)
		{
			User.CreateUserIfMissing(Context);
			_user = User.GetUser(Context.Message.Author.Id);

			if (_user != null)
			{
				// will check for users with a access level of user or root
				if ((int)_user.ContainerAccessLevel < 2)
				{
					if (Container.IsValidContainer(containerName))
						_container = Container.GetContainerByName(containerName);
					else
					{
						await Context.Channel.SendMessageAsync($"Oops... Could not find a container with id {_container.ContainerId} on file");
						return 0;
					}

					if (!CanExecuteStateChange(_container, function))
					{
						await Context.Channel.SendMessageAsync($"Cannot run the provided function: `{function}` on the container: `{containerName}` as it is already in that state");
						return 0;
					}

					if (IsValidCommand(function))
						SshController.SshClient.RunCommand($"docker {function} {containerName}");
				}

				else
				{
					await Context.Channel.SendMessageAsync("You don't have sufficient priveleges to access this command");
					return 0;
				}
			}

			else
			{
				await Context.Channel.SendMessageAsync($"Oops... Could not find a player with id {_user.UserId} on file");
				return 0;
			}
			return 1;
		}
	}
}
