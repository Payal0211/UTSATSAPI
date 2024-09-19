ALTER PROCEDURE [dbo].[Sproc_UpdateNurtureLogs]
@ClientID BIGINT = 0,
@CompanyID BIGINT = 0
AS
BEGIN
		
	IF EXISTS(SELECT 1 FROM gen_NurtureEmail_Logs WITH(NOLOCK) WHERE ClientID = @ClientID)
	BEGIN
		UPDATE gen_NurtureEmail_Logs SET 
		LastEmailSentDateTime = GETDATE(),
		EmailSentCount = ISNULL(EmailSentCount,0) + 1
		WHERE ClientID = @ClientID
	END
	ELSE
	BEGIN
		INSERT INTO gen_NurtureEmail_Logs
		(
			ClientID,
			CompanyId,
			CreatedDateTime,
			LastEmailSentDateTime,
			EmailSentCount
			--,SubjectID
		)
		VALUES
		(
			@ClientID,
			@CompanyID,
			GETDATE(),
			GETDATE(),
			1
		)
	END
	
	

END
	
