using System;

namespace Blyatmir_Putin_Bot
{
    public class Env
    {
        private static string _botToken = "BOT_TOKEN";
        private static string _botPrefix = "BOT_PREFIX";
        private static string _configLocation = "CONFIG";

        public static string BotToken => _botToken;
        public static string BotPrefix => _botPrefix;
        public static string ConfigLocation => _configLocation;

        public static void LoadVariables()
        {
            string botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
            string botPrefix = Environment.GetEnvironmentVariable("BOT_PREFIX");
            string configLocation = Environment.GetEnvironmentVariable("CONFIG");

            if (!string.IsNullOrWhiteSpace(botToken) && BotToken == "BOT_TOKEN")
                _botToken = botToken;

            if (!string.IsNullOrWhiteSpace(botPrefix) && BotPrefix == "BOT_PREFIX")
                _botPrefix = botPrefix;

            //if (!string.IsNullOrWhiteSpace(configLocation) && ConfigLocation == "CONFIG")
                _configLocation = "/config";

            Console.WriteLine($"Config should be located in {ConfigLocation}");
        }
    }
}
