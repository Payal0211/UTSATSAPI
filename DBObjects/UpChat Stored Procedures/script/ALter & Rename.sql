IF EXISTS(SELECT * FROM dbo.gen_SalesHiringRequest_Channel_Details)
exec sp_rename 'dbo.gen_SalesHiringRequest_Channel_Details', 'gen_SalesHiringRequest_Channel_UserDetails'

IF COL_LENGTH('dbo.gen_SalesHiringRequest_Channel_UserDetails', 'UserName') IS NULL
BEGIN
    ALTER TABLE dbo.gen_SalesHiringRequest_Channel_UserDetails
	ADD UserName NVARCHAR(1000);
END

IF COL_LENGTH('dbo.gen_SalesHiringRequest_Channel_UserDetails', 'UserDesignation') IS NULL
BEGIN
    ALTER TABLE dbo.gen_SalesHiringRequest_Channel_UserDetails
	ADD UserDesignation NVARCHAR(1000);
END