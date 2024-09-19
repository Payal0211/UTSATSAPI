USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[sproc_Inc_GetUsers_RoleDetails]    Script Date: 20-02-2023 09:52:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sproc_UTS_Inc_GetUsers_RoleDetails]
	@PageIndex				INT = 1,
	@PageSize				INT = 80,
	@SortExpression			nvarchar(100) = 'CreatedbyDatetime',
	@SortDirection			nvarchar(100) = 'desc',
	@Id						bigint = NULL,
	@EmployeeID				varchar(200) = NULL,
	@FullName				varchar(200) = NULL,
	@UserRole				varchar(200) = NULL
AS
BEGIN
	
		DECLARE @WhereClauseSQL nvarchar(max) = ''
		DECLARE @MainSQL nvarchar(max) = ''
		DECLARE @TmpDBSQL nvarchar(max) = ''
		DECLARE @WhereClauseSQL1 nvarchar(max) = ''

		IF isnull(@EmployeeID,'') <> ''
			SET @WhereClauseSQL += ' AND U.EmployeeID LIKE ''%' + CONVERT(nvarchar,@EmployeeID) + '%'''
		IF isnull(@FullName,'') <> ''
			SET @WhereClauseSQL += ' AND U.Fullname LIKE ''%' + CONVERT(nvarchar,@FullName) + '%'''
		IF isnull(@UserRole,'') <> ''
			SET @WhereClauseSQL += ' AND UserRole LIKE ''%' + CONVERT(nvarchar,@UserRole) + '%'''

			SET @MainSQL= ';WITH CTE_Records AS (SELECT U.ID, U.EmployeeID, (ISNULL(U.FirstName,'''') + '' '' + ISNULL(U.LastName,'''')) FullName, 
															ISNULL(U.EmailID,'''') EmailID,U.CreatedByDatetime,1 as TotalCountID,UR.UserRole
													  FROM	 usr_User U WITH(NOLOCK) 
															 inner join usr_UserRoleDetails URD WITH(NOLOCK) ON URD.USER_ID = U.ID
															 inner join usr_UserRole UR WITH(NOLOCK) ON UR.ID = URD.UserRole_ID															 
													  WHERE 1=1   ' + @WhereClauseSQL + '
								)
								,Cte_TotalCount AS(
										Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
								)
								SELECT U.ID, U.EmployeeID, U.FullName, U.EmailID,(CONVERT(varchar, ISNULL(CreatedByDatetime, ''''), 103) + '' ''  + convert(VARCHAR(8), CreatedByDatetime, 14)) CreatedByDatetime,T.TotalRecords,
									UserRole	
								FROM CTE_Records U 
								INNER JOIN Cte_TotalCount T on T.TotalCountID =U.TotalCountID WHERE 1=1 ' + @WhereClauseSQL1 + ''
								

					  If @TmpDBSQL <> ''
						BEGIN
								SET @MainSQL = @TmpDBSQL + @MainSQL								
						END

					     If	@SortExpression = 'CreatedbyDatetime'
									SET @MainSQL += ' ORDER BY cast(' + @SortExpression + ' as datetime) ' + @SortDirection
							ELSe 
								SET @MainSQL += ' ORDER BY UserRole asc,FullName asc'-- + @SortExpression + ' ' + @SortDirection

					   IF(@PageSize>0)
						  BEGIN
								SET @MainSQL= 	@MainSQL+ '	OFFSET ' + CONVERT(nvarchar, ((@PageIndex - 1)  * @PageSize)) + ' ROWS 
												FETCH NEXT ' + CONVERT(nvarchar, @PageSize) + ' ROWS ONLY 
										  ';
						   END
		PRINT(@MainSQL)
		EXECUTE sp_executesql @MainSQL		  	
		
		
END