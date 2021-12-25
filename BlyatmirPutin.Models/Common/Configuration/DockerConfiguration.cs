using BlyatmirPutin.Models.Interfaces;

namespace BlyatmirPutin.Models.Common.Configuration
{
	/// <summary>
	/// Used when running the bot in a docker environment
	/// </summary>
	public class DockerConfiguration : IConfiguration
	{
		public string Token { get; set; }

		public string Activity { get; set; }

		public static IConfiguration Load()
		{
			DockerConfiguration config = new DockerConfiguration
			{
				Activity = Environment.GetEnvironmentVariable("BOT_ACTIVITY") ?? "",
				Token = Environment.GetEnvironmentVariable("BOT_TOKEN") ?? ""
			};

			return config;
		}
	}
}
