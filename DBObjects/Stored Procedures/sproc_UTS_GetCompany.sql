
--exec sproc_UTS_GetCompany
ALTER PROCEDURE [dbo].[sproc_UTS_GetCompany]
	@PageIndex				INT = 1,
	@PageSize				INT = 50,
	@SortExpression			nvarchar(100) = 'Id',
	@SortDirection			nvarchar(100) = 'DESC',
	@Company				nvarchar(100) = null,
	@CompanyDomain			nvarchar(100) = NULL,
	@Location			nvarchar(100) = NULL,
	@Contact_Status		nvarchar(100) = NULL,
	@GEO				nvarchar(50) = NULL,
	@AM_SalesPerson		nvarchar(50) = NULL,                    
	@NBD_SalesPerson    nvarchar(50) = NULL,
	@TeamLead			nvarchar(50) = NULL,
	@LeadType			nvarchar(50) = NULL,
	@LeadUser			nvarchar(50) = NULL
	
AS
BEGIN
	
		DECLARE @WhereClauseSQL nvarchar(max) = ''
		DECLARE @MainSQL nvarchar(max) = ''
		DECLARE @TmpDBSQL nvarchar(max) = ''

			IF isnull(@Company,'') <> ''
				SET @WhereClauseSQL += ' AND C.Company LIKE ''%' + CONVERT(nvarchar,@Company) + '%'''

			IF isnull(@CompanyDomain,'') <> ''
				SET @WhereClauseSQL += ' AND D.CompanyDomain LIKE ''%' + CONVERT(nvarchar,@CompanyDomain) + '%'''

			IF isnull(@Location,'') <> ''
				SET @WhereClauseSQL += ' AND C.Location LIKE ''%' + CONVERT(nvarchar,@Location) + '%'''

			IF isnull(@Contact_Status,'') <> ''
				SET @WhereClauseSQL += ' AND S.Contact_Status LIKE ''%' + CONVERT(nvarchar,@Contact_Status) + '%'''

			IF isnull(@GEO,'') <> ''
				SET @WhereClauseSQL += 'AND G.GEO LIKE ''%' + CONVERT(nvarchar,@GEO) + '%'''

			IF(isnull(@AM_SalesPerson,'')<> '')
				SET @WhereClauseSQL += 'AND UAM.FULLNAME LIKE ''%' + CONVERT(nvarchar,@AM_SalesPerson) + '%'''

			IF(isnull(@NBD_SalesPerson,'')<> '')
				SET @WhereClauseSQL += 'AND UNBD.FUllNAME LIKE ''%' + CONVERT(nvarchar,@NBD_SalesPerson) + '%'''

			IF(isnull(@TeamLead,'')<> '')
				SET @WhereClauseSQL += 'AND UTeamLead.FULLNAME LIKE ''%' + CONVERT(nvarchar,@TeamLead) + '%'''

			IF(isnull(@LeadType,'')<> '')
				SET @WhereClauseSQL += 'AND C.Lead_Type LIKE ''%' + CONVERT(nvarchar,@LeadType) + '%'''

			IF(isnull(@LeadUser,'')<> '')
				SET @WhereClauseSQL += 'AND Lusr.FUllNAME LIKE ''%' + CONVERT(nvarchar,@LeadUser) + '%'''


		SET @MainSQL= ';WITH CTE_Records AS (SELECT C.ID
													,C.Company
													,C.LinkedInProfile
													,isnull(C.CompanySize,0) CompanySize
													,C.Phone
													,D.CompanyDomain
													,C.Location
													,Contact_Status = isnull(S.Contact_Status,'''')
													,1 as TotalCountID
													,Isnull(S.color,'''') Color
													,ISNULL(C.Score,0) Score
													,ISNULL(C.Category,'''') Category
													,CompanyWeightedAverageCriteriaID = ISNULL(CWAC.ID,0)
													,ExistsOrNot = case when isnull(CWAC.CompanyID,0)=0 then 0 else 1 end
													,G.GEO
													,ISNULL(UNBD.FUllNAME,'''') NBD_SalesPerson 
													,ISNULL(UAM.FULLNAME,'''') AM_SalesPerson
													,ISNULL(UTeamLead.FULLNAME,'''') TeamLead
													,ISNULL(C.Lead_Type,'''') Lead_Type
													,ISNULL(Lusr.FUllNAME,'''') LeadUser
											FROM	gen_Company C WITH(NOLOCK) 
											LEFT JOIN prg_CompanyDomain D WITH(NOLOCK) ON D.ID = C.domain_id
											LEFT JOIN prg_ContactStatus S WITH(NOLOCK) ON S.ID = C.Client_StatusID
											Left JOIN gen_CompanyWeightedAverageCriteria CWAC WITH(NOLOCK) ON C.ID = CWAC.CompanyID	
											Left JOIN prg_GEO G WITH(NOLOCK) ON C.GEO_ID = G.ID	
											Left join usr_user UNBD WITH(NOLOCK) ON UNBD.ID = C.NBD_SalesPersonID
											Left join usr_user UAM WITH(NOLOCK) On  C.AM_SalesPersonID = UAM.ID and UAM.userTypeid =4
											Left join usr_UserHierarchy UH WITH(NOLOCK) ON UH.UserID= C.AM_SalesPersonID
											Left join usr_user UTeamLead WITH(NOLOCK) On UTeamLead.ID = UH.ParentID
											left join gen_CompanyLeadType_UserDetails CLT on CLT.CompanyID = C.ID
											left join usr_user Lusr on Lusr.ID = CLT.LeadType_UserID
											WHERE 1=1 ' + @WhereClauseSQL + '
							)
							,Cte_TotalCount AS(
									Select Count(1) as TotalRecords,1 as TotalCountID FROM CTE_Records 
							)
							SELECT	ID,Company,LinkedInProfile,CompanySize,Phone,CompanyDomain,Location,Contact_Status,T.TotalRecords,Color,Score,Category,CompanyWeightedAverageCriteriaID,ExistsOrNot,GEO,NBD_SalesPerson,AM_SalesPerson,TeamLead,Lead_Type,LeadUser
							FROM	CTE_Records U 
									INNER JOIN Cte_TotalCount T on T.TotalCountID =U.TotalCountID '
								

		If @TmpDBSQL <> ''
			BEGIN
					SET @MainSQL = @TmpDBSQL + @MainSQL								
			END
	
			--SET @MainSQL += ' ORDER BY cast(' + @SortExpression + ' as datetime) ' + @SortDirection
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
