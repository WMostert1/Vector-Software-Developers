CREATE TRIGGER [trg_UserStamp]
	ON [dbo].[User]
	AFTER UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		IF UPDATE(TMStamp) OR UPDATE(LastModifiedID) RETURN;
		UPDATE [dbo].[User]
		SET TMStamp = GETDATE(), LastModifiedID = CURRENT_USER
		WHERE UserID IN (SELECT UserID FROM deleted)
	END
