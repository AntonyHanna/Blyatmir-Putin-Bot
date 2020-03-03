using Discord.Commands;
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
		public ContainerPermissions ContainerAccessLevel { get; set; } = ContainerPermissions.jack;
		public string IntroSong { get; set; } = "default.mp3";

		public User()
		{
			this.UserId = default;
		}

		public User(ulong userId)
		{
			this.UserId = userId;

			UserList.Add(this);
			Write(UserList);
		}

		public User(SocketUser user)
		{
			this.UserId = user.Id;

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

			for (int i = 0; i < UserList.Count(); i++)
				if (!UserExists(userId))
					new User(userId);


			return Task.CompletedTask;
		}

		/// <summary>
		/// Get a user object
		/// </summary>
		/// <param name="userId">The ID of the user you're trying to view, will create a new user if no user is found</param>
		/// <returns></returns>
		public static User GetUser(ulong userId)
		{
			IEnumerable<User> result = from user in UserList
									   where user.UserId == userId
									   select user;
			if (result.Count() == 0)
				return null;

			return result.First();
		}

		internal static void CreateUserIfMissing(SocketCommandContext context)
		{
			if (!UserExists(context.Message.Author.Id))
				new User(context.Message.Author);
		}

		public static void SetContainerAccessLevel(ulong userId, ContainerPermissions newContainerPermissions)
		{
			GetUser(userId).ContainerAccessLevel = newContainerPermissions;

			Write(UserList);
		}
	}
}
