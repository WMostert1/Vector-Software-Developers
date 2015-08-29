CREATE TRIGGER [trg_BlockStamp]
	ON [dbo].[Block]
	AFTER UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		IF UPDATE(TMStamp) OR UPDATE(LastModifiedID) RETURN;
		UPDATE [dbo].[Block]
		SET TMStamp = GETDATE(), LastModifiedID = CURRENT_USER
		WHERE BlockID IN (SELECT BlockID FROM deleted)
	END
