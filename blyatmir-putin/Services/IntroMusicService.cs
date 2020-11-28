using System.Threading.Tasks;
using blyatmir_putin.Core.Models;
using Discord.Commands;
using Discord.WebSocket;

namespace blyatmir_putin.Services
{
	public class IntroMusicService : ModuleBase<SocketCommandContext>
	{
		// implement a queue of sorts for when multiple people join
		public static Task PlayIntroMusic(SocketUser user, SocketVoiceState previousState, SocketVoiceState newState)
		{
			Task.Run(async () => {
				Logger.Debug($"Attempting to run Intro Music for [{user.Username}]");

				// Stops this from triggering when a user is muted or deafened
				bool isSameChannel = previousState.VoiceChannel == newState.VoiceChannel;

				if (user.IsBot)
				{
					Logger.Warning("Aborting Intro Music:\tUser is actually a bot :o");
					return;
				}

				if(isSameChannel)
				{
					Logger.Warning("Aborting Intro Music:\tUser didn't change channels");
					return;
				}

				if(newState.VoiceChannel == null)
				{
					Logger.Warning("Aborting Intro Music:\tUser is no longer in a voice channel");
					return;
				}
					

				User userData = User.GetUser(user.Id);
				Guild guildData = Guild.GetGuildData(newState.VoiceChannel.Guild);

				if (userData.IntroSong == null)
				{
					Logger.Warning("Aborting Intro Music:\tUser has not set an intro song");
					return;
				}

				if(guildData == null)
				{
					Logger.Warning($"Aborting Intro Music:\tNo guild found with id: [{newState.VoiceChannel.Guild.Id}]");
					return;
				}

				if(!guildData.EnableIntroMusic)
				{
					Logger.Warning("Aborting Intro Music:\tIntro Music is not enabled for this guild");
					return;
				}
					

					AudioService audioService = AudioService.GetAudioService(newState.VoiceChannel.Guild);

				if (audioService == null)
				{
					Logger.Debug($"No Audio Service found for [{newState.VoiceChannel.Guild.Name}], now creating a new Audio Service...");
					audioService = new AudioService(newState);
				}
					

				bool result = await audioService.ConnectToVoiceAsync();
				
				if(!result)
				{
					Logger.Warning($"Failed to connect to voice channel [{newState.VoiceChannel.Name}] in [{newState.VoiceChannel.Guild.Name}] now aborting Intro Music...");
					return;
				}

				await audioService.StreamToVoiceAsync(userData.IntroSong);
				Logger.Debug("Intro Music has finished successfully");
			});

			return Task.CompletedTask;
		}
	}
}
