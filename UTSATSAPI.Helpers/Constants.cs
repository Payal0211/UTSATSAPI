namespace UTSATSAPI.Helpers
{
    public static class Constants
    {
        /// <summary>
        /// ProcConstant
        /// </summary>
        public static class ProcConstant
        {
            #region ATS-UTS sync process
            public static string Sproc_UTSAdmin_EditHrByATS = "Sproc_UTSAdmin_EditHrByATS";
            public static string Sproc_UTS_AddEdit_ATSHR = "Sproc_UTS_AddEdit_ATSHR";
            public static string Sproc_HR_EditPOC = "Sproc_HR_EditPOC";
            public static string Sproc_HR_POC_ClientPortal = "Sproc_HR_POC_ClientPortal";
            public static string Sproc_RemoveMatchmakeTalentFromUTSViaATS = "Sproc_RemoveMatchmakeTalentFromUTSViaATS";
            #endregion

            #region Company profile
            public static string Sproc_Get_CompanyDetailsFrom_Prompt = "Sproc_Get_CompanyDetailsFrom_Prompt";

            public static string Sproc_Get_Company_Funding_Details = "Sproc_Get_Company_Funding_Details";
            public static string Sproc_Add_Company_Funding_Details = "Sproc_Add_Company_Funding_Details";
            public static string Sproc_Delete_Company_Funding_Details = "Sproc_Delete_Company_Funding_Details";

            public static string Sproc_Get_Company_CultureandPerksDetails = "Sproc_Get_Company_CultureandPerksDetails";
            public static string Sproc_Add_Company_CultureandPerksDetails = "Sproc_Add_Company_CultureandPerksDetails";
            public static string Sproc_Delete_Company_CultureandPerksDetails = "Sproc_Delete_Company_CultureandPerksDetails";

            public static string Sproc_Get_Company_PerksDetails = "Sproc_Get_Company_PerksDetails";
            public static string Sproc_Add_Company_PerksDetails = "Sproc_Add_Company_PerksDetails";
            public static string Sproc_Delete_Company_PerksDetails = "Sproc_Delete_Company_PerksDetails";

            public static string Sproc_Get_Basic_CompanyDetails = "Sproc_Get_Basic_CompanyDetails";
            public static string Sproc_Update_Basic_CompanyDetails = "Sproc_Update_Basic_CompanyDetails";

            public static string Sproc_Add_YoutubeLink = "Sproc_Add_YoutubeLink";
            public static string Sproc_Get_Company_YouTubeDetails = "Sproc_Get_Company_YouTubeDetails";
            public static string Sproc_Delete_Company_YouTubeDetails = "Sproc_Delete_Company_YouTubeDetails";

            public static string sproc_UTS_GetCompanyEngagementDetails = "sproc_UTS_GetCompanyEngagementDetails";
            public static string Sproc_Update_Company_EngagementDetails = "Sproc_Update_Company_EngagementDetails";

            public static string sp_UTS_GetPOCUserIDByCompanyID = "sp_UTS_GetPOCUserIDByCompanyID";
            public static string sproc_UTS_UpdatePOCUserIDsByCompanyID = "sproc_UTS_UpdatePOCUserIDsByCompanyID";
            public static string sp_UTS_GetPOCUserIDByCompanyID_Edit = "sp_UTS_GetPOCUserIDByCompanyID_Edit";

            public static string sproc_UTS_GetContactDetails = "sproc_UTS_GetContactDetails";
            public static string sproc_UTS_UpdateContactDetails = "sproc_UTS_UpdateContactDetails";

            public static string Sproc_CompanyDetail_TransferToATS = "Sproc_CompanyDetail_TransferToATS";
            public static string sproc_Update_Company_Details_From_Scrapping = "sproc_Update_Company_Details_From_Scrapping";
            public static string Sproc_UTS_GetCompanyWhatsappDetails = "Sproc_UTS_GetCompanyWhatsappDetails";
            public static string Sproc_UTS_SaveCompanyWhatsappDetails = "Sproc_UTS_SaveCompanyWhatsappDetails";
            public static string Sproc_UTS_SaveCompanyWhatsappMemberDetails = "Sproc_UTS_SaveCompanyWhatsappMemberDetails";

            public static string Sproc_Update_Company_HiringTypePricing = "Sproc_Update_Company_HiringTypePricing";
            public static string Sproc_Get_Company_HiringTypePricing = "Sproc_Get_Company_HiringTypePricing";
            #endregion

            #region Email
            public static string Sproc_EmailHRTypeChanged = "Sproc_EmailHRTypeChanged";
            public static string Sproc_Get_Candidate_Details_For_Job = "Sproc_Get_Candidate_Details_For_Job";
            public static string Sproc_Add_Company_Transactions_With_ATS = "Sproc_Add_Company_Transactions_With_ATS";
            #endregion

            #region CommonAPI Controller SP's

            public static string Sproc_Accept_HR = "Sproc_Accept_HR";
            public static string sproc_Talent_History_Insert = "sproc_Talent_History_Insert";
            public static string sproc_HiringRequest_History_Insert = "sproc_HiringRequest_History_Insert";
            public static string sproc_gen_ContactTalentPriorityupdate = "sproc_gen_ContactTalentPriorityupdate";
            public static string Sproc_Update_HrStatus_SubStatus_ClientPortal = "Sproc_Update_HrStatus_SubStatus_ClientPortal";

            #endregion

            //#region Schedular
            //public static string Sproc_Get_ListOfHR_For_NewCandidate_Added_Email_ClientPortal = "Sproc_Get_ListOfHR_For_NewCandidate_Added_Email_ClientPortal";
            //public static string Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal = "Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal";
            //public static string Sproc_Get_Engagement_Renewal_Emails_EngagementList = "Sproc_Get_Engagement_Renewal_Emails_EngagementList";
            //public static string Sproc_Get_Engagement_Renewal_Emails_Details = "Sproc_Get_Engagement_Renewal_Emails_Details";
            //public static string Sproc_Get_Summary_Emails = "Sproc_Get_Summary_Emails";
            //public static string Sproc_Get_Nurture_Email_List = "Sproc_Get_Nurture_Email_List";
            //public static string Sproc_SelfSignUpUserWithoutJobPost_ClientPortal = "Sproc_SelfSignUpUserWithoutJobPost_ClientPortal";
            //public static string Sproc_Get_Nurture_Logs_Emails = "Sproc_Get_Nurture_Logs_Emails";
            //public static string Sproc_UpdateNurtureLogs = "Sproc_UpdateNurtureLogs";
            //public static string Sproc_Get_SchedularUpdates = "Sproc_Get_SchedularUpdates";
            //#endregion

            //#region Client
            //public static string sproc_UTS_GetClientList = "sproc_UTS_GetClientList";
            //public static string sp_UTS_GetPOCDetails = "sp_UTS_GetPOCDetails";
            //public static string sp_UTS_GetCompanyClientDetails = "sp_UTS_GetCompanyClientDetails";
            //public static string sp_UTS_GetClientWiseHRDetails = "sp_UTS_GetClientWiseHRDetails";
            //public static string Sproc_GetPointOfContact_UserDetails = "Sproc_GetPointOfContact_UserDetails";
            //public static string sproc_GetClientHappinessSurvey = "sproc_GetClientHappinessSurvey";
            //public static string sproc_UTS_GetAutoCompleteCompanies = "sproc_UTS_GetAutoCompleteCompanies";
            //public static string sproc_GetClientHappinessSurveyOptions = "sproc_GetClientHappinessSurveyOptions";
            //public static string sproc_GetClientHappynessSurveyFeedbackDetailForEmail = "sproc_GetClientHappynessSurveyFeedbackDetailForEmail";
            //public static string sproc_UTS_GetAutoCompleteHubSpotCompanies = "sproc_UTS_GetAutoCompleteHubSpotCompanies";
            //public static string sproc_UTS_GetCompanyDetails = "sproc_UTS_GetCompanyDetails";

            //public static string sproc_UTS_GetCompanyDetailsForEdit = "sproc_UTS_GetCompanyDetailsForEditClient";
            //public static string Sproc_AM_Change_For_Company = "Sproc_AM_Change_For_Company";
            //public static string Sp_UTS_PreviewJobPost_ClientPortal = "sp_UTS_PreviewJobPost_ClientPortal";
            //public static string Sproc_UTS_GetAddClientAccess = "Sproc_UTS_GetAddClientAccess";
            //public static string Sproc_GetActiveSalesUserList = "Sproc_GetActiveSalesUserList";
            //public static string Sproc_Get_CreditWithAmount = "Sproc_Get_CreditWithAmount";
            //public static string Sproc_Fetch_All_SalesUsers_WithHead_For_Client = "Sproc_Fetch_All_SalesUsers_WithHead_For_Client";
            //public static string Sproc_Update_SpaceID_For_Client = "Sproc_Update_SpaceID_For_Client";
            //public static string Sproc_Get_SalesUserWithHead_FOr_HiringRequest = "Sproc_Get_SalesUserWithHead_FOr_HiringRequest";
            //public static string Sproc_Get_Credit_Transaction_CompanyWise = "Sproc_Get_Credit_Transaction_CompanyWise";
            //public static string Sproc_Reset_AllHR_TalentStatus = "Sproc_Reset_AllHR_TalentStatus";
            //public static string Sproc_Add_AWS_Email_Payload = "Sproc_Add_AWS_Email_Payload";
            //public static string Sproc_Add_AWS_SES_EmailTracking = "Sproc_Add_AWS_SES_EmailTracking";
            //public static string Sproc_Add_AWS_SES_EmailSending_Details = "Sproc_Add_AWS_SES_EmailSending_Details";
            //#endregion

            //#region Company profile
            //public static string Sproc_Get_CompanyDetailsFrom_Prompt = "Sproc_Get_CompanyDetailsFrom_Prompt";

            //public static string Sproc_Get_Company_Funding_Details = "Sproc_Get_Company_Funding_Details";
            //public static string Sproc_Add_Company_Funding_Details = "Sproc_Add_Company_Funding_Details";
            //public static string Sproc_Delete_Company_Funding_Details = "Sproc_Delete_Company_Funding_Details";

            //public static string Sproc_Get_Company_CultureandPerksDetails = "Sproc_Get_Company_CultureandPerksDetails";
            //public static string Sproc_Add_Company_CultureandPerksDetails = "Sproc_Add_Company_CultureandPerksDetails";
            //public static string Sproc_Delete_Company_CultureandPerksDetails = "Sproc_Delete_Company_CultureandPerksDetails";

            //public static string Sproc_Get_Company_PerksDetails = "Sproc_Get_Company_PerksDetails";
            //public static string Sproc_Add_Company_PerksDetails = "Sproc_Add_Company_PerksDetails";
            //public static string Sproc_Delete_Company_PerksDetails = "Sproc_Delete_Company_PerksDetails";

            //public static string Sproc_Get_Basic_CompanyDetails = "Sproc_Get_Basic_CompanyDetails";
            //public static string Sproc_Update_Basic_CompanyDetails = "Sproc_Update_Basic_CompanyDetails";

            //public static string Sproc_Add_YoutubeLink = "Sproc_Add_YoutubeLink";
            //public static string Sproc_Get_Company_YouTubeDetails = "Sproc_Get_Company_YouTubeDetails";
            //public static string Sproc_Delete_Company_YouTubeDetails = "Sproc_Delete_Company_YouTubeDetails";

            //public static string sproc_UTS_GetCompanyEngagementDetails = "sproc_UTS_GetCompanyEngagementDetails";
            //public static string Sproc_Update_Company_EngagementDetails = "Sproc_Update_Company_EngagementDetails";

            //public static string sp_UTS_GetPOCUserIDByCompanyID = "sp_UTS_GetPOCUserIDByCompanyID";
            //public static string sproc_UTS_UpdatePOCUserIDsByCompanyID = "sproc_UTS_UpdatePOCUserIDsByCompanyID";
            //public static string sp_UTS_GetPOCUserIDByCompanyID_Edit = "sp_UTS_GetPOCUserIDByCompanyID_Edit";

            //public static string sproc_UTS_GetContactDetails = "sproc_UTS_GetContactDetails";
            //public static string sproc_UTS_UpdateContactDetails = "sproc_UTS_UpdateContactDetails";

            //public static string Sproc_CompanyDetail_TransferToATS = "Sproc_CompanyDetail_TransferToATS";
            //public static string sproc_Update_Company_Details_From_Scrapping = "sproc_Update_Company_Details_From_Scrapping";
            //public static string Sproc_UTS_GetCompanyWhatsappDetails = "Sproc_UTS_GetCompanyWhatsappDetails";
            //public static string Sproc_UTS_SaveCompanyWhatsappDetails = "Sproc_UTS_SaveCompanyWhatsappDetails";
            //public static string Sproc_UTS_SaveCompanyWhatsappMemberDetails = "Sproc_UTS_SaveCompanyWhatsappMemberDetails";
            //#endregion

            //#region Company
            //public static string sproc_UTS_GetCompany = "sproc_UTS_GetCompany";
            //public static string sproc_UTS_GetCompanyLegalInfo = "sproc_UTS_GetCompanyLegalInfo";
            //public static string Sproc_UTS_GetHubSpotCompanyList = "Sproc_UTS_GetHubSpotCompanyList";
            //public static string SPROC_Gen_Company_History = "SPROC_Gen_Company_History";
            //public static string sproc_FreeCreditUpdate = "sproc_FreeCreditUpdate"; 
            //public static string Sproc_RegisterClient_ClientPortal = "Sproc_RegisterClient_ClientPortal";
            //public static string Sproc_ValidateAddPayPerCreditClient = "Sproc_ValidateAddPayPerCreditClient";
            //public static string Sproc_ValidateAddPayPerCreditClient_temp = "Sproc_ValidateAddPayPerCreditClient_temp";
            //public static string Sproc_InviteClient_UTS_Admin = "Sproc_InviteClient_UTS_Admin"; 
            //public static string Sproc_InsertTicket = "Sproc_InsertTicket"; 
            //public static string Sproc_UTS_UpdateCompanyCreditInfo = "Sproc_UTS_UpdateCompanyCreditInfo"; 
            //public static string Sproc_Identify_HRAssociated_WithDemoAccount = "Sproc_Identify_HRAssociated_WithDemoAccount";
            //public static string Sproc_GetStandOutDetails_ClientPortal = "Sproc_GetStandOutDetails_ClientPortal";
            //public static string Sp_UTS_PreviewJobPostUpdate_ClientPortal = "Sp_UTS_PreviewJobPostUpdate_ClientPortal";
            //public static string sp_UTS_PreviewJobPostUpdate_UTSAdmin = "sp_UTS_PreviewJobPostUpdate_UTSAdmin";
            //public static string Sproc_Get_Add_CompanyData_Send_Details_To_ATS = "Sproc_Get_Add_CompanyData_Send_Details_To_ATS";


            //#endregion

            //#region Dashboard
            //public static string sproc_Dashboard_GetHRsCountByManagers = "exec sproc_Dashboard_GetHRsCountByManagers @IsManaged, @Month, @Year";
            //public static string sproc_Dashboard_GetHRsCountbyOpsMAnagers = "exec sproc_Dashboard_GetHRsCountbyOpsMAnagers @IsManaged, @Month, @Year";
            //#endregion

            //#region DealList
            //public static string sproc_UTS_GetDealList = "sproc_UTS_GetDealList";
            //public static string sproc_UTS_GetDealCompanydetails = "sproc_UTS_GetDealCompanydetails";
            //public static string sproc_GetDealLeadDetais = "sproc_GetDealLeadDetais";
            //public static string sproc_GetDealprimaryClient = "sproc_GetDealprimaryClient";
            //public static string sproc_GetDealSecondaryClient = "sproc_GetDealSecondaryClient";
            //public static string sproc_GetDealDEALActivity = "sproc_GetDealDEALActivity";
            //public static string sproc_UTS_GetFilterTypeForDeals = "sproc_UTS_GetFilterTypeForDeals";
            //public static string Sproc_Get_HRDetails_From_DealId = "Sproc_Get_HRDetails_From_DealId";
            //#endregion

            //#region Hr Status
            //public static string Sproc_Update_HrStatus = "Sproc_Update_HrStatus";
            //public static string Sproc_UTS_Get_HRStatus = "Sproc_UTS_Get_HRStatus";
            //#endregion

            //#region Hiring 
            //public static string Sproc_UTS_AddEdit_HR = "Sproc_UTS_AddEdit_HR";
            //public static string Sproc_UTS_AddEdit_HRDebrief = "Sproc_UTS_AddEdit_HRDebrief";
            //public static string Sproc_UTS_CheckCreditAvailablilty = "Sproc_UTS_CheckCreditAvailablilty";

            //public static string sproc_Interviewdetailsdbrief = "sproc_Interviewdetailsdbrief";
            //public static string Sproc_GetCompanyDetail = "Sproc_GetCompanyDetail";
            //public static string Sproc_GetInterviewDetails = "Sproc_GetInterviewDetails";
            //public static string sproc_FetchHiringInterviewerDetails = "sproc_FetchHiringInterviewerDetails";
            //public static string sproc_UTS_ViewTalent = "sproc_UTS_ViewTalent";
            //public static string sproc_UTS_getCompanyNameByHiringRequestID = "sproc_UTS_getCompanyNameByHiringRequestID";
            //public static string sproc_UTS_GetTalentProfileLog = "sproc_UTS_GetTalentProfileLog";
            //public static string sproc_UTS_GetProfileShareDetail = "sproc_UTS_GetProfileShareDetail";
            //public static string sproc_UTS_GetTalentScoreCard = "sproc_UTS_GetTalentScoreCard";
            //public static string Sproc_GetExtractedSkills_FromJD = "Sproc_GetExtractedSkills_FromJD";
            //public static string Sproc_DumpJDDetailsintoTempTable = "Sproc_DumpJDDetailsintoTempTable";
            //public static string Sproc_Get_JDskillDataByJDDumpID = "Sproc_Get_JDskillDataByJDDumpID";
            //public static string sproc_GetOnBoardData_Result = "sproc_GetOnBoardData";
            //public static string sproc_UTS_GetAutoCompleteContacts = "sproc_UTS_GetAutoCompleteContacts";
            //public static string sproc_UTS_GetAutoCompleteCompany = "sproc_UTS_GetAutoCompleteCompany";

            //public static string Sproc_GET_PostAcceptance_detail = "Sproc_GET_PostAcceptance_detail";
            //public static string Sproc_GET_PostAcceptance_detail_Availability = "Sproc_GET_PostAcceptance_detail_Availability";
            //public static string Sproc_GET_PostAcceptance_detail_HowSoon = "Sproc_GET_PostAcceptance_detail_HowSoon";
            //public static string sproc_update_Talent_ConfirmPostAcceptance = "sproc_update_Talent_ConfirmPostAcceptance";
            //public static string Sproc_Add_Confirm_Interview_Slot = "Sproc_Add_Confirm_Interview_Slot";
            //public static string sproc_ShortlistedTalents_Update = "sproc_ShortlistedTalents_Update";
            //public static string sproc_CloneHRFromExistHR = "sproc_CloneHRFromExistHR";
            //public static string sproc_UTS_GetChildCompanyList = "sproc_UTS_GetChildCompanyList";
            //public static string sproc_UTS_UpdatePartnershipDetails_ForHR = "sproc_UTS_UpdatePartnershipDetails_ForHR";
            //public static string Sproc_Update_HRDetails_From_Special_User = "sproc_Update_HRDetails_From_Special_User";

            //public static string Sproc_FetchSimilarRoles_UTS = "Sproc_FetchSimilarRoles_UTS";
            //public static string SP_PayPerHire_Calculation_Update = "SP_PayPerHire_Calculation_Update";
            //public static string SP_PayPerHire_CalculationDetails = "SP_PayPerHire_CalculationDetails";
            //public static string Sp_UTS_GetCreditUtilization_BasedOnHR = "Sp_UTS_GetCreditUtilization_BasedOnHR";
            //public static string Sproc_UTSAdmin_EditHrByATS = "Sproc_UTSAdmin_EditHrByATS";
            //public static string Sproc_UTS_ContactAsActive = "Sproc_UTS_ContactAsActive";
            //public static string sp_UTS_TalentCalculation_PayPerHire = "sp_UTS_TalentCalculation_PayPerHire";
            //public static string Sproc_Get_ShortListedDetails_ForCreditFlow_ClientPortal = "Sproc_Get_ShortListedDetails_ForCreditFlow_ClientPortal";

            ////History 
            //public static string SPROC_Gen_SalesHiringRequest_History = "SPROC_Gen_SalesHiringRequest_History";
            //public static string SPROC_Gen_SalesHiringRequest_Details_History = "SPROC_Gen_SalesHiringRequest_Details_History";
            //public static string sproc_Talent_History_Insert = "sproc_Talent_History_Insert";
            //public static string sproc_Delete_HR_AfterOnBoard = "sproc_Delete_HR_AfterOnBoard";
            //public static string sproc_DeleteSalesHiringRequestAndDetails = "sproc_DeleteSalesHiringRequestAndDetails";
            //public static string Sproc_Get_AMNBD_For_Replacement = "Sproc_Get_AMNBD_For_Replacement";
            //public static string Sproc_Add_TalentReplacement_details = "Sproc_Add_TalentReplacement_details";
            //public static string sproc_TalentClientAssociation_Insert = "sproc_TalentClientAssociation_Insert";
            //public static string Sproc_Talent_Status_Change_ClientPortal = "Sproc_Talent_Status_Change_ClientPortal";
            //public static string Sproc_UTS_GetCloseHRHistory = "Sproc_UTS_GetCloseHRHistory";
            //public static string Sproc_CreateJDFromPrompt_ClientPortal = "Sproc_CreateJDFromPrompt_ClientPortal";
            //public static string Sproc_Get_UpdateHR_Details = "Sproc_Get_UpdateHR_Details";
            //public static string Sproc_UTS_DeleteTestHR = "Sproc_UTS_DeleteTestHR";
            //public static string sproc_CloneHRFromExistHR_DemoAccount = "sproc_CloneHRFromExistHR_DemoAccount";
            //public static string Sproc_CreateJDFromPrompt_UTSAdmin = "Sproc_CreateJDFromPrompt_UTSAdmin";
            //public static string Sproc_SearchLocation = "Sproc_SearchLocation";
            //public static string Sproc_Get_NearByMAppingCities = "Sproc_Get_NearByMAppingCities";
            //public static string Sproc_HR_POC_ClientPortal = "Sproc_HR_POC_ClientPortal";
            //public static string Sproc_HR_EditPOC = "Sproc_HR_EditPOC";
            //public static string sp_UTS_get_HRHistory_UsingPagination = "sp_UTS_get_HRHistory_UsingPagination";
            //public static string Sproc_Update_POC_ShowHideFlag = "Sproc_Update_POC_ShowHideFlag";
            //public static string Sproc_Get_Frequency_Office_Visit = "Sproc_Get_Frequency_Office_Visit";
            //public static string Sproc_HiringTypeDetails_ClientPortal = "Sproc_HiringTypeDetails_ClientPortal";
            //public static string Sproc_AddUpdate_TalentNotes_ClientPortal = "Sproc_AddUpdate_TalentNotes_ClientPortal";
            //public static string Sproc_Fetch_TalentNotesEmailsLog_HRWise = "Sproc_Fetch_TalentNotesEmailsLog_HRWise";
            //public static string Sproc_Fetch_TalentNotesEmailsLog = "Sproc_Fetch_TalentNotesEmailsLog";
            //public static string Sproc_AddUpdate_TalentNotesEmailsLog = "Sproc_AddUpdate_TalentNotesEmailsLog";
            //public static string Sproc_Fetch_TalentNotesEmailsLog_HRWise_TalentWise = "Sproc_Fetch_TalentNotesEmailsLog_HRWise_TalentWise";
            //public static string Sproc_SearchLocationOnly = "Sproc_SearchLocationOnly";
            //#endregion

            //#region Talent Status
            //public static string sproc_UTS_UpdateHrDetailRoleStatusWithTalentStatus = "sproc_UTS_UpdateHiringRequestDetailRoleStatusWithTalentStatus";
            //public static string sproc_UTS_UpdateTalentRejectReason = "sproc_UTS_UpdateTalentRejectReason";
            //public static string sp_UTS_get_HRTalentProfileReason = "sp_UTS_get_HRTalentProfileReason";
            //public static string Sproc_UTS_Update_OnHold_Status_To_PreviousStatus = "Sproc_UTS_Update_OnHold_Status_To_PreviousStatus";
            //public static string Sproc_Get_RejectionReasonForTalent = "Sproc_Get_RejectionReasonForTalent";

            //#endregion

            //#region Interview
            //public static string sproc_UTS_GetHiringInterview = "sproc_UTS_GetHiringInterview";
            //public static string Sproc_InsertBookSlot = "Sproc_InsertBookSlot";
            //public static string Sproc_InsertBookSlot_CHECK = "Sproc_InsertBookSlot_CHECK";
            //public static string sproc_Get_ContactPointofContact = "sproc_Get_ContactPointofContact";
            //public static string Sproc_Update_TalentSelected = "Sproc_Update_TalentSelected";
            //public static string Sproc_Update_TalentHRStatus = "Sproc_Update_TalentHRStatus";
            //public static string Sproc_UpdateIsLaterSlot = "Sproc_UpdateIsLaterSlot";
            //public static string sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime_ByID = "sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime_ByID";
            //public static string sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime = "sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime";
            //public static string Sproc_Add_TalentSelected_ClientFeedback = "Sproc_Add_TalentSelected_ClientFeedback";

            //public static string sproc_UTS_GetCurrentInterviewerDetails = "sproc_UTS_GetCurrentInterviewerDetails";
            //public static string sproc_Insert_NextInterviewRoundDetails_WithFeedbackOption = "sproc_Insert_NextInterviewRoundDetails_WithFeedbackOption";
            //public static string sproc_Insert_NextRound_InterviewerDetails = "sproc_Insert_NextRound_InterviewerDetails";
            //public static string sproc_UpdateInterviewStatus_WithFeedback_SlotBookNow = "sproc_UpdateInterviewStatus_WithFeedback_SlotBookNow";
            //public static string sproc_Get_InterviewRoundDetails = "sproc_Get_InterviewRoundDetails";
            //public static string Sproc_AfterInterviewCancel_TalentAsProfileShared = "Sproc_AfterInterviewCancel_TalentAsProfileShared";
            //#endregion

            //#region Interviewer Details
            //public static string sproc_UTS_FetchInterviewerDetails = "sproc_UTS_FetchInterviewerDetails";
            //public static string sproc_UTS_AddUpdateDeleteInterviewerDetails = "sproc_UTS_AddUpdateDeleteInterviewerDetails";
            //#endregion

            #region Master
            //public static string Sproc_UTS_Get_PrgSkill_NotusedinMappingtables = "Sproc_UTS_Get_PrgSkill_NotusedinMappingtables";
            //public static string Sproc_prg_TempSkills_Insert = "Sproc_prg_TempSkills_Insert";
            //public static string sproc_GetReportingUsers = "sproc_GetReportingUsers";
            //public static string Sproc_Get_CountryList = "Sproc_Get_CountryList";
            public static string Sproc_CurrencyExchangeRate = "Sproc_CurrencyExchangeRate";
            //public static string sproc_UpdateCurrencyExchangeRate = "sproc_UpdateCurrencyExchangeRate";
            //public static string sproc_GetUserBy_LeadType = "Sproc_GetUserBy_LeadType";
            //public static string sproc_GetTalentRoles = "sproc_GetTalentRoles";
            //public static string Sproc_UTS_Update_Role = "Sproc_UTS_Update_Role";
            //public static string Sproc_GetContactTimeZone = "Sproc_GetContactTimeZone";
            //public static string Sproc_UTS_GetUserByType = "Sproc_UTS_GetUserByType";
            //public static string Sproc_UTS_GetSalesPerson = "Sproc_UTS_GetSalesPerson";
            #endregion

            //#region User
            //public static string sproc_UTS_GetUsers = "sproc_UTS_GetUsers";
            //public static string sproc_UTS_Inc_GetUsers_RoleDetails = "sproc_UTS_Inc_GetUsers_RoleDetails";
            //public static string sproc_UTS_GetBDR_Marketingusers = "sproc_UTS_GetBDR_Marketingusers";
            //public static string SPROC_UTS_UserGeoValues = "SPROC_UTS_UserGeoValues";
            //public static string sproc_UTS_InsertUserGeo = "sproc_UTS_InsertUserGeo";
            //public static string sproc_User_History_Insert = "sproc_User_History_Insert";
            //public static string sproc_UpdateOrInsert_UserRoleDetails = "sproc_UpdateOrInsert_UserRoleDetails";
            //public static string sproc_MaintainUserType_TeamAndLevelWise = "sproc_MaintainUserType_TeamAndLevelWise";
            //public static string sproc_Get_HRDetails_For_UserPriority_Email = "Sproc_Get_HRDetails_For_UserPriority_Email";
            //public static string sproc_GetUserHierarchy = "sproc_GetUserHierarchy";
            //public static string Sproc_Check_Validation_Message_For_Updating_Priority_Counts = "Sproc_Check_Validation_Message_For_Updating_Priority_Counts";
            //#endregion

            //#region AMAssignments
            //public static string sproc_UTS_GetAMAssignments = "sproc_UTS_GetAMAssignments";
            //public static string SP_UTS_ESales_Get_Client_AM_Details = "SP_UTS_ESales_Get_Client_AM_Details";
            //public static string sproc_UTS_Update_EmployeeID_FromInvoiceAPIResponse = "sproc_UTS_Update_EmployeeID_FromInvoiceAPIResponse";
            //public static string Sproc_Get_AM_User = "Sproc_Get_AM_User";
            //#endregion

            //#region AMAssignmentsRule
            //public static string sproc_UTS_GetAMAssignmentRules = "sproc_UTS_GetAMAssignmentRules";
            //#endregion

            //#region View All HR
            //public static string sproc_UTS_GetAllHRs = "sproc_UTS_GetAllHRs";
            //public static string sproc_FetchMissingAction = "sproc_FetchMissingAction";
            //public static string sp_UTS_get_HRClientDetails = "sp_UTS_get_HRClientDetails";
            //public static string sp_UTS_get_HRTalentDetails = "sp_UTS_get_HRTalentDetails";
            //public static string sp_UTS_get_HRTalentDetails_UsingPagination = "sp_UTS_get_HRTalentDetails_UsingPagination";
            //public static string sproc_UTS_NextActionsForTalent = "sproc_UTS_NextActionsForTalent";
            //public static string Sproc_UTS_Get_TalentDetails_For_Managed_HR = "Sproc_UTS_Get_TalentDetails_For_Managed_HR";
            //public static string sproc_UTS_NextActionsForManagedHR = "sproc_UTS_NextActionsForManagedHR";
            //public static string sp_UTS_get_HRHistory = "sp_UTS_get_HRHistory";
            //public static string sp_getInterviewdetailsForViewAllHR = "sp_getInterviewdetailsForViewAllHR";
            //public static string sproc_UTS_Insert_HiringRequest_PriorityHistory = "sproc_UTS_Insert_HiringRequest_PriorityHistory";
            //public static string sproc_HiringRequest_History_Insert = "sproc_HiringRequest_History_Insert";
            //public static string sproc_UTS_gen_ContactTalentPriorityupdate = "sproc_UTS_gen_ContactTalentPriorityupdate";
            //public static string Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion = "Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion";
            //public static string sproc_UTS_UpdateDPAmountDetails = "sproc_UTS_UpdateDPAmountDetails";
            //public static string Sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion = "Sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion";
            //public static string sproc_UTS_UpdateIsHrtypedpandDPAmount = "sproc_UTS_UpdateIsHrtypedpandDPAmount";
            //public static string Sproc_Get_Validation_Message_For_Priority = "Sproc_Get_Validation_Message_For_Priority";
            //public static string sproc_Insert_HiringRequest_Priority_Revise_Flow = "sproc_Insert_HiringRequest_Priority_Revise_Flow";
            //public static string Sproc_Get_Priority_Set_Remaining_Count_Details = "Sproc_Get_Priority_Set_Remaining_Count_Details";
            //public static string Sproc_Check_Validation_Message_For_User_Edit_TR = "Sproc_Check_Validation_Message_For_User_Edit_TR";
            //public static string Sproc_UpdateTR = "Sproc_UpdateTR";
            //public static string Sproc_UTS_FetchUsersWithSpecialEditAccess = "Sproc_UTS_FetchUsersWithSpecialEditAccess";

            //public static string sproc_Get_HiringRequest_SLADetails = "sproc_Get_HiringRequest_SLADetails";
            //public static string Sproc_Update_SLADate = "Sproc_Update_SLADate";
            //public static string Sproc_Get_SLAUpdate_History = "Sproc_Get_SLAUpdate_History";
            //public static string Sproc_Get_SLAUpdateDetails_ForEmail = "Sproc_Get_SLAUpdateDetails_ForEmail";
            //public static string Sproc_Get_Latest_ATS_HRStatus_Details_For_Notification = "Sproc_Get_Latest_ATS_HRStatus_Details_For_Notification";
            //public static string sp_UTS_GetStandOutDetails = "sp_UTS_GetStandOutDetails";
            
            //public static string sproc_AssignedPOCToUnAssignedHRs = "sproc_AssignedPOCToUnAssignedHRs";
            //public static string sproc_UTS_GetAllUnAssignedHRs = "sproc_UTS_GetAllUnAssignedHRs";
            //#endregion

            //#region Talent
            //public static string sproc_UTS_GetTalent = "sproc_UTS_GetTalent";
            //public static string Sproc_UTS_ManagedTalent = "Sproc_UTS_ManagedTalent";
            //public static string sproc_UTS_GetTalentLegalInfo = "sproc_UTS_GetTalentLegalInfo";
            //public static string Sproc_UpdateRejectionStage = "Sproc_UpdateRejectionStage";
            //#endregion

            //#region Onboard
            //public static string sproc_UTS_Update_SalesUserID_LegalOrKickoffDone = "sproc_UTS_Update_SalesUserID_LegalOrKickoffDone";
            //public static string sproc_UTS_Update_LastWorkingDay_For_TalentinReplacement = "sproc_UTS_Update_LastWorkingDay_For_TalentinReplacement";
            //public static string Sproc_Get_OnBoardContract_Details = "Sproc_Get_OnBoardContract_Details";
            //public static string Sproc_Get_UTS_OnBoardContract_Details = "Sproc_UTS_Get_OnBoardContract_Details";
            //public static string Sproc_UTS_Get_OnBoardClientTeamMemberDeatils = "Sproc_UTS_Get_OnBoardClientTeamMemberDeatils";
            //public static string Sproc_OnBoardPolicy_DeviceMaster_Update = "Sproc_OnBoardPolicy_DeviceMaster_Update";
            //public static string Sproc_Get_OnBoardClientFeedBack = "Sproc_Get_OnBoardClientFeedBack";
            //public static string sproc_Get_PreOnboarding_Details_For_AMAssignment = "sproc_Get_PreOnboarding_Details_For_AMAssignment";
            //public static string Sproc_Add_OnBoardClientContractDetails = "Sproc_Add_OnBoardClientContractDetails";
            //public static string sproc_UTS_GetAllOpenInprocessHR_BasedOnContact = "sproc_UTS_GetAllOpenInprocessHR_BasedOnContact";
            //public static string sproc_Update_PreOnBoardingDetails_for_AMAssignment = "sproc_Update_PreOnBoardingDetails_for_AMAssignment";
            //public static string sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment = "sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment";
            //public static string sproc_Update_Onboarding_Details_For_Second_Tab_AMAssignment = "sproc_Update_Onboarding_Details_For_Second_Tab_AMAssignment";
            //public static string Sproc_Insert_Onboarding_Summary = "Sproc_Insert_Onboarding_Summary";
            //public static string Sproc_Get_OnBoardTalents = "Sproc_Get_OnBoardTalents";
            //public static string Sproc_Get_engagement_Edit_All_BR_PR = "Sproc_Get_engagement_Edit_All_BR_PR";
            //public static string Sproc_UTS_Admin_OnboardingStatusUpdates = "Sproc_UTS_Admin_OnboardingStatusUpdates";
            //public static string sproc_UTS_Get_Onboarding_LegalInfo = "sproc_UTS_Get_Onboarding_LegalInfo";
            //#endregion

            //#region Incentives

            //#region PlacementFees
            //public static string Sproc_UTS_Get_Inc_PlacementFees_Slab = "Sproc_UTS_Get_Inc_PlacementFees_Slab";
            //public static string Sproc_Update_Inc_PlacementFees_Slab = "Sproc_Update_Inc_PlacementFees_Slab";
            //#endregion

            //#region Contracts
            //public static string Sproc_Get_Inc_Contracts = "Sproc_Get_Inc_Contracts";
            //public static string Sproc_Update_Inc_Contracts = "Sproc_Update_Inc_Contracts";
            //#endregion

            //public static string Sproc_Get_Inc_SALGoal = "Sproc_Get_Inc_SALGoal";
            //public static string Sproc_Update_Inc_SALGoal = "Sproc_Update_Inc_SALGoal";

            //public static string Sproc_Get_Inc_TalentDeploySlab = "Sproc_Get_Inc_TalentDeploySlab";
            //public static string Sproc_UTS_Get_Acheived_Target_Details = "Sproc_UTS_Get_Acheived_Target_Details";
            //public static string Sproc_UTS_Get_Inc_AM_NRSlab = "Sproc_UTS_Get_Inc_AM_NRSlab";
            //#endregion

            //#region Engagement Dashboard
            //public static string Sproc_UTS_Get_BusinessDashboard = "Sproc_UTS_Get_BusinessDashboard";
            //public static string Sproc_talent_engagement_Details_For_PHP_API = "Sproc_talent_engagement_Details_For_PHP_API";
            //public static string Sproc_Update_CancelEngagementDetails = "Sproc_Update_CancelEngagementDetails";
            //#endregion

            //#region Engagement
            //public static string Sproc_Add_Actual_BR_PR_NR = "Sproc_Add_Actual_BR_PR_NR";
            //public static string Sproc_Calculate_ActualNR_From_BR_PR = "Sproc_Calculate_ActualNR_From_BR_PR";
            //public static string Sproc_UTS_DashBoard_OnBoardCount = "Sproc_UTS_DashBoard_OnBoardCount";
            //public static string Sproc_Add_Invoice_Details_For_Engagements = "Sproc_Add_Invoice_Details_For_Engagements";
            //public static string Sproc_Update_OnBoardClientContractEndDateDetails = "Sproc_Update_OnBoardClientContractEndDateDetails";
            //public static string Sproc_Get_Renewal_Details = "Sproc_Get_Renewal_Details";
            //public static string Sproc_Add_Contract_Renewal_Details = "Sproc_Add_Contract_Renewal_Details";
            //public static string sproc_Get_TSC_List_Result = "sproc_Get_TSC_List";
            //public static string Sproc_GetTSCUserEditDetail = "Sproc_GetTSCUserEditDetail";
            //public static string Sproc_UpdateTSCUser = "Sproc_UpdateTSCUser";
            //public static string Sproc_Get_User_For_TSCAutoAssignment_BasedOnRoundRobin = "Sproc_Get_User_For_TSCAutoAssignment_BasedOnRoundRobin";
            //public static string Sproc_GetEmailDetailForTSCAssignment = "Sproc_GetEmailDetailForTSCAssignment";
            //public static string Sproc_UTS_Get_PayOut_Basic_Information = "Sproc_UTS_Get_PayOut_Basic_Information";
            //public static string Sproc_UTS_Save_AMDetails_In_PayOut = "Sproc_UTS_Save_AMDetails_In_PayOut";
            //public static string Sproc_TalentEngagementConverted_C2H = "Sproc_TalentEngagementConverted_C2H";
            //public static string Sproc_Get_OnBoardDetailFor_C2H = "Sproc_Get_OnBoardDetailFor_C2H";
            //public static string Sproc_Update_RenewalInitiated = "Sproc_Update_RenewalInitiated";
            //#endregion

            //#region ATS
            //public static string Sproc_GET_ALL_HR_Details_For_PHP_API = "Sproc_GET_ALL_HR_Details_For_PHP_API";
            //public static string sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI = "sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI";
            //#endregion

            //#region Talentreplacement
            //public static string Sproc_Get_Hierarchy_For_Email = "Sproc_Get_Hierarchy_For_Email";
            //public static string Sproc_Get_Engagemetns_For_Replacement_BasedOn_LWD_Option = "Sproc_Get_Engagemetns_For_Replacement_BasedOn_LWD_Option";
            //public static string Sproc_Get_HRIDEngagementID_ForReplacement = "Sproc_Get_HRIDEngagementID_ForReplacement";
            //#endregion

            //#region Reports 
            //#region Demand Funnel 
            //public static string sproc_GetChannelWise_ActionReport = "sproc_GetChannelWise_ActionReport_Revised";
            //public static string sproc_GetChannelWiseFunnelReportData = "sproc_GetChannelWiseFunnelReportData_Revised";
            //public static string sproc_GetChannelWiseFunnelReportData_For_Summary = "sproc_GetChannelWiseFunnelReportData_For_Summary_Revised";
            //public static string sproc_GetChannelWise_ActionReport_For_Summary = "sproc_GetChannelWise_ActionReport_For_Summary_Revised";
            //public static string sproc_GetChannelWiseFunnelReportData_For_PopUP = "sproc_GetChannelWiseFunnelReportData_For_PopUP_Revised";
            //public static string sproc_GetChannelWiseFunnelReportData_For_Summary_Revised = "sproc_GetChannelWiseFunnelReportData_For_Summary_Revised";
            //#endregion

            //#region Supply Funnel

            //public static string sproc_GetSupplyChannelWise_ActionReport = "sproc_GetSupplyChannelWise_ActionReport";
            //public static string sproc_GetSupplyChannelWiseFunnelReportData = "sproc_GetSupplyChannelWiseFunnelReportData";
            //public static string sproc_GetSupplyChannelWiseFunnelReportData_For_Summary = "sproc_GetSupplyChannelWiseFunnelReportData_For_Summary";
            //public static string sproc_GetSupplyChannelWise_ActionReport_For_Summary = "sproc_GetSupplyChannelWise_ActionReport_For_Summary";
            //public static string sproc_GetSupplyWiseChannelWiseFunnelReportData_For_PopUP = "sproc_GetSupplyWiseChannelWiseFunnelReportData_For_PopUP";
            //#endregion

            //#region Team Demand Funnel

            ////public static string sproc_GetChannelWise_ActionReport_ForSalesUsers = "sproc_GetChannelWise_ActionReport_ForSalesUsers";
            //public static string sproc_GetChannelWiseFunnelReportData_ForSalesUsers = "sproc_GetChannelWiseFunnelReportData_ForSalesUsers";
            //public static string sproc_GetChannelWiseFunnelReportData_For_Summary_ForSalesUsers = "sproc_GetChannelWiseFunnelReportData_For_Summary_ForSalesUsers";
            ////public static string sproc_GetChannelWise_ActionReport_For_Summary_ForSalesUsers = "sproc_GetChannelWise_ActionReport_For_Summary_ForSalesUsers";
            ////public static string sproc_GetChannelWiseFunnelReportData_For_SalesUser_PopUP = "sproc_GetChannelWiseFunnelReportData_For_SalesUser_PopUP";

            //#region Revised SPs

            //public static string sproc_GetChannelWise_ActionReport_ForSalesUsers_Revised = "sproc_GetChannelWise_ActionReport_ForSalesUsers_Revised";
            //public static string sproc_GetChannelWise_ActionReport_For_Summary_ForSalesUsers_Revised = "sproc_GetChannelWise_ActionReport_For_Summary_ForSalesUsers_Revised";
            //public static string sproc_GetChannelWiseFunnelReportData_For_SalesUser_PopUP_Revised = "sproc_GetChannelWiseFunnelReportData_For_SalesUser_PopUP_Revised";

            //#endregion

            //#endregion

            //#region Client Report
            //public static string sproc_ClientBasedReport = "sproc_ClientBasedReport";
            //public static string sproc_ClientBasedReport_PopUp = "sproc_ClientBasedReport_PopUp";
            //#endregion

            //#region Client Log Report
            //public static string Sproc_GetClientLogReport = "Sproc_GetClientLogReport";
            //public static string Sproc_GetClientActivityLog = "Sproc_GetClientActivityLog";
            //#endregion

            //#region Skill wise
            //public static string sproc_SkillReport_ActionWise = "sproc_SkillReport_ActionWise";
            //public static string sproc_SkillReport_HRWise = "sproc_SkillReport_HRWise";
            //public static string sproc_SkillReport_ActionWise_PopUp = "sproc_SkillReport_ActionWise_PopUp";
            //public static string sproc_SkillReport_HRWise_PopUp = "sproc_SkillReport_HRWise_PopUp";
            //#endregion

            //#region SLA Report
            //public static string Sproc_SLA_OverAll_Summary_Report_Static_Stages = "Sproc_SLA_OverAll_Summary_Report_Static_Stages";
            //public static string Sproc_SLA_Report_For_Static_Stages = "Sproc_SLA_Report_For_Static_Stages";
            //public static string Sproc_SLA_Missed_Summary_Report = "Sproc_SLA_Missed_Summary_Report";
            //public static string Sproc_UTS_SLA_Report_For_Static_Stages = "Sproc_UTS_SLA_Report_For_Static_Stages";
            //public static string Sproc_UTS_SLA_OverAll_Summary_Report_Static_Stages = "Sproc_UTS_SLA_OverAll_Summary_Report_Static_Stages";
            //public static string Sproc_Get_SalesHead_Users = "Sproc_Get_SalesHead_Users";
            //#endregion

            //#region JDParsingDump Report
            //public static string sproc_JDParsingDumpReport = "sproc_JDParsingDumpReport";
            //#endregion

            //#region Hiring Request Report
            ///// <summary>
            ///// The sproc hiring request report
            ///// </summary>
            //public static string sproc_HiringRequest_Report = "sproc_HiringRequest_Report";
            ///// <summary>
            ///// The sproc hiring request report pop up
            ///// </summary>
            //public static string sproc_HiringRequest_Report_PopUp = "sproc_HiringRequest_Report_PopUp";
            //#endregion

            //#region HubSpot Funnel 
            //public static string Sproc_HubSpot_Client_Funnel_Report = "Sproc_HubSpot_Client_Funnel_Report";
            //public static string Sproc_HubSpot_Client_Funnel_Report_PopUP = "Sproc_HubSpot_Client_Funnel_Report_PopUP";

            //#endregion

            //#region Client portal Treking reports
            //public static string sproc_UTS_ClientList = "sproc_UTS_ClientList";
            //public static string Sproc_Get_UTS_ClientPortalTracking_Details = "Sproc_Get_UTS_ClientPortalTracking_Details";
            //public static string Sproc_Get_UTS_ClientPortalTracking_Details_Popup = "Sproc_Get_UTS_ClientPortalTracking_Details_Popup";
            //#endregion

            //#region Replacement Report
            //public static string sproc_Get_Replacement_Report = "sproc_Get_Replacement_Report";
            //#endregion

            //#region Backout Report            
            //public static string sproc_Get_TalentBackout_Report = "sproc_Get_TalentBackout_Report";
            //#endregion


            //#region AWS SES Treking reports
            //public static string Sproc_Get_AWS_SES_Tracking_Details = "Sproc_Get_AWS_SES_Tracking_Details";
            //public static string Sproc_Get_AWS_SES_Tracking_Details_Popup = "Sproc_Get_AWS_SES_Tracking_Details_Popup";
            //public static string Sproc_Get_Email_SubjectList = "Sproc_Get_Email_SubjectList";
            //#endregion
            //#endregion

            //#region Incentive Report
            //public static string Sproc_Get_Managers = "Sproc_Get_Managers";
            //public static string Sproc_Inc_Incentive_Report = "sproc_Inc_Incentive_Report";
            //public static string Sproc_Get_Validation_Message_For_Incentive_Report = "Sproc_Get_Validation_Message_For_Incentive_Report";
            //public static string Sproc_GetInc_ReportDetails = "Sproc_GetInc_ReportDetails";
            //public static string Sproc_GetInc_ReportDetails_AM_NR = "Sproc_GetInc_ReportDetails_AM_NR";
            //public static string Sproc_GetInc_ReportDetails_Contract_Booster = "Sproc_GetInc_ReportDetails_Contract_Booster";
            //public static string Sproc_Get_Users_BasedOnUserRole = "Sproc_Get_Users_BasedOnUserRole";
            //public static string Sroc_Get_Hierarchy = "Sroc_Get_Hierarchy";
            //public static string Sproc_Insert_Target_Acheived_Details = "Sproc_Insert_Target_Acheived_Details";
            //#endregion

            //#region Incentive Target
            //public static string sproc_Get_IncentiveTarget_List_New_flow = "sproc_Get_IncentiveTarget_List_New_flow";
            //public static string Sproc_Get_ALL_User_HIERARCHY_SaleTarget_For_Parent = "Sproc_Get_ALL_User_HIERARCHY_SaleTarget_For_Parent";
            //public static string sproc_insert_Incentive_UserTarget = "sproc_insert_Incentive_UserTarget";
            //#endregion

            //#region UserTeam
            //public static string Sproc_GetTeams = "Sproc_GetTeams";
            //#endregion

            //#region Logger
            //public static string UTS_ReactAPI_LoggingInProc = "UTS_ReactAPI_LoggingInProc";
            //#endregion

            //public static string sproc_TalentHR_CancelledHR_List = "sproc_TalentHR_CancelledHR_List";

            ////UTS-3310: Constants to hold the procedure name for I2S Api's.
            //#region I2S Reports
            //public static string sproc_Get_InterviewToSuccess_PopUp = "Sproc_Get_InterviewToSuccess_PopUp";
            //public static string sproc_GetInterviewToSuccessReport = "sproc_GetInterviewToSuccessReport";
            //#endregion

            ////UTS-3256: Constants to hold the stored procedure name for close HR.
            //#region Close HR
            //public static string sproc_Update_Status_For_Closed_HR = "Sproc_Update_Status_For_Clsoed_HR";
            //public static string sproc_Check_Validation_Message_For_Close_HR = "Sproc_Check_Validation_Message_For_Close_HR";
            //public static string sproc_UTS_HRClose_CheckTalentOfferHiredwithMessage = "sproc_UTS_HRClose_CheckTalentOfferHiredwithMessage_UTSAdmin";
            //#endregion

            ////UTS-3641: Constants to hold the stored procedure name for Reopen HR.
            //#region Reopen HR
            ////Pay per Hire HR
            //public static string sproc_Update_Status_For_Reopen_HR = "Sproc_Update_Status_For_Reopen_HR";
            ////pay per credit HR
            //public static string Sproc_RepostedJob_ClientPortal = "Sproc_RepostedJob_ClientPortal";
            //#endregion

            //#region ClientBasedReportWithHubSpot
            //public static string sproc_ClientBasedReport_WithHubSpot = "sproc_ClientBasedReport_WithHubSpot";
            //public static string sproc_ClientBasedReport_WithHubSpot_PopUp = "sproc_ClientBasedReport_WithHubSpot_PopUp";
            //#endregion

            //#region Hiring Lost Report
            ///// <summary>
            ///// The sproc HRLost report
            ///// </summary>
            //public static string sproc_GetHRLost_Report = "sproc_GetHRLost_Report";
            ///// <summary>
            ///// The sproc hiring request report pop up
            ///// </summary>
            //public static string sproc_UTS_GetTalentDetailHRLostPopUp = "sproc_UTS_GetTalentDetailHRLostPopUp";
            //public static string Sproc_Get_UTS_TrackingLeadDetails = "Sproc_Get_UTS_TrackingLeadDetails";
            //public static string Sproc_GET_UTM_Tracking_Report_Details_PopUP = "Sproc_GET_UTM_Tracking_Report_Details_PopUP";
            //public static string Sproc_Get_TrackingLead_Details_for_ClientSource = "Sproc_Get_TrackingLead_Details_for_ClientSource";
            //public static string Sproc_Get_JobPostCount_For_UTM_Tracking_Lead = "Sproc_Get_JobPostCount_For_UTM_Tracking_Lead";
            //public static string Sproc_GET_CreditPlanDetails_ClientPortal = "Sproc_GET_CreditPlanDetails_ClientPortal";


            //#endregion

            //#region ChatGPT Response
            //public static string sproc_Get_ChatGPT_Response_For_UrlParsing_List = "sproc_Get_ChatGPT_Response_For_UrlParsing_List";
            //#endregion

            //#region Direct HR
            //public static string sproc_AddEdit_DirectHR_HRTab = "sproc_AddEdit_DirectHR_HRTab";
            //public static string sproc_AddEdit_DirectHR_DebriefTab = "sproc_AddEdit_DirectHR_DebriefTab";
            //public static string sproc_DirectHR_Details = "sproc_DirectHR_Details";
            //#endregion

            //#region Hubspot
            //public static string Sproc_AddUpdate_Hubspot_Company = "Sproc_AddUpdate_Hubspot_Company";
            //public static string sproc_UpdateCompanyLogo_forHubpsoCompany = "sproc_UpdateCompanyLogo_forHubpsoCompany";
            //public static string Sproc_AddUpdate_HubSpot_Contact = "Sproc_AddUpdate_HubSpot_Contact";
            //#endregion
        }

        public static class UpChatSproc
        {
            #region Channel
            public static string sproc_GetChannels = "sproc_GetChannels";
            public static string sp_UTS_get_HRHistoryUpChat = "sp_UTS_get_HRHistoryUpChat";
            public static string Sproc_UpChat_GetHRNotes = "Sproc_UpChat_GetHRNotes";
            #endregion

            #region User
            public static string Sproc_UpChat_Get_UserList = "Sproc_UpChat_Get_UserList";
            public static string Sproc_GetChannelUsers = "Sproc_GetChannelUsers";
            public static string Sproc_UpChat_AddChannelHistory = "Sproc_UpChat_AddChannelHistory";
            #endregion

            #region Restore
            public static string sp_Upchat_Insert_Into_StoreAPIURL = "sp_Upchat_Insert_Into_StoreAPIURL";
            public static string Sp_Upchat_GetUserListBasedOnActionPerformBy = "Sp_Upchat_GetUserListBasedOnActionPerformBy";
            #endregion

            #region Snoozed
            public static string sp_Upchat_GetInActiveHRList = "sp_Upchat_GetInActiveHRList";
            #endregion

            #region UnUsed SP
            //public static string Sproc_Get_Channels_Details = "Sproc_Get_Channels_Details";
            //public static string Sproc_Get_Channels_Message_Details = "Sproc_Get_Channels_Message_Details";
            //public static string Sproc_Get_UserList_ByChannelID = "Sproc_Get_UserList_ByChannelID";
            //public static string Sproc_Get_UserList_WhileAdd = "Sproc_Get_UserList_WhileAdd";
            //public static string Sproc_Channel_AddUser = "Sproc_Channel_AddUser";
            //public static string Sproc_Channel_RemoveUser = "Sproc_Channel_RemoveUser";
            #endregion
        }

        public static class MediaConstant
        {
            public static string CompanyLogo = "CompanyLogo";
            public static string UserProfile = "UserProfileImage";
            public static string ContractEndDate = "ContractEndDate";
            public static string ClientProfilePic = "ClientProfilePic";
            public static string RoleFrontIcon = "RoleFrontIcon";
            public static string UTSFeedback = "UTSFeedback";
        }
        public static class HiringRequestAvailability
        {
            public static string FullTime = "Full Time";
            public static string PartTime = "Part Time";
        }

        #region CTA
        public static class HrCTA
        {
            public static string Clone = "Clone";
            public static string CloseHR = "CloseHR";
            public static string EditHR = "EditHR";
            public static string EditDirectHR = "EditDirectHR";
            public static string DebriefingHR = "DebriefingHR";
            public static string DebriefingDirectHR = "DebriefingDirectHR";
            public static string TalentMatchmaking = "TalentMatchmaking";
            public static string ConvertToDP = "ConvertToDP";
            public static string ConvertToContractual = "ConvertToContractual";
            public static string AMAssignment = "AMAssignment";
            public static string AcceptHR = "AcceptHR";
            public static string UpdateTR = "UpdateTR";
            public static string ShareAProfile = "ShareAProfile";
            public static string AddNotes = "AddNotes";
            public static string ReopenHR = "ReopenHR";
        }

        public static class TalentCTA
        {
            public static string TalentAcceptance = "TalentAcceptance";
            public static string ScheduleInterview = "ScheduleInterview";
            public static string InterviewStatus = "InterviewStatus";
            public static string RescheduleInterview = "RescheduleInterview";
            public static string ConfirmSlot = "ConfirmSlot";
            public static string InterviewerDetails = "InterviewerDetails";
            public static string ScheduleAnotherInterviewRound = "ScheduleAnotherInterviewRound";
            public static string AnotherRoundInterview = "AnotherRoundInterview";
            public static string SubmitClientFeedback = "SubmitClientFeedback";
            public static string EditClientFeedback = "EditClientFeedback";
            public static string GotoOnBoard = "GotoOnBoard";
            public static string UpdateClientOnBoardStatus = "UpdateClientOnBoardStatus";
            public static string UpdateTalentOnBoardStatus = "UpdateTalentOnBoardStatus";
            public static string UpdateLegalClientOnBoardStatus = "UpdateLegalClientOnBoardStatus";
            public static string UpdateLegalTalentOnBoardStatus = "UpdateLegalTalentOnBoardStatus";
            public static string UpdateKickOffOnBoardStatus = "UpdateKickOffOnBoardStatus";
            public static string TalentStatus = "TalentStatus";
            public static string ReplaceTalent = "ReplaceTalent";
            public static string AMAssignment = "AMAssignment";
            public static string ViewEngagement = "ViewEngagement";
            public static string TSCAssignment = "AssignTsc";
            public static string SubmitFeedbackWithNoHire = "SubmitFeedbackWithNoHire";
            public static string SubmitFeedbackWithAnotherRound = "SubmitFeedbackWithAnotherRound";
            public static string CancelEngagement = "CancelEngagement";
        }

        public static class OnBoardCTA
        {            
            public static string AMAssignment = "AMAssignment";
            public static string GotoOnBoard = "GotoOnBoard";
            public static string TSCAssignment = "TSCAssignment";
        }

        #endregion
    }
}
