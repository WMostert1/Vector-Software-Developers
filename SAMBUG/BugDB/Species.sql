CREATE TABLE [dbo].[Species]
(
	[SpeciesID] INT NOT NULL PRIMARY KEY,
	[SpeciesName] VARCHAR(50) NOT NULL,
	[IsPest] BIT NOT NULL,
	[LastModifiedID] INT,
	[TMStamp] DATETIME
)
