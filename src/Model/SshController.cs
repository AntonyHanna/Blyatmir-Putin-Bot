using Renci.SshNet;
using System;

namespace Blyatmir_Putin_Bot.Model
{
	public static class SshController
	{
		private static SshClient instance;
		public static SshClient SshClient
		{
			get
			{
				if(instance == null)
				{
					instance = new SshClient(AppEnvironment.DockerIP, AppEnvironment.ServerLogin, "");
					instance.Connect();
				}
				return instance;
			}
		}

		public static void DisconnectFromService()
		{
			SshClient.Disconnect();
			SshClient.Dispose();
		}
	}
}
