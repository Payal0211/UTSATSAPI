
ALTER PROCEDURE [dbo].[Sproc_Update_SpaceID_For_Client]
  @ClientEmailID			Nvarchar(400) = null,
  @SpaceID					Nvarchar(MAX) = null
AS
BEGIN
		
		DEclare @CLientID as bigint = 0
		Declare @CompanyID as Bigint = 0

		select @CompanyID = CompanyID,@CLientID = ID 
		from   gen_Contact WITH(NOLOCK) 
		where  Lower(EmailID) = Lower(@ClientEmailID)

		UPDATE gen_Company
		SET		 = @SpaceID,
			   IsGSpaceCreated = 1
		WHERE  ID = @CompanyID

END