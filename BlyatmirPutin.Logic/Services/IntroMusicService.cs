using BlyatmirPutin.Common.Logging;
using BlyatmirPutin.DataAccess.Database;
using BlyatmirPutin.Logic.Events;
using BlyatmirPutin.Models.Common;
using BlyatmirPutin.Models.Modules;
using NetCord.Gateway;
using System.Threading.Tasks;

namespace BlyatmirPutin.Logic.Services
{
	public class IntroMusicService
	{

		public static void PlayIntroMusic(object? sender, CustomVoiceStateUpdateEventArgs voiceEventArgs)
		{
			GatewayClient client = voiceEventArgs.Client;
			VoiceState voiceState = voiceEventArgs.VoiceState;


			Task.Run(async () =>
			{
				// need to store the VoiceState in the AudioService
				// and keep its previous server in ther
				// then compare the current server to the one in audio service

				// Stops this from triggering when a user is a bot
				if (voiceState.User?.IsBot ?? true)
				{
					// logging this just ends up producing confusing logs
					return;
				}
				Member? memberData = null;
				Logger.LogDebug($"Attempting to run Intro Music for [{voiceState.User.Username}]");

				memberData = DatabaseHelper.GetById<Member>(voiceState.User.Id);

				IntroMusic? introMusic = DatabaseHelper.GetById<IntroMusic>(memberData?.CurrentIntro);
				IntroMusicModuleSettings? introMusicModuleSettings = DatabaseHelper
					.GetRows<IntroMusicModuleSettings>()
					.Find((s) => s.GuildId == voiceEventArgs.VoiceState.GuildId);

				if (memberData?.CurrentIntro == null)
				{
					Logger.LogWarning($"Failed to play intro for user '{voiceState.User.Username}', no intro was set.");
					return;
				}

				if (introMusicModuleSettings == null)
				{
					Logger.LogWarning($"Failed to play intro, no corresponding guild settings was found for id '{voiceState.GuildId}'");
					return;
				}

				if (!introMusicModuleSettings.IsEnabled)
				{
					Logger.LogDebug($"Intros are not enabled for guild '{introMusicModuleSettings.GuildId}'");
					return;
				}

				/* Create a new audio service otherwise bot will join same channel repeatedly */
				AudioService? audioService = null;

				if (!AudioService.TryGetAudioService(voiceState, out audioService))
				{
					audioService = new AudioService(client, voiceState);
				}
				else
				{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    audioService.ReinitializeService(voiceState);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }

				await audioService.ConnectToChannelAsync();
				await audioService.StreamToVoiceAsync(introMusic?.IntroName);
				await audioService.DisconnectFromChannelAsync();

				Logger.LogDebug("Intro Music has finished successfully");
			});
		}
	}
}
