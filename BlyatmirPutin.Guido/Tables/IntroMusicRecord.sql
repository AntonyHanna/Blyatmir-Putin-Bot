 CREATE TABLE [dbo].[IntroMusicRecord] (
	 [Id] INT IDENTITY,
	 [UserId] BIGINT,
	 [IntroId] INT,
	 [DateSet] DATETIME,
	 [DateUnset] DATETIME

	 PRIMARY KEY ([Id])
 );
