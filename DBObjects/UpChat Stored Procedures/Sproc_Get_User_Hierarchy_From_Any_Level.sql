CREATE PROCEDURE [DBO].[Sproc_Get_User_Hierarchy_From_Any_Level]
  @childID  bigint = 12
AS
BEGIN

		Declare @MainPArentId  as bigint
	
		;WITH RCTE AS
		(
			SELECT *, 1 AS Lvl 
			FROM	usr_UserHierarchy WITH(NOLOCK)
			WHERE	UserID = ISNULL(@childID,0) or ParentID = Isnull(@childID,0)

			UNION ALL

			SELECT	rh.*, Lvl+1 AS Lvl 
			FROM	dbo.usr_UserHierarchy rh
					INNER JOIN RCTE rc ON rh.UserID = rc.ParentId
	
		)
		SELECT TOP 1 @MainPArentId = p.id
		FROM	RCTE r
				inner JOIN dbo.usr_User p ON p.id = r.ParentId
		WHere   p.IsActive = 1
		ORDER BY lvl DESC

	

		;WITH cteHierarchy AS 
			(
				SELECT  U.ID UserID, ISNULL(h.ParentID,0) ParentID, 0 AS LEVEL_DEPTH 
				FROM    usr_User U WITH(NOLOCK)
				LEFT JOIN usr_UserHierarchy H WITH(NOLOCK) ON U.ID = H.UserID
				WHERE U.ID = Isnull(@MainPArentId,0)
		
				UNION ALL 

				SELECT	Super.UserID,Super.ParentID, 
						Sub.LEVEL_DEPTH + 1 AS LEVEL_DEPTH 
				FROM	usr_UserHierarchy AS Super WITH(NOLOCK) 
						INNER JOIN cteHierarchy AS SUB ON 
						SUB.UserID = SUPER.ParentID
				) 

				SELECT  distinct H.UserID
						--CONVERT(INT, H.ParentID) AS UNDER_PARENT,U.fullName as child,U1.FullName as parent, CONVERT(decimal(18,0),ISNULL(ut.User_Target,0))
						--UserTarget, CONVERT(decimal(18,0),ISNULL(ut.Self_Target,0)) AS SelfTarget					
				FROM	cteHierarchy H
						LEFT JOIN usr_User U WITH(NOLOCK) ON U.Id = H.UserID
						LEFT JOIN usr_User U1 WITH(NOLOCK) ON U1.Id = CONVERT(INT, H.ParentID)
						LEFT JOIN gen_Inc_UserTargets ut WITH(NOLOCK) on h.UserID = ut.UserID 
				WHERE   Isnull(U.FullName,'') <> '' 	
						and U.UserTypeID in (4,9,11,12)
				ORDER BY H.UserID ASC

END
	