using System;
using System.Collections.Generic;
using System.Text;

namespace Blyatmir_Putin_Bot
{
    public class Env
    {
        private static string _botToken = "BOT_TOKEN_HERE";
        private static string _botPrefix = "BOT_PREFIX_HERE";

        public static string BotToken => _botToken;
        public static string BotPrefix => _botPrefix;

        public static void LoadVariables()
        {
            string botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
            string botPrefix = Environment.GetEnvironmentVariable("BOT_PREFIX");

            if(!string.IsNullOrWhiteSpace(botToken) && BotToken == "BOT_TOKEN_HERE")
                _botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");

            if(!string.IsNullOrWhiteSpace(botPrefix) && BotPrefix == "BOT_PREFIX_HERE")
                _botPrefix = Environment.GetEnvironmentVariable("BOT_PREFIX");
        }
    }
}
