using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UTSATSAPIDBConnection _dbContext;
        public IUsrUserRepository usrUsers { get; private set; }

        //public ICustomExceptionRepository customExceptions { get; private set; }
        //public IGenAibasedHrtalentMatchMakingRepository genAibasedHrtalentMatchMakings { get; private set; }
        //public IGenAibasedHrtalentMatchMakingDetailRepository genAibasedHrtalentMatchMakingDetails { get; private set; }
        //public IGenAmAssignmentRuleHistoryRepository genAmAssignmentRuleHistorys { get; private set; }
        //public IGenAmnbdAssignmentEmailsendDetailRepository genAmnbdAssignmentEmailsendDetails { get; private set; }
        //public IGenBlogRepository genBlogs { get; private set; }
        //public IGenClientAmAssignmentRepository genClientAmAssignments { get; private set; }
        //public IGenClientAmAssignmentHistoryRepository genClientAmAssignmentHistorys { get; private set; }
        //public IGenClientCalendarEventRepository genClientCalendarEvents { get; private set; }
        public IGenClientHappinessSurveyRepository genClientHappinessSurveys { get; private set; }
        public IGenClientHappinessSurveyOptionRepository genClientHappinessSurveyOptions { get; private set; }
        public IGenClientHappinessSurveyQuestionRepository genClientHappinessSurveyQuestions { get; private set; }
        //public IGenClientPayOutAdditionalLineItemRepository genClientPayOutAdditionalLineItems { get; private set; }
        public IGenCompanyRepository genCompanys { get; private set; }
        public IGenCompanyContractDetailRepository genCompanyContractDetails { get; private set; }
        public IGenCompanyLeadTypeUserDetailRepository genCompanyLeadTypeUserDetails { get; private set; }
        //public IGenCompanyLegalInfoRepository genCompanyLegalInfos { get; private set; }
        //public IGenCompanyUpdateRepository genCompanyUpdates { get; private set; }
        //public IGenCompanyWeightedAverageCriterionRepository genCompanyWeightedAverageCriterions { get; private set; }
        public IGenContactRepository genContacts { get; private set; }
        //public IGenContactGetHelpQuestionRepository genContactGetHelpQuestions { get; private set; }
        //public IGenContactHistoryRepository genContactHistorys { get; private set; }
        //public IGenContactInterviewFeedbackRepository genContactInterviewFeedbacks { get; private set; }
        //public IGenContactNotificationHistoryRepository genContactNotificationHistorys { get; private set; }
        //public IGenContactPendingActionRepository genContactPendingActions { get; private set; }
        public IGenContactPointofContactRepository genContactPointofContacts { get; private set; }
        public IGenContactTalentPriorityRepository genContactTalentPrioritys { get; private set; }
        public IGenDealRepository genDeals { get; private set; }
        //public IGenDealContactRepository genDealContacts { get; private set; }
        //public IGenDealTicketRepository genDealTickets { get; private set; }
        //public IGenDirectPlacementRepository genDirectPlacements { get; private set; }
        //public IGenEsalesAmApiResponseRepository genEsalesAmApiResponses { get; private set; }
        //public IGenFrontendClientRepository genFrontendClients { get; private set; }
        //public IGenHistoryRepository genHistorys { get; private set; }
        //public IGenHrnoteRepository genHrnotes { get; private set; }
        //public IGenHrnotesTagUserDetailRepository genHrnotesTagUserDetails { get; private set; }
        //public IGenHubSpotCompanyRepository genHubSpotCompanys { get; private set; }
        //public IGenHubSpotContactRepository genHubSpotContacts { get; private set; }
        //public IGenIncAmNrUserRoleDetailRepository genIncAmNrUserRoleDetails { get; private set; }
        //public IGenIncAmTalentDeployUserRoleDetailRepository genIncAmTalentDeployUserRoleDetails { get; private set; }
        //public IGenIncBaseSalUserRoleDetailRepository genIncBaseSalUserRoleDetails { get; private set; }
        //public IGenIncContractTalentUserRoleDetailRepository genIncContractTalentUserRoleDetails { get; private set; }
        //public IGenIncContractUserRoleDetailRepository genIncContractUserRoleDetails { get; private set; }
        //public IGenIncPlacementFeesTalentUserRoleDetailRepository genIncPlacementFeesTalentUserRoleDetails { get; private set; }
        //public IGenIncSalesIncentiveTargetDetailRepository genIncSalesIncentiveTargetDetails { get; private set; }
        //public IGenIncSalesIncentiveTargetSummaryRepository genIncSalesIncentiveTargetSummarys { get; private set; }
        //public IGenIncSalesIncentiveTargetSummaryHistoryRepository genIncSalesIncentiveTargetSummaryHistorys { get; private set; }
        //public IGenIncTargetAcheivedDetailRepository genIncTargetAcheivedDetails { get; private set; }
        //public IGenIncUserTargetRepository genIncUserTargets { get; private set; }
        //public IGenIncUserTargetsHistoryRepository genIncUserTargetsHistorys { get; private set; }
        //public IGenInterviewSlotsMasterRepository genInterviewSlotsMasters { get; private set; }
        //public IGenManagedTalentRepository genManagedTalents { get; private set; }
        //public IGenManagedTalentHistoryRepository genManagedTalentHistorys { get; private set; }
        //public IGenMenuMasterRepository genMenuMasters { get; private set; }
        //public IGenMenuRightRepository genMenuRights { get; private set; }
        //public IGenOnBoardClientContractDetailRepository genOnBoardClientContractDetails { get; private set; }
        //public IGenOnBoardClientDevicePolicyDetailRepository genOnBoardClientDevicePolicyDetails { get; private set; }
        //public IGenOnBoardClientInvoiceSummaryRepository genOnBoardClientInvoiceSummarys { get; private set; }
        //public IGenOnBoardClientLeavePolicyRepository genOnBoardClientLeavePolicys { get; private set; }
        //public IGenOnBoardClientTeamRepository genOnBoardClientTeams { get; private set; }
        public IGenOnBoardTalentRepository genOnBoardTalents { get; private set; }
        //public IGenOnBoardTalentsClientFeedbackRepository genOnBoardTalentsClientFeedbacks { get; private set; }
        //public IGenOnBoardTalentsLegalDetailRepository genOnBoardTalentsLegalDetails { get; private set; }
        public IGenOnBoardTalentsReplacementDetailRepository genOnBoardTalentsReplacementDetails { get; private set; }
        //public IGenOnlyDealIdRepository genOnlyDealIds {get; private set; }
        //public IGenPayoutInformationRepository genPayoutInformations { get; private set; }
        //public IGenPmsTalentRedirectUrlDetailRepository genPmsTalentRedirectUrlDetails { get; private set; }
        //public IGenPostalCodeswithDetailRepository genPostalCodeswithDetails {get; private set; }
        public IGenSalesHiringRequestRepository genSalesHiringRequests { get; private set; }
        //public IGenSalesHiringRequestDetailRepository genSalesHiringRequestDetails {get; private set; }
        //public IGenSalesHiringRequestDetailsHistoryRepository genSalesHiringRequestDetailsHistorys {get; private set; }
        //public IGenSalesHiringRequestHistoryRepository genSalesHiringRequestHistorys {get; private set; }
        //public IGenSalesHiringRequestInterviewerDetailRepository genSalesHiringRequestInterviewerDetails {get; private set; }
        //public IGenSalesHiringRequestPriorityRepository genSalesHiringRequestPrioritys {get; private set; }
        //public IGenSalesHiringRequestPriorityHistoryRepository genSalesHiringRequestPriorityHistorys {get; private set; }
        public IGenSalesHiringRequestSkillDetailRepository genSalesHiringRequestSkillDetails {get; private set; }
        //public IGenSalesHrJddumpRepository genSalesHrJddumps {get; private set; }
        //public IGenSalesHrJddumpSkillDetailRepository genSalesHrJddumpSkillDetails {get; private set; }
        //public IGenSalesHrTracceptedDetailRepository genSalesHrTracceptedDetails {get; private set; }
        //public IGenShortlistedTalentRepository genShortlistedTalents {get; private set; }
        //public IGenShortlistedTalentInterviewDetailRepository genShortlistedTalentInterviewDetails {get; private set; }
        //public IGenSocialMediaLoginDetailRepository genSocialMediaLoginDetails {get; private set; }
        //public IGenSystemConfigurationRepository genSystemConfigurations {get; private set; }
        public IGenTalentRepository genTalents { get; private set; }
        //public IGenTalentApplicationToolRepository genTalentApplicationTools { get; private set; }
        //public IGenTalentAssesmentScoreRepository genTalentAssesmentScores { get; private set; }
        //public IGenTalentAssesmentSkillScoreRepository genTalentAssesmentSkillScores { get; private set; }
        //public IGenTalentCertificationsDetailRepository genTalentCertificationsDetails { get; private set; }
        //public IGenTalentClientTeleRepository genTalentClientTeles { get; private set; }
        //public IGenTalentComfortableAreaDetailRepository genTalentComfortableAreaDetails { get; private set; }
        //public IGenTalentComfortableWorkingDetailRepository genTalentComfortableWorkingDetails { get; private set; }
        //public IGenTalentEducationDetailRepository genTalentEducationDetails { get; private set; }
        //public IGenTalentFinancialDetailRepository genTalentFinancialDetails { get; private set; }
        //public IGenTalentGetHelpQuestionRepository genTalentGetHelpQuestions { get; private set; }
        //public IGenTalentHistoryRepository genTalentHistorys { get; private set; }
        //public IGenTalentInterestDetailRepository genTalentInterestDetails { get; private set; }
        //public IGenTalentInterviewFeedbackRepository genTalentInterviewFeedbacks { get; private set; }
        //public IGenTalentKeyQualityDetailRepository genTalentKeyQualityDetails { get; private set; }
        //public IGenTalentLegalInfoRepository genTalentLegalInfos { get; private set; }
        //public IGenTalentNotificationHistoryRepository genTalentNotificationHistorys { get; private set; }
        //public IGenTalentPayOutAdditionalLineItemRepository genTalentPayOutAdditionalLineItems { get; private set; }
        //public IGenTalentPendingActionRepository genTalentPendingActions { get; private set; }
        //public IGenTalentPointofContactRepository genTalentPointofContacts { get; private set; }
        //public IGenTalentPreferableWorkingDetailRepository genTalentPreferableWorkingDetails { get; private set; }
        //public IGenTalentPrimarySkillDetailRepository genTalentPrimarySkillDetails { get; private set; }
        //public IGenTalentProfessionalExperienceRepository genTalentProfessionalExperiences { get; private set; }
        //public IGenTalentProjectDetailRepository genTalentProjectDetails { get; private set; }
        //public IGenTalentResumeDetailRepository genTalentResumeDetails { get; private set; }
        //public IGenTalentSecondarySkillDetailRepository genTalentSecondarySkillDetails { get; private set; }
        //public IGenTalentSelectedInterviewDetailRepository genTalentSelectedInterviewDetails { get; private set; }
        //public IGenTalentSelectedInterviewerDetailRepository genTalentSelectedInterviewerDetails { get; private set; }
        //public IGenTalentSelectedNextRoundInterviewDetailRepository genTalentSelectedNextRoundInterviewDetails { get; private set; }
        //public IGenTalentSelectedPostAcceptanceDetailRepository genTalentSelectedPostAcceptanceDetails { get; private set; }
        //public IGenTalentSelectedPostAcceptanceNotMatchDetailRepository genTalentSelectedPostAcceptanceNotMatchDetails { get; private set; }
        //public IGenTalentSelectionHistoryRepository genTalentSelectionHistorys { get; private set; }
        //public IGenTalentTestimonialOrFeedbackRepository genTalentTestimonialOrFeedbacks { get; private set; }
        public IGenTeamRepository genTeams { get; private set; }
        //public IGenTeamDistributionRepository genTeamDistributions { get; private set; }
        //public IGenTestimonialRepository genTestimonials { get; private set; }
        //public IGenTicketRepository genTickets { get; private set; }
        //public IGenUserHistoryRepository genUserHistorys { get; private set; }
        //public IGenUtsAssignmentHistoryRepository genUtsAssignmentHistorys { get; private set; }
        //public IGenUtsAtsApiRecordRepository genUtsAtsApiRecords { get; private set; }
        //public IGenUtsOnBoardLineItemRepository genUtsOnBoardLineItems { get; private set; }
        //public IHubSpotEventTypeRepository hubSpotEventTypes { get; private set; }
        //public IHubspotWebhookNotificationRepository hubspotWebhookNotifications { get; private set; }
        public IPrgActionFilterRepository prgActionFilters { get; private set; }
        //public IPrgAmAssignmentRuleRepository prgAmAssignmentRules { get; private set; }
        //public IPrgAmdealContactRepository prgAmdealContacts { get; private set; }
        //public IPrgApitimeZoneResponseRepository prgApitimeZoneResponses { get; private set; }
        //public IPrgBenchMarkRepository prgBenchMarks { get; private set; }
        //public IPrgClientEngagementModelRepository prgClientEngagementModels { get; private set; }
        public IPrgClientHappinessSurveyFeedbackOptionRepository prgClientHappinessSurveyFeedbackOptions { get; private set; }
        public IPrgClientHappinessSurveyFeedbackQuestionRepository prgClientHappinessSurveyFeedbackQuestions { get; private set; }
        //public IPrgClientInvoiceStatusRepository prgClientInvoiceStatuss { get; private set; }
        //public IPrgClientLegalDocumentTypeRepository prgClientLegalDocumentTypes { get; private set; }
        //public IPrgClientStageRepository prgClientStages { get; private set; }
        //public IPrgCompanyCurrentOpenPositionRepository prgCompanyCurrentOpenPositions { get; private set; }
        //public IPrgCompanyDomainRepository prgCompanyDomains { get; private set; }
        //public IPrgCompanySizeRepository prgCompanySizes { get; private set; }
        //public IPrgCompanyTargetGroupRepository prgCompanyTargetGroups { get; private set; }
        //public IPrgCompanyTypeofInterviewerRepository prgCompanyTypeofInterviewers { get; private set; }
        //public IPrgContactActionRepository prgContactActions { get; private set; }
        //public IPrgContactStatusRepository prgContactStatuss { get; private set; }
        //public IPrgContactTimeZoneRepository prgContactTimeZones { get; private set; }
        public IPrgCountryRegionRepository prgCountryRegions { get; private set; }
        //public IPrgCrmApiDetailRepository prgCrmApiDetails { get; private set; }
        //public IPrgCrmApiPartnerRepository prgCrmApiPartners { get; private set; }
        //public IPrgCrmApiTestsResponseRepository prgCrmApiTestsResponses { get; private set; }
        //public IPrgCurrencyExchangeRateRepository prgCurrencyExchangeRates { get; private set; }
        //public IPrgDealPipelineRepository prgDealPipelines { get; private set; }
        public IPrgDealStageRepository prgDealStages { get; private set; }
        //public IPrgDealStatusRepository prgDealStatuss { get; private set; }
        //public IPrgDealStatusIdpipelineStageIdRepository prgDealStatusIdpipelineStageIds { get; private set; }
        public IPrgDealTypeRepository prgDealTypes { get; private set; }
        public IPrgDepartmentRepository prgDepartments { get; private set; }
        //public IPrgEmploymentStatusRepository prgEmploymentStatuss { get; private set; }
        //public IPrgExtractedSkillsforJdparsingRepository prgExtractedSkillsforJdparsings { get; private set; }
        public IPrgGeoRepository prgGeos { get; private set; }
        //public IPrgHiringRequestRoleStatusRepository prgHiringRequestRoleStatuss { get; private set; }
        public IPrgHiringRequestStatusRepository prgHiringRequestStatuss { get; private set; }
        //public IPrgHistoryActionRepository prgHistoryActions { get;private set; }
        //public IPrgHowSoonClientWantTalentRepository prgHowSoonClientWantTalents { get;private set; }
        //public IPrgHrdeleteReasonRepository prgHrdeleteReasons { get; private set;}
        //public IPrgIncAmNrSlabRepository prgIncAmNrSlabs { get;private set;}
        //public IPrgIncAmSlabRepository prgIncAmSlabs { get;private set; }
        //public IPrgIncContractsSlabRepository prgIncContractsSlabs { get; private set;}
        //public IPrgIncPlacementFeesSlabRepository prgIncPlacementFeesSlabs { get;private set; }
        //public IPrgIncSalgoalSlabRepository prgIncSalgoalSlabs { get; private set;}
        //public IPrgInterviewStatusRepository prgInterviewStatuss { get; private set;}
        //public IPrgMatchMakingFactorRepository prgMatchMakingFactors { get;private set; }
        //public IPrgMatchMakingSubFactorRepository prgMatchMakingSubFactors { get;private set; }
        public IPrgModeOfWorkingRepository prgModeOfWorkings { get; private set; }
        //public IPrgOnBoardLegalKickOffStatusRepository prgOnBoardLegalKickOffStatuss { get;private set; }
        //public IPrgOnBoardPolicyDeviceMasterRepository prgOnBoardPolicyDeviceMasters { get; private set;}
        //public IPrgOrgLevelRepository prgOrgLevels { get;private set; }
        //public IPrgPartialEngagementTypeRepository prgPartialEngagementTypes { get; private set;}
        public IPrgPipelineRepository prgPipelines { get; private set; }
        //public IPrgPreOnBoardStatusRepository prgPreOnBoardStatuss { get; private set;}
        //public IPrgRateCardRepository prgRateCards { get; private set;}
        //public IPrgRequirementRepository prgRequirements { get;private set; }
        //public IPrgRolesAndResponsilbityRepository prgRolesAndResponsilbitys { get; private set;}
        public IPrgSkillRepository prgSkills { get; private set;}
        //public IPrgSkillCategoryRepository prgSkillCategorys { get; private set;}
        //public IPrgSkillRankRepository prgSkillRanks { get; private set;}
        //public IPrgSkillSourceRepository prgSkillSources { get; private set;}
        //public IPrgSkillsInAssesmentRepository prgSkillsInAssesments { get; private set;}
        //public IPrgSkillsRoleRepository prgSkillsRoles { get; private set;}
        //public IPrgSlatimeRepository prgSlatimes { get; private set;}
        //public IPrgSocialProviderRepository prgSocialProviders { get;private set; }
        public IPrgSummaryStagesForReportRepository prgSummaryStagesForReports { get; private set; }
        //public IPrgTalentAccountStatusRepository prgTalentAccountStatuss { get; private set;}
        //public IPrgTalentActionRepository prgTalentActions { get; private set;}
        //public IPrgTalentApplicationToolRepository prgTalentApplicationTools { get; private set;}
        //public IPrgTalentAssociatedWithUplerRepository prgTalentAssociatedWithUplers { get; private set;}
        //public IPrgTalentCategoryRepository prgTalentCategorys { get; private set;}
        //public IPrgTalentCertificationRepository prgTalentCertifications { get; private set;}
        //public IPrgTalentComfortableAreaRepository prgTalentComfortableAreas { get; private set;}
        //public IPrgTalentComfortableWorkingRepository prgTalentComfortableWorkings { get; private set;}
        //public IPrgTalentCommunicationSkillRepository prgTalentCommunicationSkills { get; private set;}
        //public IPrgTalentInterestRepository prgTalentInterests { get; private set;}
        //public IPrgTalentJoinningRepository prgTalentJoinnings { get; private set;}
        //public IPrgTalentKeyQualityRepository prgTalentKeyQualitys { get; private set;}
        //public IPrgTalentLegalDocumentTypeRepository prgTalentLegalDocumentTypes { get; private set;}
        //public IPrgTalentPayoutStatusRepository prgTalentPayoutStatuss { get;private set; }
        //public IPrgTalentPreferableWorkingRepository prgTalentPreferableWorkings { get; private set;}
        //public IPrgTalentPrimarySkillRepository prgTalentPrimarySkills { get; private set;}
        //public IPrgTalentRejectReasonRepository prgTalentRejectReasons { get; private set;}
        public IPrgTalentRoleRepository prgTalentRoles { get; private set; }
        //public IPrgTalentRoleCategoryRepository prgTalentRoleCategorys { get; private set;}
        //public IPrgTalentRoleTemplateRepository prgTalentRoleTemplates { get; private set;}
        //public IPrgTalentSkillBenchMarkRepository prgTalentSkillBenchMarks { get; private set;}
        //public IPrgTalentStatusRepository prgTalentStatuss { get; private set;}
        //public IPrgTalentStatusAfterClientSelectionRepository prgTalentStatusAfterClientSelections { get; private set;}
        //public IPrgTalentTimeZoneRepository prgTalentTimeZones { get; private set;}
        //public IPrgTalentTypeRepository prgTalentTypes { get;private set; }
        //public IPrgTempApplicationToolRepository prgTempApplicationTools { get; private set;}
        //public IPrgTempApplicationToolsMergePrgTalentApplicationToolRepository prgTempApplicationToolsMergePrgTalentApplicationTools { get; private set;}
        //public IPrgTempCertificationRepository prgTempCertifications { get; private set;}
        //public IPrgTempCertificationsMergePrgTalentCertificationRepository prgTempCertificationsMergePrgTalentCertifications { get;private set; }
        //public IPrgTempInterestRepository prgTempInterests { get; private set;}
        //public IPrgTempInterestMergePrgTalentInterestRepository prgTempInterestMergePrgTalentInterests { get; private set;}
        public IPrgTempSkillRepository prgTempSkills { get; private set;}
        //public IPrgTempSkillsMergePrgSkillRepository prgTempSkillsMergePrgSkills { get;private set; }
        //public IPrgTicketCategoryRepository prgTicketCategorys { get; private set;}
        //public IPrgTicketStatusRepository prgTicketStatuss { get;private set; }
        //public IPrgTicketTechnicalJustificationRepository prgTicketTechnicalJustifications { get;private set; }
        //public IPrgTimeZoneRepository prgTimeZones { get; private set;}
        //public IPrgTimeZoneCostPreferenceRepository prgTimeZoneCostPreferences { get; private set;}
        //public IPrgTimeZonePreferenceRepository prgTimeZonePreferences { get;private set; }
        //public ISkillRole1Repository skillRole1s { get;private set; }
        //public IUsrUserGeoDetailRepository usrUserGeoDetails { get;private set; }
        public IUsrUserHierarchyRepository usrUserHierarchys { get;private set;}
        //public IUsrUserPriorityCountLogRepository usrUserPriorityCountLogs { get;private set; }
        public IUsrUserRoleRepository usrUserRoles { get; private set; }
        public IUsrUserRoleDetailRepository usrUserRoleDetails { get; private set; }
        //public IUsrUserTeamRepository usrUserTeams { get;private set; }
        public IUsrUserTypeRepository usrUserTypes { get; private set; }
        public IUsrUserTypeAndRoleDetailRepository usrUserTypeAndRoleDetails { get; private set; }
        public IStoreAPIUrlRepository storeAPIUrl { get; private set; }

        public UnitOfWork(UTSATSAPIDBConnection dbContext)
        {
            _dbContext = dbContext;
            usrUsers = new UsrUserRepository(_dbContext);
            prgTalentRoles = new PrgTalentRoleRepository(_dbContext);
            genCompanys = new GenCompanyRepository(_dbContext);
            genContacts = new GenContactRepository(_dbContext);
            genTalents = new GenTalentRepository(_dbContext);
            usrUserTypes = new UsrUserTypeRepository(_dbContext);
            prgGeos = new PrgGeoRepository(_dbContext);
            usrUserRoleDetails = new UsrUserRoleDetailRepository(_dbContext);
            usrUserRoles = new UsrUserRoleRepository(_dbContext);
            prgPipelines = new PrgPipelineRepository(_dbContext);
            genDeals = new GenDealRepository(_dbContext);
            prgDealStages = new PrgDealStageRepository(_dbContext);
            prgDealTypes = new PrgDealTypeRepository(_dbContext);
            genCompanyContractDetails = new GenCompanyContractDetailRepository(_dbContext);
            genCompanyLeadTypeUserDetails = new GenCompanyLeadTypeUserDetailRepository(_dbContext);
            genClientHappinessSurveys = new GenClientHappinessSurveyRepository(_dbContext);
            prgClientHappinessSurveyFeedbackOptions = new PrgClientHappinessSurveyFeedbackOptionRepository(_dbContext);
            prgClientHappinessSurveyFeedbackQuestions = new PrgClientHappinessSurveyFeedbackQuestionRepository(_dbContext);
            genClientHappinessSurveyOptions = new GenClientHappinessSurveyOptionRepository(_dbContext);
            genClientHappinessSurveyQuestions = new GenClientHappinessSurveyQuestionRepository(_dbContext);
            genContactPointofContacts = new GenContactPointofContactRepository(_dbContext);
            genSalesHiringRequests = new GenSalesHiringRequestRepository(_dbContext);
            prgHiringRequestStatuss = new PrgHiringRequestStatusRepository(_dbContext);
            genContactTalentPrioritys = new GenContactTalentPriorityRepository(_dbContext);
            genOnBoardTalentsReplacementDetails = new GenOnBoardTalentsReplacementDetailRepository(_dbContext);
            genOnBoardTalents = new GenOnBoardTalentRepository(_dbContext);
            genTeams = new GenTeamRepository(_dbContext);
            prgModeOfWorkings = new PrgModeOfWorkingRepository(_dbContext);
            prgActionFilters = new PrgActionFilterRepository(_dbContext);
            prgSummaryStagesForReports = new PrgSummaryStagesForReportRepository(_dbContext);
            prgDepartments = new PrgDepartmentRepository(_dbContext);
            usrUserTypeAndRoleDetails = new UsrUserTypeAndRoleDetailRepository(_dbContext);
            storeAPIUrl = new StoreAPIUrlRepository(_dbContext);
            usrUserHierarchys = new UsrUserHierarchyRepository(_dbContext);
            prgCountryRegions = new PrgCountryRegionRepository(_dbContext);
            genSalesHiringRequestSkillDetails = new GenSalesHiringRequestSkillDetailRepository(_dbContext);
            prgSkills = new PrgSkillRepository(_dbContext);
            prgTempSkills = new PrgTempSkillRepository(_dbContext);
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

    }
}
