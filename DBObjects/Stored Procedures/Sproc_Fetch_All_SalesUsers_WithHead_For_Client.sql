ALTER PROCEDURE [dbo].[Sproc_Fetch_All_SalesUsers_WithHead_For_Client]
  @ClientEmail				nvarchar(400) = null
AS
BEGIN

		DEclare @CLientID as bigint = 0
		Declare @CompanyID as Bigint = 0

		select @CompanyID = CompanyID,@CLientID = ID 
		from   gen_Contact WITH(NOLOCK) 
		where  Lower(EmailID) = Lower(@ClientEmail)


		IF OBJECT_ID('tempdb..#HRListClientWise') IS NOT NULL
			DROP TABLE #HRListClientWise

		CREATE TABLE #HRListClientWise
		(
			ClientID		  bigint,
			HRID			  bigint,
			SalesUserID		  bigint,
			SalesUserHeadID   bigint,
			SalesUserEmail	  Nvarchar(200),
			SalesUserHeadEmail Nvarchar(200)
		)
		
		INSERT INTO #HRListClientWise(ClientID,HRID,SalesUserID)
		SELECT @ClientID,ID,SalesUserID
		FROM   gen_SalesHiringRequest WITH(NOLOCK)  
		WHERE  ContactID = @ClientID

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
		SET     H.SalesUserEmail = U.EmailID
		FROM    #HRListClientWise H
				inner join usr_User U WITH(NOLOCK) ON U.Id = H.SalesUserHeadID

		select * from #HRListClientWise

		DROP TABLE #HRListClientWise
END