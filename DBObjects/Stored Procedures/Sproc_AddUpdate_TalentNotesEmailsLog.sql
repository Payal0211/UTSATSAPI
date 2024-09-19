ALTER PROCEDURE [dbo].[Sproc_AddUpdate_TalentNotesEmailsLog]	
	@AtsNoteId			NVARCHAR(500) = 0
AS
BEGIN

	UPDATE gen_TalentNotes_ClientPortal SET IsEmailSentToClient = 1 
	WHERE ATSNoteID = @AtsNoteId

END
