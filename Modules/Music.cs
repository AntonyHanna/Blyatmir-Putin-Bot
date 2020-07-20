using System.Threading.Tasks;
using Blyatmir_Putin_Bot.Model;
using Discord.Commands;

namespace Blyatmir_Putin_Bot.Modules
{
	public class Music : ModuleBase<SocketCommandContext>
	{
		[Command("join", RunMode = RunMode.Async)]
		public async Task JoinVoice()
		{
			// check if the guild already has someone in a voice channel
			// if none
			// add to guild channel list and proceed to connect and play songs
			// if not none
			// is it the same channel
			// yes - playe next song
			// no - move channel and next song

			AudioService audioService = AudioService.GetAudioService(Context.Guild);

			if (audioService == null)
				audioService = new AudioService(Context);

			if (audioService.NeedsToConnectToVoiceChannel())
				await audioService.ConnectToVoiceAsync();

			await audioService.StreamToVoiceAsync("zeldo.mp3");



		}

		[Command("disconnect")]
		[Alias(new string[] { "dc", "leave" })]
		public async Task DisconnectFromVoiceChannel()
		{
			AudioService audioService = AudioService.GetAudioService(Context.Guild);

			await audioService.DisconnectFromVoiceAsync();
		}
	}
}
