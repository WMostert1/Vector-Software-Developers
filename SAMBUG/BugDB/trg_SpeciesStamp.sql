CREATE TRIGGER [trg_ScpeicesStamp]
	ON [dbo].[Species]
	AFTER UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		IF UPDATE(TMStamp) OR UPDATE(LastModifiedID) RETURN;
		UPDATE [dbo].[Species]
		SET TMStamp = GETDATE(), LastModifiedID = CURRENT_USER
		WHERE SpeciesID IN (SELECT SpeciesID FROM deleted)
	END