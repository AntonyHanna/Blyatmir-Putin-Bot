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
			{ typeof(Member), "CREATE TABLE Member (Id BIGINT, CurrentIntro INT, PRIMARY KEY (Id));" },
			{ typeof(Game), "CREATE TABLE Game (Id INT IDENTITY, WasPosted BIT DEFAULT 0, Title NVARCHAR NOT NULL, Blurb NVARCHAR, Developer NVARCHAR, Publisher NVARCHAR, Banner NVARCHAR, Poster NVARCHAR, StartDate DATETIME NOT NULL, EndDate DATETIME NOT NULL, PRIMARY KEY (Id));" },
			{ typeof(IntroMusic), "CREATE TABLE IntroMusic (Id INT IDENTITY, UploaderId BIGINT NOT NULL, IntroName NVARCHAR NOT NULL, FilePath NVARCHAR NOT NULL, DateAdded DATETIME DEFAULT CURRENT_TIMESTAMP, PRIMARY KEY (Id));" },
			{ typeof(IntroMusicRecord), "CREATE TABLE IntroMusicRecord (Id INT IDENTITY, UserId BIGINT, IntroId INT, DateSet DATETIME, DateUnset DATETIME, PRIMARY KEY (Id));" },
			{ typeof(FModule), "CREATE TABLE FModule (Id INT IDENTITY, GuildId BIGINT, IsEnabled BIGINT default 1, Threshold INT DEFAULT 3, Cooldown INT DEFAULT 30, PRIMARY KEY (Id));" },
			{ typeof(GameNotifierModule), "CREATE TABLE GameNotifierModule (Id INT IDENTITY, GuildId BIGINT, IsEnabled BIGINT DEFAULT 1, PRIMARY KEY (Id));" },
			{ typeof(IntroMusicModule), "CREATE TABLE IntroMusicModule (Id INT IDENTITY, GuildId BIGINT, IsEnabled BIT DEFAULT 1, PRIMARY KEY (Id));" }
		};
	}
}
