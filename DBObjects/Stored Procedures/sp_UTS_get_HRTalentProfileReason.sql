ALTER PROCEDURE [dbo].[sp_UTS_get_HRTalentProfileReason]  
	@HR_ID		bigint = 0,  
	@Talent_ID	bigint = 0
AS
BEGIN

		IF OBJECT_ID('tempdb..##TalentReason') IS not null
			DROP TABLE #TalentReason

		Create TABLE #TalentReason 
		(
			RejectedReason NVARCHAR(MAX),
			OnHoldReason NVARCHAR(MAX),
			CancelledReason NVARCHAR(MAX),
			TalentOtherReason NVARCHAR(MAX),
			TalentStatusID_BasedOnHR INT,
			RejectReasonID INT,
			RejectionComments NVARCHAR(MAX),
			RejectionMessageToTalents NVARCHAR(MAX),
			RejectActualReason NVARCHAR(MAX)
		)
		
		INSERT INTO #TalentReason
		Select	 
			 
						RejectedReason = CASE 
									WHEN ISnull(CTP.RejectReasonID,0) > 0 and Isnull(CTP.TalentStatusID_BasedOnHR,0) = 7
									THEN PTRR.MainKey 
									WHEN ISnull(CTP.RejectReasonID,0) <= 0 and Isnull(CTP.TalentStatusID_BasedOnHR,0) = 7
									THEN 'Other' 
									ELSE PTRR.Reason 
								END,
						--WHEN ISnull(CTP.RejectReasonID,0) <= 0 and Isnull(CTP.TalentStatusID_BasedOnHR,0) = 7
						--					THEN 'Other'
						--					ELSE PTRR.Reason
						--				END,
						OnHoldReason =  CASE WHEN ISnull(CTP.RejectReasonID,0) <= 0 and Isnull(CTP.TalentStatusID_BasedOnHR,0) = 6
											THEN 'Other'
										ELSE PTRR.Reason
										END,
						CancelledReason = CASE WHEN Isnull(CTP.TalentStatusID_BasedOnHR,0) = 5 AND H.Status_ID IN (3, 4, 6)
												THEN 'Auto-Cancelled'
											WHEN ISnull(CTP.RejectReasonID,0) <= 0 AND  Isnull(CTP.TalentStatusID_BasedOnHR,0) = 5
											THEN 'Other'
											ELSE PTRR.Reason
										END,
						-- Get the other reason for the talent status
						TalentOtherReason = CASE 
											WHEN CTP.RejectReasonID <= 0 THEN CTP.OtherRejectReason
										END,

						---- Get the remarks for the talent status 
						--TalentRemarks = CASE
						--					WHEN TSACS.ID = 6 AND CTP.RejectReasonID <= 0 THEN CTP.OnHoldRemark
						--					WHEN CTP.RejectReasonID <= 0 AND (TSACS.ID = 5 OR TSACS.ID = 7) THEN CTP.LossRemark
						--				END,
						TalentStatusID_BasedOnHR=Isnull(CTP.TalentStatusID_BasedOnHR,0),
						RejectReasonID = CTP.RejectReasonID,
						RejectionComments = CTP.LossRemark,
						RejectionMessageToTalents = CTP.OtherRejectReason,
						RejectActualReason = PTRR.Reason 

		from	gen_Talent T WITH(NOLOCK)
				inner join gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID=T.ID
				inner join gen_SalesHiringRequest H WITH(NOLOCK) ON H.ID=CTP.HiringRequestID
				inner JOIN prg_HiringRequestStatus RS WITH(NOLOCK) ON RS.ID = H.Status_ID
				inner join gen_SalesHiringRequest_Details HD WITH(NOLOCK) ON H.ID = HD.HiringRequest_ID
				inner join prg_TalentStatus_AfterClientSelection TSACS WITH(NOLOCK)  ON  TSACS.ID = CTP.TalentStatusID_BasedOnHR
				left join  prg_TalentType TalType with(nolock) on T.Talent_TypeID = TalType.ID 
				left join  prg_TalentRejectReason PTRR WITH(NOLOCK) ON  PTRR.ID = CTP.RejectReasonID and Isnull(CTP.RejectReasonID,0) > 0 --UTS-3372: Show the Reason for changing talent status
		WHERE   H.ID = @HR_ID and CTP.TalentID = @Talent_ID 


		   --SELECT * FROM #TalentReason
	 

		   SELECT   ActualReason = CASE WHEN TR.RejectReasonID > 0 THEN
										CASE WHEN Isnull(TR.TalentStatusID_BasedOnHR,0) = 7 THEN Isnull(TR.RejectedReason,'') 
											 WHEN Isnull(TR.TalentStatusID_BasedOnHR,0) = 6 THEN Isnull(TR.OnHoldReason,'')
											 WHEN Isnull(TR.TalentStatusID_BasedOnHR,0) = 5 THEN Isnull(TR.CancelledReason,'')
										END
									ELSE Isnull(TR.TalentOtherReason,'')
									END,
					TR.RejectionComments,
					TR.RejectionMessageToTalents,
					TR.RejectActualReason
		   FROM		#TalentReason TR

		 DROP TABLE #TalentReason

END

