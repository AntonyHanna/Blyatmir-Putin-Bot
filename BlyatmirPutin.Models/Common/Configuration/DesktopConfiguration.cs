using BlyatmirPutin.Models.Interfaces;

namespace BlyatmirPutin.Models.Common.Configuration
{
	/// <summary>
	/// Used when running the bot on a desktop environment
	/// </summary>
	public class DesktopConfiguration : IConfiguration
	{
		public string Token { get; set; } = " ";

		public string Activity { get; set; } = " ";
	}
}
