CREATE PROCEDURE [dbo].[sproc_UTS_GetTalentScoreCard]
@TalentID BIGINT = NULL 
AS

declare @totalattempts int
set @totalattempts = (select count(1) from gen_TalentAssesmentScore where TalentID = @TalentID and IsActive = 1)

select ps.Skill as SkillTest,'Pass' as result, gts.SkillScore as Score, @totalattempts as Attempts, 'www.google.com' as Report   
 FROM gen_TalentAssesmentSkill_Scores gts 
 left join prg_Skills ps with(nolock) on ps.ID = gts.AssesmentSkill_ID
where gts.Talent_ID = @TalentID