CREATE TABLE [dbo].[ScoutStop]
(
	[ScoutStopID] BIGINT NOT NULL PRIMARY KEY IDENTITY ,
	[UserID] BIGINT NOT NULL,
	[BlockID] BIGINT NOT NULL,
	[NumberOfTrees] INT NOT NULL,
	[Latitude] REAL NOT NULL,
	[Longitude] REAL NOT NULL,
	[Date] DATETIME NOT NULL,
	[LastModifiedID] INT,
	[TMStamp] DATETIME,
	CONSTRAINT [FK_ScoutStop_ToUser] FOREIGN KEY ([UserID]) REFERENCES [User]([UserID]),
	CONSTRAINT [FK_ScoutStop_ToBlock] FOREIGN KEY ([BlockID]) REFERENCES [Block]([BlockID])
)
