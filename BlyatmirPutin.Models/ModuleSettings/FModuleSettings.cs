using System.ComponentModel.DataAnnotations;

namespace BlyatmirPutin.Models.Modules
{
	/// <summary>
	/// Represent the settings for the F module in the Database
	/// </summary>
	public class FModuleSettings
	{
		/// <summary>
		/// The unique identifier for the F module in the Database
		/// </summary>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// The Discord Guild Id that this entry is associated with
		/// </summary>
		public ulong GuildId { get; set; }

		/// <summary>
		/// Represents the threshold required for a F to be posted
		/// </summary>
		public int Threshold { get; set; }

		/// <summary>
		/// The cooldown between when a F message is posted 
		/// and when the next can be posted
		/// </summary>
		public int Cooldown { get; set; }

		/// <summary>
		/// Controls whether this module is enabled for this Guild
		/// </summary>
		public bool IsEnabled { get; set; }
	}
}
