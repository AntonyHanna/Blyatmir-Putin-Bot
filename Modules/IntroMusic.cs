using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Blyatmir_Putin_Bot.Modules
{
	[Group("intromusic")]
	[Alias("im")]
	public class IntroMusic : ModuleBase<SocketCommandContext>
	{
		private string songDirectory => $"{AppEnvironment.ConfigLocation}/resources/introMusic/";

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

			string preparedFileName = appendFileNameIfExists(attachment.Filename);

			DeleteIntroSong(userData.IntroSong);

			userData.IntroSong = preparedFileName;
			User.Write(User.UserList);

			DownloadFromLink(attachment.Url, preparedFileName);

			await Context.Channel.SendMessageAsync($"Intro Music for `{Context.Message.Author.Username}` has been set to `{preparedFileName}`");
		}

		private bool FileExists(string fileName)
		{
			if (File.Exists($"{songDirectory}{fileName}")) return true;
			return false;
		}

		private string appendIdToName(string originalName)
		{
			int i = 1;
			string newName = originalName.Substring(0, originalName.Length - 4) + $"({i}).mp3";

			while (FileExists(newName))
			{
				newName = originalName.Substring(0, originalName.Length - 7) + $"({i}).mp3";
				i++;
			}

			return newName;
		}

		private string appendFileNameIfExists(string fileName)
		{
			string result = fileName;

			if (FileExists(fileName)) result = appendIdToName(fileName);

			return result;
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
				client.DownloadFileAsync(new System.Uri(uri), $"{songDirectory}{name}");
			}
		}

		private void DeleteIntroSong(string songName)
		{
			File.Delete($"{songDirectory}{songName}");
		}

		[Command("default")]
		[Alias("-d")]
		[RequireBotPermission(GuildPermission.Administrator)]
		internal void SetUserIntroToDefault(SocketUser user) 
			=> SetSongToDefault(user);

		[Command("default")]
		[Alias("-d")]
		[RequireBotPermission(GuildPermission.Administrator)]
		private void SetUserIntroToDefault() 
			=> SetSongToDefault(Context.User);
		
		// this will probably end up being an issue since it'll allow users to default others intromusic
		// the only solution is to how a baked in permissions system
		private void SetSongToDefault(SocketUser user)
		{
			User userInfo = User.GetUser(user.Id);
			userInfo.IntroSong = "default.mp3";
			User.Write(User.UserList);

			Context.Channel.SendMessageAsync($"Intro Music for User: `{user.Username}` has been set to the default");
		}
	}
}
