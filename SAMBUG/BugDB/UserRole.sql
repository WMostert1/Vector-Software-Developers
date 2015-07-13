CREATE TABLE [dbo].[UserRole]
(
	[UserRoleID] INT NOT NULL PRIMARY KEY DEFAULT NEXT VALUE FOR [dbo].[UserRole_UserRoleID_Sequence],
	[UserID] INT NOT NULL,
	[RoleID] INT NOT NULL,
	CONSTRAINT [FK_UserRole_ToUser] FOREIGN KEY ([UserID]) REFERENCES [User]([UserID]),
	CONSTRAINT [FK_UserRole_ToRole] FOREIGN KEY ([RoleID]) REFERENCES [Role]([RoleID])
)
