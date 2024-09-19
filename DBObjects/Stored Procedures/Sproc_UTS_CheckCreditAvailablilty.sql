ALTER PROCEDURE [DBO].[Sproc_UTS_CheckCreditAvailablilty] 
	@CompanyId BIGINT = NULL
AS
BEGIN
	SELECT
	ID AS CompanyID, 
	Company AS CompanyName,
	JPCreditBalance, 
	JobPostCredit, 
	IsPostaJob,
	IsProfileView,
	IsCreditAvailable = 
	CASE
		WHEN IsPostaJob = 1 AND JPCreditBalance >= JobPostCredit THEN CAST (1 AS BIT)
		ELSE CAST (0 AS BIT) END 
	FROM gen_company 
	WITH (NOLOCK) WHERE ID = @CompanyId
END
GO
