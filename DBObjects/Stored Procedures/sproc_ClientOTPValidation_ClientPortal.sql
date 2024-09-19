ALTER PROCEDURE [dbo].[sproc_ClientOTPValidation_ClientPortal]
	@EmailId nvarchar(250) = NULL,
	@Otp nvarchar(50) = NULL
AS
BEGIN
		IF EXISTS (SELECT	1 
					FROM	gen_Contact WITH(NOLOCK) 
					WHERE IsActive =1 AND EmailID = @EmailId AND OTP = @Otp)
		BEGIN

			IF EXISTS(	SELECT	1 
					FROM	gen_Contact WITH(NOLOCK) 
					WHERE IsActive =1 AND EmailID = @EmailId AND OTP = @Otp and OTP_IsActive = 1 and 
							OTP_ExpiredDate > Getdate()
				  )
				  BEGIN

						PRINT 'Valid'

						SELECT	ID ,'Success' AS [Message]
						FROM	gen_Contact WITH(NOLOCK) 
						WHERE IsActive =1 AND EmailID = @EmailId AND OTP = @Otp and OTP_IsActive = 1 and 
							OTP_ExpiredDate > Getdate()

						Update gen_Contact SET    
						OTP_IsActive = 0,
						OTP_ExpiredDate = GETDATE(),
						IsEmailVerified = 1
						WHERE  EmailID = @EmailId	
				  END
			ELSE
			BEGIN
				IF EXISTS(	SELECT	1 
						FROM	gen_Contact WITH(NOLOCK) 
						WHERE IsActive =1 AND EmailID = @EmailId AND OTP = @Otp and OTP_IsActive = 1 and 
								OTP_ExpiredDate < Getdate()
					  )
					  BEGIN

					  PRINT 'InValid'

							SELECT	ID ,'OTP Expired' AS [Message]
							FROM	gen_Contact WITH(NOLOCK) 
							WHERE IsActive =1 AND EmailID = @EmailId AND OTP = @Otp and OTP_IsActive = 1 and 
								OTP_ExpiredDate < Getdate()
					
					  END
			END
		END
		ELSE
		BEGIN
			SELECT 0 AS ID, 'Invalid OTP' AS [Message]
		END
END
