using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Xml.Serialization;
using static Blyatmir_Putin_Bot.BotConfig;

namespace Blyatmir_Putin_Bot.model
{
    public static class PersistantStorage
    {
        private static readonly string _location = Path.Combine(AppEnvironment.ConfigLocation, "GuildData.xml");
        public static List<GuildData> ServerDataList = new List<GuildData>(PersistantStorage.Read());

        /// <summary>
        /// Makes sure that the persistant storage has been properly initialized
        /// </summary>
        public static void InitializeStorage()
        {
            if (!File.Exists(_location))
            {
                List<GuildData> initializatinList = new List<GuildData>();
                //initializatinList.Add(new ServerData());

                using (StreamWriter sr = new StreamWriter(_location))
                using (XmlWriter writer = XmlWriter.Create(sr, XmlSettings()))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<GuildData>));
                    serializer.Serialize(writer, initializatinList);
                }

                Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Log \t     A new GuildData file has been created");
            }
        }

        public static List<GuildData> Read()
        {
            InitializeStorage();
            using (XmlReader reader = XmlReader.Create(_location))
            {
                XmlSerializer serialize = new XmlSerializer(typeof(List<GuildData>));
                return serialize.Deserialize(reader) as List<GuildData>;
            }
        }

        public static void Write()
        {
            using (StreamWriter sr = new StreamWriter(_location))
            using (XmlWriter writer = XmlWriter.Create(sr, XmlSettings()))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<GuildData>));
                serializer.Serialize(writer, ServerDataList);
            }
        }

        /// <summary>
        /// The settings that should be used by all XmlWriters
        /// </summary>
        /// <returns></returns>
        private static XmlWriterSettings XmlSettings()
        {
            XmlWriterSettings settings = new XmlWriterSettings();

            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.CloseOutput = true;

            return settings;
        }

        /// <summary>
        /// Generates guild data for every server that the bot is present in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void GenerateGuildData(object sender, ElapsedEventArgs e)
        {
            //Create the config directory if it doesn't exist
            if (!Directory.Exists(AppEnvironment.ConfigLocation))
                Directory.CreateDirectory(AppEnvironment.ConfigLocation);

            //loop through all the guilds
            for (int j = 0; j < Client.Guilds.Count; j++)
            {
                //indexing for readonly collections
                var guild = Client.Guilds.ElementAt(j);
                bool isPresent = false;

                //dont run if there is no guild data 
                //otherwise compare the guild ids and only add the ones that are different
                if (PersistantStorage.ServerDataList.Count > 0)
                    foreach (var gld in PersistantStorage.ServerDataList)
                        if (guild.Id == gld.GuildId)
                            isPresent = true;

                //for the ones not present add them to data
                if (!isPresent)
                {
                    PersistantStorage.ServerDataList.Add(new GuildData(guild));
                    PersistantStorage.Write();
                    Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Create      Default data has been written for Guild: {guild.Name}");
                }
            }
        }

        /// <summary>
        /// For use with the GuildAvailable event
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static Task GenerateGuildData(SocketGuild arg)
        {
            //Create the config directory if it doesn't exist
            if (!Directory.Exists(AppEnvironment.ConfigLocation))
                Directory.CreateDirectory(AppEnvironment.ConfigLocation);

            //loop through all the guilds
            for (int j = 0; j <= Client.Guilds.Count; j++)
            {
                //indexing for readonly collections
                var guild = Client.Guilds.ElementAt(j);
                bool isPresent = false;

                //dont run if there is no guild data 
                //otherwise compare the guild ids and only add the ones that are different
                if (PersistantStorage.ServerDataList.Count > 0)
                    foreach (var gld in PersistantStorage.ServerDataList)
                        if (guild.Id == gld.GuildId)
                            isPresent = true;

                //for the ones not present add them to data
                if (!isPresent)
                {
                    PersistantStorage.ServerDataList.Add(new GuildData(guild));
                    PersistantStorage.Write();
                    Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Create      Default data has been written for Guild: {guild.Name}");
                }
            }

            return Task.CompletedTask;
        }

        public static Task GenerateGuildData()
        {
            //Create the config directory if it doesn't exist
            if (!Directory.Exists(AppEnvironment.ConfigLocation))
                Directory.CreateDirectory(AppEnvironment.ConfigLocation);

            //loop through all the guilds
            for (int j = 0; j < Client.Guilds.Count; j++)
            {
                //indexing for readonly collections
                var guild = Client.Guilds.ElementAt(j);
                bool isPresent = false;

                //dont run if there is no guild data 
                //otherwise compare the guild ids and only add the ones that are different
                if (PersistantStorage.ServerDataList.Count > 0)
                    foreach (var gld in PersistantStorage.ServerDataList)
                        if (guild.Id == gld.GuildId)
                            isPresent = true;

                //for the ones not present add them to data
                if (!isPresent)
                {
                    PersistantStorage.ServerDataList.Add(new GuildData(guild));
                    PersistantStorage.Write();
                    Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} Create      Default data has been written for Guild: {guild.Name}");
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets a specific guilds GuildData
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static GuildData GetServerData(SocketCommandContext context)
        {
            foreach (GuildData data in ServerDataList)
                if (data.GuildId == context.Guild.Id)
                    return data;
  
            return default;
        }

        /// <summary>
        /// Gets a specific guilds GuildData
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static GuildData GetServerData(IGuild guild)
        {
            foreach (GuildData data in ServerDataList)
                if (data.GuildId == guild.Id)
                    return data;

            return default;
        }

        /// <summary>
        /// Calculate the different score statistics
        /// </summary>
        /// <param name="guildData"></param>
        public static void PointCalculations(GuildData guildData)
        {
            if (guildData.Points > guildData.HighestPoints)
                guildData.HighestPoints = guildData.Points;

            if (guildData.Points < guildData.LowestPoints)
                guildData.LowestPoints = guildData.Points;

            Write();
        }
    }
}
