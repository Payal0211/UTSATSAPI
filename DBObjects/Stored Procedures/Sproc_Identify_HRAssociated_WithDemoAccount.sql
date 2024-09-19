CREATE PROCEDURE [dbo].[Sproc_Identify_HRAssociated_WithDemoAccount]
	@HRID BIGINT = NULL
AS
BEGIN	

	SELECT CO.ID AS 'CompanyID' FROM gen_SalesHiringRequest HR WITH(NOLOCK) 
	INNER JOIN gen_Contact C WITH(NOLOCK) ON HR.ContactID = C.ID 
	INNER JOIN gen_Company CO WITH(NOLOCK) ON C.CompanyID = CO.ID

END