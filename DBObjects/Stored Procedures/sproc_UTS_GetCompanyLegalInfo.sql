USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[sproc_GetCompanyLegalInfo]    Script Date: 15-02-2023 17:06:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- exec sproc_GetCompanyLegalInfo 1,100,'ID','DESC','',0,'','','','up'

CREATE PROCEDURE [dbo].[sproc_UTS_GetCompanyLegalInfo]
	@PageIndex				INT = 1,
	@PageSize				INT = 50,
	@SortExpression			nvarchar(100) = 'ID',
	@SortDirection			nvarchar(100) = 'DESC',
	@Id						bigint = NULL,
	@CompanyId               bigint = NULL,
	@DocumentType			nvarchar(100) = NULL,
	@DocumentName			nvarchar(100) = NULL,
	@AgreementStatus		nvarchar(100) = NULL,
	@Company		nvarchar(100) = NULL
AS
BEGIN
	DECLARE @WhereClauseSQL nvarchar(max) = ''
	DECLARE @MainSQL nvarchar(max) = ''

	IF( isnull(@CompanyId,0) >0		)
			SET @WhereClauseSQL += ' AND L.CompanyId = ' + CONVERT(nvarchar,@CompanyId)
    IF isnull(@DocumentType,'') <> ''
			SET @WhereClauseSQL += ' AND L.DocumentType LIKE ''%' + CONVERT(nvarchar,@DocumentType) + '%'''
	IF isnull(@DocumentName,'') <> ''
			SET @WhereClauseSQL += ' AND L.DocumentName LIKE ''%' + CONVERT(nvarchar,@DocumentName) + '%'''
    IF isnull(@AgreementStatus,'') <> ''
			SET @WhereClauseSQL += ' AND L.AgreementStatus LIKE ''%' + CONVERT(nvarchar,@AgreementStatus) + '%'''
	IF isnull(@Company,'') <> ''
			SET @WhereClauseSQL += ' AND T.Company LIKE ''%' + CONVERT(nvarchar,@Company) + '%'''
	

	SET @MainSQL= ';WITH CTE_Records AS ( SELECT ID = L.ID,T.Company,CompanyID = L.CompanyId,DocumentType = L.DocumentType,DocumentName = L.DocumentName,DocumentURL = L.DocumentURL,AgreementStatus = L.AgreementStatus ,1 as TotalCountID,(ISNULL(L.SignedDate, 
''''))  SignedDate,(ISNULL(L.Validity_StartDate, ''''))  Validity_StartDate,(ISNULL(L.Validity_EndDate, ''''))  Validity_EndDate
					FROM	 gen_CompanyLegalInfo L WITH(NOLOCK) 
					INNER JOIN gen_Company T WITH(NOLOCK) ON T.ID = L.CompanyID
					  WHERE 1=1 ' + @WhereClauseSQL + '
					  )
					  ,Cte_TotalCount AS(
										Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
								)
								SELECT ID,Company,CompanyID,DocumentType,DocumentName,DocumentURL,AgreementStatus,T.TotalRecords, SignedDate = case when SignedDate=''01/01/1900'' then '''' else  CONVERT(VARCHAR(10), SignedDate, 103) end, Validity_StartDate = case when 
								Validity_StartDate=''01/01/1900'' then '''' else  CONVERT(VARCHAR(10), Validity_StartDate, 103) end ,Validity_EndDate = case when Validity_EndDate=''01/01/1900'' then '''' else  CONVERT(VARCHAR(10), Validity_EndDate, 103) end 
								FROM CTE_Records U 
								INNER JOIN Cte_TotalCount T on T.TotalCountID =U.TotalCountID '

						
		
		If @SortExpression = 'Validity_StartDate' or @SortExpression = 'Validity_EndDate' or @SortExpression = 'SignedDate'
				BEGIN
					SET @MainSQL += ' ORDER BY cast(U.' + @SortExpression + ' as datetime) ' + @SortDirection
				END
		ELSE
			SET @MainSQL += ' ORDER BY ' + @SortExpression + ' ' + @SortDirection


	    PRINT(@MainSQL)
	EXECUTE sp_executesql @MainSQL		

END
