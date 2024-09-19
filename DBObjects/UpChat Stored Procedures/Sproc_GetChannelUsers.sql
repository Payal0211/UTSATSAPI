ALTER PROCEDURE [dbo].[sproc_GetChannelUsers] 
@HRID int
AS  
BEGIN 
	
	declare @SalesUserID bigint;
	select @SalesUserID=SalesUserID from gen_SalesHiringRequest with(nolock) where ID=@HRID
	
	IF OBJECT_ID('temp_db..#UserIdList') IS NOT NULL
		DROP TABLE #UserIdList

	CREATE TABLE #UserIdList (UserID bigint);

	
	DECLARE @UserEmpID NVARCHAR(50) = '';

	SET @UserEmpID = (SELECT ISNULL(EmployeeID,'') from usr_User with(nolock) where ID = @SalesUserID);

	IF OBJECT_ID('temp_db..#UserTbl') IS NOT NULL
		DROP TABLE #UserTbl;

	CREATE TABLE #UserTbl (ID bigint, UserName nvarchar(200));

	INSERT INTO #UserTbl (ID, UserName)
		EXEC sproc_GetUserHierarchy @SalesUserID, @UserEmpID;

	
	INSERT INTO #UserIdList (UserID)
		SELECT distinct Z.UserID FROM 
		(
			SELECT ID as UserID FROM #UserTbl
			
			union all 

			select  ID as UserID
			from    usr_User with(NOlock)
			where   EmployeeID IN ('12sds','UP0001','UP0002','UP0132','UP0019','UP0012','UP1945','UP1945')
		) Z;



	SELECT 
	photoUrl = ISNULL(U.ProfilePic,''),
	userDesignation = ISNULL(U.Designation,''),
	userEmpId = ISNULL(U.EmployeeID,''),
	userInitial = dbo.Fun_GetInitials(ISNULL(U.fullname,'')),
	userName = ISNULL(U.fullname,'')
	FROM usr_User U WITH(nolock)
	INNER JOIN #UserIdList UL on UL.UserId = U.ID
	WHERE U.isActive = 1 

END
