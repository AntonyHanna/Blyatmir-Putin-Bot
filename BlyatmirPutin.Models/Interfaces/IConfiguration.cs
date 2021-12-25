namespace BlyatmirPutin.Models.Interfaces
{
	public interface IConfiguration
	{
		/// <summary>
		/// The Discord API bot token
		/// </summary>
		public string Token { get; set; }

		/// <summary>
		/// The activity to be displayed by the bot
		/// </summary>
		public string Activity { get; set; }
	}
}
