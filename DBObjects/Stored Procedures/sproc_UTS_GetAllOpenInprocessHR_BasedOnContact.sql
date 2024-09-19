USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[sp_UTS_GetAllOpenInprocessHR_BasedOnContact]    Script Date: 26-07-2023 18:04:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sproc_UTS_GetAllOpenInprocessHR_BasedOnContact]
@ContactID bigint = null,
@HR_ID bigint = null
AS
BEGIN
	
	SELECT 
		HR.ID AS HRID,
		HR.HR_Number AS HRNumber,
		HR.Status_ID AS StatusID,
		HRS.HiringRequest_Status AS HRStatus,
		HR.NoofTalents
	FROM gen_SalesHiringRequest HR LEFT JOIN prg_HiringRequestStatus HRS ON HRS.ID = HR.Status_ID 
	WHERE HR.ContactID = @ContactID AND 
		  HR.Status_ID IN (1,2) AND 
		  HR.IsActive = 1 AND
		  HR.ID NOT IN (@HR_ID)
	
END