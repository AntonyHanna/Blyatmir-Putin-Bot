-- CREATE TABLE IntroMusicRecord (
	-- UserId BIGINT,
	-- IntroId INT,
	-- DateSet DATETIME,
	-- DateUnset DATETIME

	-- PRIMARY KEY (UserId, IntroId, DateSet)
-- );


CREATE TABLE IntroMusicModule (
	Id INT IDENTITY,
	GuildId BIGINT,
	IsEnabled BIT DEFAULT 1

	PRIMARY KEY (Id)
);