CREATE TRIGGER AfterInsertTrigger_Gen_Company
ON gen_company
AFTER INSERT
AS
BEGIN

	DECLARE 
		@CompanyName NVARCHAR(1000) = NULL,
		@CompanyID BIGINT = NULL,
		@UserID INT = NULL,
		@CompanyTypeID INT = NULL,  
		@IsTransparentPricing BIT = NULL,
		@AM_SalesPersonID INT = NULL,
		@NBD_SalesPersonID INT = NULL

		SELECT TOP 1 
		@CompanyID = ID, 
		@CompanyName = Company,
		@UserID = ModifiedByID, 	
		@CompanyTypeID = CompanyTypeID, 
		@IsTransparentPricing = IsTransparentPricing, 
		@AM_SalesPersonID = AM_SalesPersonID,
		@NBD_SalesPersonID = NBD_SalesPersonID
		FROM inserted 

		INSERT INTO Gen_Company_Updates
		(
			CompanyID, CompanyName, 
			CreatedByID, CreatedByDatetime, 
			CompanyTypeID, IsTransparentPricing, 
			AM_SalesPersonID, NBD_SalesPersonID,
			[Action]
		)
		VALUES
		(
			@CompanyID, @CompanyName, 
			@UserID, GETDATE(), 
			@CompanyTypeID, @IsTransparentPricing,
			@AM_SalesPersonID, @NBD_SalesPersonID, 
			'Insert'
		)
    
END;