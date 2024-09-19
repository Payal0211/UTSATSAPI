CREATE Procedure [dbo].[Sproc_Get_RejectTalentPopUpDetail]--250,1033
@HRDetail_ID bigint = NULL,
@TalentID bigint = NULL
AS
BEGIN
DECLARE @HR_ID BIGINT = NULL
		iF(@HRDetail_ID>0)
		BEGIN
			SELECT @HR_ID = HiringRequest_ID FROM gen_SalesHiringRequest_Details WITH(NOLOCK) WHERE ID = @HRDetail_ID
		END
		IF(@HR_ID>0 AND @TalentID>0)
		BEGIN
			SELECT ISNULL(T.Name,'') AS TalentName,
				   ISNULL(TS.AccountStatus,'') AS TalentStatus,
				   ISNULL(CTP.ID,0) AS ContactPriorityId,
				   ISNULL(@HRDetail_ID,0) AS HiringRequest_Detail_ID,
				   ISNULL(@TalentID,0) AS TalentID
				   FROM gen_ContactTalentPriority CTP WITH(NOLOCK) 
			INNER JOIN gen_Talent T WITH(NOLOCK) ON CTP.TalentID = T.ID
			INNER JOIN prg_TalentAccountStatus TS WITH(NOLOCK) ON TS.ID = T.TalentStatusID_AfterClientSelection
			WHERE CTP.TalentID = @TalentID AND CTP.HiringRequestID = @HR_ID
		END
END
