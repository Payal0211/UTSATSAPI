
CREATE PROCEDURE [Dbo].[Sproc_Get_SalesUserWithHead_FOr_HiringRequest]
  @HRID				bigint =0
AS
BEGIN
		
		IF OBJECT_ID('tempdb..#HRListClientWise') IS NOT NULL
			DROP TABLE #HRListClientWise

		CREATE TABLE #HRListClientWise
		(
			ClientID		  bigint,
			HRID			  bigint,
			SalesUserID		  bigint default 0,
			SalesUserHeadID   bigint default 0,
			SalesUserEmail	  Nvarchar(200) default '',
			SalesUserHeadEmail Nvarchar(200) default '',
			GSpaceID		  Nvarchar(MAX) default '',
			TokenObject		  Nvarchar(MAx) default ''
		)

		INSERT INTO #HRListClientWise(ClientID,HRID,SalesUserID)
		SELECT ContactID,ID,SalesUserID
		FROM   gen_SalesHiringRequest WITH(NOLOCK)  
		WHERE  ID = @HRID


		UPDATE  H
		SET     H.SalesUserEmail = U.EmailID
		FROM    #HRListClientWise H
				inner join usr_User U WITH(NOLOCK) ON U.Id = H.SalesUserID

		UPDATE  H
		SET     H.SalesUserHeadID = UH.ParentID
		FROM    #HRListClientWise H
				inner join usr_User U WITH(NOLOCK) ON U.Id = H.SalesUserID
				inner join usr_UserHierarchy UH WITH(NOLOCK)  ON U.ID = UH.UserID

		UPDATE  H
		SET     H.SalesUserHeadEmail = U.EmailID
		FROM    #HRListClientWise H
				inner join usr_User U WITH(NOLOCK) ON U.Id = H.SalesUserHeadID

		UPDATE   H
		SET      H.GSpaceID = Isnull(Co.GSpaceID,''),
				 H.TokenObject = Isnull(Co.GspaceTokenObject,'')
		FROM     #HRListClientWise H
				 inner join gen_Contact C WITH(NOLOCK) ON H.ClientID = C.ID	
				 inner join gen_Company CO WITH(NOLOCK) ON CO.ID = C.CompanyID

		select * from #HRListClientWise
END