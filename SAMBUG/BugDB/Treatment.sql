CREATE TABLE [dbo].[Treatment]
(
	[TreatmentID] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BlockID] BIGINT NOT NULL,
	[Date] DATETIME NOT NULL,
	[Comments] VARCHAR(100),
	[LastModifiedID] INT,
	[TMStamp] DATETIME,
	CONSTRAINT [FK_Treatment_ToBlock] FOREIGN KEY ([BlockID]) REFERENCES [Block]([BlockID])
)

