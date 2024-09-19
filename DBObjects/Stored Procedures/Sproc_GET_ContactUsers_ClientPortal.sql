ALTER PROCEDURE [dbo].[Sproc_GET_ContactUsers_ClientPortal]
  @CompanyID		bigint = 0,
  @Search			NVARCHAR(250) = NULL,
  @RoleID			BIGINT = 0
AS
BEGIN
	
	DECLARE @Users TABLE
	(
		FirstName NVARCHAR(250),
		LastName NVARCHAR(250),
		FullName NVARCHAR(500),
		EmailID NVARCHAR(250),
		ContactNumber NVARCHAR(250),
		Department NVARCHAR(250),
		Designation NVARCHAR(250),
		IsActive BIT,
		ID BIGINT,
		OrganizationRole NVARCHAR(250),
		Access NVARCHAR(250),
		RoleID INT,
		LastActivityDate NVARCHAR(250),
		IsMainUser BIT
	)

	INSERT INTO @Users
	SELECT * FROM 
	(
		--SELECT  FirstName = ISNULL(FirstName,''),
		--		LastName = ISNULL(LastName,''),
		--		FullName = ISNULL(FirstName,'') + ISNULL(LastName,''),
		--		EmailID = ISNULL(EmailID,''),
		--		ContactNumber = ISNULL(ContactNumber,''),
		--		Department = Isnull(Department,''),
		--		Designation = ISNULL(Designation,''),
		--		IsActive = ISNULL(CU.IsActive,0),
		--		ID = CU.ID,
		--		OrganizationRole = UR.[ROLE],
		--		Access = CASE WHEN CU.RoleID = 1 THEN 'All Jobs + Billing'
		--					  WHEN CU.RoleID = 2 THEN 'All Jobs'
		--					  ELSE 'Own job posts only'
		--				 END,
		--		RoleID = ISNULL(CU.RoleID,0),
		--		LastActivityDate = Convert(Nvarchar(15),ISNULL(CU.ModifiedDateTime, CU.CreatedByDateTime),103),
		--		IsMainUser = CASE WHEN ISNULL(CU.IsActive,0) = 0 THEN 0 ELSE 1 END
		--FROM    gen_ContactUsers_ClientPortal CU WITH(NOLOCK)
		--LEFT JOIN Prg_UserRole_ClientPortal UR WITH(NOLOCK) ON CU.RoleID = UR.ID
		--WHERE   CompanyID = @CompanyID 
		----AND CU.IsActive = 1

		--UNION ALL

		SELECT  FirstName = ISNULL(FirstName,''),
				LastName = ISNULL(LastName,''),
				FullName = ISNULL(FullName,''),
				EmailID = ISNULL(EmailID,''),
				ContactNumber = ISNULL(ContactNo,''),
				Department = Isnull(Department,''),
				Designation = ISNULL(Designation,''),
				IsActive = ISNULL(CU.IsActive,0),
				ID = CU.ID,
				OrganizationRole = UR.[ROLE],
				Access = CASE WHEN CU.RoleID = 1 THEN 'All Jobs + Billing'
							  WHEN CU.RoleID = 2 THEN 'All Jobs'
							  ELSE 'Own job posts only'
						 END,
				RoleID = ISNULL(CU.RoleID,0),
				LastActivityDate = Convert(Nvarchar(15),ISNULL(CU.LastModifiedDatetime, CU.CreatedByDateTime),103),
				IsMainUser = 0
		FROM    gen_contact CU WITH(NOLOCK)	
		LEFT JOIN Prg_UserRole_ClientPortal UR WITH(NOLOCK) ON CU.RoleID = UR.ID
		WHERE   CompanyID = @CompanyID and ISNULL(CU.IsActive,0) = 0 --AND ISNULL(CU.IsJobPostUser,0) = 1 

		UNION ALL

		SELECT  FirstName = ISNULL(FirstName,''),
				LastName = ISNULL(LastName,''),
				FullName = ISNULL(FullName,''),
				EmailID = ISNULL(EmailID,''),
				ContactNumber = ISNULL(ContactNo,''),
				Department = Isnull(Department,''),
				Designation = ISNULL(Designation,''),
				IsActive = ISNULL(CU.IsActive,0),
				ID = CU.ID,
				OrganizationRole = UR.[ROLE],
				Access = CASE WHEN CU.RoleID = 1 THEN 'All Jobs + Billing'
							  WHEN CU.RoleID = 2 THEN 'All Jobs'
							  ELSE 'Own job posts only'
						 END,
				RoleID = ISNULL(CU.RoleID,0),
				LastActivityDate = Convert(Nvarchar(15),ISNULL(CU.LastModifiedDatetime, CU.CreatedByDateTime),103),
				IsMainUser = 1
		FROM    gen_contact CU WITH(NOLOCK)
		LEFT JOIN Prg_UserRole_ClientPortal UR WITH(NOLOCK) ON CU.RoleID = UR.ID
		WHERE   CompanyID = @CompanyID and CU.IsActive = 1 --AND ISNULL(CU.IsJobPostUser,0) = 0 
	) Q 
	--WHERE Q.RoleID = CASE WHEN @RoleID = 0 THEN Q.RoleID ELSE @RoleID END 
	--OR Q.EmailID LIKE '%' + ISNULL(@Search,Q.EmailID) + '%'

	IF(@RoleID > 0)
	BEGIN
		DELETE FROM @Users WHERE RoleID <> @RoleID
	END

	IF(ISNULL(@Search, '') <> '')
	BEGIN
		print 'Riya'
		DELETE FROM @Users WHERE EmailID NOT LIKE '%' + @Search + '%'
	END

	SELECT * FROM @Users

END