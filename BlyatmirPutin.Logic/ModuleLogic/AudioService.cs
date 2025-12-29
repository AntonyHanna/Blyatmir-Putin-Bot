using BlyatmirPutin.Common.Logging;
using NetCord.Gateway;
using NetCord.Gateway.Voice;
using NetCord.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BlyatmirPutin.Logic
{
	public class AudioService
	{
		/// <summary>
		/// Contains all <seealso cref="AudioService"/> instances for all Guilds
		/// </summary>
		private static readonly Dictionary<ulong, AudioService> AudioServices = new Dictionary<ulong, AudioService>();

		/// <summary>
		/// The bot client
		/// </summary>
		private GatewayClient _gatewayClient { get; set; }

		public VoiceState? _voiceState { get; set; }

		public VoiceClient? _voiceClient { get; set; }

		private ulong _guildId { get; set; }

		private AudioService(ulong guildId)
		{
			this._guildId = guildId;
			AudioServices.Add(guildId, this);
		}

		public AudioService(GatewayClient client, VoiceClient voiceClient) : this(voiceClient.GuildId)
		{
			this._gatewayClient = client;
			this._voiceClient = voiceClient;
		}

		public AudioService(GatewayClient client, VoiceState voiceState) : this(voiceState.GuildId)
		{
			this._gatewayClient = client;
			this._voiceState = voiceState;
		}

		public static bool TryGetAudioService(VoiceClient voiceClient, out AudioService? service)
		{
			service = null;
			return AudioServices.TryGetValue(voiceClient.GuildId, out service);
		}

		public static bool TryGetAudioService(VoiceState voiceState, out AudioService? service)
		{
			service = null;
			return AudioServices.TryGetValue(voiceState.GuildId, out service);
		}

		public static Process? CreateFfmpegProcess(string path)
		{
			ProcessStartInfo startInfo = new("ffmpeg")
			{
				RedirectStandardOutput = true,
			};

			// Set reconnect attempts in case of a lost connection to 1
			//startInfo.ArgumentList.Add("-reconnect");
			//startInfo.ArgumentList.Add("1");

			// Set reconnect attempts in case of a lost connection for streamed media to 1
			//startInfo.ArgumentList.Add("-reconnect_streamed");
			//startInfo.ArgumentList.Add("1");

			// Set the maximum delay between reconnection attempts to 5 seconds
			//startInfo.ArgumentList.Add("-reconnect_delay_max");
			//startInfo.ArgumentList.Add("5");

			// Specify the input
			startInfo.ArgumentList.Add("-i");
			startInfo.ArgumentList.Add(path);

			// Set the logging level to quiet mode
			startInfo.ArgumentList.Add("-loglevel");
			startInfo.ArgumentList.Add("panic");

			// Set the number of audio channels to 2 (stereo)
			startInfo.ArgumentList.Add("-ac");
			startInfo.ArgumentList.Add("2");

			// Set the output format to 16-bit signed little-endian
			startInfo.ArgumentList.Add("-f");
			startInfo.ArgumentList.Add("s16le");

			// Set the audio sampling rate to 48 kHz
			startInfo.ArgumentList.Add("-ar");
			startInfo.ArgumentList.Add("48000");

			// Direct the output to stdout
			startInfo.ArgumentList.Add("pipe:1");

			return Process.Start(startInfo);
		}

		public async Task<bool> StreamToVoiceAsync(string fileName)
		{
			Process? ffmpegProcess = CreateFfmpegProcess($"./data/user-intros/{fileName}") ?? null;

			if (ffmpegProcess == null)
			{
				Logger.LogWarning($"Failed to create ffmpeg process for file [{fileName}]");
				return false;
			}

			Stream outStream = this._voiceClient.CreateOutputStream();
			OpusEncodeStream opusStream = new OpusEncodeStream(outStream, PcmFormat.Short, VoiceChannels.Stereo, OpusApplication.Audio);

			await ffmpegProcess.StandardOutput.BaseStream.CopyToAsync(opusStream);
			await opusStream.FlushAsync();

			await opusStream.DisposeAsync();
			await outStream.DisposeAsync();
			
			ffmpegProcess.Close();

			return true;
		}

		public void ReinitializeService(VoiceClient voiceClient)
		{
			this._voiceClient = voiceClient;
			this._voiceState = null;
		}

		public void ReinitializeService(VoiceState voiceState)
		{
			this._voiceState = voiceState;
			this._voiceClient = null;
		}

		public async Task ConnectToChannelAsync()
		{
			if (this._voiceState == null || this._voiceState.ChannelId == null)
			{
				return;
			}

			if (this._voiceClient == null)
			{
				this._voiceClient = await this._gatewayClient.JoinVoiceChannelAsync(this._guildId, this._voiceState.ChannelId.Value, new VoiceClientConfiguration()
				{
					Logger = new ConsoleLogger(LogLevel.Trace)
				});
			}

			await this._voiceClient.StartAsync();
			await this._voiceClient.EnterSpeakingStateAsync(new SpeakingProperties(SpeakingFlags.Microphone));
		}

		public async Task DisconnectFromChannelAsync()
		{
			VoiceStateProperties vsProperties = new VoiceStateProperties(this._guildId, null);
			await this._gatewayClient.UpdateVoiceStateAsync(vsProperties);

			AudioServices.Remove(this._guildId);
		}
	}
}
