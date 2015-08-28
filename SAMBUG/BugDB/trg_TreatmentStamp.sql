CREATE TRIGGER [trg_TreatmentStamp]
	ON [dbo].[Treatment]
	AFTER UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		IF UPDATE(TMStamp) OR UPDATE(LastModifiedID) RETURN;
		UPDATE [dbo].[Treatment]
		SET TMStamp = GETDATE(), LastModifiedID = CURRENT_USER
		WHERE TreatmentID IN (SELECT TreatmentID FROM deleted)
	END