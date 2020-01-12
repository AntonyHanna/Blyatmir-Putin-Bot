using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using VideoLibrary;

namespace Blyatmir_Putin_Bot.Modules
{
	[Group("video")]
	public class Video : ModuleBase<SocketCommandContext>
	{
		[Command("download")]
		[Alias("dl")]
		public async Task DownloadVideo([Remainder] string link)
		{
			YouTube youtube = YouTube.Default;
			var video = await youtube.GetVideoAsync(link);

			string filePath = $"{AppEnvironment.ConfigLocation}\\videos\\{video.FullName}";
			await File.WriteAllBytesAsync( filePath, video.GetBytesAsync().Result);

			EmbedBuilder embed = new EmbedBuilder
			{
				Title = "Video has been downloaded",
				Color = Color.Red,
				Description = video.FullName,
				Url = video.Uri
			};

			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}
	}
}
