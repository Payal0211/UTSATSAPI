--SPROC_UTS_UserGeoValues 150
CREATE PROCEDURE [dbo].[SPROC_UTS_UserGeoValues]
	@UserID AS BIGINT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
    
		SELECT GeoID = (select isnull((STUFF((
						select	distinct ',' + Convert(Varchar(MAX),isnull(UG.GEO_ID,''))
						from	[dbo].[usr_UserGeoDetails] UG WITH(NOLOCK) 	
						where	UG.User_ID =U.ID
						For	XML Path('')),1,1,''
			)),'')
			)
	from [dbo].[Usr_user] U WITH(NOLOCK) 
	WHERE U.Id = @UserID

END


