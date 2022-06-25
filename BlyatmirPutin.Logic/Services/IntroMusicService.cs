using BlyatmirPutin.Common.Logging;
using BlyatmirPutin.DataAccess.Database;
using BlyatmirPutin.Models.Common;
using BlyatmirPutin.Models.Modules;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace BlyatmirPutin.Logic.Services
{
	public class IntroMusicService
	{
		public static Task PlayIntroMusic(SocketUser user, SocketVoiceState previousState, SocketVoiceState newState)
		{
			Task.Run(async () => {
				// Stops this from triggering when a user is muted or deafened
				bool isSameChannel = previousState.VoiceChannel == newState.VoiceChannel;

				if (user.IsBot || isSameChannel || newState.VoiceChannel == null)
				{
					// logging this just ends up producing confusing logs
					return;
				}

				Logger.LogDebug($"Attempting to run Intro Music for [{user.Username}]");

				Member? memberData = DatabaseHelper.GetById<Member>(user.Id);
				IntroMusic? introMusic = DatabaseHelper.GetById<IntroMusic>(memberData?.CurrentIntro);
				IntroMusicModuleSettings? introMusicModuleSettings = DatabaseHelper
					.GetRows<IntroMusicModuleSettings>()
					.Find((s) => s.GuildId == newState.VoiceChannel.Guild.Id);

				if (memberData?.CurrentIntro == null)
				{
					Logger.LogWarning($"Failed to play intro for user '{user.Username}', no intro was set.");
					return;
				}

				if (introMusicModuleSettings == null)
				{
					Logger.LogWarning($"Failed to play intro, no corresponding guild settings was found for id '{newState.VoiceChannel.Guild.Id}'");
					return;
				}

				if (!introMusicModuleSettings.IsEnabled)
				{
					Logger.LogDebug($"Intros are not enabled for guild '{introMusicModuleSettings.GuildId}'");
					return;
				}

				/* Create a new audio service otherwise bot will join same channel repeatedly */
				AudioService audioService = new AudioService(newState);

				if (!await audioService.StreamToVoiceAsync(introMusic?.IntroName))
				{
					Logger.LogWarning($"Failed to connect to voice channel [{newState.VoiceChannel.Name}] in [{newState.VoiceChannel.Guild.Name}]");
					return;
				}

				Logger.LogDebug("Intro Music has finished successfully");
			});

			return Task.CompletedTask;
		}
	}
}
