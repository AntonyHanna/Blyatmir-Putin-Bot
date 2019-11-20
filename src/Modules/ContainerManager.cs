using Blyatmir_Putin_Bot.Model;
using Discord.Commands;
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


		/// <summary>
		/// Checks if the container in it's current status can execute the commmand
		/// </summary>
		/// <param name="cont">The container that will be affected by a state changing command</param>
		/// <param name="function">A state changing function</param>
		/// <returns></returns>
		private static int CanExecuteStateChange(Container cont, string function)
		{
			if (Container.GetContainerCurrentRunState(cont.ContainerId) == "running" && function == "start")
				return 1;
			if (Container.GetContainerCurrentRunState(cont.ContainerId) == "exited" && function == "stop")
				return 2;
			if (Container.GetContainerCurrentRunState(cont.ContainerId) == "restarting")
				return 3;
			return 0;
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

				_user.ContainerAccessLevel = permissions;
				User.Write(User.UserList);
			}
			await Context.Channel.SendMessageAsync($"User with id: `{_user.UserId}'s' permissions have been changed to {permissions}");
		}

		private async Task<int> RunCommand(string function, string containerName)
		{
			await User.CreateUserIfMissing(Context);
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

					if (CanExecuteStateChange(_container, function) != 0)
						return 0;

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
