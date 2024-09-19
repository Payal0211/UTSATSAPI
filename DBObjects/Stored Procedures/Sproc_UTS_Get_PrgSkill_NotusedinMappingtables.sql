USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[Sproc_Get_PrgSkill_NotusedinMappingtables]    Script Date: 16-01-2023 11:36:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[Sproc_UTS_Get_PrgSkill_NotusedinMappingtables]    
  @Talent_Id   bigint = null,    
  @HiringRequest_Detail_Id  bigint = null,    
  @Talent_RoleID  int = 0    
     
AS    
BEGIN    
  DECLARE  @Role_ID  int = 0    
  DECLARE  @Hiringrequest_ID bigint = 0    
      
   IF @Talent_Id > 0     
  BEGIN    
    If @Talent_RoleID > 0     
     BEGIN    
      SET @Role_ID = @Talent_RoleID    
     END    
    ELSE    
     BEGIN    
      SELECT @Role_ID = RoleID     
      from gen_Talent WITH(NOLOCK)    
      WHERE ID = @Talent_Id    
     END    
        
    print @Role_ID    
    
        
        
    SELECT  cast(S.ID AS Nvarchar(50)) as ID,S.Skill AS value    
    FROM prg_Skills S WITH(NOLOCK)    
    WHERE not exists(select ID from gen_TalentPrimarySkill_Details TPS WITH(NOLOCK) WHERE TPS.PrimarySkill_ID = s.ID and TPS.TalentID = @Talent_Id)     
      and not exists(select ID from gen_TalentSecondarySkill_Details TSS WITH(NOLOCK) WHERE TSS.SecondarySkill_ID = s.ID and TSS.TalentID = @Talent_Id)    
      and not exists(select ID from prg_Skills_Roles SR WITH(NOLOCK)  WHERE SR.Skill_ID = S.ID and SR.Role_ID = @Role_ID) and S.IsActive = 1    
    union all    
    
    select  TempSkill_ID as ID,TempSkill AS value    
    from prg_TempSkills Temp WITH(NOLOCK)    
    where Status_ID  is null and addedbyTalent = 1 and createdById = @Talent_Id    
      and not exists(select ID from gen_TalentPrimarySkill_Details TPS WITH(NOLOCK) WHERE isnull(TempSkill_ID,'') <> '' AND TPS.TempSkill_ID = Temp.TempSkill_ID and TPS.TalentID = @Talent_Id)     
      and not exists(select ID from gen_TalentSecondarySkill_Details TSS WITH(NOLOCK) WHERE isnull(TempSkill_ID,'') <> '' AND TSS.TempSkill_ID = Temp.TempSkill_ID and TSS.TalentID = @Talent_Id)    
        
    
  END    
  ELSE IF @HiringRequest_Detail_Id > 0     
    BEGIN    
    SELECT @Role_ID = Role_ID,@Hiringrequest_ID =HiringRequest_ID     
    from gen_SalesHiringRequest_Details WITH(NOLOCK)    
    WHERE ID = @HiringRequest_Detail_Id    
    
    SELECT  cast(S.ID AS Nvarchar(50)) ID,S.Skill as value    
    FROM prg_Skills S WITH(NOLOCK)    
    WHERE not exists(select ID from gen_SalesHiringRequest_SkillDetails TPS WITH(NOLOCK) WHERE TPS.Skill_ID = s.ID and TPS.HiringRequest_ID = @Hiringrequest_ID)     
      and not exists(select ID from prg_Skills_Roles SR WITH(NOLOCK)  WHERE SR.Skill_ID = S.ID and SR.Role_ID = @Role_ID)and S.IsActive = 1    
      
    END    
  ELSE IF @Talent_Id = 0 and @HiringRequest_Detail_Id =  0 and @Talent_RoleID > 0    
  BEGIN    
  
  
    SELECT  cast(S.ID AS Nvarchar(50)) as ID,S.Skill as value    
    FROM prg_Skills S WITH(NOLOCK)          
    WHERE not exists(select ID from prg_Skills_Roles SR WITH(NOLOCK)  WHERE SR.Skill_ID = S.ID and SR.Role_ID = @Talent_RoleID)and S.IsActive = 1    
    
  END 
  
  ELSE IF @Talent_Id = 0 and @HiringRequest_Detail_Id =  0 and @Talent_RoleID = 0
  BEGIN

	SELECT  cast(S.ID AS Nvarchar(50)) as ID,S.Skill as value     
    FROM prg_Skills S WITH(NOLOCK) where IsActive = 1;	

  END

END


