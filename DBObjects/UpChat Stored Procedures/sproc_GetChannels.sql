ALTER PROCEDURE [dbo].[sproc_GetChannels] 
@HRId INT NULL = 0 
AS
BEGIN
IF @HRId > 0
	BEGIN
		SELECT		[HRID] = ISNULL(H.ID,0),
					[CompanyInitial] = dbo.Fun_GetInitials(ISNULL(COM.Company,'')),
					[CompanyName] = ISNULL(COM.Company,''),
					[Role] = ISNULL(H.RequestForTalent,''),
					[HRNumber] = ISNULL(H.HR_Number,''),
					[HRStatus] = ISNULL(HRS.HiringRequest_Status,''),
					[HRStatusID]= ISNULL(HRS.ID,0) ,
					[IsPinned] = cast(0 as bit),
					[IsSnoozed] = cast(0 as bit)
		FROM		
					gen_SalesHiringRequest H with(nolock) 
					INNER JOIN prg_HiringRequestStatus HRS with(nolock) on H.Status_ID = HRS.ID
					INNER JOIN gen_Contact CO with(nolock) on H.ContactID = CO.ID
					INNER JOIN gen_Company COM with(nolock) on CO.CompanyID = COM.ID
					where H.ID = @HRId and H.IsActive = 1
		ORDER BY 	H.LastModifiedDatetime DESC
	END
ELSE
	BEGIN
		SELECT		[HRID] = ISNULL(H.ID,0),
					[CompanyInitial] = dbo.Fun_GetInitials(ISNULL(COM.Company,'')),
					[CompanyName] = ISNULL(COM.Company,''),
					[Role] = ISNULL(H.RequestForTalent,''),
					[HRNumber] = ISNULL(H.HR_Number,''),
					[HRStatus] = ISNULL(HRS.HiringRequest_Status,''),
					[HRStatusID]=ISNULL(HRS.ID,0) ,
					[IsPinned] = cast(0 as bit),
					[IsSnoozed] = cast(0 as bit)
		FROM		
					gen_SalesHiringRequest H with(nolock)
					INNER JOIN gen_SalesHiringRequest_Details HD WITH(NOLOCK) ON H.ID = HD.HiringRequest_ID
					INNER JOIN prg_HiringRequestStatus HRS with(nolock) on H.Status_ID = HRS.ID
					INNER JOIN gen_Contact CO with(nolock) on H.ContactID = CO.ID
					INNER JOIN gen_Company COM with(nolock) on CO.CompanyID = COM.ID
					where CAST(H.CreatedByDateTime AS DATE) > '2023-08-01' AND H.IsActive = 1 AND H.Status_ID IN (1,2)
		ORDER BY 	H.LastModifiedDatetime DESC
	END
END