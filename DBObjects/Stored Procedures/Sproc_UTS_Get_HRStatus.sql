-- =============================================
-- Author:		Himani Sawadiawala
-- Create date: 5 April 2024
-- Description:	Sproc_UTS_Get_HRStatus
-- =============================================
--EXEC [dbo].[Sproc_UTS_Get_HRStatus] 1840
ALTER PROCEDURE [dbo].[Sproc_UTS_Get_HRStatus] 
(
    @HRID BIGINT,
	@CompanyID BIGINT = NULL
)
AS
BEGIN

    IF(isnull(@CompanyID,0) = 0)
	BEGIN
		print ' if isnull(@CompanyID,0) = 0 '

						SELECT 
						H.ID AS HRID,
						H.HR_Number AS HR_Number,
						case 
						WHEN  ISNULL(H.IsActive,0) = 0 and H.JobStatusID = 3 then JS.JobStatus --'Draft' 
						WHEN  ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 1 AND H.JobStatusID = 4 then JS.JobStatus --'Open' 
						WHEN  ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 1 AND H.JobStatusID = 1 then JS.JobStatus --'Active' 
						When  ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 2 AND H.JobStatusID = 1 then JS.JobStatus  --'Active' (in process, after profile shared)  
						WHEN  H.JobStatusID = 2 AND H.Status_ID = 3 then JS.JobStatus + ' - ' + S.HiringRequest_Status --Closed - Won
						WHEN  H.JobStatusID = 2 AND H.Status_ID = 4 then JS.JobStatus + ' - ' + S.HiringRequest_Status --Closed - Cancelled
						WHEN  H.JobStatusID = 2 AND H.Status_ID = 6 then JS.JobStatus + ' - ' + S.HiringRequest_Status --Closed - Lost
						WHEN  H.JobStatusID = 2 AND H.Status_ID = 9 then JS.JobStatus + ' - ' + S.HiringRequest_Status --Closed - Expired
						WHEN  H.JobStatusID = 1 AND H.Status_ID = 7 then S.HiringRequest_Status --Active - but no longer accepting applicaitons
						WHEN  H.JobStatusID = 1 AND H.Status_ID = 8 then JS.JobStatus + ' - ' + S.HiringRequest_Status --Active - Reposted
						WHEN  H.JobStatusID = 5 then JS.JobStatus --Re-Open
					ELSE
						JS.JobStatus
					END AS HRStatus,
				case 
					WHEN ISNULL(H.IsActive,0) = 0 and H.JobStatusID = 3 THEN 101 --'Draft' 
					WHEN ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 1 AND H.JobStatusID = 4 THEN 102  --'Open'
					WHEN ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 1 AND H.JobStatusID = 1 THEN 106  --'Active' 
					WHEN ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 2 AND H.JobStatusID = 1 THEN 106  --'Active' (in process, after profile shared)  
					WHEN  H.JobStatusID = 2 AND H.Status_ID = 3 THEN 105 --Closed - Won
					WHEN  H.JobStatusID = 2 AND H.Status_ID = 4 THEN 108 --Closed - Cancelled
					WHEN  H.JobStatusID = 2 AND H.Status_ID = 6 THEN 109 --Closed - Lost
					WHEN  H.JobStatusID = 2 AND H.Status_ID = 9 then 103 --Closed - Expired
					WHEN  H.JobStatusID = 1 AND H.Status_ID = 7 THEN 107 --Active - but no longer accepting applicaitons
					WHEN  H.JobStatusID = 1 AND H.Status_ID = 8 THEN 104 --Active - Reposted
					WHEN  H.JobStatusID = 5 then 110 --Re-Open
				ELSE
					201 
				END AS HRStatusCode 
				FROM	gen_SalesHiringRequest H WITH(NOLOCK)
				INNER JOIN prg_JobStatus_ClientPortal JS WITH(NOLOCK) ON JS.ID = H.JobStatusID
				LEFT JOIN prg_HiringRequestStatus S WITH(NOLOCK) ON S.ID = H.Status_ID
				WHERE H.ID = CASE WHEN ISNULL(@HRID,0) = 0 THEN H.ID ELSE @HRID END 

    END
	ELSE IF( @CompanyID >0)
	BEGIN
			print 'ELSE IF( @CompanyID >0) '
			SELECT 
						H.ID AS HRID,
						H.HR_Number AS HR_Number,
						case 
						WHEN  ISNULL(H.IsActive,0) = 0 and H.JobStatusID = 3 then JS.JobStatus --'Draft' 
						WHEN  ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 1 AND H.JobStatusID = 4 then JS.JobStatus --'Open' 
						WHEN  ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 1 AND H.JobStatusID = 1 then JS.JobStatus --'Active' 
						When  ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 2 AND H.JobStatusID = 1 then JS.JobStatus  --'Active' (in process, after profile shared)  
						WHEN  H.JobStatusID = 2 AND H.Status_ID = 3 then JS.JobStatus + ' - ' + S.HiringRequest_Status --Closed - Won
						WHEN  H.JobStatusID = 2 AND H.Status_ID = 4 then JS.JobStatus + ' - ' + S.HiringRequest_Status --Closed - Cancelled
						WHEN  H.JobStatusID = 2 AND H.Status_ID = 6 then JS.JobStatus + ' - ' + S.HiringRequest_Status --Closed - Lost
						WHEN  H.JobStatusID = 2 AND H.Status_ID = 9 then JS.JobStatus + ' - ' + S.HiringRequest_Status --Closed - Expired
						WHEN  H.JobStatusID = 1 AND H.Status_ID = 7 then S.HiringRequest_Status --Active - but no longer accepting applicaitons
						WHEN  H.JobStatusID = 1 AND H.Status_ID = 8 then JS.JobStatus + ' - ' + S.HiringRequest_Status --Active - Reposted
						WHEN  H.JobStatusID = 5 then JS.JobStatus --Re-Open
					ELSE
						JS.JobStatus
					END AS HRStatus,
				case 
					WHEN ISNULL(H.IsActive,0) = 0 and H.JobStatusID = 3 THEN 101 --'Draft' 
					WHEN ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 1 AND H.JobStatusID = 4 THEN 102  --'Open'
					WHEN ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 1 AND H.JobStatusID = 1 THEN 106  --'Active' 
					WHEN ISNULL(H.IsActive,0) = 1 AND H.Status_ID = 2 AND H.JobStatusID = 1 THEN 106  --'Active' (in process, after profile shared)  
					WHEN  H.JobStatusID = 2 AND H.Status_ID = 3 THEN 105 --Closed - Won
					WHEN  H.JobStatusID = 2 AND H.Status_ID = 4 THEN 108 --Closed - Cancelled
					WHEN  H.JobStatusID = 2 AND H.Status_ID = 6 THEN 109 --Closed - Lost
					WHEN  H.JobStatusID = 2 AND H.Status_ID = 9 then 103 --Closed - Expired
					WHEN  H.JobStatusID = 1 AND H.Status_ID = 7 THEN 107 --Active - but no longer accepting applicaitons
					WHEN  H.JobStatusID = 1 AND H.Status_ID = 8 THEN 104 --Active - Reposted
					WHEN  H.JobStatusID = 5 then 110 --Re-Open
				ELSE
					201 
				END AS HRStatusCode 
				FROM	gen_SalesHiringRequest H WITH(NOLOCK)
				inner join gen_Contact C WITH(nolock) ON H.ContactID = C.ID
				
				INNER JOIN prg_JobStatus_ClientPortal JS WITH(NOLOCK) ON JS.ID = H.JobStatusID
				INNER JOIN prg_HiringRequestStatus S WITH(NOLOCK) ON S.ID = H.Status_ID
				WHERE C.CompanyID = @CompanyID
	END


END


