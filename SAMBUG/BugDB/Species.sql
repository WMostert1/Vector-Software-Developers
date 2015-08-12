CREATE TABLE [dbo].[Species]
(
	[SpeciesID] BIGINT NOT NULL PRIMARY KEY IDENTITY ,
	[SpeciesName] VARCHAR(50) NOT NULL,
	[Lifestage] INT NOT NULL,
	[IdealPicture] IMAGE NOT NULL,
	[IsPest] BIT NOT NULL,
	[LastModifiedID] INT,
	[TMStamp] DATETIME
)
