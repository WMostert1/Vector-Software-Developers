CREATE TABLE [dbo].[ScoutStop]
(
	[ScoutStopID] INT NOT NULL PRIMARY KEY DEFAULT NEXT VALUE FOR [dbo].[ScoutStop_ScoutStopID_Sequence],
	[UserID] INT NOT NULL,
	[BlockID] INT NOT NULL,
	[NumberOfTrees] INT NOT NULL,
	[Latitude] REAL NOT NULL,
	[Longitude] REAL NOT NULL,
	[Date] DATETIME NOT NULL,
	[LastModifiedID] INT,
	[TMStamp] DATETIME,
	CONSTRAINT [FK_ScoutStop_ToUser] FOREIGN KEY ([UserID]) REFERENCES [User]([UserID]),
	CONSTRAINT [FK_ScoutStop_ToBlock] FOREIGN KEY ([BlockID]) REFERENCES [Block]([BlockID])
)
