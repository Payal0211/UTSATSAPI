ALTER Procedure [dbo].[Sproc_Get_HRTalentDetails_ClientPortal]
@HR_ID bigint   = Null
AS
BEGIN

		--select TR.TalentRole,Isnull(T.Fees,'') As Salary,Joining =  Isnull(Tj.Joinning,''),
		--Isnull(T.Name,'') as Name,
		--isnull(TCS.TalentStatus,'') as TalentStatus,
		--PhotoImage = Isnull(T.ProfileURL,''),
		--TimezonePreference = HRD.TimeZone_FromTime + ' To ' + HRD.TimeZone_EndTime,
		--Experience= case when FLOOR(isnull(T.TotalExpYears,0)) <> CEILING(isnull(T.TotalExpYears,0)) then cast(Isnull(floor(isnull(T.TotalExpYears,0)),0) as nvarchar(5))  + '+ Years' else replace( replace( cast(Isnull(floor(isnull(T.TotalExpYears,0)),0) as nvarchar(5)),'.0',''),'.00','') + ' Years' end,
		--PrimarySkills = isnull((STUFF((
		--									select	distinct ', ' +  isnull(ss1.Skill,'')
		--									from	prg_Skills SS1  WITH(NOLOCK) 	
		--											inner join gen_TalentPrimarySkill_Details TPD WITH(NOLOCK) ON TPD.TalentID = T.ID
		--											and TPD.PrimarySkill_ID = SS1.ID
		--									WHERE	isnull(SS1.IsActive,0) =1 AND  ISNULL(TPD.PrimarySkill_ID,0) <> 0
		--									For	XML Path('')),1,1,''
		--								)),'')										
										
		--								+ ', ' + isnull((STUFF((				            
		--									select	distinct ', ' +  isnull(TempSkill,'')
		--									from	prg_TempSkills TS1  WITH(NOLOCK)
		--											inner join gen_TalentPrimarySkill_Details TPD WITH(NOLOCK) ON TPD.TalentID = T.ID
		--											AND TPD.TempSkill_ID = cast(TS1.TempSkill_ID as nvarchar(10))
		--									WHERE   isnull(TS1.Status_ID,0) <> 2 AND  ISNULL(TPD.TempSkill_ID,'') <> ''
		--									For	XML Path('')),1,1,''
		--								)),''),
		--Isnull(isnull(CTP.ATS_Talent_LiveURL,T.ATS_Talent_LiveURL),'') ATSTalentLiveURL,
		--Isnull(isnull(CTP.ATS_Non_NDAURL,T.ATS_Non_NDAURL),'') ATSNonNDAURL
		--							from	gen_SalesHiringRequest HR with(nolock) 
		--					inner join gen_SalesHiringRequest_Details HRD with(nolock) on HR.ID = HRD.HiringRequest_ID
		--					inner join gen_ContactTalentPriority  CTP with(nolock) ON HR.ID = CTP.HiringRequestID 	 AND CTP.ContactID = HR.ContactID AND CTP.HiringRequest_Detail_ID = HRD.ID  	
		--					inner join prg_TalentStatus_AfterClientSelection TCS WITH(Nolock) On TCS.ID = CTP.TalentStatusID_BasedOnHR			
		--					inner join gen_Talent T with(nolock) ON T.ID = CTP.TalentID
		--					inner join prg_TalentRoles TR  with(nolock) ON HRD.Role_ID = TR.ID
		--					left join prg_TalentJoinning TJ WITH(NOLOCK) ON TJ.ID = T.Joining_ID
		--					where HR.ID = @HR_ID



		select
		TalentRole = '',
		Salary = '',
		Joining =  '',
		Name = '',
		TalentStatus = '',
		PhotoImage = '',
		TimezonePreference = '',
		Experience= '',
		PrimarySkills = '',
		ATSTalentLiveURL = '',
		ATSNonNDAURL = '' 
		from	gen_SalesHiringRequest HR with(nolock)  
		where HR.ID = 0
END