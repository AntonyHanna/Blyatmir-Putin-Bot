CREATE TABLE [dbo].[IntroMusicModule] (
	[Id] INT IDENTITY,
	[GuildId] BIGINT,
	[IsEnabled] BIT DEFAULT 1

	PRIMARY KEY ([Id])
);