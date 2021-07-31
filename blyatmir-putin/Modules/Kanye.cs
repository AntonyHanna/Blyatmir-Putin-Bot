using blyatmir_putin.Core.Interfaces;
using blyatmir_putin.Core.Models;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace blyatmir_putin.Modules
{
	public class Kanye : ModuleBase<SocketCommandContext>
	{
		public class KanyeSchema
		{
			public string Quote;
		}

		[Command("Kanye")]
		[Alias("Kanye")]
		public async Task KanyeSays()
		{
			string json = WebHelper.GetRequest(@"https://api.kanye.rest/");

			KanyeSchema kanye = JsonConvert.DeserializeObject<KanyeSchema>(json);

			EmbedBuilder embed = new EmbedBuilder
			{
				Color = Color.Gold,
				Author = new EmbedAuthorBuilder
				{
					IconUrl = @"https://www.gannett-cdn.com/-mm-/8bd21b2bb98d09920df33a192a65dd25a2cca2cc/c=0-138-2996-1831/local/-/media/2016/02/14/USATODAY/USATODAY/635910536775961939-GTY-509642162-79592586.JPG",
					Name = "Pablo",
					Url = @"https://www.gannett-cdn.com/-mm-/8bd21b2bb98d09920df33a192a65dd25a2cca2cc/c=0-138-2996-1831/local/-/media/2016/02/14/USATODAY/USATODAY/635910536775961939-GTY-509642162-79592586.JPG"
				},
				Title = "Kanye says:",
				Description = kanye.Quote,
				ImageUrl = @"https://i.pinimg.com/736x/d8/58/3f/d8583fe287203342606b495f3045209a.jpg",
			};

			await Context.Channel.SendMessageAsync(embed: embed.Build());
		}
	}
}
