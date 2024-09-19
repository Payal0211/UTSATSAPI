-- =============================================
-- Author:		Himani Sawadiawala
-- Create date: 10 january 2024
-- Description:	Calculate Uplersfees, ClientPay, TalentPay
-- =============================================
ALTER FUNCTION dbo.PayPerHire_Calculation 
(
    @IsTransparentPricing BIT,
	@AdHocBudget DECIMAL(18,2), 
	@BudgetFrom DECIMAL(18,2),
	@BudgetTo DECIMAL(18,2),
	@UplersFeesInPer DECIMAL(18,2),
	@IsHrDp BIT NULL = 0
)
RETURNS @CalculateResult TABLE(IsTransparentPricing BIT, Budget NVARCHAR(300), UplersFeeInAmount NVARCHAR(300), ClientPay NVARCHAR(300), TalentPay NVARCHAR(300))
AS
BEGIN
	DECLARE  @Budget NVARCHAR(300)
	DECLARE  @UplersFeeInAmount NVARCHAR(300)
	DECLARE  @ClientPay NVARCHAR(300)
	DECLARE  @TalentPay NVARCHAR(300)

	IF @AdHocBudget > 0
	BEGIN

		SET @Budget = CAST(@AdHocBudget AS NVARCHAR(100)) 
		
		IF @IsHrDp = 1
		BEGIN
			SET @UplersFeeInAmount = CAST(((@AdHocBudget * @UplersFeesInPer * 12)/100) AS DECIMAL(18,2));
		END
		ELSE
		BEGIN
			SET @UplersFeeInAmount = CAST(((@AdHocBudget * @UplersFeesInPer)/100) AS DECIMAL(18,2));
		END

		IF @IsTransparentPricing = 1
		BEGIN
			SET @ClientPay = @AdHocBudget + @UplersFeeInAmount
			SET @TalentPay = @AdHocBudget
		END
		ELSE
		BEGIN
			SET @ClientPay = @AdHocBudget
			SET @TalentPay = @AdHocBudget - @UplersFeeInAmount
		END
	END
	ELSE
	BEGIN
	    DECLARE  @Min_UplersFeeInAmount DECIMAL(18,2)
		DECLARE  @Min_ClientPay DECIMAL(18,2)
		DECLARE  @Min_TalentPay DECIMAL(18,2)

			IF @BudgetFrom > 0
			BEGIN

				IF @IsHrDp = 1
				BEGIN
					SET @Min_UplersFeeInAmount = (@BudgetFrom * @UplersFeesInPer * 12)/100;
				END
				ELSE
				BEGIN
					SET @Min_UplersFeeInAmount = (@BudgetFrom * @UplersFeesInPer)/100;
				END

				IF @IsTransparentPricing = 1
				BEGIN
					SET @Min_ClientPay = @BudgetFrom + @Min_UplersFeeInAmount
					SET @Min_TalentPay = @BudgetFrom
				END
				ELSE
				BEGIN
					SET @Min_ClientPay = @BudgetFrom
					SET @Min_TalentPay = @BudgetFrom - @Min_UplersFeeInAmount
				END
			END

		DECLARE  @Max_UplersFeeInAmount DECIMAL(18,2)
		DECLARE  @Max_ClientPay DECIMAL(18,2)
		DECLARE  @Max_TalentPay DECIMAL(18,2)

			IF @BudgetTo > 0
			BEGIN

				IF @IsHrDp = 1
				BEGIN
					SET @Max_UplersFeeInAmount = (@BudgetTo * @UplersFeesInPer * 12)/100;
				END
				ELSE
				BEGIN
					SET @Max_UplersFeeInAmount = (@BudgetTo * @UplersFeesInPer)/100;
				END

				IF @IsTransparentPricing = 1
				BEGIN
					SET @Max_ClientPay = @BudgetTo + @Max_UplersFeeInAmount
					SET @Max_TalentPay = @BudgetTo
				END
				ELSE
				BEGIN
					SET @Max_ClientPay = @BudgetTo
					SET @Max_TalentPay = @BudgetTo - @Max_UplersFeeInAmount
				END
			END

			SET @Budget = CAST(@BudgetFrom AS NVARCHAR(100)) + ' - ' + CAST(@BudgetTo AS NVARCHAR(100))
			SET @UplersFeeInAmount = CAST(@Min_UplersFeeInAmount AS NVARCHAR(100)) + ' - ' + CAST(@Max_UplersFeeInAmount AS NVARCHAR(100))
			SET @ClientPay = CAST(@Min_ClientPay AS NVARCHAR(100)) + ' - ' + CAST(@Max_ClientPay AS NVARCHAR(100))
			SET @TalentPay = CAST(@Min_TalentPay AS NVARCHAR(100)) + ' - ' + CAST(@Max_TalentPay AS NVARCHAR(100))
	END

	INSERT INTO @CalculateResult(IsTransparentPricing, Budget, UplersFeeInAmount, ClientPay, TalentPay)
	VALUES(@IsTransparentPricing, @Budget, @UplersFeeInAmount, @ClientPay, @TalentPay)

	RETURN
END

