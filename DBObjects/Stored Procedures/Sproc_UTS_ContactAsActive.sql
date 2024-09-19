ALTER PROCEDURE [dbo].[Sproc_UTS_ContactAsActive]
	@ContactID BIGINT NULL
AS
BEGIN
	--IF EXISTS (SELECT TOP 1 ID FROM gen_Contact WITH (NOLOCK) WHERE ID = @ContactID AND IsActive = 0)
	IF EXISTS (SELECT TOP 1 ID FROM gen_Contact WITH (NOLOCK) WHERE ID = @ContactID AND ISNULL(Password,'') = '')
	BEGIN
		UPDATE gen_Contact
		SET IsActive = 1 , [Password] = 'Uplers@123'
		WHERE ID = @ContactID
	END
	DECLARE @CompanyID as bigint = 0
	SELECT @CompanyID = CompanyID From gen_Contact with(nolock) where ID = @ContactID

	IF EXISTS (SELECT ID FROM gen_Company WITH (NOLOCK) WHERE ID = @CompanyID AND IsActive = 0)
	BEGIN
		
		---RIYA (09 MAY 2024) Update the Company typeId of the company if null.
		DECLARE @Companys TABLE
		(
			ID BIGINT,
			HRTypeID INT,
			IsTransparent BIT
		)
		INSERT INTO @Companys
		SELECT  CO.ID, HR.HRTypeID, HR.IsTransparentPricing
		FROM [dbo].[gen_SalesHiringRequest] HR WITH(NOLOCK) 
		INNER JOIN gen_Contact C WITH(NOLOCK) ON C.ID = HR.ContactId
		INNER JOIN gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID
		WHERE ISNULL(CO.CompanyTypeID,0) = 0 AND ISNULL(CO.AnotherCompanyTypeID,0) = 0 AND CO.ID = @CompanyID
		ORDER BY CO.ID DESC

		UPDATE C SET 
		C.IsActive = 1,
		C.CompanyTypeID = CASE WHEN ISNULL(C.CompanyTypeId,0) = 0 THEN CASE WHEN S.HRTypeID = 1 THEN 1 ELSE 2 END ELSE C.CompanyTypeId END,
		C.IsTransparentPricing = CASE WHEN C.IsTransparentPricing IS NULL THEN  
										CASE WHEN  S.IsTransparent = 1 THEN 1 ELSE 0 END 
								 ELSE C.IsTransparentPricing END
		FROM gen_Company C WITH(UPDLOCK)
			LEFT JOIN @Companys S ON C.ID = S.ID 
		WHERE C.ID = @CompanyID
		
	END

	
END
