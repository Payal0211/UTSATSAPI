ALTER PROCEDURE [dbo].[sproc_UTS_GetClientHappinessSurveysDetails_ClientPortal]

@ClientHappinessSurveyID BIGINT =  NULL

AS
BEGIN	
		DECLARE @ContactID BIGINT = 0,
				@OtherClientEmailID NVARCHAR(200) = 0,
				@Name NVARCHAR(600) = NULL,
				@Other_Client_Name NVARCHAR(600)=NULL,
				@EmailID NVARCHAR(400) = NULL,
				@FeedbackStatus NVARCHAR(100) = NULL

		IF EXISTS(SELECT 1 FROM [dbo].[gen_ClientHappinessSurvey] WITH(NOLOCK) WHERE ID = @ClientHappinessSurveyID)
		BEGIN
		SELECT TOP 1
			@ContactID = Contact_ID,
			@OtherClientEmailID = Other_ClientEmailID,
			@Other_Client_Name = Other_Client_Name,
			@FeedbackStatus = case when isnull(Feedback_SubmittedDate,'') <> '' then 'Completed' else 'Pending' end
			FROM [dbo].[gen_ClientHappinessSurvey] WITH(NOLOCK) WHERE ID = @ClientHappinessSurveyID
		END
		IF(ISNULL(@OtherClientEmailID,'') <> '') 
			BEGIN
				IF(ISNULL(@Other_Client_Name,'') <> '') 
					BEGIN
						Set @Name = @Other_Client_Name
					END
				ELSE
					BEGIN
						IF(ISNULL(@ContactID,0) <> 0)
						BEGIN
							IF EXISTS(SELECT 1 FROM [dbo].[gen_Contact] WITH(NOLOCK) WHERE ID = @ContactID)
							BEGIN
							SELECT 
								@Name = FullName
							FROM gen_Contact with(nolock) WHERE ID = @ContactID
							END
						END
					END
					SET @EmailID = @OtherClientEmailID
			END
		ELSE
		BEGIN
			IF(ISNULL(@ContactID,0) <> 0)
			BEGIN
				IF EXISTS(SELECT 1 FROM [dbo].[gen_Contact] WITH(NOLOCK) WHERE ID = @ContactID)
					BEGIN
						SELECT 
						@Name = FullName,
						@EmailID = EmailID
						FROM gen_Contact with(nolock) WHERE ID = @ContactID
					END
			END
		END

		SELECT @Name AS 'Name' , @EmailID AS 'EmailID',@FeedbackStatus AS FeedbackStatus
END
