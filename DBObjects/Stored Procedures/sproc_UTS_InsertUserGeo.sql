CREATE procedure sproc_UTS_InsertUserGeo
	@PWID   AS varchar(100)   = NULL,
	@UserID   AS BIGINT  = NULL
AS
BEGIN

delete from usr_UserGeoDetails  WHERE User_ID = @UserID
SELECT  @UserID AS UserID,f_split.val AS PWID INTO #PWID FROM [dbo].[f_split](@PWID, ',');

INSERT INTO usr_UserGeoDetails(User_ID,GEO_ID)
			((SELECT @UserID, P.PWID FROM  #PWID P where isnull(p.PWID,0) <> 0 ))

END