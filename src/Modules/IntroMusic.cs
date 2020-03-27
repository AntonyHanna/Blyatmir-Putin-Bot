using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blyatmir_Putin_Bot.Model;
using Discord;
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
		[Alias("-s")]
		public async Task SetIntroMusic([Remainder] string songName)
		{
			User userData = User.GetUser(Context.Message.Author.Id);
			userData.IntroSong = songName;
			User.Write(User.UserList);

			await Context.Channel.SendMessageAsync($"Intro Music for `{Context.Message.Author.Username}` has been set to `{songName}`");
		}

		[Command("remove")]
		[Alias("-r")]
		public async Task RemoveIntroMusic()
		{
			User userData = User.GetUser(Context.Message.Author.Id);
			userData.IntroSong = null;
			User.Write(User.UserList);

			await Context.Channel.SendMessageAsync($"Intro Music for `{Context.Message.Author.Username}` has been removed");
		}


		[Command("upload")]
		[Alias("-u")]
		public async Task UploadIntroMusic()
		{
			Attachment attachment = Context.Message.Attachments.First();
			if(File.Exists($"{AppEnvironment.ConfigLocation}/resources/introMusic/{attachment.Filename}"))
			{
				// Send Embed and return
				await Context.Channel.SendMessageAsync("File already exists with this name");
				return;
			}

			if(!attachment.Filename.Contains(".mp3"))
			{
				// Send embed and return
				await Context.Channel.SendMessageAsync("File is of the wrong type, I only accept \".mp3\" files because tony is lazy");
				return;
			}
			using(WebClient client = new WebClient())
			{
				client.DownloadFileAsync(new System.Uri(attachment.Url), $"{AppEnvironment.ConfigLocation}/resources/introMusic/{attachment.Filename}");
				await Context.Channel.SendMessageAsync($"File: `{attachment.Filename}` has been uploaded");
			}
		}
	}
}
