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
	public class Video : ModuleBase<SocketCommandContext>
	{
		[Command("download")]
		[Alias("dl")]
		public async Task DownloadVideo([Remainder] string link)
		{
			YouTube youtube = YouTube.Default;
			var video = await youtube.GetVideoAsync(link);

			string filePath = $"{AppEnvironment.ConfigLocation}\\videos\\{video.FullName.Remove(video.FullName.ToCharArray().Length - 14)}.mp4";
			await File.WriteAllBytesAsync( filePath, video.GetBytesAsync().Result);

			EmbedBuilder embed = new EmbedBuilder
			{
				Title = "Download Link Ready",
				Color = Color.Red,
				Description = $"Click [here]({video.Uri}) to be redirected to the download",
				Footer = new EmbedFooterBuilder
				{
					Text = video.FullName.Remove(video.FullName.ToCharArray().Length - 14)
				}
			};

			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}
	}
}
