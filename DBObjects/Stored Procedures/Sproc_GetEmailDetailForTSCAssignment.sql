ALTER PROCEDURE [dbo].[Sproc_GetEmailDetailForTSCAssignment]
	@onBoardID bigint = NULL
AS
BEGIN
	IF EXISTS(SELECT 1 FROM gen_OnBoardTalents WITH(NOLOCK) WHERE ID =@onBoardID)
		BEGIN
				SELECT 
				ISNULL(H.HR_Number,'') as HRID,
				ISNULL(OBT.EngagemenID,0)as EngagementID,
				ISNULL(OU.FullName,'') AS OldTSCName,
				ISNULL(OU.EmailId,'') as OldTSCEmail,
				ISNULL(NU.FullName,'') AS NewTSCName,
				ISNULL(NU.EmailId,'') as NewTSCEmail,
				ISNULL(H.RequestForTalent,'') AS Role,
				ISNULL(C.FullName,'') as ClientName
				FROM gen_OnBoardTalents OBT WITH(NOLOCK)
				INNER JOIN gen_saleshiringRequest H WITH(NOLOCK) ON H.ID = OBT.HiringRequest_ID  
				INNER JOIN gen_Contact C WITH(NOLOCK) ON OBT.ContactID = C.ID
				INNER JOIN usr_user NU WITH(NOLOCK) ON OBT.TSC_PersonID = NU.ID
				INNER JOIN usr_user OU WITH(NOLOCK) ON OBT.OldTSC_PersonID = OU.ID
		END
		
END

