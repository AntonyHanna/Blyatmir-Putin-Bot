using System;
using System.ComponentModel.DataAnnotations;

namespace BlyatmirPutin.Models.Records
{
	public class IntroMusicVote
	{
		/// <summary>
		/// The databases unique identifier for this intro music vote
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// The Discord Id of the user who put in the vote
		/// </summary>
		public ulong VoterID { get; set; }

		/// <summary>
		/// The Discord Id of the user this vote is against
		/// </summary>
		public ulong TargetUserID { get; set; }

		/// <summary>
		/// The guild where the vote was placed
		/// </summary>
		public ulong GuildID { get; set; }

		/// <summary>
		/// The timestamp of when the vote was placed
		/// </summary>
		public long VoteTimestamp { get; set; }

		public IntroMusicVote()
		{
			this.Id = Guid.NewGuid().ToString();
			this.VoteTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}

		public override bool Equals(object? obj)
			=> obj != null && obj is IntroMusicVote && this.Id == (obj as IntroMusicVote)?.Id;

		public override int GetHashCode()
			=> this.Id.GetHashCode();
	}
}
