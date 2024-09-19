ALTER PROCEDURE sproc_UTS_GetAutoCompleteCompanies
	@Company			Varchar(100) =null
AS
BEGIN
		
			DECLARE @MainSQL As NVARCHAr(MAX)

			IF  @Company <> ''
				BEGIN
						SET @MainSQL = '	select	row_Number() over(order by C.Id) Row_ID,
													C.Id CompanyID,Co.ID as ContactID,Isnull(C.Company,'''') Company, ISnull(Co.EmailID,'''') EmailID, ISNULL(Co.FullName,'''') Client 
											from	gen_Company C WITH(NOLOCK) 
											inner join gen_Contact Co WITH(NOLOCK) ON Co.CompanyID = C.Id										
											where	(Company like ''' + @Company + '%'') 
											order by CompanyID 
								
										'
					END
			 ELSE IF @Company = ''
				BEGIN
						SET @MainSQL = '	select	999 Row_ID,
													C.Id CompanyID,Co.ID as ContactID,Isnull(C.Company,'''') Company, ISnull(Co.EmailID,'''') EmailID, ISNULL(Co.FullName,'''') Client 
											from	gen_Company C WITH(NOLOCK) 
											inner join gen_Contact Co WITH(NOLOCK) ON Co.CompanyID = C.Id	 										
										'
					END

			print @MainSQL
			exec sp_executesql @MainSQL		


END
