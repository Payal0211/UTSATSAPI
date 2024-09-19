CREATE PROCEDURE [dbo].[Sproc_AfterInterviewCancel_TalentAsProfileShared]
	@HRID BIGINT = NULL,
	@TalentID BIGINT = NULL,
	@LoginUserID INT = NULL
AS
BEGIN	

	UPDATE dbo.gen_ContactTalentPriority SET
	TalentStatusID_BasedOnHR = 2,
	ModifiedByID = @LoginUserID,
	ModifiedByDatetime = GETDATE()
	WHERE HiringRequestID = @HRID AND TalentID = @TalentID

END