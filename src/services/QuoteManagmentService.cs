using Blyatmir_Putin_Bot.model;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;
using System.Globalization;
using System;

namespace Blyatmir_Putin_Bot.services
{
    public class QuoteManagamentService
    {
        public static IGuildUser Quoter { get; set; }
        public static RestUserMessage QuoteConfirmationMessage;
        public static SocketMessage Quote { get; set; }
        private static SocketMessage quoteInQuestion { get; set; }
        private static List<IGuildUser> quotedUser { get; set; }

        private static Timer _timer;

        private static EmbedAuthorBuilder author;
        private static EmbedBuilder embed;
        private static EmbedFooterBuilder footer;

        private static IReadOnlyCollection<SocketUser> mentionedUsers;

        public static async Task QuoteIntentAsync(SocketMessage message)
        {
            if (!message.Author.IsBot)
            {
                mentionedUsers = message.MentionedUsers;

                if(mentionedUsers.Count() > 0)
                {
                    if (message.Content.Contains($"- <@{mentionedUsers.ElementAt(0).Id}>"))
                        quoteInQuestion = Quote = message;

                    else
                        return;
                }

                if (quoteInQuestion != null)
                {
                    GetValues();

                    if(PersistantStorage.GetServerData(Quoter.Guild).QuoteChannelId != 0)
                    {
                        if (IsPotentialQuote())
                        {
                            await SendQuoteConfirmationMessageAsync();
                            StartTimeoutTrigger();
                        }
                    }

                    else
                    {
                        ResetEmbedConstructors();

                        author.Name = "Failed to run the quote service";

                        embed.Color = Color.Red;
                        embed.Description = $"You must first specify a quote channel for this server, " +
                            $"before you can use this command, for more information see the docs (tbd)";
                        embed.ImageUrl = "https://cdn.discordapp.com/attachments/559700127275679762/595113538540797962/sadputin.png";

                        footer.IconUrl = "https://cdn.discordapp.com/attachments/559700127275679762/595110812314632205/585bad69cb11b227491c3284.png";
                        footer.Text = "This was an automated message, don't even at me comrade";

                        await Quote.Channel.SendMessageAsync(embed: embed.Build());
                        Reset();
                    }
                }
            }
        }

        private static void GetValues()
        {
            Quoter = quoteInQuestion.Author as IGuildUser;
            quotedUser = quoteInQuestion.MentionedUsers as List<IGuildUser>;
        }

        private static bool IsPotentialQuote()
        {
            if (quoteInQuestion.Content.Contains("-"))
                return true;

            else
                return false;
        }

        private static async Task SendQuoteConfirmationMessageAsync()
        {
            ResetEmbedConstructors();

            author.Name = "Want me to quote this bish...";

            embed.Color = Color.Green;
            embed.ThumbnailUrl = $"https://cdn.discordapp.com/attachments/559700127275679762/594028718683455498/Fucked-sm.png";
            embed.Description = $"I may be mostly blind, I may or may not have done one too many shooeys but this looks like a quote and just like karen did with the kids I can take them away.\n\n " +
                $"**Quote In Question**\n\n" +
                $"\"*{ Quote.Content }*\" \n\n" +
                $"`Click on one of the below reactions to continue...`";

            footer.IconUrl = "https://cdn.discordapp.com/attachments/559700127275679762/595110812314632205/585bad69cb11b227491c3284.png";
            footer.Text = "This was an automated message and will expire in an hour if there is no response";

            quoteInQuestion = null; //stops the bot from responding to previously sent messages and its own

            QuoteConfirmationMessage = await Quote.Channel.SendMessageAsync(embed: embed.Build());
            await AddReactionsAsync();
        }

        /// <summary>
        /// Add reactions to the message
        /// </summary>
        /// <returns></returns>
        private static async Task AddReactionsAsync()
        {
            await QuoteConfirmationMessage.AddReactionsAsync(new Emoji[]
            {
                new Emoji("\U00002705"),
                new Emoji("\U0000274E")
            });
        }


        private static void StartTimeoutTrigger()
        {
            _timer = new Timer(10000)
            {
                AutoReset = false
            };

            _timer.Elapsed += TimeoutConfirmationRequestAsync;
            _timer.Start();
        }
        private static async Task<RestUserMessage> SendTimeoutMessageAsync()
        {
            ResetEmbedConstructors();

            author.IconUrl = $"{BotConfig.Client.CurrentUser.GetAvatarUrl()}";
            author.Name = $"You all left me to die, so i've let your quote rot away!";

            embed.Color = Color.Red;
            embed.Title = "\n" +
                "Quote Timed Out  :(";
            embed.Description = $"A quote request has timed out for the following message\n\n" +
                $"\"*{Quote.Content}*\"";

            footer.IconUrl = "https://cdn.discordapp.com/attachments/559700127275679762/595117013203025950/902023-1.png";
            footer.Text = "This was an automated message, don't even at me comrade";

            return await Quote.Channel.SendMessageAsync(embed: embed.Build());
        }

        private static async void TimeoutConfirmationRequestAsync(object obj, ElapsedEventArgs args)
        {
            //remove the message from chat in the case of no response
            await QuoteConfirmationMessage.DeleteAsync();

            //notify user that the quote has time out
            await SendTimeoutMessageAsync();

            //remove the association to the message
            Reset();
        }

        /// <summary>
        /// Quote in question has been denied, remove all prompts and notifications
        /// </summary>
        /// <returns></returns>
        public static async Task QuoteDeniedAsync()
        {
            await Quote.Channel.DeleteMessageAsync(QuoteConfirmationMessage.Id);
            Reset();

            //stop timers
            _timer.Stop();
        }

        /// <summary>
        /// Quote in question has been confirmed, transfer the quote to quotes channel
        /// </summary>
        /// <param name="reaction"></param>
        /// <returns></returns>
        public static async Task QuoteConfirmedAsync()
        {
            //stop the timer
            _timer.Stop();

            //remove the confirmation message message
            await QuoteConfirmationMessage.DeleteAsync();

            GuildData guildData = PersistantStorage.GetServerData(Quoter.Guild);
            DateTime currentTime = DateTime.Now;

            if(guildData.QuoteChannelId != 0)
            {
                ResetEmbedConstructors();

                embed.Color = Color.Green;
                embed.ThumbnailUrl = $"{mentionedUsers.ElementAt(0).GetAvatarUrl()}";
                embed.Description = $"{Quote.Content}";

                footer.IconUrl = Quoter.GetAvatarUrl(ImageFormat.Auto);
                footer.Text = $"Quoted by: {Quoter} | {currentTime.ToString("dddd, dd MMMM yyyy h:mm:ss tt")}";

                var quoteChannel = await Quoter.Guild.GetTextChannelAsync(guildData.QuoteChannelId);
                await quoteChannel.SendMessageAsync(embed: embed.Build());

                Reset();
            }
        }

        /// <summary>
        /// Reset the variables associated with the QuoteManamgmentService
        /// </summary>
        private static void Reset()
        {
            QuoteConfirmationMessage = null;
            Quoter = null;
            Quote = null;
            quotedUser = null;
        }

        /// <summary>
        /// Resets the embed constructors for re-use throughout the class
        /// </summary>
        private static void ResetEmbedConstructors()
        {
            author = new EmbedAuthorBuilder();
            embed = new EmbedBuilder();
            footer = new EmbedFooterBuilder();

            embed.Author = author;
            embed.Footer = footer;
        }
    }
}