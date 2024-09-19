using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Models.ViewModels.Reports;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class ReportsRepository : ControllerBase, IReport
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public ReportsRepository(TalentConnectAdminDBContext _db, IUnitOfWork unitOfWork)
        {
            db = _db;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Public Methods

        #region Commom 

        public async Task<UsrUser> GetUsrUserByCondition(Expression<Func<UsrUser, bool>> expression)
        {
            return await _unitOfWork.usrUsers.GetSingleByCondition(expression);
        }
        public async Task<IEnumerable<UsrUser>> GetALLUsrUserByCondition(Expression<Func<UsrUser, bool>> expression)
        {
            return await _unitOfWork.usrUsers.GetAllByCondition(expression);
        }
        public async Task<IEnumerable<PrgModeOfWorking>> GetALLModeOFWorkingByCondition(Expression<Func<PrgModeOfWorking, bool>> expression)
        {
            return await _unitOfWork.prgModeOfWorkings.GetAllByCondition(expression);
        }
        public async Task<IEnumerable<PrgSummaryStagesForReport>> GetALLPrgSummaryStagesForReports()
        {
            return await _unitOfWork.prgSummaryStagesForReports.GetAll();
        }
        public async Task<IEnumerable<PrgActionFilter>> GetALLPrgActionFilters()
        {
            return await _unitOfWork.prgActionFilters.GetAll();
        }
        public IEnumerable<PrgTalentRole> GetALLPrgTalentRolesByCondition(Expression<Func<PrgTalentRole, bool>> expression)
        {
            return _unitOfWork.prgTalentRoles.GetAllByCondition(expression).Result.OrderBy(x => x.TalentRole);
        }

        #endregion

        #region Demand Funnel Report
        public async Task<string> DFR_ActionWise_Listing(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetChannelWise_ActionReport, strParams);
            return await ConvertToJsonAsync(query, true);
        }
        public async Task<string> DFR_HRWise_Listing(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetChannelWiseFunnelReportData, strParams);
            return await ConvertToJsonAsync(query, true);
        }
        public async Task<string> DFR_HRWise_Summary(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetChannelWiseFunnelReportData_For_Summary, strParams);
            return await ConvertToJsonAsync(query, true);
        }
        public async Task<string> DFR_ActionWise_Summary(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetChannelWiseFunnelReportData_For_Summary_Revised, strParams);
            return await ConvertToJsonAsync(query, true);
        }
        public async Task<List<sproc_GetChannelWiseFunnelReportData_For_PopUP_Result>> DFR_HRDetails(string paramasString)
        {
            return db.Set<sproc_GetChannelWiseFunnelReportData_For_PopUP_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_GetChannelWiseFunnelReportData_For_PopUP, paramasString)).ToList();
        }
        #endregion

        #region Supply Funnel Report
        public async Task<string> SFR_ActionWise_Listing(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetSupplyChannelWise_ActionReport, strParams);
            return await ConvertToJsonAsync(query);
        }
        public async Task<string> SFR_HRWise_Listing(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetSupplyChannelWiseFunnelReportData, strParams);
            return await ConvertToJsonAsync(query);
        }
        public async Task<string> SFR_HRWise_Summary(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetSupplyChannelWiseFunnelReportData_For_Summary, strParams);
            return await ConvertToJsonAsync(query);
        }
        public async Task<string> SFR_ActionWise_Summary(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetSupplyChannelWise_ActionReport_For_Summary, strParams);
            return await ConvertToJsonAsync(query);
        }
        public async Task<List<sproc_GetSupplyWiseChannelWiseFunnelReportData_For_PopUP_Result>> SFR_HRDetails(string paramasString)
        {
            return db.Set<sproc_GetSupplyWiseChannelWiseFunnelReportData_For_PopUP_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_GetSupplyWiseChannelWiseFunnelReportData_For_PopUP, paramasString)).ToList();
        }
        #endregion

        #region Team Demand Funnel Report
        public async Task<string> TDF_ActionWise_Listing(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetChannelWise_ActionReport_ForSalesUsers_Revised, strParams);
            return await ConvertToJsonAsync(query);
        }
        public async Task<string> TDF_HRWise_Listing(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetChannelWiseFunnelReportData_ForSalesUsers, strParams);
            return await ConvertToJsonAsync(query);
        }
        public async Task<string> TDF_ActionWise_Summary_ForSalesUsers(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetChannelWise_ActionReport_For_Summary_ForSalesUsers_Revised, strParams);
            return await ConvertToJsonAsync(query);
        }
        public async Task<string> TDF_HRWise_Summary_ForSalesUsers(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.sproc_GetChannelWiseFunnelReportData_For_Summary_ForSalesUsers, strParams);
            return await ConvertToJsonAsync(query);
        }
        public async Task<List<sproc_GetChannelWiseFunnelReportData_For_SalesUser_PopUP_Revised_Result>> TDF_HRDetails(string paramasString)
        {
            return db.Set<sproc_GetChannelWiseFunnelReportData_For_SalesUser_PopUP_Revised_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_GetChannelWiseFunnelReportData_For_SalesUser_PopUP_Revised, paramasString)).ToList();
        }
        #endregion

        #region Client Report
        public async Task<List<sproc_ClientBasedReport_Result>> GetClientReport(string paramasString)
        {
            return db.Set<sproc_ClientBasedReport_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_ClientBasedReport, paramasString)).ToList();
        }

        public async Task<List<sproc_ClientBasedReport_PopUp_Result>> GetClientReport_Popup(string paramasString)
        {
            return db.Set<sproc_ClientBasedReport_PopUp_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_ClientBasedReport_PopUp, paramasString)).ToList();
        }
        #endregion

        #region Client Log Report
        public async Task<List<Sproc_GetClientLogReport_Result>> GetClientLogReport(string paramasString)
        {
            return db.Set<Sproc_GetClientLogReport_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GetClientLogReport, paramasString)).ToList();
        }

        public async Task<List<Sproc_GetClientActivityLog_Result>> GetClientActivityLog(long? ContactID)
        {
            return db.Set<Sproc_GetClientActivityLog_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GetClientActivityLog, ContactID)).ToList();
        }
        #endregion

        #region Skill wise report
        public async Task<List<sproc_SkillReport_ActionWise_Result>> sproc_SkillReport_ActionWise(string paramasString)
        {
            return db.Set<sproc_SkillReport_ActionWise_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_SkillReport_ActionWise, paramasString)).ToList();
        }
        public async Task<List<sproc_SkillReport_HRWise_Result>> sproc_SkillReport_HRWise(string paramasString)
        {
            return db.Set<sproc_SkillReport_HRWise_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_SkillReport_HRWise, paramasString)).ToList();
        }

        public async Task<List<sproc_SkillReport_ActionWise_PopUp_Result>> sproc_SkillReport_ActionWise_PopUp(string paramasString)
        {
            return db.Set<sproc_SkillReport_ActionWise_PopUp_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_SkillReport_ActionWise_PopUp, paramasString)).ToList();
        }
        public async Task<List<sproc_SkillReport_HRWise_PopUp_Result>> sproc_SkillReport_HRWise_PopUp(string paramasString)
        {
            return db.Set<sproc_SkillReport_HRWise_PopUp_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_SkillReport_HRWise_PopUp, paramasString)).ToList();
        }
        #endregion

        #region SLA Report

        public async Task<List<Sproc_SLA_OverAll_Summary_Report_Static_Stages_Result>> SLA_Summary(string paramasString)
        {
            return db.Set<Sproc_SLA_OverAll_Summary_Report_Static_Stages_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_SLA_OverAll_Summary_Report_Static_Stages, paramasString)).ToList();
        }

        public async Task<List<Sproc_SLA_Report_For_Static_Stages_Result>> SLA_Report(string paramasString)
        {
            return db.Set<Sproc_SLA_Report_For_Static_Stages_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_SLA_Report_For_Static_Stages, paramasString)).ToList();
        }
        public async Task<List<sproc_JDParsingDumpReport_Result>> GetJDParsingDumpReport(string paramasString)
        {
            return db.Set<sproc_JDParsingDumpReport_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_JDParsingDumpReport, paramasString)).ToList();
        }

        #endregion

        #region
        public async Task<string> HFR_ActionWise_Listing(string strParams)
        {
            string query = string.Format("EXEC {0} {1}", Constants.ProcConstant.Sproc_HubSpot_Client_Funnel_Report, strParams);
            return await ConvertToJsonAsync(query, true);
        }

        public async Task<List<Sproc_HubSpot_Client_Funnel_Report_PopUP_Result>> HFR_DealDetails(string paramasString)
        {
            return db.Set<Sproc_HubSpot_Client_Funnel_Report_PopUP_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_HubSpot_Client_Funnel_Report_PopUP, paramasString)).ToList();
        }
        #endregion

        #region Hiring Request Report

        /// <summary>
        /// Hrs the report.
        /// </summary>
        /// <param name="paramasString">The paramas string.</param>
        /// <returns></returns>
        public async Task<List<sproc_HiringRequest_Report_Result>> HR_Report(string paramasString)
        {
            return db.Set<sproc_HiringRequest_Report_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_HiringRequest_Report, paramasString)).ToList();
        }

        /// <summary>
        /// Hrs the popup report.
        /// </summary>
        /// <param name="paramasString">The paramas string.</param>
        /// <returns></returns>
        public async Task<List<sproc_HiringRequest_PopupReport_Result>> HR_PopupReport(string paramasString)
        {
            return db.Set<sproc_HiringRequest_PopupReport_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_HiringRequest_Report_PopUp, paramasString)).ToList();
        }

        /// <summary>
        /// Get All the HR status
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PrgHiringRequestStatus>> GetPrgHiringRequestStatus()
        {
            return await _unitOfWork.prgHiringRequestStatuss.GetAll();
        }
        public async Task<List<Sproc_Get_SalesHead_Users_Result>> Sproc_Get_SalesHead_Users_Result()
        {
            return await db.Set<Sproc_Get_SalesHead_Users_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Get_SalesHead_Users)).ToListAsync().ConfigureAwait(false);
        }
        #endregion

        #region ClientBasedReportWithHubSpot
        public async Task<List<sproc_ClientBasedReport_WithHubSpot_Result>> GetClientBasedReportWithHubSpot(string paramasString)
        {
            return await db.Set<sproc_ClientBasedReport_WithHubSpot_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_ClientBasedReport_WithHubSpot, paramasString)).ToListAsync();
        }
        public async Task<List<sproc_ClientBasedReport_WithHubSpot_PopUp_Result>> GetClientBasedReportWithHubSpotPopUp(string paramasString)
        {
            return await db.Set<sproc_ClientBasedReport_WithHubSpot_PopUp_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_ClientBasedReport_WithHubSpot_PopUp, paramasString)).ToListAsync();
        }

        #endregion

        #region HR Lost Report
        public async Task<List<sproc_GetHRLost_Report_Result>> HRLost_Report(string paramasString)
        {
            return db.Set<sproc_GetHRLost_Report_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_GetHRLost_Report, paramasString)).ToList();
        }

        public async Task<List<Sproc_Get_UTS_TrackingLeadDetails_Result>> TrackingLeadDetail(string paramasString)
        {
            return db.Set<Sproc_Get_UTS_TrackingLeadDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_UTS_TrackingLeadDetails, paramasString)).ToList();
        }
        public async Task<List<Sproc_GET_UTM_Tracking_Report_Details_PopUP_Result>> LeadDetailPopUP_List(string paramasString)
        {
            return db.Set<Sproc_GET_UTM_Tracking_Report_Details_PopUP_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GET_UTM_Tracking_Report_Details_PopUP, paramasString)).ToList();

        }
        public async Task<List<sproc_UTS_GetTalentDetailHRLostPopUp_Result>> GetTalentDetail(long? HRID)
        {
            return db.Set<sproc_UTS_GetTalentDetailHRLostPopUp_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetTalentDetailHRLostPopUp, HRID)).ToList();
        }

        public HRLostFilterViewModel GetHRLostFiltersLists()
        {
            HRLostFilterViewModel model = new()
            {

                SalesUser = db.UsrUsers.Where(x => x.IsActive == true && x.UserTypeId == 4 || x.UserTypeId == 9).Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.FullName
                }).OrderBy(x => x.Value).ToList()
            };

            return model;
        }
        public async Task<TrackingLeadDetailFilterViewModel> GetTrackingLeadFiltersData()
        {
            var resultData = db.Set<Sproc_Get_JobPostCount_For_UTM_Tracking_Lead_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Get_JobPostCount_For_UTM_Tracking_Lead)).ToList();


            TrackingLeadDetailFilterViewModel model = new()
            {

                Get_JobPostCount_For_UTM_Tracking_Lead = resultData.Select(y => new SelectListItem
                {
                    Text = y.JobCount.ToString(),
                    Value = y.JobCount.ToString()
                }).OrderBy(x => x.Text).ToList(),

                UTM_Source = db.PrgUtmSources.Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.UtmSource
                }).OrderBy(x => x.Text).ToList(),

                UTM_Campaign = db.PrgUtmCampaigns.Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.UtmCampaign
                }).OrderBy(x => x.Text).ToList(),
                UTM_Content = db.PrgUtmContents.Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.UtmContent
                }).OrderBy(x => x.Text).ToList(),
                UTM_Medium = db.PrgUtmMedia.Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.UtmMedium
                }).OrderBy(x => x.Text).ToList(),
                UTM_Placement = db.PrgUtmPlacements.Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.UtmPlacement
                }).OrderBy(x => x.Text).ToList(),
                UTM_Term = db.PrgUtmTerms.Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.UtmTerm
                }).OrderBy(x => x.Text).ToList(),
                Ref_Url = db.PrgRefUrls.Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.RefUrl
                }).OrderBy(x => x.Text).ToList()
            };

            return model;
        }


        #endregion

        #region Client portal tracking details
        public async Task<List<sproc_UTS_ClientList_Result>> ClientList()
        {
            return db.Set<sproc_UTS_ClientList_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.sproc_UTS_ClientList)).ToList();
        }
        public async Task<List<Sproc_Get_Email_SubjectList_Result>> Sproc_Get_Email_SubjectList()
        {
            return db.Set<Sproc_Get_Email_SubjectList_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Get_Email_SubjectList)).ToList();
        }
        public async Task<List<Sproc_Get_UTS_ClientPortalTracking_Details_Result>> ClientPortalTracking_Details(string paramasString)
        {
            return db.Set<Sproc_Get_UTS_ClientPortalTracking_Details_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_UTS_ClientPortalTracking_Details, paramasString)).ToList();
        }
        public async Task<List<Sproc_Get_UTS_ClientPortalTracking_Details_Popup_Result>> ClientPortalTracking_Details_Popup(string paramasString)
        {
            return db.Set<Sproc_Get_UTS_ClientPortalTracking_Details_Popup_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_UTS_ClientPortalTracking_Details_Popup, paramasString)).ToList();
        }
        #endregion

        #region Replacement Report
        public async Task<List<sproc_Get_Replacement_Report_Result>> sproc_Get_Replacement_Report(string paramasString)
        {
            return await db.Set<sproc_Get_Replacement_Report_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Get_Replacement_Report, paramasString)).ToListAsync();
        }
        #endregion

        #region Talent Backout Report
        public async Task<List<Sproc_Get_TalentBackout_Report_Result>> Sproc_Get_TalentBackout_Report(string paramasString)
        {
            return await db.Set<Sproc_Get_TalentBackout_Report_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Get_TalentBackout_Report, paramasString)).ToListAsync();
        }
        #endregion

        #region AWS SES tracking details
        
        public async Task<List<Sproc_Get_AWS_SES_Tracking_Details_Result>> AWSSESTracking_Details(string paramasString)
        {
            return db.Set<Sproc_Get_AWS_SES_Tracking_Details_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_AWS_SES_Tracking_Details, paramasString)).ToList();
        }
        public async Task<List<Sproc_Get_AWS_SES_Tracking_Details_Popup_Result>> AWSSESTracking_Details_Popup(string paramasString)
        {
            return db.Set<Sproc_Get_AWS_SES_Tracking_Details_Popup_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_AWS_SES_Tracking_Details_Popup, paramasString)).ToList();
        }
        #endregion
        #endregion

        #region Private methods
        private async Task<string> ConvertToJsonAsync(string query, bool IsDemandFunnel = false)
        {
            string json = "";
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                await db.Database.OpenConnectionAsync().ConfigureAwait(false);

                using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    var rows = new List<Dictionary<string, object>>();

                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            object columnValue = reader.GetValue(i);

                            if (columnName == "Stage")
                            {
                                columnValue = Convert.ToString(columnValue).Replace("_1", "");
                            }

                            if (columnName.Contains("_Total"))
                            {
                                columnName = Convert.ToString(columnName).Replace("_Total", "");
                            }

                            if (IsDemandFunnel) // Remove odr pool column only from demand funnel report
                            {
                                if (!columnName.Contains("Pool Total") && !columnName.Contains("Odr Total") &&
                                !columnName.Contains("_Pool") && !columnName.Contains("_Odr"))
                                {
                                    row[columnName] = columnValue;
                                }
                            }
                            else
                            {
                                row[columnName] = columnValue;
                            }
                        }
                        rows.Add(row);
                    }

                    json = System.Text.Json.JsonSerializer.Serialize(rows);
                }
            }
            return json;
        }

       


        #endregion
    }
}
