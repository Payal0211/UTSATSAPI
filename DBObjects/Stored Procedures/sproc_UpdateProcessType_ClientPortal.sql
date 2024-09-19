ALTER PROCEDURE [dbo].[sproc_UpdateProcessType_ClientPortal]
@ContactID BIGINT NULL,
@GUID NVARCHAR(200) NULL,
@IPAddress NVARCHAR(100) NULL,
@ProcessType NVARCHAR(100) NULL

AS
BEGIN	

		IF Exists(select 1 from gen_RoleAndHiringType_ClientPortal WITH(NOLOCK) where ContactId = @ContactID and GUID = @GUID)
			BEGIN
					UPDATE gen_RoleAndHiringType_ClientPortal
					SET	   ProcessType = @ProcessType,
						   IPAddress  = @IPAddress,
						   ModifiedByID  = @ContactID,
						   ModifiedByDateTime = Getdate(),
						   IsActive = 0
					where  ContactId = @ContactID and GUID = @GUID
					
			END

		IF Exists(select 1 from gen_SkillAndBudget_ClientPortal WITH(NOLOCK) where ContactId = @ContactID and GUID = @GUID)
			BEGIN
					UPDATE gen_SkillAndBudget_ClientPortal
					SET	   ProcessType = @ProcessType,
						   IPAddress  = @IPAddress,
						   ModifiedByID  = @ContactID,
						   ModifiedByDateTime = Getdate(),
						   IsActive = 0
					where  ContactId = @ContactID and GUID = @GUID
					
			END

		IF Exists(select 1 from gen_JobPost_Employment_Details_ClientPortal WITH(NOLOCK) where ContactId = @ContactID and GUID = @GUID)
			BEGIN
					UPDATE	gen_JobPost_Employment_Details_ClientPortal
					SET	    ProcessType = @ProcessType,
							IPAddress      = @IPAddress,
						    ModifiedByID  =  @ContactID,
						    ModifiedByDateTime = Getdate(),
						    IsActive = 0
					where   ContactId = @ContactID and GUID = @GUID
			END	

		
END

