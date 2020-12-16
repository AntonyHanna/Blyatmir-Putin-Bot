using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using blyatmir_putin.Core.Attributes;
using blyatmir_putin.Core.Database;
using blyatmir_putin.Core.Models;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace blyatmir_putin.Modules
{
	[Group("intromusic")]
	[Alias("im")]
	[DirectoryRequired("config/user-intros")]
	public class IntroMusic : ModuleBase<SocketCommandContext>
	{
		private string songDirectory => $"{Startup.AppConfig.RootDirectory}/user-intros/";
		private static DataContext DbContext => Startup.context;

		[Command("set")]
		[Alias("-s")]
		public async Task SetIntroMusic()
		{
			Logger.Debug($"Attempting to set the Intro Music for [{Context.Message.Author.Username}]");
			Attachment attachment = Context.Message.Attachments.First();

			if(attachment == null)
			{
				Logger.Warning("Aborting Intro Setting modificaiton: No file was provided");
				await DisplayMessage("No file was provided, no changes have been made");
				return;
			}

			if (!IsWithinFileSizeLimit(attachment.Size))
			{
				Logger.Warning("Aborting Intro Setting modificaiton: The file provided exceeded the file size limit");
				await DisplayMessage($"File: `{attachment.Filename}` exceeded the 300kb file size limit");
				return;
			}

			if (!IsMp3(attachment.Filename))
			{
				Logger.Warning("Aborting Intro Setting modificaiton: File provided has the wrong extension");
				await DisplayMessage("File is of the wrong type, I only accept \".mp3\" files because tony is lazy");
				return;
			}

			User userData = User.GetUser(Context.Message.Author.Id);
			string safeFileName = GenerateSafeFileName(attachment.Filename);

			userData.IntroSong = safeFileName;
			await DbContext.SaveChangesAsync();

			DownloadAttachment(attachment.Url, safeFileName);

			await DisplayMessage($"Intro Music for `{Context.Message.Author.Username}` has been set to `{safeFileName}`");
			Logger.Debug($"Intro Music has been successfully changed for [{Context.Message.Author.Username}]");
		}

		[Command("remove")]
		[Alias("-r")]
		public async Task RemoveIntroMusic()
		{
			Logger.Debug($"Attempting to remove Intro Music for [{Context.Message.Author.Username}]");
			User userData = User.GetUser(Context.Message.Author.Id);
			userData.IntroSong = null;
			await DbContext.SaveChangesAsync();

			DeleteIntroSong(userData.IntroSong);
			await Context.Channel.SendMessageAsync($"Intro Music for `{Context.Message.Author.Username}` has been removed");
			Logger.Debug($"Intro Music has been successfully removed for [{Context.Message.Author.Username}]");
		}

		[Command("join", RunMode = RunMode.Async)]
		[Alias("-j")]
		public async Task TriggerIntroMusic(SocketUser user = null)
		{
			Logger.Debug($"Triggering Intro Music in [{Context.Message.Channel.Name}] @ [{Context.Guild.Name}]");
		
			if(user == null)
			{
				Logger.Debug("No user passed to command, assuming author is the user");
				user = Context.Message.Author;
			}

			User userData = User.GetUser(user.Id);
			AudioService audioService = new AudioService(Context);

			await audioService.StreamToVoiceAsync(userData.IntroSong);
		}

		[Command("default")]
		[Alias("-d")]
		[RequireBotPermission(GuildPermission.Administrator)]
		internal async Task SetUserIntroToDefault(SocketUser user)
		{
			Logger.Debug($"Attempting to set Intro Music for [{user.Username}] to default value");
			await SetSongToDefault(user);
			Logger.Debug($"Intro Music for [{user.Username}] has successfully been set to default");
		}

		private async Task DisplayMessage(string message)
		{
			await Context.Channel.SendMessageAsync(message);
		}
	
		private static bool IsWithinFileSizeLimit(int fileSize, int maxSize = 307200)
		{
			if (fileSize > maxSize) return false;
			return true;
		}

		private static bool IsMp3(string fileName)
		{
			// get the last 4 chars of file name (should be extension)
			string fileFormat = fileName.Substring(fileName.Length - 4, 4);

			if (fileFormat == ".mp3") return true;
			return false;
		}

		private bool FileExists(string fileName)
		{
			if (File.Exists($"{songDirectory}{fileName}")) return true;
			return false;
		}

		private string AppendIdToName(string originalName)
		{
			int i = 1;
			// 4 is used to remove .mp3 from fileName
			string newName = originalName.Substring(0, originalName.Length - 4) + $"({i}).mp3";

			while (FileExists(newName))
			{
				// 7 is used to remove the end of fileName (#).mp3
				newName = originalName.Substring(0, originalName.Length - 7) + $"({i}).mp3";
				i++;
			}

			return newName;
		}

		private string GenerateSafeFileName(string fileName)
		{
			string result = fileName;

			if (FileExists(fileName)) result = AppendIdToName(fileName);

			return result;
		}

		private void DownloadAttachment(string uri, string name)
		{
			Logger.Debug("Downloading attachmenmt from message");
			using (WebClient client = new WebClient())
			{
				client.DownloadFileAsync(new System.Uri(uri), $"{songDirectory}{name}");
			}

			Logger.Debug("Successfully downloaded attachment from message");
		}

		private void DeleteIntroSong(string songName)
		{
			Logger.Debug($"Attempting to delete the Intro Song file [{songName}]");
			File.Delete($"{songDirectory}{songName}");
			Logger.Debug($"Intro Music has been successfully deleted");
		}
		
		// this will probably end up being an issue since it'll allow users to default others intromusic
		// the only solution is to how a baked in permissions system
		private async Task SetSongToDefault(SocketUser user)
		{
			User userInfo = User.GetUser(user.Id);
			userInfo.IntroSong = "default.mp3";

			await DbContext.SaveChangesAsync();

			await Context.Channel.SendMessageAsync($"Intro Music for User: `{user.Username}` has been set to the default");
		}
	}
}
