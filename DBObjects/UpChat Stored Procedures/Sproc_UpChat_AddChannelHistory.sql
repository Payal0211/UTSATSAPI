ALTER PROCEDURE Sproc_UpChat_AddChannelHistory
@ChannelID NVARCHAR(MAX) NULL,
@ChannelActionID INT NULL,
@CreatedByID BIGINT NULL,
@CreatedDateTime NVARCHAR(100) NULL,
@UserEmpID NVARCHAR(100) NULL,
@IsATSUser BIT NULL,
@HRID BIGINT NULL
AS
BEGIN
	IF @ChannelID IS NOT NULL AND @HRID IS NOT NULL AND @ChannelActionID IS NOT NULL AND (@UserEmpID IS NOT NULL OR @UserEmpID <> '')
	BEGIN
		INSERT INTO gen_ChannelHistory(ChannelID, ChannelActionID, CreatedByID, CreatedDateTime, UserEmpID, IsATSUser, HiringRequestID)
		values(@ChannelID, @ChannelActionID, @CreatedByID, GETUTCDATE(), @UserEmpID, @IsATSUser, @HRID)
	END
END
