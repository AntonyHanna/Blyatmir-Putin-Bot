using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System;
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
			//new field woo hoo
			EmbedFieldBuilder field = new EmbedFieldBuilder()
			{
				Name = "Roll",
				Value = $"`{this.RollDice()}`",
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
		/// <param name="maxValue"></param>
		/// <returns></returns>
		[Command("dice")]
		[Alias("d")]
		public async Task DiceRollAsync(int maxValue)
		{
			//new field woo hoo
			EmbedFieldBuilder field = new EmbedFieldBuilder()
			{
				Name = "Roll",
				Value = $"`{this.RollDice(maxValue)}`",
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
				FooterText = $"Rolled on some mystical dice from bum fuck nowhere seems to have {maxValue} possible value(s)... BLYAT!"
			};

			//send the message
			await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
		}
	}
}
