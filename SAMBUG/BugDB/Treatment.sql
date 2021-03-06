﻿CREATE TABLE [dbo].[Treatment]
(
	[TreatmentID] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[BlockID] BIGINT NOT NULL,
	[Date] DATETIME NOT NULL,
	[Comments] VARCHAR(500),
	[LastModifiedID] SYSNAME DEFAULT CURRENT_USER NOT NULL,
	[TMStamp] DATETIME DEFAULT GETDATE() NOT NULL	
	CONSTRAINT [FK_Treatment_ToBlock] FOREIGN KEY ([BlockID]) REFERENCES [Block]([BlockID]) ON DELETE CASCADE
)

