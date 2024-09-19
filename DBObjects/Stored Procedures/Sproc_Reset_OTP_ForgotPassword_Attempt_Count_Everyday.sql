USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[Sproc_Reset_OTP_ForgotPassword_Attempt_Count_Everyday]    Script Date: 03-06-2024 11:04:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Sproc_Reset_OTP_ForgotPassword_Attempt_Count_Everyday]
AS
BEGIN
		
		UPDATE   gen_Contact
		SET		 OTPAttemptCount = 0
		WHERE    Isnull(OTPAttemptCount,0) <> 0

		UPDATE  gen_Contact
		SET		ForgotEmailAttemptCount = 0
		WHERE   Isnull(ForgotEmailAttemptCount,0) <> 0

END