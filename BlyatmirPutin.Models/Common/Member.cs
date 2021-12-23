namespace BlyatmirPutin.Models.Common
{
	/// <summary>
	/// Represents a Discord User in the Database
	/// </summary>
	public class Member
	{
		/// <summary>
		/// The members User ID according to Discord
		/// </summary>
		public ulong Id { get; set; }

		/// <summary>
		/// The Id to the users intro
		/// </summary>
		public int CurrentIntro { get; set; }
	}
}
