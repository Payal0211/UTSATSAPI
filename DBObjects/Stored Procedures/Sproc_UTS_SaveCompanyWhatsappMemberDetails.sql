ALTER PROCEDURE [dbo].[Sproc_UTS_SaveCompanyWhatsappMemberDetails]	@WhatsappDetailID	bigint = NULL,	@UserID Bigint = NULL,	@IsAdmin Bit = NULL,	@LoggedInUserID BIGINT = NULLASBEGIN				INSERT INTO gen_Company_WhatsappMember_Details		(			WhatsappDetailID,
			UserID,
			IsAdmin,
			CreatedByID,
			CreatedByDateTime		)		VALUES		(			@WhatsappDetailID,			@UserID,			@IsAdmin,			@LoggedInUserID,			GETDATE()		)END