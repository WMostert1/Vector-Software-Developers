CREATE TABLE [dbo].[ScoutBug]
(
	[ScoutBugID] INT NOT NULL PRIMARY KEY,
	[ScoutStopID] INT NOT NULL,
	[SpeciesID] INT NOT NULL,
	[SpeciesCount] INT NOT NULL,
	[Comments] VARCHAR(100),
	[LastModifiedID] INT,
	[TMStamp] DATETIME,
	CONSTRAINT [FK_ScoutBug_ToScoutStop] FOREIGN KEY ([ScoutStopID]) REFERENCES [ScoutStop]([ScoutStopID]),
	CONSTRAINT [FK_ScoutBug_ToSpecies] FOREIGN KEY ([SpeciesID]) REFERENCES [Species]([SpeciesID])
)
