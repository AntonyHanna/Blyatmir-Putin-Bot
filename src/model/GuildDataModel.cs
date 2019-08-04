using Discord;
using Discord.WebSocket;

namespace Blyatmir_Putin_Bot.Model
{
    public class GuildData
    {
        public SocketGuild Guild { get; }

        public ulong GuildId { get; set; }
        public string GuildName { get; set; }
        //public Warning Warnings { get; set; }
        /// <summary>
        /// Is the server listed in highscores list
        /// </summary>
        public bool IsListed { get; set; }
        public int Points { get; set; }
        public int HighestPoints { get; set; }
        public int LowestPoints { get; set; }
        public ulong QuoteChannelId { get; set; }
        public ulong AnnouncmentChannelId { get; set; }

        public GuildData()
        {
            this.Guild = default;
            this.GuildName = default;
            this.GuildId = default;
            //this.Warnings = null;
            this.IsListed = true;
            this.Points = default;
            this.HighestPoints = default;
            this.LowestPoints = default;
            this.QuoteChannelId = default;
            this.AnnouncmentChannelId = default;
        }
        public GuildData(IGuild guild)
        {
            this.Guild = guild as SocketGuild;
            this.GuildName = guild.Name;
            this.GuildId = guild.Id;

            //this.Warnings = null;
            this.IsListed = true;
            this.Points = default;
            this.HighestPoints = default;
            this.LowestPoints = default;
            this.QuoteChannelId = default;
            this.AnnouncmentChannelId = default;
        }

        /// <summary>
        /// Sets a value for the QuoteChannel property
        /// </summary>
        /// <param name="channel"></param>
        public void SetQuoteChannel(ITextChannel channel)
        {
            this.QuoteChannelId = (channel as SocketTextChannel).Id;
        }

        /// <summary>
        /// Sets a value for the AnnouncmentChannel property
        /// </summary>
        /// <param name="channel"></param>
        public void SetAnnouncmentChannel(ITextChannel channel)
        {
            this.AnnouncmentChannelId = (channel as SocketTextChannel).Id;
        }
    }
}

