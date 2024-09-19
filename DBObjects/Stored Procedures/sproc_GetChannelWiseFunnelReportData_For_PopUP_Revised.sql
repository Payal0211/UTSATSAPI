
ALTER PROCEDURE [dbo].[sproc_GetChannelWiseFunnelReportData_For_PopUP_Revised]
  @AdHocType			nVarchar(10) = null,
  @TeamManagerId		bigint = null,
  @StageName			nvarchar(100) = null,
  @From_Date1			nvarchar(20) = null,
  @To_date1				nvarchar(20) = null,
  @From_Date2			nvarchar(20) = null,
  @To_date2				nvarchar(20) = null, 
  @Option				bit  = null,  -- to See HR created by datetime wise or to see actual action time wise data
  @ModeOfWorkId			nvarchar(5)  = NULL,
  @Hr_No				Nvarchar(200) = null,	
  @SalesPerson			Nvarchar(200) = null,
  @CompnayName			Nvarchar(200) = null,
  @Role					Nvarchar(200) = null,
  @Managed_Self			Nvarchar(200) = null,
  @Availability			Nvarchar(200) = null,
  @TalentName			Nvarchar(200) = null,
  @IsHiringNeed			Nvarchar(200) = null,
  @TypeOfHR				nvarchar(5)  = NULL,
  @CompanyCategory      nvarchar(5)  = NULL,
  @IsReplacement		bit = NULL,
  @Heads                nvarchar(100) = NULL,
  @LeadUserID		    bigint			= 0,   -- To filter Record based on Lead User (Inbound/Outbound)
  @IsHRFocused			bit				= 0,
  @Geos	     			nvarchar(20)  = NULL
AS
BEGIN

		IF @IsHRFocused != 1
			SET @IsHRFocused = NULL;

		--Option  = 1 Then Display Action Wise.
		--Option = 0 Then display HR Date Wise.
		Declare @AdHocTypeVal as int
		DECLARE @WhereClause As Nvarchar(MAX) = '1=1'
		DECLARE @MainSQL nvarchar(max) = ''
		DECLARE @IsHRTypeDP bit 
		declare @ModeOfWork nvarchar(50) = ''
		DECLARE @GeoIDs int = 0
		DECLARE @Geo nvarchar(max) = null
		Declare @MainPArentId  as bigint

		IF @Geos = '-1'
			SET @Geos = null

		IF IsNULL(@Hr_No,'') <> ''
			SET @WhereClause += ' AND HR_No LIKE ''%' + CONVERT(nvarchar(200),@Hr_No) + '%'''
		IF IsNULL(@SalesPerson,'') <> ''
			SET @WhereClause += ' AND SalesPerson LIKE ''%' + CONVERT(nvarchar(200),@SalesPerson) + '%'''
		IF IsNULL(@CompnayName,'') <> ''
			SET @WhereClause += ' AND CompnayName LIKE ''%' + CONVERT(nvarchar(200),@CompnayName) + '%'''
		IF IsNULL(@Role,'') <> ''
			SET @WhereClause += ' AND Role LIKE ''%' + CONVERT(nvarchar(200),@Role) + '%'''
		IF IsNULL(@Managed_Self,'') <> ''
			SET @WhereClause += ' AND Managed_Self LIKE ''%' + CONVERT(nvarchar(200),@Managed_Self) + '%'''
		IF IsNULL(@Availability,'') <> ''
			SET @WhereClause += ' AND Availability LIKE ''%' + CONVERT(nvarchar(200),@Availability) + '%'''
		IF IsNULL(@TalentName,'') <> ''
			SET @WhereClause += ' AND TalentName LIKE ''%' + CONVERT(nvarchar(200),@TalentName) + '%'''

		if @AdHocType = 'Pool'
		   Set @AdHocTypeVal = 0
		else if @AdHocType = 'Odr'
		   Set @AdHocTypeVal = 1
		 else 
			 Set @AdHocTypeVal = 2   --for Total		

		IF (@TypeOfHR = '' OR @TypeOfHR = '-1')
		BEGIN 
			SET @IsHRTypeDP = NULL;
		END
		ELSE
		BEGIN
			SET @IsHRTypeDP = @TypeOfHR;
		END

		IF (@ModeOfWorkId !='' or @ModeOfWorkId !='0')
			BEGIN
				set @ModeOfWork = (SELECT ModeOfWorking FROM prg_ModeOfWorking with(nolock) where ID = @ModeOfWorkId)
			END

		IF OBJECT_ID('tempdb..#GeoTbl') Is not null
			DROP TABLE #GeoTbl

		CREATE TABLE #GeoTbl(GeoID int)

		IF (Isnull(@Geos,'') != '')
			BEGIN
				If Lower(@Geos) = 'india'
				  Select @Geo = cast(ID as nvarchar(50)) from prg_GEO WITH(NOLOCK) where Lower(GEO) = Lower(@Geos)
				Else
				  Select  @Geo = stuff((select ',' + cast(ID as nvarchar(max)) from prg_GEO WITH(NOLOCK) where Lower(Geo) <> 'india' FOR XML PATH ('')), 1, 1, '')
			
				 Insert into #GeoTbl
				 select val from f_split(@Geo,',');
			END
		ELSE
			BEGIN
				SET @Geos = ''
			END

		

		IF @CompanyCategory = ''
		BEGIN 
			SET @CompanyCategory = NULL;
		END

		PRINT @ModeOfWork;
		PRINT @IsHRTypeDP

		DECLARE @LeadUser_UserType BIGINT = 0;

		--User Type ID: 11 is OutBound (BDR)
		--User Type ID: 12 is InBound (MDR/Marketing)
		IF @LeadUserID <> 0
			SET @LeadUser_UserType = ISNULL((SELECT UserTypeID from usr_User with(nolock) where ID = @LeadUserID),0);

		IF OBJECT_ID ('tempdb..#ChannelReportPopUpDetail') is not null
			DRop TABLE #ChannelReportPopUpDetail

		CREATE TABLE #ChannelReportPopUpDetail
		(
			HR_No			Nvarchar(50),
			SalesPerson		Nvarchar(100),
			CompnayName		Nvarchar(100),
			Role			Nvarchar(100),
			Managed_Self    Nvarchar(100),
			TalentName		Nvarchar(500),
			Availability	NVarchar(100) default '',
			HRStatus		NVarchar(100) default ''
		)

		IF OBJECT_ID('tempdb..#AllProfileSharedUsers') Is not null
			DROP TABLE #AllProfileSharedUsers

		IF OBJECT_ID('tempdb..##HeadTempTbl2') IS NOT NULL
				DROP TABLE #HeadTempTbl2;

		--IF OBJECT_ID('tempdb..#HeadTblHrchy') IS NOT NULL
		--		DROP TABLE #HeadTblHrchy;

		CREATE TABLE #HeadTempTbl2(HeadID bigint);
		--CREATE TABLE #HeadTblHrchy(UserID bigint, ParentID bigint);

		IF @Heads <> '' and @TeamManagerId = 0
		BEGIN
			INSERT INTO #HeadTempTbl2(HeadID)
				select val from f_split(@Heads,',');

			--INSERT INTO #HeadTblHrchy(UserID, ParentID)
			--	select UH.UserID, UH.ParentID from usr_UserHierarchy UH with(nolock)
			--	INNER JOIN #HeadTempTbl2 H on UH.ParentID = H.HeadID;
		END
		ELSE
		BEGIN
			--Get All Head Level User of Demand Dept.
			INSERT INTO #HeadTempTbl2(HeadID)
				SELECT distinct U.ID from usr_User U with(nolock) where U.IsActive = 1 and U.DeptID = 1 and U.LevelID = 1 and U.ID = CASE WHEN @TeamManagerId > 0 THEN @TeamManagerId ELSE U.ID END;
		END	
		
		BEGIN -- Payal (04-03-2024) (Remove Test Companies Using Configuration)

			     IF OBJECT_ID('tempdb..#TempRemoveCompanies') IS NOT NULL
					DROP TABLE #TempRemoveCompanies;

				Declare @Values as Nvarchar(Max) = ''

				select  @Values = [Value]  
				from   gen_SystemConfiguration WITH(NOLOCK)
				where  [Key]  ='RemoveCompaniesforUTSAdminReports'

				select  cast(val as bigint) as RemoveCompanyID into #TempRemoveCompanies from dbo.f_Split(@Values,',')

		END

		CREATE TABLE #AllProfileSharedUsers
		(
			UserId			  int  default 0,
			HiringRequestID	  bigint default 0,
			Talent_ID		  bigint,
			IsODR			  bit,
			IsReplacement	  bit
		)

		Insert Into #AllProfileSharedUsers(HiringRequestID, Talent_ID, IsODR,IsReplacement)
		Select 
		distinct	His.HiringRequest_ID,His.Talent_ID,0 as IsODR,IsNull(His.IsReplacement,0)
		from		gen_History His With(NOlock) 
					--INNER JOIN gen_ContactTalentPriority CTP with(nolock) on His.HiringRequest_ID = CTP.HiringRequestID and His.Talent_ID = CTP.TalentID
					--inner join usr_User U with(nolock) on CTP.CreatedByID = U.ID 
					--INNER join gen_Contact C with(Nolock) On C.ID = CTP.ContactID
					--INNER join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
		Where		His.Action_ID = 6
					and Isnull(His.IsReplacement,0) = Isnull(@IsReplacement,Isnull(His.IsReplacement,0)) --and CTP.TalentStatusID_BasedOnHR != 5
					--AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
		


		IF OBJECT_ID('tempdb..#AllProfileSharedUsersHRWiseActionReport') Is not null
			DROP TABLE #AllProfileSharedUsersHRWiseActionReport

		CREATE TABLE #AllProfileSharedUsersHRWiseActionReport
		(
			UserId			  int  default 0,
			HiringRequestID	  bigint default 0,
			Talent_ID		  bigint,
			IsODR			  bit,
			IsReplacement	  bit
		)

		Insert Into #AllProfileSharedUsersHRWiseActionReport(HiringRequestID, Talent_ID, IsODR,IsReplacement)
		Select 
		distinct	H.ID,CTP.TalentID,0 as IsODR,IsNull(CTP.IsReplacement,0)
		from		gen_SalesHiringRequest H With(NOlock) 
					INNER JOIN gen_ContactTalentPriority CTP with(nolock) on H.ID = CTP.HiringRequestID 
					inner join usr_User U with(nolock) on CTP.CreatedByID = U.ID 
					--INNER join gen_Contact C with(Nolock) On C.ID = CTP.ContactID
					--INNER join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
		where      Isnull(CTP.IsReplacement,0) = Isnull(@IsReplacement,Isnull(CTP.IsReplacement,0))
				  --AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
	

		begin --Insert All User Hierarchies in Temp Table

			IF OBJECT_ID('tempdb..#AllHrchyPopUp') is not null
				drop table #AllHrchyPopUp;

			CREATE TABLE #AllHrchyPopUp(UserID bigint, ParentID bigint, MainParentID bigint);

			--IF OBJECT_ID('tempdb..#GetLeadUsersCompany') Is not null
			--	DROP TABLE #GetLeadUsersCompany
				
			--CREATE TABLE #GetLeadUsersCompany
			--(
			--	UserID			bigint,
			--	CompanyId		bigint
			--)

			--IF @LeadUserID <> 0
			--	BEGIN
			--			;WITH cteUser AS (    
			--				SELECT	   u.ID AS UserID, 
			--						   uh.ParentID,
			--						   u.FullName AS Name,
			--						   @MainPArentId as MainParentID,
			--						   0 AS [Level]
			--				FROM       [dbo].[usr_User] u with(nolock)
			--						   LEFT JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID
			--				WHERE      u.ID = @LeadUserID and U.IsActive = 1		
 
			--				UNION ALL
 
			--				SELECT u.ID AS UserID,
			--						child.ParentID,
			--						u.FullName AS Name,
			--						cte.MainParentID as MainParentID,
			--						[Level]+1 AS [Level]
			--				FROM       [dbo].[usr_User] u with(nolock)
			--				INNER JOIN  [dbo].[usr_UserHierarchy] child with(nolock) ON child.UserID = u.ID
			--				INNER JOIN cteUser cte  ON cte.UserID = child.ParentID
			--		)
			--		Insert into #GetLeadUsersCompany
			--		select  distinct C.UserID,CLU.CompanyID
			--		from    cteUser C 
			--				inner join gen_CompanyLeadType_UserDetails CLU WITH(NOLOCK) ON CLU.LeadType_UserID = C.UserID
			--		UNION ALL
			--		SELECT  0,COM.ID as CompanyID
			--		FROM	gen_Company COM with(nolock)
			--		WHERE	IsActive = 1 and Lead_Type =	CASE WHEN @LeadUser_UserType = 11 THEN 'OutBound' ELSE
			--												(CASE WHEN @LeadUser_UserType = 12 THEN 'InBound' END) END

				
			--	END

				BEGIN
						;WITH cteUser_AllHrchy AS (    
						SELECT		u.ID AS UserID, 
									uh.ParentID,
									u.FullName AS Name,
									H1.HeadID as MainParentID,
									0 AS [Level]
						FROM		[dbo].[usr_User] u with(nolock)
									LEFT JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID
									LEFT JOIN #HeadTempTbl2 H1 on (uh.ParentID = H1.HeadID or uh.UserID = H1.HeadID)
						WHERE		(uh.ParentID = H1.HeadID or uh.UserID = H1.HeadID) and U.IsActive = 1	   
 
					UNION ALL
 
						SELECT		u.ID AS UserID,
									child.ParentID,
									u.FullName AS Name,
									cte.MainParentID as MainParentID,
									[Level]+1 AS [Level]
						FROM		[dbo].[usr_User] u with(nolock)
						INNER JOIN  [dbo].[usr_UserHierarchy] child with(nolock) ON child.UserID = u.ID
						INNER JOIN	cteUser_AllHrchy cte  ON cte.UserID = child.ParentID
						--INNER JOIN #HeadTempTbl2 H1 on U.ID = H1.HeadID
						WHERE		U.IsActive = 1
					)

					INSERT INTO #AllHrchyPopUp(UserID, ParentID, MainParentID)
						select C.UserID, C.ParentID, C.MainParentID from cteUser_AllHrchy C
						group by C.UserID, C.ParentID, C.MainParentID
				END
			

		end
		

		--select * from #AllHrchyPopUp
		

		select IM.Talent_ID,Max(IM.ID) ID,IM.HiringRequest_ID
		into   #MaxInterviewroundCount
		from   gen_SalesHiringRequest H with(nolock) 
			   INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID
			   inner join gen_InterviewSlotsMaster IM With(NOlOCK) ON IM.HiringRequest_ID = H.ID and HIs.Talent_ID = IM.Talent_ID and His.InterviewMaster_ID = IM.ID
			   INNER join gen_Contact C with(Nolock) On C.ID = H.ContactID
			   INNER join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
			   Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
		where  ((cast(His.CreatedByDatetime as date) between Cast(@From_Date1 as date) and cast(@To_date1 as date))
			    OR
			   (cast(His.CreatedByDatetime as date) between Cast(@From_Date2 as date) and cast(@To_date2 as date)))
			    and Isnull(H.IsHRTypeDP,0) =  ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0))
			   and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
			   and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))
			   and 
				(
					(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
					Or
					(	
						@Geos = '' and 1 = 1
					)
				)
		GRoup By IM.Talent_ID,IM.HiringRequest_ID

		

		select IM.Talent_ID,Max(IM.ID) ID,IM.HiringRequest_ID
		into   #MaxInterviewroundCountHrWise
		from   gen_SalesHiringRequest H with(nolock) 
			   INNER JOIN gen_ContactTalentPriority CTP with(Nolock) ON H.ID = CTP.HiringRequestID
			   inner join gen_InterviewSlotsMaster IM With(NOlOCK) ON IM.HiringRequest_ID = H.ID and CTP.TalentID = IM.Talent_ID --and His.InterviewMaster_ID = IM.ID
			   INNER join gen_Contact C with(Nolock) On C.ID = H.ContactID
			   INNER join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
			   Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
		where  ((cast(H.CreatedByDatetime as date) between Cast(@From_Date1 as date) and cast(@To_date1 as date))
			    OR
			   (cast(H.CreatedByDatetime as date) between Cast(@From_Date2 as date) and cast(@To_date2 as date)))
			    and Isnull(H.IsHRTypeDP,0) =  ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0))
			   and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
			   and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))
			   and 
				(
					(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
					Or
					(	
						@Geos = '' and 1 = 1
					)
				)
		GRoup By IM.Talent_ID,IM.HiringRequest_ID

		IF OBJECT_ID('tempdb..#AllHiredTalentCount') is not null
			drop table #AllHiredTalentCount;

		CREATE TABLE #AllHiredTalentCount(HR_ID bigint,TalentCount int);

		Insert into #AllHiredTalentCount
		select   OT.HiringRequest_ID,Count(1) as TalentCount
		from     gen_OnBoardTalents OT WITH(NOLOCK)
		where    OT.Status_ID = 1
		GROUP BY OT.HiringRequest_ID

		 IF @StageName = 'HR Active'
			BEGIN 
					
					IF @Option = 1
					BEGIN
						INSERT INTO #ChannelReportPopUpDetail
						SELECT  distinct ISNULL(HR_NUMBER,'') + Case when His.Action_ID = 83 then ' (' + 'ReOpen' + ') '  else '' end,ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								''  ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
								--,Isnull(HRS.HiringRequest_Status,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   ISNULL(H.IsActive,0) = 1 And His.Action_ID IN (4,77,83) AND H.JobStatusID <> 2 --AND H.Status_ID NOT IN (3,4,5,6)
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)

					END
					ELSE
					BEGIN
						INSERT INTO #ChannelReportPopUpDetail
						SELECT  distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								''  ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
								--,Isnull(HRS.HiringRequest_Status,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   H.Status_ID != 4 And  ISNULL(H.IsActive,0) = 1 AND H.JobStatusID <> 2 --AND H.Status_ID NOT IN (3,4,5)
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
					END

		  END
		ELSE IF @StageName = 'TR Active'
			BEGIN 
				IF @Option = 1
					BEGIN 
						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								cast(case when Availability ='Part Time' then Cast((Isnull(H.NoofTalents,0) - Isnull(AHT.TalentCount,0)) as decimal(18,1)) / 2 else Cast((Isnull(H.NoofTalents,0) - Isnull(AHT.TalentCount,0)) as decimal(18,1)) end as decimal(18,1)) [Name],
								Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
								--,Isnull(HRS.HiringRequest_Status,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID							
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
								Left  join #AllHiredTalentCount AHT ON AHT.HR_ID = H.ID
						WHERE   ISNULL(H.IsActive,0) = 1 AND His.Action_ID = 4 AND H.JobStatusID <> 2 --H.Status_ID NOT IN (3,4,5,6)
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and cast(case when Availability ='Part Time' then Cast((Isnull(H.NoofTalents,0) - Isnull(AHT.TalentCount,0)) as decimal(18,1)) / 2 else Cast((Isnull(H.NoofTalents,0) - Isnull(AHT.TalentCount,0)) as decimal(18,1)) end as decimal(18,1)) > 0 
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
					END
				ELSE
					BEGIN 
						INSERT INTO #ChannelReportPopUpDetail
						SELECT  distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								cast(case when Availability ='Part Time' then Cast((Isnull(H.NoofTalents,0) - Isnull(AHT.TalentCount,0)) as decimal(18,1)) / 2 else Cast((Isnull(H.NoofTalents,0) - Isnull(AHT.TalentCount,0)) as decimal(18,1)) end as decimal(18,1)) [Name],
								Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
								--,Isnull(HRS.HiringRequest_Status,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID							
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
								Left  join #AllHiredTalentCount AHT ON AHT.HR_ID = H.ID
						WHERE   ISNULL(H.IsActive,0) = 1 and  Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
					END
			END
		ELSE IF @StageName = 'TR Created'
			BEGIN 
				IF @Option = 1
				 BEGIN				
					INSERT INTO #ChannelReportPopUpDetail
					SELECT  distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
							Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
							--MAX(H.NoOfTalents) [Name],
							cast(case when H.Availability ='Part Time' then  cast(MAX(H.NoOfTalents) as decimal(18,1))/2   else ISNULL(MAX(H.NoOfTalents),0) end as decimal(18,1)) as [Name],
							Isnull(H.Availability,'') [Availability]
							,Isnull(JSC.JobStatus,'') as HRStatus
							--,Isnull(HRS.HiringRequest_Status,'') as HRStatus
					FROM	gen_SalesHiringRequest H with(nolock) 
							INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
							INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
							--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
							Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
							INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
							inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
							inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
							inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
							inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID							
							INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID ---(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
							--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
							Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
					WHERE   ISNULL(H.IsActive,0) = 1 AND His.Action_ID in (4,77,83)  
							AND ( (cast(His.CreatedByDatetime as date) between (Cast(Coalesce(@From_Date1,@From_Date2) as date))
										and cast(Coalesce(@To_date1,@To_Date2) as date))							
							) 
							and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
							and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
							and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
							and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
							--and 
							--(
							--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
							--	Or
							--	(	
							--		@LeadUserID = 0 and 1 = 1
							--	)
							--)
							AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
							and 
							(
								(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
								Or
								(	
									@Geos = '' and 1 = 1
								)
							)
					GROUP BY HR_NUMBER,U.FullName,Co.Company,Ur.TalentRole,H.Availability,H.IsManaged,JSC.JobStatus--HRS.HiringRequest_Status


					UNION ALL


					SELECT  distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
							Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
							--MAX(TRU.UpdatedTR) [Name],
							cast(case when H.Availability ='Part Time' then  cast(MAX(TRU.UpdatedTR) as decimal(18,1))/2   else ISNULL(MAX(TRU.UpdatedTR),0) end as decimal(18,1)) as [Name],
							Isnull(H.Availability,'') [Availability]
							,Isnull(JSC.JobStatus,'') as HRStatus
							--,Isnull(HRS.HiringRequest_Status,'') as HRStatus
					FROM	gen_SalesHiringRequest H with(nolock) 
							INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
							INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
							--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
							Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
							INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
							inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
							inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
							inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
							inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID							
							INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID  --(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
							--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
							INNER JOIN gen_SalesHR_TRUpdated_Details TRU with(nolock) on H.ID = TRU.HiringRequestID AND TRU.HistoryID = His.ID AND TRU.IsIncreased = 1 
							Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
					WHERE   ISNULL(H.IsActive,0) = 1 AND His.Action_ID = 79 
							AND ( (cast(His.CreatedByDatetime as date) between (Cast(Coalesce(@From_Date1,@From_Date2) as date))
										and cast(Coalesce(@To_date1,@To_Date2) as date))							
							) 
							and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
							and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
							and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
							and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
							--and 
							--(
							--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
							--	Or
							--	(	
							--		@LeadUserID = 0 and 1 = 1
							--	)
							--)
							AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
							and 
							(
								(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
								Or
								(	
									@Geos = '' and 1 = 1
								)
							)
					GROUP BY HR_NUMBER,U.FullName,Co.Company,Ur.TalentRole,H.Availability,H.IsManaged,JSC.JobStatus --HRS.HiringRequest_Status
				END
				ELSE
				BEGIN
					INSERT INTO #ChannelReportPopUpDetail
					SELECT  distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
							Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
							--Max(TRU.UpdatedTR)  [Name],
							cast(case when H.Availability ='Part Time' then  cast(MAX(H.NoOfTalents) as decimal(18,1))/2   else ISNULL(MAX(H.NoOfTalents),0) end as decimal(18,1)) as [Name],
							Isnull(H.Availability,'') [Availability]
							,Isnull(JSC.JobStatus,'') as HRStatus
							--,Isnull(HRS.HiringRequest_Status,'') as HRStatus
					FROM	gen_SalesHiringRequest H with(nolock) 
							INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
							--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
							Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
							INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
							inner join usr_User U With(Nolock) On U.Id = UH.UserID
							inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
							inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
							inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID							
							INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
							Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
					WHERE   ISNULL(H.IsActive,0) = 1 
							AND ( (cast(H.CreatedByDatetime as date) between (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
							and cast(Coalesce(@To_date1,@To_Date2) as date))							
							) and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
							and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
							and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
							and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
							--and 
							--(
							--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
							--	Or
							--	(	
							--		@LeadUserID = 0 and 1 = 1
							--	)
							--)
							and 
							(
								(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
								Or
								(	
									@Geos = '' and 1 = 1
								)
							)
					GROUP BY HR_NUMBER,U.FullName,Co.Company,Ur.TalentRole,H.Availability,H.IsManaged,JSC.JobStatus--HRS.HiringRequest_Status
					

					UNION ALL


					SELECT  distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
							Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
							--Max(TRU.UpdatedTR)  [Name],
							cast(case when H.Availability ='Part Time' then  cast(MAX(TRU.UpdatedTR) as decimal(18,1))/2   else ISNULL(MAX(TRU.UpdatedTR),0) end as decimal(18,1)) as [Name],
							Isnull(H.Availability,'') [Availability]
							,Isnull(JSC.JobStatus,'') as HRStatus
							--,Isnull(HRS.HiringRequest_Status,'') as HRStatus
					FROM	gen_SalesHiringRequest H with(nolock) 
							INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
							--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
							Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
							INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
							inner join usr_User U With(Nolock) On U.Id = UH.UserID
							inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
							inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
							inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID							
							INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
							INNER JOIN gen_SalesHR_TRUpdated_Details TRU with(nolock) on H.ID = TRU.HiringRequestID AND TRU.IsIncreased = 1 
							Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
					WHERE   ISNULL(H.IsActive,0) = 1 
							AND ( (cast(H.CreatedByDatetime as date) between (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
							and cast(Coalesce(@To_date1,@To_Date2) as date))							
							) and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
							and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
							and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
							and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
							--and 
							--(
							--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
							--	Or
							--	(	
							--		@LeadUserID = 0 and 1 = 1
							--	)
							--)
							and 
							(
								(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
								Or
								(	
									@Geos = '' and 1 = 1
								)
							)
					GROUP BY HR_NUMBER,U.FullName,Co.Company,Ur.TalentRole,H.Availability,H.IsManaged,JSC.jobStatus--HRS.HiringRequest_Status

				END
			END
		ELSE If @StageName = 'TR Information Pending'
			BEGIN 
					if @Option = 1
					BEGIN
						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								''  ,Isnull(H.Availability,'') [Availability],Isnull(JSC.JobStatus,'') as HRStatus --Isnull(HRS.HiringRequest_Status,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID ---(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   ISNULL(H.IsActive,0) = 1 And His.Action_ID = 44  AND ISNULL(H.IsAccepted,0) = 2 
								AND ( (cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date))
								) and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
					END
					ELSE
					BEGIN

						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								''  ,Isnull(H.Availability,'') [Availability],Isnull(JSC.JobStatus,'') as HRStatus --Isnull(HRS.HiringRequest_Status,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   ISNULL(H.IsActive,0) = 1  AND ISNULL(H.IsAccepted,0) = 2 		 
								AND ( (cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date))
								) and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
					END
			END
		IF @StageName = 'TR Accepted'
			BEGIN 
				IF @Option = 1
					BEGIN
					
						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(Sales.FullName,''),Isnull(Co.Company,''),
								Isnull(H.RequestForTalent,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,								
								cast(case when H.Availability ='Part Time' then  cast(sum(TRU.UpdatedTR) as decimal(18,1))/2   else SUM(ISNULL(TRU.UpdatedTR,0)) end as decimal(18,1)) as [Name],									
								Isnull(H.Availability,'') [Availability],Isnull(JSC.JobStatus,'') as HRStatus --Isnull(HRS.HiringRequest_Status,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID									
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID	
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID 
								INNER JOIN usr_User Sales with(nolock) on Sales.ID= H.SalesUserID					
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID ---(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
								Left JOIN gen_SalesHR_TRUpdated_Details TRU with(nolock) ON H.ID = TRU.HiringRequestId 
																
						WHERE   ISNULL(H.IsActive,0) = 1 and His.Action_ID = 79 and ISNULL(H.IsAccepted,0) = 1
								AND ( (cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								 and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
							   and H.TR_Accepted > 0
							   AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
							   AND ( (cast(TRU.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(TRU.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(TRU.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date) )
								and cast(TRU.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
							   group by  ISNULL(HR_NUMBER,''),ISNULL(Sales.FullName,''),Isnull(Co.Company,''),
								Isnull(H.RequestForTalent,''), H.IsManaged,H.Availability,Isnull(JSC.JobStatus,'') --Isnull(HRS.HiringRequest_Status,'') 
                   UNION ALL
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(Sales.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,								
								cast(case when H.Availability ='Part Time' then  cast(HTR.TR_Accepted as decimal(18,1))/2   else ISNULL(HTR.TR_Accepted,0) end as decimal(18,1)) as [Name],									
								Isnull(H.Availability,'') [Availability],Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_SalesHR_TRAccepted_Details HTR with(nolock) on H.ID = HTR.HiringRequest_ID
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID	
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID 
								INNER JOIN usr_User Sales with(nolock) on Sales.ID= H.SalesUserID					
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID---(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID) --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID	
								Left JOIN gen_SalesHR_TRUpdated_Details TRU with(nolock) ON H.ID = TRU.HiringRequestId 
								
						WHERE   ISNULL(H.IsActive,0) = 1 and His.Action_ID = 58 and ISNULL(H.IsAccepted,0) = 1
								AND ( (cast(HTR.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(HTR.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(HTR.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date) )
								and cast(HTR.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date))
								) and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
							   and H.TR_Accepted > 0
							   AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
							   and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
					END
				ELSE
					BEGIN
						INSERT INTO #ChannelReportPopUpDetail
						SELECT  distinct ISNULL(HR_NUMBER,''),ISNULL(Sales.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								cast(case when H.Availability ='Part Time' then  cast(H.TR_Accepted as decimal(18,1))/2   else ISNULL(H.TR_Accepted,0) end as decimal(18,1)) as [Name],									
								Isnull(H.Availability,'') [Availability],Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID 
								INNER JOIN usr_User Sales with(nolock) on Sales.ID= H.SalesUserID					
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   ISNULL(H.IsActive,0) = 1 and ISNULL(H.IsAccepted,0) = 1 				
								AND
								 ( (cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date))
								) and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
							and H.TR_Accepted > 0
							and 
							(
								(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
								Or
								(	
									@Geos = '' and 1 = 1
								)
							)
					END
			END
		ELSE IF @StageName = 'TR Lost' 
			BEGIN 
						IF @Option = 1
						BEGIN
							INSERT INTO #ChannelReportPopUpDetail
							SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
									Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
									cast(case when Availability ='Part Time' then  cast(TRA.UpdatedTR as decimal(18,1))/2   else ISNULL(TRA.UpdatedTR,0) end as decimal(18,1)) as [Name] ,
									Isnull(H.Availability,'') [Availability],Isnull(JSC.JobStatus,'') as HRStatus
							FROM	gen_SalesHiringRequest H with(nolock) 
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
									INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
									--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
									Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
									INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
									inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
									inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
									inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
									inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
									INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID --(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								    --INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
									INNER JOIN gen_SalesHR_TRUpdated_Details TRA with(nolock) ON TRA.HiringRequestID = H.ID
									Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID									 
							WHERE   ISNULL(H.IsActive,0) = 1 AND ISNULL(H.IsAccepted,0) = 1 
									and	His.Action_ID in( 80,82) 
									and TRA.IsDecreased = 1 and TRA.IsTRLost = 1 
									AND ((cast(His.CreatedByDatetime as date) between (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
											and cast(Coalesce(@To_date1,@To_Date2) as date))									
									) and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
									and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
									and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
									and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
									and cast(TRA.CreatedByDateTime as date) between Cast(@From_Date1 as date) and Cast(@To_date1 as date)
									--and 
									--(
									--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
									--	Or
									--	(	
									--		@LeadUserID = 0 and 1 = 1
									--	)
									--) 
									and 
									(
										(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
										Or
										(	
											@Geos = '' and 1 = 1
										)
									)
									AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
							

							
						END
						ELSE
						BEGIN
							INSERT INTO #ChannelReportPopUpDetail
							SELECT  distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
									Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
									cast(case when Availability ='Part Time' then  cast(TRA.UpdatedTR as decimal(18,1))/2   else ISNULL(TRA.UpdatedTR,0) end as decimal(18,1)) as [Name] ,
									Isnull(H.Availability,'') [Availability],Isnull(JSC.JobStatus,'') as HRStatus
							FROM	gen_SalesHiringRequest H with(nolock) 
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
									--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
									Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
									INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
									inner join usr_User U With(Nolock) On U.Id = UH.UserID
									inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
									inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
									inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
									INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
									INNER JOIN gen_SalesHR_TRUpdated_Details TRA with(nolock) ON TRA.HiringRequestID = H.ID
									Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
							WHERE   ISNULL(H.IsActive,0) = 1  AND ISNULL(H.IsAccepted,0) = 1 
									and TRA.IsDecreased = 1 and TRA.IsTRLost = 1
									AND ( (cast(H.CreatedByDatetime as date) between (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
									and  cast(Coalesce(@To_date1,@To_Date2) as date))									
									) and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
									and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
									and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
									and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
									--and 
									--(
									--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
									--	Or
									--	(	
									--		@LeadUserID = 0 and 1 = 1
									--	)
									--) 
									and 
									(
										(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
										Or
										(	
											@Geos = '' and 1 = 1
										)
									)
						END

			  END
		ELSE IF @StageName = 'TR Cancelled'
			BEGIN 
					IF @Option = 1
						BEGIN
							INSERT INTO #ChannelReportPopUpDetail
							SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
									Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
									cast(case when Availability ='Part Time' then  cast(TRA.UpdatedTR as decimal(18,1))/2   else ISNULL(TRA.UpdatedTR,0) end as decimal(18,1)) as [Name] ,Isnull(H.Availability,'') [Availability]
									,Isnull(JSC.JobStatus,'') as HRStatus
							FROM	gen_SalesHiringRequest H with(nolock) 
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
									INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
									--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
									Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
									INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
									inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
									inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
									inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
									inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
									INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID --(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								    --INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
									INNER JOIN gen_SalesHR_TRUpdated_Details TRA with(nolock) ON TRA.HiringRequestID = H.ID
									Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
							WHERE   His.Action_ID in( 80,82) and TRA.IsDecreased = 1 and TRA.IsTRCancel = 1 
									AND ( (cast(His.CreatedByDatetime as date) between (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
									and cast(Coalesce(@To_date1,@To_Date2) as date))									
									) and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
									and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
									and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
									and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
									and cast(TRA.CreatedByDateTime as date) between Cast(@From_Date1 as date) and Cast(@To_date1 as date)
									--and 
									--(
									--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
									--	Or
									--	(	
									--		@LeadUserID = 0 and 1 = 1
									--	)
									--)
									AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and 
									(
										(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
										Or
										(	
											@Geos = '' and 1 = 1
										)
									)
								
						END
						ELSE
						BEGIN
							INSERT INTO #ChannelReportPopUpDetail
							SELECT  distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
									Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
									cast(case when Availability ='Part Time' then  cast(TRA.UpdatedTR as decimal(18,1))/2   else ISNULL(TRA.UpdatedTR,0) end as decimal(18,1)) as [Name] ,Isnull(H.Availability,'') [Availability]
									,Isnull(JSC.JobStatus,'') as HRStatus
							FROM	gen_SalesHiringRequest H with(nolock) 
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
									--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
									Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
									INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
									inner join usr_User U With(Nolock) On U.Id = UH.UserID
									inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
									inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
									inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
									--INNER JOIN gen_SalesHR_TRAccepted_Details TRA with(nolock) ON H.ID = TRA.HiringRequest_ID --AND CAST(H.CreatedByDatetime as date) = CAST(TRA.CreatedByDateTime as DATE)
									INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
									Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
									left JOIN gen_SalesHR_TRUpdated_Details TRA with(nolock) ON TRA.HiringRequestID = H.ID 
							WHERE  TRA.IsDecreased = 1 and TRA.IsTRCancel = 1	 
									and cast(case when Availability ='Part Time' then  cast(TRA.UpdatedTR as decimal(18,1))/2   else ISNULL(TRA.UpdatedTR,0) end as decimal(18,1)) > 0
									AND ( (cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
									and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
									OR
									(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date) )
									and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date))
									) and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
									and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
									and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
									and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
									--and 
									--(
									--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
									--	Or
									--	(	
									--		@LeadUserID = 0 and 1 = 1
									--	)
									--)
									and 
									(
										(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
										Or
										(	
											@Geos = '' and 1 = 1
										)
									)
									--and TRA.TR_Accepted > 0 
						END

			  END
	   ELSE IF @StageName = 'Profile Shared'
			BEGIN 
					if @Option = 1
					begin
						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(T.Name,''),Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID								
								INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID 
								inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID and AP.Talent_ID = T.ID
								--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID --(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   H.Status_ID != 4 and His.Action_ID = 18 and ISNULL(H.IsAccepted,0) = 1 
								and ((cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--) 
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
						
					end
					else
					begin

						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(T.Name,'') [Name],Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								Inner Join gen_ContactTalentPriority CTP on H.ID = CTP.HiringRequestID 
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID								
								INNER join gen_Talent T With(Nolock) On T.Id = CTP.TalentID 
								inner join #AllProfileSharedUsersHRWiseActionReport AP on AP.HiringRequestID = H.ID --and AP.Talent_ID = T.ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   H.Status_ID != 4 and ISNULL(H.IsAccepted,0) = 1 
								and ((cast(H.CreatedByDatetime as date) between (Cast(Coalesce(@From_Date1,@From_Date2) as date))
								and cast(Coalesce(@To_date1,@To_Date2) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) 
								and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--) 
								and CTP.TalentStatusID_BasedOnHR = 2
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
						
					end
			END
		ELSE IF @StageName = 'Matchmaking Cancelled'
			BEGIN 
					if @Option = 1
					begin
						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(T.Name,'') + ' (' + Isnull(T.EmailID,'') + ')',Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID								
								INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID  
								inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID --- (AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
								--LEFT JOIN  gen_SalesHR_TRUpdated_Details TRA with(nolock) ON  H.ID = TRA.HiringRequestID and TRA.IsDecreased = 1 
						WHERE   --H.Status_ID != 1 
						        H.JobStatusID <> 4 and His.Action_ID in (20,80,81) and ISNULL(H.IsAccepted,0) = 1 and ISNULL(H.IsActive,0) = 1
								and ((cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--) 
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
					end
					else
					begin

						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(CONCAT(T.Name,' (' + TAC.TalentStatus + ')'),''),Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								Inner Join gen_ContactTalentPriority CTP on H.ID = CTP.HiringRequestID 
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID								
								INNER join gen_Talent T With(Nolock) On T.Id = CTP.TalentID 
								INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = CTP.TalentStatusID_BasedOnHR 
								inner join #AllProfileSharedUsersHRWiseActionReport AP on AP.HiringRequestID = H.ID and AP.Talent_ID = T.ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
								LEFT JOIN  gen_SalesHR_TRUpdated_Details TRA with(nolock) ON  H.ID = TRA.HiringRequestID 
						WHERE   --H.Status_ID != 1 
								H.JobStatusID <> 4 and ISNULL(H.IsAccepted,0) = 1 
								--and IsNull(AP.IsODR,0) = CASE WHEN @AdHocTypeVal <> 2 then @AdHocTypeVal else IsNull(AP.IsODR,0) end 
								and ((cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and TRA.IsDecreased = 1 
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
					
					end
			END		
		ELSE IF @StageName = 'Profile Moved to Interview'
			BEGIN
					if @Option = 1
					begin
						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								ISNULL(T.Name,'') ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER join gen_ContactTalentPriority CTP With(NOlock) on CTP.HiringRequestID = H.ID 
								INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID and CTP.TalentID = T.ID
								inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID-- (AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								inner join Gen_InterviewSlotsMaster I WITH(NOLOCK) ON I.HiringRequest_ID = H.ID and I.Talent_ID = T.ID and I.ID = His.InterViewMaster_ID
								INNER JOIN #MaxInterviewroundCount MIC ON MIC.HiringRequest_ID = I.HiringRequest_ID and MIC.Talent_ID = I.Talent_ID and MIC.ID = I.ID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   --H.Status_ID != 1 
								H.JobStatusID <> 4 and His.Action_ID = 9 AND ISNULL(H.IsAccepted,0) = 1 and ISNULL(H.IsActive,0) = 1
								AND ((cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and I.InterviewStatus_ID <> 3
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
					end
					else
					begin		
					
					

						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								--Isnull(CONCAT(T.Name,' (' + TAC.TalentStatus + ')'),''),Isnull(H.Availability,'') [Availability]
								ISNULL(T.Name,'') ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID										
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER join gen_ContactTalentPriority CTP With(NOlock) on CTP.HiringRequestID = H.ID
								INNER join gen_Talent T With(Nolock) On T.Id = Ctp.TalentID	
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								inner join #AllProfileSharedUsersHRWiseActionReport AP on AP.HiringRequestID = H.ID and AP.Talent_ID = T.ID
								inner join Gen_InterviewSlotsMaster I WITH(NOLOCK) ON I.HiringRequest_ID = CTP.HiringRequestID and I.Talent_ID = CTP.TalentID 
								INNER JOIN #MaxInterviewroundCountHrWise MIC ON MIC.HiringRequest_ID = CTP.HiringRequestID and MIC.Talent_ID = CTP.TalentID --and MIC.ID = I.ID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID								
						WHERE   --H.Status_ID != 1  
								H.JobStatusID <> 4 AND ISNULL(H.IsAccepted,0) = 1 AND ISNULL(H.IsActive,0) = 1
								and ((cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and I.InterviewStatus_ID <> 3
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
					end
			END
		ELSE IF @StageName = 'Profile Rejected'
			BEGIN
					if @Option = 1
					begin
						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(T.Name,'') 
								,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID 
								inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID -- (AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   --H.Status_ID != 1 
								H.JobStatusID <> 4 and His.Action_ID = 22 AND ISNULL(H.IsAccepted,0) = 1 
								and ((cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--) 
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
					end
					else
					begin

						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								--Isnull(CONCAT(T.Name,' (' + TAC.TalentStatus + ')'),'') ,Isnull(H.Availability,'') [Availability]
								Isnull(T.Name,'') 
								,Isnull(H.Availability,'') [Availability],Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID										
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER join gen_ContactTalentPriority CTP With(NOlock) on CTP.HiringRequestID = H.ID
								INNER join gen_Talent T With(Nolock) On T.Id = Ctp.TalentID	
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								inner join #AllProfileSharedUsersHRWiseActionReport AP on AP.HiringRequestID = H.ID and AP.Talent_ID = T.ID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   --H.Status_ID != 1 
								H.JobStatusID <> 4 AND ISNULL(H.IsAccepted,0) = 1 	 
								and ((cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) 
								and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--) 
								and CTP.TalentStatusID_BasedOnHR = 7
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
					end
			END
		ELSE IF @StageName = 'Talent Interview Completed'
			BEGIN
					if @Option = 1
					BEGIN
					
					   INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(T.Name,'') + ' - ' + Isnull(I.InterviewRound_Str,'') [Name],Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID  
								inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID 
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID --(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								INNER JOIN gen_InterviewSlotsMaster I with(nolock) ON H.ID = I.HiringRequest_ID and I.ID = His.InterviewMaster_ID and I.Talent_ID = His.Talent_ID									
								INNER JOIN #MaxInterviewroundCount MIC ON MIC.HiringRequest_ID = I.HiringRequest_ID and MIC.Talent_ID = I.Talent_ID and MIC.ID = I.ID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   --H.Status_ID != 1 
								H.JobStatusID <> 4 And His.Action_ID = 11 AND ISNULL(H.IsAccepted,0) = 1 AND  ISNULL(H.IsActive,0) = 1 
								and ((cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
								--and cTP.TalentStatusID_BasedOnHR = 7 
					END
					ELSE
					BEGIN

						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(T.Name,'') + ' - ' + Isnull(I.InterviewRound_Str,'')  [Name] ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID										
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER join gen_ContactTalentPriority CTP With(NOlock) on CTP.HiringRequestID = H.ID								
								INNER join gen_Talent T With(Nolock) On T.Id = Ctp.TalentID	
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								inner join #AllProfileSharedUsersHRWiseActionReport AP on AP.HiringRequestID = H.ID and AP.Talent_ID = T.ID
								INNER JOIN #MaxInterviewroundCountHrWise MIC ON MIC.HiringRequest_ID = CTP.HiringRequestID and MIC.Talent_ID = Ctp.TalentID	 --and MIC.ID = I.ID
								INNER JOIN gen_InterviewSlotsMaster I with(nolock) ON MIC.ID = I.ID						
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   CTP.TalentStatusID_BasedOnHR != 5 and H.JobStatusID <> 4 --H.Status_ID != 1 
								and I.InterviewStatus_ID = 6 AND ISNULL(H.IsAccepted,0) = 1
								AND  ISNULL(H.IsActive,0) = 1 
								and ((cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
					end
			END
		ELSE IF @StageName = 'Talent Hired'
			BEGIN
					if @Option = 1
					BEGIN
						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(CONCAT(T.Name,' (' + TAC.TalentStatus + ')'),''),Isnull(H.Availability,'') [Availability] 
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID 
								Inner Join gen_ContactTalentPriority CTP on His.HiringRequest_ID = CTP.HiringRequestID and His.Talent_ID = CTP.TalentID
								INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = CTP.TalentStatusID_BasedOnHR 
								inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID--(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   --H.Status_ID != 1 
								H.JobStatusID <> 4  and ISNULL(H.IsAccepted,0) = 1 And His.Action_ID = 12 
								and((cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
					END
					ELSE
					BEGIN

						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(CONCAT(T.Name,' (' + TAC.TalentStatus + ')'),'') ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID										
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER join gen_ContactTalentPriority CTP With(NOlock) on CTP.HiringRequestID = H.ID 
								INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = CTP.TalentStatusID_BasedOnHR 
								INNER JOIN gen_ContactInterviewFeedback CIF with(nolock) ON H.ID = CIF.HiringRequest_ID 
								INNER join gen_Talent T With(Nolock) On T.Id = Ctp.TalentID	
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								inner join #AllProfileSharedUsersHRWiseActionReport AP on AP.HiringRequestID = H.ID and AP.Talent_ID = T.ID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   TAC.ID = 4 and H.JobStatusID <> 4 --H.Status_ID != 1 
								AND CIF.FeedBack_Type = 'Hire' and ISNULL(H.IsAccepted,0) = 1
							    and ((cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								
					end
			END
		ELSE IF @StageName = 'Talent Reject after Interview'
			BEGIN
					if @Option = 1
					BEGIN
						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(CONCAT(T.Name,' (' + TAC.TalentStatus + ')'),'') ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID 
								Inner Join gen_ContactTalentPriority CTP on His.HiringRequest_ID = CTP.HiringRequestID and His.Talent_ID = CTP.TalentID
								INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = CTP.TalentStatusID_BasedOnHR 
								inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID ---(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   --H.Status_ID != 1 
							    H.JobStatusID <> 4	and ISNULL(H.IsAccepted,0) = 1 And His.Action_ID = 45 
								and((cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
					END
					ELSE
					BEGIN

						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(CONCAT(T.Name,' (' + TAC.TalentStatus + ')'),'') ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID										
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER join gen_ContactTalentPriority CTP With(NOlock) on CTP.HiringRequestID = H.ID 
								INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = CTP.TalentStatusID_BasedOnHR 
								INNER JOIN gen_ContactInterviewFeedback CIF with(nolock) ON H.ID = CIF.HiringRequest_ID 
								INNER join gen_Talent T With(Nolock) On T.Id = Ctp.TalentID	
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								inner join #AllProfileSharedUsersHRWiseActionReport AP on AP.HiringRequestID = H.ID and AP.Talent_ID = T.ID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   CTP.TalentStatusID_BasedOnHR = 7 and H.JobStatusID <> 4 --H.Status_ID != 1 
								and ISNULL(H.IsAccepted,0) = 1 AND CIF.FeedBack_Type = 'NoHire' 
								and ((cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
					END
			END
		ELSE IF @StageName = 'SOW Sent'
			BEGIN
				IF @Option = 1
				BEGIN
					INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(T.Name,'') ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID 
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER JOIN gen_OnBoardTalents OBT with(nolock) ON OBT.HiringRequest_ID = His.HiringRequest_ID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON ISNULL(OBT.AM_SalesPersonID,0) = UH.UserID
								INNER JOIN usr_User U with(nolock) ON UH.UserID = U.ID
								INNER JOIN gen_Talent T with(nolock) ON T.ID = His.Talent_ID and OBT.Talent_ID = T.ID
								inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID--(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID)  --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								--INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   --H.Status_ID != 1 
								H.JobStatusID <> 4 and ISNULL(H.IsAccepted,0) = 1 AND His.Action_ID = 29 
								and ((cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and ISNULL(OBT.Status_ID,0) = 1
								and ISNULL(OBT.Talent_ID,0) > 0
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
						
				END
				ELSE
				BEGIN
					INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(T.Name,'') ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								inner join gen_ContactTalentPriority CTP with(nolock) on H.ID = CTP.HiringRequestID
								INNER JOIN gen_OnBoardTalents g3 with(nolock) ON g3.HiringRequest_ID = H.ID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON ISNULL(g3.AM_SalesPersonID,0) = UH.UserID
								INNER JOIN usr_User U with(nolock) ON UH.UserID = U.ID
								INNER JOIN gen_Talent T with(nolock) ON T.ID = CTP.TalentID and g3.Talent_ID = T.ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								inner join #AllProfileSharedUsersHRWiseActionReport AP on AP.HiringRequestID = H.ID and AP.Talent_ID = T.ID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   CTP.TalentStatusID_BasedOnHR != 5 and H.JobStatusID <> 4 --H.Status_ID != 1 
								and ISNULL(H.IsAccepted,0) = 1 
							--	and IsNull(AP.IsODR,0) = CASE WHEN @AdHocTypeVal <> 2 then @AdHocTypeVal else IsNull(AP.IsODR,0) end		 
								and ((cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and ISNULL(g3.Status_ID,0) = 1
								and ISNULL(g3.Talent_ID,0) > 0
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
						
				END
					
			END
		ELSE IF @StageName = 'SOW Signed'
			BEGIN
				IF @Option = 1
				BEGIN
				    print '@StageName = SOW Signed @Option = 1 '

					
					
					INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(T.Name,'') ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID 
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER JOIN gen_OnBoardTalents g3 with(nolock) ON H.ID = g3.HiringRequest_ID And g3.Talent_ID = His.Talent_ID 
								INNER JOIN #AllProfileSharedUsers APS on H.ID = APS.HiringRequestID and APS.Talent_ID = g3.Talent_ID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON ISNULL(g3.AM_SalesPersonID,0) = UH.UserID
								INNER JOIN #AllHrchyPopUp AH on  (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID--(AH.UserID = H.SalesUserID Or AH.ParentID = H.SalesUserID or AH.MainParentID = H.SalesUserID) 
								INNER JOIN usr_User U with(nolock) ON UH.UserID = U.ID 
								---INNER JOIN gen_OnBoardTalents_LegalDetails g with(nolock) ON H.ID = g.HiringRequest_ID and g.OnBoardID = g3.ID
								--LEFT JOIN gen_CompanyLegalInfo g2 ON g.CompanyLegalID = g2.ID  AND g2.AgreementStatus = 'Signed'
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   --H.Status_ID != 1 
								H.JobStatusID <> 4 and ISNULL(H.IsAccepted,0) = 1 And His.Action_ID = 35 
								and 
								((cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(His.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(His.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and ISNULL(g3.Status_ID,0) = 1
								and ISNULL(g3.Talent_ID,0) > 0
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
						
				END
				ELSE
				BEGIN
					print '@StageName = SOW Signed ELSE '

					INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(T.Name,'') ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								inner join gen_ContactTalentPriority CTP with(nolock) on H.ID = CTP.HiringRequestID
								INNER JOIN gen_OnBoardTalents g3 with(nolock) ON H.ID = g3.HiringRequest_ID And g3.Talent_ID = CTP.TalentID 
								INNER join gen_Talent T With(Nolock) On T.Id = g3.Talent_ID 
								INNER JOIN usr_UserHierarchy UH with(nolock) ON ISNULL(g3.AM_SalesPersonID,0) = UH.UserID
								INNER JOIN usr_User U with(nolock) ON UH.UserID = U.ID 
								--INNER JOIN gen_OnBoardTalents_LegalDetails g with(nolock) ON H.ID = g.HiringRequest_ID and g.OnBoardID = g3.ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID --(UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								inner join #AllProfileSharedUsersHRWiseActionReport AP on AP.HiringRequestID = H.ID and AP.Talent_ID = T.ID
								--LEFT JOIN gen_CompanyLegalInfo g2 ON g.CompanyLegalID = g2.ID  AND g2.AgreementStatus = 'Signed'
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   CTP.TalentStatusID_BasedOnHR != 5 and H.JobStatusID <> 4 --H.Status_ID != 1 
								and ISNULL(H.IsAccepted,0) = 1 
								and IsNull(AP.IsODR,0) = CASE WHEN @AdHocTypeVal <> 2 then @AdHocTypeVal else IsNull(AP.IsODR,0) end		 
								and ((cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(cast(H.CreatedByDatetime as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and cast(H.CreatedByDatetime as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								and ((CAST(IIF(g3.ClientLegalDate > g3.TalentLegalDate,g3.ClientLegalDate,g3.TalentLegalDate) as date) >= (Cast(Coalesce(@From_Date1,@From_Date2) as date) )
								and CAST(IIF(g3.ClientLegalDate > g3.TalentLegalDate,g3.ClientLegalDate,g3.TalentLegalDate) as date) <= cast(Coalesce(@To_date1,@To_Date2) as date))
								OR
								(CAST(IIF(g3.ClientLegalDate > g3.TalentLegalDate,g3.ClientLegalDate,g3.TalentLegalDate) as date) >= (Cast(Coalesce(@From_Date2,@From_Date1) as date))
								and CAST(IIF(g3.ClientLegalDate > g3.TalentLegalDate,g3.ClientLegalDate,g3.TalentLegalDate) as date) <= cast(Coalesce(@To_Date2,@To_date1) as date)))
								and ISNULL(g3.Status_ID,0) = 1
								and ISNULL(g3.Talent_ID,0) > 0
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
						
				END
					
			END
		ELSE IF @StageName = 'Profile Feedback pending'
			BEGIN
				IF @Option = 1
				  BEGIN
						Insert into #ChannelReportPopUpDetail
						SELECT  ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(CONCAT(T.Name,' (' + TAC.TalentStatus + ')'),''),Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID 
								Inner Join gen_ContactTalentPriority CTP on His.HiringRequest_ID = CTP.HiringRequestID and His.Talent_ID = CTP.TalentID
								INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = His.TalentStatusID_BasedOnHR--CTP.TalentStatusID_BasedOnHR 
								inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) 
								and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,'')) 
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end 
								and ISNULL(H.IsAccepted,0) = 1 and H.JobStatusID not in (2,4) --H.Status_ID NOT IN (1,4) 
								and Isnull(His.IsActive,0) = 1 and His.Action_ID = 51
								AND not exists (select 1 from gen_InterviewSlotsMaster I with(nolock) WHERE H.ID = I.HiringRequest_ID AND T.ID = I.Talent_ID )
								and TAC.Id not in (5,6) and U.IsActive = 1
								and not exists (select 1 from gen_History His1 with(nolocK) where His1.HiringRequest_ID = His.HiringRequest_ID
												and His1.Talent_ID = His.Talent_ID and His1.Action_ID IN (20,21,22,61,8))
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
					  END
				ELSE
				  BEGIN

						INSERT INTO #ChannelReportPopUpDetail
						SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
								Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
								Isnull(CONCAT(T.Name,' (' + TAC.TalentStatus + ')'),'') ,Isnull(H.Availability,'') [Availability]
								,Isnull(JSC.JobStatus,'') as HRStatus
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID										
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN gen_ContactTalentPriority TP WITH(NOLOCK) ON TP.HiringRequestID= H.ID 
								INNER join gen_Talent T With(Nolock) On T.Id = TP.TalentID							
								INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = TP.TalentStatusID_BasedOnHR 
								INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
								inner join usr_User U With(Nolock) On U.Id = UH.UserID
								inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
								inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
								inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
								INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
								inner join #AllProfileSharedUsersHRWiseActionReport AP on AP.HiringRequestID = H.ID and AP.Talent_ID = T.ID
								INNER JOIN usr_User U1 with(nolock) ON UH.ParentID = U1.ID 
								Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
						WHERE   --H.Status_ID not in (1,4) 
								H.JobStatusID not in (2,4) AND ISNULL(H.IsAccepted,0) = 2 
								and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) 
								and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
								and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
								and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
								AND TP.TalentStatusID_BasedOnHR = 1 and U.IsActive = 1
								and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								--and 
								--(
								--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
								--	Or
								--	(	
								--		@LeadUserID = 0 and 1 = 1
								--	)
								--)
								and 
								(
									(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
									Or
									(	
										@Geos = '' and 1 = 1
									)
								)
				  END	
			END
		ELSE IF @StageName = 'Interview Feedback Pending'
			BEGIN
				if @Option = 1
					BEGIN
							INSERT INTO #ChannelReportPopUpDetail
							SELECT ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
									Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
									Isnull(CONCAT(T.Name,' (' + TAC.TalentStatus + ')'),'')
									,Isnull(H.Availability,'') [Availability]
									,Isnull(JSC.JobStatus,'') as HRStatus
							FROM	gen_SalesHiringRequest H with(nolock) 
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID	
									INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID									
									--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
									Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
									INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
									inner join usr_User U With(Nolock) On U.Id = H.SalesUserID
									inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
									inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
									inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
									INNER join gen_ContactTalentPriority CTP With(NOlock) on CTP.HiringRequestID = H.ID 
									INNER JOIN gen_InterviewSlotsMaster I with(nolock) ON H.ID = I.HiringRequest_ID and I.ID = His.InterviewMaster_ID
									INNER join gen_Talent T With(Nolock) On T.Id = Ctp.TalentID	and I.Talent_ID = T.ID					
									INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = His.TalentStatusID_BasedOnHR--CTP.TalentStatusID_BasedOnHR
									inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID
									INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID 
									Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
							WHERE   ISNULL(H.IsAccepted,0) = 1 and I.InterviewStatus_ID = 6   
									and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) 
									and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,'')) 
									and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))
								    and Isnull(H.IsHiringLimited,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end 
								    --and H.Status_ID NOT IN (1,4) 
									and H.JobStatusID not in (2,4)
									and His.Action_ID = 11 and U.IsActive = 1
									and not exists (select 1 from gen_History His1 with(nolocK) where His1.HiringRequest_ID = His.HiringRequest_ID
											and His1.Talent_ID = His.Talent_ID and His1.Action_ID IN (12,45,46,47,20,21,22,61,8) and His1.InterviewMaster_ID = I.ID)									
									and TAC.Id not in (5,6)
									and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
									--and 
									--(
									--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
									--	Or
									--	(	
									--		@LeadUserID = 0 and 1 = 1
									--	)
									--)
									and 
									(
										(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
										Or
										(	
											@Geos = '' and 1 = 1
										)
									)
									AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
					end
				else 
					BEGIN
							INSERT INTO #ChannelReportPopUpDetail
							SELECT distinct ISNULL(HR_NUMBER,''),ISNULL(U.FullName,''),Isnull(Co.Company,''),
									Isnull(Ur.TalentRole,''), case when H.IsManaged = 1 then 'Managed' else 'Self' end,
									Isnull(T.Name,'') ,Isnull(H.Availability,'') [Availability]
									,Isnull(JSC.JobStatus,'') as HRStatus
							FROM	gen_SalesHiringRequest H with(nolock) 
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID										
									--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
									Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
									INNER JOIN usr_UserHierarchy UH with(nolock) ON H.SalesUserID = UH.UserID
									inner join usr_User U With(Nolock) On U.Id = UH.UserID
									inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
									inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
									inner join prg_TalentRoles Ur With(Nolock) On Ur.Id = HD.Role_ID
									INNER join gen_ContactTalentPriority CTP With(NOlock) on CTP.HiringRequestID = H.ID 
									INNER JOIN gen_InterviewSlotsMaster I with(nolock) ON H.ID = I.HiringRequest_ID
									INNER join gen_Talent T With(Nolock) On T.Id = Ctp.TalentID	and I.Talent_ID = T.ID					
									INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = CTP.TalentStatusID_BasedOnHR
									inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = CTP.TalentID
									INNER JOIN #AllHrchyPopUp AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
									Inner JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
							WHERE   --H.Status_ID NOT IN (1,4) 
									H.JobStatusID in (2,4) and ISNULL(H.IsAccepted,0) = 1 and H.IsActive = 1
									and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) 
									and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
									and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,Isnull(Co.Category,''))
									and Isnull(H.IsHiringLimited,0) =  Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end
									and I.InterviewStatus_ID = 6 and TAC.Id not in (5,6)
									and U.IsActive = 1
									and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
									AND not exists (select 1 from gen_InterviewSlotsMaster I with(nolock) WHERE H.ID = I.HiringRequest_ID AND T.ID = I.Talent_ID and I.InterviewStatus_ID != 7)
									--and 
									--(
									--	(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Co.ID))
									--	Or
									--	(	
									--		@LeadUserID = 0 and 1 = 1
									--	)
									--)
									and 
									(
										(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
										Or
										(	
											@Geos = '' and 1 = 1
										)
									)
					END

					
					 	
			END
		
			


		SET @MainSQL = 'SELECT * from #ChannelReportPopUpDetail  WHERE  ' + @WhereClause + ''
		PRINT(@MainSQL)
		EXECUTE sp_executesql @MainSQL
		drop table #MaxInterviewroundCount
		drop table #MaxInterviewroundCountHrWise
END