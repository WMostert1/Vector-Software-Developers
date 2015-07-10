CREATE TABLE [dbo].[Role]
(
	[RoleID] INT NOT NULL PRIMARY KEY,
	[RoleDescription] VARCHAR(20) NOT NULL,
	[LastModifiedID] INT,
	[TMStamp] DATETIME
)
