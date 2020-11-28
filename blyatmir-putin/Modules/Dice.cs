using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	public class Dice : ModuleBase<SocketCommandContext>
	{
		/// <summary>
		/// Dice Roll between 1 and roll (inclusive)
		/// </summary>
		/// <param name="roll"></param>
		/// <returns></returns>
		private int RollDice(int roll = 5) => new Random().Next(roll) + 1;

		/// <summary>
		/// Roll a regular dice from 1-6
		/// </summary>
		/// <returns></returns>
		[Command("dice")]
		[Alias("d")]
		public async Task DiceRollAsync()
		{
			var embed = new EmbedBuilder
			{
				Color = Color.Green,
				Title = $"Dice Roll Results",
				Author = new EmbedAuthorBuilder
				{
					Name = "(╯°□°）╯︵ ┻━┻",
					IconUrl = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png",
				},
				Fields = new List<EmbedFieldBuilder>
				{
					new EmbedFieldBuilder
					{
						Name = "Roll",
						Value = $"`{this.RollDice()}`",
						IsInline = false
					}
				},
				Footer = new EmbedFooterBuilder
				{
					Text = $"Rolled on some mystical dice from bum fuck nowhere seems to have 6 possible value(s)... BLYAT!"
				}
			};

			//send the message
			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}

		/// <summary>
		/// Roll a dice from 1 to the number specified
		/// </summary>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		[Command("dice")]
		[Alias("d")]
		public async Task DiceRollAsync(int maxValue)
		{
			var embed = new EmbedBuilder
			{
				Color = Color.Green,
				Title = $"Dice Roll Results",
				Author = new EmbedAuthorBuilder
				{
					Name = "(╯°□°）╯︵ ┻━┻",
					IconUrl = $"https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/153/game-die_1f3b2.png",
				},
				Fields = new List<EmbedFieldBuilder>
				{
					new EmbedFieldBuilder
					{
						Name = "Roll",
						Value = $"`{this.RollDice(maxValue)}`",
						IsInline = false
					}
				},
				Footer = new EmbedFooterBuilder
				{
					Text = $"Rolled on some mystical dice from bum fuck nowhere seems to have {maxValue} possible value(s)... BLYAT!"
				}
			};

			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}
	}
}
