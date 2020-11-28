using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;
using blyatmir_putin.Core.Database;

namespace blyatmir_putin.Core.Models
{
	public class User
	{
		public ulong UserId { get; set; }

		public string IntroSong { get; set; } = "default.mp3";

		private static DataContext DbContext => Startup.context;

		public User()
		{
			this.UserId = default;
		}

		public User(ulong userId)
		{
			this.UserId = userId;

			DbContext.Users.Add(this);
			DbContext.SaveChanges();
		}

		public User(SocketUser user)
		{
			this.UserId = user.Id;

			DbContext.Users.Add(this);
			DbContext.SaveChanges();
		}

		public static bool UserExists(ulong userId)
		{
			foreach (User user in DbContext.Users)
			{
				if (user.UserId == userId)
				{
					return true;
				}
			}
				
			return false;
		}

		public static Task GenerateUsers(ulong userId)
		{
			for (int i = 0; i < DbContext.Users.Count(); i++)
			{
				if (!UserExists(userId))
				{
					new User(userId);
				}
					
			}
				
			return Task.CompletedTask;
		}

		/// <summary>
		/// Get a user object
		/// </summary>
		/// <param name="userId">The ID of the user you're trying to view, will create a new user if no user is found</param>
		/// <returns></returns>
		public static User GetUser(ulong userId)
		{
			User result = DbContext.Users.First(user => user.UserId == userId);

			if (result == null)
				return new User(userId);

			return result;
		}

		internal static void CreateUserIfMissing(SocketCommandContext context)
		{
			if (!UserExists(context.Message.Author.Id))
				new User(context.Message.Author);
		}
	}
}
