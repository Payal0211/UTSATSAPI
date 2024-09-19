ALTER PROCEDURE [dbo].[Sproc_UTS_Get_OnBoardClientTeamMemberDeatils]
  @OnBoard_ID		bigint = null
AS
BEGIN

		IF EXISTS(Select 1 from gen_OnBoardClientTeam with(nolock) WHERE OnBoardID = @OnBoard_ID)
		BEGIN
			SELECT DISTINCT
				ISNULL(OCT.Name,'') AS TeamName,
				ISNULL(OCT.Designation,'') AS Designation,
			    ISNULL(OCT.Email,'') AS Email,
			    ISNULL(OCT.Linkedin,'') AS Linkedin,	
			    ISNULL(OCT.ReportingTo,'') AS ReportingTo,
			    ISNULL(OCT.Buddy,'') AS Buddy
				FROM gen_OnBoardTalents OBT WITH(NOLOCK)
			    INNER JOIN gen_OnBoardClientTeam OCT WITH(NOLOCK) ON OBT.ID = OCT.OnBoardID
				WHERE OBT.ID = @OnBoard_ID;
	    END
		ELSE
		BEGIN
			SELECT		ISNULL(OCT.Name,'') AS TeamName,
						ISNULL(OCT.Designation,'') AS Designation,
						ISNULL(OCT.Email,'') AS Email,
						ISNULL(OCT.Linkedin,'') AS Linkedin,	
						ISNULL(OCT.ReportingTo,'') AS ReportingTo,
						ISNULL(OCT.Buddy,'') AS Buddy 
			FROM		gen_OnBoardClientTeam OCT WITH(NOLOCK) 
			WHERE		1 = 0
		END
END