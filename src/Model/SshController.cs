using Renci.SshNet;
using System;

namespace Blyatmir_Putin_Bot.Model
{
	public static class SshController
	{
		public static SshClient SshClient = new SshClient(AppEnvironment.DockerIP, AppEnvironment.ServerLogin, AppEnvironment.ServerPassword);

		public static int ConnectToService()
		{
			//connect to the server
			//initialise some commands
			try
			{
				SshClient.Connect();

				Console.WriteLine("Successfully Connected to Docker CLI service");
				return 1;
			}

			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return 0;
			}
		}

		public static void DisconnectFromService()
		{
			SshClient.Disconnect();
			SshClient.Dispose();
		}
	}
}
