using BlyatmirPutin.Common.Logging;
using BlyatmirPutin.DataAccess.Database;
using BlyatmirPutin.Models.Common;
using BlyatmirPutin.Models.Modules;
using BlyatmirPutin.Models.Records;
using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BlyatmirPutin.Logic.Modules
{
	[Group("intro-music", "All intro music playback commands")]
	public class IntroMusicModule : InteractionModuleBase<SocketInteractionContext>
	{
		#region Assignment Commands
		[SlashCommand("set", "Set your intro music")]
		public async Task SetNewIntroMusic(Attachment attachment)
		{
			if (attachment == null)
			{
				Logger.LogWarning("No attachment was provided, aborting SetNewIntroMusic...");

				return;
			}

			Member author;

			DownloadAttachment(attachment.Url, attachment.Filename);

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
				IntroName = attachment.Filename,
				FilePath = $"/data/user-intros/{attachment.Filename}",
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

			// create the embed to acknowledge the intro
			EmbedBuilder introSetEmbed = new EmbedBuilder
			{
				Color = Color.Teal,
				Title = "Woopty freakin doo",
				Description = "Thats great a new intro to keep track off...",
				ImageUrl = "https://c.tenor.com/9jf-DDWOCI4AAAAC/runover-kid.gif",
				Fields = new List<EmbedFieldBuilder>
			{
				new EmbedFieldBuilder
				{
					Name = "New Intro",
					Value = $"You've set you're intro to `{intro.IntroName}`"
				}
			},
				Footer = new EmbedFooterBuilder
				{
					Text = "Nothing to see here, just some casual manslaughter"
				},
				Timestamp = DateTimeOffset.UtcNow
			};

			await RespondAsync(embed: introSetEmbed.Build());
		}

		[SlashCommand("remove", "remove your intro music")]
		public async Task RemoveIntroMusic()
		{
			Member? memberDO = DatabaseHelper.GetRows<Member>()?.Where((m) => m.Id == Context.User.Id)?.First();

			if(memberDO == null)
			{
				Logger.LogWarning("No user was found in the database, aborting RemoveIntroMusic...");
				return;
			}

			memberDO.CurrentIntro = "";

			DatabaseHelper.Update(memberDO);

			EmbedBuilder introRemovedEmbed = new EmbedBuilder
			{
				Color = Color.LighterGrey,
				ImageUrl = "https://c.tenor.com/SOC7ARPKg-gAAAAC/kirby-eat.gif",
				Footer = new EmbedFooterBuilder
				{
					Text = "Your intro gone... like Carson's career"
				},
				Timestamp = DateTimeOffset.UtcNow
			};

			// remove all votes against user
			DatabaseHelper.ExecuteRawSql($"DELETE FROM IntroMusicVote WHERE TargetUserID = {memberDO.Id}");

			await RespondAsync(embed: introRemovedEmbed.Build());
		}

		[SlashCommand("vote-remove", "vote to remove someones intro")]
		public async Task VoteRemoveIntro(IGuildUser targetUser)
		{
			IntroMusicVote? voteDO = DatabaseHelper.GetRows<IntroMusicVote>()
				.First((v) => v.VoterID == Context.User.Id);

			if (voteDO != null)
			{
				Logger.LogInfo($"User '{Context.User.Username}' has already voted against user '{targetUser.Username}', ignoring new vote...");

				EmbedBuilder existingVoteEmbed = new EmbedBuilder
				{
					Color = Color.Gold,
					Author = new EmbedAuthorBuilder
					{
						Name = "Get your hand outta da gad damn cookie jar"
					},
					ImageUrl = "https://c.tenor.com/nNdmUUvMB5AAAAAd/gtfo-john-wayne.gif",
					Description = $"Get da fuck outta here, you already voted against {targetUser.Mention}",
					Footer = new EmbedFooterBuilder
					{
						Text = $"You voted on {DateTimeOffset.FromUnixTimeSeconds(voteDO.VoteTimestamp)}"
					},
					Timestamp = DateTimeOffset.UtcNow
				};

				await RespondAsync(embed: existingVoteEmbed.Build());
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

			if(settingsDO == null)
			{
				settingsDO = new IntroMusicModuleSettings
				{
					GuildId = Context.Guild.Id,
					IsEnabled = false
				};

				DatabaseHelper.Insert(settingsDO);
			}

			if(settingsDO.VoteThreshold > votes)
			{
				Logger.LogVerbose($"User '{targetUser.Username}(s)' intro is at '{votes}' " +
					$"vote(s) where threshold is '{settingsDO.VoteThreshold}', not removing intro...");

				EmbedBuilder votePlacedEmbed = new EmbedBuilder
				{
					Color = Color.Purple,
					Title = "Vote Placed",
					Description = $"Let it be known that thou has forsake thy boi {targetUser.Mention}\n\n" +
					$"The votes against {targetUser.Username} have reached {votes} vote(s), " +
					$"{settingsDO.VoteThreshold - votes} more to go..",
					ImageUrl = "https://c.tenor.com/a1QhvJTQf-kAAAAC/fine-this-is-fine.gif",
					Footer = new EmbedFooterBuilder
					{
						Text = "No hard feelings, but your intro, bad"
					},
					Timestamp = DateTimeOffset.UtcNow
				};

				await RespondAsync(embed: votePlacedEmbed.Build());
				return;
			}

			Member? targetUserDO = DatabaseHelper.GetById<Member>(targetUser.Id);

			if(targetUserDO == null)
			{
				Logger.LogWarning($"No target user DO for user with Id '{targetUser.Id}'");
				return;
			}

			IntroMusic? intro = DatabaseHelper.GetById<IntroMusic>(targetUserDO.CurrentIntro);

			targetUserDO.CurrentIntro = "";

			DatabaseHelper.Update(targetUserDO);

			// remove all votes against user
			DatabaseHelper.ExecuteRawSql($"DELETE FROM IntroMusicVote WHERE TargetUserID = {targetUserDO.Id}");

			EmbedBuilder successEmbed = new EmbedBuilder
			{
				Color = Color.Green,
				Title = "Democracy Manifest",
				Description = $"{targetUser.Mention}'s intro has reached the vote threshold... time to yeet that bitch (┛◉Д◉)┛彡┻━┻",
				Fields = new List<EmbedFieldBuilder>
				{
					new EmbedFieldBuilder
					{
						IsInline = true,
						Name = "Intro",
						Value = intro?.IntroName
					},
					new EmbedFieldBuilder
					{
						IsInline = true,
						Name = "State",
						Value = "Yoted"
					}
				},
				ImageUrl = "https://c.tenor.com/RK4tVUAJZ8MAAAAd/ship-sinking-ship.gif",
				Footer = new EmbedFooterBuilder
				{ 
					Text = "This message was brought to you by the democracy gang"
				},
				Timestamp = DateTimeOffset.UtcNow
			};

			await RespondAsync(embed: successEmbed.Build());
		}
		#endregion

		#region Service Commands
		[SlashCommand("join", "play a users intro, plays yours by default", false, RunMode.Async)]
		public async Task PlayIntro(IGuildUser? user = null)
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

			AudioService audioService = AudioService.GetAudioService(Context);

			// ensure that the service is not being used and that its resources are collected before the next use
			audioService.Dispose();

			ulong userId = (user == null) ? Context.User.Id : user.Id;

			Member member = DatabaseHelper.GetRows<Member>().Where((m) => m.Id == userId).First();
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

			await RespondAsync($"Now playing '{intro.IntroName}'...");
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
