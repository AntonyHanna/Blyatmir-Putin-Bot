using System.ComponentModel.DataAnnotations;

namespace BlyatmirPutin.Models.Common
{
	/// <summary>
	/// Represent a Discord Guild in the Database
	/// </summary>
	public class Guild
	{
		/// <summary>
		/// The Guilds ID according to Discord
		/// </summary>
		[Key]
		public ulong Id { get; init; }

		/// <summary>
		/// The prefix used for the Discord Guild
		/// </summary>
		public char Prefix { get; set; }

		public override string ToString()
		{
			return $"---------\nId: [{Id}]\nPrefix: [{Prefix}]\n---------";
		}
	}
}
