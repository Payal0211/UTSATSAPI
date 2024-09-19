CREATE PROCEDURE [dbo].[sp_UTS_GetPOCDetails]
	@CompanyID BIGINT NULL, 
	@ContactID BIGINT NULL
AS
BEGIN
IF @CompanyID > 0 and @ContactID > 0
	BEGIN
		SELECT 
			U.ID AS UserID, 
			ISNULL(U.EmployeeID,'') AS EmployeeID, 
			ISNULL(U.FullName,'') AS FullName, 
			ISNULL(U.EmailID,'') AS EmailID
			FROM gen_Company C WITH(NOLOCK) 
			INNER JOIN gen_Contact CO WITH(NOLOCK) ON C.ID = CO.CompanyID
			INNER JOIN gen_ContactPointofContact POC WITH(NOLOCK) ON CO.ID = POC.ContactID 
			INNER JOIN usr_User U WITH(NOLOCK) ON U.ID = POC.User_ID 
			WHERE C.ID = @CompanyID AND POC.ContactID = @ContactID
			ORDER BY U.FullName ASC 
	END
END
GO

		