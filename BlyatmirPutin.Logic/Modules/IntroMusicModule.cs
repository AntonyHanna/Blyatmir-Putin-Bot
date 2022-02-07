using BlyatmirPutin.Common.Logging;
using BlyatmirPutin.DataAccess.Database;
using BlyatmirPutin.Models.Common;
using BlyatmirPutin.Models.Modules;
using Discord;
using Discord.Commands;
using System.Net;

namespace BlyatmirPutin.Logic.Modules
{
	[Group("im")]
	public class IntroMusicModule : ModuleBase<SocketCommandContext>
	{
		#region Assignment Commands
		[Command("-s")]
		public async Task SetNewIntroMusic()
		{
			if(Context.Message.Attachments.Count == 0)
			{
				Logger.LogWarning("No attachment was provided, aborting SetNewIntroMusic...");
				return;
			}

			#region Get Module Settings
			IntroMusicModuleSettings? settings = DatabaseHelper.GetRows<IntroMusicModuleSettings>().Find((s) => s.GuildId == Context.Guild.Id);

			if(settings == null)
			{
				Logger.LogInfo($"No IntroMusicModuleSettings found for guild [{Context.Guild.Name}]...");
				settings = new IntroMusicModuleSettings
				{
					GuildId = Context.Guild.Id,
					IsEnabled = false
				};

				DatabaseHelper.Insert(settings);
				Logger.LogInfo("Inserting IntroMusicModuleSettings to Database");
			}

			if(!settings.IsEnabled)
			{
				Logger.LogWarning($"Not playing intro for user [{Context.User.Username}], intro is not enabled for guild [{Context.Guild.Name}]");
				return;
			}
			#endregion

			Member author;

			// grab attachment
			Attachment attachment = Context.Message.Attachments.First();
			DownloadAttachment(attachment.Url, attachment.Filename);

			// get the specific user we want
			List<Member> members = DatabaseHelper.GetRows<Member>().Where(a => a.Id == Context.User.Id).ToList();

			if (!members.Any())
			{
				// create member
				author = new Member
				{
					Id = Context.User.Id
				};

				DatabaseHelper.Insert(author);
			}
			else
			{
				author = members.First();
			}

			// create an intro entry
			IntroMusic intro = new IntroMusic
			{
				UploaderId = Context.User.Id, /* the user who uploaded this particular id */
				IntroName = attachment.Filename,
				FilePath = $"/data/user-intros/{attachment.Filename}",
			};

			IntroMusicRecord record = new IntroMusicRecord
			{
				UserId = intro.UploaderId,
				IntroId = intro.Id,
				DateSet = intro.DateAdded
			};

			DatabaseHelper.Insert(intro);
			DatabaseHelper.Insert(record);

			// link the new intro id with the member
			author.CurrentIntro = intro.Id;

			// update the user entry with their new intro id
			DatabaseHelper.Update(author);

			// create the embed to acknowledge the intro
			await Context.Channel.SendMessageAsync($"FOUND {members.Count} USER(S)");
		}

		[Command("-r")]
		public async Task RemoveIntroMusic()
		{
			Member? member = DatabaseHelper.GetRows<Member>()?.Where((m) => m.Id == Context.User.Id)?.First();

			if(member == null)
			{
				Logger.LogWarning("No user was found in the database, aborting RemoveIntroMusic...");
				return;
			}

			member.CurrentIntro = "";

			DatabaseHelper.Update(member);

			await Context.Channel.SendMessageAsync($"The IntroMusic for {Context.User.Username} has been removed");
		}


		#endregion

		#region Service Commands
		[Command("-j", RunMode = RunMode.Async)]
		public async Task Connect()
		{
			Logger.LogDebug("Attempting to connect to voice channel");
			
			AudioService audioService = AudioService.GetAudioService(Context);

			// ensure that the service is not being used and that its resources are collected before the next use
			audioService.Dispose();

			Member member = DatabaseHelper.GetRows<Member>().Where((m) => m.Id == Context.User.Id).First();
			IntroMusic intro = DatabaseHelper.GetRows<IntroMusic>().Where((m) => m.Id == member.CurrentIntro).First();
			
			if(string.IsNullOrEmpty(intro.IntroName))
			{
				Logger.LogWarning("Aborting connect operation, IntroName is either null or empty...");
				return;
			}

			try
			{
				await audioService.StreamToVoiceAsync(intro.IntroName);
			}
			catch (Exception ex)
			{
				Logger.LogCritical(ex.Message);
			}
		}

		[Command("dc")]
		public async Task Disconnect()
		{
			AudioService audioService = AudioService.GetAudioService(Context);

			try
			{
				await audioService.DisconnectAsync();
				audioService.Dispose();
			}
			catch (Exception ex)
			{
				Logger.LogError($"An error occurred while trying to disconnect from the voice channel in guild [{audioService.Guild.Name}]" +
					$"\n {ex.Message}");
			}
		}
		#endregion

		#region Helper Methods
		private void DownloadAttachment(string uri, string name)
		{
			Logger.LogDebug("Attempting to download attachmenmt from message");
			try
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadFileAsync(new Uri(uri), $"./data/user-intros/{name}");
				}
			}
			catch (Exception ex)
			{
				Logger.LogError($"Failed to download attachment from message, {ex.Message}");
			}


			Logger.LogDebug("Successfully downloaded attachment from message");
		}
		#endregion
	}
}
