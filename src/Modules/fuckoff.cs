using Blyatmir_Putin_Bot.Model;
using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	[Name("fuckoff")]
	[Summary("Tell em to fuck off")]
	[Remarks("`fuckoff [IGuildUser user] - Tell a prick to fuck off\n" +
		"fuckoff[string words] - Tell an inanimate object to fuck off`")]
	public class Fuckoff : ModuleBase<SocketCommandContext>
	{
		/// <summary>
		/// Tell a user to fuck off
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		[Command("fuckoff")]
		[Summary("Tell a prick to fuckoff")]
		public async Task FuckOffAsync(IGuildUser user)
		{
			//some literal string that should really be 
			//moved into a file so that it's not as scuffed
			string[] fuckoffASCII = new string[] { "凸ಠ益ಠ)凸",
			"凸(｀0´)凸",
			"凸(>皿<)凸",
			"┌∩┐(ಠ_ಠ)┌∩┐",
			"凸(^▼ｪ▼ﾒ^)" };

			string[] fuckOffMessage = new string[] { $"You're not even worth the English {user.Username}, vittu!",
			$"{user.Username}, Have you seen my fucks? I can't seem to find them",
			$"Fucking fuck off,  {user.Username}.",
			$"{user.Username}, shut the fuck up.",
			$"Christ on a bendy-bus,  {user.Username}, don't be such a fucking faff-arse.",
			$"What the fuck is you problem {user.Username}?",
			$"{user.Username}, why don't you go outside and play hide-and-go-fuck-yourself?",
			$"Christ on a bendy-bus,  {user.Username}, don't be such a fucking faff-arse.",
			$"Remember when you fucked off  {user.Username}? Those were the good times !",
			$"{user.Username}, back the fuck off.",
			$"Who has two thumbs and doesn't give a fuck? {user.Username}.",
			$"{user.Username}, what the fuck where you actually thinking?",
			$"{user.Username}, Thou clay-brained guts, thou knotty-pated fool, thou whoreson obscene greasy tallow-catch!",
			$"{user.Username}, do I look like I give a fuck?",
			$"Fuck you, {user.Username}",
			$"{user.Username}, there aren't enough swear-words in the English language, so now I'll have to call you perkeleen " +
			$"vittupää just to express my disgust and frustration with this crap.",
			$"{user.Username}, Here is a list I made of all the fucks I give: {Environment.NewLine}",
			$"Merry Fucking Christmas, {user.Username}.",
			$"{user.Username}: A fucking problem solving super-hero."};

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

		/// <summary>
		/// Tell a string to fuck off
		/// </summary>
		/// <param name="terms"></param>
		/// <returns></returns>
		[Command("fuckoff")]
		[Summary("Tell a prick to fuckoff")]
		public async Task FuckOffAsync([Remainder]string terms)
		{
			//some more literal strings, should really move this soon
			string[] fuckoffASCII = new string[] { "凸ಠ益ಠ)凸",
			"凸(｀0´)凸",
			"凸(>皿<)凸",
			"┌∩┐(ಠ_ಠ)┌∩┐",
			"凸(^▼ｪ▼ﾒ^)" };

			string[] fuckOffMessage = new string[] { $"You're not even worth the English {terms}, vittu!",
			$"{terms}, Have you seen my fucks? I can't seem to find them",
			$"Fucking fuck off,  {terms}.",
			$"{terms}, shut the fuck up.",
			$"Christ on a bendy-bus,  {terms}, don't be such a fucking faff-arse.",
			$"What the fuck is you problem {terms}?",
			$"{terms}, why don't you go outside and play hide-and-go-fuck-yourself?",
			$"Christ on a bendy-bus,  {terms}, don't be such a fucking faff-arse.",
			$"Remember when you fucked off  {terms}? Those were the good times !",
			$"{terms}, back the fuck off.",
			$"Who has two thumbs and doesn't give a fuck? {terms}.",
			$"{terms}, what the fuck where you actually thinking?",
			$"{terms}, Thou clay-brained guts, thou knotty-pated fool, thou whoreson obscene greasy tallow-catch!",
			$"{terms}, do I look like I give a fuck?",
			$"Fuck you, {terms}",
			$"{terms}, there aren't enough swear-words in the English language, so now I'll have to call you perkeleen " +
			$"vittupää just to express my disgust and frustration with this crap.",
			$"{terms}, Here is a list I made of all the fucks I give: {Environment.NewLine}",
			$"Merry Fucking Christmas, {terms}.",
			$"{terms}: A fucking problem solving super-hero."};


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
