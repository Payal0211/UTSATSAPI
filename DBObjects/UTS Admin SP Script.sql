---------------------Last Live done on 25-6-24

ALTER PROCEDURE [dbo].[sp_UTS_PreviewJobPostUpdate_ClientPortal]
@RoleName NVARCHAR(500) NULL,
@Currency NVARCHAR(50) NULL,
@BudgetFrom DECIMAL(18,2) NULL,
@BudgetTo DECIMAL(18,2) NULL,
@ContractDuration INT NULL,
@EmploymentType NVARCHAR(50) NULL,
@HowSoon NVARCHAR(50) NULL,
@WorkingModeID INT NULL,
@ExperienceYears INT NULL,
@IsFresherAllowed BIT NULL,
@Requirement NVARCHAR(MAX) NULL,
@RolesandResponsibilities NVARCHAR(MAX) NULL,
@Skills NVARCHAR(MAX) NULL,
@AllSkills NVARCHAR(MAX) NULL,
@NoOfTalents INT NULL,
@CompanySize INT NULL,
@CompanyLocation NVARCHAR(MAX) NULL,
@Industry_Type NVARCHAR(MAX) NULL,
@AboutCompany NVARCHAR(MAX) NULL,
@Timezone_Preference_ID BIGINT NULL,
@TimeZone_FromTime NVARCHAR(200) NULL,
@TimeZone_EndTime NVARCHAR(200) NULL,
@TimezoneID BIGINT NULL,
@ContactID BIGINT NULL,
@GUID NVARCHAR(200) NULL,
@IsHiringLimited NVARCHAR(50) = 'Temporary',
@City NVARCHAR(500) NULL,
@State NVARCHAR(500) NULL,
@Country NVARCHAR(500) NULL,
@PostalCode NVARCHAR(500) NULL,
@BudgetType INT = NULL,
@HiringTypePricingId INT = NULL,
@PayrollTypeId INT = NULL,
@PayrollPartnerName NVARCHAR(MAX) = NULL,
@JobDescription NVARCHAR(MAX) = NULL,
@IsConfidentialBudget BIT = NULL,
@GptJdId BIGINT = NULL,
@JobRoleDescription Nvarchar(MAX) = null,
@Whatweoffer  Nvarchar(MAX) = null,
@CountryTracking NVARCHAR(250) = 'India',
@SSOUserId BIGINT = NULL,	
@CompensationOption     Nvarchar(MAX) = null,
@IndustryType           Nvarchar(MAX) = null,
@HasPeopleManagementExp BIT = null,
@Prerequisites			Nvarchar(MAX) = null,
@StringSeparator        NVARCHAR(MAX) = null
AS
BEGIN	

		DECLARE @ActionPerformedBy BIGINT = 0,
		@ActionManagedByClient BIT = 1

		-- Set action performed by user based on SSO login UTS-7223
		SET @ActionPerformedBy =  (SELECT IIF(ISNULL(@SSOUserId,0) = 0, @ContactID, @SSOUserId))
		SET @ActionManagedByClient = (SELECT IIF(ISNULL(@SSOUserId,0) = 0, 1, 0))

		
		UPDATE gen_RoleAndHiringType_ClientPortal 
		SET 
		RoleName = ISNULL(@RoleName, RoleName),
		ExperienceYears = ISNULL(@ExperienceYears, ExperienceYears),
		IsFresherAllowed = ISNULL(@IsFresherAllowed, IsFresherAllowed),
		ContractDuration = ISNULL(IIF(@IsHiringLimited = 'Temporary', @ContractDuration, NULL), ContractDuration),
		NoOfTalents = ISNULL(@NoOfTalents, NoOfTalents),
		WorkingModeID = ISNULL(@WorkingModeID, WorkingModeID),
		CompanyLocation = ISNULL(@CompanyLocation, CompanyLocation),
		IsHiringLimited = ISNULL(@IsHiringLimited, IsHiringLimited),
		City = ISNULL(@City, City),
		State = ISNULL(@State, State),
		Country = ISNULL(@Country, Country),
		PostalCode = ISNULL(@PostalCode, PostalCode),
		GPTJDID = ISNULL(NULLIF(@GptJdId,0), GPTJDID)
		WHERE 
		--ContactId = @ContactID AND 
		GUID = @GUID
		--AND IsActive = 0		
		
		IF(@BudgetFrom > 0 AND ISNULL(@BudgetTo,0) = 0)
		BEGIN
			SET @BudgetTo = 0
		END

		UPDATE gen_SkillAndBudget_ClientPortal SET 
		Currency = ISNULL(@Currency, Currency),
		BudgetFrom = ISNULL(@BudgetFrom, BudgetFrom),
		BudgetTo = ISNULL(@BudgetTo, BudgetTo),
		Skills = ISNULL(@Skills, Skills),
		AllSkills = ISNULL(@AllSkills, AllSkills),
		BudgetType = ISNULL(@BudgetType,BudgetType),
		IsConfidentialBudget = ISNULL(@IsConfidentialBudget,IsConfidentialBudget),
		GPTJDID = ISNULL(NULLIF(@GptJdId,0), GPTJDID)
		WHERE 
		--ContactId = @ContactID AND 
		GUID = @GUID
		--AND IsActive = 0	

		UPDATE gen_JobPost_Employment_Details_ClientPortal SET 
		EmploymentType = ISNULL(@EmploymentType, EmploymentType),
		HowSoon = ISNULL(@HowSoon, HowSoon),
		WorkingModeID = ISNULL(@WorkingModeID, WorkingModeID),
		CompanyLocation = ISNULL(@CompanyLocation, CompanyLocation),
		Timezone_Preference_ID = ISNULL(@Timezone_Preference_ID, Timezone_Preference_ID),
		TimeZone_FromTime = ISNULL(@TimeZone_FromTime, TimeZone_FromTime),
		TimeZone_EndTime = ISNULL(@TimeZone_EndTime, TimeZone_EndTime),
		TimeZoneID = ISNULL(@TimeZoneID, TimeZoneID),
		GPTJDID = ISNULL(NULLIF(@GptJdId,0), GPTJDID)
		WHERE 
		--ContactId = @ContactID AND 
		GUID = @GUID 
		--AND IsActive = 0	

		UPDATE gen_JobPost_Roles_Responsibilities_ClientPortal SET 
		RolesResponsibilities = ISNULL(@RolesandResponsibilities, RolesResponsibilities),
		Requirements = ISNULL(@Requirement, Requirements),
		JobDescription = ISNULL(@JobDescription, JobDescription),
		GPTJDID = ISNULL(NULLIF(@GptJdId,0), GPTJDID),
		JobRoleDescription = Isnull(@JobRoleDescription,JobRoleDescription),
		Whatweoffer = Isnull(@Whatweoffer,Whatweoffer)
		WHERE 
		--ContactId = @ContactID AND 
		GUID = @GUID
		--AND IsActive = 0	

		DECLARE @JDFileName NVARCHAR(250)
		SELECT @JDFileName = JDFileName FROM gen_JobPost_Roles_Responsibilities_ClientPortal WITH(NOLOCK) where GUID = @GUID

		UPDATE gen_JobPost_StandoutToTalents_ClientPortal SET
		Industry_Type = ISNULL(@Industry_Type, Industry_Type),
		CompanySize = ISNULL(@CompanySize, CompanySize),
		about_Company_desc = ISNULL(@AboutCompany, about_Company_desc)
		WHERE 
		--ContactId = @ContactID AND 
		GUID = @GUID 
		--AND IsActive = 0	

		UPDATE gen_TransparentPricingModel_ClientPortal SET 
		HiringTypePricingId = ISNULL(@HiringTypePricingId, HiringTypePricingId),
		SpecIFicMonth = ISNULL(@ContractDuration, SpecIFicMonth),
		PayrollTypeId = ISNULL(@PayrollTypeId,PayrollTypeId),
		PayrollPartnerName = ISNULL(@PayrollPartnerName, PayrollPartnerName)
		WHERE 
		--ContactId = @ContactID AND 
		GUID = @GUID 
		--AND IsActive = 0	


		UPDATE gen_JobPost_VitalInfo_ClientPortal SET 
		CompensationOption = ISNULL(@CompensationOption, CompensationOption),
		IndustryType = ISNULL(@IndustryType, IndustryType),
		HasPeopleManagementExp=ISNULL(@HasPeopleManagementExp, HasPeopleManagementExp),
		Prerequisites = ISNULL(@Prerequisites, Prerequisites),	
		StringSeparator = ISNULL(@StringSeparator,StringSeparator),
		ModifiedbyDateTime = GETDATE()
		WHERE GUID = @GUID


		DECLARE @HR_ID BIGINT = 0
		IF @GUID LIKE '%-%'
			PRINT 'Client portal HR';
		ELSE
		BEGIN
			set @HR_ID = CAST(@GUID AS BIGINT)
		END

		print @GUID

		print @HR_ID

		

		--Update the HR details
		IF EXISTS(SELECT 1 FROM gen_SalesHiringRequest WITH(NOLOCK) WHERE GUID = @GUID OR ID = @HR_ID)
		BEGIN	
			print 'if'
			DECLARE @HRTypeId AS INT = 0,
			        @HR_TypeId AS INT = 0,
					@IsHRTypeDP	AS	BIT = 0,
			        @CompanyID AS BIGINT = 0,
			        @CompanyTypeId AS BIGINT = 0,
			        @ClientID AS BIGINT = 0,
			        @CalculatedUplersFees NVARCHAR(600) = NULL,
			        @UplersFeesPercentage DECIMAL(18,2) = NULL,
			        @HRID BIGINT,
			        @DPPercentage DECIMAL(18,2) = (SELECT TOP 1 [Value] FROM gen_SystemConfiguration WHERE [Key] = 'DP Percentage'),
			        @NRPercentage DECIMAL(18,2) = (SELECT TOP 1 [Value] FROM gen_SystemConfiguration WHERE [Key] = 'TalentCostCalculationPercentage'),
					@IsTransparentPricing BIT = NULL,
					@IsHiringLimitedBit AS BIT = NULL,
					@SpecIFicMonth AS INT = NULL,
					@Cost NVARCHAR(500),
					@RoleId BIGINT = NULL,
					@AdhocBudgetCost DECIMAL(18,2) = NULL,
					@DCSalesUserID as bigint = 0,
					@FormattedBudgetFrom NVARCHAR(MAX) = '',
					@FormattedBudgetTo NVARCHAR(MAX) = ''

			IF(ISNULL(@Currency,'') != '')
					BEGIN
						---Convert into proper formatting for budget
						SELECT @FormattedBudgetFrom = CASE WHEN ISNULL(@BudgetFrom,0) <> 0 THEN format(@BudgetFrom, N'N', Culture) ELSE '' END, 
							   @FormattedBudgetTo = CASE WHEN ISNULL(@BudgetTo,0) <> 0 THEN format(@BudgetTo, N'N', Culture) ELSE '' END
						FROM prg_CurrencyExchangeRate WITH(NOLOCK) WHERE CurrencyCode = @Currency
				    END
		
			print CAST(@IsHiringLimited AS NVARCHAR)
			
			IF(@IsHiringLimited = 'Temporary' OR @EmploymentType = 'Temporary' OR @EmploymentType = 'Part Time')
			BEGIN
				SET @IsHiringLimitedBit = 1
			END

			print CAST(@IsHiringLimitedBit AS NVARCHAR)

			SELECT 
			@HRID = ID , 
			@ClientID = ContactID,
			@DCSalesUserID =  SalesUserID,
			@HiringTypePricingId = ISNULL(@HiringTypePricingId,HiringTypePricingId)
			FROM gen_SalesHiringRequest WITH(NOLOCK) WHERE GUID = @GUID OR ID = @HR_ID

			SET @HR_ID = @HRID

			SELECT @CompanyID = CompanyID from gen_Contact WITH(NOLOCK) WHERE ID = @ClientID

			SELECT @CompanyTypeId = ISNULL(PRT.CompanyTypeID,0) FROM gen_SalesHiringRequest HR WITH(NOLOCK) 
			LEFT JOIN prg_HiringRequest_Type PRT ON PRT.ID = HR.HrTypeId
			WHERE HR.ID = @HR_ID

			IF (@RoleName <> NULL OR @RoleName <> '')
			BEGIN
				SET @RoleID  = (SELECT TOP 1 ID FROM prg_TalentRoles with(NoLock) WHERE TalentRole = @RoleName)
			END

			IF ((@RoleName <> NULL OR @RoleName <> '') AND ISNULL(@RoleID,0) = 0)
			BEGIN		
			
				PRINT @RoleName
				SELECT  
				TOP 1   @RoleID = FT_TBL.ID  
				FROM    prg_TalentRoles AS FT_TBL with(nolock)  
						INNER JOIN FREETEXTTABLE(prg_TalentRoles,TalentRole,@RoleName,LANGUAGE N'English') AS KEY_TBL  
						ON FT_TBL.ID = KEY_TBL.[KEY] ORDER BY RANK desc

				IF(ISNULL(@RoleID,0) = 0)
					BEGIN	
					
						--As per Bhuvan sir's instruction in SP 24 planning call 27-11-23

						INSERT INTO prg_TalentRoles
						(
							TalentRole,
							IsActive,
							IsAdhoc,
							CreatedbyDatetime,
							ModIFyByID,
							FrontIconImage
						)
						VALUES
						(
							@RoleName,
							1,
							1,
							GETDATE(),
							@ActionPerformedBy,
							'default-role-icon.png'
						)

						SET @RoleID = @@IDENTITY 
				  END
			END

			-- No bar for right candidate
			IF(ISNULL(@BudgetType,2) = 3 AND (ISNULL(@BudgetFrom,0) = 0 AND ISNULL(@BudgetTo,0) = 0))
			BEGIN
				--SET @Cost = 'Open for any budget'
				SET @Cost = ''
				SET @BudgetFrom = 0 
				SET	@BudgetTo = 0 
				SET	@AdhocBudgetCost = 0
			END

			-- Budget Range 
			IF(ISNULL(@BudgetFrom,0) >  0 OR ISNULL(@BudgetTo,0) > 0)
			BEGIN
				SET @Cost = (
								SELECT IIF
										(
											ISNULL(@BudgetFrom,0) = 0, 
											@FormattedBudgetFrom,
											CAST(CurrencySign AS NVARCHAR) + '' + @FormattedBudgetFrom
										) 
										+ ' to ' +  CAST(CurrencySign AS NVARCHAR) + '' + @FormattedBudgetTo + ' ' + @Currency + ' / Month'  FROM prg_CurrencyExchangeRate with(NoLock) WHERE CurrencyCode = @Currency
							)
							SET	@AdhocBudgetCost = 0
			END
			--Adhoc Budget
			IF(ISNULL(@BudgetFrom,0) >  0 AND ISNULL(@BudgetTo,0) = 0)
			BEGIN
				SET @Cost = (
								SELECT IIF
										(
											ISNULL(@BudgetFrom,0) = 0, 
											@FormattedBudgetFrom,
											+ 'Upto ' + CAST(CurrencySign AS NVARCHAR) + '' + @FormattedBudgetFrom + ' ' + @Currency + ' / Month' 										
										) 
										FROM prg_CurrencyExchangeRate with(NoLock) WHERE CurrencyCode = @Currency
							)
				SET @AdhocBudgetCost = @BudgetFrom
				SET @BudgetFrom = 0 
				SET	@BudgetTo = 0 
			END
	

			-- IF pay per hire
			IF(@CompanyTypeId = 1)
			BEGIN
				--Pick the HR_TypeID from prg_HiringType_Pricing to decide HR is DP or contractual
				SELECT TOP 1 
					@HR_TypeId = HRTypeID, 
					@UplersFeesPercentage = CASE WHEN @HiringTypePricingId = 3 AND (LOWER(@CountryTracking) = LOWER('India') OR LOWER(@CountryTracking) = LOWER('IN'))
											THEN 7.5 ELSE PricingPercent 
											END
					FROM prg_HiringType_Pricing WHERE ID = @HiringTypePricingId

				SET @HRTypeId = 1

				--IF Hr_TypeId is 1 that means HR is DP Else Contractual
				SET @IsHRTypeDP = case when @HR_TypeId = 1 then 1 else 0 end

				SET @IsTransparentPricing = 1		
			
				IF(ISNULL(@BudgetFrom,0) > 0 AND  ISNULL(@BudgetTo,0) > 0)
				BEGIN
					SELECT TOP 1 @CalculatedUplersFees = UplersFeeInAmount FROM dbo.PayPerHire_Calculation(1, 0, @BudgetFrom, @BudgetTo, @UplersFeesPercentage, @IsHRTypeDP);
				END

				IF(ISNULL(@AdhocBudgetCost,0) > 0)
				BEGIN
					SELECT TOP 1 @CalculatedUplersFees = UplersFeeInAmount FROM dbo.PayPerHire_Calculation(1, @AdhocBudgetCost, 0, 0, @UplersFeesPercentage, @IsHRTypeDP);
				END

				IF(@HR_TypeId = 1)
				BEGIN
					SET @DPPercentage = @UplersFeesPercentage
				END

				IF(@HR_TypeId = 2)
				BEGIN
					SET @NRPercentage = @UplersFeesPercentage
				END

			END		

			-- IF pay per credit
			--IF(@CompanyTypeId = 2)
			--BEGIN
				--SET @HR_TypeId = NULL;
				--SET	@IsHRTypeDP = 1;
				--SET	@HRTypeId = 4;
				--SET	@DPPercentage = NULL;
				--SET	@NRPercentage = NULL;
				--SET @IsTransparentPricing = NULL;
			--END

			IF Exists(select 1 from gen_TransparentPricingModel_ClientPortal WITH(NOLOCK) Where ContactID = @ClientID and GUID = @GUID)
			BEGIN
					SELECT @HiringTypePricingId = ISNULL(HiringTypePricingId,0),
						   @SpecIFicMonth = ISNULL(SpecIFicMonth,0),
						   @PayrollPartnerName = ISNULL(PayrollPartnerName,''),
						   @PayrollTypeId = ISNULL(PayrollTypeId,0)
					FROM   gen_TransparentPricingModel_ClientPortal WITH(NOLOCK) 
					Where  ContactID = @ClientID and GUID = @GUID 
			END

			

			--update data in gen_SalesHiringRequest
			UPDATE gen_SalesHiringRequest
			SET
			--ContactId = ISNULL(@ClientID, ContactId),
			RequestForTalent = ISNULL(@RoleName, RequestForTalent),
			--NoofTalents = ISNULL(@NoOfTalents, NoofTalents),
			IsHiringLimited = ISNULL(@IsHiringLimitedBit, IsHiringLimited),
			Availability = ISNULL(@EmploymentType, Availability),
			TalentCostCalcPercentage = ISNULL(@NRPercentage, TalentCostCalcPercentage),
			DPPercentage = ISNULL(@DPPercentage, DPPercentage),
			IsHRTypeDP = ISNULL(@IsHRTypeDP, IsHRTypeDP),
			LastModIFiedByID = ISNULL(@ActionPerformedBy, CreatedByID),
			LastModIFiedDatetime = GETDATE(),
			about_Company_desc = ISNULL(@AboutCompany, about_Company_desc),
			JDFilename = ISNULL(@JDFileName, JDFilename),
			--JDURL = ISNULL(@JDURL, JDURL),
			--IsCreatedByClient = 1,
			HiringTypePricingId = ISNULL(@HiringTypePricingId, HiringTypePricingId),
			PayrollTypeId = ISNULL(@PayrollTypeId, PayrollPartnerName),
			HR_TypeId = ISNULL(@HR_TypeId, HR_TypeId),
			PayrollPartnerName = ISNULL(@PayrollPartnerName, PayrollPartnerName),
			ClientJobModifyByID = @ContactID,
			ClientJobModifyByDatetime = GETDATE()
			--HRTypeId = ISNULL(@HRTypeId, HRTypeId)
			WHERE GUID = @GUID  OR ID = @HR_ID

			

			--update data in gen_SalesHiringRequest_Details
			UPDATE gen_SalesHiringRequest_Details
		    SET 
			InterviewerYearofExperience = ISNULL(@ExperienceYears, InterviewerYearofExperience),
			IsFresherAllowed = ISNULL(@IsFresherAllowed, IsFresherAllowed),
			--NoofEmployee = ISNULL(@NoOfTalents, NoofEmployee),
			Currency = ISNULL(@Currency, Currency),
			BudgetFrom = ISNULL(@BudgetFrom, BudgetFrom),
			BudgetTo = ISNULL(@BudgetTo, BudgetTo),
			Adhoc_BudgetCost = ISNULL(@AdhocBudgetCost, Adhoc_BudgetCost),
			HowSoon = ISNULL(@HowSoon, HowSoon), 
			TimeZone_FromTime = ISNULL(@TimeZone_FromTime, TimeZone_FromTime),
			TimeZone_EndTime = ISNULL(@TimeZone_EndTime, TimeZone_EndTime),	
			SpecIFicMonth = IIF(@IsHiringLimitedBit IS NOT NULL, ISNULL(@SpecIFicMonth, IIF(@IsHiringLimitedBit = 1, @ContractDuration, NULL)), SpecIFicMonth),
			RolesResponsibilities = ISNULL(@RolesandResponsibilities, RolesResponsibilities),
			Requirement = ISNULL(@Requirement, Requirement),
			Role_ID = ISNULL(@RoleID, Role_ID),
			YearOfExp = ISNULL(@ExperienceYears, YearOfExp),
			Cost = ISNULL(@Cost, Cost),
			LastModIFiedByID = ISNULL(@ActionPerformedBy, LastModIFiedByID),
			LastModIFiedDatetime = Getdate(),
			DurationType = IIF(@IsHiringLimitedBit IS NOT NULL, IIF(@IsHiringLimitedBit = 1, 'Short Term', 'Long Term') , DurationType),
			Timezone_ID = ISNULL(@Timezone_Preference_ID, Timezone_ID),
			CalculatedUplersfees = ISNULL(@CalculatedUplersFees, CalculatedUplersfees),
			JobDescription = ISNULL(@JobDescription, JobDescription),
			IsConfidentialBudget = ISNULL(@IsConfidentialBudget, IsConfidentialBudget),
			JobRoleDescription = Isnull(@JobRoleDescription,JobRoleDescription),
			Whatweoffer = Isnull(@Whatweoffer,Whatweoffer)
			WHERE HiringRequest_ID = @HRID
		
			DECLARE @WorkingMode NVARCHAR(100)
			SELECT @WorkingMode = ModeOfWorking FROM prg_ModeOfWorking with(NoLock) WHERE ID = @WorkingModeID
			
			--update data in gen_Direct_Placement
			UPDATE  gen_Direct_Placement
			SET
			ModeOfWork = ISNULL(@WorkingMode, ModeOfWork),
			City = ISNULL(@City, City),
			State = ISNULL(@State, State),
			Country = ISNULL(@Country, Country),
			PostalCode = ISNULL(@PostalCode, PostalCode),
			ModIFiedById = ISNULL(@ActionPerformedBy, ModIFiedById),
			ModIFiedByDateTime = GETDATE()
			WHERE HiringRequest_Id = @HRID

			--IF(@AllSkills IS NOT NULL)
			--BEGIN
			--	DELETE FROM gen_SalesHiringRequest_SkillDetails WHERE HiringRequest_ID = @HRID AND Proficiency = 'Basic' 

			--	DECLARE @GoodTohaveSkillsTable TABLE
			--	(			
			--		SkillName NVARCHAR(MAX)
			--	)
			--	INSERT INTO @GoodTohaveSkillsTable
			--	select rtrim(ltrim(val)) AS Skill from  f_split(@GoodToHaveSkills,',')

			--END

			IF(@Skills IS NULL OR @AllSkills IS NULL)
			BEGIN
				SELECT @Skills = Skills, @AllSkills = AllSKills FROM gen_SkillAndBudget_ClientPortal WHERE Guid = @Guid
			END

			IF(@Skills IS NOT NULL OR @AllSkills IS NOT NULL)
			BEGIN
				EXEC Sproc_DumpJDDetailsintoTempTable_ClientPortal 
					@ContactId      = @ClientID,              
					@TimeZone       = NULL,              
					@JDRole         = NULL,               
					@JDDescription  = NULL,              
					@JDSkills       = @Skills,      
					@HowSoon        = @HowSoon,              
					@HowLong        = NULL,              
					@YearsOfExp     = @ExperienceYears,              
					@Qty			= @NoOfTalents,              
					@JDTools        = NULL,              
					@JDFileName     = NULL,              
					@JDDetails      = NULL,        
					@YesWithIsNext  = 0,              
					@InsertedID     = 0,  
					@JDAllSkills    = @AllSkills,  
					@IsAdmin        = 0,  
					@RolesResponsibilities  = @RolesandResponsibilities,  
					@Requirement    = @Requirement,
					@HR_ID = @HRID
			 END

			 --added by himani for Update calculation
			EXEC SP_PayPerHire_Calculation_Update @HRID = @HR_ID
			-- --added by himani for maintain history table
			--EXEC SPROC_Gen_SalesHiringRequest_History @HRID = @HR_ID, @AppActionDoneBy = 4
			----added by himani for maintain history table before save new changes
			--EXEC SPROC_Gen_SalesHiringRequest_Details_History @HRID = @HR_ID, @AppActionDoneBy = 4
			Declare @HiringRequestHisotoryID as bigint = 0
			Declare @HRDetailHisotoryID as bigint = 0
			Insert into gen_SalesHiringRequest_History(HiringRequest_ID,ContactID,RequestForTalent,LastModifiedByID,LastModifiedDatetime
											,Availability,IsCreatedByClient,IsActive,AppActionDoneBy,HistoryDate,HiringTypePricingId,payrollTypeID)
			Values(@HR_ID,@ContactID,@RoleName,@ContactID,Getdate(),@EmploymentType,1,1,4,Getdate(),@HiringTypePricingId,@PayrollTypeId)

			SET @HiringRequestHisotoryID = @@IDENTITY

			Insert into gen_SalesHiringRequest_Details_History(HiringRequest_ID,Role_ID,Duration,DurationType,Cost,SpecificMonth,YearOfExp,IsFresherAllowed,HowSoon,Timezone_ID
							,LastModifiedByID,LastModifiedDatetime,Timezone_Preference_ID,TimeZone_FromTime,TimeZone_EndTime,BudgetFrom,BudgetTo,Currency,CalculatedUplersfees,JobDescription,AppActionDoneBy,HistoryDate)
			Values(@HR_ID,@RoleID,@ContractDuration,IIF(@IsHiringLimitedBit = 1, 'Short Term', 'Long Term'),@Cost,@SpecIFicMonth,@ExperienceYears,@IsFresherAllowed,@HowSoon,
				@TimezoneID,@ContactID,GetDate(),@Timezone_Preference_ID,@TimeZone_FromTime,@TimeZone_EndTime,@BudgetFrom,@BudgetTo,@Currency,@CalculatedUplersFees,@JobDescription,4,getdate())

			SET @HRDetailHisotoryID = @@IDENTITY

		   EXEC sproc_HiringRequest_History_Insert @Action = 'Update_HR', @HiringRequest_ID = @HRID,
				@Talent_ID = 0, @Created_From = 0,@CreatedById = @ActionPerformedBy,@ContactTalentPriority_ID = 0,@InterviewMaster_ID = 0,
				@HR_AcceptedDateTime = '', @OnBoard_ID = 0,@IsManagedByClient = @ActionManagedByClient ,@IsManagedByTalent = 0, @SalesUserID = @DCSalesUserID,
				@OldSalesUserID = 0, @AppActionDoneBy = 4,@SalesHistoryID = @HiringRequestHisotoryID,@SalesDetailHistoryID = @HRDetailHisotoryID 	
	  
		END
		ELSE
		BEGIN
			Print 'else'
		END
END
GO

ALTER PROCEDURE [dbo].[Sproc_UTSAdmin_EditHrByATS] 
@HrId						BIGINT			= NULL,
@ContactId 					BIGINT			= NULL,
@Availability				NVARCHAR(200)	= NULL,
@ContractDuration			INT				= NULL,
@Currency					NVARCHAR(50)	= NULL,
@AdhocBudgetCost			DECIMAL(18,2)	= NULL,
@MinimumBudget				DECIMAL(18,2)	= NULL,
@MaximumBudget				DECIMAL(18,2)	= NULL,
@IsConfidentialBudget		BIT				= NULL,
@ModeOfWorkingId			INT				= NULL,
@City						NVARCHAR(200)	= NULL,
@Country					NVARCHAR(200)	= NULL,
@JDFilename					NVARCHAR(400)	= NULL,
@JDURL						NVARCHAR(MAX)	= NULL,
@YearOfExp					DECIMAL(18,2)	= NULL,
@NoofTalents				INT				= NULL,
@TimezoneId					INT				= NULL,
@TimeZoneFromTime			NVARCHAR(200)	= NULL,
@TimeZoneEndTime			NVARCHAR(200)	= NULL,
@HowSoon					NVARCHAR(400)	= NULL,
@PartialEngagementTypeID	INT				= NULL,
@NoofHoursworking			INT				= NULL,
@DurationType				NVARCHAR(50)	= NULL,
@HrTitle					NVARCHAR(200)	= NULL,
@RoleAndResponsibilites		NVARCHAR(MAX)	= NULL,
@Requirements				NVARCHAR(MAX)	= NULL,
@JobDescription				NVARCHAR(MAX)	= NULL,
@MustHaveSkills				NVARCHAR(MAX)	= NULL,
@GoodToHaveSkills			NVARCHAR(MAX)	= NULL,
@IsHrfocused				BIT				= NULL,
@IsHRTypeDP					BIT				= NULL,
@DpPercentage				DECIMAL(18,2)	= NULL,
@NRMargin					DECIMAL(18,2)	= NULL,
@IsTransparentPricing		BIT				= NULL,
@HrTypePricingId			INT				= NULL,
@PayrollTypeId				INT				= NULL,
@PayrollPartnerName			NVARCHAR(300)	= NULL,
@IsVettedProfile			BIT				= NULL,
@IsHiringLimited			BIT				= NULL,
@LastModifiedById			INT				= NULL
AS
BEGIN

	IF EXISTS (SELECT TOP 1 ID FROM gen_SalesHiringRequest WHERE ID = @HrId)
	BEGIN
		DECLARE @Cost NVARCHAR(500) = NULL,
					@RoleID BIGINT = NULL

			--Range budget
			IF(ISNULL(@MinimumBudget,0) >  0 AND ISNULL(@MaximumBudget,0) > 0)
			BEGIN
				SET @Cost = (
								SELECT IIF
										(
											ISNULL(@MinimumBudget,0) = 0, 
											CAST(@MinimumBudget AS NVARCHAR),
											CAST(CurrencySign AS NVARCHAR) + ' ' + CAST(@MinimumBudget AS NVARCHAR) 
										) 
										+ ' to ' +  CAST(CurrencySign AS NVARCHAR) + ' ' + CAST(@MaximumBudget AS NVARCHAR) + ' ' + @Currency FROM prg_CurrencyExchangeRate with(NoLock) WHERE CurrencyCode = @Currency
							)
							SET	@AdhocBudgetCost = 0
			END
			--Adhoc Budget
			IF(ISNULL(@AdhocBudgetCost,0) >  0)
			BEGIN
				SET @Cost = (
								SELECT IIF
										(
											ISNULL(@AdhocBudgetCost,0) = 0, 
											CAST(@AdhocBudgetCost AS NVARCHAR),
											+ 'Upto ' + CAST(CurrencySign AS NVARCHAR) + ' ' + CAST(@AdhocBudgetCost AS NVARCHAR) + ' ' + @Currency + ' / Month' 										
										) 
										FROM prg_CurrencyExchangeRate with(NoLock) WHERE CurrencyCode = @Currency
							)
				SET @MinimumBudget = 0 
				SET	@MaximumBudget = 0 
			END

			--RoleID based on HR Title
			IF (@HrTitle <> NULL OR @HrTitle <> '')
			BEGIN
				SET @RoleID  = (SELECT TOP 1 ID FROM prg_TalentRoles with(NoLock) WHERE TalentRole = @HrTitle)
			END
			--If not exist then need to create new role in Master 
			IF ((@HrTitle <> NULL OR @HrTitle <> '') AND ISNULL(@RoleID,0) = 0)
			BEGIN		
			
				PRINT @HrTitle
				SELECT  
				TOP 1   @RoleID = FT_TBL.ID  
				FROM    prg_TalentRoles AS FT_TBL with(nolock)  
						INNER JOIN FREETEXTTABLE(prg_TalentRoles,TalentRole,@HrTitle,LANGUAGE N'English') AS KEY_TBL  
						ON FT_TBL.ID = KEY_TBL.[KEY] ORDER BY RANK desc

				IF(ISNULL(@RoleID,0) = 0)
					BEGIN	
					
						--As per Bhuvan sir's instruction in SP 24 planning call 27-11-23

						INSERT INTO prg_TalentRoles
						(
							TalentRole,
							IsActive,
							IsAdhoc,
							CreatedbyDatetime,
							ModIFyByID,
							FrontIconImage
						)
						VALUES
						(
							@HrTitle,
							1,
							1,
							GETDATE(),
							@LastModifiedById,
							'default-role-icon.png'
						)

						SET @RoleID = @@IDENTITY 
				  END
			END


			--Added By Jimit 08-05-2024  
			Declare @HiringRequestHisotoryID as bigint = 0
			DEclare @HRDetailID as bigint = 0
			Declare @HRDetailHisotoryID as bigint = 0

			select @HRDetailID = ID from gen_SalesHiringRequest_Details WITH(NOLOCK) WHERE HiringRequest_ID = @HrId

			Insert into gen_SalesHiringRequest_History(HiringRequest_ID,ContactID,LastModifiedByID,LastModifiedDatetime
											,Availability,IsCreatedByClient,IsActive,AppActionDoneBy,JDFilename,JDURL,NoofTalents,NoofHoursworking,
											DPPercentage,IsHRTypeDP,IsTransparentPricing,PayrollTypeId,PayrollPartnerName,PartialEngagementType_ID,
											IsVettedProfile,IsHiringLimited,HR_TypeId,HistoryDate)
			--Values(@HrId,@ContactID,@ContactID,Getdate(),@Availability,1,1,3,@JDFilename,@JddumpId,@JDURL,@NoofTalents,@NoofHoursworking,@Discovery_Call,
			--@DpPercentage,@IsHRTypeDP,@IsTransparentPricing,@PayrollTypeId,@PayrollPartnerName,@PartialEngagementTypeID,@IsVettedProfile,@IsHiringLimited,
			--IIF(ISNULL(@IsHRTypeDP, 0) = 1, 1, 2),@IsDirectHR,@Bqlink,@DealID,@ChildCompanyName)
			SELECT @HrId,
			       ContactID = Case when ContactID <> @ContactID then @ContactID else null end,
			       LastModifiedByID = @ContactID,
				   Getdate(),
				   HRAvailability = Case when Availability <> @Availability then @Availability else null end,
				   1,1,2,
				   JDfile = Case when JDFilename <> @JDFilename then @JDFilename else null end,
				   JDURL = Case when JDURL <> @JDURL then @JDURL else null end,
				   NoofTalents = Case when NoofTalents <> @NoofTalents then @NoofTalents else null end,
				   NoofHoursworking = Case when NoofHoursworking <> @NoofHoursworking then @NoofHoursworking else null end,
				   DPPercentage = Case when DPPercentage <> @DpPercentage then @DpPercentage else null end,
				   IsHRTypeDP = Case when IsHRTypeDP <> @IsHRTypeDP then @IsHRTypeDP else null end,				  
				   IsTransparentPricing = Case when IsTransparentPricing <> @IsTransparentPricing then @IsTransparentPricing else null end,
				   PayrollTypeId = Case when PayrollTypeId <> @PayrollTypeId then @PayrollTypeId else null end,
				   PayrollPartnerName = Case when PayrollPartnerName <> @PayrollPartnerName then @PayrollPartnerName else null end,
				   PartialEngagementType_ID = Case when PartialEngagementType_ID <> @PartialEngagementTypeID then @PartialEngagementTypeID else null end,
				   IsVettedProfile = Case when IsVettedProfile <> @IsVettedProfile then @IsVettedProfile else null end,
				   IsHiringLimited = Case when IsHiringLimited <> @IsHiringLimited then @IsHiringLimited else null end,
				   HR_TypeId = Case when HR_TypeId <> IIF(ISNULL(@IsHRTypeDP, 0) = 1, 1, 2) then IIF(ISNULL(@IsHRTypeDP, 0) = 1, 1, 2) else null end,
				   Getdate()
			FROM   gen_SalesHiringRequest WITH(NOLOCK) WHERE ID = @HrId 

			SET @HiringRequestHisotoryID = @@IDENTITY

			Insert into gen_SalesHiringRequest_Details_History(HiringRequest_ID,Duration,DurationType,Cost,YearOfExp,HowSoon,Timezone_ID
							,LastModifiedByID,LastModifiedDatetime,TimeZone_FromTime,TimeZone_EndTime,BudgetFrom,BudgetTo,Currency,AppActionDoneBy,PartialEngagementType_ID,
							Adhoc_BudgetCost,IsConfidentialBudget,HistoryDate)
			--Values(@HrId,@ContractDuration,@DurationType,@Cost,@YearOfExp,@HowSoon,
			--	@TimezoneID,@ContactID,GetDate(),@TimeZoneFromTime,@TimeZoneEndTime,@MinimumBudget,@MaximumBudget,@Currency,3,@PartialEngagementTypeID,
			--	@AdhocBudgetCost,@IsConfidentialBudget,@OverlapingHours, @IsFresherAllowed)
			SELECT @HrId,
				   Duration = Case when Duration <> @ContractDuration then @ContractDuration else null end,
				   DurationType = Case when DurationType <> @DurationType then @DurationType else null end,
				   Cost = Case when Cost <> @Cost then @Cost else null end,
				   YearOfExp = Case when YearOfExp <> @YearOfExp then @YearOfExp else null end,
				   HowSoon = Case when HowSoon <> @HowSoon then @HowSoon else null end,
				   Timezone_ID = Case when Timezone_ID <> @TimezoneID then @TimezoneID else null end,
				   @ContactID,GetDate(),
				   TimeZone_FromTime = Case when TimeZone_FromTime <> @TimeZoneFromTime then @TimeZoneFromTime else null end,
				   TimeZone_EndTime = Case when TimeZone_EndTime <> @TimeZoneEndTime then @TimeZoneEndTime else null end,
				   BudgetFrom = Case when BudgetFrom <> @MinimumBudget then @MinimumBudget else null end,
				   BudgetTo = Case when BudgetTo <> @MaximumBudget then @MaximumBudget else null end,
				   Currency = Case when Currency <> @Currency then @Currency else null end,
				   2,
				   PartialEngagementType_ID = Case when PartialEngagementType_ID <> @PartialEngagementTypeID then @PartialEngagementTypeID else null end,
				   Adhoc_BudgetCost = Case when Adhoc_BudgetCost <> @AdhocBudgetCost then @AdhocBudgetCost else null end,
				   IsConfidentialBudget = Case when IsConfidentialBudget <> @IsConfidentialBudget then @IsConfidentialBudget else null end,
				   getdate()
			FROM gen_SalesHiringRequest_Details WITH(NOLOCK) WHERE ID = @HRDetailID 

				SET @HRDetailHisotoryID = @@IDENTITY

			END


			---


			--update in gen_SalesHiringRequest
			UPDATE gen_SalesHiringRequest
			SET
			RequestForTalent = ISNULL(@HrTitle, RequestForTalent),
			Availability = ISNULL(@Availability, Availability),
			JDFilename = ISNULL(@JDFileName, JDFilename),
			JDURL = ISNULL(@JDURL, JDURL),
			NoofTalents = ISNULL(@NoOfTalents, NoofTalents),
			PartialEngagementType_ID = ISNULL(@PartialEngagementTypeId, PartialEngagementType_ID),
			NoofHoursworking = ISNULL(@NoofHoursworking, NoofHoursworking),
			IsHrtypeDp = ISNULL(@IsHrtypeDp, IsHrtypeDp),
			DpPercentage = ISNULL(@DPPercentage, DPPercentage),
			TalentCostCalcPercentage = ISNULL(@NRMargin, TalentCostCalcPercentage),
			IsTransparentPricing = ISNULL(@IsTransparentPricing, IsTransparentPricing),
			HiringTypePricingId = ISNULL(@HrTypePricingId, HiringTypePricingId),
			PayrollTypeId = ISNULL(@PayrollTypeId, PayrollTypeId),
			PayrollPartnerName = ISNULL(@PayrollPartnerName, PayrollPartnerName),
			IsVettedProfile = ISNULL(@IsVettedProfile, IsVettedProfile),
			IsHiringLimited = ISNULL(@IsHiringLimited, IsHiringLimited),
			LastModifiedByID = ISNULL(@LastModifiedById, LastModifiedByID),
			LastModifiedDatetime = GETDATE()
			WHERE ID = @HrId

			--Update data in gen_SalesHiringRequest_Details
			UPDATE gen_SalesHiringRequest_Details
		    SET 
			SpecIFicMonth = ISNULL(@ContractDuration, SpecIFicMonth),
			Currency = ISNULL(@Currency, Currency),
			BudgetFrom = ISNULL(@MinimumBudget, BudgetFrom),
			BudgetTo = ISNULL(@MaximumBudget, BudgetTo),
			Adhoc_BudgetCost = ISNULL(@AdhocBudgetCost, Adhoc_BudgetCost),
			Cost = ISNULL(@Cost, Cost),
			IsConfidentialBudget = ISNULL(@IsConfidentialBudget, IsConfidentialBudget),
			YearOfExp = ISNULL(@YearOfExp, YearOfExp),
			Timezone_ID = ISNULL(@TimeZoneID, Timezone_ID),
			TimeZone_FromTime = ISNULL(@TimeZoneFromTime, TimeZone_FromTime),
			TimeZone_EndTime = ISNULL(@TimeZoneEndTime, TimeZone_EndTime),
			HowSoon = ISNULL(@HowSoon, HowSoon),
			DurationType = ISNULL(@DurationType, DurationType),
			Role_ID = ISNULL(@RoleID, @RoleID),
			RolesResponsibilities = ISNULL(@RoleAndResponsibilites, RolesResponsibilities),
			Requirement = ISNULL(@Requirements, Requirement),
			JobDescription = ISNULL(@JobDescription, JobDescription),
			IsHrfocused = ISNULL(@IsHrfocused, IsHrfocused),
			LastModifiedByID = ISNULL(@LastModifiedById, LastModifiedByID),
			LastModifiedDatetime = GETDATE()
			WHERE HiringRequest_ID = @HrId
			
			DECLARE @WorkingMode NVARCHAR(100)
			SELECT @WorkingMode = ModeOfWorking FROM prg_ModeOfWorking WITH(NOLOCK) WHERE ID = @ModeOfWorkingId

			DECLARE @CountryText NVARCHAR(200)

			IF @Country <> '' AND @Country IS NOT NULL
			BEGIN
				IF EXISTS (SELECT TOP 1 ID from prg_CountryRegion WHERE Country = @Country)
				BEGIN
					SET @CountryText = (SELECT TOP 1 CAST(ID AS NVARCHAR(200)) FROM prg_CountryRegion WHERE Country like '%' + @Country + '%')
				END
				ELSE
				BEGIN
					SET @CountryText = '0'
				END
			END

			--update data in gen_Direct_Placement
			UPDATE  gen_Direct_Placement
			SET
			ModeOfWork = ISNULL(@WorkingMode, ModeOfWork),
			City = ISNULL(@City, City),
			Country = ISNULL(@CountryText, Country),
			ModIFiedById = ISNULL(@LastModifiedById, ModIFiedById),
			ModIFiedByDateTime = GETDATE()
			WHERE HiringRequest_Id = @HrId

			--update @MustHaveSkills & @GoodToHaveSkills
			IF(@MustHaveSkills IS NOT NULL OR @GoodToHaveSkills IS NOT NULL)
			BEGIN

				DECLARE @JDDUMPID BIGINT = 0;
				SET @JDDUMPID = (SELECT ISNULL(JDDump_ID, 0) FROM gen_SalesHiringRequest WHERE ID = @HrId)

				EXEC Sproc_DumpJDDetailsintoTempTable_ClientPortal 
						@ContactId      = @ContactId,              
						@TimeZone       = NULL,              
						@JDRole         = NULL,               
						@JDDescription  = NULL,              
						@JDSkills       = @MustHaveSkills,      
						@HowSoon        = @HowSoon,              
						@HowLong        = NULL,              
						@YearsOfExp     = @YearOfExp,              
						@Qty			= @NoOfTalents,              
						@JDTools        = NULL,              
						@JDFileName     = @JDFileName,              
						@JDDetails      = NULL,        
						@YesWithIsNext  = 0,              
						@InsertedID     = @JDDUMPID,  
						@JDAllSkills    = @GoodToHaveSkills,  
						@IsAdmin        = 1,  
						@RolesResponsibilities  = @RoleAndResponsibilites,  
						@Requirement    = @Requirements,
						@HR_ID = @HrId

				--EXEC [Sproc_DumpJDDetailsintoTempTable]              
				--		@ContactId					= @ContactId,              
				--		@Company					= NULL,              
				--		@TimeZone					= NULL,              
				--		@JDRole						= NULL,               
				--		@JDDescription				= NULL,              
				--		@JDSkills					= @MustHaveSkills,              
				--		@HowSoon					= @HowSoon,              
				--		@HowLong					= NULL,              
				--		@YearsOfExp					= @YearOfExp,              
				--		@Qty						= @NoOfTalents,              
				--		@JDTools					= NULL,              
				--		@JDFileName					= @JDFilename,              
				--		@JDDetails					= NULL,        
				--		@YesWithIsNext				= 0,              
				--		@InsertedID					= @JDDUMPID,  
				--		@JDAllSkills				= @GoodToHaveSkills ,  
				--		@IsAdmin					= 1,  
				--		@RolesResponsibilities		= @RoleAndResponsibilites,  
				--		@Requirement			    = @Requirements
			 END



			 --Update calculation
			EXEC SP_PayPerHire_Calculation_Update @HRID = @HrId
			----Insert history maintain
			--EXEC SPROC_Gen_SalesHiringRequest_History @HRID = @HrId, @AppActionDoneBy = 2
			--EXEC SPROC_Gen_SalesHiringRequest_Details_History @HRID = @HrId, @AppActionDoneBy = 2

			--Insert history action for Update_HR from ATS
		   EXEC sproc_HiringRequest_History_Insert @Action = 'Update_HR', @HiringRequest_ID = @HrId,
				@Talent_ID = 0, @Created_From = 0, @CreatedById = @LastModifiedById,
				@ContactTalentPriority_ID = 0, @InterviewMaster_ID = 0,
				@HR_AcceptedDateTime = '', @OnBoard_ID = 0, @SalesUserID = 0,
				@OldSalesUserID = 0, @AppActionDoneBy = 2,@SalesHistoryID = @HiringRequestHisotoryID,@SalesDetailHistoryID = @HRDetailHisotoryID 

	END



GO

ALTER PROCEDURE [dbo].[sp_UTS_get_HRClientDetails]
@HR_ID bigint
AS
BEGIN
		-------------------------------------New Status Changes-----------------------------------
		CREATE TABLE #TempTable_HrStatus (
			HRID BIGINT,
			HR_Number VARCHAR(100),
			HRStatus  VARCHAR(100),
			HRStatusCode INT
		);
		INSERT INTO #TempTable_HrStatus(HRID, HR_Number, HRStatus, HRStatusCode)
		EXEC Sproc_UTS_Get_HRStatus @HRID = @HR_ID
		-------------------------------------------------------------------------------
		
		select	top 1 U1.FullName,POC.ContactID,U1.EmailID,H.ID AS HiringID into #Temp_POC 
							from	usr_User U1 with(nolock) 
									inner join gen_ContactPointofContact POC with(nolock) on U1.ID = POC.User_ID
									inner join gen_SalesHiringRequest H with(nolock) on POC.ContactID = H.ContactID
							where H.ID = @HR_ID

		select	top 1  H.SalesUserID, U1.ID  AS ManagerID, U1.FullName as ManagerName,H.ID AS HiringID into #Temp_Hierarchy
							from	usr_User U with(nolock) 
									inner join gen_SalesHiringRequest H with(nolock) on U.ID = H.SalesUserID
									left join usr_UserHierarchy UH with(nolock) on U.ID = UH.UserID
				                    left join usr_User U1 with(nolock) ON U1.ID = UH.ParentID AND  U1.UserTypeID in (4,9) 	
							where  U1.UserTypeID in (4,9) AND  H.ID = @HR_ID

	    ;WITH CTA_OnBoardTalent AS (
		SELECT OBT.HiringRequest_ID FROM gen_OnBoardTalents OBT WITH(NOLOCK) WHERE Status_ID = 1 
										and Exists(select 1 from gen_ContactTalentPriority CTP WITH(NOLOCK) Where CTP.HiringRequestID = OBT.HiringRequest_ID and CTP.TalentID = OBT.Talent_ID 
										and CTP.TalentStatusID_BasedOnHR in (4,10) AND CTP.HiringRequestID = @HR_ID)
										and not Exists(select 1 from gen_OnBoardTalents_ReplacementDetails RP WITH(NOLOCK) Where  RP.OnboardID = OBT.ID and RP.HiringRequestID = OBT.HiringRequest_ID and RP.OldTalentId = OBT.Talent_ID AND RP.HiringRequestID = @HR_ID)
		)

		SELECT	Co.Company As CompanyName,
				C.FullName AS ClientName,
				C.EmailID AS ClientEmail,
				ISNULL(AM_User.FullName,'') AS AM_UserName,
				ISNULL(Co.LinkedInProfile,'') LinkedInProfile,
				ISNULL(Co.Category,'') Category,
				U.FullName AS SalesPerson,
				isnull(HD.Cost,0) Cost, 
				R.TalentRole AS [Role],
				(CASE WHEN H.Availability = 'Part Time' THEN Round((cast(H.NoofTalents as float)/2),2) ELSE H.NoofTalents end) as  NoOfTalents, -- Reena Jain(15-May-2023): UTS-2829 Mismatch in TR
				--TT.TalentTimeZone + ' (' + TP.WorkingTimePreference + ')' As TimeZone,
				ISNULL(TZ.TimeZoneTitle, '') AS TimeZone, --UTS-4677: Get the universal timezone.
				Managed = case when Isnull(H.IsManaged,0) = 1 then 'Managed' else  'Self-Managed' end,
				JobDetail= case when isnull(H.JDURL,'') = '' then isnull(H.JDFileName,'') ELSE isnull(H.JDURL,'') END, 
				isnull(H.JDURL,H.JDFileName) As JobDetailURL, 
				H.HR_Number,
				Priority = case when Isnull(SHP.IsPriority,0) = 1 then 'Yes' else  'No' end ,
				AdHocHR = Isnull(H.IsAdHocHR,0) , 
				PoolHR = Isnull(H.ISPoolHR,0), 
				[Availability] = H.Availability,
				AvailabilityParttime =  case when H.Availability = 'Part Time' then cast(H.NoofHoursworking as nvarchar(10)) + ' ' +P.PartialEngagementType else '' end,
				SpecificMonth = ISNULL(HD.SpecificMonth,0),
				FromTimeAndToTime = HD.TimeZone_FromTime + ' to '+ HD.TimeZone_EndTime,
				H.RequestForTalent as [Title],
				ISNULL(C.ID,0) AS ContactId,
				ISNULL(Co.ID,0) AS CompanyId,				
				CompanyURL = CASE WHEN ISNULL(Co.Website,'') = '' THEN  ''
							ELSE
								CASE WHEN ISNULL(Co.Website,'') LIKE 'http%' OR ISNULL(Co.Website,'') LIKE 'https%' 
									THEN ISNULL(Co.Website,'')
									ELSE   '//' + ISNULL(Co.Website,'')							
									END -- Send the Company URL
							END,
				ISNULL(TPOC.FullName,'') as POCFullName,
				ISNULL(TPOC.EmailID,'') as POCEmailID,
				ISNULL(TUserHierarchy.ManagerName,'') as SalesManagerName,
				ISNULL(HD.YearOfExp,0) as MinYearOfExp,
				--------New status changes------------------------
				[HRStatus] = ISNULL(TS.HRStatus, JS.JobStatus),
				[HRStatusCode] = ISNULL(TS.HRStatusCode, 201),
				--------------------------------------------------------
				[HRStatus_Old] = case 
								when ISNULL(H.IsActive,0) = 0 and S.ID = 1 then 'Draft' 
									When ISNULL(H.IsActive,0) = 1 AND H.IsAccepted = 1 AND S.ID = 1 then 'HR Accepted' 
									When  ISNULL(H.IsActive,0) = 1 AND H.IsAccepted = 0 AND S.ID = 1 then 'Acceptance Pending'  
									When  ISNULL(H.IsActive,0) = 1 AND H.IsAccepted = 2 AND S.ID = 1 then 'Info Pending'
									When  ISNULL(H.IsActive,0) = 1 AND H.IsAccepted = 1 AND S.ID = 3 then 'Completed'
									When  ISNULL(H.IsActive,0) = 1 AND H.IsAccepted = 1 AND S.ID = 2 then 'In Process'
									When  S.ID = 5 then 'On Hold'
									When  S.ID = 4 then 'Cancelled'
									When  S.ID = 6 then 'Lost'
								ELSE
									S.HiringRequest_Status 
								end,
				[HRStatusCode_Old] = case 
									when ISNULL(H.IsActive,0) = 0 and S.ID = 1 then 101 
									When ISNULL(H.IsActive,0) = 1 AND H.IsAccepted = 1 AND S.ID = 1 then 102 
									When ISNULL(H.IsActive,0) = 1 AND H.IsAccepted = 0 AND S.ID = 1 then 103  
									When ISNULL(H.IsActive,0) = 1 AND H.IsAccepted = 2 AND S.ID = 1 then 104
									When ISNULL(H.IsActive,0) = 1 AND H.IsAccepted = 1 AND S.ID = 3 then 105 
									When ISNULL(H.IsActive,0) = 1 AND H.IsAccepted = 1 AND S.ID = 2 then 106
									When  S.ID = 4 then 107 
									When  S.ID = 5 then 108
									When  S.ID = 6 then 109
								ELSE
									201 
								end,				
				StarMarkedStatusCode = ISNULL(CASE WHEN SHP.IsNextWeekPriority = 1 THEN 101 
											  WHEN SHP.IsNextWeekPriority = 0 AND SHP.IsPriority = 1 THEN 102  
										END,0),
				ISNULL(H.BQLink,'') AS BQLink, 
				ISNULL(H.Discovery_Call,'') AS Discovery_Call,
				ISNULL(geo.GEO,'') GEO,
				0 as TR_Accepted,
				ISNULL(DP.ModeOfWork,'') as ModeOfWork,
				ISNULL(DP.Address,'') as Address,
				ISNULL(DP.City,'') as City,
				ISNULL(DP.State,'') as State,
				ISNULL(CR.CountryRegion,'') as CountryRegion,
				ISNULL(DP.PostalCode,'') as PostalCode,
				CASE WHEN isnull(H.JDFilename,'') <> '' THEN 'JDFILE' ELSE 
					(CASE WHEN isnull(H.JDURL,'') <> '' THEN 'JDURL' ELSE '' END) END as JDFileOrURL,
				--Added the LeadType and LeadUser (Riya)
				ISNULL(Co.Lead_Type, 'NA') as LeadType, 
				--LeadUser = (SELECT Top 1 FullName FROM usr_User with(nolock) WHERE ID = COL.LeadType_UserID),
				LeadUser = isnull(LedUser.FullName,'') ,
				ActiveTR = CASE WHEN H.Status_ID IN (3, 4, 6) THEN 0
							    ELSE
									CASE WHEN H.Availability = 'Part Time' THEN 
									Round((cast((H.NoofTalents - (Select Count(CT.HiringRequest_ID) from CTA_OnBoardTalent CT where H.ID = CT.HiringRequest_ID)) as float)/2),2) 
									ELSE (H.NoofTalents - (Select Count(CT.HiringRequest_ID) from CTA_OnBoardTalent CT where H.ID = CT.HiringRequest_ID)) end
								END,
				ISNULL(Co.Website,'') as ViewCompanyURL,
				ISNULL(H.RequestForTalent,'') as HRTitle,
				ISNULL(H.HRTypeId, 0) AS HRTypeId, -- Added by Riya for identifying whether the HR is Pay per Hire/Credit
				ISNULL(PRT.CompanyTypeID, 0) AS CompanyTypeID, -- Added by Riya for identifying the Company type 
				case 
					when Co.CompanyTypeID > 0 and Co.AnotherCompanyTypeID > 0 then CAST(1 AS BIT)
					else CAST(0 AS BIT) 
				end as IsHybrid-----------added ny himani for clone hr UTS-6616
		FROM    gen_SalesHiringRequest H WITH(NOLOCK)
				inner join gen_SalesHiringRequest_Details HD WITH(NOLOCK) ON H.ID = HD.HiringRequest_ID
				inner join gen_Contact C WITH(NOLOCK) ON C.ID = H.ContactID
				inner join gen_Company Co WITH(NOLOCK) ON Co.Id = C.CompanyID
				------------------------------- NEw status Changes ----------------------------------------- 
				INNER JOIN #TempTable_HrStatus TS WITH(NOLOCK) ON TS.HRID = H.ID 
				INNER JOIN prg_JobStatus_ClientPortal JS WITH(NOLOCK) ON JS.ID = H.JobStatusID
				----------------------- ----------------------- ----------------------- ----------------------- 
				--left join (	select	U1.FullName,POC.ContactID,U1.EmailID 
				--			from	usr_User U1 with(nolock) 
				--					inner join gen_ContactPointofContact POC with(nolock) on U1.ID = POC.User_ID
				--			) Z on Z.ContactID = C.ID
				inner join usr_User U WITH(NOLOCK) ON H.SalesUserID=U.ID
				LEFT JOIN prg_HiringRequestStatus S WITH(NOLOCK) ON S.ID = H.Status_ID	
				LEFT JOIN #Temp_POC TPOC On TPOC.HiringID = H.ID				
				LEFT JOIN #Temp_Hierarchy TUserHierarchy ON TUserHierarchy.HiringID = H.ID
				--left join usr_UserHierarchy UH with(nolock) on U.ID = UH.UserID
				--left join (select U1.ID as ManagerID, U1.FullName as ManagerName from usr_User U1 with(nolock) where U1.UserTypeID in (4,9)) Z1
				--			on Z1.ManagerID = UH.ParentID
				LEFT JOIN prg_TalentRoles R WITH(NOLOCK) ON R.ID = HD.Role_ID
				LEFT JOIN prg_TimeZonePreference TP WITH(NOLOCK) ON TP.ID =HD.Timezone_Preference_ID
				
				LEFT JOIN prg_ContactTimeZone TZ WITH(NOLOCK) ON TZ.ID = HD.Timezone_ID --UTS-4677: Get the universal timezone.
				LEFT JOIN gen_SalesHiringRequest_Priority SHP WITH(NOLOCK) ON SHP.HiringRequestID = H.ID
				LEFT JOIN prg_PartialEngagementType P WITH(NOLOCK) ON P.ID = H.PartialEngagementType_ID
				LEFT JOIN prg_GEO geo WITH(NOLOCK) on Co.GEO_ID = geo.ID
				LEFT JOIN gen_Direct_Placement DP WITH(NOLOCK) ON DP.hiringrequest_ID = H.ID
				LEFT JOIN prg_CountryRegion CR WITH(NOLOCK) ON CR.ID = DP.Country
				LEFT JOIN gen_CompanyLeadType_UserDetails COL WITH(NOLOCK) ON COL.CompanyID = C.CompanyID			
				LEFT JOIN usr_User AM_User WITH(NOLOCK) ON AM_User.ID = Co.AM_SalesPersonID
				LEFT JOIN usr_User LedUser with(nolock) ON LedUser.ID = COL.LeadType_UserID
				LEFT JOIN prg_HiringRequest_Type PRT ON PRT.ID = H.HrTypeId				
				
				WHERE H.ID = @HR_ID 

				DROP TABLE #Temp_POC
				DROP TABLE #Temp_Hierarchy
				DROP TABLE #TempTable_HrStatus
END
GO

ALTER PROCEDURE [dbo].[sp_UTS_get_HRTalentDetails]
@HR_ID bigint  
AS
BEGIN
	
					SELECT	IMasterID =max(IntM.ID) ,IntM.HiringRequest_ID ,IntM.Talent_ID,max(Intm.InterviewRound_Count)  InterviewRound_Count
					INTO	#TempInterviewDetails
					FROM	gen_InterviewSlotsMaster IntM WITH(NOLOCK) 	
					WHERE	IntM.HiringRequest_ID = @HR_ID
					GROUP BY  IntM.HiringRequest_ID ,IntM.Talent_ID

					SELECT   IMaxOnboardTalentID = MAX(OBT.ID), OBT.HiringRequest_ID, OBT.Talent_ID 
					INTO     #TempOnboardDetails 
					FROM     gen_OnBoardTalents OBT WITH(NOLOCK)
					WHERE    OBT.HiringRequest_ID = @HR_ID
					GROUP BY OBT.HiringRequest_ID, OBT.Talent_ID

					SELECT	SI.ID ,SI.HiringRequest_ID, SI.Talent_ID 
					INTO     #TempSelectedTalent_InterviewDetails
					from gen_ShortlistedTalent_InterviewDetails SI with(nolock) 					
					inner join gen_InterviewSlotsMaster M WITH(NOLOCK) ON M.ID = SI.InterviewMaster_ID  AND SI.Talent_ID = M.Talent_ID and SI.HiringRequest_ID = @HR_ID
					where   M.InterviewStatus_ID in(1,4,5) and SI.HiringRequest_ID = @HR_ID and M.HiringRequest_ID = @HR_ID


					;WITH CTE_NextRound AS(  
					  Select FB.ContactID,FB.HiringRequest_ID,FB.HiringRequest_Detail_ID, TI.Talent_ID   	      
						,StrInterviewRound = ' ( Round '+ cast(Count(FB.ID) + 1 as varchar(10)) +' )'    
          
					  From gen_ContactInterviewFeedback FB WITH(NOLOCK)
						inner join gen_contact GC WITH(NOLOCK) ON  GC.ID = FB.ContactID  
						inner join gen_SalesHiringRequest HR WITH(NOLOCK) ON HR.ContactID = GC.ID
						LEFT JOIN gen_TalentSelected_InterviewDetails TI ON FB.Shortlisted_InterviewID = TI.Shortlisted_InterviewID
					  WHERE HR.ID = @HR_ID   
						AND FB.FeedBack_Type ='AnotherRound'  
			            
					  group by FB.ContactID,FB.HiringRequest_ID,FB.HiringRequest_Detail_ID
					  ,TI.Talent_ID          
					 )

					select  Q.* 
					from
							(Select	 
							 distinct	case when Row_Number() OVER(partition by H.ID order by T.ID) = 1 then HR_Number ELSE '' END HR_Number,
										case when Row_Number() OVER(partition by H.ID order by T.ID) = 1 then RS.HiringRequest_Status ELSE '' END RequestStatus,
										T.Name,
										isnull(CTP.ID,0) as ContactPriorityID,
										Isnull(ISNULL(CTP.ATS_Talent_LiveURL,T.ATS_Talent_LiveURL),'') ATSTalentLiveURL, --Updated by Payal(27-11-2023).. Added Reena Jain(2-Feb-2023): NDA URL,Non NDA URL
										Isnull(ISNULL(CTP.ATS_Non_NDAURL,T.ATS_Non_NDAURL),'') ATSNonNDAURL,
										ISNULL(TalentPacket.talent_resume_link,'') AS TalentResumeLink,
										--NeedToCallAWSBucket = CASE WHEN ISNULL(TalentPacket.talent_resume_link,'') <> '' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END,
										NeedToCallAWSBucket = CASE WHEN ISNULL(CTP.ATS_Talent_LiveURL,'') = '' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END,
										-- Modified By Reena Jain(5-May-2022): Add HRCost as BillRate, TalentCost as PayRate, TalentStatus
										--Convert(varchar,CTP.HR_Cost) AS BillRate,
										--Above Line Commented by Siddharth Jain Dtd: 13/Sept/2022 as not required to convert NVARCHAR type of that field.
										ISNULL(CTP.HR_Cost,'') AS BillRate,
										--case when isnull(CTP.Cost_AfterAcceptance,'''') <> '''' then isnull(CTP.Cost_AfterAcceptance,'''')  else  Isnull(T.Fees,'''') END As PayRate,
										--Added dynamic currency on 25-07-2023 by Riya
										case when isnull(CTP.TalentCost,0)  > 0 then CAST(ISNULL(PCER.CurrencySign,'$') as NVARCHAR(10)) + replace(cast(CTP.TalentCost as Nvarchar(20)),'.00','') +  ' ' + CAST(ISNULL(PCER.CurrencyCode,'USD') as NVARCHAR(10)) +  ' / Month'  else  Isnull(T.Fees,'''') END As PayRate,
										isnull(CTP.TalentCost,0) AS CTP_TalentCost,
										--isnull(TSACS.TalentStatus,'') As [Status],
										CASE WHEN ISnull(CTP.TalentStatusID_BasedOnHR,0) > 0 and ISnull(CTP.TalentStatusID_ClientPortal,0) > 0 then   
									   (Select Isnull(TSC.TalentStatus,'') from prg_TalentStatus_ClientPortal TSC   
										WITH(NOLOCK) WHERE TSC.ID = CTP.TalentStatusID_ClientPortal) --Isnull(TSC.TalentStatus,'')  
										  ELSE Isnull(TSACS.TalentStatus,'') + 
													 CASE WHEN CTP.TalentStatusID_BasedOnHR = 3 THEN
													 CASE WHEN ISNULL(CTE.StrInterviewRound,'') <> '' then ' - '+ isnull(CTE.StrInterviewRound,'') ELSE '' END
													 ELSE '' END END As [Status],
													 --+ CASE WHEN ISNULL(CTE.StrInterviewRound,'') <> '' then ' - '+ isnull(CTE.StrInterviewRound,'') ELSE '' END ENd As [Status],
										Convert(varchar,isnull(CTP.NRPercentage,H.TalentCostCalcPercentage)) + ' %' AS NR ,
										Convert(varchar,isnull(CTP.DP_Percentage,H.DPPercentage)) + ' %' AS DPPercentage,
										ISNULL(CTP.DPAmount,0) as DPAmount,
										ISNULL(CTP.DpAmount,0) as BillRateDP,
										--case when isnull(CTP.current_CTC,0)  > 0 then CAST(ISNULL(PCER.CurrencySign,'$') as NVARCHAR(10)) +replace(cast(CTP.current_CTC as Nvarchar(20)),'.00','') + ' ' +CAST(ISNULL(PCER.CurrencyCode,'USD') as NVARCHAR(10)) + ' / Month' END As PayRateDP,
										case when isnull(CTP.current_CTC,0)  > 0 then CAST(ISNULL(PCER.CurrencySign,'$') as NVARCHAR(10)) +replace(cast(CTP.current_CTC as Nvarchar(20)),'.00','') + ' ' +CAST(ISNULL(PCER.CurrencyCode,'USD') as NVARCHAR(10)) END As PayRateDP,
										TT.TimeZoneTitle as TalentTimeZone,
										NoticePeriod = isnull(TJ.Joinning,'')   ,--Payal (28-06-2022) Remove outer apply-- Modefied By Reena Jain(1 May 2022): Add Time Zone & Notoce Period
										isnull(CTP.TalentStatusID_BasedOnHR,0) TalentStatusID_BasedOnHR,
										isnull(T.ID,0) as TalentID,
										isnull(CTP.HiringRequest_Detail_ID,0) as HiringDetailID,
										
										InterViewStatusId=isnull(IntM.InterviewStatus_ID,0),
										IsdisplaySelectTimeslot = isnull((select top 1 ID from #TempSelectedTalent_InterviewDetails TSI where TSI.HiringRequest_ID =CTP.HiringRequestID AND TSI.Talent_ID = CTP.TalentID order by ID desc  )  ,0),
					
										SelectedInterviewId = isnull(TI.ID,0),
										InterviewStatus =  ISNULL(INS.InterviewStatus,'') ,
										InterviewROUND = isnull(IntM.InterviewRound_Str,''),
										InterviewDateTime = LTRIM(RTRIM(isnull((STUFF(( select char(10) + (CONVERT(varchar, ISNULL(S.ScheduledOn, ' '), 105) + ' '+ convert(VARCHAR(5),S.Interview_StartTime, 14)) +' To ' +(convert(VARCHAR (5),S.Interview_EndTime, 14)) from gen_ShortlistedTalent_InterviewDetails S WITH(NOLOCK) WHERE S.InterviewMaster_ID = IMasterID For XML Path('')),1,1,'')),''))),
										Slotconfirmed = isnull((select CONVERT(varchar, ISNULL(s.ScheduledOn, ' '), 105) + ' '  + convert(VARCHAR(5),s.Interview_StartTime, 14) +' To ' + convert(VARCHAR(5),s.Interview_EndTime, 14) from gen_ShortlistedTalent_InterviewDetails S inner join gen_TalentSelected_InterviewDetails I WITH(NOLOCK) ON S.ID = I.Shortlisted_InterviewID where isnull(I.Zoom_InterviewLink,'') != '' AND I.ID =TI.ID),''),
										Isnull(H.ContactID,0) as ContactId,
					
										--isnull(IMasterID,0) as MasterId,
										--Commented By Siddharth Jain: Dtd - 28/Oct/2022 as MasterId is already considered below with Same alias.
										Shortlisted_InterviewID =isnull(TI.Shortlisted_InterviewID,0),
										ISNULL(O.ClientOnBoarding_StatusID,0) ClientOnBoarding_StatusID,
										Isnull(O.ID,0) AS OnBoardId,
										Isnull(POB_Clientonboard.PreOnBoardStatus,'') ClientOnBoardStatus,
										ISNULL(O.TalentOnBoarding_StatusID,0) TalentOnBoarding_StatusID,
										ISNULL(O.ClientLegal_StatusID,0) LegalClientOnBoarding_StatusID,
										ISNULL(O.TalentLegal_StatusID,0) LegalTalentOnBoarding_StatusID,
										ISNULL(O.Kickoff_StatusID,0) Kickoff_StatusID,
										Isnull(POB_Talentonboard.PreOnBoardStatus,'') TalentOnBoardStatus,
										ClientFeedback = isnull(CF.FeedBack_Type,''),
										SlotGivenStatus = isnull(N.SlotGiven,''),
										IsLaterSlotGiven = isnull(N.IsLaterSlotGiven,0),
										NextRound_InterviewDetailsID = isnull(N.ID,0),
										TalentOnBoardDate =  ISNULL((CONVERT(varchar, ISNULL(OC.TalentOnBoardDate, ''), 103) + ' '  + convert(VARCHAR(8),OC.TalentOnBoardDate, 14)),'') ,
										ISNULL(Tmp.IMasterID,0) As MasterId,
										Isnull(O.EngagemenID,'') EngagemenID, -- Added by Reena Jain(29-Nov-2022): Add for AM Assignment 
										IsAMAssigned = Case when Isnull(CO.AM_SalesPersonID,0) = 0 and (U.IsNewUser = 1 and ISNULL(U.IsPartnerUser,0) = 0) then Cast(0 as int) else Cast(1 as int) end,
										ISNULL(PTAW.AssociatedWithUplers,'') as PreferredAvailability,
										[ProfileStatusCode] = CASE	WHEN TSACS.ID = 1 THEN 301
																WHEN TSACS.ID = 2 THEN 302
																WHEN TSACS.ID = 3 THEN 303
																WHEN TSACS.ID = 4 THEN 303
																WHEN TSACS.ID = 5 THEN 305
																WHEN TSACS.ID = 6 THEN 306
																WHEN TSACS.ID = 7 THEN 307
																WHEN TSACS.ID = 8 THEN 308
																WHEN TSACS.ID = 9 THEN 307
																WHEN TSACS.ID = 10 THEN 302
																WHEN TSACS.ID = 11 THEN 302
																WHEN TSACS.ID = 12 THEN 302
														END,
												CASE WHEN ISNULL(T.Designation,'') = '' THEN ISNULL(TR.TalentRole,'') ELSE ISNULL(T.Designation,'') END as TalentRole,
												ISNULL(TalType.Talent_Type,'') as TalentSource,
												ISNULL(T.TotalExpYears,0) as TotalExpYears,
										ClientFeedbackID = ISNULL(CF.ID,0), --Added By Reena Jain(25-May-2023):for Edit feedback					
										--UTS-3372: Show the Reason for changing talent status
										RejectedReason = CASE WHEN ISnull(CTP.RejectReasonID,0) <= 0 and Isnull(CTP.TalentStatusID_BasedOnHR,0) = 7
															THEN 'Other'
															ELSE 
															ISNULL(ParentName, '') + 
															IIF(ISNULL(ParentName, '') <> '' AND ISNULL(PTRR.Reason, '') <> '', ' - ', '') + 
															ISNULL(PTRR.Reason, '')
														END,
										OnHoldReason =  CASE WHEN ISnull(CTP.RejectReasonID,0) <= 0 and Isnull(CTP.TalentStatusID_BasedOnHR,0) = 6
															THEN 'Other'
														ELSE ISNULL(PTRR.Reason,'')
														END,
										CancelledReason = CASE WHEN Isnull(CTP.TalentStatusID_BasedOnHR,0) = 5 AND H.Status_ID IN (3, 4, 6)
																THEN 'Auto-Cancelled'
															WHEN ISnull(CTP.RejectReasonID,0) <= 0 AND  Isnull(CTP.TalentStatusID_BasedOnHR,0) = 5
															THEN 'Other'
															ELSE ISNULL(PTRR.Reason,'')
														END,
				
										-- Get the other reason for the talent status
										TalentOtherReason = CASE 
															WHEN CTP.RejectReasonID <= 0 THEN ISNULL(CTP.OtherRejectReason,'') ELSE ''
														END,
		   
										-- Get the remarks for the talent status 
										TalentRemarks = CASE
															WHEN TSACS.ID = 6 AND CTP.RejectReasonID <= 0 THEN ISNULL(CTP.OnHoldRemark,'')
															WHEN CTP.RejectReasonID <= 0 AND (TSACS.ID = 5 OR TSACS.ID = 7) THEN ISNULL(CTP.LossRemark,'')
															ELSE ''
														END,
				  
										-- Get the Actual interview status of the talent
										TalentInterviewStatus = CASE WHEN ISNULL(TI.Status_ID, 0) <> 0 AND ISNULL(INS.InterviewStatus,'') <> '' 
																AND Isnull(CTP.TalentStatusID_BasedOnHR,0) IN (4, 5, 6, 7)
																THEN ISNULL(TSACS.TalentStatus, '') 										
														END,

										-- Color code for the interview status
										InterviewStatusCode = CASE	
																WHEN TSACS.ID = 7 AND ISNULL(CF.FeedBack_Type,'') = '' THEN '#FFF4F4 ' -- red color (Rejected)	
																WHEN TSACS.ID = 7 AND ISNULL(CF.FeedBack_Type,'') <> '' THEN '#D8F3F2 ' -- green color (Rejected in interview)
																WHEN TSACS.ID = 6 AND ISNULL(CF.FeedBack_Type,'') = '' THEN '#ead9c8' -- yellow color (onHold)
																WHEN TSACS.ID = 6 AND ISNULL(CF.FeedBack_Type,'') <> '' THEN '#D8F3F2' -- green color (onHold in interview)
																WHEN TSACS.ID = 4 THEN '#D8F3F2' -- green color (hired)
																WHEN TSACS.ID = 5 THEN '#FFF4F4 ' -- red color (Cancelled)
																WHEN TSACS.ID = 9 THEN '#FFF4F4 ' -- red color (Delete)
																ELSE '#808080' --default color
															END,

										-- Get the currency code of the talent.
									TalentCurrenyCode = ISNULL(CTP.Talent_CurrencyCode, 'USD'),

									--Get the Currency sign.
									CurrencySign = ISNULL(PCER.CurrencySign,'$'),

									--Get the Talent POC Name
									TalentPOCName = ISNULL(OpsUser.FullName,'') ,

									--Get the HRTypeDP---				
									IsHRTypeDP = ISNULL(H.IsHRTypeDP, 0),			
								TSACS.OrderSequence , CTP.CreatedByDatetime,
								ISNULL(O.TSC_PersonID,0) AS TSC_PersonID,
								Isnull(T.EmailID,'') as EmailID,
								Isnull(CTP.UplersfeesAmount,'') as UplersfeesAmount,
								Isnull(TT2.TimeZoneTitle,'') AS ScheduleTimeZone,
								cast(Case when  Cast(getdate() as date) < Isnull(cast(Isnull(O.Joiningdate,OC.ContractStartDate) as date),cast(getdate() as date)) then 1 else 0 end as int) as IsShownTalentStatus,
								ISNULL(T.ATS_Talent_ID,0) AS ATSTalentID
						from	gen_Talent T WITH(NOLOCK)
								inner join [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID=T.ID
								inner join [dbo].[gen_SalesHiringRequest] H WITH(NOLOCK) ON H.ID=CTP.HiringRequestID
								inner JOIN prg_HiringRequestStatus RS WITH(NOLOCK) ON RS.ID = H.Status_ID
								inner join [dbo].[gen_SalesHiringRequest_Details] HD WITH(NOLOCK) ON H.ID = HD.HiringRequest_ID
								inner join prg_TalentStatus_AfterClientSelection TSACS WITH(NOLOCK)  ON  TSACS.ID = CTP.TalentStatusID_BasedOnHR
								INNER JOIN prg_ContactTimeZone TT with(nolock) ON HD.Timezone_ID = TT.ID
								INNER JOIN usr_User OpsUser with(nolock) ON OpsUser.ID = CTP.CreatedByID
								LEFT JOIN gen_SalesHiringRequest_TalentPacketDetails TalentPacket with(nolock) ON  CTP.TalentID = TalentPacket.TalentID AND CTP.HiringRequestID = TalentPacket.HiringRequestID
								LEft JOIN #TempInterviewDetails Tmp ON Tmp.HiringRequest_ID = H.ID AND Tmp.Talent_ID = T.ID
								LEft join gen_InterviewSlotsMaster IntM WITH(NOLOCK) ON IntM.ID = Tmp.IMasterID 
								--and Tmp.InterviewRound_Count = IntM.InterviewRound_Count
								Left JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON  
								--TI.InterviewRound = IntM.InterviewRound_Count and  
								TI.InterviewMaster_ID=IntM.ID AND TI.HiringRequest_ID = IntM.HiringRequest_ID AND TI.HiringRequest_Detail_ID = IntM.HiringRequest_Detail_ID AND TI.Talent_ID = IntM.Talent_ID
								LEft join prg_InterviewStatus INS WITH(NOLOCK) ON INS.ID = IntM.InterviewStatus_ID	
								LEft join #TempOnboardDetails TempOnboard ON TempOnboard.HiringRequest_ID = H.ID AND TempOnboard.Talent_ID = T.ID
								left join gen_OnBoardTalents O WITH(NOLOCK) ON O.ID = TempOnboard.IMaxOnboardTalentID and O.Talent_ID = TempOnboard.Talent_ID AND O.HiringRequest_ID = TempOnboard.HiringRequest_ID
								left join gen_OnBoardClientContractDetails OC WITH(NOLOCK) ON OC.OnBoardID = O.ID
								left join prg_PreOnBoardStatus POB_Clientonboard WITH(NOLOCK) ON POB_Clientonboard.ID = O.ClientOnBoarding_StatusID
								left join prg_PreOnBoardStatus POB_Talentonboard WITH(NOLOCK) ON POB_Talentonboard.ID = O.TalentOnBoarding_StatusID AND O.Talent_ID = T.ID
								left join prg_PreOnBoardStatus POB_TalentLegal WITH(NOLOCK) ON POB_TalentLegal.ID = O.TalentLegal_StatusID
								left join prg_PreOnBoardStatus POB_ClientLegal WITH(NOLOCK) ON POB_ClientLegal.ID = O.ClientLegal_StatusID
								left join prg_TalentJoinning TJ WITH(NOLOCK)  ON TJ.ID = T.Joining_ID	
								left join gen_ContactInterviewFeedback CF with(nolock) ON  CF.Shortlisted_InterviewID = TI.Shortlisted_InterviewID AND CF.HiringRequest_ID =  Tmp.HiringRequest_ID
								left join gen_TalentSelected_NextRound_InterviewDetails N WITH(NOLOCK) ON N.Shortlisted_InterviewID = TI.Shortlisted_InterviewID AND N.Talent_ID = Tmp.Talent_ID and N.InterviewRound = IntM.InterviewRound_Count and isnull(N.IsLaterSlotGiven,0) =1
								left join gen_Contact C WITH(NOLOCK) ON C.ID = O.ContactID -- Added by Reena Jain(29-Nov-2022): Add for AM Assignment 
								left join gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID
								left join prg_TalentRoles TR WITH(NOLOCK) ON TR.ID = T.RoleID
								left join usr_User U WITH(NOLOCK) ON U.ID = H.SalesUserID 
								left join prg_GEO G WITH(NOLOCK) ON G.ID = CO.GEO_ID
								Left join usr_User Sales WITH(NOLOCK) ON Sales.ID = CO.AM_SalesPersonID
								left join prg_TalentType TalType with(nolock) on T.Talent_TypeID = TalType.ID 
								left join prg_TalentAssociatedWithUplers PTAW with(nolock) on T.AssociatedwithUplers_ID = PTAW.ID 
								left join prg_TalentRejectReason PTRR WITH(NOLOCK) ON  PTRR.ID = CTP.RejectReasonID --UTS-3372: Show the Reason for changing talent status
								LEFT JOIN prg_CurrencyExchangeRate PCER with(nolock) ON PCER.CurrencyCode = ISNULL(CTP.Talent_CurrencyCode, 'USD')
								LEFT JOIN prg_ContactTimeZone TT2 with(nolock) ON TI.Contact_TimeZone_ID = TT2.ID
								LEFT JOIN CTE_NextRound CTE ON CTE.Talent_ID = CTP.TalentID AND H.ID = CTE.HiringRequest_ID AND  CTE.ContactID = H.ContactID AND CTE.HiringRequest_Detail_ID = HD.ID  
						WHERE H.ID=@HR_ID	
				)Q 
				order by  Q.OrderSequence asc, Q.CreatedByDatetime desc --UTS-3933: Order by the list with the talent status and then by added time.
	
	 drop table #TempInterviewDetails
END


GO

ALTER PROCEDURE [dbo].[Sproc_UTS_AddEdit_HR]  
	@HrId						BIGINT			= NULL,
	@ContactId 					BIGINT			= NULL,
	@SalesPersonID				INT				= NULL,
	@Availability				NVARCHAR(200)	= NULL,
	@ContractDuration			INT				= NULL,
	@Currency					NVARCHAR(50)	= NULL,
	@AdhocBudgetCost			DECIMAL(18,2)	= NULL,
	@MinimumBudget				DECIMAL(18,2)	= NULL,
	@MaximumBudget				DECIMAL(18,2)	= NULL,
	@IsConfidentialBudget		BIT				= NULL,
	@ModeOfWorking				NVARCHAR(100)	= NULL,
	@City						NVARCHAR(200)	= NULL,
	@Country					NVARCHAR(200)	= NULL,
	@JDFilename					NVARCHAR(400)	= NULL,
	@JDURL						NVARCHAR(MAX)	= NULL,
	@JddumpId					BIGINT			= NULL,
	@YearOfExp					DECIMAL(18,2)	= NULL,
	@NoofTalents				INT				= NULL,
	@TimezoneId					INT				= NULL,
	@TimeZoneFromTime			NVARCHAR(200)	= NULL,
	@TimeZoneEndTime			NVARCHAR(200)	= NULL,
	@OverlapingHours			DECIMAL(18,1)	= NULL,
	@HowSoon					NVARCHAR(400)	= NULL,
	@PartialEngagementTypeID	INT				= NULL,
	@NoofHoursworking			INT				= NULL,
	@DurationType				NVARCHAR(50)	= NULL,
	@IsHRTypeDP					BIT				= NULL,
	@DpPercentage				DECIMAL(18,2)	= NULL,
	@NRMargin					DECIMAL(18,2)	= NULL,
	@IsTransparentPricing		BIT				= NULL,
	@HrTypePricingId			INT				= NULL,
	@PayrollTypeId				INT				= NULL,
	@PayrollPartnerName			NVARCHAR(300)	= NULL,
	@IsVettedProfile			BIT				= NULL,
	@IsHiringLimited			BIT				= NULL,
	@IsPayPerHire				BIT				= NULL,
	@IsPayPerCredit				BIT				= NULL,
	@IsPostaJob					BIT				= NULL,
	@IsProfileView				BIT				= NULL,
	@IsDirectHR					BIT				= NULL,
	@IsDraftHR					BIT				= NULL,
	@Bqlink						NVARCHAR(MAX)	= NULL,
	@Discovery_Call				NVARCHAR(MAX)	= NULL,
	@DealID						NVARCHAR(300)	= NULL,
	@AllowSpecialEdit			BIT				= NULL,
	@ChildCompanyName			NVARCHAR(500)	= NULL,
	@IsFresherAllowed			BIT				= NULL,
	@LoginUserId				INT				= NULL
AS
BEGIN
	DECLARE @Cost NVARCHAR(500) = NULL, @HRDetailID BIGINT = NULL, @CountryText NVARCHAR(200), 
			@FormattedBudgetFrom NVARCHAR(MAX) = '',
			@FormattedBudgetTo NVARCHAR(MAX) = '',
			@FormattedAdhocBudget NVARCHAR(MAX) = ''
	DECLARE @CompanyID BIGINT, @CompanyDiscoveryCall NVARCHAR(MAX), @ComapnyTransparentPricing BIT;

	IF @HrId > 0 
	BEGIN
		--Update
		IF EXISTS (SELECT TOP 1 ID FROM gen_SalesHiringRequest WITH (NOLOCK) WHERE ID = @HrId)
		BEGIN

			--SET @HRDetailID = (SELECT HD.ID FROM gen_SalesHiringRequest_Details HD WITH (NOLOCK)
			--WHERE HD.HiringRequest_ID = @HrId)

			SELECT @HRDetailID = HD.ID FROM gen_SalesHiringRequest_Details HD WITH (NOLOCK)
			WHERE HD.HiringRequest_ID = @HrId

			---Convert into proper formatting for budget (Riya - 20 May-2024)
			SELECT @FormattedBudgetFrom = format(@MinimumBudget, N'N', Culture), 
				   @FormattedBudgetTo = format(@MaximumBudget, N'N', Culture),
				   @FormattedAdhocBudget = format(@AdhocBudgetCost, N'N', Culture)
			FROM prg_CurrencyExchangeRate WITH(NOLOCK) WHERE CurrencyCode = @Currency

			--Range budget
			IF(ISNULL(@MinimumBudget,0) >  0 AND ISNULL(@MaximumBudget,0) > 0)
			BEGIN
				SET @Cost = (
								SELECT IIF
										(
											ISNULL(@MinimumBudget,0) = 0, 
											@FormattedBudgetFrom,
											CAST(CurrencySign AS NVARCHAR) + ' ' + @FormattedBudgetFrom
										) 
										+ ' to ' +  CAST(CurrencySign AS NVARCHAR) + ' ' + @FormattedBudgetTo + ' ' + @Currency FROM prg_CurrencyExchangeRate with(NoLock) WHERE CurrencyCode = @Currency
							)
							SET	@AdhocBudgetCost = 0
			END
			--Adhoc Budget
			IF(ISNULL(@AdhocBudgetCost,0) >  0)
			BEGIN
				SET @Cost = (
								SELECT IIF
										(
											ISNULL(@AdhocBudgetCost,0) = 0, 
											@FormattedAdhocBudget,
											+ 'Upto ' + CAST(CurrencySign AS NVARCHAR) + ' ' + @FormattedAdhocBudget + ' ' + @Currency + ' / Month' 										
										) 
										FROM prg_CurrencyExchangeRate with(NoLock) WHERE CurrencyCode = @Currency
							)
				SET @MinimumBudget = 0 
				SET	@MaximumBudget = 0 
			END

			--
			BEGIN ----Payal (08-05-2024)   Insert in history tables 
			
			--EXEC SPROC_Gen_SalesHiringRequest_History @HRID = @HrId, @AppActionDoneBy = 3
			--EXEC SPROC_Gen_SalesHiringRequest_Details_History @HRID = @HrId, @AppActionDoneBy = 3


			Declare @HiringRequestHisotoryID as bigint = 0
			Declare @HRDetailHisotoryID as bigint = 0

			Insert into gen_SalesHiringRequest_History(HiringRequest_ID,ContactID,LastModifiedByID,LastModifiedDatetime
											,Availability,IsCreatedByClient,IsActive,AppActionDoneBy,JDFilename,JDDump_ID,JDURL,NoofTalents,NoofHoursworking,
											Discovery_Call,DPPercentage,IsHRTypeDP,IsTransparentPricing,PayrollTypeId,PayrollPartnerName,PartialEngagementType_ID,
											IsVettedProfile,IsHiringLimited,HR_TypeId,IsDirectHR,BQLink,DealID,ChildCompanyID,HistoryDate,HiringTypePricingId)
			--Values(@HrId,@ContactID,@ContactID,Getdate(),@Availability,1,1,3,@JDFilename,@JddumpId,@JDURL,@NoofTalents,@NoofHoursworking,@Discovery_Call,
			--@DpPercentage,@IsHRTypeDP,@IsTransparentPricing,@PayrollTypeId,@PayrollPartnerName,@PartialEngagementTypeID,@IsVettedProfile,@IsHiringLimited,
			--IIF(ISNULL(@IsHRTypeDP, 0) = 1, 1, 2),@IsDirectHR,@Bqlink,@DealID,@ChildCompanyName)
			SELECT @HrId,
			       ContactID = Case when ContactID <> @ContactID then @ContactID else null end,
			       LastModifiedByID = @ContactID,
				   Getdate(),
				   HRAvailability = Case when Availability <> @Availability then @Availability else null end,
				   1,1,3,
				   JDfile = Case when JDFilename <> @JDFilename then @JDFilename else null end,
				   JddumpId = Case when JDDump_ID <> @JddumpId then @JddumpId else null end,
				   JDURL = Case when JDURL <> @JDURL then @JDURL else null end,
				   NoofTalents = Case when NoofTalents <> @NoofTalents then @NoofTalents else null end,
				   NoofHoursworking = Case when NoofHoursworking <> @NoofHoursworking then @NoofHoursworking else null end,
				   DiscoveryCall = Case when Discovery_Call <> @Discovery_Call then @Discovery_Call else null end,
				   DPPercentage = Case when DPPercentage <> @DpPercentage then @DpPercentage else null end,
				   IsHRTypeDP = Case when IsHRTypeDP <> @IsHRTypeDP then @IsHRTypeDP else null end,				  
				   IsTransparentPricing = Case when IsTransparentPricing <> @IsTransparentPricing then @IsTransparentPricing else null end,
				   PayrollTypeId = Case when PayrollTypeId <> @PayrollTypeId then @PayrollTypeId else null end,
				   PayrollPartnerName = Case when PayrollPartnerName <> @PayrollPartnerName then @PayrollPartnerName else null end,
				   PartialEngagementType_ID = Case when PartialEngagementType_ID <> @PartialEngagementTypeID then @PartialEngagementTypeID else null end,
				   IsVettedProfile = Case when IsVettedProfile <> @IsVettedProfile then @IsVettedProfile else null end,
				   IsHiringLimited = Case when IsHiringLimited <> @IsHiringLimited then @IsHiringLimited else null end,
				   HR_TypeId = Case when HR_TypeId <> IIF(ISNULL(@IsHRTypeDP, 0) = 1, 1, 2) then IIF(ISNULL(@IsHRTypeDP, 0) = 1, 1, 2) else null end,
				   IsDirectHR = Case when IsDirectHR <> @IsDirectHR then @IsDirectHR else null end,
				   BQLink = Case when BQLink <> @Bqlink then @Bqlink else null end,
				   DealID = Case when DealID <> @DealID then @DealID else null end,
				   ChildCompanyID = Case when ChildCompanyID <> @ChildCompanyName then @ChildCompanyName else null end,
				   Getdate(),
				   HiringTypePricingId = case when HiringTypePricingId <> @HrTypePricingId then @HrTypePricingId else null end
			FROM gen_SalesHiringRequest WITH(NOLOCK) WHERE ID = @HrId 

			SET @HiringRequestHisotoryID = @@IDENTITY

			Insert into gen_SalesHiringRequest_Details_History(HiringRequest_ID,Duration,DurationType,Cost,YearOfExp,HowSoon,Timezone_ID
							,LastModifiedByID,LastModifiedDatetime,TimeZone_FromTime,TimeZone_EndTime,BudgetFrom,BudgetTo,Currency,AppActionDoneBy,PartialEngagementType_ID,
							Adhoc_BudgetCost,IsConfidentialBudget,OverlapingHours, IsFresherAllowed,HistoryDate)
			--Values(@HrId,@ContractDuration,@DurationType,@Cost,@YearOfExp,@HowSoon,
			--	@TimezoneID,@ContactID,GetDate(),@TimeZoneFromTime,@TimeZoneEndTime,@MinimumBudget,@MaximumBudget,@Currency,3,@PartialEngagementTypeID,
			--	@AdhocBudgetCost,@IsConfidentialBudget,@OverlapingHours, @IsFresherAllowed)
			SELECT @HrId,
				   Duration = Case when Duration <> @ContractDuration then @ContractDuration else null end,
				   DurationType = Case when DurationType <> @DurationType then @DurationType else null end,
				   Cost = Case when Cost <> @Cost then @Cost else null end,
				   YearOfExp = Case when YearOfExp <> @YearOfExp then @YearOfExp else null end,
				   HowSoon = Case when HowSoon <> @HowSoon then @HowSoon else null end,
				   Timezone_ID = Case when Timezone_ID <> @TimezoneID then @TimezoneID else null end,
				   @ContactID,GetDate(),
				   TimeZone_FromTime = Case when TimeZone_FromTime <> @TimeZoneFromTime then @TimeZoneFromTime else null end,
				   TimeZone_EndTime = Case when TimeZone_EndTime <> @TimeZoneEndTime then @TimeZoneEndTime else null end,
				   BudgetFrom = Case when BudgetFrom <> @MinimumBudget then @MinimumBudget else null end,
				   BudgetTo = Case when BudgetTo <> @MaximumBudget then @MaximumBudget else null end,
				   Currency = Case when Currency <> @Currency then @Currency else null end,
				   3,
				   PartialEngagementType_ID = Case when PartialEngagementType_ID <> @PartialEngagementTypeID then @PartialEngagementTypeID else null end,
				   Adhoc_BudgetCost = Case when Adhoc_BudgetCost <> @AdhocBudgetCost then @AdhocBudgetCost else null end,
				   IsConfidentialBudget = Case when IsConfidentialBudget <> @IsConfidentialBudget then @IsConfidentialBudget else null end,
				   OverlapingHours = Case when OverlapingHours <> @OverlapingHours then @OverlapingHours else null end,
				   IsFresherAllowed = Case when IsFresherAllowed <> @IsFresherAllowed then @IsFresherAllowed else null end,
				   Getdate()

			FROM gen_SalesHiringRequest_Details WITH(NOLOCK) WHERE ID = @HRDetailID 

				SET @HRDetailHisotoryID = @@IDENTITY

			END

			--------------------------------------------------------------------------------
			--Added by Himani (12-June-24)
			--IF contactid change than in all table we need to update contactid
			--------------------------------------------------------------------------------
			BEGIN
				DECLARE @IsContactChange BIT = 0;
				SELECT  @IsContactChange =
						Case when ContactID <> @ContactID then 1 else 0 end
				FROM gen_SalesHiringRequest WITH(NOLOCK) WHERE ID = @HrId

				IF @IsContactChange = 1
				BEGIN
					EXEC Sproc_UTS_Update_ContactViaCOEAccess
					@HR_ID = @HrId, 
					@ContactID = @ContactID
				END
			END
			---------------------------------------------------------------------------------
			
			--update in gen_SalesHiringRequest
			UPDATE gen_SalesHiringRequest
			SET
			ContactID = ISNULL(@ContactId,ContactID),
			SalesUserID = ISNULL(@SalesPersonID, SalesUserID),
			Availability = ISNULL(@Availability, Availability),
			JDFilename = ISNULL(@JDFileName, JDFilename),
			JDURL = ISNULL(@JDURL, JDURL),
			NoofTalents = ISNULL(@NoOfTalents, NoofTalents),
			PartialEngagementType_ID = ISNULL(@PartialEngagementTypeId, PartialEngagementType_ID),
			NoofHoursworking = ISNULL(@NoofHoursworking, NoofHoursworking),
			IsHrtypeDp = ISNULL(@IsHRTypeDP, IsHrtypeDp),
			DpPercentage = ISNULL(@DPPercentage, DPPercentage),
			TalentCostCalcPercentage = ISNULL(@NRMargin, TalentCostCalcPercentage),
			IsTransparentPricing = ISNULL(@IsTransparentPricing, IsTransparentPricing),
			HiringTypePricingId = ISNULL(@HrTypePricingId, HiringTypePricingId),
			PayrollTypeId = ISNULL(@PayrollTypeId, PayrollTypeId),
			PayrollPartnerName = ISNULL(@PayrollPartnerName, PayrollPartnerName),
			HR_TypeId = IIF(ISNULL(@IsHRTypeDP, IsHrtypeDp) = 1, 1, 2),
			IsVettedProfile = ISNULL(@IsVettedProfile, IsVettedProfile),
			IsHiringLimited = ISNULL(@IsHiringLimited, IsHiringLimited),
			DealID = ISNULL(@DealID, DealID),
			BQLink = ISNULL(@BQLink, BQLink),
			Discovery_Call = ISNULL(@Discovery_Call, Discovery_Call),
			LastModifiedByID = ISNULL(@LoginUserId, LastModifiedByID),
			LastModifiedDatetime = GETDATE()
			WHERE ID = @HrId

			SELECT @HrId AS Id--Do not changed/move, otherwise will impact on create hr

			--Update data in gen_SalesHiringRequest_Details
			UPDATE gen_SalesHiringRequest_Details
			SET 
			SpecIFicMonth = ISNULL(@ContractDuration, SpecIFicMonth),
			NoofEmployee = ISNULL(@NoOfTalents, NoofEmployee),
			Currency = ISNULL(@Currency, Currency),
			BudgetFrom = ISNULL(@MinimumBudget, BudgetFrom),
			BudgetTo = ISNULL(@MaximumBudget, BudgetTo),
			Adhoc_BudgetCost = ISNULL(@AdhocBudgetCost, Adhoc_BudgetCost),
			Cost = ISNULL(@Cost, Cost),
			IsConfidentialBudget = ISNULL(@IsConfidentialBudget, IsConfidentialBudget),
			YearOfExp = ISNULL(@YearOfExp, YearOfExp),
			Timezone_ID = ISNULL(@TimeZoneID, Timezone_ID),
			TimeZone_FromTime = ISNULL(@TimeZoneFromTime, TimeZone_FromTime),
			TimeZone_EndTime = ISNULL(@TimeZoneEndTime, TimeZone_EndTime),
			HowSoon = ISNULL(@HowSoon, HowSoon),
			OverlapingHours = ISNULL(@OverlapingHours, OverlapingHours),
			DurationType = ISNULL(@DurationType, DurationType),
			IsFresherAllowed = ISNULL(@IsFresherAllowed, IsFresherAllowed),
			LastModifiedByID = ISNULL(@LoginUserId, LastModifiedByID),
			LastModifiedDatetime = GETDATE()
			WHERE HiringRequest_ID = @HrId
			
			--update data in gen_Direct_Placement
			UPDATE  gen_Direct_Placement
			SET
			ModeOfWork = ISNULL(@ModeOfWorking, ModeOfWork),
			City = ISNULL(@City, City),
			Country = ISNULL(@Country, Country),
			ModIFiedById = ISNULL(@LoginUserId, ModIFiedById),
			ModIFiedByDateTime = GETDATE()
			WHERE HiringRequest_Id = @HrId

				--Update calculation
			EXEC SP_PayPerHire_Calculation_Update @HRID = @HrId
			
			

			--Insert history action for Update_HR from ATS
			EXEC sproc_HiringRequest_History_Insert @Action = 'Update_HR', @HiringRequest_ID = @HrId,
				@Talent_ID = 0, @Created_From = 0, @CreatedById = @LoginUserId,
				@ContactTalentPriority_ID = 0, @InterviewMaster_ID = 0,
				@HR_AcceptedDateTime = '', @OnBoard_ID = 0, @SalesUserID = 0,
				@OldSalesUserID = 0, @AppActionDoneBy = 3,@SalesHistoryID = @HiringRequestHisotoryID,@SalesDetailHistoryID = @HRDetailHisotoryID 
				



			---Added Jimit 03-06-24  UTS-7505
			--In Edit HR SP update Sales Person ID for this Company's HR with New Sales Person ID where Ayush is Sales Person Point no 3
			Declare @DCSalesEmpID as int = 0
			Declare @DCSalesUserID as bigint = 0

			
			select  @CompanyId = CompanyID from gen_Contact WITH(NOLOCK) where ID = @ContactId
			select @DCSalesEmpID = [Value] from gen_SystemConfiguration WITH(NOLOCK) where [Key] = 'ClientFrontEndPayPerHRPOC'
			Select @DCSalesUserID = ID from usr_User with(Nolock) where EmployeeID = @DCSalesEmpID

			Update H
			SET    H.SalesUserId = Isnull(@SalesPersonID,H.SalesUserID)
			from   gen_SalesHiringRequest H
				   inner join gen_Contact C ON C.ID = H.ContactID
			where  C.CompanyID = @CompanyId and H.SalesUserID = @DCSalesEmpID
			---ended---

		END
	END
	ELSE
	BEGIN

		DECLARE @HRTypeID INT = NULL, @StatusID  INT = NULL, @JobStatusID INT = NULL;
		DECLARE @DATE AS VARCHAR(12)
		SELECT @DATE = format(getdate(),'ddMMyyHHmmss')
		
		--SET HR type ID Based on Pay Per Hire/Pay per credit
		IF ISNULL(@IsPayPerHire,0) = 1
		BEGIN
			SET @HRTypeID = 1 --Sales HR
		END
		IF ISNULL(@IsPayPerCredit,0) = 1
		BEGIN
			--some values are default in pay per credit HR
			SET @DpPercentage = 0;
			SET @NRMargin = 0;

			IF @IsPostaJob = 1 AND @IsProfileView = 0
			BEGIN
				SET	@HRTypeId = 4; --PostaJobCreditBased								
			END
			IF @IsPostaJob = 1 AND @IsProfileView = 1
			BEGIN
				SET	@HRTypeId = 3; --PostaJobViewBasedCredit									
			END
			IF @IsPostaJob = 0 AND @IsProfileView = 1
			BEGIN
				SET	@HRTypeId = 6; --PostajobWithViewCreditsButnoJobPostCredits									
			END
		END

		SET @JobStatusID = 3	--Draft by default on 1st tab

		---Convert into proper formatting for budget (Riya - 20 May-2024)
		SELECT @FormattedBudgetFrom = format(@MinimumBudget, N'N', Culture), 
			   @FormattedBudgetTo = format(@MaximumBudget, N'N', Culture),
			   @FormattedAdhocBudget = format(@AdhocBudgetCost, N'N', Culture)
		FROM prg_CurrencyExchangeRate WITH(NOLOCK) WHERE CurrencyCode = @Currency

		--Range budget
		IF(ISNULL(@MinimumBudget,0) >  0 AND ISNULL(@MaximumBudget,0) > 0)
		BEGIN
			SET @Cost = (
							SELECT IIF
									(
										ISNULL(@MinimumBudget,0) = 0, 
										@FormattedBudgetFrom,
										CAST(CurrencySign AS NVARCHAR) + ' ' + @FormattedBudgetFrom
									) 
									+ ' to ' +  CAST(CurrencySign AS NVARCHAR) + ' ' + @FormattedBudgetTo + ' ' + @Currency FROM prg_CurrencyExchangeRate with(NoLock) WHERE CurrencyCode = @Currency
						)
						SET	@AdhocBudgetCost = 0
		END
		--Adhoc Budget
		IF(ISNULL(@AdhocBudgetCost,0) >  0)
		BEGIN
			SET @Cost = (
							SELECT IIF
									(
										ISNULL(@AdhocBudgetCost,0) = 0, 
										@FormattedAdhocBudget,
										CAST(CurrencySign AS NVARCHAR) + ' ' + @FormattedAdhocBudget + ' ' + @Currency + ' / Month' 										
									) 
									FROM prg_CurrencyExchangeRate with(NoLock) WHERE CurrencyCode = @Currency
						)
			SET @MinimumBudget = 0 
			SET	@MaximumBudget = 0 
		END

		INSERT INTO gen_SalesHiringRequest
		(
				ContactId,
				NoofTalents,
				IsHiringLimited,
				Availability,
				IsActive,
				IsHRTypeDP,
				DPPercentage,
				TalentCostCalcPercentage,
				CreatedByID,
				CreatedByDatetime,
				SalesUserID,
				Status_ID,
				HR_Number,
				JDFilename,
				JDURL,
				JDDump_ID,
				HRTypeId,
				HiringTypePricingId,
				IsTransparentPricing,
				PayrollTypeId,
				HR_TypeId,
				PayrollPartnerName,
				IsDirectHR,
				IsVettedProfile,
				DealID,
				BQLink,
				Discovery_Call,
				JobStatusID
		)
		Values
		(
				@ContactId,
				@NoOfTalents,
				@IsHiringLimited,
				@Availability,
				0, -- IsActive = 0 while create HR on 1st page
				@IsHRTypeDP,
				@DpPercentage,
				@NRMargin,
				@LoginUserId,
				GETDATE(),
				@SalesPersonID, 
				1, -- Default open sub status while create HR on 1st page
				'HR'+ @DATE,
				@JDFileName,
				@JDURL,
				@JddumpId,
				@HRTypeId,
				@HrTypePricingId,
				@IsTransparentPricing,
				@PayrollTypeId,
				IIF(@IsHRTypeDP = 1, 1, 2),
				@PayrollPartnerName,
				@IsDirectHR,
				@IsVettedProfile, 
				@DealID,
				@BQLink,
				@Discovery_Call,
				@JobStatusId
		)

		SET @HrId = @@IDENTITY 
		SELECT @HrId AS Id --Do not changed/move, otherwise will impact on create hr

		INSERT INTO gen_SalesHiringRequest_Details
		(
			HiringRequest_ID,
			YearOfExp,
			NoofEmployee,
			Currency,
			BudgetFrom,
			BudgetTo,
			HowSoon, 
			Timezone_Preference_ID, 
			TimeZone_FromTime,
			TimeZone_EndTime,	
			RoleStatus_ID,			
			SpecificMonth,
			Cost,
			CreatedById,
			CreatedByDatetime,
			DurationType,
			OverlapingHours,
			Timezone_ID,
			Adhoc_BudgetCost,
			IsConfidentialBudget,
			IsFresherAllowed		
			)
		VALUES
		(
			@HrId,
			@YearOfExp,
			@NoOfTalents,
			@Currency,
			@MinimumBudget,
			@MaximumBudget,
			@HowSoon,
			NULL,
			@TimeZoneFromTime, 
			@TimeZoneEndTime,
			1, 
			@ContractDuration,
			@Cost,
			@LoginUserId,
			Getdate(),
			IIF(@IsHiringLimited = 1, 'Short Term', 'Long Term'),
			@OverlapingHours,
			@TimezoneId,
			@AdhocBudgetCost,
			@IsConfidentialBudget,
			@IsFresherAllowed
		)

		SET @HRDetailID = @@IDENTITY 

		INSERT INTO gen_Direct_Placement
		(
			HiringRequest_Id,
			ModeOfWork,
			City,
			Country,
			CreatedById,
			CreatedByDateTime
		)
		VALUES
		(
			@HrId,
			@ModeOfWorking,
			@City,
			@Country,
			@LoginUserId,
			GETDATE()
		)

		--added by himani for Update calculation
		EXEC SP_PayPerHire_Calculation_Update @HRID = @HrId

		--Insert history maintain
		EXEC SPROC_Gen_SalesHiringRequest_History @HRID = @HrId, @AppActionDoneBy = 3
		EXEC SPROC_Gen_SalesHiringRequest_Details_History @HRID = @HrId, @AppActionDoneBy = 3

		

		--Added history
		IF ISNULL(@IsDraftHR,0) = 1 
		BEGIN

			EXEC sproc_HiringRequest_History_Insert @Action = 'HR_SaveAsDraft', @HiringRequest_ID = @HrId,
					@Talent_ID = 0, @Created_From = 0,@CreatedById = @LoginUserId, @ContactTalentPriority_ID = 0, @InterviewMaster_ID = 0,
					@HR_AcceptedDateTime = '', @OnBoard_ID = 0, @IsManagedByClient = 0 , @IsManagedByTalent = 0, @SalesUserID = @SalesPersonID,
					@OldSalesUserID = 0, @AppActionDoneBy = 3
		END
		ELSE
		BEGIN

			EXEC sproc_HiringRequest_History_Insert @Action = 'Create_HR', @HiringRequest_ID = @HrId,
					@Talent_ID = 0, @Created_From = 0,@CreatedById = @LoginUserId, @ContactTalentPriority_ID = 0, @InterviewMaster_ID = 0,
					@HR_AcceptedDateTime = '', @OnBoard_ID = 0,@IsManagedByClient = 0 ,@IsManagedByTalent = 0, @SalesUserID = @SalesPersonID,
					@OldSalesUserID = 0, @AppActionDoneBy = 3	
		END
		
	END


	--Update Data in Gen_Company if its pay per hire HR
	IF @IsPayPerHire = 1
	BEGIN

		
		SET @CompanyID = (SELECT CompanyID FROM gen_Contact WITH (NOLOCK) WHERE ID = @ContactId)
		
		SELECT 
		@ComapnyTransparentPricing = IsTransparentPricing,
		@CompanyDiscoveryCall = Discovery_Call
		FROM gen_company WITH (NOLOCK) 
		WHERE ID = @CompanyID

			--Update Discovery_Call in company, only when its not updated yet
			IF @CompanyDiscoveryCall IS NULL AND ISNULL(@Discovery_Call,'') <> ''
			BEGIN
				UPDATE Gen_Company
				SET Discovery_Call = @Discovery_Call
				WHERE ID = @CompanyID
			END

			--IsTransparentPricing update in company, only when its not updated yet
			IF @ComapnyTransparentPricing IS NULL AND @IsTransparentPricing IS NOT NULL
			BEGIN
				UPDATE Gen_Company
				SET IsTransparentPricing = @IsTransparentPricing
				WHERE ID = @CompanyID
			END
	END

	--Special Edit by COE team
	IF ISNULL(@AllowSpecialEdit, 0) = 1 AND ISNULL(@HrId,0) > 0 AND ISNULL(@HRDetailID,0) > 0
	BEGIN
		EXEC Sproc_Update_HRDetails_From_Special_User @HR_ID = @HrId, @HRDetail_ID = @HRDetailID, @LoggedInUserID = @LoginUserId
	
		EXEC sproc_HiringRequest_History_Insert @Action = 'EditHR_COETeam', @HiringRequest_ID = @HrId,
					@Talent_ID = 0, @Created_From = 0,@CreatedById = @LoginUserId, @ContactTalentPriority_ID = 0, @InterviewMaster_ID = 0,
					@HR_AcceptedDateTime = '', @OnBoard_ID = 0,@IsManagedByClient = 0 ,@IsManagedByTalent = 0, @SalesUserID = @SalesPersonID,
					@OldSalesUserID = 0, @AppActionDoneBy = 3
	END

	---Added by himani for when HR is add/edit, contact & company will be active if its not yet(2nd May 2024)
	EXEC Sproc_UTS_ContactAsActive @ContactID = @ContactId

	--HR Partnership Update Info
	IF EXISTS (SELECT TOP 1 ID FROM usr_user WITH (NOLOCK) WHERE Id = @SalesPersonID AND IsPartnerUser = 1)
	BEGIN
		EXEC sproc_UTS_UpdatePartnershipDetails_ForHR 
				@HRID				=		@HrId,
				@CONTACTID			=       @ContactId,
				@LOGGEDINUSERID		=		@LoginUserId,
				@CHILDCOMPANYNAME	=		@ChildCompanyName
	END

	

END


GO

