CREATE TABLE [dbo].[Species]
(
	[SpeciesID] INT NOT NULL PRIMARY KEY DEFAULT NEXT VALUE FOR [dbo].[Species_SpeciesID_Sequence],
	[SpeciesName] VARCHAR(50) NOT NULL,
	[Lifestage] INT NOT NULL,
	[IdealPicture] IMAGE NOT NULL,
	[IsPest] BIT NOT NULL,
	[LastModifiedID] INT,
	[TMStamp] DATETIME
)
