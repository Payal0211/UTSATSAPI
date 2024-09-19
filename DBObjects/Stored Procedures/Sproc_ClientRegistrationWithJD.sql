ALTER PROCEDURE [dbo].[Sproc_ClientRegistrationWithJD]  
 @CompanyName		nvarchar(200)  = NULL,  
 @ClientName		nvarchar(200)  = NULL,  
 @ClientEmail		nvarchar(200)  = NULL,  
 @ClientPhoneNo		nvarchar(20)   = NULL,   
 @CountryRegion		nvarchar(100)  = NULL,  
 @ClientPassword	nvarchar(10)   = NULL
AS  
BEGIN  
     
     DECLARE @CompanyId bigint = 0  
     DECLARE @ContactId bigint = 0  
	 DECLARE @ClientFirstName as Nvarchar(200) = ''  
	 DECLARE @ClientLastName as Nvarchar(200) = ''  
	 DECLARE @ReturnMessage as Nvarchar(200) = ''  
	 DECLARE @CName as NVARCHAR(200) = '';
	 Declare @CFullName as NVARCHAR(200) = '';
	 DECLARE @CountryRegionName NVARCHAR(200) = '';

 IF(LEN(isnull(@ClientName,''))>0)  
 BEGIN  
	SELECT @CFullName = @ClientName;
	SELECT @CName = @ClientName;
	select top 1 @ClientName =  val from f_split(@ClientName,' ')   
	select @ClientFirstName = @ClientName;
	select top 1 @ClientLastName =  val from f_split(@CName,' '); 
	select @ClientLastName =  replace(@CName,@ClientFirstName,'')  
 END  
  
 --print @ClientName  
 --print @ClientLastName  
 IF(len(isnull(@CompanyName,''))= 0 AND len(isnull(@ClientEmail,'')) >0)
 BEGIN

		DECLARE @ClientEmailCompany NVARCHAR(500) =  SUBSTRING(
										isnull(@ClientEmail,''),
										CHARINDEX('@', isnull(@ClientEmail,'')),
										LEN(isnull(@ClientEmail,''))
									  )

		SET @CompanyName  = isnull(SUBSTRING(
									@ClientEmailCompany,
									CHARINDEX('@', @ClientEmailCompany) + 1,
									CHARINDEX('.', @ClientEmailCompany) - CHARINDEX('@', @ClientEmailCompany) - 1
								  ),'')

		--SET @CompanyName = isnull(@ClientEmail,'')
 END
 IF(len(isnull(@ClientPassword,''))= 0) -- AND len(isnull(@ClientPassword,'')) >0
 BEGIN
		SET @ClientPassword  = 'Uplers@123'
 END
  
 IF(len(isnull(@CompanyName,'')) >0 AND len(isnull(@ClientEmail,'')) >0)  
 BEGIN  
   --Save Company Details  
         IF(Select count(1) from gen_Company WITH(NOLOCK) WHERE ltrim(rtrim(lower(Company)))= ltrim(rtrim(lower(@CompanyName))) ) =0  
		 BEGIN
		 
				SET @CountryRegionName = (SELECT CountryRegion from prg_CountryRegion where IsActive = 1 and ID = @CountryRegion);
				if (@CountryRegionName = '')
					INSERT INTO gen_Company(Company,CreatedByDatetime,IsActive,phone,country,Location)   
					VALUES (@CompanyName,getdate(),1,@ClientPhoneNo,@CountryRegion,@CountryRegion)
				else
					INSERT INTO gen_Company(Company,CreatedByDatetime,IsActive,phone,country,Location)   
					VALUES (@CompanyName,getdate(),1,@ClientPhoneNo,@CountryRegionName,@CountryRegionName)

				  
  
				SET @CompanyId = @@IDENTITY   
				--print(@CompanyId)
				--Save Conatct Details  if Company ID >0
				IF(@CompanyId >0)  
				begin  
				 --Save Conatct Details if Company Saved  
				  IF(Select count(1) from gen_Contact WITH(NOLOCK) WHERE lower(EmailID)= lower(@ClientEmail) ) =0  
				  BEGIN  
						 -- Payal (06-10-2021) :  Talent Email ID check 
							IF(Select count(1) from gen_Talent WITH(NOLOCK) WHERE lower(EmailID)= lower(@ClientEmail) ) =0  
							BEGIN  
								INSERT INTO gen_Contact(CompanyID,FirstName,LastName,FullName,EmailID,Username,Password,ContactNo,regions,IsPrimary,CreatedByDatetime,IsActive,IsResetPassword,IsPasswordChanged,PhotoImage,IsEmailSentforFirstHR)   
								VALUES (@CompanyId,@ClientFirstName,@ClientLastName,@CFullName,@ClientEmail,@ClientEmail,@ClientPassword,@ClientPhoneNo,@CountryRegion,1,getdate(),1,1,1,'clientDefaultImage.png',1)  
  
								SET @ContactId = @@IDENTITY
							END   
							ELSE  
							BEGIN  
								SET @ReturnMessage = 'Email ID already exist.'  
							END
					      
				   END   
				   ELSE  
				   BEGIN  
					    SET @ReturnMessage = 'Client Email ID already exist.'  
				   END 
				   
					   
				END   
         END  
         ELSE  
		 BEGIN  
				SET @ReturnMessage = 'Company name already exist.'  
		 END  
        
     
 END  
 --- End of len check if

 IF(@CompanyId>0)  
 BEGIN  
   update gen_Company SET CreatedByID = @ContactId where ID = @CompanyId  
 END 
  
 IF(@ContactId>0)  
 BEGIN  
   update gen_Contact SET CreatedByID = @ContactId where ID = @ContactId  
 END 

    SELECT @ContactId  Id , @ReturnMessage AS ReturnMessage  
  
END