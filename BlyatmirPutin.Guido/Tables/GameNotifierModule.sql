﻿CREATE TABLE [dbo].[GameNotifierModule] (
	[Id] INT IDENTITY,
	[GuildId] BIGINT,
	[IsEnabled] BIGINT DEFAULT 1

	PRIMARY KEY ([Id])
);