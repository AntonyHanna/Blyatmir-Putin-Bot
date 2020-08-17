﻿using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	public class Dice : ModuleBase<SocketCommandContext>
	{
		/// <summary>
		/// Roll a regular dice from 1-6
		/// </summary>
		/// <returns></returns>
		[Command("roll")]
		[Alias("dice")]
		public async Task DiceRollAsync()
		{
			//new field woo hoo
			EmbedFieldBuilder field = new EmbedFieldBuilder()
			{
				Name = "Roll",
				Value = $"`{new Random().Next(1, 6)}`",
				IsInline = false
			};

			//embed template
			var easyEmbed = new EasyEmbed()
			{
				AuthorName = "(╯°□°）╯︵ ┻━┻",
				AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png",
				EmbedColor = Color.Green,
				EmbedTitle = $"Dice Roll Results",
				EmbedField = field,
				FooterText = $"Rolled on some mystical dice from bum fuck nowhere seems to have 6 possible value(s)... BLYAT!"
			};

			//send the message
			await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
		}

		/// <summary>
		/// Roll a dice from 1 to the number specified
		/// </summary>
		/// <param name="num1"></param>
		/// <returns></returns>
		[Command("roll")]
		[Alias("dice")]
		public async Task DiceRollAsync(int num1)
		{
			//new field woo hoo
			EmbedFieldBuilder field = new EmbedFieldBuilder()
			{
				Name = "Roll",
				Value = $"`{new Random().Next(1, num1)}`",
				IsInline = false
			};

			//embed template
			var easyEmbed = new EasyEmbed()
			{
				AuthorName = "(╯°□°）╯︵ ┻━┻",
				AuthorIcon = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png",
				EmbedColor = Color.Green,
				EmbedTitle = $"Dice Roll Results",
				EmbedField = field,
				FooterText = $"Rolled on some mystical dice from bum fuck nowhere seems to have {num1} possible value(s)... BLYAT!"
			};

			//send the message
			await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
		}
	}
}
