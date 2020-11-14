using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Blyatmir_Putin_Bot.Model
{
	public class EnvironmentSettings : IAppSettings
	{
		public string Token { get; set; }

		public string Prefix { get; set; }

		public string RootDirectory { get; set; } = "config/";

		public string Activity { get; set; }

		public string DockerIP { get; set; }

		public string ServerUser { get; set; }

		public string ServerPassword { get; set; }

		public EnvironmentSettings()
		{
			if(!Directory.Exists(this.RootDirectory))
			{
				Logger.Warning("No Settings file found. Attempting to create the file.");
				Directory.CreateDirectory(this.RootDirectory);
				Logger.Warning("Settings file has been created.");
			}

			this.Token = Environment.GetEnvironmentVariable("BOT_TOKEN");
			this.Prefix = Environment.GetEnvironmentVariable("BOT_PREFIX");
			this.Activity = Environment.GetEnvironmentVariable("BOT_ACTIVITY");
			this.DockerIP = Environment.GetEnvironmentVariable("DOCKER_IP");
			this.ServerUser = Environment.GetEnvironmentVariable("SERVER_LOGIN");
			this.ServerPassword = Environment.GetEnvironmentVariable("SERVER_PASSWORD");
		}
	}
}
