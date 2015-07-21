CREATE TABLE [dbo].[Role]
(
	[RoleID] INT NOT NULL PRIMARY KEY DEFAULT NEXT VALUE FOR [dbo].[Role_RoleID_Sequence],
	[RoleType] INT NOT NULL,
	[RoleDescription] VARCHAR(50) NOT NULL,
	[LastModifiedID] INT,
	[TMStamp] DATETIME
)
