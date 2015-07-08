CREATE TABLE [dbo].[ScoutStop]
(
	[ScoutStopID] INT NOT NULL PRIMARY KEY,
	[UserID] INT NOT NULL,
	[BlockID] INT NOT NULL,
	[TreeAmount] INT NOT NULL,
	[Geolocation] VARCHAR(30) NOT NULL,
	[Date] DATETIME NOT NULL,
	[LastModifiedID] INT,
	[TMStamp] DATETIME,
	CONSTRAINT [FK_ScoutStop_ToUser] FOREIGN KEY ([UserID]) REFERENCES [User]([UserID]),
	CONSTRAINT [FK_ScoutStop_ToBlock] FOREIGN KEY ([BlockID]) REFERENCES [Block]([BlockID])
)
