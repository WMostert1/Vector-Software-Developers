CREATE TABLE [dbo].[Farm]
(
	[FarmID] INT NOT NULL PRIMARY KEY DEFAULT NEXT VALUE FOR [dbo].[Farm_FarmID_Sequence],
	[UserID] INT NOT NULL,
	[FarmName] VARCHAR(50) NOT NULL,
	[LastModifiedID] INT NULL,
	[TMStamp] DATETIME, 
    CONSTRAINT [FK_Farm_ToUser] FOREIGN KEY (UserID) REFERENCES [User]([UserID]) 
)
