CREATE TRIGGER [trg_ScoutBugStamp]
	ON [dbo].[ScoutBug]
	AFTER UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		IF UPDATE(TMStamp) OR UPDATE(LastModifiedID) RETURN;
		UPDATE [dbo].[ScoutBug]
		SET TMStamp = GETDATE(), LastModifiedID = CURRENT_USER
		WHERE ScoutBugID IN (SELECT ScoutBugID FROM deleted)
	END