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
	[Alias("im")]
	public class IntroMusic : ModuleBase<SocketCommandContext>
	{
		[Command("set")]
		[Alias("-s")]
		public async Task SetIntroMusic()
		{
			Attachment attachment = Context.Message.Attachments.First();

			if(attachment == null)
			{
				await Context.Channel.SendMessageAsync("No file was provided, no changes have been made");
				return;
			}

			if (File.Exists($"{AppEnvironment.ConfigLocation}/resources/introMusic/{attachment.Filename}"))
			{
				await Context.Channel.SendMessageAsync("File already exists with this name");
				return;
			}

			if (attachment.Size > 200000)
			{
				System.Console.WriteLine(attachment.Size);
				await Context.Channel.SendMessageAsync($"File: `{attachment.Filename}` exceeded the 200kb file size limit");
				return;
			}

			if (!attachment.Filename.Contains(".mp3"))
			{
				await Context.Channel.SendMessageAsync("File is of the wrong type, I only accept \".mp3\" files because tony is lazy");
				return;
			}

			User userData = User.GetUser(Context.Message.Author.Id);

			DeleteIntroSong(userData.IntroSong);

			userData.IntroSong = attachment.Filename;
			User.Write(User.UserList);

			DownloadFromLink(attachment.Url, attachment.Filename);
			await Context.Channel.SendMessageAsync($"Intro Music for `{Context.Message.Author.Username}` has been set to `{attachment.Filename}`");
		}

		[Command("remove")]
		[Alias("-r")]
		public async Task RemoveIntroMusic()
		{
			User userData = User.GetUser(Context.Message.Author.Id);
			userData.IntroSong = null;
			User.Write(User.UserList);

			DeleteIntroSong(userData.IntroSong);
			await Context.Channel.SendMessageAsync($"Intro Music for `{Context.Message.Author.Username}` has been removed");
		}

		private void DownloadFromLink(string uri, string name)
		{
			using (WebClient client = new WebClient())
			{
				client.DownloadFileAsync(new System.Uri(uri), $"{AppEnvironment.ConfigLocation}/resources/introMusic/{name}");
			}
		}

		private void DeleteIntroSong(string songName)
		{
			File.Delete($"{AppEnvironment.ConfigLocation}/resources/introMusic/{songName}");
		}
	}
}
