CREATE PROCEDURE dbo.sp_Upchat_GetInActiveHRList 
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT H.HiringRequestID 
	FROM gen_channelhistory H (NOLOCK) 
	INNER JOIN gen_SalesHiringRequest S (NOLOCK) ON H.HiringRequestID = S.ID
	WHERE S.LastActivityDate < DATEADD(Day, -45, GETDATE())

END
GO
