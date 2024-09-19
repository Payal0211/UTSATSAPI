USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[Sproc_Get_Acheived_Target_Details]    Script Date: 20-02-2023 14:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sproc_UTS_Get_Acheived_Target_Details] 
(
	 @PageIndex				INT = 1
	,@PageSize				INT = 50
	,@SortExpression		nvarchar(100) = 'HRID'
	,@SortDirection			nvarchar(100) = 'asc'
	,@HRNumber				NVARCHAR(200) = ''
	,@Client				NVARCHAR(200) = ''
	,@EngagemenID			NVARCHAR(200) = ''
	,@TalentName			NVARCHAR(200) = ''
	,@UserRole				NVARCHAR(200) = ''
	,@InvoiceNumber			NVARCHAR(200) = ''
	,@CompanyCategory		Nvarchar(200) = ''
	,@Month					INT = 11
	,@Year					INT = 2022
)
AS
BEGIN

	DECLARE @WhereClauseSQL nvarchar(max) = ''
	DECLARE @MainSQL nvarchar(max) = ''
	DECLARE @TmpDBSQL nvarchar(max) = ''
	DECLARE @WhereClauseSQL1 nvarchar(max) = ''
	DECLARE @AcheivedCondition	NVARCHAR(MAX) = '';


		
		IF ISNULL(@HRNumber,'') <> ''
			SET @AcheivedCondition += ' AND HRNumber LIKE ''%' + CONVERT(nvarchar,@HRNumber) + '%'''
		IF ISNULL(@Client,'') <> ''
			SET @AcheivedCondition += ' AND Client LIKE ''%' + CONVERT(nvarchar,@Client) + '%'''
		IF ISNULL(@EngagemenID,'') <> ''
			SET @AcheivedCondition += ' AND EngagemenID LIKE ''%' + CONVERT(nvarchar,@EngagemenID) + '%'''
		IF ISNULL(@TalentName,'') <> ''
			SET @AcheivedCondition += ' AND TalentName LIKE ''%' + CONVERT(nvarchar,@TalentName) + '%'''
		IF ISNULL(@UserRole,'') <> ''
			SET @AcheivedCondition += ' AND User_Role LIKE ''%' + CONVERT(nvarchar,@UserRole) + '%'''
		IF ISNULL(@InvoiceNumber,'') <> ''
			SET @AcheivedCondition += ' AND InvoiceNumber LIKE ''%' + CONVERT(nvarchar,@InvoiceNumber) + '%'''
		IF ISNULL(@CompanyCategory,'') <> ''
			SET @AcheivedCondition += ' AND CompanyCategory LIKE ''%' + CONVERT(nvarchar,@CompanyCategory) + '%'''

		IF @Month > 0 
			SET @WhereClauseSQL += ' AND tad.TargetMonth = ' +CONVERT(VARCHAR(5), @Month)
		IF @Year > 0 
			SET @WhereClauseSQL += ' AND tad.TargetYear = ' + CONVERT(VARCHAR(5), @Year)

		SET @MainSQL = 'With CTE_Records AS ( SELECT
							 tad.ID 
							,User_Role = usr.FullName + char(10)+'' (''+URole.UserRole+'')''
							,shr.HR_Number AS HRNumber
							,Client = gc.FullName + char(10)+'' (''+gc.EmailID+'')''
							,gpi.InvoiceNumber
							,tad.TargetDate
							,1 as TotalCountID
							,tad.CreatedByDateTime
							,obt.EngagemenID
							,T.Name as TalentName
							,Tad.CompanyCategory
							,shr.ID as HRID
							FROM gen_Inc_Target_Acheived_Details tad WITH (NOLOCK)
							Inner JOIN usr_User usr WITH (NOLOCK) ON tad.UserId = usr.ID
							Inner JOIN gen_SalesHiringRequest shr WITH (NOLOCK) ON tad.HiringRequestId = shr.ID
							--inner JOIN gen_OnBoardTalents G_OBT WITH(NOLOCK) ON G_OBT.HiringRequest_ID = shr.ID
							Inner JOIN gen_Contact gc WITH (NOLOCK) ON shr.ContactID = gc.ID
							LEFT JOIN gen_OnBoardTalents obt WITH (NOLOCK) ON tad.OnBoardId = obt.ID
							left join gen_Talent T WITH(NOLOCK) ON T.ID = OBT.Talent_ID							
							LEFT JOIN gen_Payout_Information gpi WITH (NOLOCK) ON  gpi.ESalesInvoiceID = tad.InvoiceId
							left join usr_UserRoleDetails usrRole WITH (NOLOCK) On usrRole.User_ID = usr.ID
							left join usr_UserRole URole WITH(NOLOCK) ON URole.ID = usrRole.UserRole_ID
							WHERE 1 = 1 '+ @WhereClauseSQL+'
						)
						,Cte_TotalCount AS(
										Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
						)
						SELECT U.ID, User_Role,Client,HRID,CompanyCategory,TalentName, U.HRNumber, U.InvoiceNumber, CONVERT(varchar, ISNULL(U.TargetDate, ''''), 103)  TargetDate,(CONVERT(varchar, ISNULL(CreatedByDatetime, ''''), 103) + '' ''  + convert(VARCHAR(8), CreatedByDatetime,14)) CreatedByDatetime,T.TotalRecords,EngagemenID
						FROM CTE_Records U 
						INNER JOIN Cte_TotalCount T on T.TotalCountID =U.TotalCountID WHERE 1=1 '+ @AcheivedCondition + ''

						IF	@SortExpression = 'HRID'
									SET @MainSQL += ' ORDER BY User_Role ' + @SortDirection + ', HRID  DESC'
							ELSE 
								SET @MainSQL += ' ORDER BY ' + @SortExpression + ' ' + @SortDirection

					   IF(@PageSize>0)
						  BEGIN
								SET @MainSQL= 	@MainSQL+ '	OFFSET ' + CONVERT(nvarchar, ((@PageIndex - 1)  * @PageSize)) + ' ROWS 
												FETCH NEXT ' + CONVERT(nvarchar, @PageSize) + ' ROWS ONLY 
										  ';
						   END

	--PRINT(@MainSQL)
	EXECUTE sp_executesql @MainSQL		

END