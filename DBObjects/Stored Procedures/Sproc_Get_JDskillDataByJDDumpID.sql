Create PROCEDURE [dbo].[Sproc_Get_JDskillDataByJDDumpID]  
 @JDDumpID    bigint
AS  
BEGIN  
  
	SELECT DISTINCT g1.JDSkills FROM
	dbo.gen_SalesHR_JDDump  g1  with(nolock)
	inner join 
	dbo.gen_SalesHR_JDDump_SkillDetails g2 with(nolock)  on g1.Id=g2.JDDump_ID WHERE g1.Id = @JDDumpID
  
END  
