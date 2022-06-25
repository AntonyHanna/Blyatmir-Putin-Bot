using BlyatmirPutin.Common.Logging;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlyatmirPutin.Logic
{
	public class AudioService
	{
		/// <summary>
		/// Contains all <seealso cref="AudioService"/> instances for all Guilds
		/// </summary>
		private static readonly List<AudioService> AudioServices = new List<AudioService>();

		/// <summary>
		/// The <seealso cref="SocketGuild"/> that 
		/// this <seealso cref="AudioService"/> is assigned to
		/// </summary>
		public SocketGuild Guild { get; }

		/// <summary>
		/// The ffmpeg instance
		/// </summary>
		private Process? _ffmpeg;

		/// <summary>
		/// The output of Ffmpeg
		/// </summary>
		private Stream? _ffmpegStream;

		/// <summary>
		/// The audio data ouput stream
		/// </summary>
		private AudioOutStream? _outputStream;

		/// <summary>
		/// The voice channel to try to connect to
		/// </summary>
		private IVoiceChannel? _destinationChannel;

		/// <summary>
		/// Whether this instance of <seealso cref="AudioService"/> is in a disposed state
		/// </summary>
		private bool _isDisposed;
		
		public AudioService(SocketCommandContext context)
		{
			this.Guild = context.Guild;
			this._destinationChannel = GetVoiceChannelFromCommandContext(context);

			AudioServices.Add(this);
		}

		public AudioService(SocketInteractionContext context)
		{
			this.Guild = context.Guild;
			this._destinationChannel = GetVoiceChannelFromCommandContext(context);

			AudioServices.Add(this);
		}

		public AudioService(SocketVoiceState socketVoiceState)
		{
			this.Guild = socketVoiceState.VoiceChannel.Guild;
			this._destinationChannel = socketVoiceState.VoiceChannel;

			AudioServices.Add(this);
		}

		public static AudioService GetAudioService(SocketCommandContext context)
		{
			IEnumerable<AudioService> service = AudioServices.Where(a => a.Guild == context.Guild);

			if(!service.Any())
			{
				Logger.LogDebug($"No existing audio service for guild [{context.Guild.Name}], creating a new service");
				return new AudioService(context);
			}

			Logger.LogDebug($"Using existing audio service for guild [{service.First().Guild.Name}]");

			return service.First();
		}

		public static AudioService GetAudioService(SocketInteractionContext context)
		{
			IEnumerable<AudioService> service = AudioServices.Where(a => a.Guild == context.Guild);

			if (!service.Any())
			{
				Logger.LogDebug($"No existing audio service for guild [{context.Guild.Name}], creating a new service");
				return new AudioService(context);
			}

			Logger.LogDebug($"Using existing audio service for guild [{service.First().Guild.Name}]");

			return service.First();
		}

		public static AudioService GetAudioService(SocketVoiceState voiceState)
		{
			IEnumerable<AudioService> service = AudioServices.Where(a => a.Guild == voiceState.VoiceChannel.Guild);

			if (!service.Any())
			{
				Logger.LogDebug($"No existing audio service for guild [{voiceState.VoiceChannel.Guild.Name}], creating a new service");
				return new AudioService(voiceState);
			}

			Logger.LogDebug($"Using existing audio service for guild [{service.First().Guild.Name}]");

			return service.First();
		}

		private static Process? CreateFfmpegProcess(string path)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo
			{
				FileName = "ffmpeg",
				Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
				UseShellExecute = false,
				RedirectStandardOutput = true,
			};

			if(processStartInfo == null)
			{
				Logger.LogError($"Failed to create process from path [{path}]");
				return null;
			}

			return Process.Start(processStartInfo);
		}

		public async Task<bool> StreamToVoiceAsync(string fileName)
		{
			_isDisposed = false;
			Process? ffmpegProcess = CreateFfmpegProcess($"./data/user-intros/{fileName}") ?? null;

			if (ffmpegProcess == null)
			{
				Logger.LogWarning($"Failed to create ffmpeg process for file [{fileName}]");
				return false;
			}

			this._ffmpeg = ffmpegProcess;
			this._ffmpegStream = this._ffmpeg.StandardOutput.BaseStream;

			if (this._destinationChannel == null)
			{
				Logger.LogWarning("Destination channel cannot be null");
				return false;
			}

			IAudioClient? client = null;
			try
			{
				client = await ConnectAsync(this._destinationChannel.Id);
			}
			catch (Exception ex)
			{
				Logger.LogError($"Failed to connect to voice channel\n {ex.Message}");
				return false;
			}

			if (client == null)
			{
				Logger.LogError($"Failed to connect to voice channel [{this._destinationChannel.Name}] in [{this.Guild.Name}]");
				Dispose();
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

		public async Task<IAudioClient?> ConnectAsync(ulong channelID)
		{
			SocketVoiceChannel voiceChannel = this.Guild.VoiceChannels.Where(v => v.Id == channelID).First();
			return await voiceChannel.ConnectAsync() ?? null;
		}

		public async Task<bool> DisconnectAsync()
		{
			try
			{
				if(this._destinationChannel == null)
				{
					Logger.LogWarning("Failed to disconnect from voice channel, bot wasn't in a channel");
					return false;
				}

				await this._destinationChannel.DisconnectAsync();
			}
			catch
			{
				Logger.LogWarning("Failed to disconnect from the voice channel");
				return false;
			}

			return true;
		}

		private IVoiceChannel? GetVoiceChannelFromCommandContext(SocketCommandContext context)
		{
			SocketUser user = context.Message.Author;

			IEnumerable<SocketVoiceChannel> voiceChannels = this.Guild.VoiceChannels.Where(channel => channel.Users.Contains(user));

			if (voiceChannels.Count() > 0)
			{
				return voiceChannels.ElementAt(0);
			}

			return null;
		}

		private IVoiceChannel? GetVoiceChannelFromCommandContext(SocketInteractionContext context)
		{
			SocketUser user = context.User;

			IEnumerable<SocketVoiceChannel> voiceChannels = this.Guild.VoiceChannels.Where(channel => channel.Users.Contains(user));

			if (voiceChannels.Count() > 0)
			{
				return voiceChannels.ElementAt(0);
			}

			return null;
		}

		public void Dispose()
		{
			if(!_isDisposed)
			{
				Logger.LogDebug($"Attempting to dispose of AudioService for guild [{this.Guild.Name}]");
				this._ffmpeg?.Close();
				this._ffmpegStream?.Close();
				this._outputStream?.Close();
			}
			_isDisposed = true;
		}
	}
}
