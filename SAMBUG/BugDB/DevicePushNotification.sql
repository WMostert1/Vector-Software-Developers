CREATE TABLE [dbo].[DevicePushNotification]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [UserID] BIGINT NOT NULL, 
    [RegID] TEXT NOT NULL, 
    CONSTRAINT [FK_DevicePushNotification_ToUser] FOREIGN KEY ([UserID]) REFERENCES [User]([UserID]), 
)
