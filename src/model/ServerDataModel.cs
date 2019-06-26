using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace Blyatmir_Putin_Bot.model
{
    public class ServerDataModel
    {
        //foreach server the bot is in new entry in json
        //serverdata should hold info on how many points the server has
        //how many warnings players from the server have

        public IGuild Server { get; set; }
        public bool HSData { get; set; } //is the server opted in for other servers viewing data
        public int points { get; set; }
        public int highestPoints { get; set; }
        public int lowestPoints { get; set; }
    }
}
