using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Models.Models
{
    public partial class UTSATSAPIDBConnection : DbContext
    {

        public UTSATSAPIDBConnection(DbContextOptions<UTSATSAPIDBConnection> options)
            : base(options)
        {
        }

        public virtual DbSet<ColorCode> ColorCodes { get; set; } = null!;
        public virtual DbSet<CustomException> CustomExceptions { get; set; } = null!;
        public virtual DbSet<GenAibasedHrtalentMatchMaking> GenAibasedHrtalentMatchMakings { get; set; } = null!;
        public virtual DbSet<GenAibasedHrtalentMatchMakingDetail> GenAibasedHrtalentMatchMakingDetails { get; set; } = null!;
        public virtual DbSet<GenAmAssignmentRuleHistory> GenAmAssignmentRuleHistories { get; set; } = null!;
        public virtual DbSet<GenAmnbdAssignmentEmailsendDetail> GenAmnbdAssignmentEmailsendDetails { get; set; } = null!;
        public virtual DbSet<GenAssessmentWiseTalentDetail> GenAssessmentWiseTalentDetails { get; set; } = null!;
        public virtual DbSet<GenAtsHrstatusDetail> GenAtsHrstatusDetails { get; set; } = null!;
        public virtual DbSet<GenAtsHrstatusTalentDetail> GenAtsHrstatusTalentDetails { get; set; } = null!;
        public virtual DbSet<GenAtscompanyContactDataSend> GenAtscompanyContactDataSends { get; set; } = null!;
        public virtual DbSet<GenAtsupscreenApiRecordsClientPortal> GenAtsupscreenApiRecordsClientPortals { get; set; } = null!;
        public virtual DbSet<GenAvailableClientInHubSpotClientPortal> GenAvailableClientInHubSpotClientPortals { get; set; } = null!;
        public virtual DbSet<GenAwsEmailPayload> GenAwsEmailPayloads { get; set; } = null!;
        public virtual DbSet<GenAwsSesEmailSendingDetail> GenAwsSesEmailSendingDetails { get; set; } = null!;
        public virtual DbSet<GenAwsSesEmailTracking> GenAwsSesEmailTrackings { get; set; } = null!;
        public virtual DbSet<GenAwsTrackingEmailSubject> GenAwsTrackingEmailSubjects { get; set; } = null!;
        public virtual DbSet<GenBlog> GenBlogs { get; set; } = null!;
        public virtual DbSet<GenCcEmailId> GenCcEmailIds { get; set; } = null!;
        public virtual DbSet<GenChannelBookmarkMessage> GenChannelBookmarkMessages { get; set; } = null!;
        public virtual DbSet<GenChannelHistory> GenChannelHistories { get; set; } = null!;
        public virtual DbSet<GenChannelMessageReaction> GenChannelMessageReactions { get; set; } = null!;
        public virtual DbSet<GenChannelUserMessage> GenChannelUserMessages { get; set; } = null!;
        public virtual DbSet<GenClientActionHistorySenttoAtsLogClientPortal> GenClientActionHistorySenttoAtsLogClientPortals { get; set; } = null!;
        public virtual DbSet<GenClientAmAssignment> GenClientAmAssignments { get; set; } = null!;
        public virtual DbSet<GenClientAmAssignmentHistory> GenClientAmAssignmentHistories { get; set; } = null!;
        public virtual DbSet<GenClientCalendarEvent> GenClientCalendarEvents { get; set; } = null!;
        public virtual DbSet<GenClientHappinessSurvey> GenClientHappinessSurveys { get; set; } = null!;
        public virtual DbSet<GenClientHappinessSurveyOption> GenClientHappinessSurveyOptions { get; set; } = null!;
        public virtual DbSet<GenClientHappinessSurveyQuestion> GenClientHappinessSurveyQuestions { get; set; } = null!;
        public virtual DbSet<GenClientJob> GenClientJobs { get; set; } = null!;
        public virtual DbSet<GenClientPayOutAdditionalLineItem> GenClientPayOutAdditionalLineItems { get; set; } = null!;
        public virtual DbSet<GenClientenquiryDatum> GenClientenquiryData { get; set; } = null!;
        public virtual DbSet<GenCompany> GenCompanies { get; set; } = null!;
        public virtual DbSet<GenCompanyAmMappingHistory> GenCompanyAmMappingHistories { get; set; } = null!;
        public virtual DbSet<GenCompanyContractDetail> GenCompanyContractDetails { get; set; } = null!;
        public virtual DbSet<GenCompanyCultureandPerksDetail> GenCompanyCultureandPerksDetails { get; set; } = null!;
        public virtual DbSet<GenCompanyDetail> GenCompanyDetails { get; set; } = null!;
        public virtual DbSet<GenCompanyFundingDetail> GenCompanyFundingDetails { get; set; } = null!;
        public virtual DbSet<GenCompanyHistory> GenCompanyHistories { get; set; } = null!;
        public virtual DbSet<GenCompanyLeadTypeUserDetail> GenCompanyLeadTypeUserDetails { get; set; } = null!;
        public virtual DbSet<GenCompanyLegalInfo> GenCompanyLegalInfos { get; set; } = null!;
        public virtual DbSet<GenCompanyPerkDetail> GenCompanyPerkDetails { get; set; } = null!;
        public virtual DbSet<GenCompanyUpdate> GenCompanyUpdates { get; set; } = null!;
        public virtual DbSet<GenCompanyWeightedAverageCriterion> GenCompanyWeightedAverageCriteria { get; set; } = null!;
        public virtual DbSet<GenCompanyWhatsappDetail> GenCompanyWhatsappDetails { get; set; } = null!;
        public virtual DbSet<GenCompanyWhatsappMemberDetail> GenCompanyWhatsappMemberDetails { get; set; } = null!;
        public virtual DbSet<GenCompanyYouTubeLink> GenCompanyYouTubeLinks { get; set; } = null!;
        public virtual DbSet<GenContact> GenContacts { get; set; } = null!;
        public virtual DbSet<GenContactGetCallBackHistory> GenContactGetCallBackHistories { get; set; } = null!;
        public virtual DbSet<GenContactGetHelpQuestion> GenContactGetHelpQuestions { get; set; } = null!;
        public virtual DbSet<GenContactHistory> GenContactHistories { get; set; } = null!;
        public virtual DbSet<GenContactInterviewFeedback> GenContactInterviewFeedbacks { get; set; } = null!;
        public virtual DbSet<GenContactNotificationHistory> GenContactNotificationHistories { get; set; } = null!;
        public virtual DbSet<GenContactPendingAction> GenContactPendingActions { get; set; } = null!;
        public virtual DbSet<GenContactPointofContact> GenContactPointofContacts { get; set; } = null!;
        public virtual DbSet<GenContactTalentPriority> GenContactTalentPriorities { get; set; } = null!;
        public virtual DbSet<GenContactUsersClientPortal> GenContactUsersClientPortals { get; set; } = null!;
        public virtual DbSet<GenContractRenewalDetail> GenContractRenewalDetails { get; set; } = null!;
        public virtual DbSet<GenCrctpShortlistedTalentsClientPortal> GenCrctpShortlistedTalentsClientPortals { get; set; } = null!;
        public virtual DbSet<GenCustomAssessmentEmailDetail> GenCustomAssessmentEmailDetails { get; set; } = null!;
        public virtual DbSet<GenDeal> GenDeals { get; set; } = null!;
        public virtual DbSet<GenDealContact> GenDealContacts { get; set; } = null!;
        public virtual DbSet<GenDealTicket> GenDealTickets { get; set; } = null!;
        public virtual DbSet<GenDirectPlacement> GenDirectPlacements { get; set; } = null!;
        public virtual DbSet<GenEmailIdsWithNonEnterpriseList> GenEmailIdsWithNonEnterpriseLists { get; set; } = null!;
        public virtual DbSet<GenEmailList> GenEmailLists { get; set; } = null!;
        public virtual DbSet<GenEsalesAmApiResponse> GenEsalesAmApiResponses { get; set; } = null!;
        public virtual DbSet<GenFrontendClient> GenFrontendClients { get; set; } = null!;
        public virtual DbSet<GenGptJdresponse> GenGptJdresponses { get; set; } = null!;
        public virtual DbSet<GenHistory> GenHistories { get; set; } = null!;
        public virtual DbSet<GenHrAtstalentDetailsCtp> GenHrAtstalentDetailsCtps { get; set; } = null!;
        public virtual DbSet<GenHrcustomAssessment> GenHrcustomAssessments { get; set; } = null!;
        public virtual DbSet<GenHrnote> GenHrnotes { get; set; } = null!;
        public virtual DbSet<GenHrnotesTagUserDetail> GenHrnotesTagUserDetails { get; set; } = null!;
        public virtual DbSet<GenHubSpotCompany> GenHubSpotCompanies { get; set; } = null!;
        public virtual DbSet<GenHubSpotContact> GenHubSpotContacts { get; set; } = null!;
        public virtual DbSet<GenIncAmNrUserRoleDetail> GenIncAmNrUserRoleDetails { get; set; } = null!;
        public virtual DbSet<GenIncAmTalentDeployUserRoleDetail> GenIncAmTalentDeployUserRoleDetails { get; set; } = null!;
        public virtual DbSet<GenIncBaseSalUserRoleDetail> GenIncBaseSalUserRoleDetails { get; set; } = null!;
        public virtual DbSet<GenIncContractTalentUserRoleDetail> GenIncContractTalentUserRoleDetails { get; set; } = null!;
        public virtual DbSet<GenIncContractUserRoleDetail> GenIncContractUserRoleDetails { get; set; } = null!;
        public virtual DbSet<GenIncPlacementFeesTalentUserRoleDetail> GenIncPlacementFeesTalentUserRoleDetails { get; set; } = null!;
        public virtual DbSet<GenIncSalesIncentiveTargetDetail> GenIncSalesIncentiveTargetDetails { get; set; } = null!;
        public virtual DbSet<GenIncSalesIncentiveTargetSummary> GenIncSalesIncentiveTargetSummaries { get; set; } = null!;
        public virtual DbSet<GenIncSalesIncentiveTargetSummaryHistory> GenIncSalesIncentiveTargetSummaryHistories { get; set; } = null!;
        public virtual DbSet<GenIncTargetAcheivedDetail> GenIncTargetAcheivedDetails { get; set; } = null!;
        public virtual DbSet<GenIncUserTarget> GenIncUserTargets { get; set; } = null!;
        public virtual DbSet<GenIncUserTargetsHistory> GenIncUserTargetsHistories { get; set; } = null!;
        public virtual DbSet<GenInterviewSlotsMaster> GenInterviewSlotsMasters { get; set; } = null!;
        public virtual DbSet<GenJobPostEmploymentDetailsClientPortal> GenJobPostEmploymentDetailsClientPortals { get; set; } = null!;
        public virtual DbSet<GenJobPostRolesResponsibilitiesClientPortal> GenJobPostRolesResponsibilitiesClientPortals { get; set; } = null!;
        public virtual DbSet<GenJobPostStandoutToTalentsClientPortal> GenJobPostStandoutToTalentsClientPortals { get; set; } = null!;
        public virtual DbSet<GenJobPostSubscriptionHistoryClientPortal> GenJobPostSubscriptionHistoryClientPortals { get; set; } = null!;
        public virtual DbSet<GenJobPostTransactionHistoryClientPortal> GenJobPostTransactionHistoryClientPortals { get; set; } = null!;
        public virtual DbSet<GenJobPostVitalInfoClientPortal> GenJobPostVitalInfoClientPortals { get; set; } = null!;
        public virtual DbSet<GenManageNotificationEmailsHistory> GenManageNotificationEmailsHistories { get; set; } = null!;
        public virtual DbSet<GenManageNotificationEmailsHistoryForMonday> GenManageNotificationEmailsHistoryForMondays { get; set; } = null!;
        public virtual DbSet<GenManagedTalent> GenManagedTalents { get; set; } = null!;
        public virtual DbSet<GenManagedTalentHistory> GenManagedTalentHistories { get; set; } = null!;
        public virtual DbSet<GenMenuMaster> GenMenuMasters { get; set; } = null!;
        public virtual DbSet<GenMenuRight> GenMenuRights { get; set; } = null!;
        public virtual DbSet<GenNearByLocation> GenNearByLocations { get; set; } = null!;
        public virtual DbSet<GenNurtureEmailLog> GenNurtureEmailLogs { get; set; } = null!;
        public virtual DbSet<GenNurtureEmailSummary> GenNurtureEmailSummaries { get; set; } = null!;
        public virtual DbSet<GenOnBoardClientContractDetail> GenOnBoardClientContractDetails { get; set; } = null!;
        public virtual DbSet<GenOnBoardClientDevicePolicyDetail> GenOnBoardClientDevicePolicyDetails { get; set; } = null!;
        public virtual DbSet<GenOnBoardClientInvoiceSummary> GenOnBoardClientInvoiceSummaries { get; set; } = null!;
        public virtual DbSet<GenOnBoardClientLeavePolicy> GenOnBoardClientLeavePolicies { get; set; } = null!;
        public virtual DbSet<GenOnBoardClientTeam> GenOnBoardClientTeams { get; set; } = null!;
        public virtual DbSet<GenOnBoardTalent> GenOnBoardTalents { get; set; } = null!;
        public virtual DbSet<GenOnBoardTalentsClientFeedback> GenOnBoardTalentsClientFeedbacks { get; set; } = null!;
        public virtual DbSet<GenOnBoardTalentsLegalDetail> GenOnBoardTalentsLegalDetails { get; set; } = null!;
        public virtual DbSet<GenOnBoardTalentsReplacementDetail> GenOnBoardTalentsReplacementDetails { get; set; } = null!;
        public virtual DbSet<GenOnlyDealId> GenOnlyDealIds { get; set; } = null!;
        public virtual DbSet<GenPayPerViewModelClientPortal> GenPayPerViewModelClientPortals { get; set; } = null!;
        public virtual DbSet<GenPaymentHistoryClientPortal> GenPaymentHistoryClientPortals { get; set; } = null!;
        public virtual DbSet<GenPaymentOrderDetail> GenPaymentOrderDetails { get; set; } = null!;
        public virtual DbSet<GenPayoutInformation> GenPayoutInformations { get; set; } = null!;
        public virtual DbSet<GenPmsTalentRedirectUrlDetail> GenPmsTalentRedirectUrlDetails { get; set; } = null!;
        public virtual DbSet<GenPostAcceptanceDetail> GenPostAcceptanceDetails { get; set; } = null!;
        public virtual DbSet<GenPostJobEmailNotificationClientPortal> GenPostJobEmailNotificationClientPortals { get; set; } = null!;
        public virtual DbSet<GenPostalCodeswithDetail> GenPostalCodeswithDetails { get; set; } = null!;
        public virtual DbSet<GenRazorPayWebhookHistory> GenRazorPayWebhookHistories { get; set; } = null!;
        public virtual DbSet<GenRoleAndHiringTypeClientPortal> GenRoleAndHiringTypeClientPortals { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequest> GenSalesHiringRequests { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequestChannel> GenSalesHiringRequestChannels { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequestChannelUserDetail> GenSalesHiringRequestChannelUserDetails { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequestDetail> GenSalesHiringRequestDetails { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequestDetailsHistory> GenSalesHiringRequestDetailsHistories { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequestHistory> GenSalesHiringRequestHistories { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequestInterviewerDetail> GenSalesHiringRequestInterviewerDetails { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequestPriority> GenSalesHiringRequestPriorities { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequestPriorityHistory> GenSalesHiringRequestPriorityHistories { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequestSkillDetail> GenSalesHiringRequestSkillDetails { get; set; } = null!;
        public virtual DbSet<GenSalesHiringRequestTalentPacketDetail> GenSalesHiringRequestTalentPacketDetails { get; set; } = null!;
        public virtual DbSet<GenSalesHrJddump> GenSalesHrJddumps { get; set; } = null!;
        public virtual DbSet<GenSalesHrJddumpSkillDetail> GenSalesHrJddumpSkillDetails { get; set; } = null!;
        public virtual DbSet<GenSalesHrTracceptedDetail> GenSalesHrTracceptedDetails { get; set; } = null!;
        public virtual DbSet<GenSalesHrTrupdatedDetail> GenSalesHrTrupdatedDetails { get; set; } = null!;
        public virtual DbSet<GenSalesHrwiseContactIdforAt> GenSalesHrwiseContactIdforAts { get; set; } = null!;
        public virtual DbSet<GenSchedularHistory> GenSchedularHistories { get; set; } = null!;
        public virtual DbSet<GenShortistedTalentUnlockDetailClientPortal> GenShortistedTalentUnlockDetailClientPortals { get; set; } = null!;
        public virtual DbSet<GenShortlistedTalent> GenShortlistedTalents { get; set; } = null!;
        public virtual DbSet<GenShortlistedTalentInterviewDetail> GenShortlistedTalentInterviewDetails { get; set; } = null!;
        public virtual DbSet<GenShortlistedTalentsClientPortal> GenShortlistedTalentsClientPortals { get; set; } = null!;
        public virtual DbSet<GenSkillAndBudgetClientPortal> GenSkillAndBudgetClientPortals { get; set; } = null!;
        public virtual DbSet<GenSlaEditHistory> GenSlaEditHistories { get; set; } = null!;
        public virtual DbSet<GenSocialMediaLoginDetail> GenSocialMediaLoginDetails { get; set; } = null!;
        public virtual DbSet<GenSystemConfiguration> GenSystemConfigurations { get; set; } = null!;
        public virtual DbSet<GenTalent> GenTalents { get; set; } = null!;
        public virtual DbSet<GenTalentApplicationTool> GenTalentApplicationTools { get; set; } = null!;
        public virtual DbSet<GenTalentAssesmentScore> GenTalentAssesmentScores { get; set; } = null!;
        public virtual DbSet<GenTalentAssesmentSkillScore> GenTalentAssesmentSkillScores { get; set; } = null!;
        public virtual DbSet<GenTalentBackOutDetailsForAt> GenTalentBackOutDetailsForAts { get; set; } = null!;
        public virtual DbSet<GenTalentCertificationsDetail> GenTalentCertificationsDetails { get; set; } = null!;
        public virtual DbSet<GenTalentClientTele> GenTalentClientTeles { get; set; } = null!;
        public virtual DbSet<GenTalentComfortableAreaDetail> GenTalentComfortableAreaDetails { get; set; } = null!;
        public virtual DbSet<GenTalentComfortableWorkingDetail> GenTalentComfortableWorkingDetails { get; set; } = null!;
        public virtual DbSet<GenTalentEducationDetail> GenTalentEducationDetails { get; set; } = null!;
        public virtual DbSet<GenTalentFinancialDetail> GenTalentFinancialDetails { get; set; } = null!;
        public virtual DbSet<GenTalentGetHelpQuestion> GenTalentGetHelpQuestions { get; set; } = null!;
        public virtual DbSet<GenTalentHistory> GenTalentHistories { get; set; } = null!;
        public virtual DbSet<GenTalentInterestDetail> GenTalentInterestDetails { get; set; } = null!;
        public virtual DbSet<GenTalentInterviewFeedback> GenTalentInterviewFeedbacks { get; set; } = null!;
        public virtual DbSet<GenTalentKeyQualityDetail> GenTalentKeyQualityDetails { get; set; } = null!;
        public virtual DbSet<GenTalentLegalInfo> GenTalentLegalInfos { get; set; } = null!;
        public virtual DbSet<GenTalentNotesClientPortal> GenTalentNotesClientPortals { get; set; } = null!;
        public virtual DbSet<GenTalentNotificationHistory> GenTalentNotificationHistories { get; set; } = null!;
        public virtual DbSet<GenTalentPayOutAdditionalLineItem> GenTalentPayOutAdditionalLineItems { get; set; } = null!;
        public virtual DbSet<GenTalentPendingAction> GenTalentPendingActions { get; set; } = null!;
        public virtual DbSet<GenTalentPointofContact> GenTalentPointofContacts { get; set; } = null!;
        public virtual DbSet<GenTalentPreferableWorkingDetail> GenTalentPreferableWorkingDetails { get; set; } = null!;
        public virtual DbSet<GenTalentPrimarySkillDetail> GenTalentPrimarySkillDetails { get; set; } = null!;
        public virtual DbSet<GenTalentProfessionalExperience> GenTalentProfessionalExperiences { get; set; } = null!;
        public virtual DbSet<GenTalentProjectDetail> GenTalentProjectDetails { get; set; } = null!;
        public virtual DbSet<GenTalentResumeDetail> GenTalentResumeDetails { get; set; } = null!;
        public virtual DbSet<GenTalentSecondarySkillDetail> GenTalentSecondarySkillDetails { get; set; } = null!;
        public virtual DbSet<GenTalentSelectedInterviewDetail> GenTalentSelectedInterviewDetails { get; set; } = null!;
        public virtual DbSet<GenTalentSelectedInterviewerDetail> GenTalentSelectedInterviewerDetails { get; set; } = null!;
        public virtual DbSet<GenTalentSelectedNextRoundInterviewDetail> GenTalentSelectedNextRoundInterviewDetails { get; set; } = null!;
        public virtual DbSet<GenTalentSelectedPostAcceptanceDetail> GenTalentSelectedPostAcceptanceDetails { get; set; } = null!;
        public virtual DbSet<GenTalentSelectedPostAcceptanceNotMatchDetail> GenTalentSelectedPostAcceptanceNotMatchDetails { get; set; } = null!;
        public virtual DbSet<GenTalentSelectionHistory> GenTalentSelectionHistories { get; set; } = null!;
        public virtual DbSet<GenTalentTestimonialOrFeedback> GenTalentTestimonialOrFeedbacks { get; set; } = null!;
        public virtual DbSet<GenTeam> GenTeams { get; set; } = null!;
        public virtual DbSet<GenTeamDistribution> GenTeamDistributions { get; set; } = null!;
        public virtual DbSet<GenTestimonial> GenTestimonials { get; set; } = null!;
        public virtual DbSet<GenTicket> GenTickets { get; set; } = null!;
        public virtual DbSet<GenTrackingLeadClientPortal> GenTrackingLeadClientPortals { get; set; } = null!;
        public virtual DbSet<GenTransparentPricingModelClientPortal> GenTransparentPricingModelClientPortals { get; set; } = null!;
        public virtual DbSet<GenTscuser> GenTscusers { get; set; } = null!;
        public virtual DbSet<GenUnicornRoleWithTalentDetailClientPortal> GenUnicornRoleWithTalentDetailClientPortals { get; set; } = null!;
        public virtual DbSet<GenUserHistory> GenUserHistories { get; set; } = null!;
        public virtual DbSet<GenUtsAssignmentHistory> GenUtsAssignmentHistories { get; set; } = null!;
        public virtual DbSet<GenUtsAtsApiRecord> GenUtsAtsApiRecords { get; set; } = null!;
        public virtual DbSet<GenUtsOnBoardLineItem> GenUtsOnBoardLineItems { get; set; } = null!;
        public virtual DbSet<GenUtsadminReactPayload> GenUtsadminReactPayloads { get; set; } = null!;
        public virtual DbSet<GenUtsatsapiQueue> GenUtsatsapiQueues { get; set; } = null!;
        public virtual DbSet<GenUtsreactApiRecord> GenUtsreactApiRecords { get; set; } = null!;
        public virtual DbSet<HubSpotEventType> HubSpotEventTypes { get; set; } = null!;
        public virtual DbSet<HubspotWebhookNotification> HubspotWebhookNotifications { get; set; } = null!;
        public virtual DbSet<PrgAchievementWithUplersReason> PrgAchievementWithUplersReasons { get; set; } = null!;
        public virtual DbSet<PrgActionFilter> PrgActionFilters { get; set; } = null!;
        public virtual DbSet<PrgAmAssignmentRule> PrgAmAssignmentRules { get; set; } = null!;
        public virtual DbSet<PrgAmdealContact> PrgAmdealContacts { get; set; } = null!;
        public virtual DbSet<PrgApitimeZoneResponse> PrgApitimeZoneResponses { get; set; } = null!;
        public virtual DbSet<PrgAssessmentType> PrgAssessmentTypes { get; set; } = null!;
        public virtual DbSet<PrgAtshrstatus> PrgAtshrstatuses { get; set; } = null!;
        public virtual DbSet<PrgBenchMark> PrgBenchMarks { get; set; } = null!;
        public virtual DbSet<PrgCityStateWise> PrgCityStateWises { get; set; } = null!;
        public virtual DbSet<PrgClientEngagementModel> PrgClientEngagementModels { get; set; } = null!;
        public virtual DbSet<PrgClientHappinessSurveyFeedbackOption> PrgClientHappinessSurveyFeedbackOptions { get; set; } = null!;
        public virtual DbSet<PrgClientHappinessSurveyFeedbackQuestion> PrgClientHappinessSurveyFeedbackQuestions { get; set; } = null!;
        public virtual DbSet<PrgClientInvoiceStatus> PrgClientInvoiceStatuses { get; set; } = null!;
        public virtual DbSet<PrgClientLegalDocumentType> PrgClientLegalDocumentTypes { get; set; } = null!;
        public virtual DbSet<PrgClientStage> PrgClientStages { get; set; } = null!;
        public virtual DbSet<PrgClientStageWithHubSpotStage> PrgClientStageWithHubSpotStages { get; set; } = null!;
        public virtual DbSet<PrgCompanyCurrentOpenPosition> PrgCompanyCurrentOpenPositions { get; set; } = null!;
        public virtual DbSet<PrgCompanyDomain> PrgCompanyDomains { get; set; } = null!;
        public virtual DbSet<PrgCompanyPerk> PrgCompanyPerks { get; set; } = null!;
        public virtual DbSet<PrgCompanySize> PrgCompanySizes { get; set; } = null!;
        public virtual DbSet<PrgCompanyTargetGroup> PrgCompanyTargetGroups { get; set; } = null!;
        public virtual DbSet<PrgCompanyType> PrgCompanyTypes { get; set; } = null!;
        public virtual DbSet<PrgCompanyTypeofInterviewer> PrgCompanyTypeofInterviewers { get; set; } = null!;
        public virtual DbSet<PrgContactAction> PrgContactActions { get; set; } = null!;
        public virtual DbSet<PrgContactStatus> PrgContactStatuses { get; set; } = null!;
        public virtual DbSet<PrgContactTimeZone> PrgContactTimeZones { get; set; } = null!;
        public virtual DbSet<PrgCountryRegion> PrgCountryRegions { get; set; } = null!;
        public virtual DbSet<PrgCreditOptionClientPortal> PrgCreditOptionClientPortals { get; set; } = null!;
        public virtual DbSet<PrgCreditOptionsWithHrtypeId> PrgCreditOptionsWithHrtypeIds { get; set; } = null!;
        public virtual DbSet<PrgCreditWithAmountUtilizationClientPortal> PrgCreditWithAmountUtilizationClientPortals { get; set; } = null!;
        public virtual DbSet<PrgCrmApiDetail> PrgCrmApiDetails { get; set; } = null!;
        public virtual DbSet<PrgCrmApiPartner> PrgCrmApiPartners { get; set; } = null!;
        public virtual DbSet<PrgCrmApiTestsResponse> PrgCrmApiTestsResponses { get; set; } = null!;
        public virtual DbSet<PrgCurrencyExchangeRate> PrgCurrencyExchangeRates { get; set; } = null!;
        public virtual DbSet<PrgDealPipeline> PrgDealPipelines { get; set; } = null!;
        public virtual DbSet<PrgDealStage> PrgDealStages { get; set; } = null!;
        public virtual DbSet<PrgDealStatus> PrgDealStatuses { get; set; } = null!;
        public virtual DbSet<PrgDealStatusIdpipelineStageId> PrgDealStatusIdpipelineStageIds { get; set; } = null!;
        public virtual DbSet<PrgDealType> PrgDealTypes { get; set; } = null!;
        public virtual DbSet<PrgDepartment> PrgDepartments { get; set; } = null!;
        public virtual DbSet<PrgEditBrprReason> PrgEditBrprReasons { get; set; } = null!;
        public virtual DbSet<PrgEmploymentStatus> PrgEmploymentStatuses { get; set; } = null!;
        public virtual DbSet<PrgEngagementStatus> PrgEngagementStatuses { get; set; } = null!;
        public virtual DbSet<PrgExtractedSkillsforJdparsing> PrgExtractedSkillsforJdparsings { get; set; } = null!;
        public virtual DbSet<PrgFrequencyOfficeVisit> PrgFrequencyOfficeVisits { get; set; } = null!;
        public virtual DbSet<PrgGeo> PrgGeos { get; set; } = null!;
        public virtual DbSet<PrgHiringRequestReportStage> PrgHiringRequestReportStages { get; set; } = null!;
        public virtual DbSet<PrgHiringRequestRoleStatus> PrgHiringRequestRoleStatuses { get; set; } = null!;
        public virtual DbSet<PrgHiringRequestStatus> PrgHiringRequestStatuses { get; set; } = null!;
        public virtual DbSet<PrgHiringRequestType> PrgHiringRequestTypes { get; set; } = null!;
        public virtual DbSet<PrgHiringTypePricing> PrgHiringTypePricings { get; set; } = null!;
        public virtual DbSet<PrgHiringTypeTransparentPricing> PrgHiringTypeTransparentPricings { get; set; } = null!;
        public virtual DbSet<PrgHistoryAction> PrgHistoryActions { get; set; } = null!;
        public virtual DbSet<PrgHistoryChannelAction> PrgHistoryChannelActions { get; set; } = null!;
        public virtual DbSet<PrgHowSoonClientWantTalent> PrgHowSoonClientWantTalents { get; set; } = null!;
        public virtual DbSet<PrgHrStatusDisplay> PrgHrStatusDisplays { get; set; } = null!;
        public virtual DbSet<PrgHrcloseReason> PrgHrcloseReasons { get; set; } = null!;
        public virtual DbSet<PrgHrdeleteReason> PrgHrdeleteReasons { get; set; } = null!;
        public virtual DbSet<PrgHrtalentTag> PrgHrtalentTags { get; set; } = null!;
        public virtual DbSet<PrgIncAmNrSlab> PrgIncAmNrSlabs { get; set; } = null!;
        public virtual DbSet<PrgIncAmSlab> PrgIncAmSlabs { get; set; } = null!;
        public virtual DbSet<PrgIncContractsSlab> PrgIncContractsSlabs { get; set; } = null!;
        public virtual DbSet<PrgIncPlacementFeesSlab> PrgIncPlacementFeesSlabs { get; set; } = null!;
        public virtual DbSet<PrgIncSalgoalSlab> PrgIncSalgoalSlabs { get; set; } = null!;
        public virtual DbSet<PrgInterviewStatus> PrgInterviewStatuses { get; set; } = null!;
        public virtual DbSet<PrgJobPostStageMessage> PrgJobPostStageMessages { get; set; } = null!;
        public virtual DbSet<PrgJobStatusClientPortal> PrgJobStatusClientPortals { get; set; } = null!;
        public virtual DbSet<PrgJobType> PrgJobTypes { get; set; } = null!;
        public virtual DbSet<PrgMatchMakingFactor> PrgMatchMakingFactors { get; set; } = null!;
        public virtual DbSet<PrgMatchMakingSubFactor> PrgMatchMakingSubFactors { get; set; } = null!;
        public virtual DbSet<PrgModeOfWorking> PrgModeOfWorkings { get; set; } = null!;
        public virtual DbSet<PrgNbdfunnelStage> PrgNbdfunnelStages { get; set; } = null!;
        public virtual DbSet<PrgOnBoardLegalKickOffStatus> PrgOnBoardLegalKickOffStatuses { get; set; } = null!;
        public virtual DbSet<PrgOnBoardPolicyDeviceMaster> PrgOnBoardPolicyDeviceMasters { get; set; } = null!;
        public virtual DbSet<PrgOnboardLostReason> PrgOnboardLostReasons { get; set; } = null!;
        public virtual DbSet<PrgOrgLevel> PrgOrgLevels { get; set; } = null!;
        public virtual DbSet<PrgPartialEngagementType> PrgPartialEngagementTypes { get; set; } = null!;
        public virtual DbSet<PrgPauseHrReason> PrgPauseHrReasons { get; set; } = null!;
        public virtual DbSet<PrgPayrollType> PrgPayrollTypes { get; set; } = null!;
        public virtual DbSet<PrgPerk> PrgPerks { get; set; } = null!;
        public virtual DbSet<PrgPipeline> PrgPipelines { get; set; } = null!;
        public virtual DbSet<PrgPitchMeRole> PrgPitchMeRoles { get; set; } = null!;
        public virtual DbSet<PrgPreOnBoardStatus> PrgPreOnBoardStatuses { get; set; } = null!;
        public virtual DbSet<PrgRateCard> PrgRateCards { get; set; } = null!;
        public virtual DbSet<PrgRefUrl> PrgRefUrls { get; set; } = null!;
        public virtual DbSet<PrgRejectionMsgForTalent> PrgRejectionMsgForTalents { get; set; } = null!;
        public virtual DbSet<PrgRequirement> PrgRequirements { get; set; } = null!;
        public virtual DbSet<PrgRescheduleInterviewReason> PrgRescheduleInterviewReasons { get; set; } = null!;
        public virtual DbSet<PrgRolesAndResponsilbity> PrgRolesAndResponsilbities { get; set; } = null!;
        public virtual DbSet<PrgSkill> PrgSkills { get; set; } = null!;
        public virtual DbSet<PrgSkillCategory> PrgSkillCategories { get; set; } = null!;
        public virtual DbSet<PrgSkillRank> PrgSkillRanks { get; set; } = null!;
        public virtual DbSet<PrgSkillSource> PrgSkillSources { get; set; } = null!;
        public virtual DbSet<PrgSkillsInAssesment> PrgSkillsInAssesments { get; set; } = null!;
        public virtual DbSet<PrgSkillsRole> PrgSkillsRoles { get; set; } = null!;
        public virtual DbSet<PrgSlaEditReason> PrgSlaEditReasons { get; set; } = null!;
        public virtual DbSet<PrgSlatime> PrgSlatimes { get; set; } = null!;
        public virtual DbSet<PrgSocialProvider> PrgSocialProviders { get; set; } = null!;
        public virtual DbSet<PrgState> PrgStates { get; set; } = null!;
        public virtual DbSet<PrgStateDistrict> PrgStateDistricts { get; set; } = null!;
        public virtual DbSet<PrgStateSubDistrict> PrgStateSubDistricts { get; set; } = null!;
        public virtual DbSet<PrgSubscriptionPackageClientPortal> PrgSubscriptionPackageClientPortals { get; set; } = null!;
        public virtual DbSet<PrgSummaryStagesForReport> PrgSummaryStagesForReports { get; set; } = null!;
        public virtual DbSet<PrgTalentAccountStatus> PrgTalentAccountStatuses { get; set; } = null!;
        public virtual DbSet<PrgTalentAction> PrgTalentActions { get; set; } = null!;
        public virtual DbSet<PrgTalentApplicationTool> PrgTalentApplicationTools { get; set; } = null!;
        public virtual DbSet<PrgTalentAssociatedWithUpler> PrgTalentAssociatedWithUplers { get; set; } = null!;
        public virtual DbSet<PrgTalentCategory> PrgTalentCategories { get; set; } = null!;
        public virtual DbSet<PrgTalentCertification> PrgTalentCertifications { get; set; } = null!;
        public virtual DbSet<PrgTalentComfortableArea> PrgTalentComfortableAreas { get; set; } = null!;
        public virtual DbSet<PrgTalentComfortableWorking> PrgTalentComfortableWorkings { get; set; } = null!;
        public virtual DbSet<PrgTalentCommunicationSkill> PrgTalentCommunicationSkills { get; set; } = null!;
        public virtual DbSet<PrgTalentInterest> PrgTalentInterests { get; set; } = null!;
        public virtual DbSet<PrgTalentJoinning> PrgTalentJoinnings { get; set; } = null!;
        public virtual DbSet<PrgTalentKeyQuality> PrgTalentKeyQualities { get; set; } = null!;
        public virtual DbSet<PrgTalentLegalDocumentType> PrgTalentLegalDocumentTypes { get; set; } = null!;
        public virtual DbSet<PrgTalentPayoutStatus> PrgTalentPayoutStatuses { get; set; } = null!;
        public virtual DbSet<PrgTalentPreferableWorking> PrgTalentPreferableWorkings { get; set; } = null!;
        public virtual DbSet<PrgTalentPrimarySkill> PrgTalentPrimarySkills { get; set; } = null!;
        public virtual DbSet<PrgTalentRejectReason> PrgTalentRejectReasons { get; set; } = null!;
        public virtual DbSet<PrgTalentRole> PrgTalentRoles { get; set; } = null!;
        public virtual DbSet<PrgTalentRoleCategory> PrgTalentRoleCategories { get; set; } = null!;
        public virtual DbSet<PrgTalentRoleTemplate> PrgTalentRoleTemplates { get; set; } = null!;
        public virtual DbSet<PrgTalentSecondarySkill> PrgTalentSecondarySkills { get; set; } = null!;
        public virtual DbSet<PrgTalentSkillBenchMark> PrgTalentSkillBenchMarks { get; set; } = null!;
        public virtual DbSet<PrgTalentStatus> PrgTalentStatuses { get; set; } = null!;
        public virtual DbSet<PrgTalentStatusAfterClientSelection> PrgTalentStatusAfterClientSelections { get; set; } = null!;
        public virtual DbSet<PrgTalentStatusClientPortal> PrgTalentStatusClientPortals { get; set; } = null!;
        public virtual DbSet<PrgTalentTimeZone> PrgTalentTimeZones { get; set; } = null!;
        public virtual DbSet<PrgTalentType> PrgTalentTypes { get; set; } = null!;
        public virtual DbSet<PrgTempApplicationTool> PrgTempApplicationTools { get; set; } = null!;
        public virtual DbSet<PrgTempApplicationToolsMergePrgTalentApplicationTool> PrgTempApplicationToolsMergePrgTalentApplicationTools { get; set; } = null!;
        public virtual DbSet<PrgTempCertification> PrgTempCertifications { get; set; } = null!;
        public virtual DbSet<PrgTempCertificationsMergePrgTalentCertification> PrgTempCertificationsMergePrgTalentCertifications { get; set; } = null!;
        public virtual DbSet<PrgTempInterest> PrgTempInterests { get; set; } = null!;
        public virtual DbSet<PrgTempInterestMergePrgTalentInterest> PrgTempInterestMergePrgTalentInterests { get; set; } = null!;
        public virtual DbSet<PrgTempSkill> PrgTempSkills { get; set; } = null!;
        public virtual DbSet<PrgTempSkillsMergePrgSkill> PrgTempSkillsMergePrgSkills { get; set; } = null!;
        public virtual DbSet<PrgTicketCategory> PrgTicketCategories { get; set; } = null!;
        public virtual DbSet<PrgTicketStatus> PrgTicketStatuses { get; set; } = null!;
        public virtual DbSet<PrgTicketTechnicalJustification> PrgTicketTechnicalJustifications { get; set; } = null!;
        public virtual DbSet<PrgTimeZone> PrgTimeZones { get; set; } = null!;
        public virtual DbSet<PrgTimeZoneCostPreference> PrgTimeZoneCostPreferences { get; set; } = null!;
        public virtual DbSet<PrgTimeZonePreference> PrgTimeZonePreferences { get; set; } = null!;
        public virtual DbSet<PrgUnicornRolesClientPortal> PrgUnicornRolesClientPortals { get; set; } = null!;
        public virtual DbSet<PrgUserRoleClientPortal> PrgUserRoleClientPortals { get; set; } = null!;
        public virtual DbSet<PrgUtmCampaign> PrgUtmCampaigns { get; set; } = null!;
        public virtual DbSet<PrgUtmContent> PrgUtmContents { get; set; } = null!;
        public virtual DbSet<PrgUtmMedium> PrgUtmMedia { get; set; } = null!;
        public virtual DbSet<PrgUtmPlacement> PrgUtmPlacements { get; set; } = null!;
        public virtual DbSet<PrgUtmSource> PrgUtmSources { get; set; } = null!;
        public virtual DbSet<PrgUtmTerm> PrgUtmTerms { get; set; } = null!;
        public virtual DbSet<RestoreChannel> RestoreChannels { get; set; } = null!;
        public virtual DbSet<SkillRole1> SkillRole1s { get; set; } = null!;
        public virtual DbSet<StoreApiurl> StoreApiurls { get; set; } = null!;
        public virtual DbSet<Testtable> Testtables { get; set; } = null!;
        public virtual DbSet<UsrUser> UsrUsers { get; set; } = null!;
        public virtual DbSet<UsrUserGeoDetail> UsrUserGeoDetails { get; set; } = null!;
        public virtual DbSet<UsrUserHierarchy> UsrUserHierarchies { get; set; } = null!;
        public virtual DbSet<UsrUserPriorityCountLog> UsrUserPriorityCountLogs { get; set; } = null!;
        public virtual DbSet<UsrUserRole> UsrUserRoles { get; set; } = null!;
        public virtual DbSet<UsrUserRoleDetail> UsrUserRoleDetails { get; set; } = null!;
        public virtual DbSet<UsrUserTeam> UsrUserTeams { get; set; } = null!;
        public virtual DbSet<UsrUserType> UsrUserTypes { get; set; } = null!;
        public virtual DbSet<UsrUserTypeAndRoleDetail> UsrUserTypeAndRoleDetails { get; set; } = null!;
        public virtual DbSet<UsrUserTypeKeyoutcome> UsrUserTypeKeyoutcomes { get; set; } = null!;

        #region All Complex Types
        public virtual DbSet<Sproc_Update_Basic_CompanyDetails_Result> Sproc_Update_Basic_CompanyDetails_Result { get; set; } = null!;
        public virtual DbSet<sproc_UTS_UpdateContactDetails_Result> sproc_UTS_UpdateContactDetails_Result { get; set; } = null!;
        public virtual DbSet<Sproc_Add_Company_Transactions_With_ATS_Result> Sproc_Add_Company_Transactions_With_ATS_Result { get; set; } = null!;
        public virtual DbSet<Sproc_UTS_AddEdit_ATSHR_Result> Sproc_UTS_AddEdit_ATSHR_Result { get; set; } = null!;
        public virtual DbSet<Sproc_CurrencyExchangeRate_Result> Sproc_CurrencyExchangeRate_Result { get; set; } = null!;
        public virtual DbSet<sp_UTS_get_PasswordData_Result> sp_UTS_get_PasswordData_Result { get; set; } = null!;
       
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=27.109.9.66;Database=TalentConnect; user id=himani; password=himani@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ColorCode>(entity =>
            {
                entity.ToTable("color_codes");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.HexCode)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("hex_code");
            });

            modelBuilder.Entity<CustomException>(entity =>
            {
                entity.ToTable("CustomException");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ErrorMessage).HasColumnType("text");

                entity.Property(e => e.ExceptionSource).HasMaxLength(200);

                entity.Property(e => e.FunctionName).HasMaxLength(200);

                entity.Property(e => e.MachineName).HasMaxLength(100);

                entity.Property(e => e.RequestUrl).HasColumnName("RequestURL");

                entity.Property(e => e.UserAgent).HasMaxLength(200);
            });

            modelBuilder.Entity<GenAibasedHrtalentMatchMaking>(entity =>
            {
                entity.ToTable("gen_AIBased_HRTalentMatchMaking");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionId).HasColumnName("ActionID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestIds).HasColumnName("HiringRequest_IDs");

                entity.Property(e => e.IsMultipleHr)
                    .HasColumnName("IsMultipleHR")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.MatchingPercentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");
            });

            modelBuilder.Entity<GenAibasedHrtalentMatchMakingDetail>(entity =>
            {
                entity.ToTable("gen_AIBased_HRTalentMatchMaking_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AiId).HasColumnName("AI_ID");

                entity.Property(e => e.CalcWeightage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.FactorWiseCalcWeightage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HiringRequestIds).HasColumnName("HiringRequest_IDs");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.MatchingFactorCalcId).HasColumnName("MatchingFactor_CalcID");

                entity.Property(e => e.MatchingFactorCalcValue)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("MatchingFactor_CalcValue");

                entity.Property(e => e.MatchingFactorDisplayValue)
                    .HasMaxLength(500)
                    .HasColumnName("MatchingFactor_DisplayValue");

                entity.Property(e => e.MatchingSubFactorId).HasColumnName("MatchingSubFactor_ID");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TotalHrskills).HasColumnName("TotalHRSkills");
            });

            modelBuilder.Entity<GenAmAssignmentRuleHistory>(entity =>
            {
                entity.ToTable("gen_AM_AssignmentRuleHistory");

                entity.Property(e => e.AmassignmentRuleEngagementModelId)
                    .HasMaxLength(100)
                    .HasColumnName("AMAssignmentRuleEngagementModelID");

                entity.Property(e => e.AmassignmentRuleId).HasColumnName("AMAssignmentRuleID");

                entity.Property(e => e.AmuserId).HasColumnName("AMUserID");

                entity.Property(e => e.BauUtcTagging)
                    .HasMaxLength(20)
                    .HasColumnName("BAU_UTC_Tagging");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EngagemenId)
                    .HasMaxLength(50)
                    .HasColumnName("EngagemenID");

                entity.Property(e => e.Geoid).HasColumnName("GEOID");

                entity.Property(e => e.HrNumber)
                    .HasMaxLength(50)
                    .HasColumnName("HR_Number");

                entity.Property(e => e.InvoiceGuid)
                    .HasMaxLength(50)
                    .HasColumnName("InvoiceGUID");

                entity.Property(e => e.InvoiceHistoryId).HasColumnName("InvoiceHistoryID");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.IsCreatedFromUts)
                    .HasColumnName("IsCreatedFromUTS")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<GenAmnbdAssignmentEmailsendDetail>(entity =>
            {
                entity.ToTable("gen_AMNBD_Assignment_EmailsendDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmSalesPersonId).HasColumnName("AM_SalesPersonID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.NbdSalesPersonId).HasColumnName("NBD_SalesPersonID");
            });

            modelBuilder.Entity<GenAssessmentWiseTalentDetail>(entity =>
            {
                entity.ToTable("gen_AssessmentWiseTalentDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssessmentId).HasColumnName("AssessmentID");

                entity.Property(e => e.AtstalentId).HasColumnName("ATSTalentID");
            });

            modelBuilder.Entity<GenAtsHrstatusDetail>(entity =>
            {
                entity.ToTable("gen_ATS_HRStatus_Details");

                entity.HasIndex(e => e.Hrid, "NonClusteredIndex-20231006-114706");

                entity.Property(e => e.AtshrstatusId).HasColumnName("ATSHRStatusId");

                entity.Property(e => e.CreatedByDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedById).HasColumnName("createdByID");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.ModifiedByDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.PublishedDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("Published_Datetime");

                entity.Property(e => e.TotalTalent).HasColumnName("Total_Talent");
            });

            modelBuilder.Entity<GenAtsHrstatusTalentDetail>(entity =>
            {
                entity.ToTable("gen_ATS_HRStatus_TalentDetails");

                entity.Property(e => e.AssociatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.AtsTalentId).HasColumnName("ATS_TalentID");

                entity.Property(e => e.CreatedByDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedById).HasColumnName("createdByID");

                entity.Property(e => e.HrstatusDetailsId).HasColumnName("HRStatus_DetailsID");

                entity.Property(e => e.ModifiedByDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");
            });

            modelBuilder.Entity<GenAtscompanyContactDataSend>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("gen_ATSCompanyContactDataSend");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.Isactive).HasColumnName("ISActive");
            });

            modelBuilder.Entity<GenAtsupscreenApiRecordsClientPortal>(entity =>
            {
                entity.ToTable("gen_ATSUpscreenApi_Records_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.FromApiUrl).HasMaxLength(200);

                entity.Property(e => e.ToApiUrl).HasMaxLength(200);
            });

            modelBuilder.Entity<GenAvailableClientInHubSpotClientPortal>(entity =>
            {
                entity.ToTable("gen_Available_ClientInHubSpot_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientEmail).HasMaxLength(100);

                entity.Property(e => e.ClientName).HasMaxLength(100);

                entity.Property(e => e.Company).HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.HubSpotCompanyId).HasColumnName("HubSpotCompanyID");

                entity.Property(e => e.HubSpotContactId).HasColumnName("HubSpotContactID");
            });

            modelBuilder.Entity<GenAwsEmailPayload>(entity =>
            {
                entity.ToTable("gen_AWS_Email_Payload");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<GenAwsSesEmailSendingDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("gen_AWS_SES_EmailSending_Details");

                entity.Property(e => e.ClientDestination)
                    .HasMaxLength(500)
                    .HasColumnName("Client_Destination");

                entity.Property(e => e.EmailSubject).HasColumnName("Email_Subject");

                entity.Property(e => e.EventType).HasMaxLength(100);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("timestamp");
            });

            modelBuilder.Entity<GenAwsSesEmailTracking>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("gen_AWS_SES_EmailTracking");

                entity.Property(e => e.BounceSubType)
                    .HasMaxLength(200)
                    .HasColumnName("bounceSubType");

                entity.Property(e => e.BounceType)
                    .HasMaxLength(100)
                    .HasColumnName("bounceType");

                entity.Property(e => e.BouncedRecipients).HasColumnName("bouncedRecipients");

                entity.Property(e => e.ClientDestination)
                    .HasMaxLength(500)
                    .HasColumnName("Client_Destination");

                entity.Property(e => e.EmailLink).HasColumnName("Email_Link");

                entity.Property(e => e.EmailLinkTags).HasColumnName("Email_LinkTags");

                entity.Property(e => e.EmailSubject).HasColumnName("Email_Subject");

                entity.Property(e => e.EventType).HasMaxLength(100);

                entity.Property(e => e.IpAddress).HasMaxLength(50);

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("timestamp");
            });

            modelBuilder.Entity<GenAwsTrackingEmailSubject>(entity =>
            {
                entity.ToTable("gen_AWS_Tracking_Email_Subject");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DisplayName).HasMaxLength(1000);

                entity.Property(e => e.EmailSubject).HasMaxLength(1000);
            });

            modelBuilder.Entity<GenBlog>(entity =>
            {
                entity.ToTable("gen_Blogs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Blog).HasMaxLength(2000);

                entity.Property(e => e.BlogImage).HasMaxLength(100);

                entity.Property(e => e.BlogUrl).HasColumnName("BlogURL");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");
            });

            modelBuilder.Entity<GenCcEmailId>(entity =>
            {
                entity.ToTable("gen_CC_EmailIDs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CcemailIds).HasColumnName("CCEmailIds");

                entity.Property(e => e.CcemailNames).HasColumnName("CCEmailNames");
            });

            modelBuilder.Entity<GenChannelBookmarkMessage>(entity =>
            {
                entity.ToTable("gen_Channel_BookmarkMessage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");
            });

            modelBuilder.Entity<GenChannelHistory>(entity =>
            {
                entity.ToTable("gen_ChannelHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChannelActionId).HasColumnName("ChannelActionID");

                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");

                entity.Property(e => e.IsAtsuser).HasColumnName("IsATSUser");

                entity.Property(e => e.UserEmpId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UserEmpID");
            });

            modelBuilder.Entity<GenChannelMessageReaction>(entity =>
            {
                entity.ToTable("gen_Channel_MessageReactions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<GenChannelUserMessage>(entity =>
            {
                entity.ToTable("gen_Channel_UserMessages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");

                entity.Property(e => e.MessageRepliedId).HasColumnName("MessageRepliedID");

                entity.Property(e => e.MessageSentDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenClientActionHistorySenttoAtsLogClientPortal>(entity =>
            {
                entity.ToTable("gen_ClientActionHistorySenttoATS_Log_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientHistoryId).HasColumnName("ClientHistoryID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.IsSent).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<GenClientAmAssignment>(entity =>
            {
                entity.ToTable("gen_ClientAmAssignment");

                entity.Property(e => e.AssignedAmuserId).HasColumnName("AssignedAMUserID");

                entity.Property(e => e.BauUtsTagging)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BAU_UTS_Tagging");

                entity.Property(e => e.ClientEmailId)
                    .HasMaxLength(50)
                    .HasColumnName("ClientEmailID");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EngagemenId)
                    .HasMaxLength(50)
                    .HasColumnName("EngagemenID");

                entity.Property(e => e.EngagementModelId)
                    .HasColumnName("EngagementModelID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GeoId).HasColumnName("GEO_ID");

                entity.Property(e => e.HrNumber)
                    .HasMaxLength(50)
                    .HasColumnName("HR_Number");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsImport).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SalesPersonId).HasColumnName("SalesPersonID");

                entity.Property(e => e.TalentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GenClientAmAssignmentHistory>(entity =>
            {
                entity.ToTable("gen_ClientAmAssignment_History");

                entity.Property(e => e.AssignedAmuserId).HasColumnName("AssignedAMUserID");

                entity.Property(e => e.BauUtsTagging)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BAU_UTS_Tagging");

                entity.Property(e => e.ClientAmassignmentId).HasColumnName("ClientAMAssignmentID");

                entity.Property(e => e.ClientEmailId)
                    .HasMaxLength(50)
                    .HasColumnName("ClientEmailID");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EngagemenId)
                    .HasMaxLength(50)
                    .HasColumnName("EngagemenID");

                entity.Property(e => e.EngagementModelId)
                    .HasColumnName("EngagementModelID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GeoId).HasColumnName("GEO_ID");

                entity.Property(e => e.HrNumber)
                    .HasMaxLength(50)
                    .HasColumnName("HR_Number");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SalesPersonId).HasColumnName("SalesPersonID");

                entity.Property(e => e.TalentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GenClientCalendarEvent>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK_gen_Events");

                entity.ToTable("gen_ClientCalendarEvents");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.SlotGivenById).HasColumnName("SlotGiven_ByID");

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.Property(e => e.Subject).HasMaxLength(100);

                entity.Property(e => e.ThemeColor).HasMaxLength(10);
            });

            modelBuilder.Entity<GenClientHappinessSurvey>(entity =>
            {
                entity.ToTable("gen_ClientHappinessSurvey");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("Company_ID");

                entity.Property(e => e.ContactId).HasColumnName("Contact_ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.FeedbackComments).HasColumnName("Feedback_Comments");

                entity.Property(e => e.FeedbackSubmittedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Feedback_SubmittedDate");

                entity.Property(e => e.FeedbackUrl)
                    .HasMaxLength(200)
                    .HasColumnName("Feedback_URL");

                entity.Property(e => e.IsEmailSend).HasColumnName("Is_EmailSend");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.OtherClientEmailId)
                    .HasMaxLength(100)
                    .HasColumnName("Other_ClientEmailID");

                entity.Property(e => e.OtherClientName)
                    .HasMaxLength(500)
                    .HasColumnName("Other_Client_Name");

                entity.Property(e => e.OtherCompanyName)
                    .HasMaxLength(500)
                    .HasColumnName("Other_Company_Name");

                entity.Property(e => e.TestimonialOptions).HasMaxLength(200);
            });

            modelBuilder.Entity<GenClientHappinessSurveyOption>(entity =>
            {
                entity.ToTable("gen_ClientHappinessSurvey_Options");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HappynessSurvayFeedbackId).HasColumnName("HappynessSurvay_Feedback_ID");

                entity.Property(e => e.HappynessSurvayOptionId).HasColumnName("HappynessSurvay_OptionID");
            });

            modelBuilder.Entity<GenClientHappinessSurveyQuestion>(entity =>
            {
                entity.ToTable("gen_ClientHappinessSurvey_Questions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HappinessSurveyFeedbackId).HasColumnName("HappinessSurvey_FeedbackID");

                entity.Property(e => e.HappinessSurveyQuestionId).HasColumnName("HappinessSurvey_QuestionID");
            });

            modelBuilder.Entity<GenClientJob>(entity =>
            {
                entity.ToTable("Gen_Client_Jobs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CurrentStepId).HasColumnName("CurrentStepID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.LastModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.NextStepId).HasColumnName("NextStepID");
            });

            modelBuilder.Entity<GenClientPayOutAdditionalLineItem>(entity =>
            {
                entity.ToTable("gen_Client_PayOut_Additional_LineItems");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(500)
                    .HasColumnName("Item_Name");

                entity.Property(e => e.PayOutId).HasColumnName("PayOut_ID");
            });

            modelBuilder.Entity<GenClientenquiryDatum>(entity =>
            {
                entity.ToTable("gen_Clientenquiry_Data");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientEmail).HasMaxLength(100);

                entity.Property(e => e.ClientIpaddress)
                    .HasMaxLength(100)
                    .HasColumnName("ClientIPAddress");

                entity.Property(e => e.ClientName).HasMaxLength(100);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CompanyName).HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(100);
            });

            modelBuilder.Entity<GenCompany>(entity =>
            {
                entity.ToTable("gen_Company");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AboutCompanyDesc).HasColumnName("about_Company_desc");

                entity.Property(e => e.Address).HasMaxLength(2000);

                entity.Property(e => e.AmSalesPersonId).HasColumnName("AM_SalesPersonID");

                entity.Property(e => e.AnotherCompanyTypeId).HasColumnName("AnotherCompanyTypeID");

                entity.Property(e => e.Category)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");

                entity.Property(e => e.ClientStatusId).HasColumnName("Client_StatusID");

                entity.Property(e => e.Company).HasMaxLength(500);

                entity.Property(e => e.CompanyLogo).HasMaxLength(200);

                entity.Property(e => e.CompanySizeRangeorAdhoc)
                    .HasMaxLength(500)
                    .HasColumnName("CompanySize_RangeorAdhoc");

                entity.Property(e => e.CompanyTypeId).HasColumnName("CompanyTypeID");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreditAmount)
                    .HasColumnType("decimal(18, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreditCurrency).HasMaxLength(20);

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.DiscoveryCall).HasColumnName("Discovery_Call");

                entity.Property(e => e.GeoId).HasColumnName("GEO_ID");

                entity.Property(e => e.GspaceId).HasColumnName("GSpaceID");

                entity.Property(e => e.Gstnumber)
                    .HasMaxLength(250)
                    .HasColumnName("GSTNumber");

                entity.Property(e => e.HubSpotCompany).HasColumnName("HubSpot_Company");

                entity.Property(e => e.Industry)
                    .HasMaxLength(100)
                    .HasColumnName("industry");

                entity.Property(e => e.IndustryType)
                    .HasMaxLength(2000)
                    .HasColumnName("Industry_Type");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsFromSignUp).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsGspaceCreated).HasColumnName("IsGSpaceCreated");

                entity.Property(e => e.JpcreditBalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("JPCreditBalance");

                entity.Property(e => e.LeadType)
                    .HasMaxLength(100)
                    .HasColumnName("Lead_Type");

                entity.Property(e => e.LinkedInProfile).HasMaxLength(300);

                entity.Property(e => e.Location).HasMaxLength(200);

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.NbdSalesPersonId).HasColumnName("NBD_SalesPersonID");

                entity.Property(e => e.Pannumber)
                    .HasMaxLength(250)
                    .HasColumnName("PANNumber");

                entity.Property(e => e.ParentCompanyId).HasColumnName("ParentCompanyID");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.Score).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .HasColumnName("state");

                entity.Property(e => e.Website).HasMaxLength(200);

                entity.Property(e => e.Zip)
                    .HasMaxLength(50)
                    .HasColumnName("zip");
            });

            modelBuilder.Entity<GenCompanyAmMappingHistory>(entity =>
            {
                entity.ToTable("gen_Company_AM_Mapping_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.NewAmSalesPersonId).HasColumnName("NewAM_SalesPersonID");

                entity.Property(e => e.OldAmSalesPersonId).HasColumnName("OldAM_SalesPersonID");
            });

            modelBuilder.Entity<GenCompanyContractDetail>(entity =>
            {
                entity.ToTable("gen_CompanyContractDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LegalCompanyAddress).HasMaxLength(2000);

                entity.Property(e => e.LegalCompanyName).HasMaxLength(500);

                entity.Property(e => e.SigningAuthorityDesignation).HasMaxLength(100);

                entity.Property(e => e.SigningAuthorityEmail).HasMaxLength(100);

                entity.Property(e => e.SigningAuthorityFirstName).HasMaxLength(100);

                entity.Property(e => e.SigningAuthorityLastName).HasMaxLength(100);

                entity.Property(e => e.SigningAuthorityName).HasMaxLength(200);
            });

            modelBuilder.Entity<GenCompanyCultureandPerksDetail>(entity =>
            {
                entity.ToTable("gen_Company_CultureandPerksDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CultureImage).HasMaxLength(500);

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenCompanyDetail>(entity =>
            {
                entity.ToTable("gen_Company_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyType).HasMaxLength(200);

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Headquaters).HasMaxLength(100);

                entity.Property(e => e.IsSelfFunded).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.TotalFundings).HasMaxLength(100);
            });

            modelBuilder.Entity<GenCompanyFundingDetail>(entity =>
            {
                entity.ToTable("gen_Company_FundingDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasMaxLength(200);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.FundingRound).HasMaxLength(50);

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Month)
                    .HasMaxLength(20)
                    .HasColumnName("MONTH");

                entity.Property(e => e.Series).HasMaxLength(200);

                entity.Property(e => e.Year)
                    .HasMaxLength(5)
                    .HasColumnName("YEAR");
            });

            modelBuilder.Entity<GenCompanyHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("gen_Company_History");

                entity.Property(e => e.AboutCompanyDesc).HasColumnName("about_Company_desc");

                entity.Property(e => e.Action).HasMaxLength(100);

                entity.Property(e => e.Address).HasMaxLength(2000);

                entity.Property(e => e.AmSalesPersonId).HasColumnName("AM_SalesPersonID");

                entity.Property(e => e.AnotherCompanyTypeId).HasColumnName("AnotherCompanyTypeID");

                entity.Property(e => e.Category)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");

                entity.Property(e => e.ClientStatusId).HasColumnName("Client_StatusID");

                entity.Property(e => e.Company).HasMaxLength(500);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CompanyLogo).HasMaxLength(200);

                entity.Property(e => e.CompanyTypeId).HasColumnName("CompanyTypeID");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.CreatedByDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreditAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreditCurrency).HasMaxLength(50);

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.DiscoveryCall).HasColumnName("Discovery_Call");

                entity.Property(e => e.GeoId).HasColumnName("GEO_ID");

                entity.Property(e => e.GspaceId).HasColumnName("GSpaceID");

                entity.Property(e => e.HubSpotCompany).HasColumnName("HubSpot_Company");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Industry)
                    .HasMaxLength(100)
                    .HasColumnName("industry");

                entity.Property(e => e.IndustryType)
                    .HasMaxLength(2000)
                    .HasColumnName("Industry_Type");

                entity.Property(e => e.IsGspaceCreated).HasColumnName("IsGSpaceCreated");

                entity.Property(e => e.JpcreditBalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("JPCreditBalance");

                entity.Property(e => e.LeadType)
                    .HasMaxLength(100)
                    .HasColumnName("Lead_Type");

                entity.Property(e => e.LinkedInProfile).HasMaxLength(300);

                entity.Property(e => e.Location).HasMaxLength(200);

                entity.Property(e => e.ModifiedByDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.NbdSalesPersonId).HasColumnName("NBD_SalesPersonID");

                entity.Property(e => e.ParentCompanyId).HasColumnName("ParentCompanyID");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.Score).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .HasColumnName("state");

                entity.Property(e => e.Website).HasMaxLength(200);

                entity.Property(e => e.Zip)
                    .HasMaxLength(50)
                    .HasColumnName("zip");
            });

            modelBuilder.Entity<GenCompanyLeadTypeUserDetail>(entity =>
            {
                entity.ToTable("gen_CompanyLeadType_UserDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.LeadTypeUserId).HasColumnName("LeadType_UserID");
            });

            modelBuilder.Entity<GenCompanyLegalInfo>(entity =>
            {
                entity.ToTable("gen_CompanyLegalInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AgreementStatus).HasMaxLength(50);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DocumentFile).HasMaxLength(200);

                entity.Property(e => e.DocumentName).HasMaxLength(500);

                entity.Property(e => e.DocumentType).HasMaxLength(200);

                entity.Property(e => e.DocumentUrl).HasColumnName("DocumentURL");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LegalDescription).HasMaxLength(2000);

                entity.Property(e => e.RenewalDate).HasColumnType("datetime");

                entity.Property(e => e.SignedDate).HasColumnType("datetime");

                entity.Property(e => e.ValidityEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Validity_EndDate");

                entity.Property(e => e.ValidityStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Validity_StartDate");
            });

            modelBuilder.Entity<GenCompanyPerkDetail>(entity =>
            {
                entity.ToTable("gen_Company_PerkDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Perks).HasMaxLength(200);
            });

            modelBuilder.Entity<GenCompanyUpdate>(entity =>
            {
                entity.ToTable("gen_CompanyUpdates");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyUpdates).HasMaxLength(2000);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");
            });

            modelBuilder.Entity<GenCompanyWeightedAverageCriterion>(entity =>
            {
                entity.ToTable("gen_CompanyWeightedAverageCriteria");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CurrentOpenPositionId).HasColumnName("CurrentOpenPosition_ID");

                entity.Property(e => e.ExpansionPlanId).HasColumnName("ExpansionPlanID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LeadPlanId).HasColumnName("LeadPlanID");

                entity.Property(e => e.TargetGroupId).HasColumnName("TargetGroup_ID");

                entity.Property(e => e.TeamSizeId).HasColumnName("TeamSize_ID");
            });

            modelBuilder.Entity<GenCompanyWhatsappDetail>(entity =>
            {
                entity.ToTable("gen_Company_WhatsappDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");
            });

            modelBuilder.Entity<GenCompanyWhatsappMemberDetail>(entity =>
            {
                entity.ToTable("gen_Company_WhatsappMember_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.WhatsappDetailId).HasColumnName("WhatsappDetailID");
            });

            modelBuilder.Entity<GenCompanyYouTubeLink>(entity =>
            {
                entity.ToTable("gen_Company_YouTubeLinks");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenContact>(entity =>
            {
                entity.ToTable("gen_Contact");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(2000);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.ContactNo).HasMaxLength(50);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.Designation).HasMaxLength(50);

                entity.Property(e => e.EmailId)
                    .HasMaxLength(200)
                    .HasColumnName("EmailID");

                entity.Property(e => e.EncryptedPassword).HasMaxLength(500);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(300);

                entity.Property(e => e.HubSpotContactId).HasColumnName("HubSpot_ContactID");

                entity.Property(e => e.IsEmailSentforFirstHr)
                    .HasColumnName("IsEmailSentforFirstHR")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsFromSignUp).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsJobPostUser).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsResetPassword).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastLoginTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.LinkedIn).HasMaxLength(300);

                entity.Property(e => e.Otp)
                    .HasMaxLength(10)
                    .HasColumnName("OTP");

                entity.Property(e => e.OtpCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("OTP_CreatedDate");

                entity.Property(e => e.OtpExpiredDate)
                    .HasColumnType("datetime")
                    .HasColumnName("OTP_ExpiredDate");

                entity.Property(e => e.OtpIsActive).HasColumnName("OTP_IsActive");

                entity.Property(e => e.OtpattemptCount).HasColumnName("OTPAttemptCount");

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.PasswordAttemptCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.Regions)
                    .HasMaxLength(50)
                    .HasColumnName("regions");

                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .HasColumnName("state");

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.Property(e => e.YearsofExperience).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Zip)
                    .HasMaxLength(50)
                    .HasColumnName("zip");
            });

            modelBuilder.Entity<GenContactGetCallBackHistory>(entity =>
            {
                entity.ToTable("gen_Contact_GetCallBackHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CallBackCreatedDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("CallBack_CreatedDatetime");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.IscallBackShow).HasColumnName("ISCallBackShow");
            });

            modelBuilder.Entity<GenContactGetHelpQuestion>(entity =>
            {
                entity.ToTable("gen_Contact_GetHelpQuestions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("Contact_ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.GetHelpQuestion).HasColumnName("GetHelp_Question");
            });

            modelBuilder.Entity<GenContactHistory>(entity =>
            {
                entity.ToTable("gen_ContactHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppActionDoneBy).HasDefaultValueSql("((4))");

                entity.Property(e => e.AtstalentId).HasColumnName("ATSTalent_ID");

                entity.Property(e => e.ClientActionId).HasColumnName("ClientActionID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Device)
                    .HasMaxLength(200)
                    .HasColumnName("device");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.Ipaddress)
                    .HasMaxLength(100)
                    .HasColumnName("IPAddress");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.Location).HasMaxLength(200);

                entity.Property(e => e.LoggedInTime).HasColumnType("datetime");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TalentStatusId).HasColumnName("Talent_StatusID");
            });

            modelBuilder.Entity<GenContactInterviewFeedback>(entity =>
            {
                entity.ToTable("gen_ContactInterviewFeedback");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnsCompanyCulture)
                    .HasMaxLength(4000)
                    .HasColumnName("Ans_CompanyCulture");

                entity.Property(e => e.AnsInterpretationCommunicationSkills)
                    .HasMaxLength(4000)
                    .HasColumnName("Ans_InterpretationCommunicationSkills");

                entity.Property(e => e.AnsProfessionalism)
                    .HasMaxLength(4000)
                    .HasColumnName("Ans_Professionalism");

                entity.Property(e => e.AnsTalentCurrentRole)
                    .HasMaxLength(4000)
                    .HasColumnName("Ans_TalentCurrentRole");

                entity.Property(e => e.AnsTalentfittoCulture)
                    .HasMaxLength(4000)
                    .HasColumnName("Ans_TalentfittoCulture");

                entity.Property(e => e.CognitiveSkillRating)
                    .HasMaxLength(150)
                    .HasColumnName("Cognitive_SkillRating");

                entity.Property(e => e.CommunicationSkillRating)
                    .HasMaxLength(150)
                    .HasColumnName("Communication_SkillRating");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.FeedBackType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FeedBack_Type");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.ImprovedSkill)
                    .HasMaxLength(150)
                    .HasColumnName("Improved_Skill");

                entity.Property(e => e.IsNextroundDetailsSubmitted).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.NohireReconsiderHiringTalent).HasColumnName("Nohire_ReconsiderHiringTalent");

                entity.Property(e => e.NohireTalentDontFitCurrentRole)
                    .HasMaxLength(4000)
                    .HasColumnName("Nohire_TalentDontFitCurrentRole");

                entity.Property(e => e.NohireTalentImprove)
                    .HasMaxLength(4000)
                    .HasColumnName("Nohire_TalentImprove");

                entity.Property(e => e.OnHoldDescription)
                    .HasMaxLength(4000)
                    .HasColumnName("OnHold_Description");

                entity.Property(e => e.RateCompanyCulture)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Rate_CompanyCulture");

                entity.Property(e => e.RateInterpretationCommunicationSkills)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Rate_InterpretationCommunicationSkills");

                entity.Property(e => e.RateProfessionalism)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Rate_Professionalism");

                entity.Property(e => e.RateTalentCurrentRole)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Rate_TalentCurrentRole");

                entity.Property(e => e.RateTalentfittoCulture)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Rate_TalentfittoCulture");

                entity.Property(e => e.RescheduledDescription)
                    .HasMaxLength(4000)
                    .HasColumnName("Rescheduled_Description");

                entity.Property(e => e.ShortlistedInterviewId).HasColumnName("Shortlisted_InterviewID");

                entity.Property(e => e.TechnicalSkillRating)
                    .HasMaxLength(150)
                    .HasColumnName("Technical_SkillRating");

                entity.Property(e => e.TopSkill)
                    .HasMaxLength(150)
                    .HasColumnName("Top_Skill");
            });

            modelBuilder.Entity<GenContactNotificationHistory>(entity =>
            {
                entity.ToTable("gen_Contact_NotificationHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");
            });

            modelBuilder.Entity<GenContactPendingAction>(entity =>
            {
                entity.ToTable("gen_ContactPendingAction");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.PendingAction).HasMaxLength(500);
            });

            modelBuilder.Entity<GenContactPointofContact>(entity =>
            {
                entity.ToTable("gen_ContactPointofContact");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("Company_ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<GenContactTalentPriority>(entity =>
            {
                entity.ToTable("gen_ContactTalentPriority");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AllowedtoSelectforHr).HasColumnName("AllowedtoSelectforHR");

                entity.Property(e => e.AtsNonNdaurl).HasColumnName("ATS_Non_NDAURL");

                entity.Property(e => e.AtsTalentLiveUrl).HasColumnName("ATS_Talent_LiveURL");

                entity.Property(e => e.AtsTalentResume).HasColumnName("ATS_Talent_Resume");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CostAfterAcceptance)
                    .HasMaxLength(50)
                    .HasColumnName("Cost_AfterAcceptance");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CurrencyFinalHrCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Currency_Final_HR_Cost");

                entity.Property(e => e.CurrencyHrBaseCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Currency_HR_Base_Cost");

                entity.Property(e => e.CurrentCtc)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Current_CTC");

                entity.Property(e => e.DpPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DP_Percentage");

                entity.Property(e => e.Dpamount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DPAmount");

                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FinalHrCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Final_HR_Cost");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");

                entity.Property(e => e.HrBaseCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("HR_Base_Cost");

                entity.Property(e => e.HrCost)
                    .HasMaxLength(50)
                    .HasColumnName("HR_Cost");

                entity.Property(e => e.HrtalentGuid)
                    .HasMaxLength(50)
                    .HasColumnName("HRTalentGUID");

                entity.Property(e => e.HrtalentTagId).HasColumnName("HRTalentTagID");

                entity.Property(e => e.IsHrtypeDp).HasColumnName("IsHRTypeDP");

                entity.Property(e => e.LossRemark).IsUnicode(false);

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.NrorDpchangeDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("NRorDPChangeDateTime");

                entity.Property(e => e.Nrpercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("NRPercentage");

                entity.Property(e => e.OnHoldRemark).IsUnicode(false);

                entity.Property(e => e.OtherRejectReason).HasMaxLength(2000);

                entity.Property(e => e.PerticularHrTimeZoneIdonAcceptance).HasColumnName("Perticular_HR_TimeZoneIDOnAcceptance");

                entity.Property(e => e.PreviousTalentStatusIdBasedOnHr).HasColumnName("Previous_TalentStatusID_BasedOnHR");

                entity.Property(e => e.RejectReasonId).HasColumnName("RejectReasonID");

                entity.Property(e => e.RejectionStage).HasMaxLength(250);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.SalesPersonId).HasColumnName("SalesPerson_ID");

                entity.Property(e => e.TalentCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TalentCurrencyCode)
                    .HasMaxLength(10)
                    .HasColumnName("Talent_CurrencyCode");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");

                entity.Property(e => e.TalentKeepsOwnPreferences).HasDefaultValueSql("((0))");

                entity.Property(e => e.TalentPriority).HasMaxLength(50);

                entity.Property(e => e.TalentStatusIdBasedOnHr).HasColumnName("TalentStatusID_BasedOnHR");

                entity.Property(e => e.TalentStatusIdClientPortal).HasColumnName("TalentStatusID_ClientPortal");

                entity.Property(e => e.UplersfeesAmount).HasMaxLength(300);

                entity.Property(e => e.ViewedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenContactUsersClientPortal>(entity =>
            {
                entity.ToTable("gen_ContactUsers_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.ContactNumber).HasMaxLength(20);

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Department).HasMaxLength(100);

                entity.Property(e => e.Designation).HasMaxLength(100);

                entity.Property(e => e.EmailId)
                    .HasMaxLength(100)
                    .HasColumnName("EmailID");

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");
            });

            modelBuilder.Entity<GenContractRenewalDetail>(entity =>
            {
                entity.ToTable("gen_Contract_Renewal_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractDuration).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.IsContractRenewed).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.NewBr)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("NewBR");

                entity.Property(e => e.NewNr)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("NewNR");

                entity.Property(e => e.NewPr)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("NewPR");

                entity.Property(e => e.RenewalEndDate).HasColumnType("datetime");

                entity.Property(e => e.RenewalStartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenCrctpShortlistedTalentsClientPortal>(entity =>
            {
                entity.ToTable("gen_CRCTP_ShortlistedTalents_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AtsTalentId).HasColumnName("ATS_TalentID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.MyListDateTime).HasColumnType("datetime");

                entity.Property(e => e.ProfileCredit).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StatusChangeDateTime).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UnLockDateTime).HasColumnType("datetime");

                entity.Property(e => e.UtstalentId).HasColumnName("UTSTalentID");
            });

            modelBuilder.Entity<GenCustomAssessmentEmailDetail>(entity =>
            {
                entity.ToTable("gen_CustomAssessmentEmailDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssessmentId).HasColumnName("AssessmentID");
            });

            modelBuilder.Entity<GenDeal>(entity =>
            {
                entity.ToTable("gen_Deals");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActualCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Actual_Cost");

                entity.Property(e => e.AgreementSignedMonth)
                    .HasMaxLength(100)
                    .HasColumnName("agreement_signed_month");

                entity.Property(e => e.AgreementSignedWeek)
                    .HasMaxLength(100)
                    .HasColumnName("agreement_signed_week");

                entity.Property(e => e.AmId).HasColumnName("AM_ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 8)");

                entity.Property(e => e.AmountInCompanyCurrency).HasColumnType("decimal(18, 8)");

                entity.Property(e => e.AmountInHomeCurrency)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("amount_in_home_currency");

                entity.Property(e => e.AssociatedCompanyIds)
                    .HasMaxLength(500)
                    .HasColumnName("associatedCompanyIds");

                entity.Property(e => e.AssociatedDealIds)
                    .HasMaxLength(500)
                    .HasColumnName("associatedDealIds");

                entity.Property(e => e.AssociatedTicketIds)
                    .HasMaxLength(1000)
                    .HasColumnName("associatedTicketIds");

                entity.Property(e => e.AssociatedVids)
                    .HasMaxLength(500)
                    .HasColumnName("associatedVids");

                entity.Property(e => e.BdrMdrEmail)
                    .HasMaxLength(100)
                    .HasColumnName("BDR_MDR_Email");

                entity.Property(e => e.BdrMdrName)
                    .HasMaxLength(50)
                    .HasColumnName("BDR_MDR_Name");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.ClientName).HasMaxLength(200);

                entity.Property(e => e.CloseDate).HasColumnType("datetime");

                entity.Property(e => e.ClosedLostReason).HasMaxLength(500);

                entity.Property(e => e.ClosedLostReason1)
                    .HasMaxLength(100)
                    .HasColumnName("closed_lost_reason");

                entity.Property(e => e.ClosedWonReason).HasMaxLength(500);

                entity.Property(e => e.CompanyDealId).HasColumnName("Company_Deal_ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.CustomerType).HasMaxLength(50);

                entity.Property(e => e.DealDate).HasColumnType("datetime");

                entity.Property(e => e.DealId).HasColumnName("DealID");

                entity.Property(e => e.DealName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DealNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Deal_Number");

                entity.Property(e => e.DealOwner)
                    .HasMaxLength(100)
                    .HasColumnName("Deal_Owner");

                entity.Property(e => e.DealProjectId)
                    .HasMaxLength(50)
                    .HasColumnName("Deal_Project_ID");

                entity.Property(e => e.DealService).HasMaxLength(500);

                entity.Property(e => e.DealServiceId).HasColumnName("DealServiceID");

                entity.Property(e => e.DealSource)
                    .HasMaxLength(100)
                    .HasColumnName("deal_source");

                entity.Property(e => e.DealStage).HasMaxLength(100);

                entity.Property(e => e.DealStageId).HasColumnName("DealStageID");

                entity.Property(e => e.DealType).HasMaxLength(100);

                entity.Property(e => e.DealTypeId).HasColumnName("DealTypeID");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.DiscoveryCallDoneMonth)
                    .HasMaxLength(100)
                    .HasColumnName("discovery_call_done_month");

                entity.Property(e => e.DiscoveryCallDoneWeek)
                    .HasMaxLength(100)
                    .HasColumnName("discovery_call_done_week");

                entity.Property(e => e.DiscoveryCallScheduledMonth)
                    .HasMaxLength(100)
                    .HasColumnName("discovery_call_scheduled_month");

                entity.Property(e => e.DiscoveryCallScheduledWeek)
                    .HasMaxLength(100)
                    .HasColumnName("discovery_call_scheduled_week");

                entity.Property(e => e.Elite).HasMaxLength(50);

                entity.Property(e => e.EmailId)
                    .HasMaxLength(50)
                    .HasColumnName("EmailID");

                entity.Property(e => e.EngageModelId).HasColumnName("EngageModelID");

                entity.Property(e => e.FutureProspectMonth)
                    .HasMaxLength(100)
                    .HasColumnName("future_prospect_month");

                entity.Property(e => e.GeoId).HasColumnName("GEO_ID");

                entity.Property(e => e.HaveYouUpdateTheUtsSystemWithInterviewScheduled)
                    .HasMaxLength(10)
                    .HasColumnName("have_you_update_the_uts_system_with_interview_scheduled");

                entity.Property(e => e.HaveYouUpdatedTheUtsSystemWithInterviewFeedback)
                    .HasMaxLength(10)
                    .HasColumnName("have_you_updated_the_uts_system_with_interview_feedback");

                entity.Property(e => e.HaveYouUpdatedTheUtsSystemWithProfileFeedback)
                    .HasMaxLength(10)
                    .HasColumnName("have_you_updated_the_uts_system_with_profile_feedback");

                entity.Property(e => e.HiringRequestRaisedMonth)
                    .HasMaxLength(10)
                    .HasColumnName("hiring_request_raised_month");

                entity.Property(e => e.HiringRequestRaisedWeek)
                    .HasMaxLength(10)
                    .HasColumnName("hiring_request_raised_week");

                entity.Property(e => e.HoldReasonId).HasColumnName("HoldReason_ID");

                entity.Property(e => e.HrId).HasColumnName("HR_ID");

                entity.Property(e => e.HsClosedAmountInHomeCurrency)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("hs_closed_amount_in_home_currency");

                entity.Property(e => e.HubspotOwnerId).HasColumnName("hubspot_owner_id");

                entity.Property(e => e.InterviewDoneMonth)
                    .HasMaxLength(100)
                    .HasColumnName("interview_done_month");

                entity.Property(e => e.InterviewDoneWeek)
                    .HasMaxLength(100)
                    .HasColumnName("interview_done_week");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.LostReasonId).HasColumnName("LostReason_ID");

                entity.Property(e => e.MeetingScheduledDate)
                    .HasColumnType("datetime")
                    .HasColumnName("meeting_scheduled_date");

                entity.Property(e => e.MeetingScheduledTime)
                    .HasMaxLength(100)
                    .HasColumnName("meeting_scheduled_time");

                entity.Property(e => e.MovedToSalesFutureProspectMonth)
                    .HasMaxLength(100)
                    .HasColumnName("moved_to_sales_future_prospect_month");

                entity.Property(e => e.NoofDedicated).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Payrate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("payrate");

                entity.Property(e => e.Pipeline)
                    .HasMaxLength(100)
                    .HasColumnName("pipeline");

                entity.Property(e => e.PipelineId).HasColumnName("Pipeline_ID");

                entity.Property(e => e.PoolVsOdrSplit)
                    .HasMaxLength(100)
                    .HasColumnName("pool_vs_odr_split");

                entity.Property(e => e.PortalId).HasColumnName("PortalID");

                entity.Property(e => e.ProfileSharedMonth)
                    .HasMaxLength(100)
                    .HasColumnName("profile_shared_month");

                entity.Property(e => e.ProfileSharedWeek)
                    .HasMaxLength(100)
                    .HasColumnName("profile_shared_week");

                entity.Property(e => e.ProjectId)
                    .HasMaxLength(200)
                    .HasColumnName("Project_Id");

                entity.Property(e => e.ProspectAchievedMonth)
                    .HasMaxLength(100)
                    .HasColumnName("prospect_achieved_month");

                entity.Property(e => e.ProspectAchievedWeek)
                    .HasMaxLength(100)
                    .HasColumnName("prospect_achieved_week");

                entity.Property(e => e.RfqId).HasColumnName("RFQ_ID");

                entity.Property(e => e.SalAchievedMonth)
                    .HasMaxLength(20)
                    .HasColumnName("SAL_Achieved_Month");

                entity.Property(e => e.SalAchievedWeek)
                    .HasMaxLength(100)
                    .HasColumnName("sal_achieved_week");

                entity.Property(e => e.Source).HasMaxLength(100);

                entity.Property(e => e.SourceId)
                    .HasMaxLength(100)
                    .HasColumnName("SourceID");

                entity.Property(e => e.SubServiceIds)
                    .HasMaxLength(500)
                    .HasColumnName("SubService_IDs");

                entity.Property(e => e.SupplyChannel)
                    .HasMaxLength(100)
                    .HasColumnName("supply_channel");

                entity.Property(e => e.TalentName)
                    .HasMaxLength(100)
                    .HasColumnName("talent_name");

                entity.Property(e => e.TempEngageModelId).HasColumnName("Temp_EngageModelID");
            });

            modelBuilder.Entity<GenDealContact>(entity =>
            {
                entity.ToTable("gen_DealContacts");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.DealId).HasColumnName("DealID");
            });

            modelBuilder.Entity<GenDealTicket>(entity =>
            {
                entity.ToTable("gen_DealTickets");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DealId).HasColumnName("DealID");

                entity.Property(e => e.TicketId).HasColumnName("TicketID");
            });

            modelBuilder.Entity<GenDirectPlacement>(entity =>
            {
                entity.ToTable("gen_Direct_Placement");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.AtsJobLocationId).HasColumnName("ATS_JobLocationID");

                entity.Property(e => e.AtsNearByCities).HasColumnName("ATS_NearByCities");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.FrequencyOfficeVisitId).HasColumnName("FrequencyOfficeVisitID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_Id");

                entity.Property(e => e.IsOpenToWorkNearByCities).HasDefaultValueSql("((0))");

                entity.Property(e => e.JobLocation).HasMaxLength(500);

                entity.Property(e => e.ModeOfWork).HasMaxLength(50);

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.PostalCode).HasMaxLength(100);

                entity.Property(e => e.State).HasMaxLength(100);
            });

            modelBuilder.Entity<GenEmailIdsWithNonEnterpriseList>(entity =>
            {
                entity.ToTable("gen_EMailIds_With_NonEnterprise_Lists");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientName).HasMaxLength(200);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CompanyName).HasMaxLength(200);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(50)
                    .HasColumnName("EmailID");
            });

            modelBuilder.Entity<GenEmailList>(entity =>
            {
                entity.ToTable("gen_Email_Lists");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(50);
            });

            modelBuilder.Entity<GenEsalesAmApiResponse>(entity =>
            {
                entity.ToTable("gen_Esales_AM_API_Response");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.EsaleSalesPersonId)
                    .HasMaxLength(50)
                    .HasColumnName("EsaleSalesPersonID");

                entity.Property(e => e.EsalesParentId)
                    .HasMaxLength(50)
                    .HasColumnName("EsalesParentID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");

                entity.Property(e => e.NbdsalesPersonName)
                    .HasMaxLength(200)
                    .HasColumnName("NBDSalesPersonName");

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");

                entity.Property(e => e.RequestPayload).HasDefaultValueSql("('')");

                entity.Property(e => e.SalesPersonName).HasMaxLength(200);
            });

            modelBuilder.Entity<GenFrontendClient>(entity =>
            {
                entity.ToTable("gen_FrontendClient");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDatetime).HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EmailID");

                entity.Property(e => e.Otp)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("OTP");
            });

            modelBuilder.Entity<GenGptJdresponse>(entity =>
            {
                entity.ToTable("Gen_GPT_JDResponse");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Gptprompt).HasColumnName("GPTPrompt");

                entity.Property(e => e.Jdtext).HasColumnName("JDText");

                entity.Property(e => e.Url).HasColumnName("URL");
            });

            modelBuilder.Entity<GenHistory>(entity =>
            {
                entity.ToTable("gen_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionId).HasColumnName("Action_ID");

                entity.Property(e => e.ContactTalentPriorityId).HasColumnName("ContactTalentPriority_ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.HistoryFrom).HasColumnName("History_From");

                entity.Property(e => e.HrAcceptedDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("HR_AcceptedDateTime");

                entity.Property(e => e.HrCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("HR_Cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HrPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("HR_Percentage");

                entity.Property(e => e.InterviewMasterId).HasColumnName("InterviewMaster_ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.NextActionId).HasColumnName("NextAction_Id");

                entity.Property(e => e.OldSalesUserId).HasColumnName("OldSalesUserID");

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");

                entity.Property(e => e.OpsUserId).HasColumnName("OpsUserID");

                entity.Property(e => e.SalesUserId).HasColumnName("SalesUserID");

                entity.Property(e => e.SlaDueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("SLA_DueDate");

                entity.Property(e => e.TalentCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Talent_Cost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TalentStatusIdBasedOnHr).HasColumnName("TalentStatusID_BasedOnHR");
            });

            modelBuilder.Entity<GenHrAtstalentDetailsCtp>(entity =>
            {
                entity.ToTable("gen_HR_ATSTalentDetails_CTP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AgreedShift).HasMaxLength(200);

                entity.Property(e => e.AtsTalentId).HasColumnName("ATS_Talent_ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CtpId).HasColumnName("CTP_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.HrType)
                    .HasMaxLength(50)
                    .HasColumnName("HR_Type");

                entity.Property(e => e.HrpricingType)
                    .HasMaxLength(100)
                    .HasColumnName("HRPricingType");

                entity.Property(e => e.HrtypeText)
                    .HasMaxLength(100)
                    .HasColumnName("HRTypeText");

                entity.Property(e => e.NoticePeriod).HasMaxLength(200);

                entity.Property(e => e.PreferredAvailability).HasMaxLength(200);

                entity.Property(e => e.TalentCurrentFeeYearly)
                    .HasMaxLength(100)
                    .HasColumnName("Talent_current_fee_yearly");

                entity.Property(e => e.TalentDesignation)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_Designation");

                entity.Property(e => e.TalentEmailId)
                    .HasMaxLength(50)
                    .HasColumnName("Talent_Email_ID");

                entity.Property(e => e.TalentExpectedFeeYearly)
                    .HasMaxLength(100)
                    .HasColumnName("Talent_Expected_fee_yearly");

                entity.Property(e => e.TalentName)
                    .HasMaxLength(500)
                    .HasColumnName("Talent_Name");

                entity.Property(e => e.YearsOfExperience).HasMaxLength(200);
            });

            modelBuilder.Entity<GenHrcustomAssessment>(entity =>
            {
                entity.ToTable("gen_HRCustomAssessment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");

                entity.Property(e => e.LastModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.SubmissionDeadline).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenHrnote>(entity =>
            {
                entity.ToTable("gen_HRNotes");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenHrnotesTagUserDetail>(entity =>
            {
                entity.ToTable("gen_HRNotes_TagUserDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssignedUserId).HasColumnName("Assigned_UserID");

                entity.Property(e => e.NoteId).HasColumnName("Note_ID");
            });

            modelBuilder.Entity<GenHubSpotCompany>(entity =>
            {
                entity.ToTable("gen_HubSpotCompany");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .HasColumnName("address");

                entity.Property(e => e.Annualrevenue)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("annualrevenue");

                entity.Property(e => e.AssignedAmuserId).HasColumnName("AssignedAMUserID");

                entity.Property(e => e.Category)
                    .HasMaxLength(5)
                    .HasColumnName("category");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.Company).HasMaxLength(200);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.Domain)
                    .HasMaxLength(500)
                    .HasColumnName("domain");

                entity.Property(e => e.EngagementModelId).HasColumnName("EngagementModelID");

                entity.Property(e => e.GeoId).HasColumnName("GEO_ID");

                entity.Property(e => e.HsObjectId).HasColumnName("hs_object_id");

                entity.Property(e => e.HubspotOwnerId).HasColumnName("hubspot_owner_id");

                entity.Property(e => e.Industry)
                    .HasMaxLength(50)
                    .HasColumnName("industry");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsContactInserted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsCreatedFromUts)
                    .HasColumnName("IsCreatedFromUTS")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsWallet).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastInvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.LastInvoiceId).HasColumnName("LastInvoice_id");

                entity.Property(e => e.LeadSourceChannel)
                    .HasMaxLength(100)
                    .HasColumnName("lead_source_channel");

                entity.Property(e => e.Lifecyclestage)
                    .HasMaxLength(100)
                    .HasColumnName("lifecyclestage");

                entity.Property(e => e.LinkedinCompanyPage)
                    .HasMaxLength(500)
                    .HasColumnName("linkedin_company_page");

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.Numberofemployees).HasColumnName("numberofemployees");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.PortalId).HasColumnName("PortalID");

                entity.Property(e => e.RegionName)
                    .HasMaxLength(100)
                    .HasColumnName("region_name");

                entity.Property(e => e.Source).HasMaxLength(100);

                entity.Property(e => e.SourceId).HasMaxLength(100);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .HasColumnName("state");

                entity.Property(e => e.WalletBalance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Website).HasMaxLength(200);

                entity.Property(e => e.Zip)
                    .HasMaxLength(50)
                    .HasColumnName("zip");

                entity.Property(e => e.ZohoPrimaryCustomerId)
                    .HasMaxLength(255)
                    .HasColumnName("ZOHO_Primary_Customer_ID");
            });

            modelBuilder.Entity<GenHubSpotContact>(entity =>
            {
                entity.ToTable("gen_HubSpotContact");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Associatedcompanyid).HasColumnName("associatedcompanyid");

                entity.Property(e => e.CanonicalVid).HasColumnName("canonical-vid");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.Company).HasMaxLength(200);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.ContactNo).HasMaxLength(100);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.Designation)
                    .HasMaxLength(100)
                    .HasColumnName("designation");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(100)
                    .HasColumnName("EmailID");

                entity.Property(e => e.EngagementModelId).HasColumnName("EngagementModelID");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GeoId).HasColumnName("GEO_ID");

                entity.Property(e => e.HsPersona)
                    .HasMaxLength(100)
                    .HasColumnName("hs_persona");

                entity.Property(e => e.HubspotOwnerId).HasColumnName("hubspot_owner_id");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsCompanyInserted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsContact).HasColumnName("is-contact");

                entity.Property(e => e.IsCreatedFromUts)
                    .HasColumnName("IsCreatedFromUTS")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Jobtitle)
                    .HasMaxLength(200)
                    .HasColumnName("jobtitle");

                entity.Property(e => e.LastInvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.LastInvoiceId).HasColumnName("LastInvoice_id");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LeadSource)
                    .HasMaxLength(100)
                    .HasColumnName("lead_source");

                entity.Property(e => e.Lifecyclestage)
                    .HasMaxLength(100)
                    .HasColumnName("lifecyclestage");

                entity.Property(e => e.MergedVids)
                    .HasMaxLength(500)
                    .HasColumnName("merged-vids");

                entity.Property(e => e.PortalId).HasColumnName("PortalID");

                entity.Property(e => e.Regions)
                    .HasMaxLength(50)
                    .HasColumnName("regions");

                entity.Property(e => e.Skype)
                    .HasMaxLength(50)
                    .HasColumnName("skype");

                entity.Property(e => e.Venture)
                    .HasMaxLength(50)
                    .HasColumnName("venture");

                entity.Property(e => e.Website)
                    .HasMaxLength(50)
                    .HasColumnName("website");

                entity.Property(e => e.ZohoCustomerId)
                    .HasMaxLength(255)
                    .HasColumnName("ZOHO_Customer_ID");

                entity.Property(e => e.ZohoOrganizationId).HasColumnName("ZohoOrganization_ID");
            });

            modelBuilder.Entity<GenIncAmNrUserRoleDetail>(entity =>
            {
                entity.ToTable("gen_Inc_AM_NR_UserRole_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractSlabId).HasColumnName("Contract_Slab_ID");

                entity.Property(e => e.IncAmountInr)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_INR");

                entity.Property(e => e.IncAmountUsd)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_USD");

                entity.Property(e => e.IncPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Inc_Percentage");

                entity.Property(e => e.UserRoleId).HasColumnName("User_Role_ID");
            });

            modelBuilder.Entity<GenIncAmTalentDeployUserRoleDetail>(entity =>
            {
                entity.ToTable("gen_Inc_AM_TalentDeploy_UserRole_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IncAmountInr)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_INR");

                entity.Property(e => e.IncAmountUsd)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_USD");

                entity.Property(e => e.IncPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Inc_Percentage");

                entity.Property(e => e.SlabId).HasColumnName("Slab_ID");

                entity.Property(e => e.UserRoleId).HasColumnName("User_Role_ID");
            });

            modelBuilder.Entity<GenIncBaseSalUserRoleDetail>(entity =>
            {
                entity.ToTable("gen_Inc_BaseSAL_UserRole_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IncAmountInr)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_INR");

                entity.Property(e => e.IncAmountUsd)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_USD");

                entity.Property(e => e.IncPercentage).HasColumnName("Inc_Percentage");

                entity.Property(e => e.SalgoalSlabId).HasColumnName("SALGoal_Slab_ID");

                entity.Property(e => e.UserRoleId).HasColumnName("User_Role_ID");
            });

            modelBuilder.Entity<GenIncContractTalentUserRoleDetail>(entity =>
            {
                entity.ToTable("gen_Inc_Contract_Talent_UserRole_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IncAmountInr)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_INR");

                entity.Property(e => e.IncAmountUsd)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_USD");

                entity.Property(e => e.IncPercentage)
                    .HasColumnType("decimal(18, 1)")
                    .HasColumnName("Inc_Percentage");

                entity.Property(e => e.UserRoleId).HasColumnName("User_Role_ID");
            });

            modelBuilder.Entity<GenIncContractUserRoleDetail>(entity =>
            {
                entity.ToTable("gen_Inc_Contract_UserRole_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractSlabId).HasColumnName("Contract_Slab_ID");

                entity.Property(e => e.IncAmountInr)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_INR");

                entity.Property(e => e.IncAmountUsd)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_USD");

                entity.Property(e => e.IncPercentage)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Percentage");

                entity.Property(e => e.UserRoleId).HasColumnName("User_Role_ID");
            });

            modelBuilder.Entity<GenIncPlacementFeesTalentUserRoleDetail>(entity =>
            {
                entity.ToTable("gen_Inc_PlacementFees_Talent_UserRole_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IncAmountInr)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_INR");

                entity.Property(e => e.IncAmountUsd)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Inc_Amount_USD");

                entity.Property(e => e.IncPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Inc_Percentage");

                entity.Property(e => e.PlacementFeesSlabId).HasColumnName("PlacementFees_Slab_Id");

                entity.Property(e => e.UserRoleId).HasColumnName("User_Role_ID");
            });

            modelBuilder.Entity<GenIncSalesIncentiveTargetDetail>(entity =>
            {
                entity.ToTable("gen_Inc_Sales_Incentive_Target_Detail");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmNrAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AM_NR_Amount");

                entity.Property(e => e.AmNrPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AM_NR_Percentage");

                entity.Property(e => e.AmNrSlab)
                    .HasMaxLength(50)
                    .HasColumnName("AM_NR_Slab");

                entity.Property(e => e.AmTargetAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AM_Target_Amount");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CbCalculatedAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CB_CalculatedAmount");

                entity.Property(e => e.CbSlab)
                    .HasMaxLength(50)
                    .HasColumnName("CB_Slab");

                entity.Property(e => e.CbSlabAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CB_SlabAmount");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.ContractBooster).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.DpAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DP_Amount");

                entity.Property(e => e.DpCalculatedAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DP_CalculatedAmount");

                entity.Property(e => e.DpSlab)
                    .HasMaxLength(50)
                    .HasColumnName("DP_Slab");

                entity.Property(e => e.DpSlabAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DP_SlabAmount");

                entity.Property(e => e.HrId).HasColumnName("HR_ID");

                entity.Property(e => e.IncMonth).HasColumnName("Inc_Month");

                entity.Property(e => e.IncYear).HasColumnName("Inc_Year");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");

                entity.Property(e => e.SelfAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("selfAmount");

                entity.Property(e => e.SelfPercentageSlab).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SlabName).HasMaxLength(50);

                entity.Property(e => e.TalentId).HasColumnName("TalentID");

                entity.Property(e => e.TalentIncentiveAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Talent_Incentive_Amount");

                entity.Property(e => e.TiCalculatedAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TI_CalculatedAmount");

                entity.Property(e => e.TiSlab)
                    .HasMaxLength(50)
                    .HasColumnName("TI_Slab");

                entity.Property(e => e.TiSlabAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TI_SlabAmount");
            });

            modelBuilder.Entity<GenIncSalesIncentiveTargetSummary>(entity =>
            {
                entity.ToTable("gen_Inc_Sales_Incentive_Target_Summary");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmNrPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AM_NR_Percentage");

                entity.Property(e => e.AmTargetPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AM_Target_Percentage");

                entity.Property(e => e.CreateByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DpPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DP_Percentage");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.SelfAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Self_Amount");

                entity.Property(e => e.SelfPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Self_Percentage");

                entity.Property(e => e.TalentIncentivePercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Talent_Incentive_Percentage");

                entity.Property(e => e.TeamAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Team_Amount");

                entity.Property(e => e.TeamPercentange)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Team_Percentange");

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
            });

            modelBuilder.Entity<GenIncSalesIncentiveTargetSummaryHistory>(entity =>
            {
                entity.ToTable("gen_Inc_Sales_Incentive_Target_Summary_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmNrPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AM_NR_Percentage");

                entity.Property(e => e.AmTargetPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AM_Target_Percentage");

                entity.Property(e => e.CreateByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DpPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DP_Percentage");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.SelfAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Self_Amount");

                entity.Property(e => e.SelfPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Self_Percentage");

                entity.Property(e => e.TalentIncentivePercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Talent_Incentive_Percentage");

                entity.Property(e => e.TeamAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Team_Amount");

                entity.Property(e => e.TeamPercentange)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Team_Percentange");

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
            });

            modelBuilder.Entity<GenIncTargetAcheivedDetail>(entity =>
            {
                entity.ToTable("gen_Inc_Target_Acheived_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmNrPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AM_NR_Percentage");

                entity.Property(e => e.AmNrSlab)
                    .HasMaxLength(50)
                    .HasColumnName("AM_NR_Slab");

                entity.Property(e => e.CbCalculatedAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CB_CalculatedAmount");

                entity.Property(e => e.CbSlab)
                    .HasMaxLength(50)
                    .HasColumnName("CB_Slab");

                entity.Property(e => e.CbSlabAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CB_SlabAmount");

                entity.Property(e => e.CompanyCategory).HasMaxLength(5);

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DpCalculatedAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DP_CalculatedAmount");

                entity.Property(e => e.DpSlab)
                    .HasMaxLength(50)
                    .HasColumnName("DP_Slab");

                entity.Property(e => e.DpSlabAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DP_SlabAmount");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.SlaMasterName)
                    .HasMaxLength(100)
                    .HasColumnName("SlaMaster_Name");

                entity.Property(e => e.TargetDate).HasColumnType("datetime");

                entity.Property(e => e.TiCalculatedAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TI_CalculatedAmount");

                entity.Property(e => e.TiSlab)
                    .HasMaxLength(50)
                    .HasColumnName("TI_Slab");

                entity.Property(e => e.TiSlabAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TI_SlabAmount");
            });

            modelBuilder.Entity<GenIncUserTarget>(entity =>
            {
                entity.ToTable("gen_Inc_UserTargets");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.SelfTarget)
                    .HasColumnType("decimal(18, 1)")
                    .HasColumnName("Self_Target");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserTarget)
                    .HasColumnType("decimal(18, 1)")
                    .HasColumnName("User_Target");
            });

            modelBuilder.Entity<GenIncUserTargetsHistory>(entity =>
            {
                entity.ToTable("gen_Inc_UserTargets_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.SelfTarget)
                    .HasColumnType("decimal(18, 1)")
                    .HasColumnName("Self_Target");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserTarget)
                    .HasColumnType("decimal(18, 1)")
                    .HasColumnName("User_Target");
            });

            modelBuilder.Entity<GenInterviewSlotsMaster>(entity =>
            {
                entity.ToTable("gen_InterviewSlotsMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.InterviewRoundCount).HasColumnName("InterviewRound_Count");

                entity.Property(e => e.InterviewRoundStr)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("InterviewRound_Str");

                entity.Property(e => e.InterviewStatusId).HasColumnName("InterviewStatus_ID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.RescheduleReasonId).HasColumnName("RescheduleReasonID");

                entity.Property(e => e.RescheduledBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Rescheduled_By");

                entity.Property(e => e.RescheduledInterviewId).HasColumnName("Rescheduled_InterviewID");

                entity.Property(e => e.RescheduledMessageShown)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Rescheduled_MessageShown");

                entity.Property(e => e.RescheduledReasons)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Rescheduled_Reasons");

                entity.Property(e => e.RescheduledTypeId).HasColumnName("Rescheduled_TypeID");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");
            });

            modelBuilder.Entity<GenJobPostEmploymentDetailsClientPortal>(entity =>
            {
                entity.ToTable("gen_JobPost_Employment_Details_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AchievementId).HasColumnName("AchievementID");

                entity.Property(e => e.ClientIpaddress)
                    .HasMaxLength(200)
                    .HasColumnName("ClientIPAddress");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmploymentType).HasMaxLength(200);

                entity.Property(e => e.Gptjdid).HasColumnName("GPTJDID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(200)
                    .HasColumnName("GUID");

                entity.Property(e => e.HowSoon).HasMaxLength(200);

                entity.Property(e => e.Ipaddress)
                    .HasMaxLength(50)
                    .HasColumnName("IPAddress");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsHrTypeDp).HasColumnName("IsHrTypeDP");

                entity.Property(e => e.JobTypeId).HasColumnName("JobTypeID");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ProcessType).HasMaxLength(50);

                entity.Property(e => e.TimeZoneEndTime)
                    .HasMaxLength(100)
                    .HasColumnName("TimeZone_EndTime");

                entity.Property(e => e.TimeZoneFromTime)
                    .HasMaxLength(100)
                    .HasColumnName("TimeZone_FromTime");

                entity.Property(e => e.TimeZoneId).HasColumnName("TimeZoneID");

                entity.Property(e => e.TimezonePreferenceId).HasColumnName("Timezone_Preference_ID");

                entity.Property(e => e.WorkingModeId).HasColumnName("WorkingModeID");
            });

            modelBuilder.Entity<GenJobPostRolesResponsibilitiesClientPortal>(entity =>
            {
                entity.ToTable("gen_JobPost_Roles_Responsibilities_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientIpaddress)
                    .HasMaxLength(200)
                    .HasColumnName("ClientIPAddress");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Gptjdid).HasColumnName("GPTJDID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(200)
                    .HasColumnName("GUID");

                entity.Property(e => e.Ipaddress)
                    .HasMaxLength(50)
                    .HasColumnName("IPAddress");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.JdfileName).HasColumnName("JDFileName");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ProcessType).HasMaxLength(50);
            });

            modelBuilder.Entity<GenJobPostStandoutToTalentsClientPortal>(entity =>
            {
                entity.ToTable("gen_JobPost_StandoutToTalents_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AboutCompanyDesc).HasColumnName("about_Company_desc");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CompanysLinkedInProfile).HasMaxLength(600);

                entity.Property(e => e.ContactsLinkedInProfile).HasMaxLength(600);

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Gptjdid).HasColumnName("GPTJDID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(100)
                    .HasColumnName("GUID");

                entity.Property(e => e.IndustryType)
                    .HasMaxLength(2000)
                    .HasColumnName("Industry_Type");

                entity.Property(e => e.Ipaddress)
                    .HasMaxLength(50)
                    .HasColumnName("IPAddress");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ProcessType).HasMaxLength(50);
            });

            modelBuilder.Entity<GenJobPostSubscriptionHistoryClientPortal>(entity =>
            {
                entity.ToTable("gen_JobPost_Subscription_History_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BalanceType).HasMaxLength(100);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreditCurrency).HasMaxLength(20);

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.PaymentHistoryId).HasColumnName("PaymentHistoryID");

                entity.Property(e => e.Sladate)
                    .HasColumnType("datetime")
                    .HasColumnName("SLADate");
            });

            modelBuilder.Entity<GenJobPostTransactionHistoryClientPortal>(entity =>
            {
                entity.ToTable("gen_JobPost_TransactionHistory_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AtsTalentId).HasColumnName("ATS_TalentId");

                entity.Property(e => e.BalanceUsedType).HasMaxLength(100);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.ContactUserId).HasColumnName("ContactUserID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreditOptionId).HasColumnName("CreditOptionID");

                entity.Property(e => e.CreditUsed).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.JobPostedDate).HasColumnType("datetime");

                entity.Property(e => e.JobRepostedDate).HasColumnType("datetime");

                entity.Property(e => e.JpcreditBalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("JPCreditBalance");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.SubScriptionId).HasColumnName("SubScriptionID");

                entity.Property(e => e.UtilizedCreditAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UtilizedCreditCurreny).HasMaxLength(50);
            });

            modelBuilder.Entity<GenJobPostVitalInfoClientPortal>(entity =>
            {
                entity.ToTable("gen_JobPost_VitalInfo_ClientPortal");

                entity.Property(e => e.CreatedbyDateTime).HasColumnType("datetime");

                entity.Property(e => e.Guid)
                    .HasMaxLength(500)
                    .HasColumnName("GUID");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.ModifiedbyDateTime).HasColumnType("datetime");

                entity.Property(e => e.StringSeparator).HasMaxLength(100);
            });

            modelBuilder.Entity<GenManageNotificationEmailsHistory>(entity =>
            {
                entity.ToTable("gen_Manage_Notification_Emails_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");

                entity.Property(e => e.Subject).HasMaxLength(500);
            });

            modelBuilder.Entity<GenManageNotificationEmailsHistoryForMonday>(entity =>
            {
                entity.ToTable("gen_Manage_Notification_Emails_History_For_Monday");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");

                entity.Property(e => e.Subject).HasMaxLength(500);
            });

            modelBuilder.Entity<GenManagedTalent>(entity =>
            {
                entity.ToTable("gen_ManagedTalent");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.Hrcost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("HRCost");

                entity.Property(e => e.ManagedHrid).HasColumnName("ManagedHRId");

                entity.Property(e => e.ManagedTalentCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ManagedTalentEmailId)
                    .HasMaxLength(100)
                    .HasColumnName("ManagedTalentEmailID");

                entity.Property(e => e.ManagedTalentFirstName).HasMaxLength(200);

                entity.Property(e => e.ManagedTalentLastName).HasMaxLength(200);

                entity.Property(e => e.ManagedTalentLevel)
                    .HasMaxLength(100)
                    .HasColumnName("ManagedTalent_Level");

                entity.Property(e => e.ManagedTalentRoleId).HasColumnName("ManagedTalentRoleID");

                entity.Property(e => e.ManagedTalentTypeId).HasColumnName("ManagedTalent_TypeID");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.Nrpercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("NRPercentage");

                entity.Property(e => e.PocFullName)
                    .HasMaxLength(200)
                    .HasColumnName("POC_FullName");

                entity.Property(e => e.ProposalConfirmDate).HasColumnType("datetime");

                entity.Property(e => e.ScopeOfWork).HasMaxLength(2000);
            });

            modelBuilder.Entity<GenManagedTalentHistory>(entity =>
            {
                entity.ToTable("gen_ManagedTalent_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.Hrcost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("HRCost");

                entity.Property(e => e.ManagedHrid).HasColumnName("ManagedHRId");

                entity.Property(e => e.ManagedTalentCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ManagedTalentEmailId)
                    .HasMaxLength(100)
                    .HasColumnName("ManagedTalentEmailID");

                entity.Property(e => e.ManagedTalentFirstName).HasMaxLength(200);

                entity.Property(e => e.ManagedTalentLastName).HasMaxLength(200);

                entity.Property(e => e.ManagedTalentLevel)
                    .HasMaxLength(100)
                    .HasColumnName("ManagedTalent_Level");

                entity.Property(e => e.ManagedTalentRoleId).HasColumnName("ManagedTalentRoleID");

                entity.Property(e => e.ManagedTalentTypeId).HasColumnName("ManagedTalent_TypeID");

                entity.Property(e => e.Nrpercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("NRPercentage");

                entity.Property(e => e.PocFullName)
                    .HasMaxLength(200)
                    .HasColumnName("POC_FullName");

                entity.Property(e => e.ProposalConfirmDate).HasColumnType("datetime");

                entity.Property(e => e.ScopeOfWork).HasMaxLength(2000);
            });

            modelBuilder.Entity<GenMenuMaster>(entity =>
            {
                entity.ToTable("gen_MenuMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ControllerName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LiClass)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MenuUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("MenuURL");

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ParentMenuId).HasColumnName("ParentMenuID");
            });

            modelBuilder.Entity<GenMenuRight>(entity =>
            {
                entity.ToTable("gen_MenuRights");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.GenMenuRights)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_gen_MenuRights_gen_MenuMaster");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.GenMenuRights)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_gen_MenuRights_usr_UserType");
            });

            modelBuilder.Entity<GenNearByLocation>(entity =>
            {
                entity.ToTable("gen_NearByLocations");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

                entity.Property(e => e.MappingDistrictId).HasColumnName("MappingDistrictID");
            });

            modelBuilder.Entity<GenNurtureEmailLog>(entity =>
            {
                entity.ToTable("gen_NurtureEmail_Logs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastEmailSentDateTime).HasColumnType("datetime");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            });

            modelBuilder.Entity<GenNurtureEmailSummary>(entity =>
            {
                entity.ToTable("gen_NurtureEmail_Summary");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.Month).HasColumnName("MONTH");

                entity.Property(e => e.Year).HasColumnName("YEAR");
            });

            modelBuilder.Entity<GenOnBoardClientContractDetail>(entity =>
            {
                entity.ToTable("gen_OnBoardClientContractDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdditionalCostPerMonthRdpsecurity)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AdditionalCostPerMonth_RDPSecurity");

                entity.Property(e => e.ClientBaseCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ClientRemark).HasColumnName("Client_Remark");

                entity.Property(e => e.ContractCancelledDate).HasColumnType("datetime");

                entity.Property(e => e.ContractEndDate).HasColumnType("datetime");

                entity.Property(e => e.ContractEndDateUpdateReason).HasColumnName("ContractEndDate_UpdateReason");

                entity.Property(e => e.ContractLostReasonId).HasColumnName("ContractLost_ReasonID");

                entity.Property(e => e.ContractStartDate).HasColumnType("datetime");

                entity.Property(e => e.ContractType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceCostAsPerPolicy).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DevicesPoliciesOption)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ExpectationFromTalentFirstMonth)
                    .HasMaxLength(2000)
                    .HasColumnName("ExpectationFromTalent_FirstMonth");

                entity.Property(e => e.ExpectationFromTalentFirstWeek)
                    .HasMaxLength(2000)
                    .HasColumnName("ExpectationFromTalent_FirstWeek");

                entity.Property(e => e.FirstClientBillingDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceRaiseTo).HasMaxLength(255);

                entity.Property(e => e.InvoiceRaiseToEmail).HasMaxLength(255);

                entity.Property(e => e.IsContractCompleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsRecurring).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LastWorkingDate).HasColumnType("datetime");

                entity.Property(e => e.LostDate).HasColumnType("datetime");

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");

                entity.Property(e => e.ProceedWithClientLeavePolicyFileUpload)
                    .HasMaxLength(200)
                    .HasColumnName("ProceedWithClient_LeavePolicyFileUpload");

                entity.Property(e => e.ProceedWithClientLeavePolicyLink)
                    .HasMaxLength(200)
                    .HasColumnName("ProceedWithClient_LeavePolicyLink");

                entity.Property(e => e.ProceedWithClientLeavePolicyOption)
                    .HasMaxLength(200)
                    .HasColumnName("ProceedWithClient_LeavePolicyOption");

                entity.Property(e => e.ProceedWithUplersExitPolicyOption)
                    .HasMaxLength(200)
                    .HasColumnName("ProceedWithUplers_ExitPolicyOption");

                entity.Property(e => e.ProceedWithUplersLeavePolicyOption)
                    .HasMaxLength(200)
                    .HasColumnName("ProceedWithUplers_LeavePolicyOption");

                entity.Property(e => e.PunchEndTime).HasMaxLength(50);

                entity.Property(e => e.PunchStartTime).HasMaxLength(50);

                entity.Property(e => e.TalentContractConcern)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_ContractConcern");

                entity.Property(e => e.TalentDevicePolicesConcern)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_DevicePolicesConcern");

                entity.Property(e => e.TalentExitPolicyConcern)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_ExitPolicyConcern");

                entity.Property(e => e.TalentLeavePolicyConcern)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_LeavePolicyConcern");

                entity.Property(e => e.TalentMonthlyCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TalentOnBoardDate).HasColumnType("datetime");

                entity.Property(e => e.TalentRemark).HasColumnName("Talent_Remark");

                entity.Property(e => e.TalentWorkingTimeZone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TalentWorkingTimeZoneConcern)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_WorkingTimeZoneConcern");

                entity.Property(e => e.TimezonePreferenceId).HasColumnName("Timezone_Preference_ID");

                entity.Property(e => e.TotalApprovedLeaves).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalNumberofLeaves).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateReasonSupportFilename)
                    .HasMaxLength(500)
                    .HasColumnName("UpdateReason_SupportFilename");

                entity.Property(e => e.UpdatedContractEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_ContractEndDate");

                entity.Property(e => e.WorkingDays)
                    .HasMaxLength(2000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GenOnBoardClientDevicePolicyDetail>(entity =>
            {
                entity.ToTable("gen_OnBoardClientDevicePolicy_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientDeviceDescription).HasColumnName("Client_DeviceDescription");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.IsRecurring).HasDefaultValueSql("((0))");

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");

                entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<GenOnBoardClientInvoiceSummary>(entity =>
            {
                entity.ToTable("gen_OnBoardClientInvoiceSummary");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DeviceCostAsPerPolicy).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IsRecurring).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.NextMonthAdditionalPayout)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("NextMonth_AdditionalPayout");

                entity.Property(e => e.NextMonthBaseCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("NextMonth_BaseCost");

                entity.Property(e => e.NextMonthCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NextMonthDeviceCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("NextMonth_DeviceCost");

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");

                entity.Property(e => e.TalentAdditionalPayout).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TalentBaseCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TalentPackage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TalentPayOut).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalFirstMonthCost).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<GenOnBoardClientLeavePolicy>(entity =>
            {
                entity.ToTable("gen_OnBoardClientLeavePolicy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasMaxLength(50);

                entity.Property(e => e.Day)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Occasion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");
            });

            modelBuilder.Entity<GenOnBoardClientTeam>(entity =>
            {
                entity.ToTable("gen_OnBoardClientTeam");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Buddy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Designation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Linkedin).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");

                entity.Property(e => e.ReportingTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GenOnBoardTalent>(entity =>
            {
                entity.ToTable("gen_OnBoardTalents");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmSalesPersonId).HasColumnName("AM_SalesPersonID");

                entity.Property(e => e.City).HasMaxLength(200);

                entity.Property(e => e.ClientClosureDate).HasColumnType("datetime");

                entity.Property(e => e.ClientCurrencyCode)
                    .HasMaxLength(10)
                    .HasColumnName("Client_CurrencyCode");

                entity.Property(e => e.ClientLegalDate).HasColumnType("datetime");

                entity.Property(e => e.ClientLegalStatusId)
                    .HasColumnName("ClientLegal_StatusID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ClientOnBoardingDate).HasColumnType("datetime");

                entity.Property(e => e.ClientOnBoardingStatusId)
                    .HasColumnName("ClientOnBoarding_StatusID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CurrencyExchangeRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DeleteHrreason).HasColumnName("DeleteHRReason");

                entity.Property(e => e.DeleteHrreasonId).HasColumnName("DeleteHRReason_Id");

                entity.Property(e => e.Dpamount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DPAmount");

                entity.Property(e => e.Dppercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DPPercentage");

                entity.Property(e => e.EngagemenId)
                    .HasMaxLength(50)
                    .HasColumnName("EngagemenID");

                entity.Property(e => e.FinalHrCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Final_HR_Cost");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.HiredDate).HasColumnType("datetime");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.IsHrmanaged)
                    .HasColumnName("IsHRManaged")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IstZoomEndTime).HasColumnName("IST_Zoom_EndTime");

                entity.Property(e => e.IstZoomScheduledOn)
                    .HasColumnType("datetime")
                    .HasColumnName("IST_Zoom_ScheduledOn");

                entity.Property(e => e.IstZoomStartTime).HasColumnName("IST_Zoom_StartTime");

                entity.Property(e => e.Joiningdate).HasColumnType("datetime");

                entity.Property(e => e.KickoffDate).HasColumnType("datetime");

                entity.Property(e => e.KickoffStatusId)
                    .HasColumnName("Kickoff_StatusID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.KickoffTimezonePreferenceId).HasColumnName("Kickoff_Timezone_Preference_ID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LossRemark).IsUnicode(false);

                entity.Property(e => e.ManagedTalentId).HasColumnName("ManagedTalentID");

                entity.Property(e => e.ModeOfworkingId).HasColumnName("ModeOFWorkingID");

                entity.Property(e => e.NbdSalesPersonId).HasColumnName("NBD_SalesPersonID");

                entity.Property(e => e.Nrpercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("NRPercentage");

                entity.Property(e => e.OldTscPersonId).HasColumnName("OldTSC_PersonID");

                entity.Property(e => e.ShortlistedInterviewId).HasColumnName("Shortlisted_InterviewID");

                entity.Property(e => e.Sowcount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("SOWCount")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Sowdocument).HasColumnName("SOWDocument");

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.TalentCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Talent_Cost");

                entity.Property(e => e.TalentCurrencyCode)
                    .HasMaxLength(10)
                    .HasColumnName("Talent_CurrencyCode");

                entity.Property(e => e.TalentCurrencyExchangeRate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Talent_CurrencyExchangeRate");

                entity.Property(e => e.TalentFeesStr).HasMaxLength(100);

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TalentLegalDate).HasColumnType("datetime");

                entity.Property(e => e.TalentLegalStatusId)
                    .HasColumnName("TalentLegal_StatusID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TalentOnBoardingDate).HasColumnType("datetime");

                entity.Property(e => e.TalentOnBoardingStatusId)
                    .HasColumnName("TalentOnBoarding_StatusID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TscPersonId).HasColumnName("TSC_PersonID");

                entity.Property(e => e.TsceditReason).HasColumnName("TSCEditReason");

                entity.Property(e => e.ZoomEndTime).HasColumnName("Zoom_EndTime");

                entity.Property(e => e.ZoomInterviewKitPassword)
                    .HasMaxLength(200)
                    .HasColumnName("Zoom_InterviewKit_password");

                entity.Property(e => e.ZoomInterviewKitUsername)
                    .HasMaxLength(200)
                    .HasColumnName("Zoom_InterviewKit_username");

                entity.Property(e => e.ZoomInterviewLink)
                    .HasMaxLength(2000)
                    .HasColumnName("Zoom_InterviewLink");

                entity.Property(e => e.ZoomMeetingId)
                    .HasMaxLength(100)
                    .HasColumnName("Zoom_MeetingID");

                entity.Property(e => e.ZoomMeetingscheduledOn)
                    .HasColumnType("datetime")
                    .HasColumnName("Zoom_MeetingscheduledOn");

                entity.Property(e => e.ZoomStartTime).HasColumnName("Zoom_StartTime");
            });

            modelBuilder.Entity<GenOnBoardTalentsClientFeedback>(entity =>
            {
                entity.ToTable("gen_OnBoardTalents_ClientFeedback");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.FeedbackCreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.FeedbackType).HasMaxLength(50);

                entity.Property(e => e.FileName).HasMaxLength(200);

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");
            });

            modelBuilder.Entity<GenOnBoardTalentsLegalDetail>(entity =>
            {
                entity.ToTable("gen_OnBoardTalents_LegalDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyLegalId).HasColumnName("CompanyLegalID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");
            });

            modelBuilder.Entity<GenOnBoardTalentsReplacementDetail>(entity =>
            {
                entity.ToTable("gen_OnBoardTalents_ReplacementDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientAgreementDateForReplacement).HasColumnType("datetime");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");

                entity.Property(e => e.LastWorkingDay).HasColumnType("datetime");

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.NewHrid).HasColumnName("NewHRID");

                entity.Property(e => e.NewOnBoardId).HasColumnName("NewOnBoard_Id");

                entity.Property(e => e.OldTalentClosureDate).HasColumnType("datetime");

                entity.Property(e => e.OnboardId).HasColumnName("OnboardID");

                entity.Property(e => e.ReplaceTalentClosureDate).HasColumnType("datetime");

                entity.Property(e => e.ReplacementDate).HasColumnType("datetime");

                entity.Property(e => e.ReplacementHandledById).HasColumnName("ReplacementHandledByID");

                entity.Property(e => e.ReplacementInitiatedBy).HasMaxLength(20);

                entity.Property(e => e.ReplacementStage).HasMaxLength(100);

                entity.Property(e => e.TalentAgreementDateForReplacement).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenOnlyDealId>(entity =>
            {
                entity.ToTable("gen_OnlyDealID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DealId).HasColumnName("DealID");

                entity.Property(e => e.IsInserted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<GenPayPerViewModelClientPortal>(entity =>
            {
                entity.ToTable("gen_PayPerViewModel_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreditBalance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");
            });

            modelBuilder.Entity<GenPaymentHistoryClientPortal>(entity =>
            {
                entity.ToTable("gen_Payment_History_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Cgstpercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CGSTPercentage");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Currency).HasMaxLength(20);

                entity.Property(e => e.Guid).HasMaxLength(200);

                entity.Property(e => e.Igstpercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("IGSTPercentage");

                entity.Property(e => e.InvoiceId)
                    .HasMaxLength(250)
                    .HasColumnName("InvoiceID");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(250)
                    .HasColumnName("Order_ID");

                entity.Property(e => e.PayerId)
                    .HasMaxLength(100)
                    .HasColumnName("PayerID");

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(500)
                    .HasColumnName("PaymentID");

                entity.Property(e => e.PaymentMethod).HasMaxLength(100);

                entity.Property(e => e.PaymentToken).HasMaxLength(250);

                entity.Property(e => e.RazorPayPaymentStatus).HasMaxLength(500);

                entity.Property(e => e.ReceiptId)
                    .HasMaxLength(250)
                    .HasColumnName("ReceiptID");

                entity.Property(e => e.Sgstpercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("SGSTPercentage");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxPercentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalInvoiceAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<GenPaymentOrderDetail>(entity =>
            {
                entity.ToTable("gen_PaymentOrderDetails");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Currency).HasMaxLength(50);

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(250)
                    .HasColumnName("Order_ID");

                entity.Property(e => e.OrderResponse).HasColumnName("Order_Response");

                entity.Property(e => e.ReceiptId)
                    .HasMaxLength(50)
                    .HasColumnName("ReceiptID");
            });

            modelBuilder.Entity<GenPayoutInformation>(entity =>
            {
                entity.ToTable("gen_Payout_Information");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActualBillRate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Actual_Bill_Rate");

                entity.Property(e => e.ActualCommentsBr).HasColumnName("Actual_CommentsBR");

                entity.Property(e => e.ActualCommentsPr).HasColumnName("Actual_CommentsPR");

                entity.Property(e => e.ActualCurrency)
                    .HasMaxLength(5)
                    .HasColumnName("Actual_Currency");

                entity.Property(e => e.ActualNrPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Actual_NR_Percentage");

                entity.Property(e => e.ActualPayOutDate).HasColumnType("datetime");

                entity.Property(e => e.ActualPayRate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Actual_pay_Rate");

                entity.Property(e => e.AmSalesPersonId).HasColumnName("AM_SalesPersonID");

                entity.Property(e => e.ClientFinalPayOutAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Client_FinalPayOutAmount");

                entity.Property(e => e.ClientInvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.ClientInvoiceDocument)
                    .HasMaxLength(200)
                    .HasColumnName("Client_InvoiceDocument");

                entity.Property(e => e.ClientPayOutAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Client_PayOutAmount");

                entity.Property(e => e.ClientProRataAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Client_ProRataAmount");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.ContractType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CurrencyExcahangeDate).HasColumnType("datetime");

                entity.Property(e => e.CurrencyExchangeCode).HasMaxLength(10);

                entity.Property(e => e.CurrencyExchangeRate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EngagementStatusId).HasColumnName("EngagementStatusID");

                entity.Property(e => e.EsalesInvoiceAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("ESales_InvoiceAmount");

                entity.Property(e => e.EsalesInvoiceDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ESales_InvoiceDate");

                entity.Property(e => e.EsalesInvoiceId).HasColumnName("ESalesInvoiceID");

                entity.Property(e => e.EsalesInvoiceLineItemTotal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("ESales_InvoiceLineItem_Total");

                entity.Property(e => e.EsalesInvoiceNumber)
                    .HasMaxLength(50)
                    .HasColumnName("ESales_InvoiceNumber");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequestDetail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.InvoiceDocument).HasMaxLength(200);

                entity.Property(e => e.InvoiceNumber).HasMaxLength(50);

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.OnBoardId).HasColumnName("OnBoardID");

                entity.Property(e => e.PayOutAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PayOutDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentInInr)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PaymentInINR");

                entity.Property(e => e.PaymentReleasedInr)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PaymentReleasedINR");

                entity.Property(e => e.PerDayClientCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PerDayTalentCostInr)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PerDayTalentCost_INR");

                entity.Property(e => e.ReasonBr).HasColumnName("Reason_BR");

                entity.Property(e => e.ReasonPr).HasColumnName("Reason_PR");

                entity.Property(e => e.StatusId).HasColumnName("Status_Id");

                entity.Property(e => e.TalentCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TalentCurrencyCode)
                    .HasMaxLength(10)
                    .HasColumnName("Talent_CurrencyCode");

                entity.Property(e => e.TalentCurrencyExchangeRate)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Talent_CurrencyExchangeRate");

                entity.Property(e => e.TalentFinalPayOutInInr)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Talent_FinalPayOutInINR");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");

                entity.Property(e => e.TalentProRataAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Talent_ProRataAmount");

                entity.Property(e => e.TalentProRataAmountInr)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Talent_ProRataAmount_INR");

                entity.Property(e => e.Tds)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TDS");

                entity.Property(e => e.TdscalculatedAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TDSCalculatedAmount");

                entity.Property(e => e.TotalApprovedLeaves).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalNumberofLeaves).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TscPersonId).HasColumnName("TSC_PersonID");

                entity.Property(e => e.TsceditReason).HasColumnName("TSCEditReason");

                entity.Property(e => e.UserInvoiceNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<GenPmsTalentRedirectUrlDetail>(entity =>
            {
                entity.ToTable("gen_PMS_Talent_RedirectURL_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifyByDatetime).HasColumnType("datetime");

                entity.Property(e => e.PmsUuid)
                    .HasMaxLength(100)
                    .HasColumnName("PMS_UUID");

                entity.Property(e => e.RedirectUrl)
                    .HasMaxLength(400)
                    .HasColumnName("Redirect_Url");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");
            });

            modelBuilder.Entity<GenPostAcceptanceDetail>(entity =>
            {
                entity.ToTable("gen_PostAcceptanceDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcceptedAvailability)
                    .HasMaxLength(200)
                    .HasColumnName("Accepted_Availability");

                entity.Property(e => e.AcceptedHowsoon)
                    .HasMaxLength(200)
                    .HasColumnName("Accepted_Howsoon");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.RadioAvailability)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Radio_Availability");

                entity.Property(e => e.RadioHowsoon)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Radio_Howsoon");

                entity.Property(e => e.RadioShift)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Radio_Shift");

                entity.Property(e => e.ShortListedId).HasColumnName("ShortListed_ID");

                entity.Property(e => e.StrTimeZone)
                    .HasMaxLength(500)
                    .HasColumnName("Str_TimeZone");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TimeZoneIds)
                    .HasMaxLength(100)
                    .HasColumnName("TimeZone_IDs");
            });

            modelBuilder.Entity<GenPostJobEmailNotificationClientPortal>(entity =>
            {
                entity.ToTable("gen_PostJob_Email_Notification_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.Subject).HasMaxLength(200);
            });

            modelBuilder.Entity<GenPostalCodeswithDetail>(entity =>
            {
                entity.ToTable("gen_PostalCodeswithDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(50)
                    .HasColumnName("country_code");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(50)
                    .HasColumnName("postal_code");

                entity.Property(e => e.Province)
                    .HasMaxLength(200)
                    .HasColumnName("province");

                entity.Property(e => e.StateEn)
                    .HasMaxLength(200)
                    .HasColumnName("state_en");
            });

            modelBuilder.Entity<GenRazorPayWebhookHistory>(entity =>
            {
                entity.ToTable("gen_RazorPay_Webhook_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmountCaptured).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.RazorPayPaymentId).HasMaxLength(500);

                entity.Property(e => e.WebhookActionStatus).HasMaxLength(250);
            });

            modelBuilder.Entity<GenRoleAndHiringTypeClientPortal>(entity =>
            {
                entity.ToTable("gen_RoleAndHiringType_ClientPortal");

                entity.Property(e => e.AtsJobLocationId).HasColumnName("ATS_JobLocationID");

                entity.Property(e => e.AtsNearByCities).HasColumnName("ATS_NearByCities");

                entity.Property(e => e.Availability).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(500);

                entity.Property(e => e.Country).HasMaxLength(500);

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.FrequencyOfficeVisitId).HasColumnName("FrequencyOfficeVisitID");

                entity.Property(e => e.Gptjdid).HasColumnName("GPTJDID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.Ipaddress)
                    .HasMaxLength(50)
                    .HasColumnName("IPAddress");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsFresherAllowed).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsHiringLimited).HasMaxLength(50);

                entity.Property(e => e.IsOpenToWorkNearByCities).HasDefaultValueSql("((0))");

                entity.Property(e => e.JobLocation).HasMaxLength(500);

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.PostalCode).HasMaxLength(20);

                entity.Property(e => e.ProcessType).HasMaxLength(50);

                entity.Property(e => e.RoleName).HasMaxLength(100);

                entity.Property(e => e.ShowHrpocdetailsToTalents).HasColumnName("ShowHRPOCDetailsToTalents");

                entity.Property(e => e.State).HasMaxLength(500);

                entity.Property(e => e.WorkingModeId).HasColumnName("WorkingModeID");
            });

            modelBuilder.Entity<GenSalesHiringRequest>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AboutCompanyDesc).HasColumnName("about_Company_desc");

                entity.Property(e => e.AcceptedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.AcceptedById).HasColumnName("AcceptedByID");

                entity.Property(e => e.AdhocPassDate).HasColumnType("datetime");

                entity.Property(e => e.AmSalesPersonId).HasColumnName("AM_SalesPersonID");

                entity.Property(e => e.Availability).HasMaxLength(100);

                entity.Property(e => e.Bqlink).HasColumnName("BQLink");

                entity.Property(e => e.ChildCompanyId).HasColumnName("ChildCompanyID");

                entity.Property(e => e.ChildContactId).HasColumnName("ChildContactID");

                entity.Property(e => e.ClientJobModifyByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ClientJobModifyById).HasColumnName("ClientJobModifyByID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DealId).HasColumnName("DealID");

                entity.Property(e => e.DeleteHrreason).HasColumnName("DeleteHRReason");

                entity.Property(e => e.DeleteHrreasonId).HasColumnName("DeleteHRReason_Id");

                entity.Property(e => e.DiscoveryCall).HasColumnName("Discovery_Call");

                entity.Property(e => e.Dppercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DPPercentage");

                entity.Property(e => e.Guid)
                    .HasMaxLength(100)
                    .HasColumnName("GUID");

                entity.Property(e => e.HrNumber)
                    .HasMaxLength(50)
                    .HasColumnName("HR_Number");

                entity.Property(e => e.HrTypeId1).HasColumnName("HR_TypeId");

                entity.Property(e => e.HrtypeId).HasColumnName("HRTypeId");

                entity.Property(e => e.IsAccepted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsAdHocHr).HasColumnName("IsAdHocHR");

                entity.Property(e => e.IsCreatedByClient).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDirectHr).HasColumnName("IsDirectHR");

                entity.Property(e => e.IsHiringLimited).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsHrtypeDp).HasColumnName("IsHRTypeDP");

                entity.Property(e => e.IsPartnerHr).HasColumnName("IsPartnerHR");

                entity.Property(e => e.IsPoolHr)
                    .IsRequired()
                    .HasColumnName("IsPoolHR")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.JddumpId).HasColumnName("JDDump_ID");

                entity.Property(e => e.Jdfilename)
                    .HasMaxLength(200)
                    .HasColumnName("JDFilename");

                entity.Property(e => e.Jdurl).HasColumnName("JDURL");

                entity.Property(e => e.JobExpiredOrclosedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("JobExpiredORClosedDate");

                entity.Property(e => e.JobExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.JobStatusId).HasColumnName("JobStatusID");

                entity.Property(e => e.JobTypeId).HasColumnName("JobTypeID");

                entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LossRemark).IsUnicode(false);

                entity.Property(e => e.MonthDuration).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NbdSalesPersonId).HasColumnName("NBD_SalesPersonID");

                entity.Property(e => e.NotAcceptedHrreason).HasColumnName("NotAcceptedHRReason");

                entity.Property(e => e.OnHoldRemark).IsUnicode(false);

                entity.Property(e => e.PartialEngagementTypeId).HasColumnName("PartialEngagementType_ID");

                entity.Property(e => e.PauseHrreason).HasColumnName("PauseHRReason");

                entity.Property(e => e.PauseHrreasonId).HasColumnName("PauseHRReasonID");

                entity.Property(e => e.PayrollPartnerName).HasMaxLength(300);

                entity.Property(e => e.PreviousHiringRequestId).HasColumnName("Previous_HiringRequest_ID");

                entity.Property(e => e.ReplaceOnBoardId)
                    .HasColumnName("ReplaceOnBoardID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RequestForTalent).HasMaxLength(2000);

                entity.Property(e => e.SalesUserId).HasColumnName("SalesUserID");

                entity.Property(e => e.ShowHrpocdetailsToTalents).HasColumnName("ShowHRPOCDetailsToTalents");

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.TalentCostCalcPercentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TrAccepted).HasColumnName("TR_Accepted");
            });

            modelBuilder.Entity<GenSalesHiringRequestChannel>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest_Channel");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChannelInviteLink).HasMaxLength(200);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");

                entity.Property(e => e.LastMessageDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenSalesHiringRequestChannelUserDetail>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest_Channel_UserDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");

                entity.Property(e => e.UserDesignation).HasMaxLength(1000);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserName).HasMaxLength(1000);
            });

            modelBuilder.Entity<GenSalesHiringRequestDetail>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddOtherRole).HasMaxLength(100);

                entity.Property(e => e.AdhocBudgetCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Adhoc_BudgetCost");

                entity.Property(e => e.AmbitionBoxRating).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AssociatedwithUplersId).HasColumnName("AssociatedwithUplers_ID");

                entity.Property(e => e.BudgetFrom).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BudgetTo).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CalculatedUplersfees).HasMaxLength(300);

                entity.Property(e => e.CommunicationType).HasMaxLength(50);

                entity.Property(e => e.Cost).HasMaxLength(100);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Duration).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DurationType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GlassdoorRating).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.HowSoon).HasMaxLength(200);

                entity.Property(e => e.HrCost)
                    .HasMaxLength(200)
                    .HasColumnName("HR_Cost");

                entity.Property(e => e.InterviewerDesignation).HasMaxLength(50);

                entity.Property(e => e.InterviewerEmailId)
                    .HasMaxLength(50)
                    .HasColumnName("InterviewerEmailID");

                entity.Property(e => e.InterviewerId).HasColumnName("Interviewer_ID");

                entity.Property(e => e.InterviewerName).HasMaxLength(100);

                entity.Property(e => e.InterviewerYearofExperience).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.IsConfidentialBudget).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsFresherAllowed).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsHrfocused)
                    .HasColumnName("IsHRFocused")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.NotMatchAfterAcceptanceTimezoneId).HasColumnName("NotMatch_AfterAcceptance_Timezone_ID");

                entity.Property(e => e.OverlapingHours).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.PartialEngagementTypeId).HasColumnName("PartialEngagementType_ID");

                entity.Property(e => e.PreviousRoleStatusId).HasColumnName("Previous_RoleStatusID");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.RoleStatusId).HasColumnName("RoleStatus_ID");

                entity.Property(e => e.ScreeningQuestionsGeneratedDateTime).HasColumnType("datetime");

                entity.Property(e => e.TimeZoneEndTime)
                    .HasMaxLength(50)
                    .HasColumnName("TimeZone_EndTime");

                entity.Property(e => e.TimeZoneFromTime)
                    .HasMaxLength(50)
                    .HasColumnName("TimeZone_FromTime");

                entity.Property(e => e.TimezoneId).HasColumnName("Timezone_ID");

                entity.Property(e => e.TimezonePreference)
                    .HasMaxLength(2000)
                    .HasColumnName("Timezone_Preference");

                entity.Property(e => e.TimezonePreferenceId).HasColumnName("Timezone_Preference_ID");

                entity.Property(e => e.TypeofInterviewerId).HasColumnName("TypeofInterviewer_ID");

                entity.Property(e => e.YearOfExp).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<GenSalesHiringRequestDetailsHistory>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest_Details_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddOtherRole).HasMaxLength(100);

                entity.Property(e => e.AdhocBudgetCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Adhoc_BudgetCost");

                entity.Property(e => e.AmbitionBoxRating).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.AssociatedwithUplersId).HasColumnName("AssociatedwithUplers_ID");

                entity.Property(e => e.BudgetFrom).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BudgetTo).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CalculatedUplersfees).HasMaxLength(300);

                entity.Property(e => e.CommunicationType).HasMaxLength(50);

                entity.Property(e => e.Cost).HasMaxLength(100);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Duration).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DurationType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GlassdoorRating).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.HistoryDate).HasColumnType("datetime");

                entity.Property(e => e.HistoryId).HasColumnName("History_ID");

                entity.Property(e => e.HowSoon).HasMaxLength(200);

                entity.Property(e => e.HrCost)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("HR_Cost");

                entity.Property(e => e.InterviewerDesignation).HasMaxLength(50);

                entity.Property(e => e.InterviewerEmailId)
                    .HasMaxLength(50)
                    .HasColumnName("InterviewerEmailID");

                entity.Property(e => e.InterviewerId).HasColumnName("Interviewer_ID");

                entity.Property(e => e.InterviewerName).HasMaxLength(100);

                entity.Property(e => e.InterviewerYearofExperience).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.IsFresherAllowed).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsHrfocused).HasColumnName("IsHRFocused");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.NotMatchAfterAcceptanceTimezoneId).HasColumnName("NotMatch_AfterAcceptance_Timezone_ID");

                entity.Property(e => e.OverlapingHours).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.PartialEngagementTypeId).HasColumnName("PartialEngagementType_ID");

                entity.Property(e => e.PreviousRoleStatusId).HasColumnName("Previous_RoleStatusID");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.RoleStatusId).HasColumnName("RoleStatus_ID");

                entity.Property(e => e.TimeZoneEndTime)
                    .HasMaxLength(50)
                    .HasColumnName("TimeZone_EndTime");

                entity.Property(e => e.TimeZoneFromTime)
                    .HasMaxLength(50)
                    .HasColumnName("TimeZone_FromTime");

                entity.Property(e => e.TimezoneId).HasColumnName("Timezone_ID");

                entity.Property(e => e.TimezonePreference)
                    .HasMaxLength(2000)
                    .HasColumnName("Timezone_Preference");

                entity.Property(e => e.TimezonePreferenceId).HasColumnName("Timezone_Preference_ID");

                entity.Property(e => e.TypeofInterviewerId).HasColumnName("TypeofInterviewer_ID");

                entity.Property(e => e.YearOfExp).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<GenSalesHiringRequestHistory>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AboutCompanyDesc).HasColumnName("about_Company_desc");

                entity.Property(e => e.AcceptedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.AcceptedById).HasColumnName("AcceptedByID");

                entity.Property(e => e.AdhocPassDate).HasColumnType("datetime");

                entity.Property(e => e.AmSalesPersonId).HasColumnName("AM_SalesPersonID");

                entity.Property(e => e.AtsJobLocationId).HasColumnName("ATS_JobLocationID");

                entity.Property(e => e.AtsNearByCities).HasColumnName("ATS_NearByCities");

                entity.Property(e => e.Availability).HasMaxLength(100);

                entity.Property(e => e.Bqlink).HasColumnName("BQLink");

                entity.Property(e => e.ChildCompanyId).HasColumnName("ChildCompanyID");

                entity.Property(e => e.ChildContactId).HasColumnName("ChildContactID");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.ClientJobModifyByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ClientJobModifyById).HasColumnName("ClientJobModifyByID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DealId).HasColumnName("DealID");

                entity.Property(e => e.DeleteHrreason).HasColumnName("DeleteHRReason");

                entity.Property(e => e.DeleteHrreasonId).HasColumnName("DeleteHRReason_Id");

                entity.Property(e => e.DiscoveryCall).HasColumnName("Discovery_Call");

                entity.Property(e => e.Dppercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DPPercentage");

                entity.Property(e => e.FrequencyOfficeVisitId).HasColumnName("FrequencyOfficeVisitID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(200)
                    .HasColumnName("GUID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.HistoryDate).HasColumnType("datetime");

                entity.Property(e => e.HistoryId).HasColumnName("History_ID");

                entity.Property(e => e.HrNumber)
                    .HasMaxLength(50)
                    .HasColumnName("HR_Number");

                entity.Property(e => e.HrTypeId1).HasColumnName("HR_TypeId");

                entity.Property(e => e.HrtypeId).HasColumnName("HRTypeId");

                entity.Property(e => e.IsAdHocHr).HasColumnName("IsAdHocHR");

                entity.Property(e => e.IsDirectHr).HasColumnName("IsDirectHR");

                entity.Property(e => e.IsHrtypeDp).HasColumnName("IsHRTypeDP");

                entity.Property(e => e.IsPartnerHr).HasColumnName("IsPartnerHR");

                entity.Property(e => e.IsPoolHr).HasColumnName("IsPoolHR");

                entity.Property(e => e.JddumpId).HasColumnName("JDDump_ID");

                entity.Property(e => e.Jdfilename)
                    .HasMaxLength(200)
                    .HasColumnName("JDFilename");

                entity.Property(e => e.Jdurl).HasColumnName("JDURL");

                entity.Property(e => e.JobExpiredOrclosedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("JobExpiredORClosedDate");

                entity.Property(e => e.JobLocation).HasMaxLength(500);

                entity.Property(e => e.JobStatusId).HasColumnName("JobStatusID");

                entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LossRemark).IsUnicode(false);

                entity.Property(e => e.ModeOfWork).HasMaxLength(100);

                entity.Property(e => e.MonthDuration).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NbdSalesPersonId).HasColumnName("NBD_SalesPersonID");

                entity.Property(e => e.NotAcceptedHrreason).HasColumnName("NotAcceptedHRReason");

                entity.Property(e => e.OnHoldRemark).IsUnicode(false);

                entity.Property(e => e.PartialEngagementTypeId).HasColumnName("PartialEngagementType_ID");

                entity.Property(e => e.PauseHrreason).HasColumnName("PauseHRReason");

                entity.Property(e => e.PauseHrreasonId).HasColumnName("PauseHRReasonID");

                entity.Property(e => e.PayrollPartnerName).HasMaxLength(600);

                entity.Property(e => e.PreviousHiringRequestId).HasColumnName("Previous_HiringRequest_ID");

                entity.Property(e => e.ReplaceOnBoardId).HasColumnName("ReplaceOnBoardID");

                entity.Property(e => e.RequestForTalent).HasMaxLength(2000);

                entity.Property(e => e.SalesUserId).HasColumnName("SalesUserID");

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.TalentCostCalcPercentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TrAccepted).HasColumnName("TR_Accepted");
            });

            modelBuilder.Entity<GenSalesHiringRequestInterviewerDetail>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest_InterviewerDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.InterviewerDesignation).HasMaxLength(50);

                entity.Property(e => e.InterviewerEmailId)
                    .HasMaxLength(50)
                    .HasColumnName("InterviewerEmailID");

                entity.Property(e => e.InterviewerName).HasMaxLength(100);

                entity.Property(e => e.InterviewerYearofExperience).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.IsUsedAddMore).HasDefaultValueSql("((0))");

                entity.Property(e => e.TypeofInterviewerId).HasColumnName("TypeofInterviewer_ID");
            });

            modelBuilder.Entity<GenSalesHiringRequestPriority>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest_Priority");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");

                entity.Property(e => e.IsNextWeekPriority).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.PriorityEndDate).HasColumnType("datetime");

                entity.Property(e => e.PriorityStartDate).HasColumnType("datetime");

                entity.Property(e => e.SalesUserId).HasColumnName("SalesUserID");
            });

            modelBuilder.Entity<GenSalesHiringRequestPriorityHistory>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest_Priority_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");

                entity.Property(e => e.IsNextWeekPriority).HasDefaultValueSql("((0))");

                entity.Property(e => e.PriorityEndDate).HasColumnType("datetime");

                entity.Property(e => e.PriorityStartDate).HasColumnType("datetime");

                entity.Property(e => e.SalesUserId).HasColumnName("SalesUserID");
            });

            modelBuilder.Entity<GenSalesHiringRequestSkillDetail>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest_SkillDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.Proficiency).HasMaxLength(50);

                entity.Property(e => e.SkillId).HasColumnName("Skill_ID");

                entity.Property(e => e.TempSkillId)
                    .HasMaxLength(50)
                    .HasColumnName("TempSkill_ID");
            });

            modelBuilder.Entity<GenSalesHiringRequestTalentPacketDetail>(entity =>
            {
                entity.ToTable("gen_SalesHiringRequest_TalentPacketDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssessmentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("assessment_date");

                entity.Property(e => e.AssessmentTool)
                    .HasMaxLength(500)
                    .HasColumnName("assessment_tool");

                entity.Property(e => e.ClientAssesmentReport).HasColumnName("client_assesment_report");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");

                entity.Property(e => e.Score)
                    .HasMaxLength(50)
                    .HasColumnName("score");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");

                entity.Property(e => e.TalentResumeLink).HasColumnName("talent_resume_link");
            });

            modelBuilder.Entity<GenSalesHrJddump>(entity =>
            {
                entity.ToTable("gen_SalesHR_JDDump");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Company).HasMaxLength(500);

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.HowLong).HasMaxLength(2000);

                entity.Property(e => e.HowSoon).HasMaxLength(2000);

                entity.Property(e => e.JdRequirement)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("JD_Requirement");

                entity.Property(e => e.JdRolesResponsibilities)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("JD_RolesResponsibilities");

                entity.Property(e => e.JdallSkills).HasColumnName("JDAllSkills");

                entity.Property(e => e.Jddescription).HasColumnName("JDDescription");

                entity.Property(e => e.Jddetails).HasColumnName("JDDetails");

                entity.Property(e => e.JdfileName)
                    .HasMaxLength(500)
                    .HasColumnName("JDFileName");

                entity.Property(e => e.Jdrole).HasColumnName("JDRole");

                entity.Property(e => e.JdskillPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("JDSkill_Percentage");

                entity.Property(e => e.Jdskills).HasColumnName("JDSkills");

                entity.Property(e => e.Jdtools).HasColumnName("JDTools");

                entity.Property(e => e.Qty).HasMaxLength(500);

                entity.Property(e => e.TimeZone).HasMaxLength(500);

                entity.Property(e => e.YearsOfExp).HasMaxLength(2000);
            });

            modelBuilder.Entity<GenSalesHrJddumpSkillDetail>(entity =>
            {
                entity.ToTable("gen_SalesHR_JDDump_SkillDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JddumpId).HasColumnName("JDDump_ID");

                entity.Property(e => e.SkillId).HasColumnName("Skill_ID");

                entity.Property(e => e.TempSkillId)
                    .HasMaxLength(50)
                    .HasColumnName("TempSkill_ID");
            });

            modelBuilder.Entity<GenSalesHrTracceptedDetail>(entity =>
            {
                entity.ToTable("gen_SalesHR_TRAccepted_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.TrAccepted).HasColumnName("TR_Accepted");
            });

            modelBuilder.Entity<GenSalesHrTrupdatedDetail>(entity =>
            {
                entity.ToTable("gen_SalesHR_TRUpdated_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequestID");

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");

                entity.Property(e => e.IsTrcancel).HasColumnName("IsTRCancel");

                entity.Property(e => e.IsTrlost).HasColumnName("IsTRLost");

                entity.Property(e => e.OriginalTr).HasColumnName("OriginalTR");

                entity.Property(e => e.PreviousHrstatusId).HasColumnName("Previous_HRStatus_Id");

                entity.Property(e => e.UpdatedTr).HasColumnName("UpdatedTR");
            });

            modelBuilder.Entity<GenSalesHrwiseContactIdforAt>(entity =>
            {
                entity.ToTable("gen_SalesHRWise_ContactIDforATS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(500)
                    .HasColumnName("GUID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.HrwiseContactId).HasColumnName("HRWise_ContactID");
            });

            modelBuilder.Entity<GenSchedularHistory>(entity =>
            {
                entity.ToTable("gen_Schedular_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastRunDateTime).HasColumnType("datetime");

                entity.Property(e => e.SchedularName).HasMaxLength(500);

                entity.Property(e => e.Spname)
                    .HasMaxLength(1000)
                    .HasColumnName("SPName");

                entity.Property(e => e.Spprofiler).HasColumnName("SPProfiler");
            });

            modelBuilder.Entity<GenShortistedTalentUnlockDetailClientPortal>(entity =>
            {
                entity.ToTable("gen_ShortistedTalent_Unlock_Detail_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AtsTalentId).HasColumnName("ATS_TalentID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.Hrid).HasColumnName("HRID");
            });

            modelBuilder.Entity<GenShortlistedTalent>(entity =>
            {
                entity.ToTable("gen_ShortlistedTalents");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientJoinWithin)
                    .HasMaxLength(2000)
                    .HasColumnName("Client_JoinWithin");

                entity.Property(e => e.ClientReadytoworkShift)
                    .HasMaxLength(2000)
                    .HasColumnName("Client_ReadytoworkShift");

                entity.Property(e => e.ClientReadytoworkhrs)
                    .HasMaxLength(2000)
                    .HasColumnName("Client_Readytoworkhrs");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.IsConfirmed).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsInterested).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPostAacceptanceConfirm).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSeenByTalent).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.SeenDatetime).HasColumnType("datetime");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TalentJoinWithin)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_JoinWithin");

                entity.Property(e => e.TalentReadytoworkShift)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_ReadytoworkShift");

                entity.Property(e => e.TalentReadytoworkhrs)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_Readytoworkhrs");
            });

            modelBuilder.Entity<GenShortlistedTalentInterviewDetail>(entity =>
            {
                entity.ToTable("gen_ShortlistedTalent_InterviewDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DurationInHours).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.InterviewEndTime).HasColumnName("Interview_EndTime");

                entity.Property(e => e.InterviewMasterId).HasColumnName("InterviewMaster_ID");

                entity.Property(e => e.InterviewSlot).HasMaxLength(50);

                entity.Property(e => e.InterviewStartTime).HasColumnName("Interview_StartTime");

                entity.Property(e => e.IstInterviewEndTime).HasColumnName("IST_Interview_EndTime");

                entity.Property(e => e.IstInterviewStartTime).HasColumnName("IST_Interview_StartTime");

                entity.Property(e => e.IstScheduledOn)
                    .HasColumnType("datetime")
                    .HasColumnName("IST_ScheduledOn");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.NextRoundId).HasColumnName("NextRound_ID");

                entity.Property(e => e.ScheduledOn).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TimeZoneId).HasColumnName("TimeZone_ID");
            });

            modelBuilder.Entity<GenShortlistedTalentsClientPortal>(entity =>
            {
                entity.ToTable("gen_ShortlistedTalents_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Ctpid).HasColumnName("CTPId");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");
            });

            modelBuilder.Entity<GenSkillAndBudgetClientPortal>(entity =>
            {
                entity.ToTable("gen_SkillAndBudget_ClientPortal");

                entity.Property(e => e.BudgetFrom).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.BudgetTo).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Currency).HasMaxLength(20);

                entity.Property(e => e.Gptjdid).HasColumnName("GPTJDID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.Ipaddress)
                    .HasMaxLength(50)
                    .HasColumnName("IPAddress");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsConfidentialBudget).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ProcessType).HasMaxLength(50);
            });

            modelBuilder.Entity<GenSlaEditHistory>(entity =>
            {
                entity.ToTable("gen_SLA_Edit_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");

                entity.Property(e => e.Sladate)
                    .HasColumnType("datetime")
                    .HasColumnName("SLADate");
            });

            modelBuilder.Entity<GenSocialMediaLoginDetail>(entity =>
            {
                entity.ToTable("gen_SocialMediaLogin_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.ResponseEmailId)
                    .HasMaxLength(50)
                    .HasColumnName("ResponseEmailID");

                entity.Property(e => e.ResponseName).HasMaxLength(500);

                entity.Property(e => e.SocialMediaLogin).HasMaxLength(100);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<GenSystemConfiguration>(entity =>
            {
                entity.ToTable("gen_SystemConfiguration");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Key)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Value).IsUnicode(false);
            });

            modelBuilder.Entity<GenTalent>(entity =>
            {
                entity.ToTable("gen_Talent");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AadharcardPhotoCopy).HasMaxLength(200);

                entity.Property(e => e.AddOtherRole).HasMaxLength(100);

                entity.Property(e => e.AssociatedwithUplersId).HasColumnName("AssociatedwithUplers_ID");

                entity.Property(e => e.AtsNonNdaurl)
                    .HasMaxLength(500)
                    .HasColumnName("ATS_Non_NDAURL");

                entity.Property(e => e.AtsTalentId).HasColumnName("ATS_Talent_ID");

                entity.Property(e => e.AtsTalentInramount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("ATS_Talent_INRAmount");

                entity.Property(e => e.AtsTalentLiveUrl)
                    .HasMaxLength(500)
                    .HasColumnName("ATS_Talent_LiveURL");

                entity.Property(e => e.City).HasMaxLength(200);

                entity.Property(e => e.CodeRepositoryUrl)
                    .HasMaxLength(400)
                    .HasColumnName("CodeRepositoryURL");

                entity.Property(e => e.ComfortableAreaId).HasColumnName("ComfortableArea_ID");

                entity.Property(e => e.CommunicationSkillsId).HasColumnName("CommunicationSkills_ID");

                entity.Property(e => e.ContactNumber).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CurrentCtc)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("CurrentCTC");

                entity.Property(e => e.CurrentEmploymentStatusId).HasColumnName("CurrentEmploymentStatus_ID");

                entity.Property(e => e.Cvfilename)
                    .HasMaxLength(200)
                    .HasColumnName("CVFilename");

                entity.Property(e => e.Designation).HasMaxLength(600);

                entity.Property(e => e.EmailId)
                    .HasMaxLength(200)
                    .HasColumnName("EmailID");

                entity.Property(e => e.ExpectedSalary).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FaceBookId)
                    .HasMaxLength(100)
                    .HasColumnName("FaceBookID");

                entity.Property(e => e.Fees).HasMaxLength(200);

                entity.Property(e => e.FinalCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.HearAboutUplers).HasMaxLength(200);

                entity.Property(e => e.InstagramId)
                    .HasMaxLength(100)
                    .HasColumnName("InstagramID");

                entity.Property(e => e.IsAccountCreated).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsApproved).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsEmailSent).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsLegalSigned).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsOdrActive)
                    .HasColumnName("is_odr_active")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPasswordChanged).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsProfileSubmitted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsResetPassword).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsTalentNotificationSend).HasDefaultValueSql("((1))");

                entity.Property(e => e.JoiningId).HasColumnName("Joining_ID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.LinkedInProfile).HasMaxLength(500);

                entity.Property(e => e.MinimumFees)
                    .HasMaxLength(200)
                    .HasColumnName("Minimum_Fees");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Otp)
                    .HasMaxLength(10)
                    .HasColumnName("OTP");

                entity.Property(e => e.OtpCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("OTP_CreatedDate");

                entity.Property(e => e.OtpExpiredDate)
                    .HasColumnType("datetime")
                    .HasColumnName("OTP_ExpiredDate");

                entity.Property(e => e.OtpIsActive).HasColumnName("OTP_IsActive");

                entity.Property(e => e.PackageBasedonShift).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PancardPhotoCopy).HasMaxLength(200);

                entity.Property(e => e.PartialEngagementTypeId).HasColumnName("PartialEngagementType_ID");

                entity.Property(e => e.Password).HasMaxLength(400);

                entity.Property(e => e.PhotoImage).HasMaxLength(200);

                entity.Property(e => e.PmsSocialProviderId).HasColumnName("PMS_social_Provider_ID");

                entity.Property(e => e.PmsStatus)
                    .HasColumnName("PMS_status")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PmsUuid)
                    .HasMaxLength(100)
                    .HasColumnName("PMS_uuid");

                entity.Property(e => e.PortfolioPluginUrl)
                    .HasMaxLength(500)
                    .HasColumnName("PortfolioPluginURL");

                entity.Property(e => e.PreviousTalentAccountStatusId).HasColumnName("PreviousTalentAccount_Status_ID");

                entity.Property(e => e.ProfileUrl)
                    .HasMaxLength(200)
                    .HasColumnName("ProfileURL");

                entity.Property(e => e.ResumeId).HasColumnName("ResumeID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.SignatureFile).HasMaxLength(200);

                entity.Property(e => e.SkypeId)
                    .HasMaxLength(100)
                    .HasColumnName("SkypeID");

                entity.Property(e => e.State).HasMaxLength(200);

                entity.Property(e => e.TalentAccountStatusId).HasColumnName("TalentAccount_Status_ID");

                entity.Property(e => e.TalentCategory).HasMaxLength(50);

                entity.Property(e => e.TalentStatusIdAfterClientSelection).HasColumnName("TalentStatusID_AfterClientSelection");

                entity.Property(e => e.TalentTimeZoneId).HasColumnName("TalentTimeZone_ID");

                entity.Property(e => e.TalentTypeId).HasColumnName("Talent_TypeID");

                entity.Property(e => e.TotalExpYears).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.Username).HasMaxLength(200);

                entity.Property(e => e.WhatsAppNumber).HasMaxLength(50);

                entity.Property(e => e.Zipcode).HasMaxLength(10);
            });

            modelBuilder.Entity<GenTalentApplicationTool>(entity =>
            {
                entity.ToTable("gen_TalentApplicationTool");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicationToolId).HasColumnName("ApplicationTool_ID");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TempApplicationToolsId)
                    .HasMaxLength(50)
                    .HasColumnName("TempApplicationTools_ID");
            });

            modelBuilder.Entity<GenTalentAssesmentScore>(entity =>
            {
                entity.ToTable("gen_TalentAssesmentScore");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AbstractReasoning).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AbstractReasoningBenchmarkId).HasColumnName("AbstractReasoning_benchmark_ID");

                entity.Property(e => e.AttentionToDetails).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AttentionToDetailsBenchmarkId).HasColumnName("AttentionToDetails_benchmark_ID");

                entity.Property(e => e.CodilityFile)
                    .HasMaxLength(200)
                    .HasColumnName("Codility_File");

                entity.Property(e => e.CodilityScore).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CodilityUrl)
                    .HasMaxLength(200)
                    .HasColumnName("Codility_URL");

                entity.Property(e => e.CognitiveAbilities).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CognitiveBenchmarkId).HasColumnName("Cognitive_benchmark_ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DataAnalysis).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DataAnalysisBenchmarkId).HasColumnName("DataAnalysis_benchmark_ID");

                entity.Property(e => e.HackerRank).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HackerRankFile)
                    .HasMaxLength(200)
                    .HasColumnName("HackerRank_File");

                entity.Property(e => e.HackerRankUrl)
                    .HasMaxLength(200)
                    .HasColumnName("HackerRank_URL");

                entity.Property(e => e.HiremeeDescription).HasMaxLength(2000);

                entity.Property(e => e.HiremeeFile)
                    .HasMaxLength(200)
                    .HasColumnName("Hiremee_File");

                entity.Property(e => e.HiremeeScore).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HiremeeUrl)
                    .HasMaxLength(200)
                    .HasColumnName("Hiremee_URL");

                entity.Property(e => e.InterviewPercentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InterviewScore).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InterviewUrl)
                    .HasMaxLength(200)
                    .HasColumnName("Interview_URL");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsHiremeeChecked).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.LogicalReasoning).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LogicalReasoningBenchmarkId).HasColumnName("LogicalReasoning_benchmark_ID");

                entity.Property(e => e.MettleScore).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MettleScoreFile)
                    .HasMaxLength(200)
                    .HasColumnName("MettleScore_File");

                entity.Property(e => e.MettleScoreUrl)
                    .HasMaxLength(200)
                    .HasColumnName("MettleScore_URL");

                entity.Property(e => e.NumericalAbilities).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NumericalAbility).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NumericalAbilityBenchmarkId).HasColumnName("NumericalAbility_benchmark_ID");

                entity.Property(e => e.NumericalBenchmarkId).HasColumnName("Numerical_benchmark_ID");

                entity.Property(e => e.Rank).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SecondarySkillInterviewPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("SecondarySkill_InterviewPercentage");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.TalentId).HasColumnName("TalentID");

                entity.Property(e => e.TotalScore).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VersantScore).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VersantScoreFile)
                    .HasMaxLength(200)
                    .HasColumnName("VersantScore_File");

                entity.Property(e => e.VersantScoreUrl)
                    .HasMaxLength(200)
                    .HasColumnName("VersantScore_URL");
            });

            modelBuilder.Entity<GenTalentAssesmentSkillScore>(entity =>
            {
                entity.ToTable("gen_TalentAssesmentSkill_Scores");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssesmentScoreId).HasColumnName("AssesmentScore_ID");

                entity.Property(e => e.AssesmentSkillId).HasColumnName("AssesmentSkill_ID");

                entity.Property(e => e.AssesmentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SkillScore).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");
            });

            modelBuilder.Entity<GenTalentBackOutDetailsForAt>(entity =>
            {
                entity.ToTable("gen_TalentBackOut_Details_For_ATS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AtstalentId).HasColumnName("ATSTalentId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EngagementEndDate)
                    .HasMaxLength(50)
                    .HasColumnName("engagement_end_date");

                entity.Property(e => e.EngagementId)
                    .HasMaxLength(50)
                    .HasColumnName("engagement_id");

                entity.Property(e => e.EngagementStartDate)
                    .HasMaxLength(50)
                    .HasColumnName("engagement_start_date");

                entity.Property(e => e.EngagementStatus)
                    .HasMaxLength(50)
                    .HasColumnName("engagement_status");

                entity.Property(e => e.HiredDate)
                    .HasMaxLength(50)
                    .HasColumnName("hired_date");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.LastWorkingDate)
                    .HasMaxLength(50)
                    .HasColumnName("last_working_date");

                entity.Property(e => e.LostDate)
                    .HasMaxLength(50)
                    .HasColumnName("lost_date");

                entity.Property(e => e.TalentStatus)
                    .HasMaxLength(50)
                    .HasColumnName("talent_status");
            });

            modelBuilder.Entity<GenTalentCertificationsDetail>(entity =>
            {
                entity.ToTable("gen_TalentCertifications_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CertificationsId).HasColumnName("Certifications_ID");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TempCertificationId)
                    .HasMaxLength(50)
                    .HasColumnName("TempCertification_ID");
            });

            modelBuilder.Entity<GenTalentClientTele>(entity =>
            {
                entity.ToTable("gen_TalentClientTele");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");
            });

            modelBuilder.Entity<GenTalentComfortableAreaDetail>(entity =>
            {
                entity.ToTable("gen_TalentComfortableArea_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ComfortableAreaId).HasColumnName("ComfortableArea_ID");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");
            });

            modelBuilder.Entity<GenTalentComfortableWorkingDetail>(entity =>
            {
                entity.ToTable("gen_TalentComfortableWorking_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ComfortableWorkingId).HasColumnName("ComfortableWorking_ID");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");
            });

            modelBuilder.Entity<GenTalentEducationDetail>(entity =>
            {
                entity.ToTable("gen_TalentEducationDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Degree).HasMaxLength(200);

                entity.Property(e => e.Fieldofstudy).HasMaxLength(200);

                entity.Property(e => e.InstituteName).HasMaxLength(200);

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");
            });

            modelBuilder.Entity<GenTalentFinancialDetail>(entity =>
            {
                entity.ToTable("gen_Talent_FinancialDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountHolderName).HasMaxLength(100);

                entity.Property(e => e.AccountType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BankAccountNumber).HasMaxLength(20);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.ContactNumber).HasMaxLength(20);

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(50)
                    .HasColumnName("EmailID");

                entity.Property(e => e.Gstno)
                    .HasMaxLength(25)
                    .HasColumnName("GSTNo");

                entity.Property(e => e.Ifsccode)
                    .HasMaxLength(50)
                    .HasColumnName("IFSCCode");

                entity.Property(e => e.LastModifyById).HasColumnName("LastModifyByID");

                entity.Property(e => e.LastModifyDatetime).HasColumnType("datetime");

                entity.Property(e => e.LegalName).HasMaxLength(200);

                entity.Property(e => e.NatureOfService).HasMaxLength(200);

                entity.Property(e => e.Panno)
                    .HasMaxLength(20)
                    .HasColumnName("PANNo");

                entity.Property(e => e.PinCode).HasMaxLength(10);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.TalentId).HasColumnName("TalentID");
            });

            modelBuilder.Entity<GenTalentGetHelpQuestion>(entity =>
            {
                entity.ToTable("gen_Talent_GetHelpQuestions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.GetHelpQuestion).HasColumnName("GetHelp_Question");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");
            });

            modelBuilder.Entity<GenTalentHistory>(entity =>
            {
                entity.ToTable("gen_TalentHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssociatedwithUplersId).HasColumnName("AssociatedwithUplers_ID");

                entity.Property(e => e.ContactNumber).HasMaxLength(50);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CurrentEmploymentStatusId).HasColumnName("CurrentEmploymentStatus_ID");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(200)
                    .HasColumnName("EmailID");

                entity.Property(e => e.Fees).HasMaxLength(200);

                entity.Property(e => e.FinalCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.JoiningId).HasColumnName("Joining_ID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.RemarkOrreason)
                    .HasMaxLength(2000)
                    .HasColumnName("RemarkORReason");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.TalentAccountStatusId).HasColumnName("TalentAccount_Status_ID");

                entity.Property(e => e.TalentActionId).HasColumnName("TalentActionID");

                entity.Property(e => e.TalentCategory).HasMaxLength(50);

                entity.Property(e => e.TalentId).HasColumnName("TalentID");

                entity.Property(e => e.TalentStatusIdAfterClientSelection).HasColumnName("TalentStatusID_AfterClientSelection");

                entity.Property(e => e.TalentTimeZoneId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TalentTimeZone_ID");

                entity.Property(e => e.TalentTypeId).HasColumnName("Talent_TypeID");

                entity.Property(e => e.TotalExpYears).HasColumnType("decimal(18, 1)");
            });

            modelBuilder.Entity<GenTalentInterestDetail>(entity =>
            {
                entity.ToTable("gen_TalentInterest_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InterestId).HasColumnName("Interest_ID");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TempInterestId)
                    .HasMaxLength(50)
                    .HasColumnName("TempInterest_ID");
            });

            modelBuilder.Entity<GenTalentInterviewFeedback>(entity =>
            {
                entity.ToTable("gen_TalentInterviewFeedback");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BetterWork).HasMaxLength(50);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.ExperienceRate)
                    .HasMaxLength(100)
                    .HasColumnName("Experience_Rate");

                entity.Property(e => e.FinancialGrowth).HasMaxLength(50);

                entity.Property(e => e.FinancialStability).HasMaxLength(50);

                entity.Property(e => e.GetEnoughTime)
                    .HasMaxLength(100)
                    .HasColumnName("GetEnough_Time");

                entity.Property(e => e.GreatCompany).HasMaxLength(50);

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.MoreInformation).HasMaxLength(4000);

                entity.Property(e => e.NecessaryInformation).HasMaxLength(200);

                entity.Property(e => e.NewSkills).HasMaxLength(50);

                entity.Property(e => e.NoThenHowMuchYouNeed)
                    .HasMaxLength(100)
                    .HasColumnName("NoThen_HowMuchYouNeed");

                entity.Property(e => e.RoleMatchwithCurrentSkills).HasMaxLength(200);

                entity.Property(e => e.ShortlistedInterviewId).HasColumnName("Shortlisted_InterviewID");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");
            });

            modelBuilder.Entity<GenTalentKeyQualityDetail>(entity =>
            {
                entity.ToTable("gen_TalentKeyQuality_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.KeyQualitiesId).HasColumnName("KeyQualities_ID");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");
            });

            modelBuilder.Entity<GenTalentLegalInfo>(entity =>
            {
                entity.ToTable("gen_TalentLegalInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AgreementStatus).HasMaxLength(50);

                entity.Property(e => e.DocumentName).HasMaxLength(500);

                entity.Property(e => e.DocumentType).HasMaxLength(200);

                entity.Property(e => e.DocumentUrl)
                    .HasMaxLength(500)
                    .HasColumnName("DocumentURL");

                entity.Property(e => e.EngagementId)
                    .HasMaxLength(50)
                    .HasColumnName("EngagementID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.LastUpdatedbyId).HasColumnName("LastUpdatedbyID");

                entity.Property(e => e.LegalDescription).HasMaxLength(2000);

                entity.Property(e => e.RenewalDate).HasColumnType("datetime");

                entity.Property(e => e.SignedDate).HasColumnType("datetime");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");

                entity.Property(e => e.ValidityEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Validity_EndDate");

                entity.Property(e => e.ValidityStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Validity_StartDate");
            });

            modelBuilder.Entity<GenTalentNotesClientPortal>(entity =>
            {
                entity.ToTable("gen_TalentNotes_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AtsnoteId)
                    .HasMaxLength(250)
                    .HasColumnName("ATSNoteID");

                entity.Property(e => e.AtstalentId).HasColumnName("ATSTalentId");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.LastModifiedByDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenTalentNotificationHistory>(entity =>
            {
                entity.ToTable("gen_Talent_NotificationHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");
            });

            modelBuilder.Entity<GenTalentPayOutAdditionalLineItem>(entity =>
            {
                entity.ToTable("gen_Talent_PayOut_Additional_LineItems");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(500)
                    .HasColumnName("Item_Name");

                entity.Property(e => e.PayOutId).HasColumnName("PayOut_ID");
            });

            modelBuilder.Entity<GenTalentPendingAction>(entity =>
            {
                entity.ToTable("gen_TalentPendingActions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.PendingAction).HasMaxLength(500);

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");
            });

            modelBuilder.Entity<GenTalentPointofContact>(entity =>
            {
                entity.ToTable("gen_TalentPointofContact");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.UserEmployeeId)
                    .HasMaxLength(50)
                    .HasColumnName("UserEmployeeID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<GenTalentPreferableWorkingDetail>(entity =>
            {
                entity.ToTable("gen_TalentPreferableWorking_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsAddedAfterPostAcceptance)
                    .HasColumnName("IsAdded_AfterPostAcceptance")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PreferableWorkingId).HasColumnName("PreferableWorking_ID");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");
            });

            modelBuilder.Entity<GenTalentPrimarySkillDetail>(entity =>
            {
                entity.ToTable("gen_TalentPrimarySkill_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PrimarySkillId).HasColumnName("PrimarySkill_ID");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");

                entity.Property(e => e.TempSkillId)
                    .HasMaxLength(50)
                    .HasColumnName("TempSkill_ID");

                entity.Property(e => e.YearsOfExp).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<GenTalentProfessionalExperience>(entity =>
            {
                entity.ToTable("gen_TalentProfessionalExperience");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyName).HasMaxLength(200);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.Skills).HasMaxLength(2000);

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.ToDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenTalentProjectDetail>(entity =>
            {
                entity.ToTable("gen_TalentProject_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyName).HasMaxLength(200);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Skills).HasMaxLength(2000);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");
            });

            modelBuilder.Entity<GenTalentResumeDetail>(entity =>
            {
                entity.ToTable("gen_TalentResumeDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.TalentAddress).HasMaxLength(500);

                entity.Property(e => e.TalentCertification).HasMaxLength(200);

                entity.Property(e => e.TalentContactNo).HasMaxLength(50);

                entity.Property(e => e.TalentDesignation).HasMaxLength(200);

                entity.Property(e => e.TalentEmailId)
                    .HasMaxLength(200)
                    .HasColumnName("TalentEmailID");

                entity.Property(e => e.TalentFirstName).HasMaxLength(200);

                entity.Property(e => e.TalentFullName).HasMaxLength(200);

                entity.Property(e => e.TalentLanguages).HasMaxLength(100);

                entity.Property(e => e.TalentLastName).HasMaxLength(100);

                entity.Property(e => e.TalentResumeFileName).HasMaxLength(100);
            });

            modelBuilder.Entity<GenTalentSecondarySkillDetail>(entity =>
            {
                entity.ToTable("gen_TalentSecondarySkill_Details");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SecondarySkillId).HasColumnName("SecondarySkill_ID");

                entity.Property(e => e.SkillRating).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");

                entity.Property(e => e.TempSkillId)
                    .HasMaxLength(50)
                    .HasColumnName("TempSkill_ID");
            });

            modelBuilder.Entity<GenTalentSelectedInterviewDetail>(entity =>
            {
                entity.ToTable("gen_TalentSelected_InterviewDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdditionalNotes).HasMaxLength(2000);

                entity.Property(e => e.ClientFeedbackGivendatetime).HasColumnType("datetime");

                entity.Property(e => e.ClientJoinWithin)
                    .HasMaxLength(2000)
                    .HasColumnName("Client_JoinWithin");

                entity.Property(e => e.ClientReadytoworkShift)
                    .HasMaxLength(2000)
                    .HasColumnName("Client_ReadytoworkShift");

                entity.Property(e => e.ClientReadytoworkhrs)
                    .HasMaxLength(2000)
                    .HasColumnName("Client_Readytoworkhrs");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.ContactTimeZoneId).HasColumnName("Contact_TimeZone_ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.InterviewClientDynamicStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Interview_ClientDynamic_Status");

                entity.Property(e => e.InterviewFeedbackStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Interview_FeedbackStatus");

                entity.Property(e => e.InterviewMasterId).HasColumnName("InterviewMaster_ID");

                entity.Property(e => e.InterviewRoundStr)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InterviewTalentDynamicStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Interview_TalentDynamic_Status");

                entity.Property(e => e.IsConfirmed).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsInterviewCompletedEmailSent).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.RescheduleDatetime).HasColumnType("datetime");

                entity.Property(e => e.ShortlistedInterviewId).HasColumnName("Shortlisted_InterviewID");

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.TalentFeedbackGivendatetime).HasColumnType("datetime");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TalentJoinWithin)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_JoinWithin");

                entity.Property(e => e.TalentReadytoworkShift)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_ReadytoworkShift");

                entity.Property(e => e.TalentReadytoworkhrs)
                    .HasMaxLength(2000)
                    .HasColumnName("Talent_Readytoworkhrs");

                entity.Property(e => e.ZoomInterviewKitPassword)
                    .HasMaxLength(200)
                    .HasColumnName("Zoom_InterviewKit_password");

                entity.Property(e => e.ZoomInterviewKitUsername)
                    .HasMaxLength(200)
                    .HasColumnName("Zoom_InterviewKit_username");

                entity.Property(e => e.ZoomInterviewLink).HasColumnName("Zoom_InterviewLink");

                entity.Property(e => e.ZoomMeetingId)
                    .HasMaxLength(100)
                    .HasColumnName("Zoom_MeetingID");

                entity.Property(e => e.ZoomMeetingscheduledOn)
                    .HasColumnType("datetime")
                    .HasColumnName("Zoom_MeetingscheduledOn");
            });

            modelBuilder.Entity<GenTalentSelectedInterviewerDetail>(entity =>
            {
                entity.ToTable("gen_TalentSelected_InterviewerDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.InterviewMasterId).HasColumnName("InterviewMaster_ID");

                entity.Property(e => e.InterviewerId).HasColumnName("Interviewer_ID");

                entity.Property(e => e.NextRoundId).HasColumnName("NextRound_ID");

                entity.Property(e => e.ShortlistedInterviewId).HasColumnName("Shortlisted_InterviewID");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TalentSelectedInterviewId).HasColumnName("TalentSelected_Interview_ID");
            });

            modelBuilder.Entity<GenTalentSelectedNextRoundInterviewDetail>(entity =>
            {
                entity.ToTable("gen_TalentSelected_NextRound_InterviewDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.IsLaterSlotGiven).HasDefaultValueSql("((0))");

                entity.Property(e => e.NextRoundOption)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShortlistedInterviewId).HasColumnName("Shortlisted_InterviewID");

                entity.Property(e => e.SlotGiven)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TalentSelectedInterviewId).HasColumnName("TalentSelected_Interview_ID");
            });

            modelBuilder.Entity<GenTalentSelectedPostAcceptanceDetail>(entity =>
            {
                entity.ToTable("gen_TalentSelected_PostAcceptanceDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AfterMatchFinalTimeZoneId).HasColumnName("AfterMatchFinal_TimeZoneID");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.HrAvailability)
                    .HasMaxLength(100)
                    .HasColumnName("HR_Availability");

                entity.Property(e => e.HrHowSoon)
                    .HasMaxLength(200)
                    .HasColumnName("HR_HowSoon");

                entity.Property(e => e.HrNoofHoursworking).HasColumnName("HR_NoofHoursworking");

                entity.Property(e => e.HrPartialEngagementTypeId).HasColumnName("HR_PartialEngagementType_ID");

                entity.Property(e => e.HrTimeZoneId).HasColumnName("HR_TimeZoneID");

                entity.Property(e => e.NotAcceptedTimeZoneId).HasColumnName("NotAccepted_TimeZoneID");

                entity.Property(e => e.TalentAvailability)
                    .HasMaxLength(100)
                    .HasColumnName("Talent_Availability");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TalentJoining)
                    .HasMaxLength(200)
                    .HasColumnName("Talent_Joining");

                entity.Property(e => e.TalentNoofHoursworking).HasColumnName("Talent_NoofHoursworking");

                entity.Property(e => e.TalentPartialEngagementTypeId).HasColumnName("Talent_PartialEngagementType_ID");

                entity.Property(e => e.TalentPreferableworkingId).HasColumnName("Talent_PreferableworkingID");

                entity.Property(e => e.TalentSelectedInterviewAcceptanceId).HasColumnName("Talent_SelectedInterview_AcceptanceID");

                entity.Property(e => e.TalentTimeZoneId).HasColumnName("Talent_TimeZoneID");

                entity.Property(e => e.TimezonePreference)
                    .HasMaxLength(2000)
                    .HasColumnName("Timezone_Preference");
            });

            modelBuilder.Entity<GenTalentSelectedPostAcceptanceNotMatchDetail>(entity =>
            {
                entity.ToTable("gen_TalentSelected_PostAcceptance_NotMatchDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HiringRequestDetailId).HasColumnName("HiringRequest_Detail_ID");

                entity.Property(e => e.HiringRequestId).HasColumnName("HiringRequest_ID");

                entity.Property(e => e.OptionSelected).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecommendedMessage).HasMaxLength(500);

                entity.Property(e => e.TalentAvailability).HasColumnName("Talent_Availability");

                entity.Property(e => e.TalentHowSoon).HasColumnName("Talent_HowSoon");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.TalentSelectedInterviewAcceptanceId).HasColumnName("Talent_SelectedInterview_AcceptanceID");

                entity.Property(e => e.TalentTimeZonePreferences).HasColumnName("Talent_TimeZonePreferences");
            });

            modelBuilder.Entity<GenTalentSelectionHistory>(entity =>
            {
                entity.ToTable("gen_TalentSelectionHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.NewStatusId).HasColumnName("NewStatus_ID");

                entity.Property(e => e.OldStatusId).HasColumnName("OldStatus_ID");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<GenTalentTestimonialOrFeedback>(entity =>
            {
                entity.ToTable("gen_TalentTestimonialOrFeedback");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientName).HasMaxLength(200);

                entity.Property(e => e.Role).HasMaxLength(200);

                entity.Property(e => e.SkillIds)
                    .HasMaxLength(500)
                    .HasColumnName("Skill_IDs");

                entity.Property(e => e.TalentId).HasColumnName("Talent_ID");
            });

            modelBuilder.Entity<GenTeam>(entity =>
            {
                entity.ToTable("gen_Teams");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DeptId).HasColumnName("DeptID");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.Team).HasMaxLength(100);

                entity.Property(e => e.TeamHeadId).HasColumnName("TeamHeadID");

                entity.Property(e => e.TeamRoleId).HasColumnName("TeamRoleID");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            });

            modelBuilder.Entity<GenTeamDistribution>(entity =>
            {
                entity.ToTable("gen_Team_Distribution");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FlagType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Geoid).HasColumnName("GEOID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<GenTestimonial>(entity =>
            {
                entity.ToTable("gen_Testimonials");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Company).HasMaxLength(200);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.GivenBy).HasMaxLength(100);

                entity.Property(e => e.Testimonials).HasMaxLength(2000);
            });

            modelBuilder.Entity<GenTicket>(entity =>
            {
                entity.ToTable("gen_Ticket");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comments).HasMaxLength(1000);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.OtherCategory).HasMaxLength(500);

                entity.Property(e => e.PageUrl).HasMaxLength(500);

                entity.Property(e => e.ScreenShot).HasMaxLength(500);

                entity.Property(e => e.TicketCategoryId).HasColumnName("TicketCategoryID");

                entity.Property(e => e.TicketStatusId).HasColumnName("TicketStatusID");

                entity.Property(e => e.TicketSubject).HasMaxLength(100);

                entity.Property(e => e.TicketTechnicalJustificationId).HasColumnName("Ticket_TechnicalJustificationID");

                entity.Property(e => e.VedioUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<GenTrackingLeadClientPortal>(entity =>
            {
                entity.ToTable("gen_TrackingLead_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CookieId).HasColumnName("CookieID");

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Device).HasMaxLength(100);

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(100)
                    .HasColumnName("IP_Address");

                entity.Property(e => e.IsFinalSignUp).HasColumnName("IsFinalSignUP");

                entity.Property(e => e.RefUrl).HasColumnName("ref_url");

                entity.Property(e => e.State).HasMaxLength(100);

                entity.Property(e => e.UtmCampaign)
                    .HasMaxLength(500)
                    .HasColumnName("UTM_Campaign");

                entity.Property(e => e.UtmContent)
                    .HasMaxLength(500)
                    .HasColumnName("UTM_Content");

                entity.Property(e => e.UtmMedium)
                    .HasMaxLength(500)
                    .HasColumnName("UTM_Medium");

                entity.Property(e => e.UtmPlacement)
                    .HasMaxLength(500)
                    .HasColumnName("UTM_Placement");

                entity.Property(e => e.UtmSource)
                    .HasMaxLength(500)
                    .HasColumnName("UTM_Source");

                entity.Property(e => e.UtmTerm)
                    .HasMaxLength(500)
                    .HasColumnName("UTM_Term");
            });

            modelBuilder.Entity<GenTransparentPricingModelClientPortal>(entity =>
            {
                entity.ToTable("gen_TransparentPricingModel_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Gptjdid).HasColumnName("GPTJDID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.Istransparent).HasColumnName("ISTransparent");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.PricingPercentage).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<GenTscuser>(entity =>
            {
                entity.ToTable("gen_TSCUsers");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<GenUnicornRoleWithTalentDetailClientPortal>(entity =>
            {
                entity.ToTable("gen_UnicornRole_With_Talent_Detail_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AtsTalentId).HasColumnName("ATS_TalentID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.TalentId).HasColumnName("TalentID");

                entity.Property(e => e.UnicornRoleId).HasColumnName("UnicornRoleID");
            });

            modelBuilder.Entity<GenUserHistory>(entity =>
            {
                entity.ToTable("gen_UserHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionId).HasColumnName("ActionID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.ReporteeUserId).HasColumnName("ReporteeUserID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            });

            modelBuilder.Entity<GenUtsAssignmentHistory>(entity =>
            {
                entity.ToTable("gen_UTS_Assignment_History");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(50)
                    .HasColumnName("EmailID");

                entity.Property(e => e.Geoid).HasColumnName("GEOID");

                entity.Property(e => e.JsonRequest).HasColumnName("JSonRequest");

                entity.Property(e => e.JsonResponse).HasColumnName("JSonResponse");

                entity.Property(e => e.NbdLeadEmployeeId)
                    .HasMaxLength(50)
                    .HasColumnName("NBD_Lead_EmployeeID");

                entity.Property(e => e.SalesPersonEmployeeId)
                    .HasMaxLength(50)
                    .HasColumnName("SalesPerson_EmployeeID");
            });

            modelBuilder.Entity<GenUtsAtsApiRecord>(entity =>
            {
                entity.ToTable("gen_UtsAtsApi_Records");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.FromApiUrl).HasMaxLength(200);

                entity.Property(e => e.HrId).HasColumnName("HrID");

                entity.Property(e => e.ToApiUrl).HasMaxLength(200);
            });

            modelBuilder.Entity<GenUtsOnBoardLineItem>(entity =>
            {
                entity.ToTable("gen_UTS_OnBoard_LineItem");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractEndDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.DedicatedUser).HasMaxLength(100);

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EngagemenId)
                    .HasMaxLength(50)
                    .HasColumnName("EngagemenID");

                entity.Property(e => e.EsalesLineItemId)
                    .HasColumnName("Esales_LineItemID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FrequencyPeriod).HasMaxLength(50);

                entity.Property(e => e.InvoicePatternId).HasColumnName("Invoice_Pattern_ID");

                entity.Property(e => e.InvoiceTypeId).HasColumnName("Invoice_Type_ID");

                entity.Property(e => e.IsAddContractEndDateInItemDesc)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsCreatedFromUts)
                    .IsRequired()
                    .HasColumnName("IsCreatedFromUTS")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsLineItemDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.ManageSelfManage).HasColumnName("Manage_SelfManage");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.Ponumber)
                    .HasMaxLength(200)
                    .HasColumnName("PONumber");

                entity.Property(e => e.PrefferedEngagementModelId).HasColumnName("PrefferedEngagementModel_ID");

                entity.Property(e => e.ProjectExecutionId).HasColumnName("ProjectExecutionID");

                entity.Property(e => e.ProjectIdOrderId).HasColumnName("ProjectID_OrderID");

                entity.Property(e => e.ProjectName).HasMaxLength(200);

                entity.Property(e => e.QuoteTat).HasColumnName("Quote_TAT");

                entity.Property(e => e.QuoteTatType).HasColumnName("Quote_TAT_Type");

                entity.Property(e => e.QuoteTypeId).HasColumnName("Quote_TypeID");

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RowStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Row_Status")
                    .IsFixedLength();

                entity.Property(e => e.ServiceName).HasMaxLength(200);

                entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Svid)
                    .HasMaxLength(50)
                    .HasColumnName("SVID");

                entity.Property(e => e.TempProjectSelect).HasMaxLength(5);

                entity.Property(e => e.UtsGuid)
                    .HasMaxLength(100)
                    .HasColumnName("UTS_GUID");
            });

            modelBuilder.Entity<GenUtsadminReactPayload>(entity =>
            {
                entity.ToTable("gen_UTSAdmin_ReactPayloads");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apiname)
                    .HasMaxLength(100)
                    .HasColumnName("APIName");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedByDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Hrid).HasColumnName("HRID");
            });

            modelBuilder.Entity<GenUtsatsapiQueue>(entity =>
            {
                entity.ToTable("gen_UTSATSAPI_Queue");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ControllerActionName).HasMaxLength(200);

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.ToApiUrl).HasMaxLength(200);
            });

            modelBuilder.Entity<GenUtsreactApiRecord>(entity =>
            {
                entity.ToTable("gen_UTSReactApi_Records");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.FromApiUrl).HasMaxLength(200);

                entity.Property(e => e.ToApiUrl).HasMaxLength(200);
            });

            modelBuilder.Entity<HubSpotEventType>(entity =>
            {
                entity.ToTable("HubSpotEventType");

                entity.Property(e => e.CraetedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SubscriptionType).HasMaxLength(100);

                entity.Property(e => e.SubscriptionTypeCode)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<HubspotWebhookNotification>(entity =>
            {
                entity.ToTable("HubspotWebhookNotification");

                entity.Property(e => e.ChangeFlag).HasMaxLength(50);

                entity.Property(e => e.ChangeSource).HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OccurredAt).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgAchievementWithUplersReason>(entity =>
            {
                entity.ToTable("Prg_AchievementWithUplers_Reasons");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Hrtype)
                    .HasMaxLength(50)
                    .HasColumnName("HRType");

                entity.Property(e => e.Reason).HasMaxLength(500);
            });

            modelBuilder.Entity<PrgActionFilter>(entity =>
            {
                entity.ToTable("prg_ActionFilters");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionFilter).HasMaxLength(100);

                entity.Property(e => e.Color).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgAmAssignmentRule>(entity =>
            {
                entity.ToTable("prg_AM_AssignmentRule");

                entity.Property(e => e.BauUtcTagging)
                    .HasMaxLength(20)
                    .HasColumnName("BAU_UTC_Tagging");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DedicatedType).HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EngagementModelId).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsAssignToBau).HasColumnName("IsAssignToBAU");

                entity.Property(e => e.IsAssignToUts).HasColumnName("IsAssignToUTS");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PrgAmdealContact>(entity =>
            {
                entity.ToTable("prg_AMDealContacts");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_date");

                entity.Property(e => e.DealId).HasColumnName("Deal_id");

                entity.Property(e => e.HubSpotCustomerId).HasColumnName("HubSpot_Customer_id");

                entity.Property(e => e.SalesPersonId).HasColumnName("SalesPerson_Id");
            });

            modelBuilder.Entity<PrgApitimeZoneResponse>(entity =>
            {
                entity.ToTable("prg_APITimeZoneResponse");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryCode).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.FromAbbreviation).HasMaxLength(10);

                entity.Property(e => e.FromTimeStamp).HasMaxLength(100);

                entity.Property(e => e.FromZoneName).HasMaxLength(100);

                entity.Property(e => e.ToAbbreviation).HasMaxLength(10);

                entity.Property(e => e.ToTimeStamp).HasMaxLength(100);

                entity.Property(e => e.ToZoneName).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgAssessmentType>(entity =>
            {
                entity.ToTable("prg_AssessmentType");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<PrgAtshrstatus>(entity =>
            {
                entity.ToTable("prg_ATSHRStatus");

                entity.Property(e => e.Atshrstatus)
                    .HasMaxLength(200)
                    .HasColumnName("ATSHRStatus");

                entity.Property(e => e.DisplayName).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgBenchMark>(entity =>
            {
                entity.ToTable("prg_BenchMark");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BenchMark)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrgCityStateWise>(entity =>
            {
                entity.ToTable("prg_City_StateWise");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City).HasMaxLength(500);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.State).HasMaxLength(500);
            });

            modelBuilder.Entity<PrgClientEngagementModel>(entity =>
            {
                entity.ToTable("prg_ClientEngagementModel");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.IsDisplay).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModelName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgClientHappinessSurveyFeedbackOption>(entity =>
            {
                entity.ToTable("prg_ClientHappinessSurvey_FeedbackOptions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HappynessSurvayOption)
                    .HasMaxLength(500)
                    .HasColumnName("HappynessSurvay_Option");
            });

            modelBuilder.Entity<PrgClientHappinessSurveyFeedbackQuestion>(entity =>
            {
                entity.ToTable("prg_ClientHappinessSurvey_FeedbackQuestions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HappinessSurveyQuestion)
                    .HasMaxLength(500)
                    .HasColumnName("HappinessSurvey_Question");
            });

            modelBuilder.Entity<PrgClientInvoiceStatus>(entity =>
            {
                entity.ToTable("prg_ClientInvoiceStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InvoiceStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Invoice_Status");
            });

            modelBuilder.Entity<PrgClientLegalDocumentType>(entity =>
            {
                entity.ToTable("prg_ClientLegalDocumentType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientDocumentType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Client_DocumentType");

                entity.Property(e => e.LegalDescription).HasMaxLength(4000);
            });

            modelBuilder.Entity<PrgClientStage>(entity =>
            {
                entity.ToTable("Prg_ClientStage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionId)
                    .HasMaxLength(20)
                    .HasColumnName("ActionID");

                entity.Property(e => e.ClientStageName).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgClientStageWithHubSpotStage>(entity =>
            {
                entity.ToTable("Prg_ClientStage_WithHubSpotStage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionId)
                    .HasMaxLength(20)
                    .HasColumnName("ActionID");

                entity.Property(e => e.ClientStageName).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgCompanyCurrentOpenPosition>(entity =>
            {
                entity.ToTable("prg_Company_CurrentOpenPosition");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Weighted).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WeightedPercentage).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<PrgCompanyDomain>(entity =>
            {
                entity.ToTable("prg_CompanyDomain");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyDomain).HasMaxLength(200);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrgCompanyPerk>(entity =>
            {
                entity.ToTable("prg_Company_Perks");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PerksName).HasMaxLength(255);
            });

            modelBuilder.Entity<PrgCompanySize>(entity =>
            {
                entity.ToTable("prg_Company_Size");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.TargetGroupId)
                    .HasColumnName("Target_GroupID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Weighted).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WeightedPercentage).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<PrgCompanyTargetGroup>(entity =>
            {
                entity.ToTable("prg_Company_TargetGroup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Weighted).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WeightedPercentage).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<PrgCompanyType>(entity =>
            {
                entity.ToTable("prg_CompanyType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyType).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgCompanyTypeofInterviewer>(entity =>
            {
                entity.ToTable("prg_CompanyTypeofInterviewer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TypeofInterviewer)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgContactAction>(entity =>
            {
                entity.ToTable("prg_contact_Actions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionName).HasMaxLength(100);

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.DisplayName).HasMaxLength(100);

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<PrgContactStatus>(entity =>
            {
                entity.ToTable("prg_ContactStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.ContactStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Contact_Status");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<PrgContactTimeZone>(entity =>
            {
                entity.ToTable("prg_ContactTimeZone");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsttimeDiffMin).HasColumnName("ISTTimeDiffMin");

                entity.Property(e => e.ShortName).HasMaxLength(50);

                entity.Property(e => e.TimeZone).HasMaxLength(50);

                entity.Property(e => e.TimeZoneTitle).HasMaxLength(500);
            });

            modelBuilder.Entity<PrgCountryRegion>(entity =>
            {
                entity.ToTable("prg_CountryRegion");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Country)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CountryRegion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrgCreditOptionClientPortal>(entity =>
            {
                entity.ToTable("prg_CreditOption_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreditAmountId).HasColumnName("CreditAmountID");

                entity.Property(e => e.CreditUsed).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<PrgCreditOptionsWithHrtypeId>(entity =>
            {
                entity.ToTable("prg_CreditOptionsWithHRTypeIDs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreditOptionId).HasColumnName("CreditOptionID");

                entity.Property(e => e.HrtypeId).HasColumnName("HRTypeID");
            });

            modelBuilder.Entity<PrgCreditWithAmountUtilizationClientPortal>(entity =>
            {
                entity.ToTable("prg_CreditWithAmountUtilization_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreditAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CurrenyCode).HasMaxLength(10);

                entity.Property(e => e.Inramount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("INRAmount");
            });

            modelBuilder.Entity<PrgCrmApiDetail>(entity =>
            {
                entity.ToTable("prg_CRM_ApiDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AuthorizationType)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.AuthorizationValue)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.CreateByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.LastModifiedByDateTime).HasColumnType("date");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.PartnerId).HasColumnName("PartnerID");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PrgCrmApiDetails)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_prg_CRM_ApiDetails_prg_CRM_ApiPartners");
            });

            modelBuilder.Entity<PrgCrmApiPartner>(entity =>
            {
                entity.ToTable("prg_CRM_ApiPartners");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apipartner)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("APIPartner");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrgCrmApiTestsResponse>(entity =>
            {
                entity.ToTable("prg_CRM_ApiTestsResponse");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApiDetailId).HasColumnName("ApiDetailID");

                entity.Property(e => e.CreateByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.InviteUrl).HasMaxLength(500);

                entity.Property(e => e.LastModifiedByDateTime).HasColumnType("date");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PublicTestLink).HasMaxLength(500);

                entity.Property(e => e.SessionsUrl).HasMaxLength(500);

                entity.Property(e => e.TestsId).HasColumnName("TestsID");

                entity.HasOne(d => d.ApiDetail)
                    .WithMany(p => p.PrgCrmApiTestsResponses)
                    .HasForeignKey(d => d.ApiDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_prg_CRM_ApiTestsResponse_prg_CRM_ApiDetails");
            });

            modelBuilder.Entity<PrgCurrencyExchangeRate>(entity =>
            {
                entity.ToTable("prg_CurrencyExchangeRate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryCode).HasMaxLength(50);

                entity.Property(e => e.Culture).HasMaxLength(50);

                entity.Property(e => e.CurrencyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencySign).HasMaxLength(10);

                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PrgDealPipeline>(entity =>
            {
                entity.ToTable("prg_DealPipeline");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HubSpotPipeline)
                    .HasMaxLength(100)
                    .HasColumnName("HubSpot_Pipeline");

                entity.Property(e => e.Pipeline).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgDealStage>(entity =>
            {
                entity.ToTable("prg_DealStages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ColorCode).HasMaxLength(50);

                entity.Property(e => e.HubSpotStage)
                    .HasMaxLength(100)
                    .HasColumnName("HubSpot_Stage");

                entity.Property(e => e.IsDisplay).HasDefaultValueSql("((1))");

                entity.Property(e => e.PipelineId)
                    .HasMaxLength(100)
                    .HasColumnName("PipelineID");

                entity.Property(e => e.Stage).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgDealStatus>(entity =>
            {
                entity.ToTable("prg_DealStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DealStatus)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgDealStatusIdpipelineStageId>(entity =>
            {
                entity.ToTable("prg_DealStatusIDPipelineStageId");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DealStageId).HasColumnName("DealStageID");

                entity.Property(e => e.PipelineId).HasMaxLength(50);

                entity.Property(e => e.StageId)
                    .HasMaxLength(50)
                    .HasColumnName("stageId");
            });

            modelBuilder.Entity<PrgDealType>(entity =>
            {
                entity.ToTable("prg_DealType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DealType).HasMaxLength(50);

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");
            });

            modelBuilder.Entity<PrgDepartment>(entity =>
            {
                entity.ToTable("prg_Departments");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeptName).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgEditBrprReason>(entity =>
            {
                entity.ToTable("prg_EditBRPR_Reason");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Reason).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgEmploymentStatus>(entity =>
            {
                entity.ToTable("prg_EmploymentStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmploymentStatus).HasMaxLength(200);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrgEngagementStatus>(entity =>
            {
                entity.ToTable("prg_EngagementStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EngagementStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgExtractedSkillsforJdparsing>(entity =>
            {
                entity.ToTable("prg_ExtractedSkillsforJDParsing");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExtractedSkills).HasMaxLength(500);
            });

            modelBuilder.Entity<PrgFrequencyOfficeVisit>(entity =>
            {
                entity.ToTable("Prg_Frequency_Office_Visit");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Frequency).HasMaxLength(500);
            });

            modelBuilder.Entity<PrgGeo>(entity =>
            {
                entity.ToTable("prg_GEO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Geo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GEO");
            });

            modelBuilder.Entity<PrgHiringRequestReportStage>(entity =>
            {
                entity.ToTable("prg_HiringRequest_Report_Stage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HrreportStageName)
                    .HasMaxLength(200)
                    .HasColumnName("HRReportStageName");
            });

            modelBuilder.Entity<PrgHiringRequestRoleStatus>(entity =>
            {
                entity.ToTable("prg_HiringRequest_RoleStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.HiringRequestRoleStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("HiringRequest_RoleStatus");

                entity.Property(e => e.HrStatusId).HasColumnName("HR_StatusID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrgHiringRequestStatus>(entity =>
            {
                entity.ToTable("prg_HiringRequestStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AtsHiringRequestStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ATS_HiringRequest_Status");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.HiringRequestStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("HiringRequest_Status");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsUsedinAts).HasColumnName("IsUsedinATS");

                entity.Property(e => e.JobStatusId).HasColumnName("JobStatus_ID");
            });

            modelBuilder.Entity<PrgHiringRequestType>(entity =>
            {
                entity.ToTable("prg_HiringRequest_Type");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgHiringTypePricing>(entity =>
            {
                entity.ToTable("prg_HiringType_Pricing");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EngagementType).HasMaxLength(20);

                entity.Property(e => e.HrtypeId).HasColumnName("HRTypeID");

                entity.Property(e => e.PricingPercent).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Type).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgHiringTypeTransparentPricing>(entity =>
            {
                entity.ToTable("prg_HiringType_TransparentPricing");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgHistoryAction>(entity =>
            {
                entity.ToTable("prg_History_Actions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionName).HasMaxLength(250);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Ctaname)
                    .HasMaxLength(100)
                    .HasColumnName("CTAName");

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.IsDisplayInActivity).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");
            });

            modelBuilder.Entity<PrgHistoryChannelAction>(entity =>
            {
                entity.ToTable("prg_History_ChannelActions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionName).HasMaxLength(100);

                entity.Property(e => e.DisplayName).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgHowSoonClientWantTalent>(entity =>
            {
                entity.ToTable("prg_HowSoonClientWantTalent");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HowSoonClientwantTalent).HasMaxLength(50);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrgHrStatusDisplay>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Prg_HR_Status_Display");

                entity.Property(e => e.BackGroundColor).HasMaxLength(50);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.TextColor).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgHrcloseReason>(entity =>
            {
                entity.ToTable("prg_HRCloseReason");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Reason).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgHrdeleteReason>(entity =>
            {
                entity.ToTable("prg_HRDeleteReason");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.Reason)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SequenceId).HasColumnName("Sequence_ID");
            });

            modelBuilder.Entity<PrgHrtalentTag>(entity =>
            {
                entity.ToTable("prg_HRTalentTags");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TalentTagName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Talent_TagName");
            });

            modelBuilder.Entity<PrgIncAmNrSlab>(entity =>
            {
                entity.ToTable("prg_Inc_AM_NR_Slab");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractsSlab)
                    .HasMaxLength(50)
                    .HasColumnName("Contracts_Slab");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.MaxLimitPercentage).HasColumnName("MaxLimit_Percentage");

                entity.Property(e => e.MinLimitPercentage).HasColumnName("MinLimit_Percentage");
            });

            modelBuilder.Entity<PrgIncAmSlab>(entity =>
            {
                entity.ToTable("prg_Inc_AM_Slab");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MaxLimitPercentage)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("MaxLimit_Percentage");

                entity.Property(e => e.MinLimitPercentage)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("MinLimit_Percentage");

                entity.Property(e => e.Slab).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgIncContractsSlab>(entity =>
            {
                entity.ToTable("prg_Inc_Contracts_Slab");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractMonth).HasMaxLength(50);

                entity.Property(e => e.MaxLimitMonths).HasColumnName("MaxLimit_Months");

                entity.Property(e => e.MinLimitMonths).HasColumnName("MinLimit_Months");
            });

            modelBuilder.Entity<PrgIncPlacementFeesSlab>(entity =>
            {
                entity.ToTable("prg_Inc_PlacementFees_Slab");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MaxLimitPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("MaxLimit_Percentage");

                entity.Property(e => e.MinLimitPercentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("MinLimit_Percentage");

                entity.Property(e => e.PlacementFeesSlab)
                    .HasMaxLength(50)
                    .HasColumnName("PlacementFees_Slab");
            });

            modelBuilder.Entity<PrgIncSalgoalSlab>(entity =>
            {
                entity.ToTable("prg_Inc_SALGoal_Slab");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MaxLimitPercentage).HasColumnName("MaxLimit__Percentage");

                entity.Property(e => e.MinLimitPercentage).HasColumnName("MinLimit_Percentage");

                entity.Property(e => e.Salgoal)
                    .HasMaxLength(50)
                    .HasColumnName("SALGoal");
            });

            modelBuilder.Entity<PrgInterviewStatus>(entity =>
            {
                entity.ToTable("prg_InterviewStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InterviewStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<PrgJobPostStageMessage>(entity =>
            {
                entity.ToTable("Prg_JobPostStageMessages");

                entity.Property(e => e.AtsHrstatusId).HasColumnName("ATS_HRStatusID");
            });

            modelBuilder.Entity<PrgJobStatusClientPortal>(entity =>
            {
                entity.ToTable("prg_JobStatus_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DisplayName).HasMaxLength(100);

                entity.Property(e => e.JobStatus).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgJobType>(entity =>
            {
                entity.ToTable("prg_JobType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobType).HasMaxLength(500);
            });

            modelBuilder.Entity<PrgMatchMakingFactor>(entity =>
            {
                entity.ToTable("Prg_MatchMakingFactors");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.MatchMakingFactor).HasMaxLength(100);

                entity.Property(e => e.Weightage).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<PrgMatchMakingSubFactor>(entity =>
            {
                entity.ToTable("Prg_MatchMakingSubFactors");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.FactorId).HasColumnName("FactorID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.MappingColumn)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MappingTable)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MultiplyingFactor).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SubFactor)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgModeOfWorking>(entity =>
            {
                entity.ToTable("prg_ModeOfWorking");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ModeOfWorking).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgNbdfunnelStage>(entity =>
            {
                entity.ToTable("prg_NBDFunnelStage");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionId)
                    .HasMaxLength(50)
                    .HasColumnName("ActionID");

                entity.Property(e => e.StageName).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgOnBoardLegalKickOffStatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("prg_OnBoardLegalKickOffStatus");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsKickOff).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsLegal).HasDefaultValueSql("((0))");

                entity.Property(e => e.LegalKickOffStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgOnBoardPolicyDeviceMaster>(entity =>
            {
                entity.ToTable("prg_OnBoardPolicy_DeviceMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeviceCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DeviceCostInr)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("DeviceCost_INR");

                entity.Property(e => e.DeviceDescription).HasMaxLength(2000);

                entity.Property(e => e.DeviceName).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgOnboardLostReason>(entity =>
            {
                entity.ToTable("prg_OnboardLostReasons");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Reason).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgOrgLevel>(entity =>
            {
                entity.ToTable("prg_OrgLevels");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LevelName).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgPartialEngagementType>(entity =>
            {
                entity.ToTable("prg_PartialEngagementType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.PartialEngagementType).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgPauseHrReason>(entity =>
            {
                entity.ToTable("prg_PauseHR_Reasons");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<PrgPayrollType>(entity =>
            {
                entity.ToTable("prg_PayrollType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PayrollType).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgPerk>(entity =>
            {
                entity.ToTable("prg_Perks");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Perks).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgPipeline>(entity =>
            {
                entity.ToTable("prg_Pipeline");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PipelineId).HasMaxLength(50);

                entity.Property(e => e.PipelineLabel).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgPitchMeRole>(entity =>
            {
                entity.ToTable("prg_PitchMeRoles");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDatetime).HasColumnType("datetime");

                entity.Property(e => e.PitchMeRole)
                    .HasMaxLength(500)
                    .HasColumnName("PitchMe_Role");

                entity.Property(e => e.PitchMeRoleId).HasColumnName("PitchMe_RoleID");
            });

            modelBuilder.Entity<PrgPreOnBoardStatus>(entity =>
            {
                entity.ToTable("prg_PreOnBoardStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDisplayToClient).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDisplayToTalent).HasDefaultValueSql("((1))");

                entity.Property(e => e.PreOnBoardStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgRateCard>(entity =>
            {
                entity.ToTable("prg_RateCard");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientCost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.HrTimeZoneId).HasColumnName("HR_TimeZoneID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.TalentCategory).HasMaxLength(50);

                entity.Property(e => e.TalentCost).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<PrgRefUrl>(entity =>
            {
                entity.ToTable("prg_Ref_Url");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RefUrl).HasColumnName("Ref_Url");
            });

            modelBuilder.Entity<PrgRejectionMsgForTalent>(entity =>
            {
                entity.ToTable("prg_RejectionMsgForTalent");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MainKey).HasMaxLength(500);

                entity.Property(e => e.RejectionReasonId).HasColumnName("RejectionReasonID");
            });

            modelBuilder.Entity<PrgRequirement>(entity =>
            {
                entity.ToTable("prg_Requirements");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ReqText).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgRescheduleInterviewReason>(entity =>
            {
                entity.ToTable("prg_RescheduleInterviewReason");

                entity.Property(e => e.Reason).HasMaxLength(1000);
            });

            modelBuilder.Entity<PrgRolesAndResponsilbity>(entity =>
            {
                entity.ToTable("prg_RolesAndResponsilbity");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoleText).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgSkill>(entity =>
            {
                entity.ToTable("prg_Skills");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.PrimarySkillId).HasColumnName("PrimarySkill_ID");

                entity.Property(e => e.SecondarySkillId).HasColumnName("SecondarySkill_ID");

                entity.Property(e => e.Skill).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgSkillCategory>(entity =>
            {
                entity.ToTable("prg_SkillCategory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.SkillCategory)
                    .HasMaxLength(50)
                    .HasColumnName("Skill_Category");
            });

            modelBuilder.Entity<PrgSkillRank>(entity =>
            {
                entity.ToTable("prg_SkillRank");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<PrgSkillSource>(entity =>
            {
                entity.ToTable("prg_SkillSources");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.SkillSource)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgSkillsInAssesment>(entity =>
            {
                entity.ToTable("prg_SkillsInAssesment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsPrimary).HasDefaultValueSql("((1))");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.SkillId).HasColumnName("Skill_ID");
            });

            modelBuilder.Entity<PrgSkillsRole>(entity =>
            {
                entity.ToTable("prg_Skills_Roles");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.RankId).HasColumnName("Rank_ID");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.SkillId).HasColumnName("Skill_ID");
            });

            modelBuilder.Entity<PrgSlaEditReason>(entity =>
            {
                entity.ToTable("Prg_SLA_Edit_Reasons");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Reason).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgSlatime>(entity =>
            {
                entity.ToTable("prg_SLATime");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedByDateTIme");

                entity.Property(e => e.ModifiedByDateTime).HasColumnType("datetime");

                entity.Property(e => e.Slatime)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("SLATime");
            });

            modelBuilder.Entity<PrgSocialProvider>(entity =>
            {
                entity.ToTable("prg_Social_Provider");

                entity.Property(e => e.SocialProviderName)
                    .HasMaxLength(100)
                    .HasColumnName("Social_Provider_Name");
            });

            modelBuilder.Entity<PrgState>(entity =>
            {
                entity.ToTable("prg_States");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.State).HasMaxLength(500);
            });

            modelBuilder.Entity<PrgStateDistrict>(entity =>
            {
                entity.ToTable("prg_State_District");

                entity.HasIndex(e => e.Id, "ui_prg_State_District_ID")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Country).HasMaxLength(200);

                entity.Property(e => e.Latitude).HasMaxLength(200);

                entity.Property(e => e.Longitude)
                    .HasMaxLength(200)
                    .HasColumnName("longitude");
            });

            modelBuilder.Entity<PrgStateSubDistrict>(entity =>
            {
                entity.ToTable("prg_State_SubDistrict");

                entity.HasIndex(e => e.Id, "ui_prg_State_District_ID")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "ui_prg_State_SubDistrict_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            });

            modelBuilder.Entity<PrgSubscriptionPackageClientPortal>(entity =>
            {
                entity.ToTable("prg_Subscription_Package_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Package).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PackageName).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgSummaryStagesForReport>(entity =>
            {
                entity.ToTable("prg_SummaryStagesForReports");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CurrentStageActionId)
                    .HasMaxLength(50)
                    .HasColumnName("CurrentStage_ActionID");

                entity.Property(e => e.CurrentStageExtraActions).HasColumnName("Current_Stage_ExtraActions");

                entity.Property(e => e.NextStageActionId)
                    .HasMaxLength(50)
                    .HasColumnName("NextStage_ActionID");

                entity.Property(e => e.NextStageExtraActions).HasColumnName("Next_Stage_ExtraActions");

                entity.Property(e => e.Sladays).HasColumnName("SLADays");

                entity.Property(e => e.StageVersion).HasMaxLength(50);

                entity.Property(e => e.SummaryStage).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgTalentAccountStatus>(entity =>
            {
                entity.ToTable("prg_TalentAccountStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrgTalentAction>(entity =>
            {
                entity.ToTable("prg_Talent_Actions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionName).HasMaxLength(250);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");
            });

            modelBuilder.Entity<PrgTalentApplicationTool>(entity =>
            {
                entity.ToTable("prg_TalentApplicationTools");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicationTool).HasMaxLength(100);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.ImageName).HasMaxLength(200);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");
            });

            modelBuilder.Entity<PrgTalentAssociatedWithUpler>(entity =>
            {
                entity.ToTable("prg_TalentAssociatedWithUplers");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssociatedWithUplers)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DurationType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrgTalentCategory>(entity =>
            {
                entity.ToTable("prg_TalentCategory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TalentCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgTalentCertification>(entity =>
            {
                entity.ToTable("prg_TalentCertifications");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Certifications).HasMaxLength(50);

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.ImageName).HasMaxLength(200);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");
            });

            modelBuilder.Entity<PrgTalentComfortableArea>(entity =>
            {
                entity.ToTable("prg_TalentComfortableArea");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TalentComfortableArea).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgTalentComfortableWorking>(entity =>
            {
                entity.ToTable("prg_TalentComfortableWorking");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TalentComfortableWorking).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgTalentCommunicationSkill>(entity =>
            {
                entity.ToTable("prg_TalentCommunicationSkills");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CommunicationSkill).HasMaxLength(500);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrgTalentInterest>(entity =>
            {
                entity.ToTable("prg_TalentInterest");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.ImageName).HasMaxLength(200);

                entity.Property(e => e.Interest).HasMaxLength(50);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");
            });

            modelBuilder.Entity<PrgTalentJoinning>(entity =>
            {
                entity.ToTable("prg_TalentJoinning");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Joinning).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgTalentKeyQuality>(entity =>
            {
                entity.ToTable("prg_TalentKeyQualities");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.KeyQualities).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgTalentLegalDocumentType>(entity =>
            {
                entity.ToTable("prg_TalentLegalDocumentType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LegalDescription).HasMaxLength(4000);

                entity.Property(e => e.TalentDocumentType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Talent_DocumentType");
            });

            modelBuilder.Entity<PrgTalentPayoutStatus>(entity =>
            {
                entity.ToTable("prg_TalentPayoutStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PayOutStatus)
                    .HasMaxLength(50)
                    .HasColumnName("PayOut_Status");
            });

            modelBuilder.Entity<PrgTalentPreferableWorking>(entity =>
            {
                entity.ToTable("prg_TalentPreferableWorking");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TalentWorkingHrs).HasMaxLength(200);

                entity.Property(e => e.WorkingZone).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgTalentPrimarySkill>(entity =>
            {
                entity.ToTable("prg_TalentPrimarySkills");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TalentPrimarySkill).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgTalentRejectReason>(entity =>
            {
                entity.ToTable("prg_TalentRejectReason");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MainKey).HasMaxLength(500);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.ParentName).HasMaxLength(1000);

                entity.Property(e => e.Reason)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TalentStatusId).HasColumnName("TalentStatusID");
            });

            modelBuilder.Entity<PrgTalentRole>(entity =>
            {
                entity.ToTable("prg_TalentRoles");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedbyDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FrontIconImage).HasMaxLength(200);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsAdhoc).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifyByDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyById)
                    .HasColumnName("ModifyByID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PitchMeRoleId).HasColumnName("PitchMe_RoleID");
            });

            modelBuilder.Entity<PrgTalentRoleCategory>(entity =>
            {
                entity.ToTable("prg_TalentRoleCategory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.TalentCategoryId).HasColumnName("TalentCategoryID");
            });

            modelBuilder.Entity<PrgTalentRoleTemplate>(entity =>
            {
                entity.ToTable("prg_TalentRoleTemplate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.SkillIds)
                    .HasMaxLength(500)
                    .HasColumnName("Skill_IDs");

                entity.Property(e => e.SkillType).HasMaxLength(100);

                entity.Property(e => e.TemplateName).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgTalentSecondarySkill>(entity =>
            {
                entity.ToTable("prg_TalentSecondarySkills");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TalentSecondarySkill).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgTalentSkillBenchMark>(entity =>
            {
                entity.ToTable("prg_TalentSkillBenchMark");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TalentSkill)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgTalentStatus>(entity =>
            {
                entity.ToTable("prg_TalentStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDisplayedinAdmin).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsUsedtoSetupAccount).HasDefaultValueSql("((0))");

                entity.Property(e => e.TalentStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Talent_Status");
            });

            modelBuilder.Entity<PrgTalentStatusAfterClientSelection>(entity =>
            {
                entity.ToTable("prg_TalentStatus_AfterClientSelection");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TalentStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgTalentStatusClientPortal>(entity =>
            {
                entity.ToTable("prg_TalentStatus_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActionName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TalentStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TalentStatusIdBasedOnHr).HasColumnName("TalentStatusID_BasedOnHR");
            });

            modelBuilder.Entity<PrgTalentTimeZone>(entity =>
            {
                entity.ToTable("prg_TalentTimeZone");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClientCostPercentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TalentCostPercentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TalentTimeZone).HasMaxLength(50);

                entity.Property(e => e.TimeDuration).HasMaxLength(100);
            });

            modelBuilder.Entity<PrgTalentType>(entity =>
            {
                entity.ToTable("prg_TalentType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsRemoveValidation).HasDefaultValueSql("((0))");

                entity.Property(e => e.TalentType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Talent_Type");
            });

            modelBuilder.Entity<PrgTempApplicationTool>(entity =>
            {
                entity.ToTable("prg_TempApplicationTools");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicationToolsourceId).HasColumnName("ApplicationToolsource_ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.ImageName).HasMaxLength(200);

                entity.Property(e => e.IsSentInEmail).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.PrgApplicationToolsId).HasColumnName("Prg_ApplicationTools_ID");

                entity.Property(e => e.Source).HasMaxLength(2000);

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.TempApplicationTools).HasMaxLength(300);

                entity.Property(e => e.TempApplicationToolsId)
                    .HasMaxLength(50)
                    .HasColumnName("TempApplicationTools_ID");
            });

            modelBuilder.Entity<PrgTempApplicationToolsMergePrgTalentApplicationTool>(entity =>
            {
                entity.ToTable("prg_TempApplicationTools_Merge_PrgTalentApplicationTools");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MergeBy).HasColumnName("Merge_By");

                entity.Property(e => e.MergeDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("Merge_Datetime");

                entity.Property(e => e.PrgApplicationToolsId).HasColumnName("Prg_ApplicationTools_ID");

                entity.Property(e => e.TempApplicationToolsId).HasColumnName("Temp_ApplicationTools_ID");
            });

            modelBuilder.Entity<PrgTempCertification>(entity =>
            {
                entity.ToTable("prg_TempCertifications");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CertificationSourceId).HasColumnName("CertificationSource_ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.ImageName).HasMaxLength(200);

                entity.Property(e => e.IsSentInEmail).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.PrgCertificationId).HasColumnName("Prg_Certification_ID");

                entity.Property(e => e.Source).HasMaxLength(2000);

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.TempCertification).HasMaxLength(300);

                entity.Property(e => e.TempCertificationId)
                    .HasMaxLength(50)
                    .HasColumnName("TempCertification_ID");
            });

            modelBuilder.Entity<PrgTempCertificationsMergePrgTalentCertification>(entity =>
            {
                entity.ToTable("prg_TempCertifications_Merge_PrgTalentCertifications");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MergeBy).HasColumnName("Merge_By");

                entity.Property(e => e.MergeDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("Merge_Datetime");

                entity.Property(e => e.PrgTalentCertificationId).HasColumnName("Prg_TalentCertification_ID");

                entity.Property(e => e.TempCertificationId).HasColumnName("Temp_Certification_ID");
            });

            modelBuilder.Entity<PrgTempInterest>(entity =>
            {
                entity.ToTable("prg_TempInterest");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.ImageName).HasMaxLength(200);

                entity.Property(e => e.InterestSourceId).HasColumnName("InterestSource_ID");

                entity.Property(e => e.IsSentInEmail).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.PrgInterestId).HasColumnName("Prg_Interest_ID");

                entity.Property(e => e.Source).HasMaxLength(2000);

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.TempInterest).HasMaxLength(300);

                entity.Property(e => e.TempInterestId)
                    .HasMaxLength(50)
                    .HasColumnName("TempInterest_ID");
            });

            modelBuilder.Entity<PrgTempInterestMergePrgTalentInterest>(entity =>
            {
                entity.ToTable("prg_TempInterest_Merge_PrgTalentInterest");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MergeBy).HasColumnName("Merge_By");

                entity.Property(e => e.MergeDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("Merge_Datetime");

                entity.Property(e => e.PrgInterestId).HasColumnName("Prg_Interest_ID");

                entity.Property(e => e.TempInterestId).HasColumnName("Temp_Interest_ID");
            });

            modelBuilder.Entity<PrgTempSkill>(entity =>
            {
                entity.ToTable("prg_TempSkills");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.Guid)
                    .HasMaxLength(50)
                    .HasColumnName("GUID");

                entity.Property(e => e.IsSentInEmail).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.LastModifiedDatetime).HasColumnType("datetime");

                entity.Property(e => e.PrgSkillId).HasColumnName("Prg_Skill_ID");

                entity.Property(e => e.Role).HasMaxLength(300);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.SkillSourceId).HasColumnName("SkillSource_ID");

                entity.Property(e => e.Source).HasMaxLength(2000);

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.TempSkill).HasMaxLength(300);

                entity.Property(e => e.TempSkillId)
                    .HasMaxLength(50)
                    .HasColumnName("TempSkill_ID");
            });

            modelBuilder.Entity<PrgTempSkillsMergePrgSkill>(entity =>
            {
                entity.ToTable("prg_TempSkills_Merge_PrgSkills");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MergeBy).HasColumnName("Merge_By");

                entity.Property(e => e.MergeDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("Merge_Datetime");

                entity.Property(e => e.PrgSkillId).HasColumnName("Prg_Skill_ID");

                entity.Property(e => e.TempSkillId).HasColumnName("Temp_Skill_ID");
            });

            modelBuilder.Entity<PrgTicketCategory>(entity =>
            {
                entity.ToTable("prg_TicketCategory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.TicketCategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgTicketStatus>(entity =>
            {
                entity.ToTable("prg_TicketStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TicketStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgTicketTechnicalJustification>(entity =>
            {
                entity.ToTable("prg_Ticket_TechnicalJustification");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<PrgTimeZone>(entity =>
            {
                entity.ToTable("prg_TimeZone");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HrTimeZoneId).HasColumnName("HR_TimeZoneID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.RateCardCostPreference).HasColumnName("RateCard_CostPreference");

                entity.Property(e => e.TalentPreferableWorkingId).HasColumnName("TalentPreferableWorking_ID");

                entity.Property(e => e.TimeZoneTitle).HasMaxLength(50);
            });

            modelBuilder.Entity<PrgTimeZoneCostPreference>(entity =>
            {
                entity.ToTable("prg_TimeZoneCostPreference");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AufullPreference).HasColumnName("AUFullPreference");

                entity.Property(e => e.AupartialPreference).HasColumnName("AUPartialPreference");

                entity.Property(e => e.HrTimeZoneId).HasColumnName("HR_TimeZoneID");

                entity.Property(e => e.Istpreference).HasColumnName("ISTPreference");

                entity.Property(e => e.Ukpreference).HasColumnName("UKPreference");

                entity.Property(e => e.UsfullPreference).HasColumnName("USFullPreference");

                entity.Property(e => e.UspartialPreference).HasColumnName("USPartialPreference");
            });

            modelBuilder.Entity<PrgTimeZonePreference>(entity =>
            {
                entity.ToTable("prg_TimeZonePreference");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.HrTimeZoneId).HasColumnName("HR_TimeZoneID");

                entity.Property(e => e.WorkingTimePreference)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PrgUnicornRolesClientPortal>(entity =>
            {
                entity.ToTable("prg_UnicornRoles_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoleImage).HasMaxLength(200);

                entity.Property(e => e.RoleName).HasMaxLength(200);
            });

            modelBuilder.Entity<PrgUserRoleClientPortal>(entity =>
            {
                entity.ToTable("Prg_UserRole_ClientPortal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Role)
                    .HasMaxLength(100)
                    .HasColumnName("ROLE");

                entity.Property(e => e.RoleDescription)
                    .HasMaxLength(200)
                    .HasColumnName("Role_Description");
            });

            modelBuilder.Entity<PrgUtmCampaign>(entity =>
            {
                entity.ToTable("prg_UTM_Campaign");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UtmCampaign).HasColumnName("UTM_Campaign");
            });

            modelBuilder.Entity<PrgUtmContent>(entity =>
            {
                entity.ToTable("prg_UTM_Content");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UtmContent).HasColumnName("UTM_Content");
            });

            modelBuilder.Entity<PrgUtmMedium>(entity =>
            {
                entity.ToTable("prg_UTM_Medium");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UtmMedium).HasColumnName("UTM_Medium");
            });

            modelBuilder.Entity<PrgUtmPlacement>(entity =>
            {
                entity.ToTable("prg_UTM_Placement");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UtmPlacement).HasColumnName("UTM_Placement");
            });

            modelBuilder.Entity<PrgUtmSource>(entity =>
            {
                entity.ToTable("prg_UTM_Source");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UtmSource).HasColumnName("UTM_Source");
            });

            modelBuilder.Entity<PrgUtmTerm>(entity =>
            {
                entity.ToTable("prg_UTM_Term");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UtmTerm).HasColumnName("UTM_Term");
            });

            modelBuilder.Entity<RestoreChannel>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedDT")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Hrid).HasColumnName("HRID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.UserEmpId)
                    .HasMaxLength(200)
                    .HasColumnName("UserEmpID");
            });

            modelBuilder.Entity<SkillRole1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Skill_Role1");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedBy_Id");

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.SkillId).HasColumnName("Skill_Id");
            });

            modelBuilder.Entity<StoreApiurl>(entity =>
            {
                entity.ToTable("StoreAPIURL");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apiurl).HasColumnName("APIUrl");

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedDT")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FromAts)
                    .HasColumnName("FromATS")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");

                entity.Property(e => e.Hrid).HasColumnName("HRID");
            });

            modelBuilder.Entity<Testtable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TESTTABLE");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<UsrUser>(entity =>
            {
                entity.ToTable("usr_User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnotherUserTypeId).HasColumnName("Another_UserTypeID");

                entity.Property(e => e.BdrMdrName)
                    .HasMaxLength(50)
                    .HasColumnName("BDR_MDR_Name");

                entity.Property(e => e.ContactNumber).HasMaxLength(50);

                entity.Property(e => e.CreatedbyDatetime).HasColumnType("datetime");

                entity.Property(e => e.DeptId).HasColumnName("DeptID");

                entity.Property(e => e.Designation).HasMaxLength(50);

                entity.Property(e => e.EmailId)
                    .HasMaxLength(100)
                    .HasColumnName("EmailID");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(50)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(200);

                entity.Property(e => e.GeoId).HasColumnName("GEO_ID");

                entity.Property(e => e.HubspotOwnerId).HasColumnName("hubspot_owner_id");

                entity.Property(e => e.IsNewUser).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsOdr)
                    .HasColumnName("IsODR")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPasswordChanged).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.LevelId).HasColumnName("LevelID");

                entity.Property(e => e.ModifyByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifyById).HasColumnName("ModifyByID");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.ProfilePic).HasMaxLength(100);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.SkypeId)
                    .HasMaxLength(100)
                    .HasColumnName("SkypeID");

                entity.Property(e => e.TeamId)
                    .HasMaxLength(1000)
                    .HasColumnName("TeamID");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.Property(e => e.Utsguid)
                    .HasMaxLength(100)
                    .HasColumnName("UTSGuid");
            });

            modelBuilder.Entity<UsrUserGeoDetail>(entity =>
            {
                entity.ToTable("usr_UserGeoDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GeoId).HasColumnName("GEO_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<UsrUserHierarchy>(entity =>
            {
                entity.ToTable("usr_UserHierarchy");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<UsrUserPriorityCountLog>(entity =>
            {
                entity.ToTable("usr_UserPriorityCountLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ModifiedByDatetime).HasColumnType("datetime");

                entity.Property(e => e.ModifiedById).HasColumnName("ModifiedByID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<UsrUserRole>(entity =>
            {
                entity.ToTable("usr_UserRole");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GroupRoleId).HasColumnName("GroupRoleID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LeadType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Lead_Type");

                entity.Property(e => e.RoleLevelId).HasColumnName("RoleLevelID");

                entity.Property(e => e.UserRole)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserTeamId).HasColumnName("User_TeamID");
            });

            modelBuilder.Entity<UsrUserRoleDetail>(entity =>
            {
                entity.ToTable("usr_UserRoleDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.UserRoleId).HasColumnName("UserRole_ID");
            });

            modelBuilder.Entity<UsrUserTeam>(entity =>
            {
                entity.ToTable("usr_UserTeam");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserTeam)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("User_Team");
            });

            modelBuilder.Entity<UsrUserType>(entity =>
            {
                entity.ToTable("usr_UserType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserType).HasMaxLength(100);
            });

            modelBuilder.Entity<UsrUserTypeAndRoleDetail>(entity =>
            {
                entity.ToTable("usr_UserTypeAndRoleDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserRoleId).HasColumnName("UserRole_ID");

                entity.Property(e => e.UserTypeId).HasColumnName("UserType_ID");
            });

            modelBuilder.Entity<UsrUserTypeKeyoutcome>(entity =>
            {
                entity.ToTable("usr_UserType_Keyoutcome");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.IsPartofFunnel).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserTypeKeyoutcome)
                    .HasMaxLength(100)
                    .HasColumnName("UserType_Keyoutcome");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
