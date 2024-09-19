ALTER PROCEDURE [dbo].[Sproc_Get_JobPostingDetails_For_HomePage_ClientPortal]
    @ClientId			bigint = 0,
	@AnotherCompanyTypeId		int = 0
AS
BEGIN
		Declare @ComapnyId as Bigint = 0
		SELECT @ComapnyId = CompanyID from gen_Contact with(NOlock) where ID = @ClientId

		IF OBJECT_ID('tempdb..#HRTypeID') IS Not Null
			DROP TABLE #HRTypeID

		CREATE TABLE #HRTypeID
		(
			HRID		bigint,
			HRTypeID	int
		)

		IF Isnull(@AnotherCompanyTypeId,0) = 1 
			BEGIN
					INSERT INTO #HRTypeID
					SELECT  ID,ISNULL(HRTypeID,0)
					FROM    gen_SalesHiringRequest WITH(NOLOCK)
					WHERE   ContactID in (Select ID from gen_Contact WITH(NOLOCK) WHERE CompanyID = @ComapnyId) AND Isnull(HRTypeId,0) = 1
			END
		ELSE IF Isnull(@AnotherCompanyTypeId,0) = 2
			BEGIN
					INSERT INTO #HRTypeID
					SELECT  ID,ISNULL(HRTypeID,0)
					FROM    gen_SalesHiringRequest WITH(NOLOCK)
					WHERE   ContactID in (Select ID from gen_Contact WITH(NOLOCK) WHERE CompanyID = @ComapnyId) AND Isnull(HRTypeId,0) in (3,4,6)
			END
		ELSE
			BEGIN
					INSERT INTO #HRTypeID
					SELECT  ID,ISNULL(HRTypeID,0)
					FROM    gen_SalesHiringRequest WITH(NOLOCK)
					WHERE   ContactID in (Select ID from gen_Contact WITH(NOLOCK) WHERE CompanyID = @ComapnyId)
			END


		IF OBJECT_ID('tempdb..#HomePageJobPostingDetails') is not null
			DROP TABLE #HomePageJobPostingDetails

		CREATE TABLE #HomePageJobPostingDetails
		(
			ClientID				bigint,
			HRID					bigint,
			JobTitle				Nvarchar(Max) default '',
			ActiveJobs				int default 0,
			ClosedJobs				int default 0,
			Shortlistedcount		int default 0,
			TotalShortlistedCount 	int default 0,
			CreatedByDateTime		datetime,
			CompanyID				bigint,
			RoleId					int default 0,
			IsNew				    bit default 0,
			TotalCount				int default 0,
			MyListCount				int default 0,
			HRMyListCount			int default 0
		)
		
		Insert into #HomePageJobPostingDetails(ClientID,HRID,JobTitle,CreatedByDateTime,CompanyID,RoleId)
		select  ClientID,HRID,JobTitle,CreatedByDatetime,CompanyID,RoleId
		from (
				select	C.ID as ClientID,H.ID as HRID,H.RequestForTalent + ' - ' + H.HR_Number as JobTitle,	
						H.CreatedByDatetime,C.CompanyID,HRD.Role_ID as RoleId
				from	gen_SalesHiringRequest H WITH(NOLOCK)
						inner join gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
						inner join gen_SalesHiringRequest_Details HRD WITH(NOLOCK) ON HRD.HiringRequest_ID = H.Id
						inner join #HRTypeID HT WITH(NOLOCK) ON HT.HRID = H.ID
				where	--C.ID = @ClientId 
						C.CompanyID = @ComapnyId
						and  Isnull(H.JobStatusID,0) <> 2 --and Isnull(C.IsFromSignUp,0) = 1
						and H.IsActive = 1
				Group By C.ID,H.ID,H.RequestForTalent,H.HR_Number,H.CreatedByDatetime,C.CompanyID,HRD.Role_ID
			 )Q
		ORDER BY Q.CreatedByDatetime desc
		

		Update H
		SET    H.Shortlistedcount = Isnull(ShoertlistedTalents,0)
		From   #HomePageJobPostingDetails H
			   left join 
			   (
					select  Count(CTP.ID) ShoertlistedTalents,HR.Id as HiringRequest_ID
					from	gen_SalesHiringRequest HR with(nolock) 
							inner join gen_SalesHiringRequest_Details HRD with(nolock) on HR.ID = HRD.HiringRequest_ID
							inner join gen_ContactTalentPriority  CTP with(nolock) ON HR.ID = CTP.HiringRequestID AND CTP.ContactID = HR.ContactID 
							AND CTP.HiringRequest_Detail_ID = HRD.ID				
							
					--where	CTP.TalentStatusID_BasedOnHR in(2,3)
							--AND HRD.RoleStatus_ID  in(2,4,5)
					GRoup By HR.ID
			   )Q on Q.HiringRequest_ID = H.HRID


		Update H		
		SET    IsNew = case when Isnull(IsViewed,0)= 1 then 0 else 1 end
		FROM   #HomePageJobPostingDetails H
			   inner join 
			   (
					select  Isnull(IsViewed,0) IsViewed,HR.Id as HiringRequest_ID
					from	gen_SalesHiringRequest HR with(nolock) 
							inner join gen_ContactTalentPriority  CTP with(nolock) ON HR.ID = CTP.HiringRequestID AND CTP.ContactID = HR.ContactID					
			   )Q on Q.HiringRequest_ID = H.HRID


		--Update H		
		--SET    TotalShortlistedCount = Isnull(TotalShoertlistedTalents,0)
		--FROM   #HomePageJobPostingDetails H
		--	   inner join 
		--	   (
		--			select  Sum(H1.Shortlistedcount) TotalShoertlistedTalents
		--			from   #HomePageJobPostingDetails H1
		--	   )Q on 1 =1 

		
		Update H		
		SET    TotalCount = Isnull(TotalShoertlistedTalents,0)
		FROM   #HomePageJobPostingDetails H
			   inner join 
			   (
					select  Sum(H1.Shortlistedcount) TotalShoertlistedTalents
					from   #HomePageJobPostingDetails H1
			   )Q on 1 =1 

		Update H		
		SET    MyListCount = Isnull(BookMarkedCount,0)
		FROM   #HomePageJobPostingDetails H
			   inner join 
			   (
					select  count(SC.ID) BookMarkedCount
					from    #HomePageJobPostingDetails H1
						    inner join gen_ShortlistedTalents_ClientPortal SC WITH(NOLOCK) ON SC.HRID = H1.HRID AND SC.ContactID = H1.ClientID
					WHERE   ISNULL(SC.IsBookMarked,0) = 1
					--Group By H1.HRID
			   )Q on 1 = 1
		

		Update H
		SET    H.HRMyListCount = Isnull(BookMarkedCount,0)
		From   #HomePageJobPostingDetails H
			   left join 
			   (
					select  count(SC.ID) BookMarkedCount, H1.HRID as HiringRequest_ID
					from    #HomePageJobPostingDetails H1
						    inner join gen_ShortlistedTalents_ClientPortal SC WITH(NOLOCK) ON SC.HRID = H1.HRID
					WHERE   ISNULL(SC.IsBookMarked,0) = 1
					GRoup By H1.HRID
			   )Q on Q.HiringRequest_ID = H.HRID

		UPDATE H
		SET    H.ActiveJobs = ISNULL(X.ActiveJobs,0)
		FROM   #HomePageJobPostingDetails H
			   inner join 
			   (
					select Count(H1.ID) as ActiveJobs,C.CompanyID as CompanyID
					from   --#HomePageJobPostingDetails H inner join 
						   gen_SalesHiringRequest H1 WITH(NOLOCK) --ON H.ClientID = H1.ContactID
						   inner join gen_Contact C WITH(NOLOCK) ON C.ID = H1.ContactID
						   inner join #HRTypeID HT WITH(NOLOCK) ON HT.HRID = H1.ID

					where	--C.ID = @ClientId 
							C.CompanyID = @ComapnyId
							 and Isnull(H1.JobStatusID,0) <> 2 and H1.Isactive = 1 --and Isnull(C.IsFromSignUp,0) = 1
					Group By C.CompanyID 
			   )X ON X.CompanyID = H.CompanyID

		UPDATE H
		SET    H.ClosedJobs = ISNULL(X.ClosedJobs,0)
		FROM   #HomePageJobPostingDetails H
			   inner join 
			   (
					select Count(H1.ID) as ClosedJobs,C.CompanyID as CompanyID
					from   --#HomePageJobPostingDetails H inner join 
						   gen_SalesHiringRequest H1 WITH(NOLOCK) --ON H.ClientID = H1.ContactID
						   inner join gen_Contact C WITH(NOLOCK) ON C.ID = H1.ContactID
					where	--C.ID = @ClientId  
							C.CompanyID = @ComapnyId
							and H1.JobStatusID = 2  --and Isnull(C.IsFromSignUp,0) = 1
					Group By C.CompanyID 
			   )X ON X.CompanyID = H.CompanyID

		--If user role is for myjobs then only show the jobs posted by them.
		DECLARE @RoleID INT = 1	
		SELECT TOP 1 @RoleID = RoleID FROM LoggedInUserRole_ClientPortal(@ClientID)

		IF(ISNULL(@RoleID,0) = 3)
			BEGIN
				select	ClientID,HRID,Isnull(JobTitle,'') JobTitle,ActiveJobs,ClosedJobs,Shortlistedcount,TotalShortlistedCount = 0,Isnull(RoleId,0) RoleId,IsNew,TotalCount,MyListCount,HRMyListCount
				from	#HomePageJobPostingDetails WHERE ClientID = @ClientID
				Order By CreatedByDateTime desc
			END
		ELSE
			BEGIN
				select ClientID,HRID,Isnull(JobTitle,'') JobTitle,ActiveJobs,ClosedJobs,Shortlistedcount,TotalShortlistedCount = 0,Isnull(RoleId,0) RoleId,IsNew,TotalCount,MyListCount,HRMyListCount
				from #HomePageJobPostingDetails
				Order By CreatedByDateTime desc
			END

		Drop table #HomePageJobPostingDetails
		DROP TABLE #HRTypeID
END