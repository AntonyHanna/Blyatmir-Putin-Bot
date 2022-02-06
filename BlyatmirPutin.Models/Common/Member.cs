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
		public string CurrentIntro { get; set; }

		public override bool Equals(object? obj)
			=> obj != null && obj is Member && this.Id == (obj as Member)?.Id;

		public override int GetHashCode()
			=> this.Id.GetHashCode();
	}
}
