using System.Linq.Expressions;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Models.ViewModels.Reports;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IReport
    {
        #region Common

        Task<UsrUser> GetUsrUserByCondition(Expression<Func<UsrUser, bool>> expression);
        Task<IEnumerable<UsrUser>> GetALLUsrUserByCondition(Expression<Func<UsrUser, bool>> expression);
        Task<IEnumerable<PrgModeOfWorking>> GetALLModeOFWorkingByCondition(Expression<Func<PrgModeOfWorking, bool>> expression);
        Task<IEnumerable<PrgSummaryStagesForReport>> GetALLPrgSummaryStagesForReports();
        Task<IEnumerable<PrgActionFilter>> GetALLPrgActionFilters();
        IEnumerable<PrgTalentRole> GetALLPrgTalentRolesByCondition(Expression<Func<PrgTalentRole, bool>> expression);
        
        #endregion

        #region Demand Funnel Report (DFR)
        Task<string> DFR_ActionWise_Listing(string paramasString);
        Task<string> DFR_HRWise_Listing(string paramasString);
        Task<string> DFR_ActionWise_Summary(string paramasString);
        Task<string> DFR_HRWise_Summary(string paramasString);
        Task<List<sproc_GetChannelWiseFunnelReportData_For_PopUP_Result>> DFR_HRDetails(string paramasString);
        #endregion

        #region Supply Funnel Report (SFR)
        Task<string> SFR_ActionWise_Listing(string paramasString);        
        Task<string> SFR_HRWise_Listing(string paramasString);
        Task<string> SFR_ActionWise_Summary(string paramasString);
        Task<string> SFR_HRWise_Summary(string paramasString);
        Task<List<sproc_GetSupplyWiseChannelWiseFunnelReportData_For_PopUP_Result>> SFR_HRDetails(string paramasString);
        #endregion

        #region Team Demand Funnel Report (TDF)
        Task<string> TDF_ActionWise_Listing(string paramasString);
        Task<string> TDF_HRWise_Listing(string paramasString);
        Task<string> TDF_ActionWise_Summary_ForSalesUsers(string paramasString);
        Task<string> TDF_HRWise_Summary_ForSalesUsers(string paramasString);
        Task<List<sproc_GetChannelWiseFunnelReportData_For_SalesUser_PopUP_Revised_Result>> TDF_HRDetails(string paramasString);
        #endregion

        #region Client Report
        Task<List<sproc_ClientBasedReport_Result>> GetClientReport(string paramasString);
        Task<List<sproc_ClientBasedReport_PopUp_Result>> GetClientReport_Popup(string paramasString);
        #endregion

        #region Client Log Report
        Task<List<Sproc_GetClientLogReport_Result>> GetClientLogReport(string paramasString);
        Task<List<Sproc_GetClientActivityLog_Result>> GetClientActivityLog(long? ContactID);
        #endregion

        #region Skill wise report
        Task<List<sproc_SkillReport_ActionWise_Result>> sproc_SkillReport_ActionWise(string paramasString);
        Task<List<sproc_SkillReport_HRWise_Result>> sproc_SkillReport_HRWise(string paramasString);

        Task<List<sproc_SkillReport_ActionWise_PopUp_Result>> sproc_SkillReport_ActionWise_PopUp(string paramasString);
        Task<List<sproc_SkillReport_HRWise_PopUp_Result>> sproc_SkillReport_HRWise_PopUp(string paramasString);
        #endregion

        #region SLA Report
        Task<List<Sproc_SLA_OverAll_Summary_Report_Static_Stages_Result>> SLA_Summary(string paramasString);

        Task<List<Sproc_SLA_Report_For_Static_Stages_Result>> SLA_Report(string paramasString);
        #endregion

        #region JDParsingDump Report
        Task<List<sproc_JDParsingDumpReport_Result>> GetJDParsingDumpReport(string paramasString);
        #endregion

        #region HubSpot Funnel Report (DFR)
        Task<string> HFR_ActionWise_Listing(string paramasString);    
        Task<List<Sproc_HubSpot_Client_Funnel_Report_PopUP_Result>> HFR_DealDetails(string paramasString);
        #endregion

        #region Hiring Request Report

        /// <summary>
        /// Hrs the report.
        /// </summary>
        /// <param name="paramasString">The paramas string.</param>
        /// <returns></returns>
        Task<List<sproc_HiringRequest_Report_Result>> HR_Report(string paramasString);
        /// <summary>
        /// Hrs the popup report.
        /// </summary>
        /// <param name="paramasString">The paramas string.</param>
        /// <returns></returns>
        Task<List<sproc_HiringRequest_PopupReport_Result>> HR_PopupReport(string paramasString);

        /// <summary>
        /// Get All the HR status
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PrgHiringRequestStatus>> GetPrgHiringRequestStatus();
        Task<List<Sproc_Get_SalesHead_Users_Result>> Sproc_Get_SalesHead_Users_Result();

        #endregion

        #region ClientBasedReportWithHubSpot
        Task<List<sproc_ClientBasedReport_WithHubSpot_Result>> GetClientBasedReportWithHubSpot(string paramasString);
        Task<List<sproc_ClientBasedReport_WithHubSpot_PopUp_Result>> GetClientBasedReportWithHubSpotPopUp(string paramasString);
        #endregion

        #region HR Lost Report
        Task<List<sproc_GetHRLost_Report_Result>> HRLost_Report(string paramasString);
        Task<List<Sproc_Get_UTS_TrackingLeadDetails_Result>> TrackingLeadDetail(string paramasString);
        Task<List<Sproc_GET_UTM_Tracking_Report_Details_PopUP_Result>> LeadDetailPopUP_List(string paramasString);
        Task<List<sproc_UTS_GetTalentDetailHRLostPopUp_Result>> GetTalentDetail(long? HRID);
        HRLostFilterViewModel GetHRLostFiltersLists();
        Task<TrackingLeadDetailFilterViewModel> GetTrackingLeadFiltersData();
        #endregion

        #region Client portal tracking details
        Task<List<sproc_UTS_ClientList_Result>> ClientList();
        Task<List<Sproc_Get_Email_SubjectList_Result>> Sproc_Get_Email_SubjectList();
        Task<List<Sproc_Get_UTS_ClientPortalTracking_Details_Result>> ClientPortalTracking_Details(string paramasString);
        Task<List<Sproc_Get_UTS_ClientPortalTracking_Details_Popup_Result>> ClientPortalTracking_Details_Popup(string paramasString);
        #endregion

        #region Replacement Report
        Task<List<sproc_Get_Replacement_Report_Result>> sproc_Get_Replacement_Report(string paramasString);
        #endregion

        #region TalentBackout Report
        Task<List<Sproc_Get_TalentBackout_Report_Result>> Sproc_Get_TalentBackout_Report(string paramasString);
        #endregion

        #region AWS SES tracking details
        Task<List<Sproc_Get_AWS_SES_Tracking_Details_Result>> AWSSESTracking_Details(string paramasString);
        Task<List<Sproc_Get_AWS_SES_Tracking_Details_Popup_Result>> AWSSESTracking_Details_Popup(string paramasString);
        #endregion
    }
}
