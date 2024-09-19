CREATE PROCEDURE dbo.sp_Upchat_Insert_Into_StoreAPIURL
	@HRID BIGINT NULL,
	@ChannelID NVARCHAR(200) NULL,
	@UserEmpID NVARCHAR(200) NULL,
	@PerformAction NVARCHAR(MAX) NULL,
	@ResponseStatus NVARCHAR(MAX) NULL
AS
BEGIN
    INSERT INTO dbo.RestoreChannels(HRID,ChannelID,UserEmpID,PerformAction,ResponseStatus)
	VALUES(@HRID,@ChannelID,@UserEmpID,@PerformAction,@ResponseStatus)
END
GO

