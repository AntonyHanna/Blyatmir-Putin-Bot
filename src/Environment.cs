using System;

namespace Blyatmir_Putin_Bot
{
    public class Env
    {
        private static string _botToken = "BOT_TOKEN_HERE";
        private static string _botPrefix = "BOT_PREFIX_HERE";
        private static string _configLocation = "CONFIG_FOLDER_LOCATION";


        public static string BotToken => _botToken;
        public static string BotPrefix => _botPrefix;
        public static string ConfigLocation => _configLocation;

        public static void LoadVariables()
        {
            string botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
            string botPrefix = Environment.GetEnvironmentVariable("BOT_PREFIX");
            string configLocation = Environment.GetEnvironmentVariable("CONFIG");

            if (!string.IsNullOrWhiteSpace(botToken) && BotToken == "BOT_TOKEN_HERE")
                _botToken = botToken;

            if (!string.IsNullOrWhiteSpace(botPrefix) && BotPrefix == "BOT_PREFIX_HERE")
                _botPrefix = botPrefix;

            if (!string.IsNullOrWhiteSpace(configLocation) && ConfigLocation == "DATA_FOLDER_LOCATION")
                _configLocation = configLocation;
        }
    }
}
