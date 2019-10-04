using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blyatmir_Putin_Bot.Model
{
	public static class SshManager
	{
		public static SshClient SshClient = new SshClient(AppEnvironment.DockerIP, AppEnvironment.ServerLogin, AppEnvironment.ServerPassword);

		public static int ConnectToService()
		{
			//connect to the server
			//initialise some commands
			try
			{
				SshManager.SshClient.Connect();

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
			SshManager.SshClient.Disconnect();
			SshManager.SshClient.Dispose();
		}
	}
}
