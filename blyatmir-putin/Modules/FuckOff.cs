using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	public class Fuckoff : ModuleBase<SocketCommandContext>
	{
		[Command("fuckoff")]
		public async Task FuckOffAsync(IGuildUser user)
			=> await SendFuckOffMessage(user.Username);

		[Command("fuckoff")]
		[Summary("Tell a prick to fuckoff")]
		public async Task FuckOffAsync([Remainder] string terms)
			=> await SendFuckOffMessage(terms);


		/// <summary>
		/// Tell somebody / something to fuckoff
		/// </summary>
		/// <param name="recipient">That somebody / something</param>
		/// <returns></returns>
		private async Task SendFuckOffMessage(string recipient)
		{
			//some more literal strings, should really move this soon
			string[] fuckoffASCII = new string[] { "凸ಠ益ಠ)凸",
			"凸(｀0´)凸",
			"凸(>皿<)凸",
			"┌∩┐(ಠ_ಠ)┌∩┐",
			"凸(^▼ｪ▼ﾒ^)" };

			string[] fuckOffMessage = new string[] { $"You're not even worth the English {recipient}, vittu!",
			$"{recipient}, Have you seen my fucks? I can't seem to find them",
			$"Fucking fuck off,  {recipient}.",
			$"{recipient}, shut the fuck up.",
			$"Christ on a bendy-bus,  {recipient}, don't be such a fucking faff-arse.",
			$"What the fuck is you problem {recipient}?",
			$"{recipient}, why don't you go outside and play hide-and-go-fuck-yourself?",
			$"Christ on a bendy-bus,  {recipient}, don't be such a fucking faff-arse.",
			$"Remember when you fucked off  {recipient}? Those were the good times !",
			$"{recipient}, back the fuck off.",
			$"Who has two thumbs and doesn't give a fuck? {recipient}.",
			$"{recipient}, what the fuck where you actually thinking?",
			$"{recipient}, Thou clay-brained guts, thou knotty-pated fool, thou whoreson obscene greasy tallow-catch!",
			$"{recipient}, do I look like I give a fuck?",
			$"Fuck you, {recipient}",
			$"{recipient}, there aren't enough swear-words in the English language, so now I'll have to call you perkeleen " +
			$"vittupää just to express my disgust and frustration with this crap.",
			$"{recipient}, Here is a list I made of all the fucks I give: {Environment.NewLine}",
			$"Merry Fucking Christmas, {recipient}.",
			$"{recipient}: A fucking problem solving super-hero."};

			var rnd = new Random();

			//embed template
			var easyEmbed = new EasyEmbed()
			{
				//use rnd as random indexing between the literal strings
				AuthorName = fuckoffASCII[rnd.Next(0, fuckoffASCII.Length)],
				AuthorIcon = $"https://cdn.discordapp.com/emojis/541203072869990400.png?v=1",
				EmbedColor = Color.LightOrange,
				FooterText = fuckOffMessage[rnd.Next(0, fuckOffMessage.Length)] + " - @" + Context.Message.Author.Username
			};

			//send the message
			await Context.Channel.SendMessageAsync(embed: easyEmbed.Build());
		}
	}
}
