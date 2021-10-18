using System;
using blyatmir_putin.Core.Interfaces;

namespace blyatmir_putin.Core.Models
{
	public class EnvironmentSettings : IAppSettings
	{
		public string Token { get; set; }

		public string Prefix { get; set; }

		public string RootDirectory { get; set; } = "config/";

		public string Activity { get; set; }

		public EnvironmentSettings()
		{
			this.Token = Environment.GetEnvironmentVariable("BOT_TOKEN");
			this.Prefix = Environment.GetEnvironmentVariable("BOT_PREFIX");
			this.Activity = Environment.GetEnvironmentVariable("BOT_ACTIVITY");
		}
	}
}
