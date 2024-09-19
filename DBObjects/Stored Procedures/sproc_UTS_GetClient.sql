USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[sproc_GetClient]    Script Date: 15-02-2023 17:44:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec sproc_GetClient 1,50,'Company','ASC','','jimi','jimi',''
-- Remove Column CompanyWeightedAverageCriteriaID by Reena Jain  22 April 2022

CREATE PROCEDURE [dbo].[sproc_UTS_GetClient]
	@PageIndex				INT = 1,
	@PageSize				INT = 50,
	@SortExpression			nvarchar(100) = 'Company',
	@SortDirection			nvarchar(100) = 'ASC',
	@CompanyName			varchar(200) = NULL,
	@EmailId				varchar(200) = NULL,
	@FullName				varchar(200) = NULL,
	--@DealId					varchar(200) = NULL,	
	@CreatedByID			INT = NULL
AS
BEGIN
	
		DECLARE @WhereClauseSQL nvarchar(max) = ''
		DECLARE @MainSQL nvarchar(max) = ''
		DECLARE @TmpDBSQL nvarchar(max) = ''

		IF isnull(@CompanyName,'') <> ''
			SET @WhereClauseSQL += ' AND C.Company LIKE ''%' + CONVERT(nvarchar,@CompanyName) + '%'''
		IF isnull(@FullName,'') <> ''
			SET @WhereClauseSQL += ' AND CO.Fullname LIKE ''%' + CONVERT(nvarchar,@FullName) + '%'''
		IF isnull(@EmailId,'') <> ''
			SET @WhereClauseSQL += ' AND CO.EmailId LIKE ''%' + CONVERT(nvarchar,@EmailId) + '%'''
		

		SET @MainSQL= ';WITH CTE_Records AS (SELECT C.ID CompanyId,Co.Id  ContactId,Isnull(C.Company,'''')  Company,Isnull(CO.EmailId,'''')+ char(10) +'' (''+Username+'')'' as Email,Isnull(CO.Fullname,'''') ClientName,
													'''' Deal,1 as TotalCountID,C.CreatedByDatetime
													,[OpenHr]=(select count(ID) from gen_SalesHiringRequest SH WITH(NOLOCK) Where SH.ContactId = CO.ID AND Status_ID =1)
													,[InProcess] = (select count(ID) from gen_SalesHiringRequest SH WITH(NOLOCK) Where SH.ContactId = CO.ID AND Status_ID =2)
													,[Completed] = (select count(ID) from gen_SalesHiringRequest SH WITH(NOLOCK) Where SH.ContactId = CO.ID AND Status_ID =3),
													IsClientNotificationSend = ISnull(IsClientNotificationSend,0),
													Isnull(A.LoginCount,0) as LoginCount,
													A.LoggedInTime
													,(CONVERT(varchar, ISNULL(C.ModifiedByDatetime, ''''), 103) + '' ''  + convert(VARCHAR(8), C.ModifiedByDatetime, 14))  AS LastModifiedDatetime
											FROM	gen_Company C WITH(NOLOCK) 
													inner JOIN gen_Contact CO WITH(NOLOCK) ON C.ID = CO.CompanyID
													OUTER APPLY (
														SELECT TOP 1 LoggedInTime,COUNT(*) OVER() AS LoginCount
														FROM gen_ContactHistory COH WITH(NOLOCK) 
														WHERE COH.ClientId = CO.Id AND COH.ClientActionID = 1
														ORDER BY COH.ID DESC
														)  as A
											WHERE 1=1 and CO.IsPrimary = 1 ' + @WhereClauseSQL + '
							)
							,Cte_TotalCount AS(
									Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
							)
							SELECT	U.CompanyId,U.ContactId,U.Company, U.Email, U.ClientName, U.Deal,T.TotalRecords,(CONVERT(varchar, ISNULL(U.CreatedByDatetime, ''''), 103) + '' ''  + convert(VARCHAR(8), U.CreatedByDatetime, 14)) CreatedByDatetime,[OpenHr],[InProcess],[Completed]
									,cast(IsClientNotificationSend as int) IsClientNotificationSend,LoginCount,(CONVERT(varchar, ISNULL(U.LoggedInTime, ''''), 103) + '' ''  + convert(VARCHAR(8), U.LoggedInTime, 14))LoggedInTime,LastModifiedDatetime
							FROM	CTE_Records U 
									INNER JOIN Cte_TotalCount T on T.TotalCountID =U.TotalCountID '
								

		If @TmpDBSQL <> ''
			BEGIN
					SET @MainSQL = @TmpDBSQL + @MainSQL								
			END

		If @SortExpression = 'CreatedByDatetime'
				BEGIN
					--SET @SortExpression = 'ISNULL(CompanyId,ContactId)'
					--set @SortDirection = 'asc'
					SET @MainSQL += ' ORDER BY cast(' + @SortExpression + ' as datetime) ' + @SortDirection
									
				END
		ELSe 
			SET @MainSQL += ' ORDER BY ' + @SortExpression + ' ' + @SortDirection
		

		IF(@PageSize>0)
			BEGIN
				SET @MainSQL= 	@MainSQL+ '	OFFSET ' + CONVERT(nvarchar, ((@PageIndex - 1)  * @PageSize)) + ' ROWS 
								FETCH NEXT ' + CONVERT(nvarchar, @PageSize) + ' ROWS ONLY 
							';
			END

		

		PRINT(@MainSQL)
		EXECUTE sp_executesql @MainSQL		  	
		
		
END
