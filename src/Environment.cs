using System;
using System.Collections.Generic;
using System.Text;

namespace Blyatmir_Putin_Bot
{
    public class Env
    {
        private static string _botToken;
        private static string _botPrefix;
        public static string BotToken => _botToken;
        public static string BotPrefix => _botPrefix;

        public static void LoadVariables()
        {
            _botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
            _botPrefix = Environment.GetEnvironmentVariable("BOT_PREFIX");
        }
    }
}
