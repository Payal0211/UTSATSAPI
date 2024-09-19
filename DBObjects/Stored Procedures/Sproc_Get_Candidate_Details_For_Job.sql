CREATE PROCEDURE [dbo].[Sproc_Get_Candidate_Details_For_Job]
	@HRID			bigint = null
AS
BEGIN

			select  TalentName = ISNULL(HRCTP.Talent_Name,''),
					Designation = ISNULL(HRCTP.Talent_Designation,''),
					AISummary = ISNULL(HRCTP.AISummary,''),
					IsVideoResume = Isnull(HRCTP.IsVideoResume,''),
					VideoVetting = ISNULL(HRCTP.VideoVetting,''),
					TalentResume = ISNULL(HRCTP.TalentResume,'')
			from	gen_HR_ATSTalentDetails_CTP HRCTP WITH(NOLOCK)
					inner join gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.ID = HRCTP.CTP_ID
			WHERE	HRCTP.HiringRequest_ID = @HRID

END