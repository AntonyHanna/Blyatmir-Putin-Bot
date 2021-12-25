namespace BlyatmirPutin.Models.Common
{
	/// <summary>
	/// Represents an Intro Music in the Database
	/// </summary>
	public class IntroMusic
	{
		/// <summary>
		/// The databases unique identifier for intro music
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The Discord ID for the user who uploaded the intro music
		/// </summary>
		public ulong UploaderId { get; set; }

		/// <summary>
		/// The intro's name
		/// </summary>
		public string? IntroName { get; set; }

		/// <summary>
		/// The relative path to the intro
		/// </summary>
		public string? FilePath { get; set; }

		/// <summary>
		/// The date the intro was initially added
		/// </summary>
		public DateTime DateAdded { get; set; }
	}
}
