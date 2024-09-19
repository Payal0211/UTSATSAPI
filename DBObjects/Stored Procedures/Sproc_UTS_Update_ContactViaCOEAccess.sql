CREATE PROCEDURE [dbo].[Sproc_UTS_Update_ContactViaCOEAccess] 
	@HR_ID bigint,
	@ContactID bigint
AS
BEGIN
	
	UPDATE gen_TalentSelected_InterviewerDetails
	SET ContactID = @ContactID
	WHERE HiringRequest_ID = @HR_ID

	UPDATE gen_TalentSelected_InterviewDetails
	SET ContactID = @ContactID
	WHERE HiringRequest_ID = @HR_ID

	UPDATE gen_ShortlistedTalent_InterviewDetails
	SET ContactID = @ContactID
	WHERE HiringRequest_ID = @HR_ID

	UPDATE gen_InterviewSlotsMaster
	SET ContactID = @ContactID
	WHERE HiringRequest_ID = @HR_ID

	UPDATE gen_ShortlistedTalents
	SET ContactID = @ContactID
	WHERE HiringRequest_ID = @HR_ID

	UPDATE gen_OnBoardTalents_ReplacementDetails
	SET ContactID = @ContactID
	WHERE HiringRequestID = @HR_ID

	UPDATE gen_OnBoardTalents
	SET ContactID = @ContactID
	WHERE HiringRequest_ID = @HR_ID

	UPDATE gen_Payout_Information
	SET ContactID = @ContactID
	WHERE HiringRequest_ID = @HR_ID

	UPDATE gen_TalentSelected_NextRound_InterviewDetails
	SET ContactID = @ContactID
	WHERE HiringRequest_ID = @HR_ID

	UPDATE gen_ContactInterviewFeedback
	SET ContactID = @ContactID
	WHERE HiringRequest_ID = @HR_ID

	UPDATE gen_ContactTalentPriority
	SET ContactID = @ContactID
	WHERE HiringRequestID = @HR_ID

	UPDATE gen_SalesHiringRequest_InterviewerDetails
	SET ContactID = @ContactID
	WHERE HiringRequest_ID = @HR_ID
END
GO
