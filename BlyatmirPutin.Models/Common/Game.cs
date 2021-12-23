namespace BlyatmirPutin.Models.Common
{
	/// <summary>
	/// Represents a Game in the Database
	/// </summary>
	internal class Game
	{
		/// <summary>
		/// The unique identifier for the Game in the Database
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Represents whether the announcment 
		/// for this game has been posted already
		/// </summary>
		public bool WasPosted { get; set; }

		/// <summary>
		/// Represents the games name
		/// </summary>
		public string? Title { get; set; }

		/// <summary>
		/// Represents the games description
		/// </summary>
		public string? Blurb { get; set; }

		/// <summary>
		/// Represents the games developer
		/// </summary>
		public string? Developer { get; set; }

		/// <summary>
		/// Represents the games publisher
		/// </summary>
		public string? Publisher { get; set; }

		/// <summary>
		/// Represents the path to the games banner image
		/// </summary>
		public string? Banner { get; set; }

		/// <summary>
		/// Represents the path to the games poster image
		/// </summary>
		public string? Poster { get; set; }

		/// <summary>
		/// Represents the games sale start date
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Represents the games sale end date
		/// </summary>
		public DateTime EndDate { get; set; }
	}
}
