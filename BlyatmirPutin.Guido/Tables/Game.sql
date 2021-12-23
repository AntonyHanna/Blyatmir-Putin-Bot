CREATE TABLE [dbo].[Game] (
	[Id] INT IDENTITY,
	[WasPosted] BIT DEFAULT 0,
	[Title] NVARCHAR NOT NULL,
	[Blurb] NVARCHAR,
	[Developer] NVARCHAR,
	[Publisher] NVARCHAR,
	[Banner] NVARCHAR,
	[Poster] NVARCHAR,
	[StartDate] DATETIME NOT NULL,
	[EndDate] DATETIME NOT NULL

	PRIMARY KEY ([Id])
);