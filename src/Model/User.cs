using Discord.WebSocket;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Blyatmir_Putin_Bot.Model.Container;

namespace Blyatmir_Putin_Bot.Model
{
	public class User : PersistantStorage<User>
	{
		public static readonly List<User> UserList = new List<User>(Read());
		public ulong UserId { get; set; }
		public ContainerPermissions ContainerAccessLevel { get; set; }

		public User()
		{
			this.UserId = default;
			this.ContainerAccessLevel = ContainerPermissions.jack;
		}

		public User(ulong userId)
		{
			if (userId != 0)
				this.UserId = userId;

			this.ContainerAccessLevel = ContainerPermissions.jack;

			UserList.Add(this);
			Write(UserList);
		}

		public User(SocketUser user)
		{
			if (user.Id != 0)
				this.UserId = user.Id;

			this.ContainerAccessLevel = ContainerPermissions.jack;

			UserList.Add(this);
			Write(UserList);
		}

		public static bool UserExists(ulong userId)
		{
			foreach (User user in UserList)
				if (user.UserId == userId)
					return true;
			return false;
		}

		public static Task GenerateUsers(ulong userId)
		{
			//Create the config directory if it doesn't exist
			if (!Directory.Exists(AppEnvironment.ConfigLocation))
				Directory.CreateDirectory(AppEnvironment.ConfigLocation);

			if (UserList.Count > 0)
				for (int i = 0; i < UserList.Count(); i++)
					if (!UserExists(userId))
						UserList.Add(new User(userId));


			return Task.CompletedTask;
		}

		/// <summary>
		/// Get a user object
		/// </summary>
		/// <param name="userId">The ID of the user you're trying to view</param>
		/// <returns></returns>
		public static User GetUser(ulong userId)
		{
			// linq query to search the xml for a matching user
			// return null if no user is found
			IEnumerable<User> result = from user in UserList
									   where user.UserId == userId
									   select user;
			return result.First();
		}

		public static void SetContainerAccessLevel(ulong userId, ContainerPermissions newContainerPermissions)
		{
			GetUser(userId).ContainerAccessLevel = newContainerPermissions;

			Write(UserList);
		}
	}
}
