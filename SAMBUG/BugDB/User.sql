CREATE TABLE [dbo].[User]
(
	[UserID] INT NOT NULL PRIMARY KEY DEFAULT NEXT VALUE FOR [dbo].[User_UserID_Sequence],
	[Email] VARCHAR(50) NOT NULL,
	[Password] VARCHAR(20) NOT NULL,
	[LastModifiedID] INT NULL,
	[TMStamp] DATETIME	
)
