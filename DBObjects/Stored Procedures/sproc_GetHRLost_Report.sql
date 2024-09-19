USE [TC_QA]
GO
/****** Object:  StoredProcedure [dbo].[sproc_GetHiringRequest]    Script Date: 9/29/2023 12:03:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec [sproc_GetHRLost_Report]1,100,'HRCreatedDate','desc','2022-07-01','2023-07-01','','','',''
ALTER proc [dbo].[sproc_GetHRLost_Report]
	@PageIndex				INT = 1,
	@PageSize				INT = 50,
	@SortExpression			nvarchar(100) = 'LostDate',
	@SortDirection			nvarchar(100) = 'desc',
	@HRLostFromDate		    nvarchar(100) = NULL,
	@HRLostToDate		    nvarchar(100)  = NULL,
	@LostReason				nvarchar(200)  = NULL,
	@SalesUser				nvarchar(200)  = NULL,
	@Client			    	nvarchar(100)  = NULL,
	@searchText			    NVARCHAR(500) = ''
	
	AS
	BEGIN
	
		DECLARE @WhereClauseSQL nvarchar(max) = ''
		DECLARE @MainSQL nvarchar(max) = ''
		declare @Total_Records		Nvarchar(MAX) 

		
IF @HRLostFromDate is not null and @HRLostToDate is not null and ISNULL(@HRLostFromDate,'') <> '' and ISNULL(@HRLostToDate,'') <> '' 
   BEGIN
	   IF @HRLostFromDate = @HRLostToDate
	   begin 
			SET @WhereClauseSQL += ' AND cast(CONVERT(datetime,LostDate,103) as date) = '+ '''' + @HRLostFromDate +'''';
	   end
	   else
	   begin
			SET @WhereClauseSQL+= ' AND (CAST(CONVERT(datetime,LostDate,103) as date) BETWEEN ''' + @HRLostFromDate + ''' and ''' + @HRLostToDate + ''')'
	   end
   END
		IF isnull(@LostReason,'') <> ''
			SET @WhereClauseSQL += ' AND LostReason LIKE ''%' + CONVERT(nvarchar,@LostReason) + '%'''
		IF isnull(@Client,'') <> ''
			SET @WhereClauseSQL += ' AND Client LIKE ''%' + CONVERT(nvarchar,@Client) + '%'''
		
		IF isnull(@SalesUser,'') <> ''
			SET @WhereClauseSQL += ' AND SalesUser LIKE ''%' + CONVERT(nvarchar,@SalesUser) + '%'''
		
			

		
		;WITH CTE_Records AS (SELECT H.ID as HRID,
							   ISNULL(H.HR_Number,'') HR_Number,
							   ISNULL(SU.FullName,'') SalesUser,
							   CASE WHEN SU.IsNewUser = 1 THEN 'New' ELSE 'Repeat' END ClientType,
							   ISNULL(C.Fullname,'') + ' ('+ISNULL(C.EmailId,'') + ')' Client,
							 --  ISNULL(H.NoOfTalents,0) as TotalTR,
							   cast(case when Availability ='Part Time' then  cast(H.NoofTalents as decimal(18,1))/2   else ISNULL(H.NoofTalents,0) end as decimal(18,1)) as TotalTR,
							   cast(case when H.Availability ='Part Time' then  cast(TRA.UpdatedTR as decimal(18,1))/2   else ISNULL(TRA.UpdatedTR,0) end as decimal(18,1)) as TRLostCount,
							   ISNULL(H.CreatedByDatetime,'') as HRCreatedDate,
							   ISNULL(Co.Company,'') as Company,
							   ISNULL(TRA.Reason,'') as LostReason,
							   ISNULL(LU.FullName,'') as LostDoneBy,
							   ISNULL(TRA.CreatedByDateTime,'') LostDate,
							   ISNULL(CTP1.NoOfTalent,0) Talents,
							   1 AS TotalCountID
							FROM gen_SalesHiringRequest H WITH(NOLOCK) 
							INNER JOIN gen_SalesHiringRequest_Details HD WITH(NOLOCK) ON H.ID = HD.HiringRequest_ID
							INNER JOIN prg_HiringRequest_RoleStatus RS WITH(NOLOCK) ON RS.ID = HD.RoleStatus_ID
							INNER JOIN prg_HiringRequestStatus S WITH(NOLOCK) ON S.ID =  H.Status_ID
							INNER JOIN gen_Contact C WITH(NOLOCK) ON H.ContactID = C.ID
							INNER JOIN gen_Company Co WITH(NOLOCK) ON CO.Id = C.CompanyId
							INNER JOIN usr_User Sales WITH(NOLOCK) ON Sales.ID = H.SalesUserID	
							INNER JOIN gen_History His WITH(NOLOCK) ON His.HiringRequest_Id = H.ID 
							LEFT JOIN gen_SalesHR_TRUpdated_Details TRA with(nolock) ON TRA.HiringRequestID = H.ID 
							LEFT JOIN usr_User SU WITH(NOLOCK) ON H.SalesUserID = SU.ID 		
							LEFT JOIN usr_User LU WITH(NOLOCK) ON LU.ID = TRA.CreatedByID
							OUTER APPLY (SELECT COUNT(1) as NoOfTalent FROM gen_ContactTalentPriority CTP WITH(NOLOCK) where  CTP.HiringRequestID = H.ID)CTP1
							where His.Action_ID in (80,82) and TRA.IsDecreased = 1 and TRA.IsTRLost = 1 and H.Status_ID = 6 and  HD.RoleStatus_ID= 9)								
							,Cte_TotalCount AS(
									Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
							)
							
							SELECT	U.HRID,U.HR_Number,U.SalesUser,U.ClientType,U.Client,U.TotalTR,U.TRLostCount,U.HRCreatedDate,U.Company,
									U.LostReason,U.LostDoneBy,U.LostDate,U.Talents,T.TotalRecords
							INTO	#AllRecords_LostHR
							FROM	CTE_Records U 
									INNER JOIN Cte_TotalCount T on T.TotalCountID = U.TotalCountID WHERE 1=1 
							    AND  (
										 U.HR_Number LIKE '%' + @SearchText + '%'
										OR U.SalesUser LIKE '%' + @SearchText + '%' 
										OR U.ClientType LIKE '%' + @SearchText + '%' 
										OR U.Client LIKE '%' + @SearchText + '%'
										OR U.LostReason LIKE '%' + @SearchText + '%'
										OR U.LostDoneBy LIKE '%' + @SearchText + '%'
										OR U.Company LIKE '%' + @SearchText + '%'
									)								
								
								
		SET @MainSQL = 'select HRID,HR_Number,SalesUser,ClientType,Client,TotalTR,TRLostCount,HRCreatedDate,Company,LostReason,LostDoneBy,LostDate,Talents,TotalRecords
								FROM #AllRecords_LostHR  WHERE 1=1 ' + @WhereClauseSQL + '';

		set   @Total_Records = 'Update #AllRecords_LostHR  SET TotalRecords = (select Count(1) from #AllRecords_LostHR
									    WHERE 1 = 1  ' + @WhereClauseSQL + ')'
					
			EXECUTE sp_executesql @Total_Records
		
					
		If @SortExpression = 'LostDate'
				BEGIN
					SET @MainSQL += ' ORDER BY CONVERT(datetime,' + @SortExpression + ',103) ' + @SortDirection	
								
				END
		ELSe 
			SET @MainSQL += ' ORDER BY ' + @SortExpression + ' ' + @SortDirection		
						
		IF(@PageSize>0)
			BEGIN
				SET @MainSQL= 	@MainSQL+ '	OFFSET ' + CONVERT(nvarchar, ((@PageIndex - 1)  * @PageSize)) + ' ROWS 
								FETCH NEXT ' + CONVERT(nvarchar, @PageSize) + ' ROWS ONLY ';
			END 
							
		EXECUTE sp_executesql @MainSQL	
end