using Discord.Commands;
using Renci.SshNet;
using System;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
    public class ManageGames : ModuleBase<SocketCommandContext>
    {
        private static SshClient SshClient { get; set; }

        [Command("gs")]
        public async Task StartConnection(string function, [Remainder] string containerName)
        {
            ConnectToService();
            int result = RunCommand(function, containerName);
            DisconnectFromService();

            string functionText = default;

            if (result == 1)
            {
                if (function == "start")
                    functionText = "started";

                else if (function == "stop")
                    functionText = "stopped";

                else if (function == "restart")
                    functionText = "restarted"; 

                await Context.Channel.SendMessageAsync($"The `{containerName}` game server has been `{functionText}`");
            }
        }


        private static int ConnectToService()
        {
            //connect to the server
            //initialise some commands
            try
            {
                SshClient = new SshClient(AppEnvironment.DockerIP, AppEnvironment.ServerLogin, AppEnvironment.ServerPassword);
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

        private static void DisconnectFromService()
        {
            SshClient.Disconnect();
            SshClient.Dispose();
        }

        private static int RunCommand(string function, string container)
        {
            //check if a container with the name exists
            string result;

            try
            {
                result = SshClient.RunCommand("docker ps -a --format {{.Names}}").Result;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

            //docker [process] [container name]
            string process = string.Empty, name = string.Empty;

            foreach (string func in new string[] { "start", "stop", "restart" })
                if (func.Equals(function, StringComparison.OrdinalIgnoreCase))
                    process = function;

            if (result.Contains(container, StringComparison.OrdinalIgnoreCase))
                name = container;
            try
            {
                SshCommand command = SshClient.RunCommand($"docker {process} {name}");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            return 1;
        }
    }
}
