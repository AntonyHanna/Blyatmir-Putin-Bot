namespace BlyatmirPutin.Models.Modules
{
	/// <summary>
	/// Represent the settings for the Game Notifier module in the Database
	/// </summary>
	public class GameNotifierModuleSettings
	{
		/// <summary>
		/// The unique identifier for the Game Notifier module in the Database
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The Discord Guild Id that this entry is associated with
		/// </summary>
		public ulong GuildId { get; set; }

		/// <summary>
		/// Controls whether this module is enabled for this Guild
		/// </summary>
		public bool IsEnabled { get; set; }
	}
}
