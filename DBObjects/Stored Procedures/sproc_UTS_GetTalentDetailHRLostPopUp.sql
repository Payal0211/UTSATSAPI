ALTER PROCEDURE [dbo].[sproc_UTS_GetTalentDetailHRLostPopUp]
	@HRID BIGINT NULL
AS
BEGIN
IF @HRID > 0 
	BEGIN
		SELECT ISNULL(T.Name,'') TalentName,
			   ISNULL(T.EmailID,'') TalentEmail,
			   ISNULL(TS.TalentStatus,'') TalentStatus 
			   FROM gen_ContactTalentPriority CTP WITH(NOLOCK)
			   INNER JOIN gen_Talent T WITH(Nolock) ON CTP.TalentID = T.ID
			   INNER JOIN prg_TalentStatus_AfterClientSelection TS WITH(NOLOCK) ON CTP.TalentStatusID_BasedOnHR = TS.ID
			   WHERE CTP.HiringRequestID = @HRID  
	END
END