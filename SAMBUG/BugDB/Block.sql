CREATE TABLE [dbo].[Block]
(
	[BlockID] INT NOT NULL PRIMARY KEY DEFAULT NEXT VALUE FOR [dbo].[Block_BlockID_Sequence] ,
	[FarmID] INT NOT NULL,
	[BlockName] VARCHAR(50) NOT NULL,
	[LastModifiedID] INT ,
	[TMStamp] DATETIME,
	CONSTRAINT [FK_Farm_ToBlock] FOREIGN KEY (FarmID) REFERENCES [Farm]([FarmID])
)

