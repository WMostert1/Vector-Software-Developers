CREATE TRIGGER [trg_ScoutStopStamp]
	ON [dbo].[ScoutStop]
	AFTER UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		IF UPDATE(TMStamp) OR UPDATE(LastModifiedID) RETURN;
		UPDATE [dbo].[ScoutStop]
		SET TMStamp = GETDATE(), LastModifiedID = CURRENT_USER
		WHERE ScoutStopID IN (SELECT ScoutStopID FROM deleted)
	END