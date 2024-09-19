---------------------------------------------------------------------------------

ALTER PROCEDURE [dbo].[Sproc_Get_PackagesForPurchase_ClientPortal]  
  @ContactId BIGINT,
  @UTMCountry NVARCHAR(250) = NULL
AS  
BEGIN  

	DECLARE @CompanyId BIGINT , 
	@CreditCurrency NVARCHAR(250),
	@CompanyAddress NVARCHAR(MAX),
	@CompanyState NVARCHAR(500),
	@BillingCompanyName NVARCHAR(MAX),
	@CompanyPinCode NVARCHAR(50),
	@CompanyPAN NVARCHAR(250),
	@CompanyGSTNumber NVARCHAR(250),
	@ShowCompanyInfo BIT,
	@CreditAmount DECIMAL(18,2),
	@ExchangeRate DECIMAL(18,2),
	@DisplayCurrency NVARCHAR(250)

	SELECT TOP 1 
	@CompanyId   = GC.CompanyID,
	@CreditCurrency = C.CreditCurrency,
	@CompanyAddress = C.Address,
	@CompanyState = C.state,
	@BillingCompanyName = C.BillingCompanyName,
	@CompanyPinCode = C.zip,
	@CompanyPAN = ISNULL(C.PANNumber,''),
	@CompanyGSTNumber = ISNULL(C.GSTNumber,''),
	@CreditAmount = C.CreditAmount,
	@DisplayCurrency = C.CreditCurrency
	FROM gen_contact GC WITH(NOLOCK) INNER JOIN gen_Company C ON C.ID = GC.CompanyID
	WHERE GC.Id = @ContactId

	SET @ShowCompanyInfo = CASE WHEN 
			(SELECT COUNT(1) FROM gen_Company ST WITH(NOLOCK) WHERE ID = @CompanyID 
			  AND (NULLIF(Address,'') IS NOT NULL 
			  AND NULLIF(state,'') IS NOT NULL 
			  AND NULLIF(BillingCompanyName,'') IS NOT NULL 
			  AND NULLIF(zip,'') IS NOT NULL
			  AND NULLIF(PANNumber,'') IS NOT NULL
			  AND NULLIF(GSTNumber,'') IS NOT NULL)) > 0 THEN 0 ELSE 1 END

	IF(LOWER(@UTMCountry) = LOWER('india'))
	BEGIN
		IF(@CreditCurrency != 'INR')
		BEGIN
			SELECT TOP 1 @ExchangeRate = ExchangeRate FROM prg_CurrencyExchangeRate WITH(NOLOCK) WHERE CurrencyCode = 'INR'
			SET @CreditAmount = @CreditAmount * @ExchangeRate
			SET @DisplayCurrency = 'INR'
		END
	END

	DECLARE @plans TABLE
	(
		PricePerCredit DECIMAL(18,0),
		NoOfCredit INT
	)

	INSERT INTO @plans
	SELECT 750, 10

	INSERT INTO @plans
	SELECT 650, 30

	INSERT INTO @plans
	SELECT 600, 50

	INSERT INTO @plans
	SELECT 500, 100

	IF(@CompanyId = 16294)
	BEGIN		
		DELETE FROM @plans 
		INSERT INTO @plans
		SELECT 1250, 1
	END

	--14326
	IF(@CompanyId = 14326)
	BEGIN		
		DELETE FROM @plans 
		INSERT INTO @plans
		SELECT 1000, 1
	END

	--SELECT * FROM @plans

	DECLARE @SubscriptionPlan TABLE
	(
		ID BIGINT IDENTITY(1,1) NOT NULL,
		PackageName NVARCHAR(250),
		PricePerCredit NVARCHAR(250),
		NoOfJobs INT,
		Package DECIMAL(18,0),
		Currency NVARCHAR(250),		
		IsTopUpPackage BIT,
		TaxPercentage DECIMAL(18,0),
		CreditValue INT,
		CompanyAddress NVARCHAR(MAX),
		CompanyState NVARCHAR(500),
		BillingCompanyName NVARCHAR(MAX),
		CompanyPinCode NVARCHAR(50),
		CompanyPAN NVARCHAR(250),
		CompanyGSTNumber NVARCHAR(250)
	)
    
		INSERT INTO @SubscriptionPlan
		SELECT 
		'Pay as you go', 
		CAST(@CreditAmount AS NVARCHAR(100)) + ' ' + @DisplayCurrency, 
		CAST(@CreditAmount AS INT), 
		@CreditAmount ,
		@DisplayCurrency,		
		1,
		IIF(@DisplayCurrency = 'INR', 18, 0),
		1,
		@CompanyAddress,
		@CompanyState,
		@BillingCompanyName,
		@CompanyPinCode, 
		@CompanyPAN, 
		@CompanyGSTNumber		

		INSERT INTO @SubscriptionPlan
		SELECT 'Pay as you go', 
		CAST(N'₹ ' + CAST(PricePerCredit AS NVARCHAR(100)) AS NVARCHAR(50)), 
		CAST(PricePerCredit AS INT), 
		NoOfCredit ,
		@CreditCurrency,		
		0,
		18,
		NULL,
		@CompanyAddress,
		@CompanyState,
		@BillingCompanyName,
		@CompanyPinCode, 
		@CompanyPAN, 
		@CompanyGSTNumber
		FROM @plans

		--INSERT INTO @SubscriptionPlan
		--SELECT 'Pay as you go', 
		--N'₹ ' + CAST(650 AS NVARCHAR(100)), 
		--CAST(650 AS INT), 
		--30 ,
		--@CreditCurrency,		
		--0,
		--18,
		--NULL,
		--@CompanyAddress,
		--@CompanyState,
		--@BillingCompanyName,
		--@CompanyPinCode, 
		--@CompanyPAN, 
		--@CompanyGSTNumber

		--INSERT INTO @SubscriptionPlan
		--SELECT 'Pay as you go', 
		--N'₹ ' + CAST(600 AS NVARCHAR(100)), 
		--CAST(600 AS INT), 
		--50 ,
		--@CreditCurrency,		
		--0,
		--18,
		--NULL,
		--@CompanyAddress,
		--@CompanyState,
		--@BillingCompanyName,
		--@CompanyPinCode, 
		--@CompanyPAN, 
		--@CompanyGSTNumber

		--INSERT INTO @SubscriptionPlan
		--SELECT 'Pay as you go', 
		--N'₹ ' + CAST(500 AS NVARCHAR(100)), 
		--CAST(500 AS INT), 
		--100 ,
		--@CreditCurrency,		
		--0,
		--18,
		--NULL,
		--@CompanyAddress,
		--@CompanyState,
		--@BillingCompanyName,
		--@CompanyPinCode, 
		--@CompanyPAN, 
		--@CompanyGSTNumber


	--IF(@CompanyID = 16351)
	--BEGIN
	--	SELECT *, @ShowCompanyInfo AS ShowCompanyInfo FROM @SubscriptionPlan	WHERE IsTopUpPackage = 1
	--END
	--ELSE

	--IF(@CreditCurrency = 'INR')
	--BEGIN
	--	SELECT *, @ShowCompanyInfo AS ShowCompanyInfo FROM @SubscriptionPlan  WHERE IsTopUpPackage = 0
	--END
	--ELSE
	--BEGIN
	--	SELECT *, @ShowCompanyInfo AS ShowCompanyInfo FROM @SubscriptionPlan	WHERE IsTopUpPackage = 1
	--END


	--select @CreditAmount, @CreditCurrency 

	--SELECT *, @ShowCompanyInfo AS ShowCompanyInfo FROM @SubscriptionPlan  WHERE IsTopUpPackage = 0
	--SELECT *, @ShowCompanyInfo AS ShowCompanyInfo FROM @SubscriptionPlan	WHERE IsTopUpPackage = 1

	IF(@CompanyId = 16265)
	BEGIN
		SELECT *, @ShowCompanyInfo AS ShowCompanyInfo FROM @SubscriptionPlan  WHERE IsTopUpPackage = 0
	END
	ELSE
	BEGIN
		SELECT *, @ShowCompanyInfo AS ShowCompanyInfo FROM @SubscriptionPlan	WHERE IsTopUpPackage = 1
	END

END  
  
