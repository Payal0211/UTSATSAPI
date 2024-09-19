
ALTER PROCEDURE [dbo].[sproc_ClientBasedReport_WithHubSpot_PopUp]
	@FromDate			NVARCHAR(50)	= '',
	@ToDate				NVARCHAR(50)	= '',
	@StageName			Nvarchar(100)	= '',
	@FullName			NVARCHAR(500)	= '',
	@Company			NVARCHAR(500)	= '',
	@GEO				NVARCHAR(500)	= '',
	@SalesUser			NVARCHAR(500)	= '',
	@Hr_Number			NVARCHAR(500)	= '',
	@Name				NVARCHAR(500)	= '',
	@CompanyCategory	NVARCHAR(10)	= '',
	@SalesManagerID		bigint			= 0,
	@Status		        varchar(100)    = '',
	@SalesManagerIDs	NVARCHAR(MAX)	= '', --Added this to handle multiselect
	@LeadUserID		    bigint			= 0,   -- To filter Record based on Lead User (Inbound/Outbound)
	@IsHRFocused			bit				= 0

AS
BEGIN 

			IF @IsHRFocused <> 1
				SET @IsHRFocused = NULL;

			DECLARE @WhereClauseSQL nvarchar(max) = ''
			DECLARE @MainSQL nvarchar(max) = ''
			DECLARE @Counter INT 
			DECLARE @Val BIGINT
			SET @Counter=1

			IF isnull(@FullName,'') <> ''
				SET @WhereClauseSQL += ' AND FullName LIKE ''%' + CONVERT(nvarchar,@FullName) + '%'''
			IF isnull(@Company,'') <> ''
				SET @WhereClauseSQL += ' AND Company LIKE ''%' + CONVERT(nvarchar,@Company) + '%'''
			IF isnull(@GEO,'') <> ''
				SET @WhereClauseSQL += ' AND ISNULL(GEO,'''') LIKE ''%' + CONVERT(nvarchar,@GEO) + '%'''
			IF isnull(@SalesUser,'') <> ''
				SET @WhereClauseSQL += ' AND ISNULL(SalesUser,'''') LIKE ''%' + CONVERT(nvarchar,@SalesUser) + '%'''
			IF isnull(@Hr_Number,'') <> ''
				SET @WhereClauseSQL += ' AND Hr_Number LIKE ''%' + CONVERT(nvarchar,@Hr_Number) + '%'''
			IF isnull(@Name,'') <> ''
				SET @WhereClauseSQL += ' AND ISNULL(Name,'''') LIKE ''%' + CONVERT(nvarchar,@Name) + '%'''
			IF isnull(@Status,'') <> ''
					SET @WhereClauseSQL += ' AND Status LIKE ''%' + CONVERT(nvarchar,@Status) + '%'''

		 
		     begin  -- Create all temp tables 
					CREATE TABLE #ClientBasedReport
					(
					   ID				 int Identity (1,1),
					   StageName		 Nvarchar(100),
					   StageValue		 int  default 0,
					   Sequence			 int,
					   ActionID			 nvarchar(20)
					)

					CREATE TABLE #AllUserHierarchy(UserId bigint,ParentId bigint,UserName nvarchar(100),ParentName Nvarchar(100));

					CREATE TABLE #HR_ClientWiseRecords
					(
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
						Talent_ID				bigint,
						HistoryID				bigint,
						Availability			NVARCHAR(200),
						SalesUser				Nvarchar(100),
						HR_Number				Nvarchar(100),
						DeleteType				int
					)	

					CREATE TABLE #NBD_Clients(ID int primary key identity(1,1), ClientID bigint, NBDID bigint);

					CREATE TABLE #HubSpot_ClientPopUpDetail
					(
						DealID		Nvarchar(50),
						SalesPerson		Nvarchar(100),
						StageName		Nvarchar(100),
						DealNumber		Nvarchar(100)
					)
		
					CREATE TABLE #HeadTbl2(HeadID bigint);

					CREATE TABLE #AllHrchyPopUpForHubSpot(UserID bigint, ParentID bigint, MainParentID bigint);

					CREATE TABLE #HubSpot_ClientRecords
						(
							ID						int primary key identity(1,1),
							FullName				Nvarchar(100),
							Company                 Nvarchar(100),
							GEO						Nvarchar(50),
							SalesUser				Nvarchar(100),
							Hr_Number				Nvarchar(100),
							DealID					Nvarchar(100),
							[Name]					Nvarchar(100),
							[Status]				Nvarchar(100),
							CreatedByDatetime		datetime
						)	

					CREATE TABLE #GetLeadUsersCompany(UserID bigint,CompanyId bigint)
			end

		     begin  -- Insert into Temp tables #ClientBasedReport 
				INSERT INTO #ClientBasedReport(StageName,Sequence, ActionID)
				select ClientStageName,Sequence,ActionID from Prg_ClientStage_WithHubSpotStage;
			 end
	
			 begin  -- Insert into temp table #AllUserHierarchy 
				IF @SalesManagerID > 0
					BEGIN
						 Insert Into #AllUserHierarchy
						 exec Sproc_Get_ALL_User_HIERARCHY_For_Parent @SalesManagerID;
					END
			 end

			 BEGIN -- Payal (04-03-2024) (Remove Test Companies Using Configuration)

			     IF OBJECT_ID('tempdb..#TempRemoveCompanies') IS NOT NULL
					DROP TABLE #TempRemoveCompanies;

				Declare @Values as Nvarchar(Max) = ''

				select  @Values = [Value]  
				from   gen_SystemConfiguration WITH(NOLOCK)
				where  [Key]  ='RemoveCompaniesforUTSAdminReports'

				select  cast(val as bigint) as RemoveCompanyID into #TempRemoveCompanies from dbo.f_Split(@Values,',')

		END

			 begin  -- Handle the multi-select values for SalesManager 
				IF ISNULL(@SalesManagerIDs,'') <> '' 
					BEGIN		

						DECLARE @SalesManagerIDTemp TABLE
						(
							RowNumber INT,
							SalesManagerID BIGINT
						)
						INSERT INTO @SalesManagerIDTemp 
						SELECT  ROW_NUMBER() OVER (ORDER BY val ) row_num, val from dbo.f_split(@SalesManagerIDs,',')

					
						WHILE ( @Counter <= (SELECT COUNT(*) FROM @SalesManagerIDTemp))
							BEGIN
			
								SELECT @Val = SalesManagerID FROM @SalesManagerIDTemp WHERE RowNumber = @Counter ORDER BY SalesManagerID
								Insert Into #AllUserHierarchy
								exec Sproc_Get_ALL_User_HIERARCHY_For_Parent @Val
								SET @Counter  = @Counter  + 1
							END		
					END
			 end
	
			 begin	-- NBD Clients Table #NBD_Clients 
					INSERT INTO #NBD_Clients(ClientID, NBDID)
					SELECT distinct H.ContactID as ClientID, ISNULL(His.SalesUserID,0) NBDID  
					from gen_SalesHiringRequest H with(nolock) 
						 INNER JOIN gen_History His with(nolock) on H.ID = His.HiringRequest_ID 
						 INNER JOIN gen_Contact CO with(nolock) on H.ContactID = CO.ID
						 INNER JOIN usr_User U with(nolock) on His.SalesUserID = U.ID
					where (U.UserTypeID = 4 or U.UserTypeID =9) and U.IsNewUser = 1
					      and cast(His.CreatedByDatetime as date) between Cast(@FromDate as date) and cast(@ToDate as date);
			 end
	
	         begin  -- Insert into #HR_ClientWiseRecords 
				IF @LeadUserID <> 0
					BEGIN

						;WITH cteUser AS (    
							SELECT	   u.ID AS UserID, 
										uh.ParentID,
										u.FullName AS Name,
										0 AS [Level]
							FROM       [dbo].[usr_User] u with(nolock)
										LEFT JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID
							WHERE      u.ID = @LeadUserID and U.IsActive = 1		
 
							UNION ALL
 
							SELECT u.ID AS UserID,
									child.ParentID,
									u.FullName AS Name,
									[Level]+1 AS [Level]
							FROM       [dbo].[usr_User] u with(nolock)
							INNER JOIN  [dbo].[usr_UserHierarchy] child with(nolock) ON child.UserID = u.ID
							INNER JOIN cteUser cte  ON cte.UserID = child.ParentID
					)
					Insert into #GetLeadUsersCompany
					select  distinct C.UserID,CLU.CompanyID
					from    cteUser C 
							inner join gen_CompanyLeadType_UserDetails CLU WITH(NOLOCK) ON CLU.LeadType_UserID = C.UserID
				
							
							
				END

					
				IF (@SalesManagerID > 0 OR ISNULL(@SalesManagerIDs,'') <> '')
					BEGIN
						INSERT INTO #HR_ClientWiseRecords(HR_ID, IsAccepted, IsAdhocHR, HRStatus, SalesUserID, ParentID, CreatedByDateTime, HR_DetailID, ContactID, IsActive,Action_ID, Talent_ID, HistoryID, Availability,SalesUser,HR_Number, DeleteType)
						SELECT	H.ID, ISNULL(H.IsAccepted,0) IsAccepted , ISNULL(H.IsAdhocHR, 0) IsAdhocHR,H.JobStatusID as HRStatus --H.Status_ID as HRStatus
								, His.SalesUserID as SalesUserID, AUH.ParentId as Parentid,His.CreatedByDatetime CreatedByDatetime,
								HD.ID, H.ContactID, ISNULL(H.IsActive,0) IsActive , isnull(His.Action_ID,0) Action_ID, ISNULL(His.Talent_ID,0) Talent_ID,His.ID as HistoryID , ISNULL(H.Availability,'') Availability,
								U.FullName,H.HR_Number, ISNULL(H.DeleteType,0) as DeleteType
						FROM	gen_SalesHiringRequest H with(nolock) 
								INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
								INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
								--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
								Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
								INNER JOIN #NBD_Clients C on H.ContactID = C.ClientID and C.NBDID = His.SalesUserID
								inner join #AllUserHierarchy AUH on C.NBDID = AUH.UserID
								INNER JOIN gen_Contact CO with(nolock) on H.ContactID = CO.ID
								INNER JOIN gen_Company COM with(nolock) on CO.CompanyID = COM.ID 
								INNER JOIN usr_User U with(nolock) ON AUH.UserId = U.ID 
						WHERE  (cast(His.CreatedByDatetime as date) between Cast(@FromDate as date) and cast(@ToDate as date))	
								and U.IsNewUser = 1
								and 
								(
									(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Com.ID))
									Or
									(	
										@LeadUserID = 0 and 1 = 1
									)
								)
								AND HD.IsHRFocused = ISNULL(@IsHRFocused, HD.IsHRFocused)
								AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
					END
				ELSE
					BEGIN

					select distinct ID as SM_ID into #SalesManager from usr_User where UserTypeID = 9;
						;WITH cteUser AS (    
								SELECT	   u.ID AS UserID, 
											uh.ParentID,
											u.FullName AS Name,
											(select top 1 U.ID from usr_user U where U.ID = TS.SM_ID) as MainParentID,
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

					INSERT INTO #HR_ClientWiseRecords(HR_ID, IsAccepted, IsAdhocHR, HRStatus, SalesUserID, ParentID, CreatedByDateTime, HR_DetailID, ContactID, IsActive,Action_ID, Talent_ID, HistoryID, Availability,SalesUser,HR_Number, DeleteType)
					SELECT	H.ID, ISNULL(H.IsAccepted,0) IsAccepted , ISNULL(H.IsAdhocHR, 0) IsAdhocHR,H.JobStatusID as HRStatus  --H.Status_ID as HRStatus
							, His.SalesUserID as SalesUserID, UH.ParentID,His.CreatedByDatetime CreatedByDatetime,
							HD.ID, H.ContactID, ISNULL(H.IsActive,0) IsActive , isnull(His.Action_ID,0) Action_ID, ISNULL(His.Talent_ID,0) Talent_ID,His.ID as HistoryID , ISNULL(H.Availability,'') Availability,
							U.FullName,H.HR_Number, ISNULL(H.DeleteType,0) as DeleteType
					FROM	gen_SalesHiringRequest H with(nolock) 
							INNER JOIN gen_SalesHiringRequest_Details HD with(nolock) on H.ID = HD.HiringRequest_ID
							INNER JOIN gen_History His with(Nolock) ON H.ID = His.HiringRequest_ID	
							--INNER JOIN prg_HiringRequestStatus HRS with(nolock) on HRS.ID = H.Status_ID
							Inner join prg_JobStatus_ClientPortal JSC WITH(NOLOCK) ON JSC.ID = H.JobStatusID
							--inner join cteUser CT ON CT.UserID = H.SalesUserID
							INNER JOIN #NBD_Clients C on H.ContactID = C.ClientID and C.NBDID = HIs.SalesUserID
							inner join cteUser CT ON CT.UserID = C.NBDID
							INNER JOIN usr_User U with(nolock) ON C.NBDID = U.ID 
							INNER JOIN usr_UserHierarchy UH with(nolock) ON C.NBDID = UH.UserID
							INNER JOIN gen_Contact CO with(nolock) on H.ContactID = CO.ID
							INNER JOIN gen_Company COM with(nolock) on CO.CompanyID = COM.ID 
					WHERE  (cast(His.CreatedByDatetime as date) between Cast(@FromDate as date) and cast(@ToDate as date))	
							and U.IsNewUser = 1
							and 
								(
									(@LeadUserID <> 0 and exists(select CompanyID from #GetLeadUsersCompany G Where G.CompanyID = Com.ID))
									Or
									(	
										@LeadUserID = 0 and 1 = 1
									)
								)
							AND HD.IsHRFocused = ISNULL(@IsHRFocused, HD.IsHRFocused)
							AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
				END

					
			 end
	
			 Begin  -- Deal Related Code 
				BEGIN  --Get All Head Level User of Demand Dept. #HeadTbl2				
						INSERT INTO #HeadTbl2(HeadID)
						SELECT distinct U.ID from usr_User U with(nolock) where U.IsActive = 1 and U.DeptID = 1 and U.LevelID = 1 and U.ID = CASE WHEN @SalesManagerID > 0 THEN @SalesManagerID ELSE U.ID END;
				END	

				
				begin --Insert All User Hierarchies in Temp Table #HubSpot_ClientRecords
					If @LeadUserID = 0
						BEGIN
								;WITH cteUser_AllHrchy AS (    
									SELECT		u.ID AS UserID, 
												uh.ParentID,
												u.FullName AS Name,
												H1.HeadID as MainParentID,
												0 AS [Level]
									FROM		[dbo].[usr_User] u with(nolock)
												LEFT JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID
												LEFT JOIN #HeadTbl2 H1 on uh.ParentID = H1.HeadID
									WHERE		uh.ParentID = H1.HeadID and U.IsActive = 1	   
 
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

								INSERT INTO #HubSpot_ClientRecords(FullName,Company,GEO,SalesUser,Hr_Number,DealID,[Name],[Status],CreatedByDatetime)
								SELECT distinct	Isnull(CO.FullName,'') + + CHAR(10) + '(' + ISNULL(CO.EmailID,'') + ')' as FullName,
													C.Company as Company,G.GEO as GEO,U.FullName as SalesUser,D.Deal_Number as Hr_Number,DealID,DS.Stage as [Name],'' [Status],
													D.CreatedByDateTime
								FROM	gen_Deals D WITH(NOLOCK) 
										inner join prg_DealStages DS WITH(NOLOCK) ON DS.HubSpot_Stage = D.DealStage
										inner join usr_User U WITH(NOLOCK) ON U.ID = D.AM_ID
										INNER JOIN cteUser_AllHrchy TS ON  D.AM_ID = TS.UserID
										INNER join gen_HubSpotContact HCO with(Nolock) ON HCO.ContactID = D.clientID 										INNER join gen_HubSpotCompany HC WITH(NOLOCK) ON HC.Companyid = HCO.CompanyID										INNER join gen_Company C WITH(NOLOCK) ON C.Hubspot_company = HC.Companyid										INNER join gen_Contact CO WITH(NOLOCK) ON CO.Companyid = C.ID and D.clientID = CO.HubSpot_ContactID
										INNER Join prg_GEO G WITH(NOLOCK) ON G.ID = C.GEO_ID

								WHERE ((cast(D.CreatedByDatetime as date) between Cast(@FromDate as date) and cast(@ToDate as date)))	
										AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)

						END
					ELSE 
						BEGIN
									;WITH cteUser_AfterPS AS (    
										SELECT	    u.ID AS UserID, 
													uh.ParentID,
													u.FullName AS Name,
													uh.ParentID as MainParentID,
													0 AS [Level]
										FROM       [dbo].[usr_User] u with(nolock)
													LEFT JOIN  [dbo].[usr_UserHierarchy] uh with(nolock) ON uh.UserID = u.ID	
										WHERE	   uh.ParentID = @LeadUserID  and  U.IsActive = 1
 
									UNION ALL
 
										SELECT  u.ID AS UserID,
												child.ParentID,
												u.FullName AS Name,
												cte.MainParentID as MainParentID,
												[Level]+1 AS [Level]
										FROM    [dbo].[usr_User] u with(nolock)
												INNER JOIN  [dbo].[usr_UserHierarchy] child with(nolock) ON child.UserID = u.ID
												INNER JOIN cteUser_AfterPS cte  ON cte.UserID = child.ParentID
										 where   U.IsActive = 1
								)					
								INSERT INTO #HubSpot_ClientRecords(FullName,Company,GEO,SalesUser,Hr_Number,DealID,[Name],[Status],CreatedByDatetime)
								SELECT distinct	Isnull(CO.FullName,'') + + CHAR(10) + '(' + ISNULL(CO.EmailID,'') + ')' as FullName,
													C.Company as Company,G.GEO as GEO,U.FullName as SalesUser,D.Deal_Number as Hr_Number,DealID,DS.Stage as [Name],'' [Status],
													D.CreatedByDateTime
								FROM	gen_Deals D WITH(NOLOCK) 
										inner join prg_DealStages DS WITH(NOLOCK) ON DS.HubSpot_Stage = D.DealStage
										inner join usr_User U WITH(NOLOCK) ON (U.EmailID = D.BDR_MDR_Email or U.EmployeeID = D.BDR_MDR_Email)
										INNER JOIN cteUser_AfterPS TS ON  D.AM_ID = TS.UserID
										INNER join gen_HubSpotContact HCO with(Nolock) ON HCO.ContactID = D.clientID 										INNER join gen_HubSpotCompany HC WITH(NOLOCK) ON HC.Companyid = HCO.CompanyID										INNER join gen_Company C WITH(NOLOCK) ON C.Hubspot_company = HC.Companyid										INNER join gen_Contact CO WITH(NOLOCK) ON CO.Companyid = C.ID and D.clientID = CO.HubSpot_ContactID
										INNER Join prg_GEO G WITH(NOLOCK) ON G.ID = C.GEO_ID
										INNER JOIN #GetLeadUsersCompany CLU WITH(NOLOCK) ON CLU.CompanyID = CO.ID
								WHERE ((cast(D.CreatedByDatetime as date) between Cast(@FromDate as date) and cast(@ToDate as date)))
								  AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
						END	
				end				
			 ENd

				IF @StageName = 'DC Stage'
					BEGIN
				
							select distinct FullName,Company,GEO,SalesUser,Hr_Number,[Name],[Status]
							from	#HubSpot_ClientRecords HCR								
							WHERE  ((cast(CreatedByDatetime as date) between Cast(@FromDate as date) and cast(@ToDate as date)))
								   and [Name] in ('Discovery call Done','Discovery call Scheduled','Discover call Rescheduled','Discovery call Goals')
								   and Not Exists (Select	1 
													from	#HubSpot_ClientRecords H1 																		
													Where	H1.CreatedByDatetime > HCR.CreatedByDatetime and
															cast(H1.CreatedByDatetime as date) between Cast(@FromDate as date) and cast(@ToDate as date)
															and H1.DealID = HCR.DealID 
													) 
					END
			
				IF @StageName = 'SAL Stage'
					BEGIN
							select distinct FullName,Company,GEO,SalesUser,Hr_Number,[Name],[Status]
							from	#HubSpot_ClientRecords HCR								
							WHERE  ((cast(CreatedByDatetime as date) between Cast(@FromDate as date) and cast(@ToDate as date)))
								   and [Name] in ('Tet SAL To IV','SAL Achieved','SAL Goals')
								   and Not Exists (Select	1 
																	from	#HubSpot_ClientRecords H1 																		
																	Where	H1.CreatedByDatetime > HCR.CreatedByDatetime and
																			cast(H1.CreatedByDatetime as date) between Cast(@FromDate as date) and cast(@ToDate as date)
																			and H1.DealID = HCR.DealID 
																	) 
					END
		 
				IF @StageName = 'HR Accepted'
				  BEGIN
							SET @MainSQL= ';WITH CTE_Records AS (SELECT distinct ROW_NUMBER() OVER(PARTITION BY H.ContactID ORDER BY H.CreatedByDateTime desc) as ROWNUM,CASE WHEN ISNULL(C.EmailID,'''') = '''' THEN C.FullName ELSE (C.FullName + CHAR(10) + ''('' + ISNULL(C.EmailID,'''') + '')'') END as FullName,CO.Company,ISNULL(G.GEO,'''') GEO,H.SalesUser,H.Hr_Number,'''' as Name,'''' as Status
							FROM	#HR_ClientWiseRecords H with(nolock)						
									INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
									INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID
									left JOIN prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
							WHERE  (cast(H.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast('''+ @ToDate +''' as date))
									and ISNULL(CO.Category,'''') IN (CASE WHEN ''' + @CompanyCategory + ''' != '''' THEN  ''' + @CompanyCategory + ''' ELSE ISNULL(CO.Category,'''') END)					
									and ISNULL(H.IsAccepted,0) = 1 And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 17									
									AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and Not Exists (Select	1 
													from	#HR_ClientWiseRecords H1 with(nolock)
															inner join #ClientBasedReport CB ON Cast(H1.Action_ID as nvarchar(10)) in (select val from dbo.f_split(CB.ActionID,'','')) 
													Where	H1.CreatedByDatetime > H.CreatedByDatetime and
															cast(H1.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast('''+ @ToDate +'''  as date)
															and H1.HR_ID = H.HR_ID 
													)
													)
										,Cte_TotalCount AS(
												Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
										)
										select FullName,Company,GEO,SalesUser,Hr_Number,Name,Status from CTE_Records where ROWNUM = 1 and 1=1 ' + @WhereClauseSQL + ' ';
				  END
	
				IF @StageName = 'Profile Shared'
				 BEGIN
							SET @MainSQL= ';WITH CTE_Records AS (SELECT distinct ROW_NUMBER() OVER(PARTITION BY H.ContactID ORDER BY H.CreatedByDateTime desc) as ROWNUM,CASE WHEN ISNULL(C.EmailID,'''') = '''' THEN C.FullName ELSE (C.FullName + CHAR(10) + ''('' + ISNULL(C.EmailID,'''') + '')'') END as FullName,CO.Company,ISNULL(G.GEO,'''') GEO,H.SalesUser,H.Hr_Number, CASE WHEN ISNULL(T.EmailId,'''') = '''' THEN ISNULL(T.Name,'''') ELSE (ISNULL(T.Name,'''') + CHAR(10) + ''('' + ISNULL(T.EmailId,'''') + '')'') END as Name,ISNULL(TAS.TalentStatus,'''') as Status
							FROM	#HR_ClientWiseRecords H with(nolock)						
									INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
									INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID
									Inner join gen_Talent T WITH(NOLOCK) ON T.ID = H.Talent_ID
									inner join gen_ContactTalentPriority CTP with(nolock) on CTP.TalentID = T.ID and H.HR_ID = CTP.HiringRequestID
									inner join prg_TalentStatus_AfterClientSelection TAS WITH(NOLOCK) ON TAS.ID = CTP.TalentStatusID_BasedOnHR
									left JOIN prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
							WHERE  (cast(H.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date))
									and ISNULL(CO.Category,'''') IN (CASE WHEN ''' + @CompanyCategory + ''' != '''' THEN  ''' + @CompanyCategory + ''' ELSE ISNULL(CO.Category,'''') END)
									and ISNULL(H.IsAccepted,0) = 1 And ISNULL(H.IsActive,0) = 1 and H.Action_ID in (6,18) AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and Not Exists (Select	1 
													from	#HR_ClientWiseRecords H1 with(nolock)
															inner join #ClientBasedReport CB ON Cast(H1.Action_ID as nvarchar(10)) in (select val from dbo.f_split(CB.ActionID,'','')) 
													Where	H1.CreatedByDatetime > H.CreatedByDatetime and
															cast(H1.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date)
															and H1.HR_ID = H.HR_ID 
													)		
							UNION
							SELECT distinct ROW_NUMBER() OVER(PARTITION BY H.ContactID ORDER BY H.CreatedByDateTime desc) as ROWNUM, CASE WHEN ISNULL(C.EmailID,'''') = '''' THEN C.FullName ELSE (C.FullName + CHAR(10) + ''('' + ISNULL(C.EmailID,'''') + '')'') END as FullName,CO.Company,ISNULL(G.GEO,'''') GEO,H.SalesUser,H.Hr_Number, CASE WHEN ISNULL(T.ManagedTalentEmailID,'''') = '''' THEN (ISNULL(T.ManagedTalentFirstName,'''') + '' '' + ISNULL(T.ManagedTalentLastName,'''')) ELSE (ISNULL(T.ManagedTalentFirstName,'''') + '' '' + ISNULL(T.ManagedTalentLastName,'''') + CHAR(10) + ''('' + ISNULL(T.ManagedTalentEmailID,'''') + '')'')  END AS Name,'''' as Status
							FROM	#HR_ClientWiseRecords H with(nolock)						
									INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
									INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID
									Inner join gen_ManagedTalent T WITH(NOLOCK) ON T.ID = H.Talent_ID
									left JOIN prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
							WHERE  (cast(H.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date))
									and ISNULL(CO.Category,'''') IN (CASE WHEN ''' + @CompanyCategory + ''' != '''' THEN  ''' + @CompanyCategory + ''' ELSE ISNULL(CO.Category,'''') END)
									and ISNULL(H.IsAccepted,0) = 1 And ISNULL(H.IsActive,0) = 1 and H.Action_ID in (54)	AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and Not Exists (Select	1 
													from	#HR_ClientWiseRecords H1 with(nolock)
															inner join #ClientBasedReport CB ON Cast(H1.Action_ID as nvarchar(10)) in (select val from dbo.f_split(CB.ActionID,'','')) 
													Where	H1.CreatedByDatetime > H.CreatedByDatetime and
															cast(H1.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date)
															and H1.HR_ID = H.HR_ID)
										)
										,Cte_TotalCount AS(
												Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
										)
										select FullName,Company,GEO,SalesUser,Hr_Number,Name,Status from CTE_Records where ROWNUM = 1 and 1=1 ' + @WhereClauseSQL + ' ';

				  END

				IF @StageName = 'Interview'
				  BEGIN
							SET @MainSQL= ';WITH CTE_Records AS (SELECT distinct ROW_NUMBER() OVER(PARTITION BY H.ContactID ORDER BY H.CreatedByDateTime desc) as ROWNUM,CASE WHEN ISNULL(C.EmailID,'''') = '''' THEN C.FullName ELSE (C.FullName + CHAR(10) + ''('' + ISNULL(C.EmailID,'''') + '')'') END as FullName,CO.Company,ISNULL(G.GEO,'''') GEO,H.SalesUser,H.Hr_Number, CASE WHEN ISNULL(T.EmailId,'''') = '''' THEN ISNULL(T.Name,'''') ELSE (ISNULL(T.Name,'''') + CHAR(10) + ''('' + ISNULL(T.EmailId,'''') + '')'') END as Name,ISNULL(TAS.TalentStatus,'''') as Status
							FROM	#HR_ClientWiseRecords H with(nolock)						
									INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
									INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID
									Inner join gen_Talent T WITH(NOLOCK) ON T.ID = H.Talent_ID
									inner join gen_ContactTalentPriority CTP with(nolock) on CTP.TalentID = T.ID and H.HR_ID = CTP.HiringRequestID
									inner join prg_TalentStatus_AfterClientSelection TAS WITH(NOLOCK) ON TAS.ID = CTP.TalentStatusID_BasedOnHR
									left JOIN prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
							WHERE  (cast(H.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date))
									and ISNULL(CO.Category,'''') IN (CASE WHEN ''' + @CompanyCategory + ''' != '''' THEN  ''' + @CompanyCategory + ''' ELSE ISNULL(CO.Category,'''') END)
									and ISNULL(H.IsAccepted,0) = 1 And ISNULL(H.IsActive,0) = 1 and H.Action_ID in (7,9,10,11,46,47)
									AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and Not Exists (Select	1 
													from	#HR_ClientWiseRecords H1 with(nolock)
															inner join #ClientBasedReport CB ON Cast(H1.Action_ID as nvarchar(10)) in (select val from dbo.f_split(CB.ActionID,'','')) 
													Where	H1.CreatedByDatetime > H.CreatedByDatetime and
															cast(H1.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date)
															and H1.HR_ID = H.HR_ID 
													)
										)
										,Cte_TotalCount AS(
												Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
										)
										select FullName,Company,GEO,SalesUser,Hr_Number,Name,Status from CTE_Records where ROWNUM = 1 and 1=1 ' + @WhereClauseSQL + ' ';

				  END

				IF @StageName = 'Agreement'
				  BEGIN
							SET @MainSQL= ';WITH CTE_Records AS (SELECT distinct ROW_NUMBER() OVER(PARTITION BY H.ContactID ORDER BY H.CreatedByDateTime desc) as ROWNUM,CASE WHEN ISNULL(C.EmailID,'''') = '''' THEN C.FullName ELSE (C.FullName + CHAR(10) + ''('' + ISNULL(C.EmailID,'''') + '')'') END as FullName,CO.Company,ISNULL(G.GEO,'''') GEO,H.SalesUser,H.Hr_Number, CASE WHEN ISNULL(T.EmailId,'''') = '''' THEN ISNULL(T.Name,'''') ELSE (ISNULL(T.Name,'''') + CHAR(10) + ''('' + ISNULL(T.EmailId,'''') + '')'') END as Name,ISNULL(TAS.TalentStatus,'''') as Status
							FROM	#HR_ClientWiseRecords H with(nolock) INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
									INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID Inner join gen_Talent T WITH(NOLOCK) ON T.ID = H.Talent_ID
									inner join gen_ContactTalentPriority CTP with(nolock) on CTP.TalentID = T.ID and H.HR_ID = CTP.HiringRequestID
									inner join prg_TalentStatus_AfterClientSelection TAS WITH(NOLOCK) ON TAS.ID = CTP.TalentStatusID_BasedOnHR
									left JOIN prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
							WHERE  (cast(H.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date))
									and ISNULL(CO.Category,'''') IN (CASE WHEN ''' + @CompanyCategory + ''' != '''' THEN  ''' + @CompanyCategory + ''' ELSE ISNULL(CO.Category,'''') END)
									and ISNULL(H.IsAccepted,0) = 1 And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 12 AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and Not Exists (Select	1 
													from	#HR_ClientWiseRecords H1 with(nolock)
															inner join #ClientBasedReport CB ON Cast(H1.Action_ID as nvarchar(10)) in (select val from dbo.f_split(CB.ActionID,'','')) 
													Where	H1.CreatedByDatetime > H.CreatedByDatetime and
															cast(H1.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date)
															and H1.HR_ID = H.HR_ID 
													)
							UNION				
							SELECT distinct ROW_NUMBER() OVER(PARTITION BY H.ContactID ORDER BY H.CreatedByDateTime desc) as ROWNUM, CASE WHEN ISNULL(C.EmailID,'''') = '''' THEN C.FullName ELSE (C.FullName + CHAR(10) + ''('' + ISNULL(C.EmailID,'''') + '')'') END as FullName,CO.Company,ISNULL(G.GEO,'''') GEO,H.SalesUser,H.Hr_Number, CASE WHEN ISNULL(T.ManagedTalentEmailID,'''') = '''' THEN (ISNULL(T.ManagedTalentFirstName,'''') + '' '' + ISNULL(T.ManagedTalentLastName,'''')) ELSE (ISNULL(T.ManagedTalentFirstName,'''') + '' '' + ISNULL(T.ManagedTalentLastName,'''') + CHAR(10) + ''('' + ISNULL(T.ManagedTalentEmailID,'''') + '')'')  END AS Name,'''' as Status
							FROM	#HR_ClientWiseRecords H with(nolock) INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
									INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID Inner join gen_ManagedTalent T WITH(NOLOCK) ON T.ID = H.Talent_ID
									left JOIN prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
							WHERE  (cast(H.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date))
									and ISNULL(CO.Category,'''') IN (CASE WHEN ''' + @CompanyCategory + ''' != '''' THEN  ''' + @CompanyCategory + ''' ELSE ISNULL(CO.Category,'''') END)
									and ISNULL(H.IsAccepted,0) = 1 And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 60 and T.ProposalConfirmDate IS NOT NULL
									AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and Not Exists (Select	1 
													from	#HR_ClientWiseRecords H1 with(nolock)
															inner join #ClientBasedReport CB ON Cast(H1.Action_ID as nvarchar(10)) in (select val from dbo.f_split(CB.ActionID,'','')) 
													Where	H1.CreatedByDatetime > H.CreatedByDatetime and
															cast(H1.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date)
															and H1.HR_ID = H.HR_ID 
													)
									)
									,Cte_TotalCount AS(
											Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
									)
									select FullName,Company,GEO,SalesUser,Hr_Number,Name,Status from CTE_Records where ROWNUM = 1 and 1=1 ' + @WhereClauseSQL + ' ';
				  END

				IF @StageName = 'Closed Clients'
				  BEGIN
							SET @MainSQL= ';WITH CTE_Records AS (SELECT distinct ROW_NUMBER() OVER(PARTITION BY H.ContactID ORDER BY H.CreatedByDateTime desc) as ROWNUM,CASE WHEN ISNULL(C.EmailID,'''') = '''' THEN C.FullName ELSE (C.FullName + CHAR(10) + ''('' + ISNULL(C.EmailID,'''') + '')'') END as FullName,CO.Company,ISNULL(G.GEO,'''') GEO,H.SalesUser,H.Hr_Number, CASE WHEN ISNULL(T.EmailId,'''') = '''' THEN ISNULL(T.Name,'''') ELSE (ISNULL(T.Name,'''') + CHAR(10) + ''('' + ISNULL(T.EmailId,'''') + '')'') END as Name,ISNULL(TAS.TalentStatus,'''') as Status
							FROM	#HR_ClientWiseRecords H with(nolock) INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
									INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID Inner join gen_Talent T WITH(NOLOCK) ON T.ID = H.Talent_ID
									inner join gen_ContactTalentPriority CTP with(nolock) on CTP.TalentID = T.ID and H.HR_ID = CTP.HiringRequestID
									inner join prg_TalentStatus_AfterClientSelection TAS WITH(NOLOCK) ON TAS.ID = CTP.TalentStatusID_BasedOnHR
									left JOIN prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
							WHERE  (cast(H.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date))
									and ISNULL(CO.Category,'''') IN (CASE WHEN ''' + @CompanyCategory + ''' != '''' THEN  ''' + @CompanyCategory + ''' ELSE ISNULL(CO.Category,'''') END)
									and ISNULL(H.IsAccepted,0) = 1 And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 35 AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and Not Exists (Select	1 
													from	#HR_ClientWiseRecords H1 with(nolock)
															inner join #ClientBasedReport CB ON Cast(H1.Action_ID as nvarchar(10)) in (select val from dbo.f_split(CB.ActionID,'',''))  
													Where	H1.CreatedByDatetime > H.CreatedByDatetime and
															cast(H1.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date)
															and H1.HR_ID = H.HR_ID 
													)
							UNION
							SELECT distinct ROW_NUMBER() OVER(PARTITION BY H.ContactID ORDER BY H.CreatedByDateTime desc) as ROWNUM, CASE WHEN ISNULL(C.EmailID,'''') = '''' THEN C.FullName ELSE (C.FullName + CHAR(10) + ''('' + ISNULL(C.EmailID,'''') + '')'') END as FullName,CO.Company,ISNULL(G.GEO,'''') GEO,H.SalesUser,H.Hr_Number, CASE WHEN ISNULL(T.ManagedTalentEmailID,'''') = '''' THEN (ISNULL(T.ManagedTalentFirstName,'''') + '' '' + ISNULL(T.ManagedTalentLastName,'''')) ELSE (ISNULL(T.ManagedTalentFirstName,'''') + '' '' + ISNULL(T.ManagedTalentLastName,'''') + CHAR(10) + ''('' + ISNULL(T.ManagedTalentEmailID,'''') + '')'')  END AS Name,'''' as Status
							FROM	#HR_ClientWiseRecords H with(nolock) INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
									INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID Inner join gen_ManagedTalent T WITH(NOLOCK) ON T.ID = H.Talent_ID
									left JOIN prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
							WHERE  (cast(H.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date))
									and ISNULL(CO.Category,'''') IN (CASE WHEN ''' + @CompanyCategory + ''' != '''' THEN  ''' + @CompanyCategory + ''' ELSE ISNULL(CO.Category,'''') END)
									and ISNULL(H.IsAccepted,0) = 1 And ISNULL(H.IsActive,0) = 1 and H.Action_ID = 35 and T.ProposalConfirmDate IS NOT NULL
									AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and Not Exists (Select	1 
													from	#HR_ClientWiseRecords H1 with(nolock)
															inner join #ClientBasedReport CB ON Cast(H1.Action_ID as nvarchar(10)) in (select val from dbo.f_split(CB.ActionID,'','')) 
													Where	H1.CreatedByDatetime > H.CreatedByDatetime and
															cast(H1.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date)
															and H1.HR_ID = H.HR_ID 
													)
									)
									,Cte_TotalCount AS(
											Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
									)
									select FullName,Company,GEO,SalesUser,Hr_Number,Name,Status from CTE_Records where ROWNUM = 1 and 1=1 ' + @WhereClauseSQL + ' ';
				  END

				IF @StageName = 'Lost Prospects'
				  BEGIN

							IF OBJECT_ID('tempdb..#Client_With_Lost_HR') is not null
								DROP TABLE #Client_With_Lost_HR;
							IF OBJECT_ID('tempdb..#Client_Without_Lost_HR') is not null
								DROP TABLE #Client_Without_Lost_HR;

							CREATE TABLE #Client_With_Lost_HR (ID int primary key identity(1,1), ClientID bigint, LostCount int);

							INSERT INTO #Client_With_Lost_HR(ClientID, LostCount)
							SELECT		H.ContactID, COUNT(distinct H.ContactID)
							FROM		gen_SalesHiringRequest H with(nolock) inner join gen_History His with(nolock) on H.ID = His.HiringRequest_ID
							WHERE		CAST(His.CreatedByDatetime as date) BETWEEN @FromDate and @ToDate and H.DeleteType = 2 and His.Action_ID = 3
							GROUP BY	H.ContactID;
	


							CREATE TABLE #Client_Without_Lost_HR (ID int primary key identity(1,1), ClientID bigint, HRCount int);

							INSERT INTO #Client_Without_Lost_HR(ClientID, HRCount)
							SELECT		H.ContactID, COUNT(distinct H.ContactID)
							FROM		gen_SalesHiringRequest H with(nolock) inner join gen_History His with(nolock) on H.ID = His.HiringRequest_ID
							WHERE		CAST(His.CreatedByDatetime as date) BETWEEN @FromDate and @ToDate and ISNULL(H.DeleteType,0) IN (0,1) and His.Action_ID != 3
							GROUP BY	H.ContactID;


							SET @MainSQL= ';WITH CTE_Records AS (SELECT distinct ROW_NUMBER() OVER(PARTITION BY H.ContactID ORDER BY H.CreatedByDateTime desc) as ROWNUM,CASE WHEN ISNULL(C.EmailID,'''') = '''' THEN C.FullName ELSE (C.FullName + CHAR(10) + ''('' + ISNULL(C.EmailID,'''') + '')'') END as FullName,CO.Company,ISNULL(G.GEO,'''') GEO,H.SalesUser,H.Hr_Number, CASE WHEN ISNULL(T.EmailId,'''') = '''' THEN ISNULL(T.Name,'''') ELSE (ISNULL(T.Name,'''') + CHAR(10) + ''('' + ISNULL(T.EmailId,'''') + '')'') END as Name,ISNULL(TAS.TalentStatus,'''') as Status
							FROM	#HR_ClientWiseRecords H with(nolock)						
									INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
									INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID
									INNER JOIN #Client_With_Lost_HR LH on H.ContactID = LH.ClientID
									left JOIN #Client_Without_Lost_HR WLH on H.ContactID = WLH.ClientID
									left join gen_Talent T WITH(NOLOCK) ON T.ID = H.Talent_ID
									left join gen_ContactTalentPriority CTP with(nolock) on CTP.TalentID = T.ID and H.HR_ID = CTP.HiringRequestID
									left join prg_TalentStatus_AfterClientSelection TAS WITH(NOLOCK) ON TAS.ID = CTP.TalentStatusID_BasedOnHR
									left JOIN prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
							WHERE  (cast(H.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date))
									and ISNULL(CO.Category,'''') IN (CASE WHEN ''' + @CompanyCategory + ''' != '''' THEN  ''' + @CompanyCategory + ''' ELSE ISNULL(CO.Category,'''') END)
									and H.HRStatus = 4 and H.Action_ID = 3 and ISNULL(H.DeleteType,0) = 2
									AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and ISNULL(LH.LostCount,0) > 0 and ISNULL(WLH.HRCount,0) = 0

							UNION

							SELECT	ROW_NUMBER() OVER(PARTITION BY H.ContactID ORDER BY H.CreatedByDateTime desc) as ROWNUM,(C.FullName + CHAR(10) + ''('' + ISNULL(C.EmailID,'''') + '')'') as FullName,CO.Company,ISNULL(G.GEO,'''') GEO,H.SalesUser,H.Hr_Number,(ISNULL(T.ManagedTalentFirstName,'''') + '' '' + ISNULL(T.ManagedTalentLastName,'''') + CHAR(10) + ''('' + ISNULL(T.ManagedTalentEmailID,'''') + '')'')  Name,'''' as Status
							FROM	#HR_ClientWiseRecords H with(nolock)						
									INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
									INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID
									INNER JOIN #Client_With_Lost_HR LH on H.ContactID = LH.ClientID
									left JOIN #Client_Without_Lost_HR WLH on H.ContactID = WLH.ClientID
									left join gen_ManagedTalent T WITH(NOLOCK) ON T.ID = H.Talent_ID
									left JOIN prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
							WHERE  (cast(H.CreatedByDatetime as date) between Cast(''' + @FromDate +''' as date) and cast(''' + @ToDate +''' as date))
									and ISNULL(CO.Category,'''') IN (CASE WHEN ''' + @CompanyCategory + ''' != '''' THEN  ''' + @CompanyCategory + ''' ELSE ISNULL(CO.Category,'''') END)
									and H.HRStatus = 4 and H.Action_ID = 3 
									and T.ProposalConfirmDate IS NOT NULL and ISNULL(H.DeleteType,0) = 2
									AND not exists( select 1 from  #TempRemoveCompanies where Co.ID = RemoveCompanyID)
									and ISNULL(LH.LostCount,0) > 0 and ISNULL(WLH.HRCount,0) = 0
									)
									,Cte_TotalCount AS(
											Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
									)
									select FullName,Company,GEO,SalesUser,Hr_Number,Name,Status from CTE_Records where ROWNUM = 1 and 1=1 ' + @WhereClauseSQL + ' ';
				  END

				PRINT(@MainSQL)

				EXECUTE sp_executesql @MainSQL	
END
