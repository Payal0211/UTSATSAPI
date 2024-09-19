CREATE PROCEDURE [dbo].[Sproc_Add_AWS_SES_EmailSending_Details]
	@MessageId				Nvarchar(MAX) = null,
	@EventType				Nvarchar(100) = null,
	@Client_Destination		Nvarchar(500) = null,  
	@Email_Subject			Nvarchar(MAX) = null,	
	@timestamp				DateTime  = null
AS
BEGIN


	If NOT Exists(select 1 from gen_AWS_SES_EmailSending_Details WITH(NOLOCK) where MessageId = @MessageId and Email_Subject = @Email_Subject)
		BEGIN
			Insert into gen_AWS_SES_EmailSending_Details(MessageId,EventType,Client_Destination,Email_Subject,timestamp)
			VALUES(Isnull(@MessageId,''),Isnull(@EventType,''),Isnull(@Client_Destination,''),Isnull(@Email_Subject,''),@timestamp)
		END
										  
END	