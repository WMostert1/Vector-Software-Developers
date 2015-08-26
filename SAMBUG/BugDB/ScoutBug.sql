CREATE TABLE [dbo].[ScoutBug]
(
	[ScoutBugID] BIGINT NOT NULL PRIMARY KEY IDENTITY ,
	[ScoutStopID] BIGINT NOT NULL,
	[SpeciesID] BIGINT NOT NULL,
	[NumberOfBugs] INT NOT NULL,
	[FieldPicture] VARBINARY(MAX) NOT NULL,
	[Comments] VARCHAR(100),
	[LastModifiedID] INT,
	[TMStamp] DATETIME,
	CONSTRAINT [FK_ScoutBug_ToScoutStop] FOREIGN KEY ([ScoutStopID]) REFERENCES [ScoutStop]([ScoutStopID]),
	CONSTRAINT [FK_ScoutBug_ToSpecies] FOREIGN KEY ([SpeciesID]) REFERENCES [Species]([SpeciesID])
)
