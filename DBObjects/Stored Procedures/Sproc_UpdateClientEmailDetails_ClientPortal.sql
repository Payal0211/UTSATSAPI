USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[Sproc_prg_TempSkills_Insert]    Script Date: 25-09-2023 12:57:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sproc_UpdateClientEmailDetails_ClientPortal]
@ID BIGINT NULL
AS
BEGIN

		UPDATE gen_Contact SET IsJobPostEmailSent = 1 WHERE ID = @ID
		
END
