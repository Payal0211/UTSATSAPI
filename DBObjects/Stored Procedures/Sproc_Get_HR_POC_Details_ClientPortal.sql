USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[Sproc_prg_TempSkills_Insert]    Script Date: 25-09-2023 12:57:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sproc_Get_HR_POC_Details_ClientPortal]
@Guid NVARCHAR(100) NULL
AS
BEGIN

		SELECT 
		HR.ID,
		ISNULL(Z.FullName,'') as POCFullName,
	    ISNULL(Z.EmailID,'') as POCEmailID
		FROM gen_SalesHiringRequest HR 
		inner join gen_Contact C WITH(NOLOCK) ON C.ID = HR.ContactID
		left join (	select	U1.FullName,POC.ContactID,U1.EmailID 
							from	usr_User U1 with(nolock) 
									inner join gen_ContactPointofContact POC with(nolock) on U1.ID = POC.User_ID
							) Z on Z.ContactID = C.ID
		WHERE HR.[Guid] = @Guid
		
END
