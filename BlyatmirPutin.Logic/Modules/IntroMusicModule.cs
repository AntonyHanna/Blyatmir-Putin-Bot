using BlyatmirPutin.Common.Logging;
using BlyatmirPutin.DataAccess.Database;
using BlyatmirPutin.Models.Common;
using BlyatmirPutin.Models.Modules;
using BlyatmirPutin.Models.Records;
using NetCord;
using NetCord.Gateway;
using NetCord.Gateway.Voice;
using NetCord.Logging;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BlyatmirPutin.Logic.Modules
{
	[SlashCommand("intro-music", "All intro music playback commands", Contexts = [InteractionContextType.Guild])]
	public class IntroMusicModule : ApplicationCommandModule<ApplicationCommandContext>
	{
		#region Assignment Commands
		[SubSlashCommand("set", "Set your intro music")]
		public async Task SetNewIntroMusic(Attachment attachment)
		{
			InteractionCallback.DeferredMessage(MessageFlags.Loading);
			if (attachment == null)
			{
				Logger.LogWarning("No attachment was provided, aborting SetNewIntroMusic...");

				return;
			}

			Member author;

			DownloadAttachment(attachment.Url, attachment.FileName);

			// get the specific user we want
			List<Member> members = DatabaseHelper.GetRows<Member>().ToList();

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
				IntroName = attachment.FileName,
				FilePath = $"/data/user-intros/{attachment.FileName}",
			};

			IntroMusicRecord record = new IntroMusicRecord
			{
				UserId = intro.UploaderId,
				IntroId = intro.Id,
				DateSet = intro.DateAdded
			};

			// will always insert new entries into the db
			// regardless of whether the user already has an entry
			DatabaseHelper.Insert(intro);
			DatabaseHelper.Insert(record);

			// link the new intro id with the member
			author.CurrentIntro = intro.Id;

			// update the user entry with their new intro id
			DatabaseHelper.Update(author);
			EmbedProperties embed = new EmbedProperties()
				.WithColor(new Color(0, 128, 128))
				.WithTitle("Woopty freaking doo")
				.WithDescription("Thats great a new intro to keep track of...")
				.WithImage(new EmbedImageProperties("https://media4.giphy.com/media/v1.Y2lkPTc5MGI3NjExeHV1cXptODNhZ2JpcXhlNmF4MzRpdGZ2cXloMGZ0cXNqYXRxMnh4YiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/OHuPcrLnoYLxVFIjKI/giphy.gif"))
				.WithFields(new List<EmbedFieldProperties>()
				{
					new EmbedFieldProperties()
					{
						Name = "New Intro",
						Value = $"You've set your intro to `{intro.IntroName}`"
					}
				})
				.WithFooter(new EmbedFooterProperties()
				{
					Text = "Nothing to see here, just a man getting a handy"
				})
				.WithTimestamp(DateTimeOffset.UtcNow);
				
			InteractionMessageProperties response = new InteractionMessageProperties();

			// create the embed to acknowledge the intro
			await RespondAsync(InteractionCallback.Message(response.WithEmbeds(new List<EmbedProperties>() { embed })));
		}

		[SubSlashCommand("remove", "remove your intro music")]
		public async Task RemoveIntroMusic()
		{
			Member? memberDO = DatabaseHelper.GetRows<Member>()?.Where((m) => m.Id == Context.User.Id)?.First();

			if (memberDO == null)
			{
				Logger.LogWarning("No user was found in the database, aborting RemoveIntroMusic...");
				return;
			}

			memberDO.CurrentIntro = "";

			DatabaseHelper.Update(memberDO);

			EmbedProperties embed = new EmbedProperties()
				.WithColor(new Color(190, 190, 190))
				.WithImage(new EmbedImageProperties("https://c.tenor.com/SOC7ARPKg-gAAAAC/kirby-eat.gif"))
				.WithFooter(new EmbedFooterProperties()
				{
					Text = "Your intro gone... like Carson's career"
				})
				.WithTimestamp(DateTimeOffset.UtcNow);


			// remove all votes against user
			DatabaseHelper.ExecuteRawSql($"DELETE FROM IntroMusicVote WHERE TargetUserID = {memberDO.Id}");

			await RespondAsync(InteractionCallback.Message(new InteractionMessageProperties().WithEmbeds(new List<EmbedProperties>() { embed })));
		}

		[SubSlashCommand("vote-remove", "vote to remove someones intro")]
		public async Task VoteRemoveIntro(GuildUser targetUser)
		{
			EmbedProperties embed;
			IntroMusicVote? voteDO = DatabaseHelper.GetRows<IntroMusicVote>()
				.First((v) => v.VoterID == Context.User.Id);

			if (voteDO != null)
			{
				Logger.LogInfo($"User '{Context.User.Username}' has already voted against user '{targetUser.Username}', ignoring new vote...");

				embed = new EmbedProperties()
					.WithColor(new Color(255, 255, 128))
					.WithAuthor(new EmbedAuthorProperties()
					{
						Name = "Get your hand outta da gad damn cookie jar"
					})
					.WithImage(new EmbedImageProperties("https://media4.giphy.com/media/v1.Y2lkPTc5MGI3NjExeHV1cXptODNhZ2JpcXhlNmF4MzRpdGZ2cXloMGZ0cXNqYXRxMnh4YiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/OHuPcrLnoYLxVFIjKI/giphy.gif"))
					.WithDescription($"Get da fuck outta here, you already voted against {targetUser}")
					.WithFooter(new EmbedFooterProperties()
					{
						Text = $"You voted on {DateTimeOffset.FromUnixTimeSeconds(voteDO.VoteTimestamp)}"
					})
					.WithTimestamp(DateTimeOffset.UtcNow);

				await RespondAsync(InteractionCallback.Message(new InteractionMessageProperties().WithEmbeds(new List<EmbedProperties>() { embed })));
				return;
			}

			voteDO = new IntroMusicVote
			{
				TargetUserID = targetUser.Id,
				VoterID = Context.User.Id,
				GuildID = Context.Guild.Id
			};

			DatabaseHelper.Insert(voteDO);

			int? votes = DatabaseHelper.GetRows<IntroMusicVote>()
				?.Count((v) => v.TargetUserID == targetUser.Id);

			IntroMusicModuleSettings? settingsDO = DatabaseHelper.GetById<IntroMusicModuleSettings>(Context.Guild.Id);

			if (settingsDO == null)
			{
				settingsDO = new IntroMusicModuleSettings
				{
					GuildId = Context.Guild.Id,
					IsEnabled = false
				};

				DatabaseHelper.Insert(settingsDO);
			}

			if (settingsDO.VoteThreshold > votes)
			{
				Logger.LogVerbose($"User '{targetUser.Username}(s)' intro is at '{votes}' " +
					$"vote(s) where threshold is '{settingsDO.VoteThreshold}', not removing intro...");

				embed = new EmbedProperties()
					.WithColor(new Color(200, 115, 255))
					.WithTitle("Vote Placed")
					.WithDescription($"Let it be known that thou has forsake thy boi {targetUser}\n\n" +
						$"The votes against {targetUser.Username} have reached {votes} vote(s), " +
						$"{settingsDO.VoteThreshold - votes} more to go..")
					.WithImage(new EmbedImageProperties("https://c.tenor.com/a1QhvJTQf-kAAAAC/fine-this-is-fine.gif"))
					.WithFooter(new EmbedFooterProperties()
					{
						Text = "No hard feelings, but your intro, bad"
					})
					.WithTimestamp(DateTimeOffset.UtcNow);

				await RespondAsync(InteractionCallback.Message(new InteractionMessageProperties().WithEmbeds(new List<EmbedProperties>() { embed })));
				return;
			}

			Member? targetUserDO = DatabaseHelper.GetById<Member>(targetUser.Id);

			if (targetUserDO == null)
			{
				Logger.LogWarning($"No target user DO for user with Id '{targetUser.Id}'");
				return;
			}

			IntroMusic? intro = DatabaseHelper.GetById<IntroMusic>(targetUserDO.CurrentIntro);

			targetUserDO.CurrentIntro = "";

			DatabaseHelper.Update(targetUserDO);

			// remove all votes against user
			DatabaseHelper.ExecuteRawSql($"DELETE FROM IntroMusicVote WHERE TargetUserID = {targetUserDO.Id}");

			embed = new EmbedProperties()
				.WithColor(new Color(115, 255, 160))
				.WithTitle("Democracy Manifest")
				.WithDescription($"{targetUser}'s intro has reached the vote threshold... time to yeet that bitch (┛◉Д◉)┛彡┻━┻")
				.WithImage(new EmbedImageProperties("https://c.tenor.com/RK4tVUAJZ8MAAAAd/ship-sinking-ship.gif"))
				.WithFields(new List<EmbedFieldProperties>()
				{
					new EmbedFieldProperties
					{
						Inline = true,
						Name = "Intro",
						Value = intro?.IntroName
					},
					new EmbedFieldProperties
					{
						Inline = true,
						Name = "State",
						Value = "Yoted"
					}
				})
				.WithFooter(new EmbedFooterProperties()
				{
					Text = "This message was brought to you by the democracy gang"
				})
				.WithTimestamp(DateTimeOffset.UtcNow);

			await RespondAsync(InteractionCallback.Message(new InteractionMessageProperties().WithEmbeds(new List<EmbedProperties>() { embed })));
		}
		#endregion

		#region Service Commands
		[SubSlashCommand("join", "play a users intro, plays yours by default")]
		public async Task PlayIntro(GuildUser? user = null)
		{
			#region Get Module Settings
			IntroMusicModuleSettings? settings = DatabaseHelper.GetRows<IntroMusicModuleSettings>().Find((s) => s.GuildId == Context.Guild.Id);

			if (settings == null)
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

			if (!settings.IsEnabled)
			{
				Logger.LogWarning($"Not playing intro for user [{Context.User.Username}], intro is not enabled for guild [{Context.Guild.Name}]");
				return;
			}
			#endregion

			Logger.LogDebug("Attempting to connect to voice channel");

			if (!Context.Guild.VoiceStates.TryGetValue(Context.User.Id, out VoiceState vState))
			{
				return;
			}

			if (vState.ChannelId == null)
			{
				return;
			}

			if (Context.Guild.VoiceStates.TryGetValue(Context.Client.Id, out VoiceState botVState))
			{
				// if bot connected already, disconnect
				await Context.Client.UpdateVoiceStateAsync(new VoiceStateProperties(Context.Guild.Id, null));
			}

			VoiceClient vClient = await Context.Client.JoinVoiceChannelAsync(Context.Guild.Id, vState.ChannelId.Value, new VoiceClientConfiguration()
			{
				Logger = new ConsoleLogger(LogLevel.Trace)
			});

			await vClient.StartAsync();
			await vClient.EnterSpeakingStateAsync(new SpeakingProperties(SpeakingFlags.Microphone));

			// ensure that the service is not being used and that its resources are collected before the next use
			//audioService.Dispose();

			ulong userId = (user == null) ? Context.User.Id : user.Id;

			Member? member = DatabaseHelper.GetRows<Member>().Where((m) => m.Id == userId).FirstOrDefault();
			IntroMusic? intro = DatabaseHelper.GetRows<IntroMusic>().Where((m) => m.Id == member.CurrentIntro).FirstOrDefault();

			if (member == null)
			{
				await RespondAsync(InteractionCallback.Message($"Nah, your shits straight fucked"));
				return;
			}

			if (intro == null)
			{
				await RespondAsync(InteractionCallback.Message($"You aint got no shoes lieutenant Dan"));
				return;
			}

			Stream outStream = vClient.CreateVoiceStream();
			OpusEncodeStream stream = new OpusEncodeStream(outStream, PcmFormat.Short, VoiceChannels.Stereo, OpusApplication.Audio);
			Process? ffmpeg = AudioService.CreateFfmpegProcess("." + intro.FilePath);

			if (string.IsNullOrEmpty(intro.IntroName))
			{
				Logger.LogWarning("Aborting connect operation, IntroName is either null or empty...");
				return;
			}

			try
			{
				//await audioService.StreamToVoiceAsync(intro.IntroName);
				await RespondAsync(InteractionCallback.Message($"Now playing: {intro.IntroName}"));
				await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream);
				await stream.FlushAsync();

				VoiceStateProperties vcProperties = new VoiceStateProperties(Context.Guild.Id, null);
				await Context.Client.UpdateVoiceStateAsync(vcProperties);
			}
			catch (Exception ex)
			{
				Logger.LogCritical(ex.Message);
				await ModifyResponseAsync((MessageOptions mOptions) =>
				{
					mOptions.Content = $"Failed to play intro: {intro.IntroName}";
				});
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
