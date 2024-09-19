ALTER Procedure [dbo].[Sproc_UpdateStatusForClosedHR_ClientPortal] 
@HiringRequestID		BIGINT = NULL,
@LossRemark				AS nvarchar(MAX) = NULL,
@DeletereasonId			INT = NULL,
@LoggedInUserId			INT =NULL,
@ActionDoneBy			INT =NULL,
@IsAutoExpiredJob 		BIT = 0,
@SSOUserId				BIGINT = NULL
AS
BEGIN
   
	
	DECLARE @TalentCount		INT =0  
	DECLARE @HiredTalentCount	INT =0
	DECLARE	@StatusId			INT =0
	DECLARE @HR_RoleStatus		INT =0
	DECLARE @HRTypeId  BIGINT = 0
	DECLARE @IsManagedByClient BIT = 0
	DECLARE @ActionPerformedBy BIGINT = 0,
		@ActionManagedByClient BIT = 1

	-- Set action performed by user based on SSO login UTS-7223
	SET @ActionPerformedBy =  (SELECT IIF(ISNULL(@SSOUserId,0) = 0, @LoggedInUserId, @SSOUserId))
	SET @ActionManagedByClient = (SELECT IIF(ISNULL(@SSOUserId,0) = 0, 1, 0))

	-- If HR is pay per hire then call the UTS-Admin SP.
	SELECT TOP 1 @HRTypeId = HRTypeId FROM gen_SalesHiringRequest WITH(NOLOCK) WHERE ID = @HiringRequestID
	IF(@HRTypeId = 1)
	BEGIN
		EXEC Sproc_Update_Status_For_Clsoed_HR  
						@HiringRequestID = @HiringRequestID,
						@LossRemark	= @LossRemark,
						@DeletereasonId	= @DeletereasonId,
						@LoggedInUserId	= @LoggedInUserId,
						@ActionDoneBy = @ActionDoneBy,
						@SSOUserId = @SSOUserId

		RETURN
	END

	SELECT @TalentCount = COUNT(1) FROM gen_ContactTalentPriority WITH(NOLOCK) WHERE HiringRequestID = @HiringRequestID	  
	SELECT @HiredTalentCount = COUNT(1) FROM gen_ContactTalentPriority WITH(NOLOCK) WHERE HiringRequestID = @HiringRequestID AND TalentStatusID_BasedOnHR in(4,10)	  
	
	--Payal(23-05-2024) : Change the logic for Pay per credit HR there is no relation of Hired/ offered count with TR 
	IF(@HRTypeId <> 1 AND @HiredTalentCount > 0)
	BEGIN
		SET @HiredTalentCount = @TalentCount
	END
	    
	-- We have talents and so mark the status as loss
	IF(ISNULL(@TalentCount, 0) <> 0 AND @TalentCount= @HiredTalentCount )
	BEGIN
		SET @StatusId = 3
	END
	IF(ISNULL(@TalentCount, 0) <> 0 AND @TalentCount <> @HiredTalentCount )
	BEGIN
		SET @StatusId = 6
	END
	ELSE IF(ISNULL(@TalentCount, 0) = 0)
	BEGIN
		SET @StatusId = 4
	END
	
	--Payal(05-04-2024) Fetch the role status and update the gen_SalesHiringRequest_Details table.
	BEGIN 		
			SELECT top 1
			@HR_RoleStatus = case when HS.ID = 1 THEN 2 WHEN HS.ID = 2 THEN 4 ELSE  HRS.ID END
			FROM prg_HiringRequestStatus HS WITH(NOLOCK) INNER JOIN prg_HiringRequest_RoleStatus HRS WITH(NOLOCK) ON HS.ID = HRS.HR_StatusID
				
			WHERE HS.ID = @StatusId  
	END				
	
	UPDATE gen_SalesHiringRequest SET 
		Status_ID = @StatusId , 
		LastModifiedByID = @ActionPerformedBy,
		LastModifiedDateTime = GETDATE(),
		LastActivityDate = GETDATE(),
		ClientJobModifyByID = IIF(@ActionDoneBy = 4, @LoggedInUserId, ContactID),
		ClientJobModifyByDatetime = GETDATE(),
		IsAutoExpiredJob = @IsAutoExpiredJob,
		JobExpiredORClosedDate = GETDATE(),
		JobStatusID = 2 -- Job is closed.
		WHERE ID = @HiringRequestID

	UPDATE	HD SET		
			HD.RoleStatus_ID = case when Isnull(@HR_RoleStatus,0) = 0 then 1 else @HR_RoleStatus end				
			FROM	gen_SalesHiringRequest_Details HD WITH (UPDLOCK)												
			WHERE	HD.HiringRequest_ID = @HiringRequestID;

	SELECT 'success' AS [Message]

	IF ISNULL(@ActionDoneBy,0) = 3
	BEGIN
		SET @IsManagedByClient = 0
	END
	ELSE IF(ISNULL(@ActionDoneBy,0) = 2)
	BEGIN
		SET @ActionDoneBy = 2
		SET @IsManagedByClient = 0
	END
	ELSE 
	BEGIN
		SET @ActionDoneBy = 4
		SET @IsManagedByClient = @ActionManagedByClient
	END

		EXEC sproc_HiringRequest_History_Insert @Action = 'Close_HR', @HiringRequest_ID = @HiringRequestID,
				@Talent_ID = 0, @Created_From = 0,@CreatedById = @ActionPerformedBy,@ContactTalentPriority_ID = 0,@InterviewMaster_ID = 0,
				@HR_AcceptedDateTime = '', @OnBoard_ID = 0,@IsManagedByClient = @IsManagedByClient, @IsManagedByTalent = 0, @SalesUserID = 0,
				@OldSalesUserID = 0, @AppActionDoneBy = @ActionDoneBy
		
END

