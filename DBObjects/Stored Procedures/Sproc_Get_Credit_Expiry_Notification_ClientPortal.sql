USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[Sproc_Get_Credit_Expiry_Notification_ClientPortal]    Script Date: 09-04-2024 12:37:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Sproc_Get_Credit_Expiry_Notification_ClientPortal]
  @CompanyID		bigint = 0
AS
BEGIN

		--IF OBJECT_ID('tempdb..#ExpiryNotification') IS not null
		--	DROP TABLE #ExpiryNotification

		DECLARE @ExpiryNotification TABLE
		(
			HRID		  bigint,
			CompanyID	  bigint,
			ContactID	  bigint,
			CreditBalance	int,
			Title		  Nvarchar(100),
			HR_Number	  Nvarchar(100),
			JobPostedDate Nvarchar(20),
			DaysPending	  int
		)

		select ContactID,MAX(JCT.ID) AS ID, HRID
		Into   #MaxHRID
		FROM   gen_JobPost_TransactionHistory_ClientPortal JCT WITH(NOLOCK)		
		INNER JOIN gen_Contact GC WITH(NOLOCK) ON GC.ID = JCT.ContactID
		WHERE GC.CompanyID = @CompanyID

		--WHERE JobRepostedDate IS NOT NULL 
		--AND IsReposted = 0 
		--AND MONTH(JobRepostedDate) <> MONTH(GETDATE()) 
		--AND YEAR(JobRepostedDate) <> YEAR(GETDATE())
		GROUP BY ContactID, HRID 

		INSERT INTO @ExpiryNotification
		select  JCT.HRID,JCT.CompanyID,JCT.ContactID,CreditBalance = Isnull(JCT.CreditBalance,0),
				H.RequestForTalent,H.HR_Number,JobPostedDate = CONVERT(nvarchar(15),ISNULL(JCT.JobRepostedDate,JCT.JobPostedDate),103),
				--DATEADD (d, 30, ISNULL(JCT.JobRepostedDate,JCT.JobPostedDate)),				
				DaysPending = abs(DateDIff(d, CAST(DATEADD (d, 30, ISNULL(JCT.JobRepostedDate,JCT.JobPostedDate)) as DAte),CAST(GETDATE() as DATE)))
		FROM    gen_JobPost_TransactionHistory_ClientPortal JCT WITH(NOLOCK)
			    --inner join gen_JobPost_Subscription_History_ClientPortal JS WITH(NOLOCK) ON JS.CompanyID = JCT.CompanyID --and JS.ContactID = JCT.ContactID
				inner join gen_SalesHiringRequest H WITH(NOLOCK) ON H.ID = JCT.HRID
				inner join #MaxHRID M ON M.HRID = JCT.HRID AND M.ID = JCT.ID
				--inner join gen_Company CO WITH(NOLOCK) ON CO.ID = JCT.CompanyID
		WHERE   JCT.CompanyID = @CompanyID
			    AND DateDIff(d,CAST(GETDATE() as DATE), CAST(DATEADD (d, 30, ISNULL(JCT.JobRepostedDate,JCT.JobPostedDate)) as DAte)) <= 5 
				--AND DateDIff(d,CAST(GETDATE() as DATE), CAST(DATEADD (d, 30, ISNULL(JCT.JobRepostedDate,JCT.JobPostedDate)) as DAte)) > 0
				AND H.Status_ID in (1,2) 
				
				--AND NOT EXISTS(SELECT 1 FROM gen_JobPost_TransactionHistory_ClientPortal TH WITH(NOLOCK) WHERE TH.IsReposted = 1 AND TH.ID = M.ID)
				--AND CO.CompanyTypeID = 2

		SELECT 
			HRID,
			CompanyID,
			ContactID,
			CreditBalance,
			Title,
			HR_Number,
			JobPostedDate,
			DaysPending		
		FROM @ExpiryNotification	
		
END