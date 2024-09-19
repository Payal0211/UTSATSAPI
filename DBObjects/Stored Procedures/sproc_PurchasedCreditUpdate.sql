ALTER PROCEDURE [dbo].[sproc_PurchasedCreditUpdate]
@CompanyID bigint = null,
@Credits decimal = null,
@ContactID bigint = null,
@CreatedByID bigint = null,
@PaymentHistoryId	BIGINT = NULL,
@PackageID BIGINT NULL,
@IsActive  bit = 0,
@IsAdvancePurchase  bit = 0,
@PerCreditAmount DECIMAL(18,2) = null, 
@CreditCurrency	Nvarchar(50) = null,
@IsRazorPay   BIT = 0

AS
begin
	IF @Credits > 0 
		BEGIN
			Declare @SubscriptionId as bigint = 0		
			Declare @TransactionId as Bigint = 0
			Declare @JPCreditBalance as decimal(18,2) = 0
			DECLARE @IsDataInserted BIT = 0
			DECLARE @PackageName NVARCHAR(500)
			


			SELECT TOP 1 @PackageName = PackageName FROM prg_Subscription_Package_ClientPortal  WITH(NOLOCK) WHERE ID = @PackageID

			IF(@IsRazorPay = 1)
			BEGIN

							INSERT INTO gen_JobPost_Subscription_History_ClientPortal 
							(
								ContactID,
								BalanceType,
								CreditBalance,
								IsActive,
								CreatedByID,
								CreatedByDateTime,
								CompanyID,
								PackageID,
								PaymentHistoryID,
								IsAdvancePurchase,
								CreditAmount,
								CreditCurrency
							)
							SELECT 
								@ContactID,
								PackageName,
								@Credits, --If credits added then pick that value
								CASE WHEN @IsRazorPay = 1 THEN 0 ELSE Isnull(@IsActive,0) END, 
								@CreatedByID,
								GETDATE(),
								@CompanyID,
								@PackageID,
								@PaymentHistoryID,
								Isnull(@IsAdvancePurchase,0),
								@PerCreditAmount,
								@CreditCurrency

								FROM prg_Subscription_Package_ClientPortal WHERE ID = @PackageID

							SET @SubscriptionId = @@IDENTITY

							SET @IsDataInserted = 1
				END
			ELSE
			BEGIN
				SELECT TOP 1 @SubscriptionId = ID FROM gen_JobPost_Subscription_History_ClientPortal WITH(NOLOCK) WHERE PaymentHistoryID = @PaymentHistoryId AND IsActive = 0
			END

			print @SubscriptionId
			
			IF(@SubscriptionId = 0 AND @IsRazorPay = 0)
			BEGIN

				INSERT INTO gen_JobPost_Subscription_History_ClientPortal 
							(
								ContactID,
								BalanceType,
								CreditBalance,
								IsActive,
								CreatedByID,
								CreatedByDateTime,
								CompanyID,
								PackageID,
								PaymentHistoryID,
								IsAdvancePurchase,
								CreditAmount,
								CreditCurrency
							)
							SELECT 
								@ContactID,
								PackageName,
								@Credits, --If credits added then pick that value
								CASE WHEN @IsRazorPay = 1 THEN 0 ELSE Isnull(@IsActive,0) END, 
								@CreatedByID,
								GETDATE(),
								@CompanyID,
								@PackageID,
								@PaymentHistoryID,
								Isnull(@IsAdvancePurchase,0),
								@PerCreditAmount,
								@CreditCurrency

								FROM prg_Subscription_Package_ClientPortal WHERE ID = @PackageID

				SET @SubscriptionId = @@IDENTITY

				SET @IsDataInserted = 1
			END


			print @SubscriptionId

			IF(@SubscriptionId > 0 AND @IsRazorPay = 0)
			BEGIN

					INSERT INTO [dbo].[gen_JobPost_TransactionHistory_ClientPortal]
					 (
						[HRID]
					   ,[ContactID]
					   ,[ContactUserID]
					   ,[JobPostedDate]           
					   ,[CreditBalance]  
					   ,[CreatedByID]
					   ,[CreatedByDateTime]          
					   ,[CompanyID]
					   ,[SubScriptionID]
					   ,BalanceUsedType
					   ,UtilizedCreditAmount
					   ,UtilizedCreditCurreny
					 )
				 VALUES
					   (0
					   ,@ContactID
					   ,@ContactID
					   ,GetDate()          
					   ,@Credits          
					   ,@CreatedByID
					   ,GetDate()         
					   ,@CompanyID
					   ,@SubscriptionId
					   ,@PackageName
					   ,@PerCreditAmount,
						@CreditCurrency
					   )

					 SET @TransactionId = @@IDENTITY

					IF (@TransactionId >0)
						BEGIN							

								update gen_Company
								SET JPCreditBalance = ISNULL(JPCreditBalance,0) + @Credits
								where ID = @CompanyID

								Select @JPCreditBalance =isnull(JPCreditBalance,0) from gen_Company with(nolock) where ID = @CompanyID

								UPDATE gen_JobPost_TransactionHistory_ClientPortal
								SET JPCreditBalance = @JPCreditBalance
								WHERE ID = @TransactionId and CompanyID = @CompanyID

								UPDATE gen_JobPost_Subscription_History_ClientPortal SET IsActive = @IsActive, IsAdvancePurchase = @IsAdvancePurchase
								WHERE ID = @SubscriptionId

						END
			END
		END            
	END

