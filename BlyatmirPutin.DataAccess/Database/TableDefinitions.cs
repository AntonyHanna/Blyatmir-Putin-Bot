using BlyatmirPutin.Models.Common;
using BlyatmirPutin.Models.Modules;

namespace BlyatmirPutin.DataAccess.Database
{
	/// <summary>
	/// Provide a mapping of types to SQL for table generation
	/// </summary>
	public static class TableDefinitions
	{
		/// <summary>
		/// Mapping of types to SQL
		/// </summary>
		public static readonly Dictionary<Type, string> Mappings = new Dictionary<Type, string>()
		{
			{ typeof(Guild), "CREATE TABLE Guild (Id BIGINT, Prefix char DEFAULT '!', PRIMARY KEY(Id));" },
			{ typeof(Member), "CREATE TABLE Member (Id BIGINT, CurrentIntro NVARCHAR, PRIMARY KEY (Id));" },
			{ typeof(Game), "CREATE TABLE Game (Id INT IDENTITY, WasPosted BIT DEFAULT 0, Title NVARCHAR NOT NULL, Blurb NVARCHAR, Developer NVARCHAR, Publisher NVARCHAR, Banner NVARCHAR, Poster NVARCHAR, StartDate DATETIME NOT NULL, EndDate DATETIME NOT NULL, PRIMARY KEY (Id));" },
			{ typeof(IntroMusic), "CREATE TABLE IntroMusic (Id NVARCHAR IDENTITY, UploaderId BIGINT NOT NULL, IntroName NVARCHAR NOT NULL, FilePath NVARCHAR NOT NULL, DateAdded BIGINT, PRIMARY KEY (Id));" },
			{ typeof(IntroMusicRecord), "CREATE TABLE IntroMusicRecord (Id NVARCHAR IDENTITY, UserId BIGINT, IntroId NVARCHAR, DateSet BIGINT, PRIMARY KEY (Id));" },
			{ typeof(FModuleSettings), "CREATE TABLE FModuleSettings (Id INT IDENTITY, GuildId BIGINT, IsEnabled BIGINT default 1, Threshold INT DEFAULT 3, Cooldown INT DEFAULT 30, PRIMARY KEY (Id));" },
			{ typeof(GameNotifierModuleSettings), "CREATE TABLE GameNotifierModuleSettings (Id INT IDENTITY, GuildId BIGINT, IsEnabled BIGINT DEFAULT 1, PRIMARY KEY (Id));" },
			{ typeof(IntroMusicModuleSettings), "CREATE TABLE IntroMusicModuleSettings (Id NVARCHAR IDENTITY, GuildId BIGINT, IsEnabled BIT DEFAULT 1, VoteThreshold INT, PRIMARY KEY (Id));" }
		};
	}
}
