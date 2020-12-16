using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace blyatmir_putin.Core.Models
{
	public class AudioService
	{
		public SocketGuild Guild { get; }

		/// <summary>
		/// The ffmpeg instance
		/// </summary>
		private Process _ffmpeg;

		/// <summary>
		/// The output of Ffmpeg
		/// </summary>
		private Stream _ffmpegStream;

		/// <summary>
		/// The audio data ouput stream
		/// </summary>
		private AudioOutStream _outputStream;

		/// <summary>
		/// The voice channel to try to connect to
		/// </summary>
		private IVoiceChannel _destinationChannel;

		public AudioService(SocketCommandContext context)
		{
			this.Guild = context.Guild;
			this._destinationChannel = GetVoiceChannelFromCommandContext(context);
		}

		public AudioService(SocketVoiceState socketVoiceState)
		{
			this.Guild = socketVoiceState.VoiceChannel.Guild;
			this._destinationChannel = socketVoiceState.VoiceChannel;
		}

		private static Process CreateStream(string path)
		{
			return Process.Start(new ProcessStartInfo
			{
				FileName = "ffmpeg",
				Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
				UseShellExecute = false,
				RedirectStandardOutput = true,
			});
		}

		public async Task<bool> StreamToVoiceAsync(string fileName)
		{
			this._ffmpeg = CreateStream($"{Startup.AppConfig.RootDirectory}user-intros/{fileName}");
			this._ffmpegStream = this._ffmpeg.StandardOutput.BaseStream;

			IAudioClient client = await this.ConnectAsync(this._destinationChannel.Id);

			if (client == null)
			{
				Logger.Warning($"Failed to connect to voice channel [{this._destinationChannel.Name}] in [{this.Guild.Name}]");
				return false;
			}

			this._outputStream = client.CreatePCMStream(AudioApplication.Mixed);

			try
			{
				await this._ffmpegStream.CopyToAsync(this._outputStream);
			}
			finally
			{
				await this._outputStream.FlushAsync();
				await DisconnectAsync();
				Dispose();
			}

			return true;
		}

		public async Task<IAudioClient> ConnectAsync(ulong channelID)
		{
			SocketVoiceChannel voiceChannel = this.Guild.VoiceChannels.Where(v => v.Id == channelID).ElementAt(0);

			return (voiceChannel != null) ? await voiceChannel.ConnectAsync() : null;
		}

		public async Task<bool> DisconnectAsync()
		{
			try
			{
				await this._destinationChannel.DisconnectAsync();
			}
			catch
			{
				return false;
			}

			return true;
		}

		private IVoiceChannel GetVoiceChannelFromCommandContext(SocketCommandContext context)
		{
			SocketUser user = context.Message.Author;

			IEnumerable<SocketVoiceChannel> voiceChannels = this.Guild.VoiceChannels.Where(channel => channel.Users.Contains(user));

			if (voiceChannels.Count() > 0)
			{
				return voiceChannels.ElementAt(0);
			}

			return null;
		}

		private void Dispose()
		{
			this._ffmpeg.Close();
			this._ffmpegStream.Close();
			this._outputStream.Close();
		}
	}
}
