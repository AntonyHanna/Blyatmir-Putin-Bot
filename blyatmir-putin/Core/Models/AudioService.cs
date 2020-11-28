using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;

namespace blyatmir_putin.Core.Models
{
	public class AudioService
	{
		public static List<AudioService> AudioServices = new List<AudioService>();

		public SocketGuild Guild { get; }
		public Process Ffmpeg { get; private set; }
		public Stream FfmpegOutput { get; private set; }
		public IAudioClient Client { get; private set; }
		public AudioOutStream OutputStream { get; private set; }
		public SocketVoiceChannel VoiceChannel { get; private set; }
		public SocketVoiceState VoiceState { get; private set; }
		private SocketCommandContext Context { get; }
		public bool IsBot => this.Context.Message.Author.IsBot;
		public bool InVoiceChannel => GetContextVoiceChannel() != null;

		public AudioService(SocketCommandContext context)
		{
			this.Guild = context.Guild;
			this.VoiceChannel = null;
			this.Context = context;
			AudioServices.Add(this);
		}

		public AudioService(SocketVoiceState socketVoiceState)
		{
			this.Guild = socketVoiceState.VoiceChannel.Guild;
			this.VoiceChannel = null;
			this.VoiceState = socketVoiceState;
			AudioServices.Add(this);
		}

		public static AudioService GetAudioService(SocketGuild guild)
		{
			IEnumerable<AudioService> result = default;

			try
			{
				result = from services in AudioServices
						 where services.Guild == guild
						 select services;
			}
			catch { return null; }

			if (result.Count() == 0)
				return null;

			return result.First();
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

		public bool NeedsToConnectToVoiceChannel()
		{
			SocketGuild guild = (this.VoiceState.VoiceChannel == null) ? this.Context.Guild : this.VoiceState.VoiceChannel.Guild;
			IEnumerable<SocketVoiceChannel> channel = from vc in guild.VoiceChannels
													  where vc.Users.Contains(Startup.Client.CurrentUser as IGuildUser)
													  select vc;
			if (channel.Count() == 0)
				return true;

			if (channel.First() == this.VoiceChannel)
				return false;

			return true;
		}


		public async Task<bool> ConnectToVoiceAsync()
		{
			SocketVoiceChannel voiceChannel = (this.VoiceState.VoiceChannel == null) ?
				GetContextVoiceChannel() : this.VoiceState.VoiceChannel;

			if (this.VoiceChannel != voiceChannel && voiceChannel != null)
			{
				// For whatever reason this is called twice and causes the 
				// task to cancel, so this check makes sure that by the 
				// second call we dont run connect again
				if(GetBotCurrentVoiceChannel() == null || GetBotCurrentVoiceChannel() != voiceChannel)
					this.Client = await voiceChannel.ConnectAsync();
				
				this.VoiceChannel = voiceChannel;
				return true;
			}

			return false;
		}


		private async Task DisconnectHandler(Exception arg)
		{
			await this.OutputStream.FlushAsync();
			await this.VoiceChannel.DisconnectAsync();
			this.Client.Dispose();
			this.VoiceChannel = null;
		}

		public async Task StreamToVoiceAsync(string fileName)
		{
			this.Ffmpeg = CreateStream($"{Startup.AppConfig.RootDirectory}user-intros/{fileName}");
			this.FfmpegOutput = this.Ffmpeg.StandardOutput.BaseStream;
			this.OutputStream = this.Client.CreatePCMStream(AudioApplication.Mixed);

			try { await this.FfmpegOutput.CopyToAsync(OutputStream); }
			finally { await OutputStream.FlushAsync(); await DisconnectFromVoiceAsync(); }
		}

		public async Task DisconnectFromVoiceAsync()
		{
			AudioServices.Remove(this);
			await this.VoiceChannel.DisconnectAsync();
			this.VoiceChannel = null;

			Destroy();
		}

		private SocketVoiceChannel GetContextVoiceChannel()
		{
			SocketGuild guild = (this.VoiceState.VoiceChannel == null) ? this.Context.Guild : this.VoiceState.VoiceChannel.Guild;

			IEnumerable<SocketVoiceChannel> channel = from vc in guild.VoiceChannels
													  where vc.Users.Contains(Context.Message.Author)
													  select vc;
			if (channel.Count() == 0)
				return null;
			return channel.First();
		}

		/// <summary>
		/// Gets the <seealso cref="SocketVoiceServer"/> of the guild voice channel the user is in
		/// </summary>
		/// <param name="user">The user to look for</param>
		/// <remarks>Use instead of <seealso cref="GetContextVoiceChannel"/> when <see cref="Context"/> is not supplied</remarks>
		/// <returns></returns>
		private SocketVoiceChannel GetBotCurrentVoiceChannel()
		{
			SocketGuild guild = (this.VoiceState.VoiceChannel == null) ? this.Context.Guild : this.VoiceState.VoiceChannel.Guild;
			IEnumerable<SocketVoiceChannel> channel = from vc in guild.VoiceChannels
													  where vc.GetUser(Startup.Client.CurrentUser.Id) != null
													  select vc;
			if (channel.Count() == 0)
				return null;
			return channel.First();
		}

		/// <summary>
		/// Disposes of all streams used for this instance
		/// </summary>
		public void Destroy()
		{
			this.Ffmpeg.Dispose();
			this.FfmpegOutput.Dispose();
			this.Client.Dispose();
			this.OutputStream.Dispose();
		}
	}
}
