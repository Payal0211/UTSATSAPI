
CREATE PROCEDURE Sproc_UTS_DeleteTestHR
    @HR_ID as bigint = null
AS
BEGIN
	
	IF(@HR_ID > 0)
	BEGIN
	     print @HR_ID
		    delete from gen_OnBoardTalents_ReplacementDetails where HiringRequestID = @HR_ID

			delete  from gen_History where HiringRequest_ID = @HR_ID --AND Talent_ID in(31662)    order by ID  --AND Talent_ID in(31662) 
			delete  from gen_OnBoardTalents where  HiringRequest_ID = @HR_ID
			delete  from gen_OnBoardClientContractDetails where OnBoardID in(Select ID  from gen_OnBoardTalents WITH(NOLOCK) where  HiringRequest_ID = @HR_ID)
			delete  from gen_Payout_Information where HiringRequest_ID = @HR_ID  AND OnBoardID in(Select ID  from gen_OnBoardTalents WITH(NOLOCK) where Status_ID =1 AND HiringRequest_ID = @HR_ID)
			delete  from gen_UTS_OnBoard_LineItem where UTS_GUID in(Select GUID  from gen_OnBoardTalents WITH(NOLOCK) where Status_ID =1 AND HiringRequest_ID = @HR_ID)


			delete  from gen_SalesHiringRequest_InterviewerDetails where HiringRequest_ID = @HR_ID

			delete  from gen_TalentSelected_InterviewerDetails where HiringRequest_ID = @HR_ID
			delete  from gen_TalentSelected_NextRound_InterviewDetails where HiringRequest_ID = @HR_ID

			delete  from gen_ContactInterviewFeedback where HiringRequest_ID = @HR_ID
			delete  from gen_TalentInterviewFeedback where HiringRequest_ID = @HR_ID


			delete  from gen_TalentSelected_PostAcceptance_NotMatchDetails where HiringRequest_ID = @HR_ID
			delete  from gen_TalentSelected_PostAcceptanceDetails where HiringRequest_ID = @HR_ID

			delete  from gen_TalentSelected_InterviewDetails where HiringRequest_ID = @HR_ID
			delete  from gen_ShortlistedTalent_InterviewDetails where HiringRequest_ID = @HR_ID
			delete  from gen_InterviewSlotsMaster where HiringRequest_ID = @HR_ID
			delete  from gen_ShortlistedTalents where HiringRequest_ID  = @HR_ID



			delete  from gen_ContactTalentPriority where HiringRequestID = @HR_ID

			delete  from gen_SalesHiringRequest_SkillDetails where HiringRequest_ID = @HR_ID
			delete  from gen_SalesHiringRequest_InterviewerDetails where HiringRequest_ID = @HR_ID


			delete  from gen_SalesHiringRequest_SkillDetails where HiringRequest_ID = @HR_ID
			delete from gen_SalesHiringRequest_Details where HiringRequest_ID = @HR_ID
			delete from gen_SalesHiringRequest where ID  = @HR_ID
	END

END
GO
