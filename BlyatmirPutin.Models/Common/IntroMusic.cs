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
		public string Id { get; set; }

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
		public long DateAdded { get; set; }

		public IntroMusic()
		{
			this.Id = Guid.NewGuid().ToString();
			this.DateAdded = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}

		public override bool Equals(object? obj) 
			=> obj != null && obj is IntroMusic && this.Id == (obj as IntroMusic)?.Id;

		public override int GetHashCode()
			=> this.Id.GetHashCode();
	}
}
