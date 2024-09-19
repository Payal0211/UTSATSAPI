ALTER PROCEDURE [dbo].[sproc_UTS_GetAutoComplete_CityStateWise]  
@City			NVarchar(100) = null
AS
BEGIN

	 DECLARE @WhereClauseSQL NVARCHAR(MAX) = '';

	 IF ISNULL(@City,'') <> ''
		 BEGIN
			SET @WhereClauseSQL += 'AND (City like ''' + @City + '%''  or State like ''' + @City + '%'' or Country like ''' + @City + '%'') '
		 END

		SET @City = ltrim(rtrim(@City));
		DECLARE @MainSQL As NVARCHAr(MAX)
			
		
		SET @MainSQL = '	select	row_Number() over(order by PC.Id) Row_ID,
										Location = PC.City + '','' + PC.State + '','' + PC.Country
								from	prg_City_StateWise PC WITH(NOLOCK) 										
								where   1=1 ' + @WhereClauseSQL + ''
	
		exec sp_executesql @MainSQL	
END