CREATE TABLE [dbo].[ScoutBug]
(
	[ScoutBugID] INT NOT NULL PRIMARY KEY DEFAULT NEXT VALUE FOR [dbo].[ScoutBug_ScoutBugID_Sequence],
	[ScoutStopID] INT NOT NULL,
	[SpeciesID] INT NOT NULL,
	[NumberOfBugs] INT NOT NULL,
	[FieldPicture] IMAGE NOT NULL,
	[Comments] VARCHAR(100),
	[LastModifiedID] INT,
	[TMStamp] DATETIME,
	CONSTRAINT [FK_ScoutBug_ToScoutStop] FOREIGN KEY ([ScoutStopID]) REFERENCES [ScoutStop]([ScoutStopID]),
	CONSTRAINT [FK_ScoutBug_ToSpecies] FOREIGN KEY ([SpeciesID]) REFERENCES [Species]([SpeciesID])
)
