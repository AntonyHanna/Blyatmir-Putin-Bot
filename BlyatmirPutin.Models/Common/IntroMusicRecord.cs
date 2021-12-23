namespace BlyatmirPutin.Models.Common
{
	/// <summary>
	/// Represents a users past Intro Music in the Database
	/// </summary>
	internal class IntroMusicRecord
	{
		/// <summary>
		/// The databases unique identifier for intro music
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Represents the user who was using the intro music
		/// </summary>
		public ulong UserId { get; set; }

		/// <summary>
		/// Represents which intro was used
		/// </summary>
		public int IntroId { get; set; }

		/// <summary>
		/// Represents the date time when the user started
		/// using this intro
		/// </summary>
		public DateTime DateSet { get; set; }

		/// <summary>
		/// Represents the date time when the user stopped
		/// using this intro
		/// </summary>
		public DateTime DateUnset { get; set; }
	}
}
