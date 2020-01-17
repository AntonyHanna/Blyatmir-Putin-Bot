using System.Threading.Tasks;
using Blyatmir_Putin_Bot.Model;
using Discord.Commands;
using Discord.WebSocket;

namespace Blyatmir_Putin_Bot.Services
{
	public class IntroMusicService : ModuleBase<SocketCommandContext>
	{
		// implement a queue of sorts for when multiple people join
		public static Task PlayIntroMusic(SocketUser user, SocketVoiceState previousState, SocketVoiceState newState)
		{
			// Get the users data
			// play the song if specified
			Task.Run(async () => {
				User userData = User.GetUser(user.Id);
				Guild guildData = Guild.GetGuildData(newState.VoiceChannel.Guild);

				if (userData.IntroSong == null || guildData == null || !guildData.EnableIntroMusic)
					return;

					AudioService audioService = AudioService.GetAudioService(newState.VoiceChannel.Guild);

				if (audioService == null)
					audioService = new AudioService(newState);

				await audioService.ConnectToVoiceAsync();
				await audioService.StreamToVoiceAsync(userData.IntroSong);
			});

			return Task.CompletedTask;
		}
	}
}
