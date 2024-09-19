

ALTER PROCEDURE [dbo].[Sproc_Get_Credit_Transaction_CompanyWise]
 @CompanyId				bigint = 0,
 @PageIndex				INT = 1,
 @PageSize				INT = 100,
 @SortExpression		NVARCHAR(100) = 'CreatedByDate',
 @SortDirection			NVARCHAR(100) = 'DESC'
AS
BEGIN
		DECLARE @MainSQL nvarchar(max) = '';
		DECLARE @Total_Records	Nvarchar(MAX) 


		IF OBJECT_ID('tempdb..#CreditUtilization') Is not null
			DROP TABLE #CreditUtilization

		CREATE TABLE #CreditUtilization
		(
			TransactionID	bigint,
			HRID			bigint,
			HRNumber		nvarchar(50),
			TalentName		Nvarchar(200),
			Company			Nvarchar(200),
			Client			Nvarchar(200),
			CreatedByDate	Nvarchar(30),
			CreditBalance	int default 0,
			PackageName		Nvarchar(200) default '', 
			AmountPerCredit Decimal(18,2) default 0,
			CreditCurrency  Nvarchar(10)  default '',
			CreditUsed		decimal(18,2) default 0,
			JPCreditBalance	decimal(18,2) default 0,
			VettedCount		decimal(18,2) default 0,
			NonVettedCount  decimal(18,2) default 0,
			CurrentCurrency	nvarchar(100) default '',
			CurrentAmount	decimal(18,2) default 0
		)

		Insert into #CreditUtilization(TransactionID,HRID,HRNumber,TalentName,Company,Client,CreatedByDate,PackageName,JPCreditBalance,CurrentCurrency,CurrentAmount)
		select  JTC.ID,H.ID,H.HR_Number,Isnull(T.Name,''),Co.Company,C.FullName,Convert(datetime,JTC.CreatedByDateTime,103),Isnull(JSC.BalanceType,''),CO.JPCreditBalance,
				Isnull(CO.CreditCurrency,''),Isnull(Co.CreditAmount,0)
		from    gen_JobPost_TransactionHistory_ClientPortal JTC WITH(NOLOCK) 
		        inner join gen_JobPost_Subscription_History_ClientPortal JSC WITH(NOLOCK) ON JTC.CompanyID = JSC.CompanyID and JTC.SubScriptionID = JSC.ID
				inner join gen_Company CO WITH(NOLOCK) ON CO.ID = JTC.CompanyID
				inner join gen_Contact C WITH(NOLOCK) ON C.ID = JTC.ContactID
				inner join gen_SalesHiringRequest H WITH(NOLOCK) ON H.ID = JTC.HRID
				left join  gen_Talent T WITH(NOLOCK) ON T.ATS_Talent_ID = JTC.ATS_TalentId 
		Where   JTC.CompanyID = @CompanyId
		Order by JTC.CreatedByDateTime desc
		
		
			Update C
			SET	   C.CreditBalance = Isnull(Q.CreditBalance,0),
				   C.AmountPerCredit = Isnull(Q.AmountPerCredit,0),
				   C.CreditCurrency = Isnull(Q.CreditCurrency,''),
				   C.CreditUsed = Isnull(Q.CreditUsed,0)
			FROM   #CreditUtilization C
				   inner join
				   (
						select  Isnull(JTC.CreditBalance,0) CreditBalance,Isnull(JTC.UtilizedCreditAmount,0) AmountPerCredit,
								Isnull(JTC.UtilizedCreditCurreny,'') CreditCurrency,ISNULL(JTC.CreditUsed,0) CreditUsed,
								JTC.CompanyID,JTC.HRID,JTC.ID as TransactionId
						From    #CreditUtilization C
								inner join gen_JobPost_TransactionHistory_ClientPortal JTC WITH(NOLOCK) ON C.TransactionID = JTC.ID
						Where   JTC.CompanyID = @CompanyId					
				   )Q  ON  Q.HRID = C.HRID and Q.TransactionId = C.TransactionID
			Where  C.HRID = Q.HRID and Q.TransactionId = C.TransactionID

			UPDATE  C
			SET     C.VettedCount = ISNULL(X.NoOfVettedProfileView,0)
			FROM   #CreditUtilization C
				   inner join 
				   (
						select count(ID) as NoOfVettedProfileView,CompanyID as CompanyID
						from    gen_JobPost_TransactionHistory_ClientPortal WITH(NOLOCK)
						where	CompanyID = @CompanyID and CreditOptionID = 2
						Group By CompanyID 
				   )X ON X.CompanyID = @CompanyID

			UPDATE  C
			SET     C.NonVettedCount = ISNULL(X.NoOfNonVettedProfileView,0)
			FROM   #CreditUtilization C
				   inner join 
				   (
						select count(ID) as NoOfNonVettedProfileView,CompanyID as CompanyID
						from    gen_JobPost_TransactionHistory_ClientPortal WITH(NOLOCK)
						where	CompanyID = @CompanyID and CreditOptionID = 3
						Group By CompanyID 
				   )X ON X.CompanyID = @CompanyID

				;WITH CTE_Records 
				AS (
						select *,1 as TotalCountID,1 as TotalRecords from #CreditUtilization

				   ),
				Cte_TotalCount AS(Select Count(1) AS TotalRecords,1 AS TotalCountID FROM CTE_Records
				)
				SELECT	HRID,HRNumber,TalentName,Company,Client,CreatedByDate,CreditBalance,PackageName,
						AmountPerCredit,CreditCurrency,CreditUsed,U.TotalRecords,VettedCount,NonVettedCount,JPCreditBalance,CurrentCurrency,CurrentAmount
				INTO	#AllRecords_CreditUtilization
				FROM	CTE_Records U 
						INNER JOIN Cte_TotalCount T ON T.TotalCountID = U.TotalCountID 
				WHERE 1 = 1  

				SET @MainSQL= 'SELECT	HRID,HRNumber,TalentName,Company,Client,Convert(Nvarchar,Convert(datetime,CreatedByDate,103),103) as CreatedByDate,CreditBalance,PackageName,
										AmountPerCredit,CreditCurrency,CreditUsed,TotalRecords,VettedCount,NonVettedCount,JPCreditBalance
										,CurrentCurrency,CurrentAmount
								FROM	#AllRecords_CreditUtilization WHERE 1 = 1  '

				print @MainSQL
				

				set   @Total_Records = 'Update #AllRecords_CreditUtilization 
										SET TotalRecords = (select Count(1) from #AllRecords_CreditUtilization)
									    WHERE 1 = 1  '
					
				EXECUTE sp_executesql @Total_Records

				If @SortExpression = 'CreatedByDate'
					BEGIN
						SET @MainSQL += ' ORDER BY cast(' + @SortExpression + ' as datetime) ' + @SortDirection
									
					END
				ELSe 
					SET @MainSQL += ' ORDER BY ' + @SortExpression + ' ' + @SortDirection

				IF(@PageSize>0)
					BEGIN
						SET @MainSQL= 	@MainSQL+ '	OFFSET ' + CONVERT(nvarchar, ((@PageIndex - 1)  * @PageSize)) + ' ROWS 
										FETCH NEXT ' + CONVERT(nvarchar, @PageSize) + ' ROWS ONLY';
					END

				PRINT(@MainSQL)
				
				EXECUTE sp_executesql @MainSQL

				DROP TABLE #AllRecords_CreditUtilization

End