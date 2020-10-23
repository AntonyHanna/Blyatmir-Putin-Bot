using Renci.SshNet;
using System;

namespace Blyatmir_Putin_Bot.Model
{
	public static class SshController
	{
		internal static bool IsSshEnabled
		{
			get
			{
				if (string.IsNullOrWhiteSpace(BotConfig.AppConfig.DockerIP) || string.IsNullOrWhiteSpace(BotConfig.AppConfig.ServerUser) || string.IsNullOrWhiteSpace(BotConfig.AppConfig.ServerPassword))
					return false;
				return true;
			}
		}
		private static SshClient instance;
		public static SshClient SshClient
		{
			get
			{
				if (instance == null)
				{
					instance = new SshClient(BotConfig.AppConfig.DockerIP, BotConfig.AppConfig.ServerUser, BotConfig.AppConfig.ServerPassword);
					AttachErrorHandler();
					instance.Connect();
				}
				return instance;
			}
		}

		private static void AttachErrorHandler() => SshClient.ErrorOccurred += delegate (object sender, Renci.SshNet.Common.ExceptionEventArgs e) {
			DisconnectFromService();
			Console.WriteLine($"Disconnected the SSH connection due to: {e.Exception.Message}");
		};
		public static void DisconnectFromService()
		{
			SshClient.Disconnect();
			SshClient.Dispose();
		}
	}
}
