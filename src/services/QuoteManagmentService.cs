using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;
using System.Globalization;
using System;

namespace Blyatmir_Putin_Bot.Services
{
    public class QuoteManagamentService
    {
        /// <summary>
        /// The user that quoted the original message
        /// </summary>
        private static IGuildUser Quoter
        {
            get
            {
                if (Quote != null)
                    return Quote.Author as IGuildUser;

                else
                    return null;
            }
        }

        /// <summary>
        /// Reference to the message that was sent to confirm the quote
        /// </summary>
        public static RestUserMessage QuoteConfirmationMessage;

        /// <summary>
        /// The message containing the quote
        /// </summary>
        private static SocketMessage Quote { get; set; }

        /// <summary>
        /// A secondary reference to the Quote
        /// </summary>
        private static SocketMessage QuoteInQuestion { get; set; }

        /// <summary>
        /// Collection of users who were mentioned in the Quote
        /// </summary>
        private static IReadOnlyCollection<SocketUser> MentionedUsers;

        private static Timer _timer;

        public static async Task QuoteIntentProcessorAsync(SocketMessage message)
        {
            if (!message.Author.IsBot)
            {
                if(IsPotentialQuote(message))
                {
                    //check if the Guild has a QuoteChannel specified
                    if (PersistantStorage.GetServerData(Quoter.Guild).QuoteChannelId != 0)
                    {
                        await SendQuoteConfirmationMessageAsync();
                        StartTimeoutTrigger();
                    }

                    else
                    {
                        //send a message if the service has failed
                        var easyEmbed = new EasyEmbed()
                        {
                            AuthorName = "Failed to run the quote service",
                            EmbedColor = Color.Red,
                            EmbedImage = "https://cdn.discordapp.com/attachments/559700127275679762/595113538540797962/sadputin.png",
                            EmbedDescription = $"You must first specify a quote channel for this server, " +
                            $"before you can use this command, for more information see the docs (tbd)",
                            FooterIcon = $"https://cdn.discordapp.com/attachments/559700127275679762/595110812314632205/585bad69cb11b227491c3284.png",
                            FooterText = $"This was an automated message, don't even at me comrade"
                        };

                        await Quote.Channel.SendMessageAsync(embed: easyEmbed.Build());

                        ResetVariables();
                    }
                }
            }
        }

        /// <summary>
        /// Checks the message and determines whether it's a potential quote
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static bool IsPotentialQuote(SocketMessage message)
        {
            MentionedUsers = message.MentionedUsers;
            if (MentionedUsers.Count() > 0)
            {
                //one check for users without nicknames and second names is for users with nicknames
                //! is only present when a user is mentioned through their nickname
                if (message.Content.Contains($"- <@{MentionedUsers.ElementAt(0).Id}>") || message.Content.Contains($"- <@!{MentionedUsers.ElementAt(0).Id}>"))
                {
                    //if true assign the quoteInQuestion to the message
                    QuoteInQuestion = Quote = message;

                    return true;
                }

                else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Sends a quote confirmation message
        /// </summary>
        /// <returns></returns>
        private static async Task SendQuoteConfirmationMessageAsync()
        {
            QuoteInQuestion = null; //stops the bot from responding to previously sent messages and its own

            //embed template
            var easyEmbed = new EasyEmbed
            {
                AuthorName = "Want me to quote this bish...",
                EmbedColor = Color.Green,
                EmbedThumbnail = $"https://cdn.discordapp.com/attachments/559700127275679762/594028718683455498/Fucked-sm.png",
                EmbedDescription = $"I may be mostly blind, I may or may not have done one too many shooeys but this looks like a quote and just like karen did with the kids I can take them away.\n\n " +
                $"**Quote In Question**\n\n" +
                $"\"*{ Quote.Content }*\" \n\n" +
                $"`Click on one of the below reactions to continue...`",
                FooterIcon = $"https://cdn.discordapp.com/attachments/559700127275679762/595110812314632205/585bad69cb11b227491c3284.png",
                FooterText = $"This was an automated message and will expire in an hour if there is no response"
            };

            //send the message and keep a reference to it
            QuoteConfirmationMessage = await Quote.Channel.SendMessageAsync(embed: easyEmbed.Build());

            //add the reactions to the message
            await AddReactionsAsync();
        }

        /// <summary>
        /// Add reactions to the message
        /// </summary>
        /// <returns></returns>
        public static async Task AddReactionsAsync()
        {
            await QuoteConfirmationMessage.AddReactionsAsync(new Emoji[]
            {
                new Emoji("\U00002705"),
                new Emoji("\U0000274E")
            });
        }

        /// <summary>
        /// Starts a timer until the confirmation message timesout
        /// </summary>
        private static void StartTimeoutTrigger()
        {
            _timer = new Timer(360000)
            {
                AutoReset = false
            };

            _timer.Elapsed += TimeoutConfirmationRequestAsync;
            _timer.Start();
        }

        /// <summary>
        /// Notify the Guild that the message has timed out
        /// </summary>
        /// <returns></returns>
        private static async Task<RestUserMessage> SendTimeoutMessageAsync()
        {
            var easyEmbed = new EasyEmbed
            {
                AuthorName = $"You all left me to die, so i've let your quote rot away!",
                AuthorIcon = $"{BotConfig.Client.CurrentUser.GetAvatarUrl()}",
                EmbedColor = Color.Red,
                EmbedTitle = "\nQuote Timed Out :(",
                EmbedDescription = $"A quote request has timed out for the following message\n\n \"*{ Quote.Content }*\" \n\n",
                FooterIcon = $"https://cdn.discordapp.com/attachments/559700127275679762/595117013203025950/902023-1.png",
                FooterText = $"This was an automated message, don't even at me comrade"
            };

            return await Quote.Channel.SendMessageAsync(embed: easyEmbed.Build());
        }

        /// <summary>
        /// The event that takes place when the timeout timer concludes
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private static async void TimeoutConfirmationRequestAsync(object obj, ElapsedEventArgs args)
        {
            //remove the message from chat in the case of no response
            await QuoteConfirmationMessage.DeleteAsync();

            //notify user that the quote has time out
            await SendTimeoutMessageAsync();

            //remove the association to the message
            ResetVariables();
        }

        /// <summary>
        /// Quote in question has been denied, remove all prompts and notifications
        /// </summary>
        /// <returns></returns>
        public static async Task QuoteDeniedAsync()
        {
            await Quote.Channel.DeleteMessageAsync(QuoteConfirmationMessage.Id);
            ResetVariables();

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
                var quoteChannel = await Quoter.Guild.GetTextChannelAsync(guildData.QuoteChannelId);

                var easyEmbed = new EasyEmbed
                {
                    EmbedColor = Color.Green,
                    EmbedThumbnail = $"{MentionedUsers.ElementAt(0).GetAvatarUrl()}",
                    EmbedDescription = $"{Quote.Content}",
                    FooterIcon = Quoter.GetAvatarUrl(ImageFormat.Auto),
                    FooterText = $"Quoted by: {Quoter} | {currentTime.ToString("dddd, dd MMMM yyyy h:mm:ss tt")}"
                };

                await quoteChannel.SendMessageAsync(embed: easyEmbed.Build());

                ResetVariables();
            }
        }

        /// <summary>
        /// Reset the variables associated with the QuoteManamgmentService
        /// </summary>
        private static void ResetVariables()
        {
            QuoteConfirmationMessage = null;
            Quote = null;
        }
    }
}