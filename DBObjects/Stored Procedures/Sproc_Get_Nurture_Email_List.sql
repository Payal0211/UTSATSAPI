ALTER PROCEDURE [dbo].[Sproc_Get_Nurture_Email_List]
AS
BEGIN
		
		DECLARE @GivenMonth INT = Month(Getdate()), @GivenYear INT = Year(Getdate());
		DECLARE @PreMonth INT = null, @PreYear INT = null;

		--Select	top 1 @GivenMonth = [Month],@GivenYear = [Year] 
		--from	gen_NurtureEmail_Summary WITH(NOLOCK)
			
		-- Create a date from the given month and year (assumes the 1st of the month)
		DECLARE @CurrentDate DATE = CAST(CAST(@GivenYear AS VARCHAR(4)) + '-' + CAST(@GivenMonth AS VARCHAR(2)) + '-01' AS DATE);

		-- Get the previous month and year
		SELECT 
			@PreMonth = MONTH(DATEADD(MONTH, -1, @CurrentDate)), 
			@PreYear = YEAR(DATEADD(MONTH, -1, @CurrentDate));


		SELECT  ISNULL(ClientID, 0) AS ClientID,
				ISNULL(CompanyId, 0) AS CompanyId,
				ISNULL(CreatedDateTime, GETDATE()) AS CreatedDateTime,  -- Default to current date if NULL
				ISNULL(HRID, 0) AS HRID,
				ISNULL(TotalJobsPosted, 0) AS TotalJobsPosted,
				ISNULL(TotalJobsForProfileShared, 0) AS TotalJobsForProfileShared,
				ISNULL(TotalJobsExpired, 0) AS TotalJobsExpired,
				ISNULL(TotalJobsExpired15Days, 0) AS TotalJobsExpired15Days,
				ISNULL(TotalJobsRenewal7Days, 0) AS TotalJobsRenewal7Days,
				ISNULL(TotalApplicationsReceived, 0) AS TotalApplicationsReceived,
				ISNULL(TotalCandidatesinAssessment, 0) AS TotalCandidatesinAssessment,
				ISNULL(TotalApplicationswithVideoScreening, 0) AS TotalApplicationswithVideoScreening,
				ISNULL(TotalApplicationswithVideoResume, 0) AS TotalApplicationswithVideoResume,
				ISNULL(InterviewsScheduled, 0) AS InterviewsScheduled,
				ISNULL(TotalCandidatesmovedinInterview, 0) AS TotalCandidatesmovedinInterview,
				ISNULL(ApplicantWithHighestJobScore, '') AS ApplicantWithHighestJobScore,  -- Default to empty string
				ISNULL(AssessmentSharedCount, 0) AS AssessmentSharedCount,
				ISNULL(ApplicantsNotReviewdPast15Days, 0) AS ApplicantsNotReviewdPast15Days,
				ISNULL(Minimummatchscorenonreviewedcandidates, 0) AS Minimummatchscorenonreviewedcandidates,
				ISNULL([MONTH], 0) AS [MONTH],
				ISNULL([YEAR], 0) AS [YEAR]
		From    gen_NurtureEmail_Summary WITH(NOLOCK)
		Where	@PreMonth = [Month] and @PreYear = [YEAR]
END