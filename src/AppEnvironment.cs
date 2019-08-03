using System;

namespace Blyatmir_Putin_Bot
{
    public class AppEnvironment
    {
        private static string _botToken = "BOT_TOKEN";
        private static string _botPrefix = "BOT_PREFIX";
        private static string _configLocation = "config/";
        private static string _botActivity = "BOT_ACTIVITY";
        private static string _dockerIP = "DOCKER_IP";
        private static string _serverLogin = "SERVER_LOGIN";
        private static string _serverPassword = "SERVER_PASSWORD";

        public static string BotToken => _botToken;
        public static string BotPrefix => _botPrefix;
        public static string ConfigLocation => _configLocation;
        public static string BotActivity => _botActivity;
        public static string DockerIP => _dockerIP;
        public static string ServerLogin => _serverLogin;
        public static string ServerPassword => _serverPassword;


        /// <summary>
        /// Load environment variables
        /// </summary>
        public static void LoadVariables()
        {
            //pull the values from the environment
            string botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
            string botPrefix = Environment.GetEnvironmentVariable("BOT_PREFIX");
            string botActivity = Environment.GetEnvironmentVariable("BOT_ACTIVITY");
            string dockerIP = Environment.GetEnvironmentVariable("DOCKER_IP");
            string serverLogin = Environment.GetEnvironmentVariable("SERVER_LOGIN");
            string serverPassword = string.Empty;

            //try catch incase password is blank
            try
            {
                serverPassword = Environment.GetEnvironmentVariable("SERVER_PASSWORD");
            }

            catch
            {
                serverPassword = string.Empty;
            }

            //if the above values are not null and Bot_Token isn't default value
            //assign value
            if (!string.IsNullOrWhiteSpace(botToken) && BotToken == "BOT_TOKEN")
                _botToken = botToken;

            //the rest follow the same principle as above
            if (!string.IsNullOrWhiteSpace(botPrefix) && BotPrefix == "BOT_PREFIX")
                _botPrefix = botPrefix;

            if (!string.IsNullOrWhiteSpace(botActivity) && BotActivity == "BOT_ACTIVITY")
                _botActivity = botActivity;

            if (!string.IsNullOrWhiteSpace(dockerIP) && DockerIP == "DOCKER_IP")
                _dockerIP = dockerIP;

            if (!string.IsNullOrWhiteSpace(serverLogin) && DockerIP == "SERVER_LOGIN")
                _serverLogin = serverLogin;

            if (ServerPassword == "SERVER_PASSWORD")
                _serverPassword = serverPassword;
        }
    }
}
