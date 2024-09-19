ALTER PROCEDURE [dbo].[Sproc_UpChat_Get_UserList] 
AS
BEGIN
	SELECT		'' as channelID,
				ISNULL(U.ProfilePic,'') as photoURL, 		
				U.EmployeeID AS userEmpId, 
				U.fullname AS userName, 
				ISNULL(U.Designation,'') AS userDesignation,
				dbo.Fun_GetInitials(ISNULL(U.fullname,'')) AS userInitial,
				'' as backGroudColor,
				'' as fontColor
	FROM		usr_user as U WITH(NOLOCK)
	WHERE		U.UserTypeID IN (4,5,9,10) AND U.IsActive = 1 
	ORDER BY 	U.FullName;
END