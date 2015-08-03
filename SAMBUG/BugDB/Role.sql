CREATE TABLE [dbo].[Role]
(
	[RoleID] BIGINT NOT NULL PRIMARY KEY  IDENTITY,
	[RoleType] INT NOT NULL,
	[RoleDescription] VARCHAR(50) NOT NULL,
	[LastModifiedID] INT,
	[TMStamp] DATETIME
)
