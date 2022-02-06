namespace BlyatmirPutin.Models.Common
{
	/// <summary>
	/// Represents a users past Intro Music in the Database
	/// </summary>
	public class IntroMusicRecord
	{
		/// <summary>
		/// The databases unique identifier for intro music
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Represents the user who was using the intro music
		/// </summary>
		public ulong UserId { get; set; }

		/// <summary>
		/// Represents which intro was used
		/// </summary>
		public string IntroId { get; set; }

		/// <summary>
		/// Represents the date time when the user started
		/// using this intro
		/// </summary>
		public long DateSet { get; set; }

		public IntroMusicRecord()
		{
			this.Id = Guid.NewGuid().ToString();
		}

		public override bool Equals(object? obj)
			=> obj != null && obj is IntroMusicRecord && this.Id == (obj as IntroMusicRecord).Id;

		public override int GetHashCode() 
			=> this.Id.GetHashCode();
	}
}
