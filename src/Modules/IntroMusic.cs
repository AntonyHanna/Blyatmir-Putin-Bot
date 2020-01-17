using System.Threading.Tasks;
using Blyatmir_Putin_Bot.Model;
using Discord.Commands;

namespace Blyatmir_Putin_Bot.Modules
{
	[Group("intromusic")]
	public class IntroMusic : ModuleBase<SocketCommandContext>
	{
		// Should contain commands for configuring things about the IntoMusicService such as:
		// enabling and selecting song on user basis
		// 
		[Command("set")]
		public async Task SetIntroMusic([Remainder] string songName)
		{
			User userData = User.GetUser(Context.Message.Author.Id);
			userData.IntroSong = songName;
			User.Write(User.UserList);

			await Context.Channel.SendMessageAsync($"Intro Music for `{Context.Message.Author.Username}` has been set to `{songName}`");
		}

		[Command("remove")]
		public async Task RemoveIntroMusic()
		{
			User userData = User.GetUser(Context.Message.Author.Id);
			userData.IntroSong = null;
			User.Write(User.UserList);

			await Context.Channel.SendMessageAsync($"Intro Music for `{Context.Message.Author.Username}` has been removed");
		}
	}
}
