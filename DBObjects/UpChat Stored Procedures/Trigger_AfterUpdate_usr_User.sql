ALTER TRIGGER dbo.Trigger_AfterUpdate_usr_User
ON usr_User
AFTER UPDATE
AS
BEGIN
    IF UPDATE(FullName) OR UPDATE(Designation) OR UPDATE(ProfilePic)
    BEGIN

		DECLARE 
		@userName NVARCHAR(400), 
		@userEmpId NVARCHAR(100), 
		@Designation NVARCHAR(100), 
		@photoUrl NVARCHAR(100), 
		@userInital NVARCHAR(10)

		SELECT TOP 1 @photoUrl = ISNULL(ProfilePic,''), 		
				@userEmpId = ISNULL(EmployeeID,''), 
				@userName = ISNULL(fullname,''), 
				@Designation = ISNULL(Designation,''),
				@userInital = dbo.Fun_GetInitials(ISNULL(fullname,''))
		FROM	inserted 

		IF EXISTS (SELECT TOP 1 * FROM gen_ChannelHistory WHERE userempid = @userEmpId)
		BEGIN
			DECLARE @APIURL NVARCHAR(MAX);			DECLARE @UpChatAPI_URL NVARCHAR(200);			Declare @Object as Int;			Declare @ResponseText as Varchar(8000);			SET @UpChatAPI_URL = N'http://3.218.6.134:9096/'

			SET @APIURL = @UpChatAPI_URL + 'User/UpdateDetail';					declare @body varchar(max)					Set @body='{								"photoUrl": "'+@photoUrl+'",								"userDesignation": "'+@Designation+'",								"userEmpId": "'+@userEmpId+'",								"userName":"'+@userName +'",								"userInitial":"'+@userInital+'"								}'
        
			Exec sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;			Exec sp_OAMethod @Object, 'open', NULL, 'post', @APIURL,'false'			Exec sp_OAMethod @Object, 'setRequestHeader',null,'X-API-KEY','QXBpS2V5TWlkZGxld2FyZQ=='			EXEC sp_OAMethod @Object, 'setRequestHeader', null, 'Content-Type', 'application/json'			Exec sp_OAMethod @Object, 'send', null, @body			Exec sp_OAMethod @Object, 'responseText', @ResponseText OUTPUT			Exec sp_OADestroy @Object

			INSERT INTO StoreAPIURL(APIUrl, ResponseStatus)			VALUES(@APIURL + @body, @ResponseText)
		END
    END
END;