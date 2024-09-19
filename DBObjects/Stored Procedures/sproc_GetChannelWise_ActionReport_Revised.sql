
ALTER PROCEDURE [dbo].[sproc_GetChannelWise_ActionReport_Revised] 
@FromStartDate1			nvarchar(20) = NULL,
@FromEndDate1			nvarchar(20) = NULL,
@FromStartDate2			nvarchar(20) = NULL,
@FromEndDate2			nvarchar(20) = NULL,
@IsHiringNeed			nvarchar(5)  = NULL,
@ModeOfWorkId			nvarchar(5)  = NULL,
@TypeOfHR				nvarchar(5)  = NULL,
@CompanyCategory		nvarchar(5)  = NULL,
@Replacement			nvarchar(5)  = NULL,
@Heads					nvarchar(100) = NULL,
@LeadUserID				bigint			= 0,   -- To filter Record based on Lead User (Inbound/Outbound)
@IsHRFocused			bit				= 0,
@Geos	     			nvarchar(20)  = NULL

AS
BEGIN

SET NOCOUNT ON;
	IF @IsHRFocused != 1
		SET @IsHRFocused = NULL;
	BEGIN TRAN T1;
		DECLARE @TRHired INT = 0;
		DECLARE @IsHRTypeDP bit 
		DECLARE @ModeOfWork nvarchar(50) = ''
		DECLARE @LeadUser_UserType BIGINT = 0;
		DECLARE @GeoIDs int = 0
		DECLARE @Geo nvarchar(max) = null

		--User Type ID: 11 is OutBound (BDR)
		--User Type ID: 12 is InBound (MDR/Marketing)
		IF @LeadUserID <> 0
			SET @LeadUser_UserType = ISNULL((SELECT UserTypeID from usr_User with(nolock) where ID = @LeadUserID),0);

		DECLARE @IsReplacement bit = 0
		
		Declare @MainPArentId  as bigint
		IF (@Replacement = '0')
		BEGIN 
			SET @IsReplacement = 0;
		END
		ELSE
		BEGIN
			SET @IsReplacement = 1;
		END

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
			set @ModeOfWork = (SELECT ModeOfWorking FROM prg_ModeOfWorking WITH(NOLOCK)  where ID = @ModeOfWorkId)

			--SELECT @ModeOfWork = ModeOfWorking FROM prg_ModeOfWorking WITH(NOLOCK)  where ID = @ModeOfWorkId
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

		
		BEGIN

			BEGIN--Drop Temp Table if exists.
				IF OBJECT_ID('tempdb..#HRChannelWiseActionWise_Records1') IS NOT NULL
					DROP TABLE #HRChannelWiseActionWise_Records1;
				IF OBJECT_ID('tempdb..#Tbl_SalesManager') IS NOT NULL
					DROP TABLE #Tbl_SalesManager;
				IF OBJECT_ID('tempdb..#HeadTempTbl') IS NOT NULL
					DROP TABLE #HeadTempTbl;
			END

			BEGIN--Create Temp Table.
				CREATE TABLE #HRChannelWiseActionWise_Records1(RowType int,ManagerName NVARCHAR(300), ManagerID bigint, Duration NVARCHAR(100)); 
				CREATE TABLE #Tbl_SalesManager(ID int primary key identity(1,1), SM_ID bigint, SM_Name nvarchar(200));
				CREATE TABLE #HeadTempTbl(HeadID bigint);
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

			BEGIN--Create Columns Stage-Wise.
				
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [HR Active_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [TR Active_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [TR Created_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [TR Information Pending_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [TR Accepted_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [TR Lost_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [TR Cancelled_1] decimal(18,1) default 0;
					

					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [Profile Shared_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [Matchmaking Cancelled_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [Profile Moved to Interview_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [Profile Rejected_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [Talent Interview Completed_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [Talent Hired_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [Talent Reject after Interview_1] decimal(18,1) default 0;
					

					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [SOW Sent_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [SOW Signed_1] decimal(18,1) default 0;
					

					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [Profile Feedback Pending_1] decimal(18,1) default 0;
					ALTER TABLE #HRChannelWiseActionWise_Records1 ADD [Interview Feedback Pending_1] decimal(18,1) default 0;
			END

			IF @Heads <> ''
			BEGIN
				INSERT INTO #HeadTempTbl(HeadID)
					select val from f_split(@Heads,',');
			END
			ELSE
			BEGIN
				--Get All Head Level User of Demand Dept.
				INSERT INTO #HeadTempTbl(HeadID)
					SELECT distinct ID from usr_User with(nolock) where DeptID = 1 and LevelID = 1;
			END
		
			BEGIN--Insert all Sales Managers
				INSERT INTO #Tbl_SalesManager(SM_ID, SM_Name)
				SELECT distinct U.ID, U.FullName + '_Total' from usr_User U with(nolock) 
					--INNER JOIN usr_UserHierarchy UH with(nolock) ON UH.ParentID = U.ID
					INNER JOIN #HeadTempTbl H on U.ID = H.HeadID
					WHERE U.UserTypeID = 9  and U.IsActive = 1 and U.ID <> 370;  --Exclude Navpreet


				INSERT INTO #HRChannelWiseActionWise_Records1(RowType,ManagerName, ManagerID)
					SELECT ROW_NUMBER() OVER(PARTITION BY SM_ID ORDER BY SM_ID ASC) RowID, SM_Name, SM_ID from #Tbl_SalesManager;

				INSERT INTO #HRChannelWiseActionWise_Records1(RowType,ManagerName,ManagerID)
				VALUES (-3,'Final Total',0)

			END

		END

		DECLARE @AllManagerNames NVARCHAR(MAX) = '';
		SET @AllManagerNames = (SELECT stuff((select ',' + '[' + SM_Name + ']' from #Tbl_SalesManager RP for xml path('') ),1,1,''));

		IF Object_Id('temp_db..#HR_ChannelWiseRecords') IS Not Null
		 DROP Table #HR_ChannelWiseRecords;

		IF Object_Id('temp_db..#HR_ChannelWiseRecordsBeforeProfileShared') IS Not Null
		 DROP Table #HR_ChannelWiseRecordsBeforeProfileShared;

		IF OBJECT_ID('tempdb..#AllProfileSharedUsers') Is not null
			DROP TABLE #AllProfileSharedUsers


		CREATE TABLE #HR_ChannelWiseRecordsBeforeProfileShared
		(
			ID						int primary key identity(1,1),
			HR_ID					bigint,
			IsAccepted				int,
			IsAdhocHR				bit,
			HRStatus				int,
			SalesUserID				bigint,
			ParentID				bigint,
			CreatedByDateTime		datetime,
			HR_DetailID				bigint,
			ContactID				bigint,
			IsActive				bit,
			Action_ID				bigint,
			RequiredTR				decimal(18,2) default 0,
			Talent_ID				bigint,
			IsHistoryActive			bit,
			HistoryID				bigint,
			IsManaged				nvarchar(5),
			Availability			NVARCHAR(200),
			HiringNeedType			nvarchar(5),
			ManagerName				NVARCHAR(300),
			MainParentID			bigint,
			IsHRTypeDP				nvarchar(5),
			ModeOfWork				NVARCHAR(50),
			CompanyCategory         nvarchar(5),
			GeoID					int
		)
		
		IF OBJECT_ID('tempdb..#GetLeadUsersCompany') Is not null
			DROP TABLE #GetLeadUsersCompany
				
		CREATE TABLE #GetLeadUsersCompany
		(
			UserID			bigint,
			CompanyId		bigint
		)

		--SELECT * FROM #Tbl_SalesManager;
		select distinct  SM_ID into #SalesManager from #Tbl_SalesManager
		--IF @LeadUserID <> 0
		--	BEGIN					
		--		;WITH RCTE AS
		--		(
		--			SELECT *, 1 AS Lvl 
		--			FROM	usr_UserHierarchy WITH(NOLOCK)
		--			WHERE	UserID = ISNULL(@LeadUserID,0) or ParentID = Isnull(@LeadUserID,0)

		--			UNION ALL

		--			SELECT	rh.*, Lvl+1 AS Lvl 
		--			FROM	dbo.usr_UserHierarchy rh with(nolock)
		--					INNER JOIN RCTE rc ON rh.UserID = rc.ParentId
	
		--		)
		--		SELECT TOP 1 @MainPArentId = p.id
		--		FROM	RCTE r
		--				inner JOIN dbo.usr_User p with(nolock) ON p.id = r.ParentId
		--		WHere   p.IsActive = 1
		--		ORDER BY lvl DESC

		--		;WITH cteUser AS (    
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
		--						INNER JOIN  [dbo].[usr_UserHierarchy] child with(nolock) ON child.UserID = u.ID
		--						INNER JOIN cteUser cte  ON cte.UserID = child.ParentID
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

		--END
		
		IF @LeadUserID <> 0
			BEGIN
					;WITH cteUser AS (    
						SELECT	   u.ID AS UserID, 
									uh.ParentID,
									u.FullName AS Name,
									(select top 1 U.ID from usr_user U with(nolock) where U.ID = TS.SM_ID) as MainParentID,
									0 AS [Level]
						FROM       [dbo].[usr_User] u with(nolock)
									inner join #SalesManager TS ON U.ID = TS.SM_ID    
									Left JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID			
 
						UNION ALL
 
						SELECT u.ID AS UserID,
								child.ParentID,
								u.FullName AS Name,
								cte.MainParentID as MainParentID,
								[Level]+1 AS [Level]
						FROM       [dbo].[usr_User] u with(nolock)
						INNER JOIN  [dbo].[usr_UserHierarchy] child with(nolock) ON child.UserID = u.ID
						INNER JOIN cteUser cte  ON cte.UserID = child.ParentID
					)
				INSERT INTO #HR_ChannelWiseRecordsBeforeProfileShared(HR_ID, IsAccepted, IsAdhocHR, HRStatus, SalesUserID, ParentID, CreatedByDateTime, HR_DetailID, ContactID, IsActive,Action_ID, RequiredTR, Talent_ID, IsHistoryActive, HistoryID,IsManaged, Availability,
																	HiringNeedType, ManagerName, MainParentID, IsHRTypeDP, ModeOfWork,CompanyCategory,GeoID)
				SELECT	distinct H.ID, ISNULL(H.IsAccepted,0) IsAccepted , ISNULL(H.IsAdhocHR, 0) IsAdhocHR,H.JobStatusID as HRStatus,-- H.Status_ID as HRStatus,
						H.SalesUserID,TS.MainParentID, CAST(His.CreatedByDatetime as DaTE) CreatedByDatetime,
						HD.ID, H.ContactID, ISNULL(H.IsActive,0) IsActive , isnull(His.Action_ID,0) Action_ID, cast(case when Availability ='Part Time' then  cast(H.NoofTalents as decimal(18,1))/2   else ISNULL(H.NoofTalents,0) end as decimal(18,1)) RequiredTR, ISNULL(His.Talent_ID,0) Talent_ID, His.IsActive as IsHistoryActive, His.ID as HistoryID 
						,H.IsManaged as IsManaged, ISNULL(H.Availability,'') Availability, ISNULL(H.IsHiringLimited,0) as HiringNeedType, TS.Name as ManagerName, ISNULL(TS.Parentid,0) as MainParentID
						,ISNULL(H.IsHRTypeDP,0) as IsHRTypeDP,ISNULL(DP.ModeOfWork,'') as ModeOfWork,ISNULL(Co.Category,'')
						,Isnull(Co.GEO_ID,0)
				FROM	gen_SalesHiringRequest H with(nolock) 
						INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
						INNER JOIN prg_TalentRoles PTR with(nolock) on HD.Role_ID = PTR.ID
						INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
						--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
						Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
						INNER JOIN cteUser TS ON  H.SalesUserID = TS.UserID
						INNER JOIN usr_User U with(nolock) ON TS.UserID = U.ID 
						inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
						inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
						--INNER JOIN #GetLeadUsersCompany CLU WITH(NOLOCK) ON CLU.CompanyID = CO.ID
						INNER JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
				WHERE	((cast(His.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date))
						OR
						(cast(His.CreatedByDatetime as date) between Cast(@FromStartDate2 as date) and cast(@FromEndDate2 as date)))
						and Isnull(H.IsHRTypeDP,0) =  ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0))
					    and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
					    and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))
						and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
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
						;WITH cteUser AS (    
						SELECT	   u.ID AS UserID, 
									uh.ParentID,
									u.FullName AS Name,
									(select top 1 U.ID from usr_user U with(nolock) where U.ID = TS.SM_ID) as MainParentID,
									0 AS [Level]
						FROM       [dbo].[usr_User] u with(nolock)
									inner join #SalesManager TS ON U.ID = TS.SM_ID    
									LEFT JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID			
 
						UNION ALL
 
						SELECT u.ID AS UserID,
								child.ParentID,
								u.FullName AS Name,
								cte.MainParentID as MainParentID,
								[Level]+1 AS [Level]
						FROM       [dbo].[usr_User] u with(nolock)
						INNER JOIN  [dbo].[usr_UserHierarchy] child with(nolock) ON child.UserID = u.ID
						INNER JOIN cteUser cte  ON cte.UserID = child.ParentID
					)
		
					INSERT INTO #HR_ChannelWiseRecordsBeforeProfileShared(HR_ID, IsAccepted, IsAdhocHR, HRStatus, SalesUserID, ParentID, CreatedByDateTime, HR_DetailID, ContactID, IsActive,Action_ID, RequiredTR, Talent_ID, IsHistoryActive, HistoryID,IsManaged, Availability,
																		HiringNeedType, ManagerName, MainParentID, IsHRTypeDP, ModeOfWork,CompanyCategory,GeoID)
					SELECT	distinct H.ID, ISNULL(H.IsAccepted,0) IsAccepted , ISNULL(H.IsAdhocHR, 0) IsAdhocHR,H.JobStatusID as HRStatus, --H.Status_ID as HRStatus,
							H.SalesUserID,TS.MainParentID, CAST(His.CreatedByDatetime as DaTE) CreatedByDatetime,
							HD.ID, H.ContactID, ISNULL(H.IsActive,0) IsActive , isnull(His.Action_ID,0) Action_ID, cast(case when Availability ='Part Time' then  cast(H.NoofTalents as decimal(18,1))/2   else ISNULL(H.NoofTalents,0) end as decimal(18,1)) RequiredTR, ISNULL(His.Talent_ID,0) Talent_ID, His.IsActive as IsHistoryActive, His.ID as HistoryID 
							,H.IsManaged as IsManaged, ISNULL(H.Availability,'') Availability, ISNULL(H.IsHiringLimited,0) as HiringNeedType, TS.Name as ManagerName, ISNULL(TS.Parentid,0) as MainParentID
							,ISNULL(H.IsHRTypeDP,0) as IsHRTypeDP,ISNULL(DP.ModeOfWork,'') as ModeOfWork,ISNULL(Co.Category,''),
							Isnull(Co.Geo_Id,0)
					FROM	gen_SalesHiringRequest H with(nolock) 
							INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
							INNER JOIN prg_TalentRoles PTR with(nolock) on HD.Role_ID = PTR.ID
							INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
							--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
							Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
							INNER JOIN cteUser TS ON  H.SalesUserID = TS.UserID	
							INNER JOIN usr_User U with(nolock) ON TS.UserID = U.ID 
							inner join gen_Contact C with(Nolock) On C.ID = H.ContactID
							inner join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
							INNER JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
					WHERE  ((cast(His.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date))
						   OR
						   (cast(His.CreatedByDatetime as date) between Cast(@FromStartDate2 as date) and cast(@FromEndDate2 as date)))
						   and Isnull(H.IsHRTypeDP,0) =  ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0))
						   and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
						   and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))
						   and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))	
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


		CREATE TABLE #AllProfileSharedUsers
		(
			UserId			  int  default 0,
			HiringRequestID	  bigint default 0,
			Talent_ID		  bigint,
			IsODR			  bit,
			CTPStatusID			bigint default 0,
			IsReplacement		bit 
		)

		Insert Into #AllProfileSharedUsers(HiringRequestID, Talent_ID, IsODR, CTPStatusID,IsReplacement)
		Select 
		distinct	His.HiringRequest_ID,His.Talent_ID,0 as IsODR, 0 as CTPStatusID,Isnull(His.IsReplacement,0)
		from		gen_History His With(NOlock) 
					--INNER JOIN gen_ContactTalentPriority CTP with(nolock) on His.HiringRequest_ID = CTP.HiringRequestID and His.Talent_ID = CTP.TalentID
					--inner join usr_User U with(nolock) on CTP.CreatedByID = U.ID 
		Where		His.Action_ID = 6 and Isnull(His.IsReplacement,0) = Isnull(@IsReplacement,Isnull(His.IsReplacement,0))
					--and cast(His.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date)
		

		Update  C
		SET		C.CTPStatusID =  ISNULL(CTP.TalentStatusID_BasedOnHR,0)
		FROM    #AllProfileSharedUsers C
				INNER JOIN gen_ContactTalentPriority CTP with(nolock) on C.HiringRequestID = CTP.HiringRequestID and C.Talent_ID = CTP.TalentID
		
		CREATE TABLE #HR_ChannelWiseRecords
		(
			ID						int primary key identity(1,1),
			HR_ID					bigint,
			IsAccepted				int,
			IsAdhocHR				bit,
			HRStatus				int,
			SalesUserID				bigint,
			ParentID				bigint,
			CreatedByDateTime		datetime,
			HR_DetailID				bigint,
			ContactID				bigint,
			IsActive				bit,
			Action_ID				bigint,
			RequiredTR				decimal(18,2) default 0,
			Talent_ID				bigint,
			IsHistoryActive			bit,
			HistoryID				bigint,
			IsManaged				nvarchar(5),
			Availability			NVARCHAR(200),
			HiringNeedType			nvarchar(5),
			ManagerName				NVARCHAR(300),
			ProfileSharedTalentID   bigint,
			IsODR					bit,
			MainParentID			bigint,
			CTPStatusID				bigint default 0,
			IsHRTypeDP				nvarchar(5),
			ModeOfWork				NVARCHAR(50),
			CompanyCategory         nvarchar(5),
			InterViewMaster_ID		bigint,
			GeoId					int
		)	

		IF @LeadUserID > 0 
			BEGIN
				;WITH cteUser_AfterPS AS (    
						SELECT	   u.ID AS UserID, 
								   uh.ParentID,
								   u.FullName AS Name,
								   (select top 1 U.ID from usr_user U with(nolock) where U.ID = TS.SM_ID) as MainParentID,
								   0 AS [Level]
						FROM       [dbo].[usr_User] u with(nolock)
									inner join #SalesManager TS ON U.ID = TS.SM_ID    
								   LEFT JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID			
 
					UNION ALL
 
						SELECT u.ID AS UserID,
								child.ParentID,
								u.FullName AS Name,
								cte.MainParentID as MainParentID,
								[Level]+1 AS [Level]
						FROM       [dbo].[usr_User] u with(nolock)
						INNER JOIN  [dbo].[usr_UserHierarchy] child with(nolock) ON child.UserID = u.ID
						INNER JOIN cteUser_AfterPS cte  ON cte.UserID = child.ParentID
				)

				INSERT INTO #HR_ChannelWiseRecords(HR_ID, IsAccepted, IsAdhocHR, HRStatus, SalesUserID, ParentID, CreatedByDateTime, HR_DetailID, ContactID, IsActive,Action_ID, RequiredTR, Talent_ID, IsHistoryActive, HistoryID,IsManaged, Availability,
											HiringNeedType, ManagerName, ProfileSharedTalentID, IsODR, MainParentID, CTPStatusID, IsHRTypeDP, ModeOfWork,CompanyCategory,InterViewMaster_ID,GeoId)
				SELECT distinct	H.ID, ISNULL(H.IsAccepted,0) IsAccepted , ISNULL(H.IsAdhocHR, 0) IsAdhocHR,H.JobStatusID as HRstatus, --H.Status_ID as HRStatus,
						H.SalesUserID, TS.MainParentID as ParentID, CAST(His.CreatedByDatetime as DaTE) CreatedByDatetime,
						HD.ID, H.ContactID, ISNULL(H.IsActive,0) IsActive , isnull(His.Action_ID,0) Action_ID, cast(case when Availability ='Part Time' then  cast(H.NoofTalents as decimal(18,1))/2   else ISNULL(H.NoofTalents,0) end as decimal(18,1)) RequiredTR, ISNULL(His.Talent_ID,0) Talent_ID, His.IsActive as IsHistoryActive, His.ID as HistoryID 
						,H.IsManaged as IsManaged, ISNULL(H.Availability,'') Availability, ISNULL(H.IsHiringLimited,0) as HiringNeedType, TS.Name as ManagerName , APS.Talent_ID as ProfileSharedTalentID , ISNULL(APS.IsODR,0) as IsODR
						, ISNULL(TS.Parentid,0) as MainParentID, ISNULL(APS.CTPStatusID,0) CTPStatusID
						,ISNULL(H.IsHRTypeDP,0) as IsHRTypeDP,ISNULL(DP.ModeOfWork,'') as ModeOfWork,ISNULL(Co.Category,''),
						Isnull(His.InterviewMaster_ID,0),Isnull(Co.Geo_ID,0)
				FROM	gen_SalesHiringRequest H with(nolock) 
						INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
						INNER JOIN prg_TalentRoles PTR with(nolock) on HD.Role_ID = PTR.ID
						INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
						--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
						Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
						INNER JOIN usr_User U with(nolock) ON H.SalesUserID = U.ID 
						INNER JOIN cteUser_AfterPS TS ON  H.SalesUserID = TS.UserID
						INNER JOIN #AllProfileSharedUsers APS on H.ID = APS.HiringRequestID and His.Talent_ID = APS.Talent_ID
						INNER join gen_Contact C with(Nolock) On C.ID = H.ContactID
						INNER join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
						--INNER JOIN #GetLeadUsersCompany CLU WITH(NOLOCK) ON CLU.CompanyID = CO.ID
						INNER JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
				WHERE  ((cast(His.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date))
					   OR
					  (cast(His.CreatedByDatetime as date) between Cast(@FromStartDate2 as date) and cast(@FromEndDate2 as date)))
					   and Isnull(H.IsHRTypeDP,0) =  ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0))
					   and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
					   and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))
					   and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
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
					;WITH cteUser_AfterPS AS (    
						SELECT	   u.ID AS UserID, 
								   uh.ParentID,
								   u.FullName AS Name,
								   (select top 1 U.ID from usr_user U with(nolock) where U.ID = TS.SM_ID) as MainParentID,
								   0 AS [Level]
						FROM       [dbo].[usr_User] u with(nolock)
									inner join #SalesManager TS ON U.ID = TS.SM_ID    
								    LEFT JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID			
 
						UNION ALL
 
						SELECT u.ID AS UserID,
								child.ParentID,
								u.FullName AS Name,
								cte.MainParentID as MainParentID,
								[Level]+1 AS [Level]
						FROM    [dbo].[usr_User] u with(nolock)
								INNER JOIN  [dbo].[usr_UserHierarchy] child with(nolock) ON child.UserID = u.ID
								INNER JOIN cteUser_AfterPS cte  ON cte.UserID = child.ParentID
				)
				INSERT INTO #HR_ChannelWiseRecords(HR_ID, IsAccepted, IsAdhocHR, HRStatus, SalesUserID, ParentID, CreatedByDateTime, HR_DetailID, ContactID, IsActive,Action_ID, RequiredTR, Talent_ID, IsHistoryActive, HistoryID,IsManaged, Availability,
											HiringNeedType, ManagerName, ProfileSharedTalentID, IsODR, MainParentID, CTPStatusID, IsHRTypeDP, ModeOfWork,CompanyCategory,InterViewMaster_ID,GeoId)
				SELECT distinct	H.ID, ISNULL(H.IsAccepted,0) IsAccepted , ISNULL(H.IsAdhocHR, 0) IsAdhocHR,H.JobStatusID as HRStatus,  -- H.Status_ID as HRStatus,
						H.SalesUserID, TS.MainParentID as ParentID, CAST(His.CreatedByDatetime as DaTE) CreatedByDatetime,
						HD.ID, H.ContactID, ISNULL(H.IsActive,0) IsActive , isnull(His.Action_ID,0) Action_ID, cast(case when Availability ='Part Time' then  cast(H.NoofTalents as decimal(18,1))/2   else ISNULL(H.NoofTalents,0) end as decimal(18,1)) RequiredTR, ISNULL(His.Talent_ID,0) Talent_ID, His.IsActive as IsHistoryActive, His.ID as HistoryID 
						,H.IsManaged as IsManaged, ISNULL(H.Availability,'') Availability, ISNULL(H.IsHiringLimited,0) as HiringNeedType, TS.Name as ManagerName , APS.Talent_ID as ProfileSharedTalentID , ISNULL(APS.IsODR,0) as IsODR
						, ISNULL(TS.Parentid,0) as MainParentID, ISNULL(APS.CTPStatusID,0) CTPStatusID
						,ISNULL(H.IsHRTypeDP,0) as IsHRTypeDP,ISNULL(DP.ModeOfWork,'') as ModeOfWork,ISNULL(Co.Category,''),
						Isnull(His.InterviewMaster_ID,0),ISNULL(Co.Geo_Id,0)
				FROM	gen_SalesHiringRequest H with(nolock) 
						INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
						INNER JOIN prg_TalentRoles PTR with(nolock) on HD.Role_ID = PTR.ID
						INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
						--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
						Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
						INNER JOIN usr_User U with(nolock) ON H.SalesUserID = U.ID 
						INNER JOIN cteUser_AfterPS TS ON  H.SalesUserID = TS.UserID
						INNER JOIN #AllProfileSharedUsers APS on H.ID = APS.HiringRequestID and His.Talent_ID = APS.Talent_ID
						INNER join gen_Contact C with(Nolock) On C.ID = H.ContactID
						INNER join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
						INNER JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
				WHERE  ((cast(His.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date))
					   OR
					   (cast(His.CreatedByDatetime as date) between Cast(@FromStartDate2 as date) and cast(@FromEndDate2 as date)))
					   and Isnull(H.IsHRTypeDP,0) =  ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0))
					   and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,''))
					   and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))
					   and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
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
				
		
		begin --Insert All User Hierarchies in Temp Table

			IF OBJECT_ID('tempdb..#AllHrchy') is not null
				drop table #AllHrchy;

			CREATE TABLE #AllHrchy(UserID bigint, ParentID bigint, MainParentID bigint);

			;WITH cteUser_AllHrchy AS (    
				SELECT	   u.ID AS UserID, 
						   uh.ParentID,
						   u.FullName AS Name,
						   (select top 1 U.ID from usr_user U with(nolock) where U.ID = TS.SM_ID) as MainParentID,
						   0 AS [Level]
				FROM       [dbo].[usr_User] u with(nolock)
							inner join #SalesManager TS ON U.ID = TS.SM_ID    
				           LEFT JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID			
 
			UNION ALL
 
				SELECT u.ID AS UserID,
						child.ParentID,
						u.FullName AS Name,
						cte.MainParentID as MainParentID,
						[Level]+1 AS [Level]
				FROM       [dbo].[usr_User] u with(nolock)
				INNER JOIN  [dbo].[usr_UserHierarchy] child with(nolock) ON child.UserID = u.ID
				INNER JOIN cteUser_AllHrchy cte  ON cte.UserID = child.ParentID
			)

			INSERT INTO #AllHrchy(UserID, ParentID, MainParentID)
				select UserID, ParentID, MainParentID from cteUser_AllHrchy;
		end

		
		select IM.Talent_ID,Max(IM.ID) ID,IM.HiringRequest_ID
		into   #MaxInterviewroundCount
		from   gen_SalesHiringRequest H with(nolock) 
			   INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID
			   inner join gen_InterviewSlotsMaster IM With(NOlOCK) ON IM.HiringRequest_ID = H.ID and HIs.Talent_ID = IM.Talent_ID and His.InterviewMaster_ID = IM.ID
			   INNER join gen_Contact C with(Nolock) On C.ID = H.ContactID
			   INNER join gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
			   INNER JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
		where  ((cast(His.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date))
			    OR
			   (cast(His.CreatedByDatetime as date) between Cast(@FromStartDate2 as date) and cast(@FromEndDate2 as date)))
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

		IF OBJECT_ID('tempdb..#AllHrchyPopUp') is not null
			drop table #AllHrchyPopUp;

		CREATE TABLE #AllHrchyPopUp(UserID bigint, ParentID bigint, MainParentID bigint);

		BEGIN
						;WITH cteUser_AllHrchy AS (    
						SELECT		u.ID AS UserID, 
									uh.ParentID,
									u.FullName AS Name,
									--(select top 1 U.ID from usr_user U where U.ID = TS.SM_ID) as MainParentID,
									H1.HeadID as MainParentID,
									0 AS [Level]
						FROM		[dbo].[usr_User] u with(nolock)
									LEFT JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID
									LEFT JOIN #HeadTempTbl H1 on (uh.ParentID = H1.HeadID or uh.UserID = H1.HeadID)
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
						WHERE		U.IsActive = 1
					)

					INSERT INTO #AllHrchyPopUp(UserID, ParentID, MainParentID)
						select C.UserID, C.ParentID, C.MainParentID from cteUser_AllHrchy C
				END

		IF OBJECT_ID('tempdb..#AllHiredTalentCount') is not null
			drop table #AllHiredTalentCount;

		CREATE TABLE #AllHiredTalentCount(HR_ID bigint,TalentCount int);

		Insert into #AllHiredTalentCount
		select   OT.HiringRequest_ID,Count(1) as TalentCount
		from     gen_OnBoardTalents OT WITH(NOLOCK)
		where    OT.Status_ID = 1
		GROUP BY OT.HiringRequest_ID

		begin --HR Active Block
		PRINT('HR Active');
				begin
					UPDATE H
					SET    H.[HR Active_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.HR_ID),0) AS Value,X.MainParentID FROM (
							SELECT	
									AH.MainParentID,
									COUNT(H.HR_NUMBER) as HR_ID
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
									INNER JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
							WHERE   ISNULL(H.IsActive,0) = 1 And His.Action_ID IN (4,77,83) AND H.JobStatusID <> 2--H.Status_ID NOT IN (3,4,5,6)
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
								GROUP BY AH.MainParentID
							) X  GROUP BY X.MainParentID
							) Z on Z.MainParentID = H.ManagerID
					WHERE H.RowType = 1
				end
		

				--HR Active (Total)
				begin
					UPDATE H
					SET    H.[HR Active_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[HR Active_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				--HR Active (Final Total)
				begin
					UPDATE H
					SET    H.[HR Active_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[HR Active_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --TR Active Block
		PRINT('TR Active');				
				 UPDATE H
					SET    H.[TR Active_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						   inner join 
						   (
							SELECT ISNULL(SUM(X.HR_ID),0) AS HRValue,ISNULL(SUM(X.TRActive),0) as Value,X.MainParentID FROM (
							SELECT	distinct
									UH.ParentID,
									AH.MainParentID,
									H.ID as HR_ID,
									cast(case when Availability ='Part Time' then Cast(Isnull(H.NoofTalents,0) - Isnull(AHT.TalentCount,0) as decimal(18,1)) / 2 else (Isnull(H.NoofTalents,0) - Isnull(AHT.TalentCount,0)) end as decimal(18,1))  as TRActive
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
									AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)	 
									and 
									(
										(@Geos <> '' and exists(select GeoID from #GeoTbl G Where G.GeoID = Co.Geo_ID))
										Or
										(	
											@Geos = '' and 1 = 1
										)
									)
							) X  GROUP BY X.MainParentID
							) Z on Z.MainParentID = H.ManagerID
					WHERE H.RowType = 1
				
				 begin
					UPDATE H
					SET    H.[TR Active_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Active_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end

				--HR Active (Final Total)
				begin
					UPDATE H
					SET    H.[TR Active_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Active_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --TR Created Block
		PRINT('TR Created');
				
				begin
					UPDATE H
					SET    H.[TR Created_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.TRCreated),0)  AS Value,X.ParentID FROM (
							SELECT	
										H.ParentID,
										MAX(H.RequiredTR) as TRCreated, 
										H.ManagerName,
										H.HR_ID
							FROM		#HR_ChannelWiseRecordsBeforeProfileShared H with(nolock) 
							WHERE		Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0))
										and Isnull(H.ModeOfWork,'') = ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) 
										and Isnull(H.CompanyCategory,'') = ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,''))
										and Isnull(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
										And ISNULL(H.IsActive,0) = 1 and H.Action_ID in (4,77,83) 										
							GROUP BY	H.ParentID,H.ManagerName,H.HR_ID,H.Availability
							
							UNION ALL

							SELECT	
										H.ParentID,
										cast(case when H.Availability ='Part Time' then  cast(MAX(TRU.UpdatedTR) as decimal(18,1))/2   else ISNULL(MAX(TRU.UpdatedTR),0) end as decimal(18,1)) as TRCreated, 
										H.ManagerName,
										H.HR_ID
							FROM		#HR_ChannelWiseRecordsBeforeProfileShared H with(nolock) 
										INNER JOIN gen_SalesHR_TRUpdated_Details TRU with(nolock) ON H.HR_ID = TRU.HiringRequestId AND H.HistoryID = TRU.HistoryID AND TRU.IsIncreased = 1
							WHERE		Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0))
										and Isnull(H.ModeOfWork,'') = ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) 
										and Isnull(H.CompanyCategory,'') = ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,''))
										and Isnull(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
										And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 79 
							GROUP BY	H.ParentID,H.ManagerName,H.HR_ID,H.Availability

							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end
		
				--TR Created (Total)
				begin
					UPDATE H
					SET    H.[TR Created_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Created_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end

				--TR Created (Final Total)
				begin
					UPDATE H
					SET    H.[TR Created_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Created_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --TR Information Pending
		PRINT('TR Information Pending');
				
				begin
					UPDATE H
					SET    H.[TR Information Pending_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT COUNT(X.HR_ID) AS Value,X.ParentID FROM (
							SELECT	
									H.ParentID,
									H.HR_ID, 
									H.ManagerName
							FROM	#HR_ChannelWiseRecordsBeforeProfileShared H with(nolock) 
							WHERE (cast(H.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date))
								  and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(H.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) 
								  and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,''))
								  and ISNULL(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
								  and ISNULL(H.IsAccepted,0) = 2  And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 44
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end
		

				--HR Active (Total)
				begin
					UPDATE H
					SET    H.[TR Information Pending_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Information Pending_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				--HR Active (Final Total)
				begin
					UPDATE H
					SET    H.[TR Information Pending_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Information Pending_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --TR Accepted
		PRINT('TR Accepted');
				begin


					UPDATE H
					SET    H.[TR Accepted_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.TRAcceptedCount),0) AS Value,X.ParentID FROM (
						
							SELECT	H.ParentID,
									--Max(TRU.UpdatedTR) as TRAcceptedCount,
									cast(case when H.Availability ='Part Time' then  cast(sum(TRU.UpdatedTR) as decimal(18,1))/2   else SUM(ISNULL(TRU.UpdatedTR,0)) end as decimal(18,1)) as TRAcceptedCount, 
									H.ManagerName, H.HR_ID
							FROM	#HR_ChannelWiseRecordsBeforeProfileShared H with(nolock) 
									--INNER JOIN gen_SalesHiringRequest H1 with(nolock) on H.HR_ID = H1.ID
									LEFT JOIN gen_SalesHR_TRUpdated_Details TRU with(nolock) ON H.HR_ID = TRU.HiringRequestId 
							WHERE (cast(TRU.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date))
								  and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(H.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) 
								  and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,'')) 
								  and ISNULL(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
								  and ISNULL(H.IsAccepted,0) = 1  And ISNULL(H.IsActive,0) = 1 
								  and H.Action_ID = 79
								  group by H.ParentID,H.ManagerName,H.HR_ID,H.Availability
							union all

								SELECT	distinct
									H.ParentID,
									cast(case when H.Availability ='Part Time' then  cast(H1.TR_Accepted as decimal(18,1))/2   else ISNULL(H1.TR_Accepted,0) end as decimal(18,1)) as TRAcceptedCount, 
									H.ManagerName, H1.HiringRequest_ID
							FROM	#HR_ChannelWiseRecordsBeforeProfileShared H with(nolock) 
									INNER JOIN gen_SalesHR_TRAccepted_Details H1 with(nolock) on H.HR_ID = H1.HiringRequest_ID
							WHERE (cast(H1.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date))
								  and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(H.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) 
								  and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,'')) 
								  and ISNULL(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
								  and ISNULL(H.IsAccepted,0) = 1  And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 58
								  and H1.TR_Accepted > 0
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end
		

				--TR Accepted (Total)
				begin
					UPDATE H
					SET    H.[TR Accepted_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Accepted_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				--TR Accepted (Final Total)
				begin
					UPDATE H
					SET    H.[TR Accepted_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Accepted_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --TR Lost
		PRINT('TR Lost');				
				BEGIN

					UPDATE H
					SET    H.[TR Lost_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(X.TRLostCount) AS Value,X.ParentID 
							FROM (
								SELECT	DISTINCT
										H.ParentID,
										cast(case when H.Availability ='Part Time' then  cast(TRA.UpdatedTR as decimal(18,1))/2   else ISNULL(TRA.UpdatedTR,0) end as decimal(18,1)) as TRLostCount, 
										H.ManagerName, H.HR_ID
								FROM	#HR_ChannelWiseRecordsBeforeProfileShared H with(nolock) 
										LEFT JOIN gen_SalesHR_TRUpdated_Details TRA with(nolock) ON  H.HR_ID = TRA.HiringRequestID 
			    				WHERE  Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(H.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) 
									   and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,'')) 
									   and ISNULL(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
									   and H.Action_ID in( 80,82) 
									   and TRA.IsDecreased = 1 and TRA.IsTRLost = 1		
									   and cast(TRA.CreatedByDateTime as date) between Cast(@FromStartDate1 as date) and Cast(@FromEndDate1 as date)
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end
		

				--TR Lost (Total)
				begin
					UPDATE H
					SET    H.[TR Lost_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Lost_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- TR Lost (Final Total)
				begin
					UPDATE H
					SET    H.[TR Lost_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Lost_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --TR Cancelled
		PRINT('TR Cancelled')				
				begin
					UPDATE H
					SET    H.[TR Cancelled_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(X.TRCancelledCount) AS Value,X.ParentID FROM (
							SELECT	
									H.ParentID,
									cast(case when H.Availability ='Part Time' then  cast(TRA.UpdatedTR as decimal(18,1))/2   else ISNULL(TRA.UpdatedTR,0) end as decimal(18,1)) as TRCancelledCount, 
									H.ManagerName--, H1.ID
							FROM	#HR_ChannelWiseRecordsBeforeProfileShared H with(nolock) 
									LEFT JOIN gen_SalesHR_TRUpdated_Details TRA with(nolock) ON  H.HR_ID = TRA.HiringRequestID 
			    			WHERE  Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(H.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) 
								   and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,'')) 
								   and ISNULL(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
								   and H.Action_ID In( 80,82) and TRA.IsDecreased = 1 and TRA.IsTRCancel = 1
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end
		

				--TR Cancelled (Total)
				begin
					UPDATE H
					SET    H.[TR Cancelled_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Cancelled_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- TR Cancelled (Final Total)
				begin
					UPDATE H
					SET    H.[TR Cancelled_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[TR Cancelled_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --Profile Shared
		PRINT('Profile Shared')				
			 begin
					
					UPDATE H
					SET    H.[Profile Shared_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.HR_ID),0) AS Value,X.ParentID FROM (
							SELECT	distinct 
									H.ParentID,
									COUNT(H.HR_ID) HR_ID,
									H.ManagerName,
									H.ProfileSharedTalentID
							FROM	#HR_ChannelWiseRecords H with(nolock) 
									inner join gen_Talent T With(Nolock) On T.ID = H.ProfileSharedTalentID
									inner join prg_TalentStatus_AfterClientSelection TAC with(nolock) on TAC.ID = H.CTPStatusID
							WHERE	ISNULL(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
									and ISNULL(H.IsAccepted,0) = 1 and H.HRStatus != 4 And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 18
									and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,'')) 
							GROUP BY H.ParentID, H.ManagerName, H.ProfileSharedTalentID
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end
		

				--TR Cancelled (Total)
				begin
					UPDATE H
					SET    H.[Profile Shared_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Profile Shared_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- TR Cancelled (Final Total)
				begin
					UPDATE H
					SET    H.[Profile Shared_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Profile Shared_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --Matchmaking Cancelled
		PRINT('Matchmaking Cancelled')
				begin									
					UPDATE H
					SET    H.[Matchmaking Cancelled_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.HR_ID),0) AS Value,X.ParentID FROM (
							SELECT	H.ParentID,
									COUNT(distinct H1.ID) HR_ID,
									H.ManagerName,
									H.ProfileSharedTalentID
							FROM	#HR_ChannelWiseRecords H with(nolock) 
									INNER JOIN gen_SalesHiringRequest H1 with(nolocK) on H.HR_ID = H1.ID
									inner join gen_Talent T With(Nolock) On T.ID = H.ProfileSharedTalentID
									inner join prg_TalentStatus_AfterClientSelection TAC with(nolock) on TAC.ID = H.CTPStatusID
									--LEFT JOIN gen_SalesHR_TRUpdated_Details TRA with(nolock) ON  H.HR_ID = TRA.HiringRequestID and TRA.IsDecreased = 1 
							WHERE   Isnull(H.HiringNeedType,0) = Case when @IsHiringNeed = 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
								    and ISNULL(H.IsAccepted,0) = 1 and H.HRStatus !=4 And ISNULL(H.IsActive,0) = 1 and H.Action_ID in (20,80,81)
								    and Isnull(H.CompanyCategory,'') = ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,'')) 
									and cast(H.CreatedByDateTime as date) between Cast(@FromStartDate1 as date) and Cast(@FromEndDate1 as date) 
							GROUP BY H.ParentID, H.ManagerName, H.ProfileSharedTalentID,H1.ID
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end
		

				--TR Cancelled (Total)
				begin
					UPDATE H
					SET    H.[Matchmaking Cancelled_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Matchmaking Cancelled_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- Matchmaking Cancelled (Final Total)
				begin
					UPDATE H
					SET    H.[Matchmaking Cancelled_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Matchmaking Cancelled_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --Profile Moved to Interview
		PRINT('Profile Moved to Interview')				
			begin
					UPDATE H
					SET    H.[Profile Moved to Interview_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.CNT),0) AS Value,X.ParentID FROM (
							SELECT	
									H.ParentID,
									--H.RequiredTR,
									COUNT(H.HR_ID) CNT,
									H.ManagerName
							FROM	#HR_ChannelWiseRecords H with(nolock) 
									inner join gen_Talent T With(Nolock) On T.ID = H.ProfileSharedTalentID
									inner join Gen_InterviewSlotsMaster I WITH(NOLOCK) ON I.HiringRequest_ID = H.Hr_ID and I.Talent_ID = T.ID 
										and I.ID = H.InterViewMaster_ID
									INNER JOIN #MaxInterviewroundCount MIC ON MIC.HiringRequest_ID = I.HiringRequest_ID and MIC.Talent_ID = I.Talent_ID and MIC.ID = I.ID
							WHERE	Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) 
									and Isnull(H.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) 
									and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,''))
									and Isnull(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
									and ISNULL(H.IsAccepted,0) = 1 and H.HRStatus != 4 And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 9
									and I.InterviewStatus_ID <> 3
							GROUP BY H.ParentID, H.ManagerName
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end

				--Profile Moved to Interview (Total)
				begin
					UPDATE H
					SET    H.[Profile Moved to Interview_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Profile Moved to Interview_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- Profile Moved to Interview(Final Total)
				begin
					UPDATE H
					SET    H.[Profile Moved to Interview_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Profile Moved to Interview_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --Profile Rejected
		PRINT('Profile Rejected')				
			begin
					UPDATE H
					SET    H.[Profile Rejected_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.HR_ID),0) AS Value,X.ParentID FROM (
							SELECT	
									H.ParentID,
									COUNT(H.HR_ID) HR_ID,
									H.ManagerName
							FROM	#HR_ChannelWiseRecords H with(nolock) 
									inner join gen_Talent T With(Nolock) On T.ID = H.ProfileSharedTalentID
							WHERE	Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(H.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,''))
									and ISNULL(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
									and ISNULL(H.IsAccepted,0) = 1 and H.HRStatus != 4 and H.Action_ID = 22
							GROUP BY H.ParentID, H.ManagerName
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end


				--Profile Rejected (Total)
				begin
					UPDATE H
					SET    H.[Profile Rejected_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Profile Rejected_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- Profile Rejected(Final Total)
				begin
					UPDATE H
					SET    H.[Profile Rejected_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Profile Rejected_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --Talent Interview Completed
		PRINT('Talent Interview Completed')				
			begin
					UPDATE H
					SET    H.[Talent Interview Completed_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.CNT),0) AS Value,X.ParentID FROM (
							SELECT	
									H.ParentID,
									COUNT(H.HR_ID) CNT,
									H.ManagerName
							FROM	#HR_ChannelWiseRecords H with(nolock) 
									inner join gen_Talent T With(Nolock) On T.ID = H.ProfileSharedTalentID
									inner join Gen_InterviewSlotsMaster I WITH(NOLOCK) ON I.HiringRequest_ID = H.Hr_ID and I.Talent_ID = T.ID 
									and I.ID = H.InterViewMaster_ID
									INNER JOIN #MaxInterviewroundCount MIC ON MIC.HiringRequest_ID = I.HiringRequest_ID and MIC.Talent_ID = I.Talent_ID and MIC.ID = I.ID
							WHERE Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(H.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,''))
								  and ISNULL(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
								  and ISNULL(H.IsAccepted,0) = 1 and H.HRStatus != 4 And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 11
							GROUP BY H.ParentID, H.ManagerName
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end

				--Talent Interview Completed (Total)
				begin
					UPDATE H
					SET    H.[Talent Interview Completed_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Talent Interview Completed_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- Talent Interview Completed(Final Total)
				begin
					UPDATE H
					SET    H.[Talent Interview Completed_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Talent Interview Completed_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --Talent Hired
		PRINT('Talent Hired')				
			begin
					UPDATE H
					SET    H.[Talent Hired_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.HR_ID),0) AS Value,X.ParentID FROM (
							SELECT	
									H.ParentID,
									COUNT(H.HR_ID) HR_ID,
									H.ManagerName
							FROM	#HR_ChannelWiseRecords H with(nolock) 
									inner join gen_Talent T With(Nolock) On T.ID = H.ProfileSharedTalentID
							WHERE  Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(H.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,''))
								   and ISNULL(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
								   and ISNULL(H.IsAccepted,0) = 1 and H.HRStatus != 4 And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 12
							GROUP BY H.ParentID, H.ManagerName
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end


				--Talent Hired (Total)
				begin
					UPDATE H
					SET    H.[Talent Hired_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Talent Hired_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- Talent Interview Completed(Final Total)
				begin
					UPDATE H
					SET    H.[Talent Hired_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Talent Hired_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --Talent Reject after Interview
		PRINT('Talent Reject after Interview')				
			begin
					UPDATE H
					SET    H.[Talent Reject after Interview_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.HR_ID),0) AS Value,X.ParentID FROM (
							SELECT	
									H.ParentID,
									COUNT(H.HR_ID) HR_ID,
									H.ManagerName
							FROM	#HR_ChannelWiseRecords H with(nolock) 
									inner join gen_Talent T With(Nolock) On T.ID = H.ProfileSharedTalentID
							WHERE  Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(H.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(H.ModeOfWork,'')) and Isnull(H.CompanyCategory,'') =ISNULL(@CompanyCategory,ISNULL(H.CompanyCategory,''))
								   and ISNULL(H.HiringNeedType,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.HiringNeedType,0) end 
								   and ISNULL(H.IsAccepted,0) = 1 and H.HRStatus != 4 And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 45
							GROUP BY H.ParentID, H.ManagerName
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end


				--Talent Reject after Interview (Total)
				begin
					UPDATE H
					SET    H.[Talent Reject after Interview_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Talent Reject after Interview_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- Talent Reject after Interview(Final Total)
				begin
					UPDATE H
					SET    H.[Talent Reject after Interview_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Talent Reject after Interview_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end
				
		begin --SOW Sent
		PRINT('SOW Sent')				
			begin
					UPDATE H
					SET    H.[SOW Sent_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.CNT),0) AS Value,X.ParentID FROM (
							SELECT	distinct
									--UH.ParentID,
									AH.MainParentID as ParentID,
									H.ID as HR_ID,
									ISNULL(g3.AM_SalesPersonID,0) AM_SalesPersonID,
									ISNULL(SUM(CASE WHEN H.Availability = 'Part Time' THEN ISNULL(g3.SOWCount,0)/2 ELSE ISNULL(g3.SOWCount,0) end),0) as  CNT,
									U1.FullName as ManagerName,
									T.ID as Talent_ID
							FROM	gen_SalesHiringRequest H with(nolock) 
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
									INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
									--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
									Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
									INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID 
									INNER JOIN gen_OnBoardTalents g3 with(nolock) ON H.ID = g3.HiringRequest_ID AND T.ID = g3.Talent_ID 
									INNER JOIN #AllProfileSharedUsers APS on H.ID = APS.HiringRequestID and APS.Talent_ID = g3.Talent_ID
									INNER JOIN usr_UserHierarchy UH with(nolock) ON ISNULL(g3.AM_SalesPersonID,0) = UH.UserID
									INNER JOIN #AllHrchy AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
									INNER JOIN usr_User U1 with(nolock) ON UH.ParentID = U1.ID 
									INNER JOIN gen_Contact C with(Nolock) On C.ID = H.ContactID
									INNER JOIN gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
									INNER JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
							WHERE	(cast(His.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date))
								    and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,'')) 
									and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))
									and Isnull(H.IsHiringLimited,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end 
									and ISNULL(H.IsAccepted,0) = 1 and H.Status_ID != 1 and His.Action_ID = 29
									AND ISNULL(g3.Status_ID,0) = 1 
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
							GROUP BY AH.MainParentID, U1.FullName, H.ID,g3.AM_SalesPersonID, T.ID
						) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end


				--SOW Sent (Total)
				begin
					UPDATE H
					SET    H.[SOW Sent_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[SOW Sent_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- SOW Sent(Final Total)
				begin
					UPDATE H
					SET    H.[SOW Sent_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[SOW Sent_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --SOW Signed
		PRINT('SOW Signed')				
			begin
					
					UPDATE H
					SET    H.[SOW Signed_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.CNT),0) AS Value,X.ParentID FROM (
							SELECT	distinct
									--UH.ParentID,
									AH.MainParentID as ParentID,
									H.ID as HR_ID,
									ISNULL(g3.AM_SalesPersonID,0) AM_SalesPersonID,
									ISNULL(SUM(CASE WHEN H.Availability = 'Part Time' THEN ISNULL(g3.SOWCount,0)/2 ELSE ISNULL(g3.SOWCount,0) end),0) as  CNT,
									U1.FullName as ManagerName,
									T.ID as Talent_ID
							FROM	gen_SalesHiringRequest H with(nolock) 
									INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
									INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
									--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
									Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
									INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID 
									INNER JOIN gen_OnBoardTalents g3 with(nolock) ON H.ID = g3.HiringRequest_ID AND T.ID = g3.Talent_ID 
									INNER JOIN #AllProfileSharedUsers APS on H.ID = APS.HiringRequestID and APS.Talent_ID = g3.Talent_ID
									INNER JOIN usr_UserHierarchy UH with(nolock) ON ISNULL(g3.AM_SalesPersonID,0) = UH.UserID
									--INNER JOIN #AllHrchyPopUp AH on  (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
									INNER JOIN #AllHrchy AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
									INNER JOIN usr_User U1 with(nolock) ON UH.ParentID = U1.ID 
									--INNER JOIN gen_OnBoardTalents_LegalDetails g with(nolock) ON H.ID = g.HiringRequest_ID and g.OnBoardID = g3.ID	
									INNER JOIN gen_Contact C with(Nolock) On C.ID = H.ContactID
									INNER JOIN gen_Company Co WIth(Nolock) On Co.ID = C.CompanyID
									--left JOIN gen_CompanyLegalInfo g2 ON g.CompanyLegalID = g2.ID AND g2.AgreementStatus = 'Signed'
									INNER JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
							WHERE (cast(His.CreatedByDatetime as date) between Cast(@FromStartDate1 as date) and cast(@FromEndDate1 as date))
								  and Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,'')) 
								  and Isnull(H.IsHiringLimited,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end 
								  and ISNULL(H.IsAccepted,0) = 1 and H.JobStatusID <> 4 and His.Action_ID = 35
								  AND ISNULL(g3.Status_ID,0) = 1 
								  and ISNULL(g3.Talent_ID,0) > 0
								  and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))	
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
							GROUP BY AH.MainParentID, U1.FullName, H.ID,g3.AM_SalesPersonID, T.ID
							) X  GROUP BY X.ParentID
							) Z on Z.ParentID = H.ManagerID
					WHERE H.RowType = 1
				end


				--SOW Signed (Total)
				begin
					UPDATE H
					SET    H.[SOW Signed_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[SOW Signed_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- SOW Sent(Final Total)
				begin
					UPDATE H
					SET    H.[SOW Signed_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[SOW Signed_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin --Profile Feedback pending
		PRINT('Profile Feedback pending')				
			begin
					UPDATE H
					SET    H.[Profile Feedback Pending_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.ProWaitFBCount),0) AS Value,X.MainParentID FROM (
							SELECT										
									COUNT(His.Talent_ID) ProWaitFBCount,
									U1.FullName as ManagerName,
									AH.MainParentID
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
									INNER join gen_Talent T With(Nolock) On T.Id = His.Talent_ID 
									INNER join gen_ContactTalentPriority CTP With(NOlock) on CTP.HiringRequestID = H.ID  and His.Talent_ID = CTP.TalentID													
									INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = His.TalentStatusID_BasedOnHR --CTP.TalentStatusID_BasedOnHR 
									inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID
									INNER JOIN #AllHrchy AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
									INNER JOIN usr_User U1 with(nolock) ON UH.ParentID = U1.ID 
									INNER JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID
							WHERE  Isnull(H.IsHRTypeDP,0) = ISNULL(@IsHRTypeDP,Isnull(H.IsHRTypeDP,0)) 
								   and Isnull(DP.ModeOfWork,'') =ISNULL(@ModeOfWork,Isnull(DP.ModeOfWork,'')) 
								   and Isnull(Co.Category,'') =ISNULL(@CompanyCategory,ISNULL(Co.Category,''))
								   and Isnull(H.IsHiringLimited,0) = Case when @IsHiringNeed= 1 then 1 when @IsHiringNeed = 2 then 0 else Isnull(H.IsHiringLimited,0) end 
								   and ISNULL(H.IsAccepted,0) = 1 and H.JobStatusID not in (2,4) --and H.Status_ID NOT IN (1,4) 
								   and Isnull(His.IsActive,0) = 1 and His.Action_ID = 51
								   AND not exists (select 1 from gen_InterviewSlotsMaster I with(nolock) WHERE H.ID = I.HiringRequest_ID AND T.ID = I.Talent_ID )
								   and TAC.Id not in (5,6) and U.IsActive = 1
								   and not exists (select 1 from gen_History His1 with(nolocK) where His1.HiringRequest_ID = His.HiringRequest_ID
													and His1.Talent_ID = His.Talent_ID and His1.Action_ID IN (20,21,22,61,8))
								   and Isnull(HD.IsHRFocused,0) = Isnull(@IsHRFocused,Isnull(HD.IsHRFocused,0))
								 --  and 
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
							GROUP BY  U1.FullName,AH.MainParentID
							) X  GROUP BY X.MainParentID
							) Z on Z.MainParentID = H.ManagerID
					WHERE H.RowType = 1
				end

				--Profile Feedback pending (Total)
				begin
					UPDATE H
					SET    H.[Profile Feedback Pending_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Profile Feedback Pending_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- Profile Feedback pending(Final Total)
				begin
					UPDATE H
					SET    H.[Profile Feedback Pending_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Profile Feedback Pending_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		begin -- Interview Feedback Pending
		PRINT('Interview Feedback Pending')				
			begin
					UPDATE H
					SET    H.[Interview Feedback Pending_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT ISNULL(SUM(X.CNT),0) AS Value,X.MainParentID FROM (
							SELECT	
									UH.ParentID,
									COUNT(CTP.ID) CNT,
									U1.FullName as ManagerName,
									AH.MainParentID
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
									INNER join gen_ContactTalentPriority CTP With(NOlock) on CTP.HiringRequestID = H.ID 
									INNER JOIN gen_InterviewSlotsMaster I with(nolock) ON H.ID = I.HiringRequest_ID and I.ID = His.InterviewMaster_ID
									INNER join gen_Talent T With(Nolock) On T.Id = Ctp.TalentID	and I.Talent_ID = T.ID					
									INNER JOIN prg_TalentStatus_AfterClientSelection TAC WITH(NOLOCK) ON TAC.ID = His.TalentStatusID_BasedOnHR --CTP.TalentStatusID_BasedOnHR									
									inner join #AllProfileSharedUsers AP on AP.HiringRequestID = H.ID and AP.Talent_ID = His.Talent_ID
									INNER JOIN #AllHrchy AH on (UH.ParentID = AH.ParentID OR UH.ParentID = AH.MainParentID) and UH.UserID = AH.UserID
									INNER JOIN usr_User U1 with(nolock) ON UH.ParentID = U1.ID 
									INNER JOIN  gen_Direct_Placement DP WITH(NOLOCK) ON DP.HiringRequest_Id =H.ID									
							WHERE	ISNULL(H.IsAccepted,0) = 1 and I.InterviewStatus_ID = 6   
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
							GROUP BY UH.ParentID, U1.FullName,AH.MainParentID
							) X  GROUP BY X.MainParentID
							) Z on Z.MainParentID = H.ManagerID
					WHERE H.RowType = 1
				end
				--Interview Feedback Pending(Total)
				begin
					UPDATE H
					SET    H.[Interview Feedback Pending_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Interview Feedback Pending_1]) as Value,P.ManagerID from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
							GROUP BY P.ManagerID 
						  ) Z on 1 = 1 AND H.ManagerID = Z.ManagerID
					WHERE H.RowType = 3
				end


				-- Interview Feedback Pending(Final Total)
				begin
					UPDATE H
					SET    H.[Interview Feedback Pending_1] = Z.Value
					FROM   #HRChannelWiseActionWise_Records1 H 
						  inner join 
						  (
							SELECT SUM(P.[Interview Feedback Pending_1]) as Value from #HRChannelWiseActionWise_Records1 P
							WHERE P.RowType = 1
						  ) Z on 1 = 1 AND H.ManagerID = 0
					WHERE H.RowType = -3
				end

		end

		UPDATE #HRChannelWiseActionWise_Records1 SET Duration = @FromStartDate1 + ' To ' + @FromEndDate1;
		DECLARE @Sql NVARCHAR(MAX) = '';

		begin--All Pivot Code
				IF OBJECT_ID('tempdb..#FinalTbl') IS NOT NULL
					DROP TABLE #FinalTbl;

				CREATE TABLE #FinalTbl(Stage nvarchar(200),[Additional Info] Nvarchar(max) default '',Duration nvarchar(200),[Final Total] decimal(18,1) default 0.0)

				SET @Sql = (SELECT stuff((select ';' + 'ALTER TABLE  #FinalTbl ADD [' + SM_Name + '] decimal(18,1) default 0.0' from #Tbl_SalesManager RP for xml path('') ),1,1,''));
				exec sp_executesql @Sql;
		
				
				INSERT INTO #FinalTbl(Stage,Duration)
				VALUES ('','');

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''HR Active_1'' as Stage,''Those HR that are created (post draft) till they are completed, cancelled or on hold will be counted here | Here reopens will also be counted'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[HR Active_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([HR Active_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''TR Active_1'' as Stage,''Number of TR for above HR, here if we update the TR count, it should reflect. Also if more than one TR is there (say 3) and we hired 1 developer in same, then TR count should be total - hired (3 - 1 = 2) as the Hr will be still active.  | Even in case of edit TR for example in initial HR -> Required TR Were 3 and later it becomes 5, So with 1 deployed in this hr the active TR acount if we see today would be 4.'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[TR Active_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([TR Active_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''TR Created_1'' as Stage,''Number of TR created during above dates filter | Count reopens only those TR with which it reopens, if we have duplicate value open and reopen we will have maximum number of TR if its via reopen or via open (depending on reopen you increase TR or decrease tr)'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[TR Created_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([TR Created_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''TR Information Pending_1'' as Stage,''Number of TR moved into information pending during the above date filter.'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[TR Information Pending_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([TR Information Pending_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;
		
				SET @Sql ='INSERT INTO #FinalTbl SELECT ''TR Accepted_1'' as Stage,''Number of TR accepted during the above dates filter'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[TR Accepted_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([TR Accepted_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''TR Lost_1'' as Stage,''Number of TR marked as loss during the above dates filter + Reduced as per condition'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[TR Lost_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([TR Lost_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				
				SET @Sql ='INSERT INTO #FinalTbl SELECT ''TR Cancelled_1'' as Stage,''Number of TR (reduced as per cancelled conditions, cancelled)'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[TR Cancelled_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([TR Cancelled_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''Profile Shared_1'' as Stage,''Number of Talent moved to shortlisted talent during above date filter'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[Profile Shared_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([Profile Shared_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''Matchmaking Cancelled_1'' as Stage,''Number of talents who are shortlisted but are removed from mapping manually or are moved to auto cancelled if someone is hired in same HR or if he is hired into some other HR, again all happening this within above date filter'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[Matchmaking Cancelled_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([Matchmaking Cancelled_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''Profile Moved to Interview_1'' as Stage,''Number of Talent whose interview is scheduled in above date (the interview date can be different, but show when the interview is scheduled), also we will consider here as 1st entry, next time another round happens it stays here, if reschedule happens it stays here, only if rejected or hired happens or on hold happens it moves further.'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[Profile Moved to Interview_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([Profile Moved to Interview_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''Profile Rejected_1'' as Stage,''Number of talent whose profile is rejected during above dates'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[Profile Rejected_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([Profile Rejected_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''Talent Interview Completed_1'' as Stage,''Number of interview that gets completed when manually marked as completed during above filter dates'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[Talent Interview Completed_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([Talent Interview Completed_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;
				--Talent Hired

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''Talent Hired_1'' as Stage,''Number of talents marked as Hired in above dates'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[Talent Hired_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([Talent Hired_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				-- Talent Reject after Interview
				
				SET @Sql ='INSERT INTO #FinalTbl SELECT ''Talent Reject after Interview_1'' as Stage,''Number of talents marked as rejected in above dates, only if interview is scheduled earlier, if not it will fall under profile reject'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[Talent Reject after Interview_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([Talent Reject after Interview_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				-- SOW Sent

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''SOW Sent_1'' as Stage,''Number of Talent / Client engagment where preonboarding of client and talent is done'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[SOW Sent_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([SOW Sent_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				-- SOW Signed


				SET @Sql ='INSERT INTO #FinalTbl SELECT ''SOW Signed_1'' as Stage,''Number of Talent / Client engagment where client sow is signed'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[SOW Signed_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([SOW Signed_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''Profile Feedback Pending_1'' as Stage,''All the active HR, where talents are in profile shotlisted stage at any particular state irrespective of date filter'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[Profile Feedback Pending_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([Profile Feedback Pending_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				SET @Sql ='INSERT INTO #FinalTbl SELECT ''Interview Feedback Pending_1'' as Stage,''All the active HR, where talents are in interview done stage at any particular state irrespective of date filter + if the interview scheduled is there and interview date is passed 3 hours of interview time and date then it should reflect in interview feedback pending'' as [Additional Info],Duration,[Final Total],   '
						+ @AllManagerNames + ' 
					FROM  
					(
						SELECT ManagerName,[Interview Feedback Pending_1],Duration FROM #HRChannelWiseActionWise_Records1
					) AS SourceTable  
					PIVOT  
					(  
						AVG([Interview Feedback Pending_1]) 
						FOR ManagerName IN (' + @AllManagerNames + ',[Final Total],[Pool Total],[Odr Total])  
					) AS PivotTable;'

				exec sp_executesql @Sql;

				
		end
		
		SELECT * FROM #FinalTbl;

		drop table #MaxInterviewroundCount

		BEGIN--Drop Temp Table if exists.
				IF OBJECT_ID('tempdb..#HRChannelWiseActionWise_Records1') IS NOT NULL
					DROP TABLE #HRChannelWiseActionWise_Records1;
				IF OBJECT_ID('tempdb..#Tbl_SalesManager') IS NOT NULL
					DROP TABLE #Tbl_SalesManager;
				IF OBJECT_ID('tempdb..#HeadTempTbl') IS NOT NULL
					DROP TABLE #HeadTempTbl;
			END

	COMMIT TRAN T1;
END
