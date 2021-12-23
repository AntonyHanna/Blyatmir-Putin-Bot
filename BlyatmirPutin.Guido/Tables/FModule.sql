CREATE TABLE FModule (
	Id INT IDENTITY,
	GuildId BIGINT,
	IsEnabled BIGINT default 1,
	Threshold INT DEFAULT 3,
	Cooldown INT DEFAULT 30

	PRIMARY KEY (Id)
);