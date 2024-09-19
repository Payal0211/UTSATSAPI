CREATE PROCEDURE [dbo].[sp_UTS_get_dynamic_CTA]
    @HRID Bigint = 0,
    @TalentID Bigint = 0,
    @IsPayPerCredit BIT = 0,
    @IsPayPerHire BIT = 0,
    @Is_HRTypeDP BIT = 0
AS
BEGIN
    DECLARE @HRStatusID INT = 0, @HRRoleStatusID INT = 0;

			-- Create temporary table for interview details
		IF OBJECT_ID('tempdb..#TempInterviewDetails') IS NOT NULL
			DROP TABLE #TempInterviewDetails;

		SELECT  IMasterID = MAX(IntM.ID),
				IntM.HiringRequest_ID,
				IntM.Talent_ID,
				MAX(IntM.InterviewRound_Count) AS InterviewRound_Count
		INTO    #TempInterviewDetails
		FROM    gen_InterviewSlotsMaster IntM WITH (NOLOCK)
		WHERE   IntM.HiringRequest_ID = @HRID AND IntM.Talent_ID = @TalentID
		GROUP BY IntM.HiringRequest_ID, IntM.Talent_ID;

		-- Create temporary table for onboard details
		IF OBJECT_ID('tempdb..#TempOnboardDetails') IS NOT NULL
			DROP TABLE #TempOnboardDetails;

		SELECT  IMaxOnboardTalentID = MAX(OBT.ID),
				OBT.HiringRequest_ID,
				OBT.Talent_ID
		INTO    #TempOnboardDetails
		FROM    gen_OnBoardTalents OBT WITH (NOLOCK)
		WHERE   OBT.HiringRequest_ID = @HRID AND OBT.Talent_ID = @TalentID
		GROUP BY OBT.HiringRequest_ID, OBT.Talent_ID;


    -- Assuming you have a table for HRDetails and TalentDetails
    SELECT @HRStatusID = H.Status_ID,
           @HRRoleStatusID = HD.RoleStatus_ID
    FROM gen_SalesHiringRequest_Details HD WITH(NOLOCK)
    INNER JOIN gen_SalesHiringRequest H WITH(NOLOCK) ON H.ID = HD.HiringRequest_ID
    WHERE H.ID = @HRID;

    WITH CTE_PayPerCredit AS (
        SELECT 
            CTP.TalentID,
            'TalentStatus' AS CTAType,
            'Talent Status' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        WHERE @IsPayPerCredit = 1 AND @HRRoleStatusID != 5
		and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_InterviewStatus AS (
        SELECT 
            CTP.TalentID,
            'InterviewStatus' AS CTAType,
            'Interview Status' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP  WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_TalentSelected_InterviewDetails TI  WITH(NOLOCK) ON TI.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND @HRRoleStatusID != 5
          AND @HRStatusID != 3
          AND TI.Id > 0
          AND CTP.TalentStatusID_BasedOnHR NOT IN (5, 7)
          AND TI.Status_ID NOT IN (6, 7, 3)
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_RescheduleInterview AS (
        SELECT 
            CTP.TalentID,
            'RescheduleInterview' AS CTAType,
            'Reschedule Interview' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND @HRRoleStatusID != 5
          AND @HRStatusID != 3
          AND TI.Status_ID NOT IN (0, 3, 5, 6, 7)
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_ConfirmSlot AS (
        SELECT 
            CTP.TalentID,
            'ConfirmSlot' AS CTAType,
            'Confirm Slot' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND @HRRoleStatusID != 5
          AND @HRStatusID != 3
          AND TI.Status_ID = 1
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_SubmitClientFeedback AS (
        SELECT 
            CTP.TalentID,
            'SubmitClientFeedback' AS CTAType,
            'Submit Client Feedback' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND @HRRoleStatusID != 5
          AND @HRStatusID != 3
          AND TI.Status_ID = 6
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_ReleaseOfferDetails AS (
        SELECT 
            CTP.TalentID,
            CASE 
                WHEN @Is_HRTypeDP = 1 THEN 'ReleaseOfferDetails'
                ELSE 'ConfirmContractDetails'
            END AS CTAType,
            CASE 
                WHEN @Is_HRTypeDP = 1 THEN 'Release Offer Details'
                ELSE 'Confirm Contract Details'
            END AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_OnBoardTalents O WITH(NOLOCK) ON O.Talent_ID = CTP.TalentID
        INNER JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND @HRRoleStatusID != 5
          AND O.ID > 0
          AND O.Joiningdate = ''
          AND O.ClientOnBoarding_StatusID NOT IN (2, 7, 8)
          AND TI.Status_ID = 7
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_UpdateTalentOnBoardStatus AS (
        SELECT 
            CTP.TalentID,
            'UpdateTalentOnBoardStatus' AS CTAType,
            'Update Talent On Board Status' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_OnBoardTalents O WITH(NOLOCK) ON O.Talent_ID = CTP.TalentID
        INNER JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND @HRRoleStatusID != 5
          AND O.ID > 0
          AND O.ClientOnBoarding_StatusID = 2
          AND O.TalentOnBoarding_StatusID NOT IN (2, 7)
          AND TI.Status_ID = 7
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_UpdateLegalClientOnBoardStatus AS (
        SELECT 
            CTP.TalentID,
            'UpdateLegalClientOnBoardStatus' AS CTAType,
            'Update Legal Client On Board Status' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_OnBoardTalents O WITH(NOLOCK) ON O.Talent_ID = CTP.TalentID		
		INNER JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND @HRRoleStatusID != 5
          AND O.ID > 0
          AND O.ClientOnBoarding_StatusID = 2
          AND O.TSC_PersonID = 0
          AND O.ClientLegal_StatusID = 2
          AND O.TalentLegal_StatusID = 2
          AND TI.Status_ID = 7
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_TSC_Assignment AS (
        SELECT 
            CTP.TalentID,
            'TSCAssignment' AS CTAType,
            'TSC Assignment' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_OnBoardTalents O WITH(NOLOCK) ON O.Talent_ID = CTP.TalentID
		INNER JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND @HRRoleStatusID != 5
          AND O.ID > 0
          AND O.ClientOnBoarding_StatusID = 2
          AND O.TSC_PersonID = 0
          AND O.ClientLegal_StatusID = 2
          AND O.TalentLegal_StatusID = 2
          AND TI.Status_ID = 7
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_RejectTalent AS (
        SELECT 
            CTP.TalentID,
            'RejectTalent' AS CTAType,
            'Reject Talent' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
		INNER JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND CTP.TalentStatusID_BasedOnHR NOT IN (10, 5, 7, 8, 4)
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_CancelEngagement AS (
        SELECT 
            CTP.TalentID,
            'CancelEngagement' AS CTAType,
            'Cancel Engagement' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_OnBoardTalents O WITH(NOLOCK) ON O.Talent_ID = CTP.TalentID		
		LEFT JOIN gen_OnBoardClientContractDetails OC WITH (NOLOCK) ON OC.OnBoardID = O.ID
        WHERE @IsPayPerHire = 1
          AND CAST(
              CASE 
                  WHEN CTP.TalentStatusID_BasedOnHR = 10 AND O.ClientClosureDate IS NOT NULL AND CAST(GETDATE() AS DATE) < ISNULL(CAST(ISNULL(O.Joiningdate, OC.ContractStartDate) AS DATE), CAST(GETDATE() AS DATE)) THEN 1
                  WHEN CTP.TalentStatusID_BasedOnHR = 4 THEN 1
                  ELSE 0
              END
              AS INT
          ) = 1
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_ViewEngagement AS (
        SELECT 
            CTP.TalentID,
            'ViewEngagement' AS CTAType,
            'View Engagement' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_OnBoardTalents O WITH(NOLOCK) ON O.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND CTP.TalentStatusID_BasedOnHR IN (4, 10)
          AND O.ID > 0
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_AnotherRoundInterview AS (
        SELECT 
            CTP.TalentID,
            'AnotherRoundInterview' AS CTAType,
            'Another Round Interview' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
		INNER JOIN [dbo].[gen_SalesHiringRequest] H WITH (NOLOCK) ON H.ID = CTP.HiringRequestID
        LEFT JOIN #TempInterviewDetails Tmp ON Tmp.HiringRequest_ID = H.ID AND Tmp.Talent_ID = T.ID
        LEFT JOIN gen_InterviewSlotsMaster IntM WITH (NOLOCK) ON IntM.ID = Tmp.IMasterID 
        LEFT JOIN gen_TalentSelected_InterviewDetails TI WITH (NOLOCK) ON TI.InterviewMaster_ID = IntM.ID AND TI.HiringRequest_ID = IntM.HiringRequest_ID AND TI.HiringRequest_Detail_ID = IntM.HiringRequest_Detail_ID AND TI.Talent_ID = IntM.Talent_ID
        LEFT JOIN prg_InterviewStatus INS WITH (NOLOCK) ON INS.ID = IntM.InterviewStatus_ID
        LEFT JOIN #TempOnboardDetails TempOnboard ON TempOnboard.HiringRequest_ID = H.ID AND TempOnboard.Talent_ID = T.ID
		LEFT JOIN gen_ContactInterviewFeedback CF WITH(NOLOCK) ON CF.Shortlisted_InterviewID = TI.Shortlisted_InterviewID AND CF.HiringRequest_ID = Tmp.HiringRequest_ID
		LEFT JOIN gen_TalentSelected_NextRound_InterviewDetails N WITH (NOLOCK) ON N.Shortlisted_InterviewID = TI.Shortlisted_InterviewID AND N.Talent_ID = Tmp.Talent_ID AND N.InterviewRound = IntM.InterviewRound_Count AND ISNULL(N.IsLaterSlotGiven, 0) = 1
        WHERE @IsPayPerHire = 1
          AND @HRStatusID != 3
          AND ISNULL(N.SlotGiven, '') = 'Later'
          AND ISNULL(CF.FeedBack_Type, '') = 'AnotherRound'
          AND TI.Status_ID = 7
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    ),
    CTE_ScheduleInterview AS (
        SELECT 
            CTP.TalentID,
            'ScheduleInterview' AS CTAType,
            'Schedule Interview' AS CTAName,
            1 AS IsEnabled
        FROM gen_Talent T
        INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH(NOLOCK) ON CTP.TalentID = T.ID
        INNER JOIN gen_TalentSelected_InterviewDetails TI WITH(NOLOCK) ON TI.Talent_ID = CTP.TalentID
        WHERE @IsPayPerHire = 1
          AND @HRStatusID != 3
          AND (TI.Status_ID = 0 OR TI.Status_ID = 5)
		  and T.ID = @TalentID and CTP.HiringRequestID = @HRID
    )

	select distinct * from(
    -- Combine all results
    SELECT * FROM CTE_PayPerCredit
    UNION ALL
    SELECT * FROM CTE_InterviewStatus
    UNION ALL
    SELECT * FROM CTE_RescheduleInterview
    UNION ALL
    SELECT * FROM CTE_ConfirmSlot
    UNION ALL
    SELECT * FROM CTE_SubmitClientFeedback
    UNION ALL
    SELECT * FROM CTE_ReleaseOfferDetails
    UNION ALL
    SELECT * FROM CTE_UpdateTalentOnBoardStatus
    UNION ALL
    SELECT * FROM CTE_UpdateLegalClientOnBoardStatus
    UNION ALL
    SELECT * FROM CTE_TSC_Assignment
    UNION ALL
    SELECT * FROM CTE_RejectTalent
    UNION ALL
    SELECT * FROM CTE_CancelEngagement
    UNION ALL
    SELECT * FROM CTE_ViewEngagement
    UNION ALL
    SELECT * FROM CTE_AnotherRoundInterview
    UNION ALL
    SELECT * FROM CTE_ScheduleInterview
	)Q
END;



--ALTER PROCEDURE [dbo].[sp_UTS_get_dynamic_CTA]
--    @HRID                BIGINT = 0,
--    @TalentID            BIGINT = 0,
--    @IsPayPerCredit      BIT = 0,
--    @IsPayPerHire        BIT = 0,
--    @Is_HRTypeDP         BIT = 0
--AS
--BEGIN
--    DECLARE @HRStatusID INT = 0, @HRRoleStatusID INT = 0;

--    -- Get HR Status and HR Role Status
--    SELECT @HRStatusID = H.Status_ID,
--           @HRRoleStatusID = HD.RoleStatus_ID
--    FROM   gen_SalesHiringRequest_Details HD WITH (NOLOCK)
--           INNER JOIN gen_SalesHiringRequest H WITH (NOLOCK) ON H.ID = HD.HiringRequest_ID
--    WHERE  H.ID = @HRID;

--    -- Create temporary table for interview details
--    IF OBJECT_ID('tempdb..#TempInterviewDetails') IS NOT NULL
--        DROP TABLE #TempInterviewDetails;

--    SELECT  IMasterID = MAX(IntM.ID),
--            IntM.HiringRequest_ID,
--            IntM.Talent_ID,
--            MAX(IntM.InterviewRound_Count) AS InterviewRound_Count
--    INTO    #TempInterviewDetails
--    FROM    gen_InterviewSlotsMaster IntM WITH (NOLOCK)
--    WHERE   IntM.HiringRequest_ID = @HRID AND IntM.Talent_ID = @TalentID
--    GROUP BY IntM.HiringRequest_ID, IntM.Talent_ID;

--    -- Create temporary table for onboard details
--    IF OBJECT_ID('tempdb..#TempOnboardDetails') IS NOT NULL
--        DROP TABLE #TempOnboardDetails;

--    SELECT  IMaxOnboardTalentID = MAX(OBT.ID),
--            OBT.HiringRequest_ID,
--            OBT.Talent_ID
--    INTO    #TempOnboardDetails
--    FROM    gen_OnBoardTalents OBT WITH (NOLOCK)
--    WHERE   OBT.HiringRequest_ID = @HRID AND OBT.Talent_ID = @TalentID
--    GROUP BY OBT.HiringRequest_ID, OBT.Talent_ID;

--    -- Create and populate temporary table for CTA details
--    IF OBJECT_ID('tempdb..#TalentCTAs') IS NOT NULL
--        DROP TABLE #TalentCTAs;

--    CREATE TABLE #TalentCTAs (
--        TalentID    INT,
--        CTAType     NVARCHAR(50),
--        CTAName     NVARCHAR(50),
--        IsEnabled   BIT
--    );

--    INSERT INTO #TalentCTAs (TalentID, CTAType, CTAName, IsEnabled)
--    SELECT  CTP.TalentID,
--            cta.CTAType,
--            CASE 
--                WHEN @IsPayPerCredit = 1 THEN 'Talent Status'
--                WHEN CTAType = 'InterviewStatus' THEN 'Interview Status'
--                WHEN CTAType = 'RescheduleInterview' THEN 'Reschedule Interview'
--                WHEN CTAType = 'ConfirmSlot' THEN 'Confirm Slot'
--                WHEN CTAType = 'SubmitClientFeedback' THEN 'Submit Client Feedback'
--                WHEN CTAType = 'ReleaseOfferDetails' THEN 'Release Offer Details'
--                WHEN CTAType = 'ConfirmContractDetails' THEN 'Confirm Contract Details'
--                WHEN CTAType = 'UpdateTalentOnBoardStatus' THEN 'Update Talent On Board Status'
--                WHEN CTAType = 'UpdateLegalClientOnBoardStatus' THEN 'Update Legal Client On Board Status'
--                WHEN CTAType = 'TSCAssignment' THEN 'TSC Assignment'
--                WHEN CTAType = 'RejectTalent' THEN 'Reject Talent'
--                WHEN CTAType = 'CancelEngagement' THEN 'Cancel Engagement'
--                WHEN CTAType = 'ViewEngagement' THEN 'View Engagement'
--                WHEN CTAType = 'AnotherRoundInterview' THEN 'Another Round Interview'
--                WHEN CTAType = 'ScheduleInterview' THEN 'Schedule Interview'
--                ELSE 'Talent Status'
--            END AS CTAName,
--            cta.IsEnabled
--    FROM    gen_Talent T WITH (NOLOCK)
--            INNER JOIN [dbo].gen_ContactTalentPriority CTP WITH (NOLOCK) ON CTP.TalentID = T.ID
--            INNER JOIN [dbo].[gen_SalesHiringRequest] H WITH (NOLOCK) ON H.ID = CTP.HiringRequestID
--            INNER JOIN [dbo].[gen_SalesHiringRequest_Details] HD WITH (NOLOCK) ON H.ID = HD.HiringRequest_ID
--            LEFT JOIN #TempInterviewDetails Tmp ON Tmp.HiringRequest_ID = H.ID AND Tmp.Talent_ID = T.ID
--            LEFT JOIN gen_InterviewSlotsMaster IntM WITH (NOLOCK) ON IntM.ID = Tmp.IMasterID 
--            LEFT JOIN gen_TalentSelected_InterviewDetails TI WITH (NOLOCK) ON TI.InterviewMaster_ID = IntM.ID AND TI.HiringRequest_ID = IntM.HiringRequest_ID AND TI.HiringRequest_Detail_ID = IntM.HiringRequest_Detail_ID AND TI.Talent_ID = IntM.Talent_ID
--            LEFT JOIN prg_InterviewStatus INS WITH (NOLOCK) ON INS.ID = IntM.InterviewStatus_ID
--            LEFT JOIN #TempOnboardDetails TempOnboard ON TempOnboard.HiringRequest_ID = H.ID AND TempOnboard.Talent_ID = T.ID
--            LEFT JOIN gen_OnBoardTalents O WITH (NOLOCK) ON O.ID = TempOnboard.IMaxOnboardTalentID AND O.Talent_ID = TempOnboard.Talent_ID AND O.HiringRequest_ID = TempOnboard.HiringRequest_ID
--            LEFT JOIN gen_OnBoardClientContractDetails OC WITH (NOLOCK) ON OC.OnBoardID = O.ID
--            LEFT JOIN gen_ContactInterviewFeedback CF WITH (NOLOCK) ON CF.Shortlisted_InterviewID = TI.Shortlisted_InterviewID AND CF.HiringRequest_ID = Tmp.HiringRequest_ID
--            LEFT JOIN gen_TalentSelected_NextRound_InterviewDetails N WITH (NOLOCK) ON N.Shortlisted_InterviewID = TI.Shortlisted_InterviewID AND N.Talent_ID = Tmp.Talent_ID AND N.InterviewRound = IntM.InterviewRound_Count AND ISNULL(N.IsLaterSlotGiven, 0) = 1
--    CROSS APPLY (
--        SELECT 
--            CASE 
--                -- Pay Per Credit CTA
--                WHEN @IsPayPerCredit = 1 AND @HRRoleStatusID != 5 THEN 'TalentStatus'
--                -- Pay Per Hire CTA: Interview Status
--                WHEN @IsPayPerHire = 1 AND @HRRoleStatusID != 5 AND @HRStatusID != 3 AND TI.Id > 0 AND CTP.TalentStatusID_BasedOnHR NOT IN (5, 7) AND TI.Status_ID NOT IN (6, 7, 3) THEN 'InterviewStatus'
--                -- Pay Per Hire CTA: Reschedule Interview
--                WHEN @IsPayPerHire = 1 AND @HRRoleStatusID != 5 AND @HRStatusID != 3 AND TI.Status_ID NOT IN (0, 3, 5, 6, 7) THEN 'RescheduleInterview'
--                -- Pay Per Hire CTA: Confirm Slot
--                WHEN @IsPayPerHire = 1 AND @HRRoleStatusID != 5 AND @HRStatusID != 3 AND TI.Status_ID = 1 THEN 'ConfirmSlot'
--                -- Pay Per Hire CTA: Submit Client Feedback
--                WHEN @IsPayPerHire = 1 AND @HRRoleStatusID != 5 AND @HRStatusID != 3 AND TI.Status_ID = 6 THEN 'SubmitClientFeedback'
--                -- Pay Per Hire CTA: Release Offer Details or Confirm Contract Details
--                WHEN @IsPayPerHire = 1 AND @HRRoleStatusID != 5 AND O.ID > 0 AND O.Joiningdate = '' AND O.ClientOnBoarding_StatusID NOT IN (2, 7, 8) AND TI.Status_ID = 7 THEN 
--                    CASE WHEN @Is_HRTypeDP = 1 THEN 'ReleaseOfferDetails' ELSE 'ConfirmContractDetails' END
--                -- Pay Per Hire CTA: Update Talent On Board Status
--                WHEN @IsPayPerHire = 1 AND @HRRoleStatusID != 5 AND O.ID > 0 AND O.ClientOnBoarding_StatusID = 2 AND O.TalentOnBoarding_StatusID NOT IN (2, 7) AND TI.Status_ID = 7 THEN 'UpdateTalentOnBoardStatus'
--                -- Pay Per Hire CTA: Update Legal Client On Board Status
--                WHEN @IsPayPerHire = 1 AND @HRRoleStatusID != 5 AND O.ID > 0 AND O.ClientOnBoarding_StatusID = 2 AND O.TSC_PersonID = 0 AND O.ClientLegal_StatusID = 2 AND O.TalentLegal_StatusID = 2 AND TI.Status_ID = 7 THEN 'UpdateLegalClientOnBoardStatus'
--                -- Pay Per Hire CTA: Assign TSC
--                WHEN @IsPayPerHire = 1 AND @HRRoleStatusID != 5 AND O.ID > 0 AND O.ClientOnBoarding_StatusID = 2 AND O.TSC_PersonID = 0 AND O.ClientLegal_StatusID = 2 AND O.TalentLegal_StatusID = 2 AND TI.Status_ID = 7 THEN 'TSCAssignment'
--                -- Pay Per Hire CTA: Reject Talent
--                WHEN @IsPayPerHire = 1 AND CTP.TalentStatusID_BasedOnHR NOT IN (10, 5, 7, 8, 4) THEN 'RejectTalent'
--                -- Pay Per Hire CTA: Cancel Engagement
--                WHEN @IsPayPerHire = 1 AND CAST(
--                    CASE 
--                        WHEN CTP.TalentStatusID_BasedOnHR = 10 AND O.ClientClosureDate IS NOT NULL AND CAST(GETDATE() AS DATE) < ISNULL(CAST(ISNULL(O.Joiningdate, OC.ContractStartDate) AS DATE), CAST(GETDATE() AS DATE)) THEN 1
--                        WHEN CTP.TalentStatusID_BasedOnHR = 4 THEN 1
--                        ELSE 0
--                    END
--                    AS INT
--                ) = 1 THEN 'CancelEngagement'
--                -- Pay Per Hire CTA: View Engagement
--                WHEN @IsPayPerHire = 1 AND CTP.TalentStatusID_BasedOnHR IN (4, 10) AND O.ID > 0 THEN 'ViewEngagement'
--                -- Pay Per Hire CTA: Another Round Interview
--                WHEN @IsPayPerHire = 1 AND @HRStatusID != 3 AND ISNULL(N.SlotGiven, '') = 'Later' AND ISNULL(CF.FeedBack_Type, '') = 'AnotherRound' AND TI.Status_ID = 7 THEN 'AnotherRoundInterview'
--                -- Pay Per Hire CTA: Schedule Interview
--                WHEN @IsPayPerHire = 1 AND @HRStatusID != 3 AND (TI.Status_ID = 0 OR TI.Status_ID = 5) THEN 'ScheduleInterview'
--                -- Default case
--                ELSE 'TalentStatus'
--            END AS CTAType,
            
--            1 AS IsEnabled
--    ) AS cta
--    WHERE H.ID = @HRID and CTP.TalentID = @TalentID;

--    -- Select the results
--    SELECT * FROM #TalentCTAs;
--END;
