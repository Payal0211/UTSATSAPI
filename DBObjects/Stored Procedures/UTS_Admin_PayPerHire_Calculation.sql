
-- =============================================
-- Author:		Himani Sawadiawala
-- Create date: 29 Feb 2024
-- Description:	Calculate Uplersfees, ClientPay, TalentPay for UTS
-- =============================================
--SELECT * FROM dbo.UTS_Admin_PayPerHire_Calculation(1, 1200000, 0, 0, 10, 1, '₹', 'INR');
--SELECT * FROM dbo.UTS_Admin_PayPerHire_Calculation(1, 0, 1500, 2000, 10, 1, '₹', 'USD');
--SELECT * FROM dbo.UTS_Admin_PayPerHire_Calculation(1, 100000, 0, 0, 25, 0, '₹', 'USD');
--SELECT * FROM dbo.UTS_Admin_PayPerHire_Calculation(1, 0, 15000, 25000, 25, 0, '₹', 'INR');

ALTER FUNCTION [dbo].[UTS_Admin_PayPerHire_Calculation] 
(
    @IsTransparentPricing BIT,
	@AdHocBudget DECIMAL(18,2), 
	@BudgetFrom DECIMAL(18,2),
	@BudgetTo DECIMAL(18,2),
	@UplersFeesInPer DECIMAL(18,2),
	@IsHrDp BIT NULL = 0,
	@CurrencySign nvarchar(20) = '',
	@Currency varchar(20) = ''
)
RETURNS @CalculateResult TABLE(IsTransparentPricing BIT, Budget NVARCHAR(MAX), UplersFeeInAmount NVARCHAR(MAX), ClientPay NVARCHAR(MAX), TalentPay NVARCHAR(MAX))
AS
BEGIN
	DECLARE  @Budget NVARCHAR(MAX)
	DECLARE  @UplersFeeInAmount NVARCHAR(MAX)
	DECLARE  @ClientPay NVARCHAR(MAX)
	DECLARE  @TalentPay NVARCHAR(MAX)

	DECLARE  @Min_UplersFeeInAmount DECIMAL(18,2)
	DECLARE  @Min_ClientPay DECIMAL(18,2)
	DECLARE  @Min_TalentPay DECIMAL(18,2)

	DECLARE  @Max_UplersFeeInAmount DECIMAL(18,2)
	DECLARE  @Max_ClientPay DECIMAL(18,2)
	DECLARE  @Max_TalentPay DECIMAL(18,2)

	DECLARE @PerMonth NVARCHAR(50) = ' / Month';
	DECLARE @PerAnnum NVARCHAR(50) = '  / Annum';
	DECLARE @Culture nvarchar(50) = '';

	DECLARE 
	@FormattedAdhocBudget NVARCHAR(MAX) = '',
	@FormattedBudgetFrom NVARCHAR(MAX) = '', 
	@FormattedBudgetTo NVARCHAR(MAX) = '',
	@FormattedUplersFees NVARCHAR(MAX) = '',
	@FormattedMin_UplersFeeInAmount  NVARCHAR(MAX) = '',
	@FormattedMax_UplersFeeInAmount NVARCHAR(MAX) = '',
	@FormattedMin_TalentPay NVARCHAR(MAX) = '', 
	@FormattedMax_TalentPay NVARCHAR(MAX) = '',
	@FormattedMin_ClientPay NVARCHAR(MAX) = '', 
	@FormattedMax_ClientPay NVARCHAR(MAX) = ''

	IF(ISNULL(@Currency,'') <> '')
        BEGIN
            ---take Culture & CurrencySign for Convert into proper formatting for budget
            SELECT 
			@Culture = Culture, 
			@CurrencySign = CurrencySign
            FROM prg_CurrencyExchangeRate WITH(NOLOCK) WHERE CurrencyCode = @Currency

			SELECT @FormattedAdhocBudget = 
			CASE 
				WHEN ISNULL(@AdHocBudget,0) <> 0 
				THEN format(@AdHocBudget, N'N', @Culture) 
				ELSE CAST(@AdHocBudget AS NVARCHAR(MAX)) 
			END

			SELECT @FormattedBudgetFrom = CASE WHEN ISNULL(@BudgetFrom,0) <> 0 THEN format(@BudgetFrom, N'N', @Culture)	ELSE CAST(@BudgetFrom AS NVARCHAR(MAX)) END
			SELECT @FormattedBudgetTo = CASE WHEN ISNULL(@BudgetTo,0) <> 0 THEN format(@BudgetTo, N'N', @Culture) ELSE CAST(@BudgetTo AS NVARCHAR(MAX)) END
        END


	IF @IsHrDp = 1
	BEGIN
		IF @AdHocBudget > 0
		BEGIN
			DECLARE @CalculatedUplersFeeInAmount DECIMAL(18,2);
			
			--SET @CalculatedUplersFeeInAmount = CAST((@AdHocBudget*@UplersFeesInPer*12)/100 AS DECIMAL(18,2));
			SET @CalculatedUplersFeeInAmount = CAST((@AdHocBudget*@UplersFeesInPer)/100 AS DECIMAL(18,2));
			
			-----------------apply format-----------------
			
			SELECT @FormattedUplersFees = 
			CASE 
				WHEN ISNULL(@CalculatedUplersFeeInAmount,0) <> 0 
				THEN format(@CalculatedUplersFeeInAmount, N'N', @Culture) 
				ELSE CAST(@CalculatedUplersFeeInAmount AS NVARCHAR(MAX)) 
			END
			----------------------------------------------------------
			
			SET @Budget = @CurrencySign + @FormattedAdhocBudget + @PerAnnum
			SET @TalentPay = @Budget
			SET @UplersFeeInAmount = @CurrencySign + @FormattedUplersFees + @PerAnnum
		END
		ELSE
		BEGIN
			IF @BudgetFrom > 0
			BEGIN
				SET @Min_TalentPay = @BudgetFrom
				SET @Min_UplersFeeInAmount = CAST((@Min_TalentPay*@UplersFeesInPer)/100 AS DECIMAL(18,2));
			END
			IF @BudgetTo > 0
			BEGIN
				SET @Max_TalentPay = @BudgetTo
				SET @Max_UplersFeeInAmount = CAST((@Max_TalentPay*@UplersFeesInPer)/100 AS DECIMAL(18,2));
			END

			-----------------apply format-------------------------------------------------------------------------------------------------------------------------------------
			SELECT @FormattedMin_UplersFeeInAmount = CASE WHEN ISNULL(@Min_UplersFeeInAmount,0) <> 0 THEN format(@Min_UplersFeeInAmount, N'N', @Culture) ELSE CAST(@Min_UplersFeeInAmount AS NVARCHAR(MAX)) END
			SELECT @FormattedMax_UplersFeeInAmount = CASE WHEN ISNULL(@Max_UplersFeeInAmount,0) <> 0 THEN format(@Max_UplersFeeInAmount, N'N', @Culture) ELSE CAST(@Max_UplersFeeInAmount AS NVARCHAR(MAX)) END
			
			SELECT @FormattedMin_TalentPay = CASE WHEN ISNULL(@Min_TalentPay,0) <> 0 THEN format(@Min_TalentPay, N'N', @Culture) ELSE CAST(@Min_TalentPay AS NVARCHAR(MAX)) END
			SELECT @FormattedMax_TalentPay = CASE WHEN ISNULL(@Max_TalentPay,0) <> 0 THEN format(@Max_TalentPay, N'N', @Culture) ELSE CAST(@Max_TalentPay AS NVARCHAR(MAX)) END
			------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

			SET @Budget = @CurrencySign + CAST(@FormattedBudgetFrom AS NVARCHAR(MAX)) + ' to ' + @CurrencySign + CAST(@FormattedBudgetTo AS NVARCHAR(MAX)) + @PerAnnum
			SET @UplersFeeInAmount = @CurrencySign + CAST(@FormattedMin_UplersFeeInAmount AS NVARCHAR(MAX)) + ' to ' + @CurrencySign + CAST(@FormattedMax_UplersFeeInAmount AS NVARCHAR(MAX)) + @PerAnnum
			SET @TalentPay = @CurrencySign + CAST(@FormattedMin_TalentPay AS NVARCHAR(MAX)) + ' to ' + @CurrencySign  + CAST(@FormattedMax_TalentPay AS NVARCHAR(MAX)) + @PerAnnum
		END
	END
	ELSE
	BEGIN
		IF @AdHocBudget > 0
		BEGIN

			DECLARE @CalculatedUplersFeeInAmount_Adhoc DECIMAL(18,2) = 0, @CalculatedTalentPay DECIMAL(18,2) = 0;

			SET @CalculatedTalentPay = CAST(((@AdHocBudget * 100)/(100 + @UplersFeesInPer)) AS DECIMAL(18,2))
			SET @CalculatedUplersFeeInAmount_Adhoc = CAST(((@AdHocBudget - @CalculatedTalentPay)) AS DECIMAL(18,2));
			
			-----------------apply format-----------------
			SELECT @FormattedUplersFees = 
			CASE 
				WHEN ISNULL(@CalculatedUplersFeeInAmount_Adhoc,0) <> 0 
				THEN format(@CalculatedUplersFeeInAmount_Adhoc, N'N', @Culture) 
				ELSE CAST(@CalculatedUplersFeeInAmount_Adhoc AS NVARCHAR(MAX)) 
			END

			DECLARE @FormattedTalentPay NVARCHAR(MAX) = ''
			SELECT @FormattedTalentPay = 
			CASE 
				WHEN ISNULL(@CalculatedTalentPay,0) <> 0 
				THEN format(@CalculatedTalentPay, N'N', @Culture) 
				ELSE CAST(@CalculatedTalentPay AS NVARCHAR(MAX)) 
			END
			
			----------------------------------------------------------

			SET @TalentPay = @CurrencySign + @FormattedTalentPay + @PerMonth
			SET @UplersFeeInAmount = @CurrencySign +  @FormattedUplersFees + @PerMonth;
			
			SET @Budget = @CurrencySign + @FormattedAdhocBudget + @PerMonth
			SET @ClientPay = @Budget
		END
		ELSE
		BEGIN
			IF @BudgetFrom > 0
			BEGIN
				SET @Min_ClientPay = @BudgetFrom
				SET @Min_TalentPay = CAST(((@BudgetFrom * 100)/(100 + @UplersFeesInPer)) AS DECIMAL(18,2))
				SET @Min_UplersFeeInAmount = CAST(((@BudgetFrom - @Min_TalentPay)) AS DECIMAL(18,2));
			END
			IF @BudgetTo > 0
			BEGIN
				SET @Max_ClientPay = @BudgetTo
				SET @Max_TalentPay = CAST(((@BudgetTo * 100)/(100 + @UplersFeesInPer)) AS DECIMAL(18,2))
				SET @Max_UplersFeeInAmount = CAST(((@BudgetTo - @Max_TalentPay)) AS DECIMAL(18,2));
			END

			----------------apply format-------------------------------------------------------------------------------------------------------------------------------------
			SELECT @FormattedMin_UplersFeeInAmount = CASE WHEN ISNULL(@Min_UplersFeeInAmount,0) <> 0 THEN format(@Min_UplersFeeInAmount, N'N', @Culture) ELSE CAST(@Min_UplersFeeInAmount AS NVARCHAR(MAX)) END
			SELECT @FormattedMax_UplersFeeInAmount = CASE WHEN ISNULL(@Max_UplersFeeInAmount,0) <> 0 THEN format(@Max_UplersFeeInAmount, N'N', @Culture) ELSE CAST(@Max_UplersFeeInAmount AS NVARCHAR(MAX)) END
			
			SELECT @FormattedMin_TalentPay = CASE WHEN ISNULL(@Min_TalentPay,0) <> 0 THEN format(@Min_TalentPay, N'N', @Culture) ELSE CAST(@Min_TalentPay AS NVARCHAR(MAX)) END
			SELECT @FormattedMax_TalentPay = CASE WHEN ISNULL(@Max_TalentPay,0) <> 0 THEN format(@Max_TalentPay, N'N', @Culture) ELSE CAST(@Max_TalentPay AS NVARCHAR(MAX)) END

			SELECT @FormattedMin_ClientPay = CASE WHEN ISNULL(@Min_ClientPay,0) <> 0 THEN format(@Min_ClientPay, N'N', @Culture) ELSE CAST(@Min_ClientPay AS NVARCHAR(MAX)) END
			SELECT @FormattedMax_ClientPay = CASE WHEN ISNULL(@Max_ClientPay,0) <> 0 THEN format(@Max_ClientPay, N'N', @Culture) ELSE CAST(@Max_ClientPay AS NVARCHAR(MAX)) END
			------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

			SET @Budget = @CurrencySign + CAST(@FormattedBudgetFrom AS NVARCHAR(MAX)) + ' to ' + @CurrencySign  + CAST(@FormattedBudgetTo AS NVARCHAR(MAX)) + @PerMonth
			SET @UplersFeeInAmount = @CurrencySign +  CAST(@FormattedMin_UplersFeeInAmount AS NVARCHAR(MAX)) + ' to ' + @CurrencySign + CAST(@FormattedMax_UplersFeeInAmount AS NVARCHAR(MAX)) + @PerMonth
			SET @ClientPay = @CurrencySign + CAST(@FormattedMin_ClientPay AS NVARCHAR(MAX)) + ' to ' + @CurrencySign + CAST(@FormattedMax_ClientPay AS NVARCHAR(MAX)) + @PerMonth
			SET @TalentPay = @CurrencySign + CAST(@FormattedMin_TalentPay AS NVARCHAR(MAX)) + ' to ' + @CurrencySign + CAST(@FormattedMax_TalentPay AS NVARCHAR(MAX)) + @PerMonth
		END
	END

	INSERT INTO @CalculateResult(IsTransparentPricing, Budget, UplersFeeInAmount, ClientPay, TalentPay)
	VALUES(@IsTransparentPricing, @Budget, @UplersFeeInAmount, @ClientPay, @TalentPay)

	RETURN
END


GO


