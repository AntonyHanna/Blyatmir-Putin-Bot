﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.modules
{
    [Name("pintochannel")]
    [Summary("Gets channel pins and transfers to them to another channel as text")]
    [Remarks("`ptc [ITextChannel sourceChannel] - Gets channel pins and transfers to them to another channel as text`")]
    public class quotet : ModuleBase<SocketCommandContext>
    {
        [Command("pintochannel")]
        [Alias("ptc")]
        public async Task PinToTextChannel(ITextChannel sourceChannel)
        {
            var pinnedMessages = await sourceChannel.GetPinnedMessagesAsync();

            foreach(var msg in pinnedMessages)
                await Context.Channel.SendMessageAsync(msg.Content);
        }
    }
}
