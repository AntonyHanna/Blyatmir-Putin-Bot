using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blyatmir_Putin_Bot.Modules
{
	public class MaccasRun : ModuleBase<SocketCommandContext>
	{
		[Command("maccasrun")]
		public async Task Run(DateTime time, [Remainder] string location)
		{
			List<EmbedFieldBuilder> fields = new List<EmbedFieldBuilder>();

			fields.Add(new EmbedFieldBuilder
			{
				Name = "Location",
				Value = location,
				IsInline = true
			});

			fields.Add(new EmbedFieldBuilder
			{
				Name = "Time",
				Value = time.ToShortTimeString(),
				IsInline = true
			});

			EmbedBuilder embed = new EmbedBuilder
			{
				Title = "Calling all fat cunts",
				Description = "Meet at maccas to eat maccas.",
				Fields = fields,
				Color = Color.Red,
				ThumbnailUrl = "https://cdn.discordapp.com/attachments/559700127275679762/762252098263449600/McDonalds-Logo.png"
			};

			await Context.Channel.SendMessageAsync(embed: embed.Build());
			await Context.Channel.SendMessageAsync("@maccas ^^");
		}

		[Command("wantmaccas")]
		public async Task GiveMaccas()
		{
			IGuildUser user;
			SocketRole role;

			user = (IGuildUser)Context.Message.Author;
			role = await CreateRoleIfRequired(Context);

			if(role == null)
			{
				await Context.Channel.SendMessageAsync("I've created the `@maccas` role run this command again to be added...");
				return;
			}

			await user.AddRoleAsync(role);
			await Context.Message.Channel.SendMessageAsync("You can now go to maccas...");
		}

		private async Task<SocketRole> CreateRoleIfRequired(SocketCommandContext context)
		{
			SocketRole role = default;
			RestRole tmp;

			try
			{
				role = context.Guild.Roles.Where(r => r.Name == "maccas").First();
			}

			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				tmp = await context.Guild.CreateRoleAsync("maccas", isMentionable: true);
			}

			return role;
		}
	}
}