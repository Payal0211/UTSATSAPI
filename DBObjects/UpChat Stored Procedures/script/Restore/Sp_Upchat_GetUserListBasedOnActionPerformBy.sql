CREATE PROCEDURE Sp_Upchat_GetUserListBasedOnActionPerformBy
@HRId INT NULL = 0
AS
BEGIN
	SET NOCOUNT ON;
	If @HRId > 0
	BEGIN
		SELECT 
		DISTINCT 
		U.EmployeeID as userEmpId, 
		U.FullName as userName, 
		U.Designation as userDesignation, 
		U.ProfilePic as photoURL,
		dbo.Fun_GetInitials(ISNULL(U.fullname,'')) AS userInitial, 
		'' as channelID
		from gen_history H (nolock)
		INNER JOIN usr_user U (nolock) on H.createdBYID = U.ID and U.IsActive = 1
		WHERE HiringRequest_ID = @HRId
	END
END
GO
