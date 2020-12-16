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
		public static IEnumerable<AudioService> AudioServices = new List<AudioService>();

		public SocketGuild Guild { get; }

		/// <summary>
		/// Get whether the bot is currently playing anything
		/// </summary>
		public bool IsPlaying { get; private set; }

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
			(AudioServices as List<AudioService>).Add(this);
		}

		public AudioService(SocketVoiceState socketVoiceState)
		{
			this.Guild = socketVoiceState.VoiceChannel.Guild;
			this._destinationChannel = socketVoiceState.VoiceChannel;
			(AudioServices as List<AudioService>).Add(this);
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
				this.IsPlaying = true;
				await this._ffmpegStream.CopyToAsync(this._outputStream);
			}
			finally
			{
				await this._outputStream.FlushAsync();
				await DisconnectAsync();
				this.IsPlaying = false;
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

		public static AudioService GetAudioService(IGuild guild)
		{
			IEnumerable<AudioService> services = AudioServices.Where(service => service.Guild == guild);

			if (services.Count() > 0)
			{
				return services.ElementAt(0);
			}

			return null;
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
	}
}
