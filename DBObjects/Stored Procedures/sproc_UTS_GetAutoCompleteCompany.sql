ALTER PROCEDURE [dbo].[sproc_UTS_GetAutoCompleteCompany] 
	@CompanyName NVARCHAR(100) = NULL
AS
BEGIN
	SELECT 
	C.ID AS CompanyID,
	C.Company AS CompanyName
	FROM gen_company C WITH(NOLOCK) 
	WHERE C.IsActive = 1 AND 
	C.Company Like 
	CASE 
		WHEN ISNULL(@CompanyName,'') <> '' 
		THEN '%' + @CompanyName + '%' 
	ELSE C.Company 
	END
END
GO


