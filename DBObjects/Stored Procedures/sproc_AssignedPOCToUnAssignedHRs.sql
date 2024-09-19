ALTER PROCEDURE [dbo].[sproc_AssignedPOCToUnAssignedHRs]
  @HRID				bigint = 0,
  @POCID			bigint = 0,
  @LoginUserId		bigint = 0
AS
BEGIN
			
			IF  EXISTS(Select 1 from gen_SalesHiringRequest WITH(NOLOCK) where ID = @HRID and Isnull(SalesUserID,0) = 0)
				BEGIN
						UPDATE   gen_SalesHiringRequest
						SET		 SalesUserID = IsNULL(@POCID,SalesUserID),
								 LastModifiedByID = @LoginUserId,
								 LastModifiedDatetime = GETDATE()
						where    ID = @HRID and Isnull(SalesUserID,0) = 0

						EXEC   sproc_HiringRequest_History_Insert @Action = 'Assigned_POC', @HiringRequest_ID = @HrId,
								@Talent_ID = 0, @Created_From = 0,@CreatedById = @LoginUserId, @ContactTalentPriority_ID = 0, @InterviewMaster_ID = 0,
								@HR_AcceptedDateTime = '', @OnBoard_ID = 0,@IsManagedByClient = 0 ,@IsManagedByTalent = 0, @SalesUserID = @POCID,
								@OldSalesUserID = 0, @AppActionDoneBy = 3	
				END

END