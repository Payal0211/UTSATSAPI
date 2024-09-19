CREATE PROCEDURE [dbo].[Sproc_UTS_GetSalesPerson]
AS
BEGIN
    SELECT 
        usr.Id AS UserID,
        usr.FullName AS UserName
    FROM 
        usr_User usr
    INNER JOIN 
        Usr_UserType usrtype ON usr.UserTypeID = usrtype.ID
    WHERE 
        usr.UserTypeID in (4,9,11) --also need BDR Users as dicussion UTS-8406
		AND usr.IsActive = 1
	ORDER BY usr.FullName ASC
END;







