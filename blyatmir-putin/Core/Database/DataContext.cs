using Blyatmir_Putin_Bot.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Blyatmir_Putin_Bot.Core.Database
{
	public class DataContext : DbContext
	{
		public DbSet<Guild> Guilds { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<LocalGame> Games { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite("Data Source=./config/Guido.db");
	}
}
