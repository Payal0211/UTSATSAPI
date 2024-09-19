ALTER PROCEDURE [dbo].[Sproc_FetchSimilarRoles_ClientPortal]
  @RoleName					Nvarchar(200) = 0
AS
BEGIN
	--DECLARE @RoleId BIGINT = 0
	SELECT  
				FT_TBL.* 
				FROM    prg_TalentRoles AS FT_TBL with(nolock)  
						INNER JOIN FREETEXTTABLE(prg_TalentRoles,TalentRole,@RoleName,LANGUAGE N'English') AS KEY_TBL  
						ON FT_TBL.ID = KEY_TBL.[KEY] ORDER BY RANK desc

	--SELECT @RoleId

END