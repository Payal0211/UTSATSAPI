ALTER PROCEDURE [dbo].[Sproc_UTS_SaveCompanyWhatsappDetails]	@CompanyID	bigint = NULL,	@GroupID NVARCHAR(MAX) = NULL,	@GroupName NVARCHAR(MAX) = NULL,	@LoggedInUserID BIGINT = NULLASBEGIN				INSERT INTO gen_Company_WhatsappDetails		(			CompanyID,
			GroupID,
			GroupName,
			CreatedByID,
			CreatedByDateTime		)		VALUES		(			@CompanyID,			@GroupID,			@GroupName,			@LoggedInUserID,			GETDATE()		)		SELECT CAST(SCOPE_Identity() AS BIGINT) AS WhatsappDetailIDEND