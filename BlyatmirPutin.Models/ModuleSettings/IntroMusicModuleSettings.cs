namespace BlyatmirPutin.Models.Modules
{
	/// <summary>
	/// Represent the settings for the Intro Music module in the Database
	/// </summary>
	public class IntroMusicModuleSettings
	{
		/// <summary>
		/// The unique identifier for the Intro Music module in the Database
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// The Discord Guild Id that this entry is associated with
		/// </summary>
		public ulong GuildId { get; set; }

		/// <summary>
		/// Controls whether this module is enabled for this Guild
		/// </summary>
		public bool IsEnabled { get; set; }

		public IntroMusicModuleSettings()
		{
			this.Id = Guid.NewGuid().ToString();
		}

		public override bool Equals(object? obj) 
			=> obj != null && obj is IntroMusicModuleSettings && this.Id == ((IntroMusicModuleSettings)obj).Id;

		public override int GetHashCode() 
			=> this.Id.GetHashCode();
	}
}
