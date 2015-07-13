CREATE TABLE [dbo].[Treatment]
(
	[TreatmentID] INT NOT NULL PRIMARY KEY DEFAULT NEXT VALUE FOR [dbo].[Treatment_TreatmentID_Sequence],
	[BlockID] INT NOT NULL,
	[Date] DATETIME NOT NULL,
	[Comments] VARCHAR(100),
	[LastModifiedID] INT,
	[TMStamp] DATETIME,
	CONSTRAINT [FK_Treatment_ToBlock] FOREIGN KEY ([BlockID]) REFERENCES [Block]([BlockID])
)
