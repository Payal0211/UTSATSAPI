ALTER PROCEDURE [dbo].[Sproc_Update_NurtureEmail_Summary]
	@TotalApplicationsReceived					int = null,
	@TotalCandidatesinAssessment				int = null,
	@TotalApplicationswithVideoScreening 		int = null,
	@TotalApplicationswithVideoResume			int = null,
	@InterviewsScheduled						int = null,
	@TotalCandidatesmovedinInterview			int = null,
	@ApplicantWithHighestJobScore				Nvarchar(Max) = null,
	@AssessmentSharedCount						int = null,
	@ApplicantsNotReviewdPast15Days				int = null,
	@Minimummatchscorenonreviewedcandidates		int = null,
	@CompanyID									bigint = null,
	@ClientID									bigint = null,
	@HRID										bigint = null
AS
BEGIN
			
			Update  gen_NurtureEmail_Summary
			SET		TotalApplicationsReceived = Isnull(@TotalApplicationsReceived,TotalApplicationsReceived),		
					TotalCandidatesinAssessment	= Isnull(@TotalCandidatesinAssessment,TotalCandidatesinAssessment),			
					TotalApplicationswithVideoScreening = Isnull(@TotalApplicationswithVideoScreening,TotalApplicationswithVideoScreening),		
					TotalApplicationswithVideoResume = Isnull(@TotalApplicationswithVideoResume,TotalApplicationswithVideoResume),		
					InterviewsScheduled	= Isnull(@InterviewsScheduled,InterviewsScheduled),					
					TotalCandidatesmovedinInterview	= Isnull(@TotalCandidatesmovedinInterview,TotalCandidatesmovedinInterview),			
					ApplicantWithHighestJobScore = Isnull(@ApplicantWithHighestJobScore,ApplicantWithHighestJobScore),			
					AssessmentSharedCount = Isnull(@AssessmentSharedCount,AssessmentSharedCount),				
					ApplicantsNotReviewdPast15Days = Isnull(@ApplicantsNotReviewdPast15Days,ApplicantsNotReviewdPast15Days),			
					Minimummatchscorenonreviewedcandidates = Isnull(@Minimummatchscorenonreviewedcandidates,Minimummatchscorenonreviewedcandidates)
			where   ClientID = @ClientID and CompanyId = @CompanyID and HRID = @HRID


END