using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;
using static UTSATSAPI.Helpers.Constants;
using static UTSATSAPI.Helpers.Enum;

namespace UTSATSAPI.Repositories.Repositories
{
    /// <summary>
    /// ViewAllHRRepository
    /// </summary>
    /// <seealso cref="UTSATSAPI.Repositories.Interfaces.IViewAllHR" />
    public class ViewAllHRRepository : IViewAllHR
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private readonly IUniversalProcRunner _universalProcRunner;
        private IConfiguration _configuration;
        private readonly IUpChatCall _iUpChatCall;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructors
        public ViewAllHRRepository(TalentConnectAdminDBContext _db, IUniversalProcRunner universalProcRunner, IConfiguration configuration, IUpChatCall iUpChatCall, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            _universalProcRunner = universalProcRunner;
            _configuration = configuration;
            _iUpChatCall = iUpChatCall;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Public Methods

        public List<sproc_ViewAllHRs_Result> GetAllHRs(ViewAllHRViewModel viewAllHRViewModel, int loggedInUserTypeId, long LoggedInUserID)
        {
            string sidx = (string.IsNullOrEmpty(viewAllHRViewModel.Sortdatafield)) ? "CreatedDateTime" : viewAllHRViewModel.Sortdatafield;
            string sord = (string.IsNullOrEmpty(viewAllHRViewModel.Sortorder)) ? "desc" : viewAllHRViewModel.Sortorder;

            string fromDate = "";
            string toDate = "";


            int pageIndex = viewAllHRViewModel.Pagenum;
            int pageSize = viewAllHRViewModel.Pagesize;



            if (viewAllHRViewModel.FilterFields_ViewAllHRs != null && !string.IsNullOrEmpty(viewAllHRViewModel.FilterFields_ViewAllHRs.fromDate) && !string.IsNullOrEmpty(viewAllHRViewModel.FilterFields_ViewAllHRs.toDate))
            {
                fromDate = CommonLogic.ConvertString2DateTime(viewAllHRViewModel.FilterFields_ViewAllHRs.fromDate).ToString("yyyy-MM-dd");
                toDate = CommonLogic.ConvertString2DateTime(viewAllHRViewModel.FilterFields_ViewAllHRs.toDate).ToString("yyyy-MM-dd");
            }

            viewAllHRViewModel.FilterFields_ViewAllHRs ??= new();

            //Trim the blank spaces from the end of the search text.
            //Pick only first 200 characters.
            //Replace the single quotes with double quotes if any.
            string searchText = string.Empty;
            if (!string.IsNullOrEmpty(viewAllHRViewModel.searchText))
            {
                searchText = viewAllHRViewModel.searchText.TrimStart().TrimEnd();
                if (searchText.Length > 200)
                {
                    searchText = searchText.Substring(0, 200);
                }
                searchText = searchText.Replace('\'', '\"');
            }

            string TypeOfHr = string.Empty;

            if (viewAllHRViewModel.FilterFields_ViewAllHRs != null && !string.IsNullOrEmpty(viewAllHRViewModel.FilterFields_ViewAllHRs.HRType))
            {
                TypeOfHr = viewAllHRViewModel.FilterFields_ViewAllHRs.HRType.Contains(",") ? "" : viewAllHRViewModel.FilterFields_ViewAllHRs.HRType;
            }

            var AllHRListData = db.Set<sproc_ViewAllHRs_Result>().FromSqlRaw(@$"{Constants.ProcConstant.sproc_UTS_GetAllHRs}  
                                {pageIndex}, {pageSize}, '{sidx}', '{sord}', '{viewAllHRViewModel.FilterFields_ViewAllHRs.IsPoolODRBoth}'
                                , '{viewAllHRViewModel.FilterFields_ViewAllHRs.Tenure}', '{viewAllHRViewModel.FilterFields_ViewAllHRs.Position}'
                                , '{viewAllHRViewModel.FilterFields_ViewAllHRs.Company}', '{viewAllHRViewModel.FilterFields_ViewAllHRs.TR}'
                                , '{viewAllHRViewModel.FilterFields_ViewAllHRs.TypeOfEmployee}','{viewAllHRViewModel.FilterFields_ViewAllHRs.SalesRep}'
                                , '{viewAllHRViewModel.FilterFields_ViewAllHRs.HRStatus}','{viewAllHRViewModel.FilterFields_ViewAllHRs.Manager}'
                                , '{viewAllHRViewModel.FilterFields_ViewAllHRs.TimeZone}', {loggedInUserTypeId}, {LoggedInUserID} 
                                , '{fromDate}', '{toDate}', '{searchText}' , '{viewAllHRViewModel.IsHrfocused}' 
                                , '{viewAllHRViewModel.StarNextWeek}', '{TypeOfHr}' ,'{viewAllHRViewModel.IsDirectHR}', '{viewAllHRViewModel.IsFrontEndHR}', '{viewAllHRViewModel.HRTypeIds}','{viewAllHRViewModel.LeadUserID}','{viewAllHRViewModel.FilterFields_ViewAllHRs.CompanyTypeIds}','{viewAllHRViewModel.FilterFields_ViewAllHRs.Geos}'");

            return AllHRListData.ToList();
        }

        public HRDetailViewModel ShowHRDetail(long id, long? WhatToaddClick = 0)
        {
            HRDetailViewModel HRDetailViewModel = new();
            HRDetailViewModel.ClientDetail = new();

            var HR_ClientDetails = db.Set<sp_get_HRClientDetails_Result>().FromSqlRaw($"{Constants.ProcConstant.sp_UTS_get_HRClientDetails} " +
                                        $"{id} ").ToList().FirstOrDefault();

            var salesHiringRequestData = db.GenSalesHiringRequests.AsNoTracking().FirstOrDefault(x => x.Id == id);

            //if client or company details are not fetch than it will be return null
            if (HR_ClientDetails == null || salesHiringRequestData == null)
            {
                HRDetailViewModel.ClientDetail = null;
                return HRDetailViewModel;
            }

            var HR_MissingAction = db.Set<sproc_FetchMissingAction_Result>().FromSqlRaw($"{Constants.ProcConstant.sproc_FetchMissingAction} " +
                                        $"{id} ").ToList().Where(x => x.IsTalentAction == false).FirstOrDefault();
            HRDetailViewModel.FetchMissingAction = HR_MissingAction;

            if (HR_ClientDetails.AdHocHR && !HR_ClientDetails.PoolHR)
                HRDetailViewModel.AdhocPoolValue = "ODR";
            if (!HR_ClientDetails.AdHocHR && HR_ClientDetails.PoolHR)
                HRDetailViewModel.AdhocPoolValue = "Pool";
            if (HR_ClientDetails.AdHocHR && HR_ClientDetails.PoolHR)
                HRDetailViewModel.AdhocPoolValue = "ODR + Pool";

            var getHRDetails = db.GenSalesHiringRequestDetails.FirstOrDefault(x => x.HiringRequestId == id);

            if (salesHiringRequestData != null && HR_ClientDetails != null && getHRDetails != null)
            {
                HRDetailViewModel.HR_Id = (int)id;
                HRDetailViewModel.ClientDetail = HR_ClientDetails;
                HRDetailViewModel.HRDetails = getHRDetails;

                HRDetailViewModel.StarMarkedStatusCode = HR_ClientDetails.StarMarkedStatusCode;
                HRDetailViewModel.companyCategory = HR_ClientDetails.Category;

                HRDetailViewModel.IsActive = salesHiringRequestData.IsActive ?? false;
                HRDetailViewModel.ReplaceOnBoardID = salesHiringRequestData.ReplaceOnBoardId;
                HRDetailViewModel.TR_Accepted = salesHiringRequestData.TrAccepted;
                HRDetailViewModel.Is_HRTypeDP = salesHiringRequestData.IsHrtypeDp == null ? false : salesHiringRequestData.IsHrtypeDp;
                HRDetailViewModel.Guid = salesHiringRequestData.Guid;
                HRDetailViewModel.HRManagedORNot = salesHiringRequestData.IsManaged;
                HRDetailViewModel.IsAccepted = salesHiringRequestData.IsAccepted;
                HRDetailViewModel.IsPartnerShipHR = salesHiringRequestData.IsPartnerHr ?? false;
                HRDetailViewModel.IsDirectHR = salesHiringRequestData.IsDirectHr ?? false;

                HRDetailViewModel.HRRoleStatusID = getHRDetails.RoleStatusId;
                HRDetailViewModel.HRRoleStatus = db.PrgHiringRequestRoleStatuses.Where(x => x.Id == getHRDetails.RoleStatusId).Select(x => x.HiringRequestRoleStatus).FirstOrDefault();

                HRDetailViewModel.HRStatusID = salesHiringRequestData.StatusId;
                //HRDetailViewModel.HRStatus = db.PrgHiringRequestStatuses.Where(x => x.Id == salesHiringRequestData.StatusId).Select(x => x.HiringRequestStatus).FirstOrDefault();
                HRDetailViewModel.HRStatus = HR_ClientDetails.HRStatus;
                HRDetailViewModel.HRStatusCode = HR_ClientDetails.HRStatusCode;

                HRDetailViewModel.JobStatusID = salesHiringRequestData.JobStatusId;
                HRDetailViewModel.JobStatus = db.PrgJobStatusClientPortals.Where(x => x.Id == salesHiringRequestData.JobStatusId).Select(x => x.JobStatus).FirstOrDefault();

                // If BqLink and Discovery Call data is not a link then display this as blank.
                if (!string.IsNullOrEmpty(HR_ClientDetails.BQLink) &&
                    !(HR_ClientDetails.BQLink.Contains("http") ||
                    HR_ClientDetails.BQLink.Contains("https")))
                {
                    HR_ClientDetails.BQLink = string.Empty;
                }

                if (!string.IsNullOrEmpty(HR_ClientDetails.Discovery_Call) &&
                    !(HR_ClientDetails.Discovery_Call.Contains("http") ||
                    HR_ClientDetails.Discovery_Call.Contains("https")))
                {
                    HR_ClientDetails.Discovery_Call = string.Empty;
                }

                #region Check Model IsPayPerHire / IsPayPerCredit
                //default set false
                HRDetailViewModel.IsPayPerHire = false;
                HRDetailViewModel.IsPayPerCredit = false;

                if (salesHiringRequestData.HrtypeId == (short)PayPerHire.SalesHR)
                {
                    HRDetailViewModel.IsPayPerHire = true;
                }
                if (salesHiringRequestData.HrtypeId == (short)PayPerCredit.PostaJobViewBasedCredit || salesHiringRequestData.HrtypeId == (short)PayPerCredit.PostaJobCreditBased || salesHiringRequestData.HrtypeId == (short)PayPerCredit.PostajobWithViewCreditsButnoJobPostCredits)
                {
                    HRDetailViewModel.IsPayPerCredit = true;
                }
                #endregion

                HRCommonOperation commonOperation = new HRCommonOperation(db);

                #region Pay Per Hire Pricing model

                HRDetailViewModel.transparentModel = new PayPerHireModel();

                if ((bool)HRDetailViewModel.IsPayPerHire && salesHiringRequestData.IsTransparentPricing != null &&
                    salesHiringRequestData.HiringTypePricingId > 0)//Pay per Hire
                {
                    var getprgHiringTypePricing = db.PrgHiringTypePricings.FirstOrDefault(x => x.Id == salesHiringRequestData.HiringTypePricingId);
                    //var _CurrencyExchangeRate = db.PrgCurrencyExchangeRates.Where(x => x.CurrencyCode == getHRDetails.Currency).FirstOrDefault();
                    var GetprgPayrollType = db.PrgPayrollTypes.FirstOrDefault(x => x.Id == salesHiringRequestData.PayrollTypeId);

                    if (getprgHiringTypePricing != null)
                    {
                        HRDetailViewModel.transparentModel = commonOperation.GetTransparentModelData(salesHiringRequestData, getHRDetails, getprgHiringTypePricing, GetprgPayrollType, HR_ClientDetails.CompanyName ?? "NA");
                    }
                }
                else
                {
                    if (salesHiringRequestData.IsHrtypeDp != null && salesHiringRequestData.IsHrtypeDp)
                    {
                        HRDetailViewModel.EngagementType = "Direct Placement";
                    }
                    else
                    {
                        string month = string.Empty;
                        if (getHRDetails.SpecificMonth == -1)
                        {
                            month = "Indefinite Months";
                        }
                        else if (getHRDetails.SpecificMonth > 0)
                        {
                            month = getHRDetails.SpecificMonth.ToString() + " Months"; ;
                        }
                        else
                        {
                            month = "0 Months";
                        }
                        HRDetailViewModel.EngagementType = "Contract - " + month;
                    }
                    HRDetailViewModel.BudgetTitle = "Budget";
                    HRDetailViewModel.BudgetText = !string.IsNullOrEmpty(getHRDetails.Cost) ? getHRDetails.Cost : string.Empty;
                }
                #endregion

                #region Pay per Credit
                string Availability = salesHiringRequestData.Availability ?? "Full Time";

                if ((bool)HRDetailViewModel.IsPayPerCredit)
                {
                    HRDetailViewModel.PayPerCreditModel = new();
                    HRDetailViewModel.PayPerCreditModel.EngagementType = "";

                    Sp_UTS_GetCreditUtilization_BasedOnHR_Result obj = new();
                    obj = GetCreditUtilization_BasedOnHR(id);
                    if (obj != null)
                    {
                        HRDetailViewModel.PayPerCreditModel.EngagementType = obj.CreditUtilization ?? "";
                    }

                    HRDetailViewModel.PayPerCreditModel.JobType = commonOperation.PayPerCredit_JobType(salesHiringRequestData.IsHiringLimited ?? false, getHRDetails.SpecificMonth ?? 0, Availability);
                    HRDetailViewModel.PayPerCreditModel.BudgetTitle = "Salary Budget";
                    HRDetailViewModel.PayPerCreditModel.BudgetText = !string.IsNullOrEmpty(getHRDetails.Cost) ? getHRDetails.Cost : string.Empty;
                    HRDetailViewModel.IsVettedProfile = salesHiringRequestData.IsVettedProfile;
                }
                #endregion

                if (HRDetailViewModel.HRStatusID > 0 &&
                    HRDetailViewModel.HRStatusID == (short)prg_HiringRequestStatus.Won)
                {
                    HRDetailViewModel.IsHRClosed = true;
                }
                else
                    HRDetailViewModel.IsHRClosed = false;

                var obj_gen_ContactTalentPrioritydetails = db.GenContactTalentPriorities.Where(x => x.HiringRequestId == id).FirstOrDefault();

                if (obj_gen_ContactTalentPrioritydetails != null)
                    HRDetailViewModel.DpFlag = true;
                else
                    HRDetailViewModel.DpFlag = false;
                HRDetailViewModel.DpFlag = false;

                //var HRTalentDetails = db.Set<sp_get_HRTalentDetails_Result>().FromSqlRaw($"{Constants.ProcConstant.sp_UTS_get_HRTalentDetails} " +
                //                            $"{id} ").ToList();
                var HRTalentDetails = db.Set<sp_UTS_get_HRTalentDetails_UsingPagination_Result>().FromSqlRaw($"{Constants.ProcConstant.sp_UTS_get_HRTalentDetails_UsingPagination} " +
                                            $"{id} ").ToList();
                HRDetailViewModel.HRTalentDetails = HRTalentDetails;

                var TalentMIssingAction = db.Set<sproc_NextActionsForTalent_Result>().FromSqlRaw($"{Constants.ProcConstant.sproc_UTS_NextActionsForTalent} " +
                                            $"{id} ").ToList();
                HRDetailViewModel.NextActionsForTalent = TalentMIssingAction;

                #region Talent salary Dynamic details  
                if (HRDetailViewModel.HRTalentDetails != null && HRDetailViewModel.HRTalentDetails.Any())
                {
                    List<DynamicSalaryInfo> TalentDynamicInfo = new();
                    foreach (sp_UTS_get_HRTalentDetails_UsingPagination_Result obj in HRDetailViewModel.HRTalentDetails)
                    {
                        DynamicSalaryInfo dynamicInfo = new();
                        dynamicInfo.TalentID = obj.TalentID;
                        List<TalentDynamicInfo> TalentInfo = new();

                        TalentInfo = commonOperation.TalentInfo(obj, (bool)obj.IsHRTypeDP, (bool)HRDetailViewModel.IsPayPerCredit);
                        if (TalentInfo != null || TalentInfo.Count > 0)
                        {
                            dynamicInfo.TalentDynamicInfo = TalentInfo;
                        }
                        TalentDynamicInfo.Add(dynamicInfo);
                    }
                    HRDetailViewModel.DynamicSalaryInfo = new List<DynamicSalaryInfo>();
                    if (TalentDynamicInfo.Any())
                    {
                        HRDetailViewModel.DynamicSalaryInfo.AddRange(TalentDynamicInfo);
                    }
                }
                #endregion

                #region InterviewSlotDetails
                if (HRDetailViewModel.HRTalentDetails != null && HRDetailViewModel.HRTalentDetails.Any())
                {
                    HRDetailViewModel.InterviewSlotDetails = new();
                    string strTimeZone = string.Empty;

                    foreach (sp_UTS_get_HRTalentDetails_UsingPagination_Result obj in HRDetailViewModel.HRTalentDetails)
                    {
                        TalentWiseInterviewDetails interviewDetails = new TalentWiseInterviewDetails();
                        GenShortlistedTalentInterviewDetail interviewShortlisted = db.GenShortlistedTalentInterviewDetails.FirstOrDefault(x => x.Id == obj.Shortlisted_InterviewID && x.TalentId == obj.TalentID);
                        if (interviewShortlisted != null)
                        {
                            PrgContactTimeZone timeZone = db.PrgContactTimeZones.FirstOrDefault(y => y.Id == interviewShortlisted.TimeZoneId);
                            if (timeZone != null)
                            {
                                strTimeZone = timeZone.ShortName;
                            }
                        }
                        else
                        {
                            strTimeZone = string.Empty; // Empty the variable if no timezone available
                        }

                        interviewDetails.TalentID = obj.TalentID;
                        interviewDetails.SlotList = new List<InterviewSlotDetails>();

                        if (!string.IsNullOrEmpty(obj.InterviewDateTime))
                        {
                            string[] arrStr = obj.InterviewDateTime.Split("\n");
                            foreach (string str in arrStr)
                            {
                                string[] DateTimeSplit = str.Split(" ");
                                InterviewSlotDetails slotGiven_Interview = new InterviewSlotDetails();
                                slotGiven_Interview.StrDateTime = DateTimeSplit[0].Trim() + " | " + DateTimeSplit[1].Trim() + " - " + DateTimeSplit[3].Trim() + " " + strTimeZone;
                                slotGiven_Interview.Date = DateTimeSplit[0].Trim();
                                slotGiven_Interview.StartTime = DateTimeSplit[1].Trim();
                                slotGiven_Interview.EndTime = DateTimeSplit[3].Trim();
                                slotGiven_Interview.TimeZone = strTimeZone;

                                interviewDetails.SlotList.Add(slotGiven_Interview);
                            }
                        }
                        HRDetailViewModel.InterviewSlotDetails.Add(interviewDetails);
                    }
                }
                #endregion

                var totalOnBoardDetailsForManagedHR = db.GenOnBoardTalents.Where(x => x.HiringRequestId == id).ToList();
                if (salesHiringRequestData.NoofTalents == totalOnBoardDetailsForManagedHR.Count())
                    HRDetailViewModel.ShowAssignTalent = false;
                else
                    HRDetailViewModel.ShowAssignTalent = true;
            }

            var HRHistoryDetails = db.Set<sp_UTS_get_HRHistory_UsingPagination_Result>().FromSqlRaw($"{Constants.ProcConstant.sp_UTS_get_HRHistory_UsingPagination} " + $"{id} ").ToList();
            //var HRHistoryDetails = db.Set<sp_get_HRHistory_Result>().FromSqlRaw($"{Constants.ProcConstant.sp_UTS_get_HRHistory} " + $"{id} ").ToList();
            if (HRHistoryDetails != null && HRHistoryDetails.Any())
            {
                HRDetailViewModel.HRAction = HRHistoryDetails.Select(x => string.IsNullOrEmpty(x.DisplayName) ? x.ActionName : x.DisplayName).FirstOrDefault();
                HRDetailViewModel.HRActionID = HRHistoryDetails.Select(x => x.ActionID).FirstOrDefault();
                HRDetailViewModel.HRHistory = HRHistoryDetails;
            }

            var HRInterviewerDetails = db.Set<sp_getInterviewdetailsForViewAllHR_Result>().FromSqlRaw($"{Constants.ProcConstant.sp_getInterviewdetailsForViewAllHR} " + $"{HRDetailViewModel.HR_Id} ").ToList();
            HRDetailViewModel.HRInterviewDetails = HRInterviewerDetails;

            HRDetailViewModel.UsersToTag = db.UsrUsers.Where(x => x.UserTypeId != 1 && x.UserTypeId != 2 && x.IsActive == true)
                                                .Select(y => new SelectListItem
                                                {
                                                    Text = y.FullName,
                                                    Value = y.Id.ToString()
                                                }).ToList();

            #region AllowSpecialEdit
            var LoggedInUserID = SessionValues.LoginUserId;
            var loggedInUserEmployeeID = db.UsrUsers.Where(x => x.Id == LoggedInUserID).Select(a => a.EmployeeId).FirstOrDefault();

            if (loggedInUserEmployeeID != null)
            {
                object[] param = new object[] { LoggedInUserID, loggedInUserEmployeeID };
                string paramString = CommonLogic.ConvertToParamString(param);
                Sproc_UTS_FetchUsersWithSpecialEditAccess_Result result = CheckSpecialEdits(paramString);

                if (result != null)
                {
                    // If job is not closed then show the Edit HR COE CTA.
                    if (HRDetailViewModel.JobStatusID != (short)prg_JobStatus_ClientPortal.Closed)
                    {
                        HRDetailViewModel.AllowSpecialEdit = result.AllowSpecialEdit;
                    }

                    //UTS-7484: Allow HR detele to specific set of users.
                    HRDetailViewModel.AllowHRDelete = result.AllowHRDelete;
                }
            }

            #endregion

            if (HRDetailViewModel != null)
                HRDetailViewModel = customizeCTA(HRDetailViewModel);

            return HRDetailViewModel;
        }
        private HRDetailViewModel customizeCTA(HRDetailViewModel hRDetail)
        {
            var LoggedInUserTypeID = SessionValues.LoginUserTypeId;

            hRDetail.IsPayPerCredit = hRDetail.IsPayPerCredit ?? false;
            hRDetail.IsPayPerHire = hRDetail.IsPayPerHire ?? false;

            bool IsEnabled = true;

            #region HR Level CTA

            hRDetail.dynamicCTA = new DynamicCTA();

            //Set 1
            List<CTAInfo> CTA_Set1 = new List<CTAInfo>();

            //Commnent AM assignment CTA , due to onboarding enhancemnet
            //if (hRDetail.HRTalentDetails != null
            //    && hRDetail.NextActionsForTalent != null
            //    && hRDetail.NextActionsForTalent.Count() > 0 &&
            //    ((bool)!hRDetail.IsPartnerShipHR &&
            //            hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Cancelled &&
            //            hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Lost))
            //{
            //    foreach (var profile in hRDetail.HRTalentDetails.ToList())
            //    {
            //        if (!string.IsNullOrEmpty(profile.InterviewStatus) &&
            //            (profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted &&
            //            profile.ClientOnBoarding_StatusID != 2 && profile.IsAMAssigned == 0 && profile.EngagemenID != ""))
            //        {
            //            CTAInfo cTAInfo = new CTAInfo(HrCTA.AMAssignment, "AM Assignment", true);
            //            CTA_Set1.Add(cTAInfo);
            //            hRDetail.OnBoardID = profile.OnBoardId;
            //            break;
            //        }
            //    }
            //}

            if (hRDetail.HRActionID == (short)Action_Of_History.Create_HR ||
                hRDetail.HRActionID == (short)Action_Of_History.TR_Increase)
            {
                CTAInfo cTAInfo = new CTAInfo(HrCTA.DebriefingHR, "Debriefing HR", true);
                CTA_Set1.Add(cTAInfo);
            }

            hRDetail.dynamicCTA.CTA_Set1 = CTA_Set1;

            //Set 2
            List<CTAInfo> CTA_Set2 = new List<CTAInfo>();
            if (hRDetail.JobStatusID == (short)prg_JobStatus_ClientPortal.Draft ||
                hRDetail.HRStatusID == (short)prg_HiringRequestStatus.Open)
            {
                CTAInfo cTAInfo = new CTAInfo(HrCTA.EditHR, "Edit HR", true);
                CTA_Set2.Add(cTAInfo);
            }

            if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Cancelled)
            {
                CTAInfo cTAInfoNotes = new CTAInfo(HrCTA.AddNotes, "Add Notes", true);
                CTA_Set2.Add(cTAInfoNotes);
            }

            hRDetail.dynamicCTA.CTA_Set2 = CTA_Set2;

            //Clone
            if (hRDetail.ClientDetail != null && !string.IsNullOrEmpty(hRDetail.ClientDetail.HR_Number) &&
                hRDetail.JobStatusID != (short)prg_JobStatus_ClientPortal.Draft)
            {
                //if (LoggedInUserTypeID == (short)ManagerType.TalentOps || LoggedInUserTypeID == (short)ManagerType.OpsTeamManager)
                //    IsEnabled = false;
                //else
                //    IsEnabled = true;

                hRDetail.dynamicCTA.CloneHR = new CTAInfo(HrCTA.Clone, "Clone " + hRDetail.ClientDetail.HR_Number, IsEnabled);
            }

            if (hRDetail.JobStatusID != (short)prg_JobStatus_ClientPortal.Closed)
            {
                hRDetail.dynamicCTA.CloseHr = new CTAInfo(HrCTA.CloseHR, "Close HR", true);
            }

            if (hRDetail.JobStatusID == (short)prg_JobStatus_ClientPortal.Closed)
            {
                //if (LoggedInUserTypeID == (short)ManagerType.TalentOps || LoggedInUserTypeID == (short)ManagerType.OpsTeamManager)
                //    IsEnabled = false;
                //else
                //    IsEnabled = true;

                hRDetail.dynamicCTA.ReopenHR = new CTAInfo(HrCTA.ReopenHR, "Reopen HR", IsEnabled);
            }

            if ((bool)hRDetail.IsPayPerHire && hRDetail.JobStatusID != (short)prg_JobStatus_ClientPortal.Closed &&
                (hRDetail.ReplaceOnBoardID == null || hRDetail.ReplaceOnBoardID == 0) &&
                (LoggedInUserTypeID == (short)ManagerType.Administrator || LoggedInUserTypeID == (short)ManagerType.Developer ||
                LoggedInUserTypeID == (short)ManagerType.Sales || LoggedInUserTypeID == (short)ManagerType.SalesManager))
            {
                hRDetail.dynamicCTA.UpdateTR = new CTAInfo(HrCTA.UpdateTR, "Update TR", true);
            }

            #endregion

            #region HRTalentDetails CTA
            if (hRDetail != null &&
                (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Cancelled &&
                 hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Lost) &&
                hRDetail.HRTalentDetails != null && hRDetail.HRTalentDetails.Any())
            {
                hRDetail.dynamicCTA.talent_CTAs = new List<Talent_CTA>();
                List<Talent_CTA> talent_CTAs = new List<Talent_CTA>();

                foreach (sp_UTS_get_HRTalentDetails_UsingPagination_Result profile in hRDetail.HRTalentDetails)
                {
                    Talent_CTA cTA = new Talent_CTA();
                    cTA.TalentID = profile.TalentID;
                    bool RejectTalentCTAAlreadyAvailable = false;
                    List<CTAInfo> cTAInfos = new List<CTAInfo>();

                    #region CTA Conditions

                    if ((bool)hRDetail.IsPayPerCredit &&
                        hRDetail.HRRoleStatusID != (short)prg_HiringRequest_RoleStatus.OnHold &&
                        profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Deleted)
                    {
                        CTAInfo cTAInfo = new CTAInfo(TalentCTA.TalentStatus, "Talent Status", true);
                        cTAInfos.Add(cTAInfo);
                    }

                    if ((bool)hRDetail.IsPayPerHire &&
                        hRDetail.HRRoleStatusID != (short)prg_HiringRequest_RoleStatus.OnHold &&
                        profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Deleted)
                    {
                        bool IsCancelledOnHoldInReplacementRejected = true;
                        if (profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Cancelled &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.OnHold &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.InReplacement &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Rejected)
                        {
                            IsCancelledOnHoldInReplacementRejected = false;
                        }
                        if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Won &&
                            profile.SelectedInterviewId > 0 &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Cancelled &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Rejected &&
                            (profile.InterViewStatusId != (short)prg_InterviewStatus.Interview_Completed &&
                             profile.InterViewStatusId != (short)prg_InterviewStatus.Feedback_Submitted &&
                             profile.InterViewStatusId != (short)prg_InterviewStatus.Cancelled))
                        {
                            CTAInfo cTAInfo = new CTAInfo(TalentCTA.InterviewStatus, "Interview Status", true);
                            cTAInfos.Add(cTAInfo);
                        }
                        //if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Won &&
                        //        (profile.InterViewStatusId == 0 ||
                        //    profile.InterViewStatusId == (short)prg_InterviewStatus.Cancelled ||
                        //    profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted) &&
                        //    profile.TalentStatusID_BasedOnHR == (short)prg_TalentStatus_AfterClientSelection.ProfileShared)
                        //{
                        //    //if (LoggedInUserTypeID == (short)ManagerType.TalentOps || LoggedInUserTypeID == (short)ManagerType.OpsTeamManager)
                        //    //    IsEnabled = false;
                        //    //else
                        //    //    IsEnabled = true;

                        //    CTAInfo cTAInfo = new CTAInfo(TalentCTA.ScheduleInterview, "Schedule Interview", IsEnabled);
                        //    cTAInfos.Add(cTAInfo);
                        //}
                        if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Won &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Cancelled &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Rejected &&
                            profile.InterViewStatusId != 0 &&
                            profile.InterViewStatusId != (short)prg_InterviewStatus.Interview_in_Process &&
                            profile.InterViewStatusId != (short)prg_InterviewStatus.Interview_Completed &&
                            profile.InterViewStatusId != (short)prg_InterviewStatus.Feedback_Submitted &&
                            profile.InterViewStatusId != (short)prg_InterviewStatus.Cancelled)
                        {
                            CTAInfo cTAInfo = new CTAInfo(TalentCTA.RescheduleInterview, "Reschedule Interview", true);
                            cTAInfos.Add(cTAInfo);
                        }
                        if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Won &&
                            profile.InterViewStatusId != 0 &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Cancelled &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Rejected &&
                            profile.InterViewStatusId == (short)prg_InterviewStatus.Slot_Given)
                        {
                            //if (LoggedInUserTypeID == (short)ManagerType.Sales || LoggedInUserTypeID == (short)ManagerType.SalesManager)
                            //    IsEnabled = false;
                            //else
                            //    IsEnabled = true;

                            CTAInfo cTAInfo = new CTAInfo(TalentCTA.ConfirmSlot, "Confirm Slot", IsEnabled);
                            cTAInfos.Add(cTAInfo);
                        }
                        if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Won &&
                            !IsCancelledOnHoldInReplacementRejected &&
                            profile.InterViewStatusId == (short)prg_InterviewStatus.Interview_Completed)
                        {
                            //if (LoggedInUserTypeID == (short)ManagerType.TalentOps || LoggedInUserTypeID == (short)ManagerType.OpsTeamManager)
                            //    IsEnabled = false;
                            //else
                            //    IsEnabled = true;

                            //CTAInfo cTAInfo = new CTAInfo(TalentCTA.SubmitClientFeedback, "Submit Client Feedback", IsEnabled);
                            //CTAInfo SubmitClientFeedback = new CTAInfo(TalentCTA.SubmitClientFeedback, "Submit As Hire", IsEnabled);
                            //cTAInfos.Add(SubmitClientFeedback);

                            CTAInfo SubmitClientFeedback = new CTAInfo(TalentCTA.SubmitClientFeedback, "Offer Position", IsEnabled);
                            cTAInfos.Add(SubmitClientFeedback);

                            CTAInfo AnotherRoundInterview = new CTAInfo(TalentCTA.SubmitFeedbackWithAnotherRound, "Move to Another Round", true);
                            cTAInfos.Add(AnotherRoundInterview);

                            CTAInfo cTAInfo = new CTAInfo(TalentCTA.SubmitFeedbackWithNoHire, "Reject Talent", true);
                            cTAInfos.Add(cTAInfo);

                            RejectTalentCTAAlreadyAvailable = true;
                        }
                        if (!IsCancelledOnHoldInReplacementRejected &&
                            profile.OnBoardId > 0 && profile.TalentOnBoardDate == "" &&
                            profile.ClientOnBoarding_StatusID != 2 && profile.ClientOnBoarding_StatusID != 7 &&
                            profile.ClientOnBoarding_StatusID != 8 &&
                            //profile.IsAMAssigned != 0 &&
                            profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted)
                        {
                            //if (LoggedInUserTypeID == (short)ManagerType.TalentOps || LoggedInUserTypeID == (short)ManagerType.OpsTeamManager)
                            //    IsEnabled = false;
                            //else
                            //    IsEnabled = true;

                            bool IsDPHr = hRDetail.Is_HRTypeDP ?? false;
                            string CTAName = IsDPHr ? "Release Offer Details" : "Confirm Contract Details";

                            CTAInfo cTAInfo = new CTAInfo(TalentCTA.GotoOnBoard, CTAName, IsEnabled);
                            cTAInfos.Add(cTAInfo);
                        }
                        if (!IsCancelledOnHoldInReplacementRejected && profile.OnBoardId > 0 && profile.ClientOnBoarding_StatusID == 2 &&
                            (profile.TalentOnBoarding_StatusID != 2 && profile.TalentOnBoarding_StatusID != 7) &&
                            profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted && profile.TalentOnBoardDate == "")
                        {
                            //if (LoggedInUserTypeID == (short)ManagerType.Sales || LoggedInUserTypeID == (short)ManagerType.SalesManager)
                            //    IsEnabled = false;
                            //else
                            //    IsEnabled = true;

                            CTAInfo cTAInfo = new CTAInfo(TalentCTA.UpdateTalentOnBoardStatus, "Update Talent On Board Status", IsEnabled);
                            cTAInfos.Add(cTAInfo);
                        }
                        if (!IsCancelledOnHoldInReplacementRejected && profile.OnBoardId > 0 &&
                            profile.ClientOnBoarding_StatusID == 2 && (profile.TalentOnBoarding_StatusID == 2 ||
                            profile.TalentOnBoarding_StatusID == 7) && profile.LegalClientOnBoarding_StatusID != 2 &&
                            profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted && profile.TalentOnBoardDate == "")
                        {
                            //if (LoggedInUserTypeID == (short)ManagerType.TalentOps || LoggedInUserTypeID == (short)ManagerType.OpsTeamManager)
                            //    IsEnabled = false;
                            //else
                            //    IsEnabled = true;

                            CTAInfo cTAInfo = new CTAInfo(TalentCTA.UpdateLegalClientOnBoardStatus, "Confirm legal Info", IsEnabled);
                            cTAInfos.Add(cTAInfo);
                        }
                        //Remove Auto assign TSC - UTS-8398
                        //if (!IsCancelledOnHoldInReplacementRejected && profile.OnBoardId > 0 &&
                        //    profile.ClientOnBoarding_StatusID == 2 && profile.TSC_PersonID == 0 &&
                        //    (profile.TalentOnBoarding_StatusID == 2 || profile.TalentOnBoarding_StatusID == 7) &&
                        //    profile.LegalClientOnBoarding_StatusID == 2 && profile.LegalTalentOnBoarding_StatusID == 2 &&
                        //    profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted)
                        //{
                        //    CTAInfo cTAInfo = new CTAInfo(TalentCTA.TSCAssignment, "Assign TSC", true);
                        //    cTAInfos.Add(cTAInfo);
                        //}
                        if (!RejectTalentCTAAlreadyAvailable)
                        {
                            if (profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Hired &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Cancelled &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Rejected &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.InReplacement &&
                            profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Offered)
                            {
                                CTAInfo cTAInfo = new CTAInfo(TalentCTA.TalentStatus, "Reject Talent", true);
                                cTAInfos.Add(cTAInfo);
                            }
                            if (profile.IsShownTalentStatus == 1)   //profile.TalentStatusID_BasedOnHR == (short)prg_TalentStatus_AfterClientSelection.Hired && 
                            {
                                CTAInfo cTAInfo = new CTAInfo(TalentCTA.CancelEngagement, "Cancel Engagement", true);
                                cTAInfos.Add(cTAInfo);
                            }
                        }
                        if ((profile.TalentStatusID_BasedOnHR == (short)prg_TalentStatus_AfterClientSelection.Offered ||
                            profile.TalentStatusID_BasedOnHR == (short)prg_TalentStatus_AfterClientSelection.Hired) &&
                            profile.OnBoardId > 0) //&& profile.Kickoff_StatusID == 5
                        {
                            CTAInfo cTAInfo = new CTAInfo(TalentCTA.ViewEngagement, "View Engagement", true);
                            cTAInfos.Add(cTAInfo);
                        }

                        //if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Won &&
                        //    !IsCancelledOnHoldInReplacementRejected &&
                        //    profile.NextRound_InterviewDetailsID == 0 && profile.ClientFeedback == "AnotherRound" &&
                        //    profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted)
                        //{
                        //    CTAInfo cTAInfo = new CTAInfo(TalentCTA.AnotherRoundInterview, "Another Round Interview", true);
                        //    cTAInfos.Add(cTAInfo);
                        //}
                        //if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Won && 
                        //    profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted &&
                        //    profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Cancelled &&
                        //    profile.TalentStatusID_BasedOnHR == (short)prg_TalentStatus_AfterClientSelection.OnHold &&
                        //    profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.InReplacement &&
                        //    profile.TalentStatusID_BasedOnHR != (short)prg_TalentStatus_AfterClientSelection.Rejected)
                        //{
                        //    //if (LoggedInUserTypeID == (short)ManagerType.TalentOps || LoggedInUserTypeID == (short)ManagerType.OpsTeamManager)
                        //    //    IsEnabled = false;
                        //    //else
                        //    //    IsEnabled = true;

                        //    CTAInfo cTAInfo = new CTAInfo(TalentCTA.EditClientFeedback, "Edit Client Feedback", IsEnabled);
                        //    cTAInfos.Add(cTAInfo);
                        //}
                        if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Won &&
                            !IsCancelledOnHoldInReplacementRejected &&
                            profile.SlotGivenStatus == "Later" && profile.ClientFeedback == "AnotherRound" &&
                            profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted)
                        {
                            CTAInfo cTAInfo = new CTAInfo(TalentCTA.AnotherRoundInterview, "Another Round Interview", true);
                            cTAInfos.Add(cTAInfo);
                        }
                        else if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Won &&
                            !IsCancelledOnHoldInReplacementRejected &&
                            profile.NextRound_InterviewDetailsID == 0 && profile.ClientFeedback == "AnotherRound" &&
                            profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted)
                        {
                            CTAInfo cTAInfo = new CTAInfo(TalentCTA.AnotherRoundInterview, "Another Round Interview", true);
                            cTAInfos.Add(cTAInfo);
                        }
                        else
                        {
                            if (hRDetail.HRStatusID != (short)prg_HiringRequestStatus.Won &&
                                (profile.InterViewStatusId == 0 ||
                            profile.InterViewStatusId == (short)prg_InterviewStatus.Cancelled ||
                            profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted) &&
                            profile.TalentStatusID_BasedOnHR == (short)prg_TalentStatus_AfterClientSelection.ProfileShared)
                            {
                                //if (LoggedInUserTypeID == (short)ManagerType.TalentOps || LoggedInUserTypeID == (short)ManagerType.OpsTeamManager)
                                //    IsEnabled = false;
                                //else
                                //    IsEnabled = true;

                                CTAInfo cTAInfo = new CTAInfo(TalentCTA.ScheduleInterview, "Schedule Interview", IsEnabled);
                                cTAInfos.Add(cTAInfo);
                            }
                        }
                        //if (profile.TalentStatusID_BasedOnHR == (short)prg_TalentStatus_AfterClientSelection.Offered ||
                        //    profile.TalentStatusID_BasedOnHR == (short)prg_TalentStatus_AfterClientSelection.Hired 
                        //    && profile.OnBoardId > 0)
                        //{
                        //    CTAInfo cTAInfo = new CTAInfo(TalentCTA.ReplaceTalent, "Replace Talent", true);
                        //    cTAInfos.Add(cTAInfo);
                        //}
                        //if (!IsCancelledOnHoldInReplacementRejected && profile.OnBoardId > 0 && profile.IsAMAssigned != 0 &&
                        //    profile.ClientOnBoarding_StatusID != 2 && profile.ClientOnBoarding_StatusID == 8 &&
                        //    profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted)
                        //{
                        //    //if (LoggedInUserTypeID == (short)ManagerType.TalentOps || LoggedInUserTypeID == (short)ManagerType.OpsTeamManager)
                        //    //    IsEnabled = false;
                        //    //else
                        //    //    IsEnabled = true;

                        //    CTAInfo cTAInfo = new CTAInfo(TalentCTA.UpdateClientOnBoardStatus, "Update Client On Board Status", IsEnabled);
                        //    cTAInfos.Add(cTAInfo);
                        //}
                        //if (!IsCancelledOnHoldInReplacementRejected && profile.OnBoardId > 0 &&
                        //    profile.ClientOnBoarding_StatusID == 2 && profile.Kickoff_StatusID != 5 &&
                        //    (profile.TalentOnBoarding_StatusID == 2 || profile.TalentOnBoarding_StatusID == 7) &&
                        //    profile.LegalClientOnBoarding_StatusID == 2 && profile.LegalTalentOnBoarding_StatusID != 2 &&
                        //    profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted)
                        //{
                        //    //if (LoggedInUserTypeID == (short)ManagerType.Sales || LoggedInUserTypeID == (short)ManagerType.SalesManager)
                        //    //    IsEnabled = false;
                        //    //else
                        //    //    IsEnabled = true;

                        //    CTAInfo cTAInfo = new CTAInfo(TalentCTA.UpdateLegalTalentOnBoardStatus, "Update Legal Talent On Board Status", IsEnabled);
                        //    cTAInfos.Add(cTAInfo);
                        //}
                        //if (!IsCancelledOnHoldInReplacementRejected && profile.OnBoardId > 0 &&
                        //    profile.ClientOnBoarding_StatusID == 2 && profile.Kickoff_StatusID != 5 &&
                        //    (profile.TalentOnBoarding_StatusID == 2 || profile.TalentOnBoarding_StatusID == 7) &&
                        //    profile.LegalClientOnBoarding_StatusID == 2 && profile.LegalTalentOnBoarding_StatusID == 2 &&
                        //    profile.InterViewStatusId == (short)prg_InterviewStatus.Feedback_Submitted)
                        //{
                        //    CTAInfo cTAInfo = new CTAInfo(TalentCTA.UpdateKickOffOnBoardStatus, "Update Kick Off On Board Status", true);
                        //    cTAInfos.Add(cTAInfo);
                        //}
                    }

                    #endregion

                    cTA.cTAInfoList = new List<CTAInfo>();
                    if (cTAInfos.Any())
                        cTA.cTAInfoList.AddRange(cTAInfos);

                    talent_CTAs.Add(cTA);
                }

                hRDetail.dynamicCTA.talent_CTAs = talent_CTAs;
            }
            #endregion

            return hRDetail;
        }

        public NotesViewModel SaveHRNotes(NotesViewModel notesViewModel, string userEmployeeID)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            long newNoteId = 0;
            GenHrnote hRNotes = new GenHrnote(); UsrUser LoggedInUser = new UsrUser();
            LoggedInUser = db.UsrUsers.ToList().Where(xy => xy.EmployeeId == userEmployeeID).FirstOrDefault();

            if (notesViewModel.HiringRequest_ID != null && notesViewModel.hdnNotes != null)
            {
                hRNotes.HiringRequestId = notesViewModel.HiringRequest_ID;
                hRNotes.Notes = notesViewModel.hdnNotes;
                hRNotes.CreatedById = (int)LoggedInUser.Id;
                hRNotes.CreatedByDateTime = DateTime.Parse(date);
                db.GenHrnotes.Add(hRNotes);
                db.SaveChanges();

                if (hRNotes.Id > 0)
                    newNoteId = hRNotes.Id;
            }
            if (notesViewModel.hdnUserValues != null && notesViewModel.hdnUserValues != "")
            {
                List<UserOptionVM> Userdata = new List<UserOptionVM>();
                for (int i = 0; i < notesViewModel.hdnUserValues.Split(',').Count(); i++)
                {
                    UserOptionVM user = new UserOptionVM();
                    user.ID = notesViewModel.hdnUserValues.Split(',')[i];
                    if (user.ID != "" || !string.IsNullOrEmpty(user.ID))
                    {
                        long TaggedUserID = 0;
                        bool isValidUserID = long.TryParse(user.ID, out TaggedUserID);
                        if (isValidUserID == true && TaggedUserID > 0)
                        {
                            var CheckValidUser = db.UsrUsers.FirstOrDefault(x => x.Id == TaggedUserID);
                            if (CheckValidUser != null)
                                Userdata.Add(user);
                        }

                    }
                }
                db.GenHrnotesTagUserDetails.AddRange(Userdata.Select(y => new GenHrnotesTagUserDetail { NoteId = newNoteId, AssignedUserId = Convert.ToInt32(y.ID) }));
                db.SaveChanges();
            }

            #region Data Sync to Firebased (Update Channel Status)

            //if (_configuration["UpChatAPI_IsEnabled"].ToString().ToLower().Equals("true"))
            //{
            //    try
            //    {
            //        long HrId = (long)notesViewModel.HiringRequest_ID;
            //        long NoteID = newNoteId;
            //        _iUpChatCall.AddNotesInChats(HrId, NoteID);
            //    }
            //    catch
            //    {
            //        return notesViewModel;
            //    }
            //}
            #endregion

            return notesViewModel;
        }
        public NotesViewModel UpChatSaveHRNotes(NotesViewModel notesViewModel, string userEmployeeID)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            long newNoteId = 0;
            GenHrnote hRNotes = new GenHrnote(); UsrUser LoggedInUser = new UsrUser();
            LoggedInUser = db.UsrUsers.ToList().Where(xy => xy.EmployeeId == userEmployeeID).FirstOrDefault();

            if (notesViewModel.HiringRequest_ID != null && notesViewModel.hdnNotes != null)
            {
                hRNotes.HiringRequestId = notesViewModel.HiringRequest_ID;
                hRNotes.Notes = notesViewModel.hdnNotes;
                hRNotes.CreatedById = (int)LoggedInUser.Id;
                hRNotes.CreatedByDateTime = DateTime.Parse(date);
                db.GenHrnotes.Add(hRNotes);
                db.SaveChanges();

                if (hRNotes.Id > 0)
                    newNoteId = hRNotes.Id;
            }

            return notesViewModel;
        }
        public async Task<HRFilterListViewModel> GetFiltersLists()
        {
            HRFilterListViewModel model = new()
            {
                Managers = await db.UsrUsers.Where(x => x.IsActive == true && x.UserTypeId == 9).Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.FullName
                }).OrderBy(x => x.Value).ToListAsync(),

                //Companies = db.GenCompanies.Where(x => x.IsActive == true).Select(y => new SelectListItem
                //{
                //    Text = y.Id.ToString(),
                //    Value = y.Company
                //}).OrderBy(x => x.Value).ToList(),


                Positions = await db.PrgTalentRoles.Where(x => x.IsActive == true).OrderBy(x => x.TalentRole).Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.TalentRole
                }).OrderBy(x => x.Value).ToListAsync(),


                SalesReps = await db.UsrUsers.Where(x => x.IsActive == true && x.UserTypeId == 4).Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.FullName
                }).OrderBy(x => x.Value).ToListAsync(),


                HRTypes = await db.PrgHiringRequestTypes.
                Where(x => x.IsActive == true).
                Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.Name
                }).OrderBy(y => y.Value).ToListAsync(),

                LeadTypeList = await db.UsrUsers.Where(x => x.DeptId == 1 && x.LevelId == 1 && (x.UserTypeId == 11 || x.UserTypeId == 12) && x.IsActive == true).Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = (x.UserTypeId == 12 ? "InBound" : "OutBound") + " - " + Convert.ToString(x.FullName)
                }).OrderBy(y => y.Value).ToListAsync(),

                CompanyModel = await db.PrgCompanyTypes.Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.CompanyType
                }).OrderBy(y => y.Value).ToListAsync(),

                HrStatusList = await db.PrgHrStatusDisplays.Where(x => x.IsActive == true).ToListAsync(),

                GeoList = await db.PrgGeos.Select(y => new SelectListItem
                {
                    Text = y.Id.ToString(),
                    Value = y.Geo
                }).OrderBy(y => y.Value).ToListAsync()
            };

            return model;
        }

        public GenSalesHiringRequestPriority GetHRPriorityData(long hRId)
        {
            return db.GenSalesHiringRequestPriorities.Where(x => x.Id == hRId || x.HiringRequestId == hRId).FirstOrDefault();
        }

        public GenSalesHiringRequest GetHRData(long hRId)
        {
            return db.GenSalesHiringRequests.AsNoTracking().Where(x => x.Id == hRId).FirstOrDefault();
        }

        public long HiringRequestPriority(string param)
        {
            //db.Database.ExecuteSqlRaw(string.Format("{0} {1}",Constants.ProcConstant.sproc_UTS_Insert_HiringRequest_PriorityHistory, param));
            var result = db.Set<sproc_UTS_HR_PrioritySet>().FromSqlRaw($"{Constants.ProcConstant.sproc_UTS_Insert_HiringRequest_PriorityHistory} " + $"{param} ").ToList().FirstOrDefault();

            return result.Value;

            //return 1;
        }

        public sp_get_HRClientDetails_Result GetClientCompanyDetail(long HiringRequest_ID)
        {
            return db.Set<sp_get_HRClientDetails_Result>().FromSqlRaw($"{Constants.ProcConstant.sp_UTS_get_HRClientDetails} " + $"{HiringRequest_ID} ").ToList().FirstOrDefault();
        }

        public GenSalesHiringRequest SaveSalesHiringRequest(GenSalesHiringRequest genSalesHiringRequest)
        {
            if (genSalesHiringRequest != null && genSalesHiringRequest.Id > 0)
            {
                db.Entry(genSalesHiringRequest).State = EntityState.Modified;
                db.SaveChanges();
            }
            return genSalesHiringRequest;
        }

        public GenSalesHrTracceptedDetail SaveSalesHRTracceptedDetail(GenSalesHrTracceptedDetail genSalesHrTracceptedDetail)
        {
            if (genSalesHrTracceptedDetail != null)
            {
                db.Add(genSalesHrTracceptedDetail);
                //db.Entry(genSalesHrTracceptedDetail).State = EntityState.Modified;
                db.SaveChanges();
            }
            return genSalesHrTracceptedDetail;
        }

        public string SaveHiringRequestHistory(string Action, long HiringRequest_ID, int Talent_ID, bool Created_From, long CreatedById, int ContactTalentPriority_ID, int InterviewMaster_ID, string HR_AcceptedDateTime, int OnBoard_ID, bool IsManagedByClient, bool IsManagedByTalent)
        {
            object[] param = new object[]
            {
                Action, HiringRequest_ID, Talent_ID, Created_From, CreatedById, ContactTalentPriority_ID,
                InterviewMaster_ID, HR_AcceptedDateTime, OnBoard_ID, IsManagedByClient, IsManagedByTalent, 0, 0, (short)AppActionDoneBy.UTS
            };

            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);

            return "Success";
        }

        public void sproc_UTS_gen_ContactTalentPriorityupdate(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_gen_ContactTalentPriorityupdate, param));
        }

        public List<Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion_Result> Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion(string param)
        {
            return db.Set<Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion, param)).ToList();
        }

        public void sproc_UTS_UpdateDPAmountDetails(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_UpdateDPAmountDetails, param));
        }

        public List<Sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion_Result> sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion(string param)
        {
            return db.Set<Sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion, param)).ToList();
        }

        public void sproc_UTS_UpdateIsHrtypedpandDPAmount(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_UpdateIsHrtypedpandDPAmount, param));
        }

        public CheckValidationMessage Sproc_Get_Validation_Message_For_Priority(long loggedInUser)
        {
            return db.Set<CheckValidationMessage>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Validation_Message_For_Priority, loggedInUser)).AsEnumerable().FirstOrDefault();
        }

        public void InsertHiringRequestPriorityReviseFlow(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Insert_HiringRequest_Priority_Revise_Flow, param));
        }

        public async Task<List<Sproc_Get_Priority_Set_Remaining_Count_Details_Result>> GetPrioritySetRemainingCountDetailsResult(long logingUserId)
        {
            return await db.Set<Sproc_Get_Priority_Set_Remaining_Count_Details_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Priority_Set_Remaining_Count_Details, logingUserId)).ToListAsync();
        }

        public async Task<List<Sproc_Check_Validation_Message_For_User_Edit_TR_Result>> Get_Sproc_Check_Validation_Message_For_User_Edit_TR(string param)
        {
            return await db.Set<Sproc_Check_Validation_Message_For_User_Edit_TR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Check_Validation_Message_For_User_Edit_TR, param)).ToListAsync().ConfigureAwait(false);
        }
        public async Task<List<Sproc_UpdateTR_Result>> Get_Sproc_UpdateTR(string param)
        {
            return await db.Set<Sproc_UpdateTR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UpdateTR, param)).ToListAsync().ConfigureAwait(false);
        }
        public Sproc_UTS_FetchUsersWithSpecialEditAccess_Result CheckSpecialEdits(string param)
        {
            return db.Set<Sproc_UTS_FetchUsersWithSpecialEditAccess_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_FetchUsersWithSpecialEditAccess, param)).AsEnumerable().FirstOrDefault();
        }


        // -----SLA details, Update, and history----

        public async Task<List<sproc_Get_HiringRequest_SLADetails_Result>> Get_HiringRequest_SLADetails(string param)
        {
            return await db.Set<sproc_Get_HiringRequest_SLADetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Get_HiringRequest_SLADetails, param)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<SelectListItem>?> GetSLAEditReasons()
        {
            return await db.PrgSlaEditReasons.Where(r => r.IsActive == true).Select(x => new SelectListItem { Text = x.Reason, Value = x.Id.ToString() }).ToListAsync();
        }

        public void Sproc_Update_SLADate(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_SLADate, param));
        }

        public async Task<List<Sproc_Get_SLAUpdate_History_Result>> Get_SLAUpdate_History(string param)
        {
            return await db.Set<Sproc_Get_SLAUpdate_History_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_SLAUpdate_History, param)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Sproc_Get_SLAUpdateDetails_ForEmail_Result> Sproc_Get_SLAUpdateDetails_ForEmail(string param)
        {
            return db.Set<Sproc_Get_SLAUpdateDetails_ForEmail_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_SLAUpdateDetails_ForEmail, param)).AsEnumerable().FirstOrDefault();
        }

        public Sp_UTS_GetCreditUtilization_BasedOnHR_Result GetCreditUtilization_BasedOnHR(long HRID)
        {
            return db.Set<Sp_UTS_GetCreditUtilization_BasedOnHR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sp_UTS_GetCreditUtilization_BasedOnHR, HRID)).AsEnumerable().FirstOrDefault();
        }



        public List<sproc_ViewAllUnAssignedHRs_Result> GetAllUnAssignedHRs(ViewAllUnAssignedHRViewModel viewAllUnAssignedHRViewModel, int loggedInUserTypeId, long LoggedInUserID)
        {
            string sidx = (string.IsNullOrEmpty(viewAllUnAssignedHRViewModel.Sortdatafield)) ? "CreatedDateTime" : viewAllUnAssignedHRViewModel.Sortdatafield;
            string sord = (string.IsNullOrEmpty(viewAllUnAssignedHRViewModel.Sortorder)) ? "desc" : viewAllUnAssignedHRViewModel.Sortorder;

            string fromDate = "";
            string toDate = "";


            int pageIndex = viewAllUnAssignedHRViewModel.Pagenum;
            int pageSize = viewAllUnAssignedHRViewModel.Pagesize;



            if (viewAllUnAssignedHRViewModel.FilterFields_ViewAllUnAssignedHRs != null && !string.IsNullOrEmpty(viewAllUnAssignedHRViewModel.FilterFields_ViewAllUnAssignedHRs.fromDate) && !string.IsNullOrEmpty(viewAllUnAssignedHRViewModel.FilterFields_ViewAllUnAssignedHRs.toDate))
            {
                fromDate = CommonLogic.ConvertString2DateTime(viewAllUnAssignedHRViewModel.FilterFields_ViewAllUnAssignedHRs.fromDate).ToString("yyyy-MM-dd");
                toDate = CommonLogic.ConvertString2DateTime(viewAllUnAssignedHRViewModel.FilterFields_ViewAllUnAssignedHRs.toDate).ToString("yyyy-MM-dd");
            }

            viewAllUnAssignedHRViewModel.FilterFields_ViewAllUnAssignedHRs ??= new();

            //Trim the blank spaces from the end of the search text.
            //Pick only first 200 characters.
            //Replace the single quotes with double quotes if any.
            string searchText = string.Empty;
            if (!string.IsNullOrEmpty(viewAllUnAssignedHRViewModel.searchText))
            {
                searchText = viewAllUnAssignedHRViewModel.searchText.TrimStart().TrimEnd();
                if (searchText.Length > 200)
                {
                    searchText = searchText.Substring(0, 200);
                }
                searchText = searchText.Replace('\'', '\"');
            }


            var AllHRListData = db.Set<sproc_ViewAllUnAssignedHRs_Result>().FromSqlRaw(@$"{Constants.ProcConstant.sproc_UTS_GetAllUnAssignedHRs}  
                                {pageIndex}, {pageSize}, '{sidx}', '{sord}', {loggedInUserTypeId}, {LoggedInUserID} 
                                , '{fromDate}', '{toDate}', '{searchText}' ");

            return AllHRListData.ToList();
        }

        public void sproc_AssignedPOCToUnAssignedHRs(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_AssignedPOCToUnAssignedHRs, param));
        }

        public sp_UTS_TalentCalculation_PayPerHire_Result GetTalentLevelCalculation(string param)
        {
            return db.Set<sp_UTS_TalentCalculation_PayPerHire_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sp_UTS_TalentCalculation_PayPerHire, param)).AsEnumerable().FirstOrDefault();
        }

        public async Task<object> GetAllHRDataForAdmin(long HRID)
        {
            string HRData = "";

            if (HRID == 0)
            {
                var table = db.Set<Sproc_GET_ALL_HR_Details_For_PHP_API_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.Sproc_GET_ALL_HR_Details_For_PHP_API} " + $"{null}, {true}")
                                .ToList();

                if (table.Count == 0)
                {
                    return string.Empty;
                }

                HRData = JsonConvert.SerializeObject(table);
                return HRData;

            }
            else
            {
                var table = db.Set<Sproc_GET_ALL_HR_Details_For_PHP_API_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.Sproc_GET_ALL_HR_Details_For_PHP_API} " + $"{HRID}, {false}")
                                .ToList().AsEnumerable().FirstOrDefault();

                if (table == null)
                {
                    return "";
                }

                if (!string.IsNullOrEmpty(table.JDFilename))
                {
                    table.JDPath = System.IO.Path.Combine($"{_httpContextAccessor.HttpContext.Request.Scheme}:", _httpContextAccessor.HttpContext.Request.Host.Value, "Media", "JDParsing", "JDFiles", table.JDFilename);
                }

                //HRData = JsonConvert.SerializeObject(table);
                HR_Data _data = new HR_Data();

                HRDetailWithSkills hRDetailWithSkills = new HRDetailWithSkills();
                hRDetailWithSkills.HR_Details = table;
                var HR_Skills_Details = db.Set<sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI} " + $" {HRID} ")
                                .ToList();

                foreach (var item in HR_Skills_Details)
                {
                    hRDetailWithSkills.skill.Add(new SkillDetail
                    {
                        Proficiency = item.Proficiency,
                        SkillName = item.Skill,
                        TempSkillName = item.TempSkill
                    });
                }

                var HR_InterviewerDetails = db.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == HRID).ToList();
                foreach (var item in HR_InterviewerDetails)
                {
                    hRDetailWithSkills.HR_InterviewerDetails.Add(new HR_InterviewerDetail
                    {
                        ID = item.Id,
                        HiringRequestID = item.HiringRequestId ?? 0,
                        InterviewerDesignation = item.InterviewerDesignation,
                        InterviewerEmailID = item.InterviewerEmailId,
                        InterviewerLinkedIn = item.InterviewLinkedin,
                        InterviewerName = item.InterviewerName,
                        InterviewerType = db.PrgCompanyTypeofInterviewers.Where(x => x.Id == item.TypeofInterviewerId).FirstOrDefault() == null ? ""
                                                            : db.PrgCompanyTypeofInterviewers.Where(x => x.Id == item.TypeofInterviewerId).FirstOrDefault().TypeofInterviewer,
                        InterviewerTypeID = item.TypeofInterviewerId ?? 0,
                        InterviewerYearsOfExp = item.InterviewerYearofExperience ?? 0
                    });
                }

                #region Vital info
                try
                {
                    VitalInfo vitalInfo = new VitalInfo();

                    string? compensationOptionStr = table.CompensationOption;
                    string[] compensationOption = new string[1000];

                    if (compensationOptionStr != null)
                    {
                        compensationOption = compensationOptionStr.Split('^');
                        vitalInfo.CompensationOption = compensationOption;
                    }

                    string? candidateIndustryStr = table.CandidateIndustry;
                    string[] candidateIndustry = new string[1000];
                    if (candidateIndustryStr != null)
                    {
                        candidateIndustry = candidateIndustryStr.Split('^');
                        vitalInfo.CandidateIndustry = candidateIndustry;
                    }

                    vitalInfo.Prerequisites = table.Prerequisites;
                    vitalInfo.HasPeopleManagementExp = table.HasPeopleManagementExp;

                    _data.VitalInformation = vitalInfo;
                }
                catch
                {

                }
                #endregion

                #region Send HRPOC to ATS.
                try
                {
                    object[] pocParam = new object[] { 0, "", HRID, "", 0, false };
                    string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                    List<Sproc_HR_POC_ClientPortal_Result> hrPOCs = db.Set<Sproc_HR_POC_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_HR_POC_ClientPortal, pocParamString)).AsEnumerable().ToList();

                    List<HRPOC> hRPOCs = new List<HRPOC>();
                    foreach (var i in hrPOCs)
                    {
                        HRPOC hRPOC = new HRPOC();
                        hRPOC.EmailID = i.EmailID;
                        hRPOC.FullName = i.FullName;
                        hRPOC.ContactNo = i.ContactNo;
                        hRPOCs.Add(hRPOC);
                    }
                    _data.HRPOC = hRPOCs;
                }
                catch
                {

                }
                #endregion

                _data.HRData = hRDetailWithSkills;
                _data.Status = "200";


                return JsonConvert.SerializeObject(_data);
            }
        }
        // -----SLA details, Update, and history----

        public List<sp_UTS_get_HRHistory_UsingPagination_Result> sp_UTS_get_HRHistory_UsingPagination_Result(string param)
        {
            return db.Set<sp_UTS_get_HRHistory_UsingPagination_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sp_UTS_get_HRHistory_UsingPagination, param)).ToList();
        }

        public List<sp_UTS_get_HRTalentDetails_UsingPagination_Result> sp_UTS_get_HRTalentDetails_UsingPagination_Result(string param)
        {
            return db.Set<sp_UTS_get_HRTalentDetails_UsingPagination_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sp_UTS_get_HRTalentDetails_UsingPagination, param)).ToList();
        }

        public Sproc_EmailHRTypeChanged_Result Sproc_EmailHRTypeChanged(string param)
        {
            return db.Set<Sproc_EmailHRTypeChanged_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_EmailHRTypeChanged, param)).AsEnumerable().FirstOrDefault();
        }
        #endregion
    }
}
