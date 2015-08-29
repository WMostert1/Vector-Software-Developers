CREATE TRIGGER [trg_RoleStamp]
	ON [dbo].[Role]
	AFTER UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		IF UPDATE(TMStamp) OR UPDATE(LastModifiedID) RETURN;
		UPDATE [dbo].[Role]
		SET TMStamp = GETDATE(), LastModifiedID = CURRENT_USER
		WHERE RoleID IN (SELECT RoleID FROM deleted)
	END
