using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Dynamic;
using UTSATSAPI.ATSCalls;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Models.ViewModels.Validators;
using UTSATSAPI.Repositories.Interfaces;
using static UTSATSAPI.Helpers.Constants;
using static UTSATSAPI.Helpers.Enum;

namespace UTSATSAPI.Controllers
{
    [Authorize]
    [Route("OnBoard/", Name = "OnBoard")]
    [ApiController]
    public class OnBoardController : ControllerBase
    {
        #region Variables
        private readonly ICommonInterface commonInterface;
        private readonly TalentConnectAdminDBContext db;
        private readonly IConfiguration configuration;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly IUsers _iUsers;
        private readonly IOnboard _iOnboard;
        private readonly IAMAssignment _iAMAssignment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEngagement _iEngagement;
        private readonly ITalentReplacement _iTalentReplacement;
        #endregion

        #region Constructors
        public OnBoardController(ICommonInterface _commonInterface, IConfiguration _iConfiguration,
            TalentConnectAdminDBContext talentConnectAdminDBContext, IUniversalProcRunner universalProcRunner,
            IUsers iUsers, IOnboard iOnboard, IAMAssignment iAMAssignment, IHttpContextAccessor httpContextAccessor, IEngagement iEngagement, ITalentReplacement iTalentReplacement)
        {
            commonInterface = _commonInterface;
            configuration = _iConfiguration;
            db = talentConnectAdminDBContext;
            _universalProcRunner = universalProcRunner;
            _iUsers = iUsers;
            _iOnboard = iOnboard;
            _iAMAssignment = iAMAssignment;
            _httpContextAccessor = httpContextAccessor;
            _iEngagement = iEngagement;
            _iTalentReplacement = iTalentReplacement;
        }
        #endregion

        #region Public APIs

        [HttpPost("OnBoardTalent")]
        public ObjectResult OnBoardTalent(OnBoardViewModel onBoardViewModel, long LoggedInUserId = 0)
        {
            LoggedInUserId = SessionValues.LoginUserId;

            #region PreValidation
            if (onBoardViewModel == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please check the datatype or naming convention" });
            #endregion

            #region Validation
            OnBoardViewModelValidators validationRules = new OnBoardViewModelValidators();
            ValidationResult validationResult_companyValidator = validationRules.Validate(onBoardViewModel);
            if (!validationResult_companyValidator.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult_companyValidator.Errors, "OnBoard") });
            }
            #endregion

            #region Update
            if (onBoardViewModel.OnboardID > 0)
            {
                GenOnBoardTalent genOnBoardTalent = db.GenOnBoardTalents.FirstOrDefault(x => x.Id == onBoardViewModel.OnboardID);
                GenOnBoardClientContractDetail onBoardClientContractDetail = new GenOnBoardClientContractDetail();

                onBoardClientContractDetail = ModelBinder.BindGenOnBoardClientContractDetail(onBoardViewModel);

                if (!string.IsNullOrEmpty(onBoardViewModel.ProceedWithClient_LeavePolicyOption))
                {
                    onBoardClientContractDetail.ProceedWithClientLeavePolicyOption = onBoardViewModel.ProceedWithClient_LeavePolicyOption;
                    var InsertedLeave = db.GenOnBoardClientLeavePolicies.Where(p => p.OnBoardId == onBoardViewModel.OnboardID).ToList();
                    if (InsertedLeave != null)
                    {
                        db.GenOnBoardClientLeavePolicies.RemoveRange(InsertedLeave);
                    }
                    onBoardClientContractDetail.ProceedWithClientLeavePolicyFileUpload = "";
                    onBoardClientContractDetail.ProceedWithClientLeavePolicyLink = "";

                    //if (onBoardViewModel["hdnRadioLeavePolicies"] == "Upload Your Policies")
                    //{
                    //    if (!string.IsNullOrEmpty(formCollection["hdnRadioYourLeavePolicies"]))
                    //    {
                    //        onBoardClientContractDetails.ProceedWithClient_LeavePolicyOption = formCollection["hdnRadioYourLeavePolicies"];
                    //        if (formCollection["hdnRadioYourLeavePolicies"] == "Leave Policy")
                    //        {
                    //            List<gen_OnBoardClientLeavePolicy> OnBoardClientLeavePolicies = new List<gen_OnBoardClientLeavePolicy>();
                    //            if (Session["OnBoardClientLeavePolicy"] != null)
                    //            {
                    //                OnBoardClientLeavePolicies = (List<gen_OnBoardClientLeavePolicy>)Session["OnBoardClientLeavePolicy"];
                    //            }
                    //            foreach (var LeavePolicies in OnBoardClientLeavePolicies)
                    //            {
                    //                gen_OnBoardClientLeavePolicy LeavePolicydata = new gen_OnBoardClientLeavePolicy();
                    //                LeavePolicydata.OnBoardID = OnBoardId;
                    //                LeavePolicydata.Occasion = LeavePolicies.Occasion;
                    //                LeavePolicydata.Day = LeavePolicies.Day;
                    //                LeavePolicydata.Date = LeavePolicies.Date;
                    //                LeavePolicydata.IsActive = true;
                    //                db.gen_OnBoardClientLeavePolicy.Add(LeavePolicydata);
                    //                db.SaveChanges();
                    //            }
                    //            Session["OnBoardClientLeavePolicy"] = null;
                    //        }
                    //        else if (formCollection["hdnRadioYourLeavePolicies"] == "Upload Client Leave Policy")
                    //        {
                    //            onBoardClientContractDetails.ProceedWithClient_LeavePolicyFileUpload = formCollection["LeavePolicyFileName"];
                    //        }
                    //        else if (formCollection["hdnRadioYourLeavePolicies"] == "Paste Link")
                    //        {
                    //            onBoardClientContractDetails.ProceedWithClient_LeavePolicyLink = formCollection["LeavePolicyPasteLinkName"];
                    //        }
                    //    }
                    //}
                }

                #region Adding TeamMembers 
                if (onBoardViewModel.TeamMemebers.Count > 0)
                {
                    foreach (var team in onBoardViewModel.TeamMemebers)
                    {
                        GenOnBoardClientTeam OnBoardClientTeamdata = new GenOnBoardClientTeam();
                        OnBoardClientTeamdata.OnBoardId = onBoardViewModel.OnboardID;
                        OnBoardClientTeamdata.Name = team.Name;
                        OnBoardClientTeamdata.Designation = team.Designation;
                        OnBoardClientTeamdata.Email = team.Email;
                        OnBoardClientTeamdata.Linkedin = team.Linkedin;
                        OnBoardClientTeamdata.ReportingTo = team.ReportingTo;
                        if (team.Buddy == "1" || team.Buddy == "Buddy")
                        {
                            OnBoardClientTeamdata.Buddy = "Buddy";
                        }
                        else
                        {
                            OnBoardClientTeamdata.Buddy = "Select Later";
                        }
                        OnBoardClientTeamdata.IsActive = true;
                        db.GenOnBoardClientTeams.Add(OnBoardClientTeamdata);
                        db.SaveChanges();
                    }
                }
                #endregion

                onBoardClientContractDetail.CreatedById = Convert.ToInt32(LoggedInUserId);

                //UTS-3991: Update the Contract details information when onboarding is completed.
                object[] param = new object[]
               {
                  onBoardClientContractDetail.OnBoardId,
                  onBoardClientContractDetail.ContractType,
                  onBoardClientContractDetail.ContractDuration,
                  onBoardViewModel.ContractStartDate,
                  onBoardViewModel.ContractEndDate,
                  onBoardClientContractDetail.TotalDurationInMonths,
                  onBoardClientContractDetail.PunchStartTime,
                  onBoardClientContractDetail.PunchEndTime,
                  onBoardClientContractDetail.WorkingDays,
                  onBoardViewModel.TalentWorkingTimeZone,
                  onBoardViewModel.FirstClientBillingDate,
                  onBoardClientContractDetail.NetPaymentDays,
                  onBoardClientContractDetail.ContractRenewalSlot,
                  onBoardViewModel.DevicesPoliciesOption,
                  onBoardClientContractDetail.TalentDeviceDetails,
                  Convert.ToDecimal(onBoardClientContractDetail.AdditionalCostPerMonthRdpsecurity),
                  Convert.ToDecimal(onBoardClientContractDetail.DeviceCostAsPerPolicy),
                  onBoardViewModel.ExpectationFromTalent_FirstWeek,
                  onBoardViewModel.ExpectationFromTalent_FirstMonth,
                  onBoardViewModel.Client_Remark,
                  onBoardViewModel.ProceedWithUplers_LeavePolicyOption,
                  onBoardViewModel.ProceedWithUplers_ExitPolicyOption,
                  onBoardViewModel.ProceedWithClient_LeavePolicyLink,
                  onBoardViewModel.ProceedWithClient_LeavePolicyFileUpload,
                  onBoardViewModel.ProceedWithClient_LeavePolicyOption,
                  onBoardClientContractDetail.CreatedById,
                  onBoardViewModel.TalentOnBoardDate,
                  Convert.ToString(onBoardClientContractDetail.TalentOnBoardTime),
                  onBoardClientContractDetail.TimezonePreferenceId
               };

                string paramasString = CommonLogic.ConvertToParamString(param);
                _iOnboard.Sproc_Add_OnBoardClientContractDetails(paramasString);

                var OnBoard_Detail = db.GenOnBoardClientContractDetails.FirstOrDefault(x => x.Id == onBoardViewModel.OnboardID);
                if (OnBoard_Detail != null)
                {
                    var ContractEndDate = OnBoard_Detail.ContractEndDate;
                    if (!string.IsNullOrEmpty(onBoardViewModel.ContractEndDate))
                    {
                        var NewContractEnddate = CommonLogic.ConvertString2DateTimeFormtatAdmin(onBoardViewModel.ContractEndDate);
                        if (ContractEndDate != NewContractEnddate)
                        {
                            ArchiveInvoice replacementData = new ArchiveInvoice();
                            var ReplacementInvoiceIDs = db.GenPayoutInformations.Where(x => x.OnBoardId == onBoardViewModel.OnboardID && x.EsalesInvoiceDate >= ContractEndDate).ToList();

                            if (ReplacementInvoiceIDs != null)
                            {
                                replacementData.invoiceId = new List<InvoiceIds>();
                                foreach (var item in ReplacementInvoiceIDs)
                                {
                                    InvoiceIds invoiceIds = new InvoiceIds();
                                    var InvoiceId = item.EsalesInvoiceId ?? 0;
                                    invoiceIds.ID = InvoiceId;
                                    replacementData.invoiceId.Add(invoiceIds);
                                }

                                replacementData.ArchiveReason = "Talent Is Replaced";

                                try
                                {
                                    ATSCall aTSCall = new ATSCall(configuration, db);
                                    var ApiResponse = aTSCall.SendReplacementDataToInvoice(replacementData);
                                    if (!string.IsNullOrEmpty(ApiResponse))
                                    {
                                        ArchiveResponseObject archiveResponseObject = JsonConvert.DeserializeObject<ArchiveResponseObject>(ApiResponse);
                                        if (archiveResponseObject != null)
                                        {
                                            if (archiveResponseObject.Result)
                                            {
                                                foreach (var item in archiveResponseObject.Data)
                                                {
                                                    var InvoiceIdUpdate = db.GenPayoutInformations.FirstOrDefault(x => x.EsalesInvoiceId == item.InvoiceID && x.OnBoardId == onBoardViewModel.OnboardID);
                                                    if (InvoiceIdUpdate != null)
                                                    {
                                                        InvoiceIdUpdate.StatusId = item.StatusID;
                                                        CommonLogic.DBOperator(db, InvoiceIdUpdate, EntityState.Modified);
                                                    }
                                                    var OnBoardTalents = db.GenOnBoardTalents.FirstOrDefault(x => x.Id == onBoardViewModel.OnboardID);
                                                    if (OnBoardTalents != null)
                                                    {
                                                        EmailBinder emailBinder = new EmailBinder(configuration, db);
                                                        emailBinder.SendEmailOnArchiveInvoice(item.InvoiceID, onBoardViewModel.OnboardID, OnBoardTalents.TalentId ?? 0, false);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "OnBoard Talent Updated" });
                                }

                            }
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "OnBoard Talent Updated" });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide OnBoard id" });

            }
            #endregion

        }

        #region Client OnBoarding First & Second Tab API

        [HttpGet]
        [Route("GetPreOnBoardingDetailForAMAssignment")]
        public async Task<ObjectResult> GetPreOnBoardingDetailForAMAssignment(long? OnBoardId, long? HR_ID)
        {
            try
            {
                AMAssignmentPreOnBoardingDetailsViewModel AMAssignmentViewModel = new();
                GenSalesHiringRequest? _SalesHiringRequest = new GenSalesHiringRequest();
                long LoggedInUserId = SessionValues.LoginUserId;

                #region PreValidation

                if (HR_ID == 0 || HR_ID == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = db.GenSalesHiringRequests.Where(x => x.Id == HR_ID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }

                    // Added by Riya
                    AMAssignmentViewModel.IsTransparentPricing = _SalesHiringRequest.IsTransparentPricing;
                }

                if (OnBoardId == 0 || OnBoardId == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide OnBoard ID." });
                }

                #endregion

                #region Fetch Pre-Onboarding details

                object[] param = new object[] { OnBoardId, HR_ID };
                string paramasString = CommonLogic.ConvertToParamString(param);

                sproc_Get_PreOnboarding_Details_For_AMAssignment_Result AMAssignmentPreOnBoardingDetails = _iOnboard.sproc_Get_PreOnboarding_Details_For_AMAssignment(paramasString);
                if (AMAssignmentPreOnBoardingDetails != null)
                {
                    AMAssignmentViewModel.PreOnboardingDetailsForAMAssignment = AMAssignmentPreOnBoardingDetails;
                    if (AMAssignmentPreOnBoardingDetails.AMUserID > 0)
                    {
                        AMAssignmentViewModel.AssignAM = true;
                    }
                    else
                    {
                        AMAssignmentViewModel.AssignAM = false;
                    }
                }

                #endregion

                #region Fetch CurrentHR's

                //Fetch the open/inprocess HR's.
                object[] paramHR = new object[] { Convert.ToInt64(_SalesHiringRequest.ContactId), HR_ID };
                string paramasStringHR = CommonLogic.ConvertToParamString(paramHR);
                List<sproc_UTS_GetAllOpenInprocessHR_BasedOnContact_Result> currentHRs = _iOnboard.sproc_UTS_GetAllOpenInprocessHR_BasedOnContact(paramasStringHR);
                AMAssignmentViewModel.CurrentHRs = currentHRs;

                #endregion

                #region FetchDropdown values

                //Fetch the TscUsers dropdown values.
                var drpTSCUsers = new List<SelectListItem>();
                drpTSCUsers = db.UsrUsers.Join(db.GenTscusers, u => u.Id, tsc => tsc.UserId,
                                                          (u, tsc) => new { u, tsc }).Select(m => new SelectListItem
                                                          {
                                                              Value = m.u.Id.ToString(),
                                                              Text = m.u.FullName
                                                          }).ToList();
                AMAssignmentViewModel.DrpTSCUserList = drpTSCUsers;

                //Fetch the Netpaymentdays dropdown values.
                var drpNetPaymentDays = new List<SelectListItem>();

                drpNetPaymentDays.Insert(0, new SelectListItem { Value = "7", Text = "7" });
                drpNetPaymentDays.Insert(1, new SelectListItem { Value = "15", Text = "15" });
                drpNetPaymentDays.Insert(2, new SelectListItem { Value = "30", Text = "30" });
                drpNetPaymentDays.Insert(3, new SelectListItem { Value = "45", Text = "45" });
                drpNetPaymentDays.Insert(4, new SelectListItem { Value = "60", Text = "60" });

                AMAssignmentViewModel.DRPNetPaymentDays = drpNetPaymentDays;

                //Add the Lead types dropdown values.
                List<SelectListItem> bindLeadTypes = new List<SelectListItem>();

                SelectListItem bindLeadType1 = new SelectListItem();
                bindLeadType1.Text = "1";
                bindLeadType1.Value = "InBound";
                bindLeadTypes.Add(bindLeadType1);

                SelectListItem bindLeadType2 = new SelectListItem();
                bindLeadType2.Text = "2";
                bindLeadType2.Value = "OutBound";
                bindLeadTypes.Add(bindLeadType2);

                SelectListItem bindLeadType3 = new SelectListItem();
                bindLeadType3.Text = "3";
                bindLeadType3.Value = "Partnership";
                bindLeadTypes.Add(bindLeadType3);

                AMAssignmentViewModel.DRPLeadTypes = bindLeadTypes;

                var leadType = Convert.ToString(AMAssignmentViewModel.PreOnboardingDetailsForAMAssignment?.InBoundType);

                if (string.IsNullOrEmpty(leadType))
                {
                    leadType = "InBound";
                }
                else if (leadType.ToLower().Contains("InBound".ToLower()))
                {
                    leadType = "InBound";
                }
                else if (leadType.ToLower().Contains("OutBound".ToLower()))
                {
                    leadType = "OutBound";
                }
                else if (leadType.ToLower().Contains("Partnership".ToLower()))
                {
                    leadType = "Partnership";
                }
                else
                {
                    leadType = "InBound";
                }

                object[] param2 = new object[] { leadType, HR_ID };
                string paramstring = CommonLogic.ConvertToParamString(param2);

                //Fetch the default LeadUsers.
                var inboundLeadTypeList = _iUsers.Sproc_GetUserBy_LeadType(paramstring);

                var LeadTypeList = inboundLeadTypeList.Select(x => new SelectListItem
                {
                    Value = x.Value.ToString(),
                    Text = x.Text.ToString()
                }).ToList();

                LeadTypeList.Insert(0, new SelectListItem() { Value = "0", Text = "--Select--" });

                AMAssignmentViewModel.DRPLeadUsers = LeadTypeList;

                //Fetch the start and end times.
                var drpStartTime = new List<SelectListItem>();

                DateTime start = new DateTime(2019, 12, 17, 0, 0, 0);
                DateTime end = new DateTime(2019, 12, 17, 23, 59, 59);
                int j = 0;
                List<SelectListItem> list = new List<SelectListItem>();
                //while (start.AddMinutes(30) <= end)
                while (start <= end)
                {
                    list.Add(new SelectListItem() { Text = start.ToString("t"), Value = start.ToString("t") });
                    start = start.AddMinutes(30);
                    j += 1;
                }
                AMAssignmentViewModel.DRPStartTime = list;

                var drpEndTime = new List<SelectListItem>();

                j = 0;
                //while (start.AddMinutes(30) <= end)
                while (start <= end)
                {
                    list.Add(new SelectListItem() { Text = start.ToString("t"), Value = start.ToString("t") });
                    start = start.AddMinutes(30);
                    j += 1;
                }
                AMAssignmentViewModel.DRPEndTime = list;

                //Fetch HRAcceptedByUserList
                var HRAcceptedByUserList = db.UsrUsers.Where(x => x.UserTypeId == 5 && x.UserTypeId == 10 && x.IsActive == true).Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = Convert.ToString(x.FullName)
                }).ToList();
                AMAssignmentViewModel.DRPHRAcceptedByUserList = HRAcceptedByUserList;
                #endregion

                #region GetReplacementDetails

                // UTS-7389: If replacement is Yes then save replacement details
                try
                {
                    var replacementDetails = GetReplacementDetails(OnBoardId);
                    if (replacementDetails != null)
                    {
                        AMAssignmentViewModel.ReplacementDetail = replacementDetails;
                    }

                    var replacementEngHRs = await ListofHRAndEngs(OnBoardId);
                    if (replacementEngHRs != null)
                    {
                        AMAssignmentViewModel.ReplacementEngAndHR = replacementEngHRs;
                    }

                }
                catch
                {

                }
                #endregion

                #region 2nd tab code merge into 1st tab

                object[] param1 = new object[] { OnBoardId, HR_ID };
                string paramasString1 = CommonLogic.ConvertToParamString(param1);

                sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment_Result SecondTabAMAssignmentOnBoardingDetails = _iOnboard.sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment(paramasString1);
                if (SecondTabAMAssignmentOnBoardingDetails != null)
                {
                    AMAssignmentViewModel.SecondTabAMAssignmentOnBoardingDetails = SecondTabAMAssignmentOnBoardingDetails;
                }

                #endregion

                #region DeviceMaster

                List<PrgOnBoardPolicyDeviceMaster> deviceMaster = db.PrgOnBoardPolicyDeviceMasters.ToList();
                if (deviceMaster != null)
                {
                    AMAssignmentViewModel.deviceMaster = deviceMaster;
                }

                #endregion

                #region Leave Policy Master
                List<SelectListItem> bindLeavePolicyDrp = new List<SelectListItem>();

                SelectListItem LeavePolicy1 = new SelectListItem();
                LeavePolicy1.Text = "1";
                LeavePolicy1.Value = "Proceed with Uplers Policies";
                bindLeavePolicyDrp.Add(LeavePolicy1);

                SelectListItem LeavePolicy2 = new SelectListItem();
                LeavePolicy2.Text = "1";
                LeavePolicy2.Value = "Upload Your Policies";
                bindLeavePolicyDrp.Add(LeavePolicy2);

                AMAssignmentViewModel.bindLeavePolicyDrp = bindLeavePolicyDrp;
                #endregion

                #region Add Exit Policy and FeedBack Process and Uplers Leave Policy
                AMAssignmentViewModel.UplersLeavePolicy = "https://www.uplers.com/talent/leave-policy/";
                AMAssignmentViewModel.Exit_Policy = "First Month - 7 Days,  Second Month Onwards - 30 Days";
                AMAssignmentViewModel.Feedback_Process = "Weekly during the first 2 weeks | Fortnightly for the next 2 months | Monthly / Quarterly feedback thereafter";

                #endregion

                #region Get Team Member Details

                List<GenOnBoardClientTeam> onBoardClientTeam = db.GenOnBoardClientTeams.Where(x => x.OnBoardId == OnBoardId).ToList();
                if (onBoardClientTeam != null)
                {
                    AMAssignmentViewModel.onBoardClientTeam = onBoardClientTeam;
                }

                #endregion

                #region ATS Call
                if (!configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    Sproc_talent_engagement_Details_For_PHP_API_Result ObjResult = await _iEngagement.TalentEngagementDetails(OnBoardId, "OnBoarding");
                    if (ObjResult != null)
                    {
                        TalentEngagementDetailsViewModel engagementDetails = new()
                        {
                            HiringRequest_ID = ObjResult.HiringRequest_ID,
                            ATSTalentId = ObjResult.ATS_Talent_ID,
                            engagement_id = ObjResult.EngagemenID,
                            engagement_start_date = ObjResult.ContractStartDate,
                            engagement_end_date = ObjResult.ContractEndDate,
                            engagement_status = ObjResult.EngagementStatus,
                            talent_status = ObjResult.Talent_status,
                            joining_date = ObjResult.joining_date,
                            lost_date = ObjResult.Lost_date,
                            last_working_date = ObjResult.Last_working_date,
                            talent_statustag = ObjResult.talent_statustag,
                            Action = ObjResult.Action,
                            Action_date = ObjResult.Action_date
                        };

                        var json = JsonConvert.SerializeObject(engagementDetails);
                        ATSCall aTSCall = new(configuration, db);
                        aTSCall.SendTalentEngagementDetails(json, LoggedInUserId, ObjResult.HiringRequest_ID);
                    }
                }
                #endregion

                if (AMAssignmentViewModel != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = AMAssignmentViewModel });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Not found" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("UpdatePreOnBoardingDetailForAMAssignment")]
        public async Task<ObjectResult> UpdatePreOnBoardingDetailForAMAssignment(UpdatePreOnBoardingDetailsForAMAssignment updatePreOnBoardingDetailsForAMAssignment)
        {
            try
            {
                long CreatedByID = SessionValues.LoginUserId;
                long HRID = 0;

                AMAssignmentPreOnBoardingDetailsViewModel AMAssignmentViewModel = new();
                GenSalesHiringRequest? _SalesHiringRequest = new GenSalesHiringRequest();

                #region PreValidation
                if (updatePreOnBoardingDetailsForAMAssignment == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                if (updatePreOnBoardingDetailsForAMAssignment.HR_ID == 0 || updatePreOnBoardingDetailsForAMAssignment.HR_ID == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    HRID = (long)updatePreOnBoardingDetailsForAMAssignment.HR_ID;
                    _SalesHiringRequest = db.GenSalesHiringRequests.Where(x => x.Id == HRID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }
                if (updatePreOnBoardingDetailsForAMAssignment.CompanyID == 0 || updatePreOnBoardingDetailsForAMAssignment.CompanyID == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Company ID." });
                }

                #endregion

                #region Save React payload

                var routeData = HttpContext.GetRouteData();
                var routeName = routeData.Values["controller"] + "/" + routeData.Values["action"];
                var ReactPayload = JsonConvert.SerializeObject(updatePreOnBoardingDetailsForAMAssignment);

                GenUtsadminReactPayload obj = new();
                obj.CompanyId = updatePreOnBoardingDetailsForAMAssignment?.CompanyID;
                obj.Hrid = HRID;
                obj.CreatedById = CreatedByID;
                obj.Apiname = routeName.ToString();
                obj.Payload = ReactPayload;
                obj.AppActionDoneBy = (short)AppActionDoneBy.UTS;

                _universalProcRunner.InsertReactPayload(obj);

                #endregion

                #region SetDynamicCTA

                DynamicOnBoardCTA dynamicOnBoardCTA = new DynamicOnBoardCTA();
                dynamicOnBoardCTA.GotoOnboard = new CTAInfo(OnBoardCTA.GotoOnBoard, "Continue", true);
                AMAssignmentViewModel.dynamicOnBoardCTA = dynamicOnBoardCTA;

                #endregion

                #region Update Company/Talent Info

                object[] param1 = new object[]
                {
                        HRID,
                        updatePreOnBoardingDetailsForAMAssignment.CompanyID,
                        CreatedByID, updatePreOnBoardingDetailsForAMAssignment.Lead_Type,
                        updatePreOnBoardingDetailsForAMAssignment.Deal_Source,
                        updatePreOnBoardingDetailsForAMAssignment.Deal_Owner,
                        updatePreOnBoardingDetailsForAMAssignment.Industry_Type,
                        updatePreOnBoardingDetailsForAMAssignment.Onboard_ID,
                        updatePreOnBoardingDetailsForAMAssignment.TalentShiftStartTime,
                        updatePreOnBoardingDetailsForAMAssignment.TalentShiftEndTime,
                        updatePreOnBoardingDetailsForAMAssignment.NetPaymentDays,
                        updatePreOnBoardingDetailsForAMAssignment.PayRate,
                        updatePreOnBoardingDetailsForAMAssignment.BillRate,
                        updatePreOnBoardingDetailsForAMAssignment.UplersFeesAmount,
                        updatePreOnBoardingDetailsForAMAssignment.TalentID,
                        updatePreOnBoardingDetailsForAMAssignment.NRMargin,
                        updatePreOnBoardingDetailsForAMAssignment.ModeOFWorkingID,
                        updatePreOnBoardingDetailsForAMAssignment.City ?? "",
                        updatePreOnBoardingDetailsForAMAssignment.StateID,
                        updatePreOnBoardingDetailsForAMAssignment.Talent_Designation,
                        updatePreOnBoardingDetailsForAMAssignment.AMSalesPersonID,
                        updatePreOnBoardingDetailsForAMAssignment.TSCUserId
                };
                string paramasString1 = CommonLogic.ConvertToParamStringWithNull(param1);

                _iOnboard.sproc_Update_PreOnBoardingDetails_for_AMAssignment(paramasString1);

                #region Insert HR History

                string action = Action_Of_History.OnboardingClient_In_Process.ToString();
                if (!string.IsNullOrEmpty(action))
                {
                    object[] args = new object[] { action, HRID, updatePreOnBoardingDetailsForAMAssignment.TalentID, false, CreatedByID, 0, 0, "", updatePreOnBoardingDetailsForAMAssignment.Onboard_ID, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                }
                #endregion

                #region SendEmail to TSC

                if (updatePreOnBoardingDetailsForAMAssignment.TSCUserId > 0)
                {
                    EmailBinder binder = new EmailBinder(configuration, db);
                    object[] param = new object[]
                    {
                        updatePreOnBoardingDetailsForAMAssignment.Onboard_ID
                    };
                    string paramasString = CommonLogic.ConvertToParamString(param);
                    Sproc_GetEmailDetailForTSCAssignment_Result result = _iEngagement.GetEmailDetailForTSCAssignment(paramasString);
                    if (result != null)
                    {
                        if (!string.IsNullOrEmpty(result.OldTSCEmail))
                        {
                            bool SendEmailToOldTSC = binder.SendTSCAssignmentEmailToOLDTSC(result);
                        }
                        if (!string.IsNullOrEmpty(result.NewTSCEmail))
                        {
                            bool SendEmailToNewTSC = binder.SendTSCAssignmentEmailToNewTSC(result);
                        }
                    }
                }
                #endregion

                #endregion

                if (updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails != null)
                {
                    #region 2nd tab 

                    object[] param = new object[]
                    {
                    updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.HR_ID,
                    updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.CompanyID,
                    updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.SigningAuthorityName,
                    updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.SigningAuthorityEmail,
                    updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.ContractDuration,
                    updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.OnBoardID,
                    updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.about_Company_desc,
                    updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.Talent_FirstWeek,
                    updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.Talent_FirstMonth,
                    updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.SoftwareToolsRequired,
                    CreatedByID
                    };
                    string paramasString = CommonLogic.ConvertToParamString(param);

                    _iOnboard.sproc_Update_Onboarding_Details_For_Second_Tab_AMAssignment(paramasString);

                    #endregion

                    #region Save & Update Device Policy and Leave Policy

                    #region Device Policy

                    GenOnBoardClientContractDetail onBoardClientContractDetails = new GenOnBoardClientContractDetail();
                    onBoardClientContractDetails = db.GenOnBoardClientContractDetails.Where(x => x.OnBoardId == updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.OnBoardID).FirstOrDefault();
                    onBoardClientContractDetails.DevicesPoliciesOption = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.hdnRadioDevicesPolicies;

                    if (updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.hdnRadioDevicesPolicies == "Client to buy a device and Uplers to Facilitate")
                    {
                        updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.TalentDeviceDetails = string.Empty;
                        var InsertedDevices = db.GenOnBoardClientDevicePolicyDetails.Where(p => p.OnBoardId == updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.OnBoardID).ToList();
                        db.GenOnBoardClientDevicePolicyDetails.RemoveRange(InsertedDevices);

                        updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.AdditionalCostPerMonth_RDPSecurity = 0;
                        updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.IsRecurring = false;
                        //OtherDeviceDescription
                        DeviceOptionVM optionvmdata = new DeviceOptionVM();

                        GenOnBoardClientDevicePolicyDetail devicepolicydetails = new GenOnBoardClientDevicePolicyDetail();

                        devicepolicydetails.OnBoardId = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.OnBoardID;
                        devicepolicydetails.DeviceId = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.DeviceID;
                        devicepolicydetails.ClientDeviceDescription = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.Client_DeviceDescription;
                        devicepolicydetails.Qty = 1;
                        devicepolicydetails.TotalCost = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.TotalCost;
                        db.GenOnBoardClientDevicePolicyDetails.Add(devicepolicydetails);
                        db.SaveChanges();
                    }
                    else if (updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.hdnRadioDevicesPolicies == "Talent to bring his own devices")
                    {
                        onBoardClientContractDetails.TalentDeviceDetails = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.TalentDeviceDetails;
                    }
                    else if (updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.hdnRadioDevicesPolicies == "Client can use remote desktop security option facilitated by Uplers (At additional cost of $100 per month)")
                    {
                        onBoardClientContractDetails.TalentDeviceDetails = string.Empty;
                        var InsertedDevices = db.GenOnBoardClientDevicePolicyDetails.Where(p => p.OnBoardId == updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.OnBoardID).ToList();
                        db.GenOnBoardClientDevicePolicyDetails.RemoveRange(InsertedDevices);

                        onBoardClientContractDetails.AdditionalCostPerMonthRdpsecurity = 100;
                        onBoardClientContractDetails.IsRecurring = true;
                    }
                    else if (updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.hdnRadioDevicesPolicies == "Add This Later")
                    {
                        onBoardClientContractDetails.TalentDeviceDetails = string.Empty;
                        var InsertedDevices = db.GenOnBoardClientDevicePolicyDetails.Where(p => p.OnBoardId == updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.OnBoardID).ToList();
                        db.GenOnBoardClientDevicePolicyDetails.RemoveRange(InsertedDevices);
                    }

                    #endregion

                    #region Leave Policy
                    onBoardClientContractDetails.ProceedWithUplersLeavePolicyOption = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.Radio_LeavePolicies;
                    if (updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.Radio_LeavePolicies == "Proceed with Uplers Policies")
                    {
                        //   onBoardClientContractDetails.ProceedWithClientLeavePolicyOption = "Leave Policy";
                        if (!string.IsNullOrEmpty(updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.LeavePolicyPasteLinkName))
                        {
                            //onBoardClientContractDetails.ProceedWithClientLeavePolicyOption = "Paste Link";
                            onBoardClientContractDetails.ProceedWithClientLeavePolicyLink = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.LeavePolicyPasteLinkName;
                        }
                        else { onBoardClientContractDetails.ProceedWithClientLeavePolicyLink = string.Empty; }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.LeavePolicyFileName))
                        {
                            // onBoardClientContractDetails.ProceedWithClientLeavePolicyOption = "Upload Client Leave Policy";
                            onBoardClientContractDetails.ProceedWithClientLeavePolicyFileUpload = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.LeavePolicyFileName;
                        }
                        else
                        {
                            onBoardClientContractDetails.ProceedWithClientLeavePolicyFileUpload = string.Empty;
                        }
                    }
                    #endregion

                    #region ExitPolicy

                    onBoardClientContractDetails.ProceedWithUplersExitPolicyOption = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.Exit_Policy;

                    #endregion

                    db.Entry(onBoardClientContractDetails).State = EntityState.Modified;
                    db.SaveChanges();

                    #endregion

                    #region Adding TeamMembers 

                    if (updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.TeamMembers != null)
                    {
                        if (updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.TeamMembers.Count > 0)
                        {
                            foreach (var team in updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.TeamMembers)
                            {
                                GenOnBoardClientTeam OnBoardClientTeamdata = new GenOnBoardClientTeam();
                                OnBoardClientTeamdata.OnBoardId = updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.OnBoardID;
                                OnBoardClientTeamdata.Name = team.Name;
                                OnBoardClientTeamdata.Designation = team.Designation;
                                OnBoardClientTeamdata.Email = team.Email;
                                OnBoardClientTeamdata.Linkedin = team.Linkedin;
                                OnBoardClientTeamdata.ReportingTo = team.ReportingTo;
                                if (team.Buddy == "1" || team.Buddy == "Buddy")
                                {
                                    OnBoardClientTeamdata.Buddy = "Buddy";
                                }
                                else
                                {
                                    OnBoardClientTeamdata.Buddy = "Select Later";
                                }
                                OnBoardClientTeamdata.IsActive = true;
                                db.GenOnBoardClientTeams.Add(OnBoardClientTeamdata);
                                db.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    #region update Client OnBoarding Status and Insert History

                    GenOnBoardTalent? onBoardTalents = db.GenOnBoardTalents.FirstOrDefault(x => x.Id == updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.OnBoardID);
                    if (onBoardTalents != null)
                    {
                        onBoardTalents.ClientOnBoardingStatusId = 2;
                        onBoardTalents.LastModifiedDatetime = DateTime.Now;
                        onBoardTalents.LastModifiedById = (int)CreatedByID;
                        onBoardTalents.ClientOnBoardingDate = DateTime.Now;
                        db.Entry(onBoardTalents).State = EntityState.Modified;
                        db.SaveChanges();

                        #region Insert HR History
                        action = Action_Of_History.OnboardingClient_Done.ToString();
                        if (!string.IsNullOrEmpty(action))
                        {
                            object[] args = new object[] { action, updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.HR_ID, onBoardTalents.TalentId, false, CreatedByID, 0, 0, "", updatePreOnBoardingDetailsForAMAssignment.updateClientOnBoardingDetails.OnBoardID, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                        }
                        #endregion
                    }

                    #endregion
                }

                #region SaveReplacementDetails

                // UTS-7389: If replacement is Yes then save replacement details
                try
                {
                    bool isReplacement = Convert.ToBoolean(updatePreOnBoardingDetailsForAMAssignment.IsReplacement);
                    await SaveUpdateReplacementDetails(updatePreOnBoardingDetailsForAMAssignment.talentReplacement, isReplacement);
                }
                catch
                {

                }

                #endregion

                #region ATS Call
                if (updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.OnBoardID != null)
                {
                    if (!configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                    {
                        ATSCall aTSCall = new ATSCall(configuration, db);

                        #region TalentEngagementDetails send to ATS
                        Sproc_talent_engagement_Details_For_PHP_API_Result result = await _iEngagement.TalentEngagementDetails(updatePreOnBoardingDetailsForAMAssignment?.updateClientOnBoardingDetails?.OnBoardID, "Update PreOnBoarding");
                        if (result != null)
                        {
                            TalentEngagementDetailsViewModel engagementDetails = new()
                            {
                                HiringRequest_ID = result.HiringRequest_ID,
                                ATSTalentId = result.ATS_Talent_ID,
                                engagement_id = result.EngagemenID,
                                engagement_start_date = result.ContractStartDate,
                                engagement_end_date = result.ContractEndDate,
                                engagement_status = result.EngagementStatus,
                                talent_status = result.Talent_status,
                                joining_date = result.joining_date,
                                lost_date = result.Lost_date,
                                last_working_date = result.Last_working_date,
                                talent_statustag = result.talent_statustag,
                                Action = result.Action,
                                Action_date = result.Action_date
                            };

                            try
                            {
                                var json = JsonConvert.SerializeObject(engagementDetails);
                                aTSCall.SendTalentEngagementDetails(json, CreatedByID, result.HiringRequest_ID);
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
                            }
                        }
                        #endregion

                        #region pay rate api call to ATS
                        HRTalentDetail talentDetail = new();
                        GenTalent Talent = db.GenTalents.Where(x => x.Id == updatePreOnBoardingDetailsForAMAssignment.TalentID).FirstOrDefault();

                        db.Entry(Talent).Reload();

                        var TalentCTP_Details = db.GenContactTalentPriorities.Where(x => x.TalentId == Talent.Id && x.HiringRequestId == HRID).FirstOrDefault();
                        if (TalentCTP_Details != null && Talent != null)
                        {
                            talentDetail.CTPID = TalentCTP_Details.Id;
                            talentDetail.Talent_current_fee = TalentCTP_Details.CurrentCtc;
                            if (updatePreOnBoardingDetailsForAMAssignment?.PayRate > 0)
                            {
                                talentDetail.Talent_Expected_fee = updatePreOnBoardingDetailsForAMAssignment.PayRate;
                            }
                            else
                            {
                                talentDetail.Talent_Expected_fee = TalentCTP_Details.TalentCost;
                            }
                            if (updatePreOnBoardingDetailsForAMAssignment?.BillRate > 0)
                            {
                                talentDetail.HR_Cost = updatePreOnBoardingDetailsForAMAssignment.BillRate;
                            }
                            else
                            {
                                talentDetail.HR_Cost = TalentCTP_Details.FinalHrCost;
                            }
                            talentDetail.HR_Cost_With_Currency = TalentCTP_Details.HrCost;
                            talentDetail.HR_Type = TalentCTP_Details.IsHrtypeDp == true ? "DP" : "Contractual";
                            talentDetail.DPAmount = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.Dpamount : 0;
                            talentDetail.TalentCurrency = TalentCTP_Details.TalentCurrencyCode == null ? "USD" : TalentCTP_Details.TalentCurrencyCode;
                            talentDetail.UTS_TalentID = Convert.ToInt64(Talent.Id);
                            talentDetail.ATS_TalentID = Convert.ToInt64(Talent.AtsTalentId);
                            talentDetail.ExchangeRateUTS = TalentCTP_Details.ExchangeRate == null ? 1 : TalentCTP_Details.ExchangeRate;
                            if (updatePreOnBoardingDetailsForAMAssignment?.UplersFeesPerc > 0)
                            {
                                talentDetail.DPorNR_Percent = updatePreOnBoardingDetailsForAMAssignment.UplersFeesPerc;
                            }
                            else
                            {
                                talentDetail.DPorNR_Percent = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.DpPercentage : TalentCTP_Details.Nrpercentage;
                            }
                        }
                        try
                        {
                            var json = JsonConvert.SerializeObject(talentDetail);
                            aTSCall.SendPayrateBillratetoATS(json, CreatedByID, HRID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
                        }
                        #endregion
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("UploadSOWDocument")]
        public async Task<ObjectResult> UploadSOWDocument([FromForm] IFormFile file, [FromQuery] long OnBoardId, [FromQuery] bool isDelete)
        {
            try
            {
                #region Validation

                if (OnBoardId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide OnBoard ID." });
                }

                if (!isDelete)
                {
                    if (file == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File Required", Details = null });
                    }
                    else if (file.Length == 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "You are uploading corrupt file", Details = null });
                    }

                    var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                    string[] allowedFileExtension = { ".pdf", ".doc", ".docx", ".txt", ".rtf", ".jpg", ".jpeg", ".png" };

                    if (!allowedFileExtension.Contains(fileExtension))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Your file format is incorrect.", Details = null });
                    }

                    var fileSize = (file.Length / 1024) / 1024;
                    if (fileSize >= 0.5)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File size must be less than 500 KB", Details = null });
                    }
                }

                #endregion

                string DocfileName = file.FileName;
                string folderPath = System.IO.Path.Combine("Media/OnBoarding/SOWDocument", OnBoardId.ToString());
                string filePath = System.IO.Path.Combine(folderPath, DocfileName);

                if (isDelete)
                {
                    //If file path exists then delete the path.
                    if (Directory.Exists(folderPath))
                    {
                        System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

                        foreach (FileInfo doc in di.GetFiles())
                        {
                            doc.Delete();
                        }

                        Directory.Delete(folderPath);
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "File removed successfully"
                    });
                }

                //If file path does not exists then create the path.
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Details = DocfileName
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("UploadLeavePolicy")]
        public async Task<ObjectResult> UploadLeavePolicy([FromForm] IFormFile file, [FromQuery] long OnBoardId, [FromQuery] bool isDelete)
        {
            try
            {
                #region Validation

                if (OnBoardId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide OnBoard ID." });
                }

                if (!isDelete)
                {
                    if (file == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File Required", Details = null });
                    }
                    else if (file.Length == 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "You are uploading corrupt file", Details = null });
                    }

                    var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                    string[] allowedFileExtension = { ".pdf", ".doc", ".docx", ".txt", ".rtf", ".jpg", ".jpeg", ".png" };

                    if (!allowedFileExtension.Contains(fileExtension))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Your file format is incorrect.", Details = null });
                    }

                    var fileSize = (file.Length / 1024) / 1024;
                    if (fileSize >= 0.5)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File size must be less than 500 KB", Details = null });
                    }
                }

                #endregion

                string DocfileName = file.FileName;
                string folderPath = System.IO.Path.Combine("Media/OnBoarding/LeavePolicy", OnBoardId.ToString());
                string filePath = System.IO.Path.Combine(folderPath, DocfileName);

                if (isDelete)
                {
                    //If file path exists then delete the path.
                    if (Directory.Exists(folderPath))
                    {
                        System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

                        foreach (FileInfo doc in di.GetFiles())
                        {
                            doc.Delete();
                        }

                        Directory.Delete(folderPath);
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "File removed successfully"
                    });
                }

                //If file path does not exists then create the path.
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Details = DocfileName
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Onboarding new changes for 2nd tab
        [HttpGet("GetLegalInfo")]
        public async Task<ObjectResult> GetLegalInfo(long? OnBoardId, long? HR_ID)
        {
            try
            {
                #region Pre-validation
                if (OnBoardId == 0 || HR_ID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Request Object is null / empty" });
                }
                #endregion

                dynamic dObject = new ExpandoObject();

                #region Fetch getLegalInfo
                object[] param = new object[] { OnBoardId, HR_ID };
                string paramasString = CommonLogic.ConvertToParamString(param);
                sproc_UTS_Get_Onboarding_LegalInfo_Result getLegalInfo = _iOnboard.sproc_UTS_Get_Onboarding_LegalInfo_Result(paramasString);

                dObject.getLegalInfo = getLegalInfo;
                #endregion

                #region GetReplacementDetails

                // UTS-7389: If replacement is Yes then save replacement details
                try
                {
                    var replacementDetails = GetReplacementDetails(OnBoardId);
                    if (replacementDetails != null)
                    {
                        dObject.ReplacementDetail = replacementDetails;
                    }

                    var replacementEngHRs = await ListofHRAndEngs(OnBoardId);
                    if (replacementEngHRs != null)
                    {
                        dObject.replacementEngHRs = replacementEngHRs;
                    }

                }
                catch
                {

                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully", Details = dObject });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("UpdateLegalInfo")]
        public async Task<ObjectResult> UpdateLegalInfo(OnBoardingLegalUpdates updates)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;

                #region Pre-validation
                if (updates == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Request Object is null / empty" });
                }
                if (updates.HiringRequestID == 0 || updates.OnBoardID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Request Object is null / empty" });
                }
                #endregion

                #region Save React payload

                var routeData = HttpContext.GetRouteData();
                var routeName = routeData.Values["controller"] + "/" + routeData.Values["action"];
                var ReactPayload = JsonConvert.SerializeObject(updates);

                GenUtsadminReactPayload obj = new();
                obj.CompanyId = updates?.CompanyID;
                obj.Hrid = updates?.HiringRequestID;
                obj.CreatedById = LoggedInUserId;
                obj.Apiname = routeName.ToString();
                obj.Payload = ReactPayload;
                obj.AppActionDoneBy = (short)AppActionDoneBy.UTS;

                _universalProcRunner.InsertReactPayload(obj);

                #endregion

                #region covert date format

                string ContractStartDate = updates.ContractStartDate != null ? updates.ContractStartDate.Value.ToString("MM/dd/yyyy") : null;
                string ContractEndDate = updates.ContractEndDate != null ? updates.ContractEndDate.Value.ToString("MM/dd/yyyy") : null;
                string ClientSOWSignDate = updates.ClientSOWSignDate != null ? updates.ClientSOWSignDate.Value.ToString("MM/dd/yyyy") : null;
                string TalentSOWSignDate = updates.TalentSOWSignDate != null ? updates.TalentSOWSignDate.Value.ToString("MM/dd/yyyy") : null;
                string ClientMSASignDate = updates.ClientMSASignDate != null ? updates.ClientMSASignDate.Value.ToString("MM/dd/yyyy") : null;
                string TalentMSASignDate = updates.TalentMSASignDate != null ? updates.TalentMSASignDate.Value.ToString("MM/dd/yyyy") : null;
                string JoiningDate = updates.JoiningDate != null ? updates.JoiningDate.Value.ToString("MM/dd/yyyy") : null;

                #endregion

                #region Save SP call Sproc_UTS_Admin_OnboardingStatusUpdates
                object[] args = new object[]
                {
                    updates.OnBoardID,
                    updates.TalentID,
                    updates.HiringRequestID,
                    updates.ContactID,
                    updates.CompanyID,
                    updates.InvoiceRaiseTo,
                    updates.InvoiceRaiseToEmail,
                    ContractStartDate,
                    ContractEndDate,
                    ClientSOWSignDate,
                    TalentSOWSignDate,
                    ClientMSASignDate,
                    TalentMSASignDate,
                    LoggedInUserId,
                    JoiningDate,
                    updates.IsIndefiniteHR
                };

                _universalProcRunner.ManipulationWithNULL(Constants.ProcConstant.Sproc_UTS_Admin_OnboardingStatusUpdates, args);
                #endregion 

                #region SaveReplacementDetails

                // UTS-7389: If replacement is Yes then save replacement details
                try
                {
                    bool isReplacement = Convert.ToBoolean(updates.IsReplacement);
                    await SaveUpdateReplacementDetails(updates.talentReplacement, isReplacement);
                }
                catch
                {

                }

                #endregion

                #region ATS call
                if (configuration["HRDataSendSwitchForPHP"].ToLower() != "local" && updates.HiringRequestID > 0)
                {
                    long talentId = 0;
                    var varHRdata = db.GenSalesHiringRequests.AsNoTracking().Where(t => t.Id == updates.HiringRequestID).FirstOrDefault();
                    if (varHRdata != null)
                    {
                        //var varTalentList = db.GenContactTalentPriorities.AsNoTracking().Where(x => x.HiringRequestId == hiringRequestId && x.TalentId == statusUpdate.TalentID).ToList();
                        var varTalentList = db.GenContactTalentPriorities.AsNoTracking().Where(x => x.HiringRequestId == updates.HiringRequestID && x.TalentId == updates.TalentID).FirstOrDefault();
                        if (varTalentList != null)
                        {
                            ContactTalentPriorityModel objCTPModel = new();
                            talentId = varTalentList.TalentId ?? 0;
                            objCTPModel.HRStatusID = varHRdata.StatusId ?? 0;
                            string HiringRequestStatus = "";
                            var varHRStatusData = db.PrgHiringRequestStatuses.AsNoTracking().Where(x => x.Id == objCTPModel.HRStatusID).FirstOrDefault();
                            if (varHRStatusData != null)
                                HiringRequestStatus = varHRStatusData.HiringRequestStatus;

                            objCTPModel.HRStatus = HiringRequestStatus;
                            objCTPModel.HRID = (long)updates.HiringRequestID;
                            //CallAtsAPIToSendTalentAndHRStatus api call
                            objCTPModel.TalentDetails = new();
                            //foreach (var talentID in varTalentList)
                            //{
                            var varGenTalent = db.GenTalents.AsNoTracking().Where(x => x.Id == talentId).FirstOrDefault();

                            TalentDetail talentDetail = new();
                            talentDetail.TalentStatus = "";

                            talentDetail.ATS_TalentID = Convert.ToInt64(varGenTalent.AtsTalentId);
                            //var TalentCTP_Details = varTalentList.Where(x => x.TalentId == varGenTalent.Id && x.HiringRequestId == hiringRequestId).FirstOrDefault();
                            var TalentCTP_Details = varTalentList;
                            if (TalentCTP_Details != null)
                            {
                                var TalStatusClientSelectionDetail = db.PrgTalentStatusAfterClientSelections.AsNoTracking().Where(x => x.Id == TalentCTP_Details.TalentStatusIdBasedOnHr).FirstOrDefault();
                                if (TalStatusClientSelectionDetail != null)
                                {
                                    talentDetail.TalentStatus = TalStatusClientSelectionDetail.TalentStatus;
                                }
                                //talentDetail.MatchMakingDateTime = Convert.ToDateTime(TalentCTP_Details.CreatedByDatetime).ToString("dd-MM-yyyy hh:mm:ss");

                            }
                            talentDetail.UTS_TalentID = varGenTalent.Id;
                            talentDetail.Talent_USDCost = varGenTalent.FinalCost ?? 0;

                            object[] objParam = new object[] { updates.HiringRequestID, varGenTalent.Id };
                            string strParamas = CommonLogic.ConvertToParamString(objParam);
                            var varTalent_RejectReason = commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas).ActualReason;

                            talentDetail.Talent_RejectReason = varTalent_RejectReason;
                            var varUsrUserById = commonInterface.TalentStatus.GetUsrUserById(SessionValues.LoginUserId);

                            try
                            {
                                if (varUsrUserById != null)
                                {
                                    talentDetail.RejectedBy = varUsrUserById.EmployeeId;
                                    talentDetail.ActionUserName = varUsrUserById.FullName;
                                    talentDetail.ActionUserEmail = varUsrUserById.EmailId;
                                    talentDetail.ActionBy = Convert.ToString(StatusChangeAction.Sales);
                                }
                            }
                            catch
                            {

                            }

                            // UTS-7093: Fetch the round details and send to ATS.
                            try
                            {
                                // UTS-7093: Fetch the round details and send to ATS.
                                object[] param = new object[] { updates.HiringRequestID, 0, varGenTalent.Id };
                                string paramString = CommonLogic.ConvertToParamString(param);

                                ATSCall aTSCallforRound = new ATSCall(configuration, db);

                                sproc_Get_InterviewRoundDetails_Result roundDetails = aTSCallforRound.sproc_Get_InterviewRoundDetails(paramString);
                                if (roundDetails != null)
                                {
                                    string? talentStatusRoundDetails = roundDetails.StrInterviewRound;
                                    if (!string.IsNullOrEmpty(talentStatusRoundDetails))
                                    {
                                        talentDetail.TalentStatusRoundDetails = talentStatusRoundDetails;
                                    }
                                }
                            }
                            catch
                            {

                            }

                            objCTPModel.TalentDetails.Add(talentDetail);
                            //}
                            try
                            {
                                var json = JsonConvert.SerializeObject(objCTPModel);
                                ATSCall aTSCall = new ATSCall(configuration, db);
                                aTSCall.SaveContactTalentPriority(json, LoggedInUserId, (long)updates.HiringRequestID);
                                //return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully" });
                            }
                            catch (Exception)
                            {
                                //return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully" });
                            }
                        }
                    }
                }
                #endregion

                #region ATS Call
                if (updates.OnBoardID != null)
                {
                    if (!configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                    {

                        Sproc_talent_engagement_Details_For_PHP_API_Result result = await _iEngagement.TalentEngagementDetails(updates.OnBoardID, "Legal");
                        if (result != null)
                        {
                            TalentEngagementDetailsViewModel engagementDetails = new()
                            {
                                HiringRequest_ID = result.HiringRequest_ID,
                                ATSTalentId = result.ATS_Talent_ID,
                                engagement_id = result.EngagemenID,
                                engagement_start_date = result.ContractStartDate,
                                engagement_end_date = result.ContractEndDate,
                                engagement_status = result.EngagementStatus,
                                talent_status = result.Talent_status,
                                joining_date = result.joining_date,
                                lost_date = result.Lost_date,
                                last_working_date = result.Last_working_date,
                                talent_statustag = result.talent_statustag,
                                Action = result.Action,
                                Action_date = result.Action_date
                            };

                            var json = JsonConvert.SerializeObject(engagementDetails);
                            ATSCall aTSCall = new(configuration, db);
                            aTSCall.SendTalentEngagementDetails(json, LoggedInUserId, result.HiringRequest_ID);
                        }
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet("GetOnboardingStatus")]
        public async Task<ObjectResult> GetOnboardingStatus(long onBoardId, string Action)
        {
            try
            {
                GenOnBoardTalent onBoardTalent = null;
                string[] actionList = { "OnboardingTalent", "LegalTalent", "OnboardingClient", "LegalClient", "KickOff" };

                #region Prevalidation
                if (string.IsNullOrEmpty(Action) || !actionList.Contains(Action))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide proper action name." });
                }
                if (onBoardId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Onboard ID must be greater than 0." });
                }
                else
                {
                    onBoardTalent = db.GenOnBoardTalents.FirstOrDefault(m => m.Id == onBoardId);
                    if (onBoardTalent == null)
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Onboard data does not exist." });
                }
                #endregion

                dynamic dObject = new ExpandoObject();

                GenOnBoardClientContractDetail OnBoardClientContractDetail = db.GenOnBoardClientContractDetails.Where(x => x.OnBoardId == onBoardTalent.Id).FirstOrDefault();
                if (OnBoardClientContractDetail != null && OnBoardClientContractDetail.ContractEndDate != null && OnBoardClientContractDetail.ContractStartDate != null)
                {
                    dObject.ContractStartDate = OnBoardClientContractDetail.ContractStartDate.Value.ToString("dd/MM/yyyy");
                    dObject.ContractEndDate = OnBoardClientContractDetail.ContractEndDate.Value.ToString("dd/MM/yyyy");
                }

                if (Action == "OnboardingTalent")
                {
                    dObject.preOnBoardStatus = db.PrgPreOnBoardStatuses.ToList().Where(x => x.IsDisplayToTalent == true && x.IsActive == true)
                        .Select(x => new MastersResponseModel
                        {
                            Id = x.Id,
                            Value = x.PreOnBoardStatus
                        });
                }
                else if (Action == "LegalTalent")
                {
                    dObject.onBoardLegalKickOffStatus = db.PrgOnBoardLegalKickOffStatuses.ToList().Where(x => x.IsActive == true && x.IsLegal == true)
                        .Select(x => new MastersResponseModel
                        {
                            Id = x.Id,
                            Value = x.LegalKickOffStatus
                        });
                }
                else if (Action == "OnboardingClient")
                {
                    dObject.preOnBoardStatus = db.PrgPreOnBoardStatuses.ToList().Where(x => x.IsDisplayToClient == true && x.IsActive == true)
                        .Select(x => new MastersResponseModel
                        {
                            Id = x.Id,
                            Value = x.PreOnBoardStatus
                        });
                }
                else if (Action == "LegalClient")
                {
                    dObject.onBoardLegalKickOffStatus = db.PrgOnBoardLegalKickOffStatuses.ToList().Where(x => x.IsActive == true && x.IsLegal == true)
                        .Select(x => new MastersResponseModel
                        {
                            Id = x.Id,
                            Value = x.LegalKickOffStatus
                        });
                }
                else if (Action == "KickOff")
                {
                    dObject.onBoardLegalKickOffStatus = db.PrgOnBoardLegalKickOffStatuses.ToList().Where(x => x.IsActive == true && x.IsKickOff == true)
                        .Select(x => new MastersResponseModel
                        {
                            Id = x.Id,
                            Value = x.LegalKickOffStatus
                        });
                    dObject.Timezonedata = db.PrgContactTimeZones.Where(y => y.IsActive == true)
                        .Select(y => new MastersResponseModel
                        {
                            Value = y.TimeZoneTitle,
                            Id = y.Id

                        }).ToList();
                }
                dObject.status = Action;

                var CompanyLegalDocInfo = (from g1 in db.GenCompanyLegalInfos
                                           join g2 in db.GenCompanies on g1.CompanyId equals g2.Id
                                           join g3 in db.GenContacts on g2.Id equals g3.CompanyId
                                           join g4 in db.GenOnBoardTalents on g3.Id equals g4.ContactId
                                           where g4.Id == onBoardId
                                           select g1).ToList();

                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem { Text = "", Value = "" });

                if (CompanyLegalDocInfo.Count == 0)
                {
                    CompanyLegalDocInfo = (from g1 in db.GenCompanyLegalInfos
                                           join g2 in db.GenCompanies on g1.CompanyId equals g2.Id
                                           join g3 in db.GenContacts on g2.Id equals g3.CompanyId
                                           select g1).ToList();

                    if (CompanyLegalDocInfo.Count > 0)
                    {
                        dObject.CompanyLegalDocInfo = CompanyLegalDocInfo.Select(x => new MastersResponseModel
                        {
                            Value = x.DocumentName,
                            Id = x.Id
                        }).ToList();
                    }
                    else
                        dObject.CompanyLegalDocInfo = listItems;
                }
                else
                {
                    dObject.CompanyLegalDocInfo = CompanyLegalDocInfo.Select(x => new MastersResponseModel
                    {
                        Value = x.DocumentName,
                        Id = x.Id
                    }).ToList();
                }

                var lastworkingdayExists = db.GenOnBoardTalentsReplacementDetails.Where(x => x.ReplaceTalentId == onBoardTalent.TalentId &&
                x.ContactId == onBoardTalent.ContactId && x.NewOnBoardId == onBoardTalent.Id &&
                x.HiringRequestId == onBoardTalent.HiringRequestId && x.LastWorkingDay == null).FirstOrDefault();

                if (lastworkingdayExists != null && lastworkingdayExists.LastWorkingDay == null)
                {
                    dObject.IsLastWorkingDayShow = true;
                }
                else
                {
                    dObject.IsLastWorkingDayShow = false;
                }

                #region GetReplacementDetails

                // UTS-7389: If replacement is Yes then save replacement details
                try
                {
                    var replacementDetails = GetReplacementDetails(onBoardId);
                    if (replacementDetails != null)
                    {
                        dObject.ReplacementDetail = replacementDetails;
                    }

                    var replacementEngHRs = await ListofHRAndEngs(onBoardId);
                    if (replacementEngHRs != null)
                    {
                        dObject.replacementEngHRs = replacementEngHRs;
                    }

                }
                catch
                {

                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("OnBoardingStatusUpdates")]
        public async Task<ObjectResult> OnBoardingStatusUpdates(OnBoardingStatusUpdate statusUpdate)
        {
            try
            {
                string[] actionList = { "OnboardingTalent", "LegalTalent", "OnboardingClient", "LegalClient", "KickOff" };

                bool IsSentEmail = false;

                GenOnBoardTalent? onBoardTalents = null;
                GenOnBoardClientContractDetail? OnBoardClientContractDetails = null;
                GenContact? _Contact = null;
                GenTalent? _Talent = null;

                var LoggedInUserId = SessionValues.LoginUserId;
                long talentId = 0;
                long hiringRequestId = 0;
                long contactId = 0;
                long OnboardId = 0;
                long ManagedTalentID = 0;

                #region Prevalidation
                if (string.IsNullOrEmpty(statusUpdate.Action) || !actionList.Contains(statusUpdate.Action))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide proper action name." });
                }
                if (statusUpdate.OnboardID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data is in valid." });
                }
                else
                {
                    onBoardTalents = db.GenOnBoardTalents.FirstOrDefault(m => m.Id == statusUpdate.OnboardID);
                    if (onBoardTalents == null)
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Onboard data not exist." });
                    else
                    {
                        talentId = statusUpdate.TalentID;
                        hiringRequestId = statusUpdate.HiringRequestID;
                        contactId = statusUpdate.ContactID;
                        OnboardId = statusUpdate.OnboardID;

                        ManagedTalentID = (onBoardTalents.TalentId == 0 && onBoardTalents.IsHrmanaged == true) ?
                                        onBoardTalents.ManagedTalentId ?? 0 : onBoardTalents.TalentId ?? 0;

                        OnBoardClientContractDetails = db.GenOnBoardClientContractDetails.FirstOrDefault(x => x.OnBoardId == OnboardId);
                        _Contact = db.GenContacts.FirstOrDefault(x => x.Id == contactId);
                        _Talent = db.GenTalents.FirstOrDefault(x => x.Id == talentId);
                    }
                }
                #endregion

                Meeting meeting = new Meeting();

                if (statusUpdate.Action == "OnboardingTalent" && statusUpdate.OnboardingTalent != null)
                {
                    onBoardTalents.TalentOnBoardingStatusId = statusUpdate.OnboardingTalent.TalentOnBoardingStatusID;
                    onBoardTalents.TalentConcernRemark = statusUpdate.OnboardingTalent.TalentConcernRemark;
                    onBoardTalents.LastModifiedDatetime = DateTime.Now;
                    onBoardTalents.LastModifiedById = (int)LoggedInUserId;
                    onBoardTalents.TalentOnBoardingDate = DateTime.Now;
                    db.Entry(onBoardTalents).State = EntityState.Modified;
                    db.SaveChanges();

                    #region Insert HR History
                    string action = string.Empty;

                    switch (onBoardTalents.TalentOnBoardingStatusId)
                    {
                        case 1:
                            action = Action_Of_History.OnboardingTalent_Pending.ToString();
                            break;
                        case 2:
                            action = Action_Of_History.OnboardingTalent_Done.ToString();
                            break;
                        case 4:
                            action = Action_Of_History.OnboardingTalent_Raise_Concern.ToString();
                            break;
                        case 7:
                            action = Action_Of_History.OnboardingTalent_Concern_Resolved_and_Done.ToString();
                            break;
                    }
                    if (!string.IsNullOrEmpty(action))
                    {
                        object[] args = new object[] { action, hiringRequestId, ManagedTalentID, false, LoggedInUserId, 0, 0, "", OnboardId, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                    }
                    #endregion
                }
                else if (statusUpdate.Action == "OnboardingClient" && statusUpdate.OnboardingClient != null)
                {
                    onBoardTalents.ClientOnBoardingStatusId = statusUpdate.OnboardingClient.ClientOnBoardingStatusID;
                    onBoardTalents.ClientConcernRemark = statusUpdate.OnboardingClient.ClientConcernRemark;
                    onBoardTalents.LastModifiedDatetime = DateTime.Now;
                    onBoardTalents.LastModifiedById = (int)LoggedInUserId;
                    onBoardTalents.ClientOnBoardingDate = DateTime.Now;
                    db.Entry(onBoardTalents).State = EntityState.Modified;
                    db.SaveChanges();

                    #region Insert HR History
                    string action = string.Empty;

                    switch (onBoardTalents.ClientOnBoardingStatusId)
                    {
                        case 1:
                            action = Action_Of_History.OnboardingClient_Pending.ToString();
                            break;
                        case 2:
                            action = Action_Of_History.OnboardingClient_Done.ToString();
                            break;
                        case 3:
                            action = Action_Of_History.OnboardingClient_Raise_Concern.ToString();
                            break;
                        case 5:
                            action = Action_Of_History.OnboardingClient_Uncompleted_Data.ToString();
                            break;
                        case 7:
                            action = Action_Of_History.OnboardingClient_Concern_Resolved_and_Done.ToString();
                            break;
                        case 8:
                            action = Action_Of_History.OnboardingClient_In_Process.ToString();
                            break;
                    }
                    if (!string.IsNullOrEmpty(action))
                    {
                        object[] args = new object[] { action, hiringRequestId, ManagedTalentID, false, LoggedInUserId, 0, 0, "", OnboardId, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                    }
                    #endregion

                }
                else if (statusUpdate.Action == "LegalTalent" && statusUpdate.LegalTalent != null)
                {
                    onBoardTalents.TalentLegalStatusId = statusUpdate.LegalTalent.TalentLegalStatusID;
                    onBoardTalents.LastModifiedDatetime = DateTime.Now;
                    onBoardTalents.LastModifiedById = (int)LoggedInUserId;

                    if (statusUpdate.LegalTalent.TalentLegalDate != null)
                        onBoardTalents.TalentLegalDate = statusUpdate.LegalTalent.TalentLegalDate;
                    else
                        onBoardTalents.TalentLegalDate = DateTime.Now;

                    db.Entry(onBoardTalents).State = EntityState.Modified;
                    db.SaveChanges();

                    UpdateClientClosureDate(statusUpdate);

                    GenOnBoardTalentsReplacementDetail onBoardTalents_ReplacementDetails = db.GenOnBoardTalentsReplacementDetails.FirstOrDefault(x => x.NewOnBoardId == OnboardId && x.ReplaceTalentId == ManagedTalentID);
                    if (onBoardTalents_ReplacementDetails != null)
                    {
                        onBoardTalents_ReplacementDetails.TalentAgreementDateForReplacement = DateTime.Now;
                        db.Entry(onBoardTalents_ReplacementDetails).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    if (OnBoardClientContractDetails != null && statusUpdate.LegalTalent.ContractEndDate != null && statusUpdate.LegalTalent.ContractStartDate != null)
                    {
                        int monthDiff = CommonLogic.GetMonthDifference(statusUpdate.LegalTalent.ContractEndDate.Value, statusUpdate.LegalTalent.ContractStartDate.Value);
                        OnBoardClientContractDetails.ContractDuration = monthDiff;
                        OnBoardClientContractDetails.TotalDurationInMonths = monthDiff;
                        OnBoardClientContractDetails.ContractStartDate = statusUpdate.LegalTalent.ContractStartDate;
                        OnBoardClientContractDetails.ContractEndDate = statusUpdate.LegalTalent.ContractEndDate;
                        db.Entry(OnBoardClientContractDetails).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //Update AM/NBD Based on AM Assignment done or not during Talent/Client Legal sign.
                    _iOnboard.sproc_UTS_Update_SalesUserID_LegalOrKickoffDone(OnboardId);

                    #region Insert HR History
                    string action = string.Empty;

                    switch (onBoardTalents.TalentLegalStatusId)
                    {
                        case 1:
                            action = Action_Of_History.Legal_Talent_Pending.ToString();
                            break;
                        case 2:
                            action = Action_Of_History.Legal_Talent_Done.ToString();
                            break;
                        case 3:
                            action = Action_Of_History.Legal_Talent_Sent.ToString();
                            break;
                    }
                    if (!string.IsNullOrEmpty(action))
                    {
                        object[] args = new object[] { action, hiringRequestId, ManagedTalentID, false, LoggedInUserId, 0, 0, "", OnboardId, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                    }
                    #endregion


                }
                else if (statusUpdate.Action == "LegalClient" && statusUpdate.LegalClient != null)
                {
                    onBoardTalents.ClientLegalStatusId = statusUpdate.LegalClient.ClientLegalStatusID;
                    onBoardTalents.LastModifiedDatetime = DateTime.Now;
                    onBoardTalents.LastModifiedById = (int)LoggedInUserId;

                    if (statusUpdate.LegalClient.ClientLegalDate != null)
                        onBoardTalents.ClientLegalDate = statusUpdate.LegalClient.ClientLegalDate;
                    else
                        onBoardTalents.ClientLegalDate = DateTime.Now;

                    #region Update the SOWDocument link.
                    if (statusUpdate.LegalClient.SOWDocumentLink != null)
                    {
                        onBoardTalents.Sowdocument = statusUpdate.LegalClient.SOWDocumentLink;
                    }

                    #endregion

                    db.Entry(onBoardTalents).State = EntityState.Modified;
                    db.SaveChanges();

                    #region Update the MSASignedDate.

                    if (statusUpdate.LegalClient.MSASignDate != null)
                    {

                        long companyID = 0;
                        GenContact? contact = db.GenContacts.Where(x => x.Id == statusUpdate.ContactID).FirstOrDefault();
                        if (contact != null)
                        {
                            GenCompany? company = db.GenCompanies.Where(x => x.Id == contact.CompanyId).FirstOrDefault();
                            if (company != null)
                            {
                                companyID = company.Id;
                            }
                        }

                        if (companyID > 0)
                        {
                            var companyLegalInfos = db.GenCompanyLegalInfos.FirstOrDefault(x => x.CompanyId == companyID && x.DocumentName == "MSA Document");
                            if (companyLegalInfos != null)
                            {
                                companyLegalInfos.SignedDate = statusUpdate.LegalClient.MSASignDate;
                                db.Entry(companyLegalInfos).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                GenCompanyLegalInfo? genCompanyLegalInfo = new GenCompanyLegalInfo()
                                {
                                    CompanyId = companyID,
                                    DocumentType = "MSA (Master Service Agreement)",
                                    DocumentName = "MSA Document",
                                    DocumentUrl = statusUpdate.LegalClient.SOWDocumentLink,
                                    SignedDate = statusUpdate.LegalClient.MSASignDate,
                                    AgreementStatus = "Signed",
                                    CreatedById = Convert.ToInt32(LoggedInUserId),
                                    CreatedByDatetime = DateTime.Now,
                                    LegalDescription = "This agreement will be signed between Uplers and client. We execute this agreement before sharing talent profile with client."
                                };

                                db.Entry(genCompanyLegalInfo).State = EntityState.Added;
                                db.SaveChanges();
                            }
                        }
                    }

                    #endregion                   

                    DateTime? LastWorkingDay = statusUpdate.LegalClient.Lastworkingdate != null ? statusUpdate.LegalClient.Lastworkingdate : (DateTime?)null;

                    GenOnBoardTalentsReplacementDetail? genOnBoardTalentsReplacementDetails = db.GenOnBoardTalentsReplacementDetails.Where(x => x.ReplaceTalentId == talentId && x.ContactId == contactId && x.NewOnBoardId == OnboardId && x.HiringRequestId == hiringRequestId && x.LastWorkingDay == null).FirstOrDefault();
                    if (genOnBoardTalentsReplacementDetails != null && genOnBoardTalentsReplacementDetails.LastWorkingDay == null)
                    {
                        genOnBoardTalentsReplacementDetails.LastWorkingDay = LastWorkingDay;
                        db.Entry(genOnBoardTalentsReplacementDetails).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    UpdateClientClosureDate(statusUpdate);

                    GenOnBoardTalentsReplacementDetail? onBoardTalents_ReplacementDetails = db.GenOnBoardTalentsReplacementDetails.Where(x => x.NewOnBoardId == OnboardId && x.ReplaceTalentId == ManagedTalentID).FirstOrDefault();
                    if (onBoardTalents_ReplacementDetails != null)
                    {
                        onBoardTalents_ReplacementDetails.ClientAgreementDateForReplacement = DateTime.Now;

                        db.Entry(onBoardTalents_ReplacementDetails).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    if (statusUpdate.LegalClient.CompanyLegalDocID != 0)
                    {
                        GenOnBoardTalentsLegalDetail _LegalDetails = db.GenOnBoardTalentsLegalDetails.FirstOrDefault(x => x.OnBoardId == OnboardId);
                        if (_LegalDetails == null)
                        {
                            GenOnBoardTalentsLegalDetail onBoardTalents_LegalDetails = new GenOnBoardTalentsLegalDetail();
                            onBoardTalents_LegalDetails.CompanyLegalId = statusUpdate.LegalClient.CompanyLegalDocID;
                            onBoardTalents_LegalDetails.OnBoardId = OnboardId;
                            onBoardTalents_LegalDetails.HiringRequestId = onBoardTalents.HiringRequestId;
                            db.GenOnBoardTalentsLegalDetails.Add(onBoardTalents_LegalDetails);
                            db.SaveChanges();
                        }
                    }

                    //Update AM/NBD Based on AM Assignment done or not during Talent/Client Legal sign.
                    _iOnboard.sproc_UTS_Update_SalesUserID_LegalOrKickoffDone(OnboardId);

                    #region Insert HR History
                    string action = string.Empty;

                    switch (onBoardTalents.ClientLegalStatusId)
                    {
                        case 1:
                            action = Action_Of_History.Legal_Client_Pending.ToString();
                            break;
                        case 2:
                            action = Action_Of_History.Legal_Client_Done.ToString();
                            break;
                        case 3:
                            action = Action_Of_History.Legal_Client_Sent.ToString();
                            break;
                    }
                    if (!string.IsNullOrEmpty(action))
                    {
                        object[] args = new object[] { action, hiringRequestId, ManagedTalentID, false, LoggedInUserId, 0, 0, "", OnboardId, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                    }
                    #endregion

                }
                else if (statusUpdate.Action == "KickOff" && statusUpdate.KickOff != null)
                {
                    onBoardTalents.KickoffStatusId = statusUpdate.KickOff.KickoffStatusID;
                    onBoardTalents.KickoffTimezonePreferenceId = statusUpdate.KickOff.KickoffTimezonePreferenceId ?? 1;
                    onBoardTalents.LastModifiedDatetime = DateTime.Now;
                    onBoardTalents.LastModifiedById = (int)LoggedInUserId;
                    statusUpdate.KickOff.KickoffDate = statusUpdate.KickOff.KickoffDate ?? DateTime.Now;

                    if (statusUpdate.KickOff.KickoffDate != null)
                    {
                        onBoardTalents.KickoffDate = statusUpdate.KickOff.KickoffDate;

                        DateTime Dt_kickoffDate = statusUpdate.KickOff.KickoffDate.Value;

                        if (onBoardTalents.IsHrmanaged == false)
                        {
                            PrgContactTimeZone? timeZone = null;

                            if (onBoardTalents.KickoffTimezonePreferenceId > 0)
                            {
                                timeZone = db.PrgContactTimeZones.FirstOrDefault(y => y.Id == onBoardTalents.KickoffTimezonePreferenceId);
                            }

                            if (OnBoardClientContractDetails != null)
                            {
                                if (timeZone != null && !string.IsNullOrEmpty(timeZone.Description))
                                {
                                    string googleMeetUrl = configuration["GoogleMeetPythonURL"].ToString();
                                    meeting = ExternalCalls.ToScheduleZoomMeeting(timeZone.Description, Dt_kickoffDate, googleMeetUrl);

                                    if (meeting != null)
                                    {
                                        // If kickoff scheduled then only generate the Zoom information.
                                        if (onBoardTalents.KickoffStatusId == 4)
                                        {
                                            onBoardTalents.ZoomInterviewLink = meeting.join_url;
                                            onBoardTalents.ZoomMeetingId = Convert.ToString(meeting.id);
                                            onBoardTalents.ZoomMeetingscheduledOn = Dt_kickoffDate;
                                            onBoardTalents.ZoomInterviewKitUsername = meeting.host_email;
                                            onBoardTalents.ZoomInterviewKitPassword = meeting.password;

                                            if (onBoardTalents.ZoomStartTime != null)
                                            {
                                                onBoardTalents.ZoomStartTime = onBoardTalents.ZoomStartTime;
                                            }

                                            if (onBoardTalents.ZoomEndTime != null)
                                            {
                                                onBoardTalents.ZoomEndTime = onBoardTalents.ZoomEndTime;
                                            }
                                        }

                                        db.Entry(onBoardTalents).State = EntityState.Modified;
                                        db.SaveChanges();

                                        if (onBoardTalents.KickoffStatusId == 5 && onBoardTalents.AmSalesPersonId != null)
                                        {
                                            _iOnboard.sproc_UTS_Update_SalesUserID_LegalOrKickoffDone(OnboardId);
                                        }

                                        #region Insert HR History
                                        string action = string.Empty;

                                        switch (onBoardTalents.KickoffStatusId)
                                        {
                                            case 1:
                                                action = Action_Of_History.Kick_Off_Pending.ToString();
                                                break;
                                            case 4:
                                                action = Action_Of_History.Kick_Off_Schedule.ToString();
                                                break;
                                            case 5:
                                                action = Action_Of_History.Kick_Off_Done.ToString();
                                                break;
                                        }
                                        if (!string.IsNullOrEmpty(action))
                                        {
                                            object[] args = new object[] { action, hiringRequestId, ManagedTalentID, false, LoggedInUserId, 0, 0, "", OnboardId, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                                            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        else
                        {
                            db.Entry(onBoardTalents).State = EntityState.Modified;
                            db.SaveChanges();

                            #region Insert HR History
                            string action = string.Empty;

                            switch (onBoardTalents.KickoffStatusId)
                            {
                                case 1:
                                    action = Action_Of_History.Kick_Off_Pending.ToString();
                                    break;
                                case 4:
                                    action = Action_Of_History.Kick_Off_Schedule.ToString();
                                    break;
                                case 5:
                                    action = Action_Of_History.Kick_Off_Done.ToString();
                                    break;
                            }
                            if (!string.IsNullOrEmpty(action))
                            {
                                object[] args = new object[] { action, hiringRequestId, ManagedTalentID, false, LoggedInUserId, 0, 0, "", OnboardId, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                            }
                            #endregion
                        }
                    }

                    #region Update After kickoff Details

                    if (statusUpdate.KickOff.IsAfterKickOff && statusUpdate.AfterKickOff != null)
                    {
                        if (OnBoardClientContractDetails != null && OnBoardClientContractDetails.ContractStartDate != null && OnBoardClientContractDetails.ContractStartDate != null)
                        {
                            int monthDiff = CommonLogic.GetMonthDifference(OnBoardClientContractDetails.ContractEndDate.Value, OnBoardClientContractDetails.ContractStartDate.Value);
                            OnBoardClientContractDetails.ContractDuration = monthDiff;
                            OnBoardClientContractDetails.TotalDurationInMonths = monthDiff;
                            OnBoardClientContractDetails.ContractStartDate = OnBoardClientContractDetails.ContractStartDate;
                            OnBoardClientContractDetails.ContractEndDate = OnBoardClientContractDetails.ContractEndDate;
                            db.Entry(OnBoardClientContractDetails).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        //Handled the logic to insert the payout summary for the onboarding details.
                        GenOnBoardTalent? _OnBoardTalents = db.GenOnBoardTalents.Where(x => x.Id == OnboardId).FirstOrDefault();
                        var Currency = "USD";
                        long HiringRequestDetail_Id = 0;
                        if (_OnBoardTalents != null)
                        {
                            HiringRequestDetail_Id = Convert.ToInt64(_OnBoardTalents.HiringRequestDetailId);
                            GenSalesHiringRequestDetail? _SalesHiringRequest_Details = db.GenSalesHiringRequestDetails.Where(x => x.Id == HiringRequestDetail_Id).FirstOrDefault();
                            if (_SalesHiringRequest_Details != null)
                            {
                                Currency = _SalesHiringRequest_Details.Currency;
                            }
                        }
                        PrgCurrencyExchangeRate? currencyExchangeRate = db.PrgCurrencyExchangeRates.Where(x => x.CurrencyCode == Currency).FirstOrDefault();
                        if (currencyExchangeRate != null)
                        {
                            var CurrencyRateINR = currencyExchangeRate.ExchangeRate;
                            object[] param = new object[]
                            {
                               OnboardId,
                               Convert.ToDecimal(CurrencyRateINR),
                               Currency,
                               statusUpdate.AfterKickOff.ZohoInvoiceNumber,
                               statusUpdate.AfterKickOff.InvoiceAmount
                            };

                            string paramString = CommonLogic.ConvertToParamString(param);
                            _iOnboard.Sproc_Insert_Onboarding_Summary(paramString);


                        }
                    }

                    #endregion


                    #region ATS Call
                    if (onBoardTalents.Id != null)
                    {
                        if (!configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                        {

                            Sproc_talent_engagement_Details_For_PHP_API_Result result = await _iEngagement.TalentEngagementDetails(onBoardTalents.Id, "OnBoarding Status Updates");
                            if (result != null)
                            {
                                TalentEngagementDetailsViewModel engagementDetails = new()
                                {
                                    HiringRequest_ID = result.HiringRequest_ID,
                                    ATSTalentId = result.ATS_Talent_ID,
                                    engagement_id = result.EngagemenID,
                                    engagement_start_date = result.ContractStartDate,
                                    engagement_end_date = result.ContractEndDate,
                                    engagement_status = result.EngagementStatus,
                                    talent_status = result.Talent_status,
                                    joining_date = result.joining_date,
                                    lost_date = result.Lost_date,
                                    last_working_date = result.Last_working_date,
                                    talent_statustag = result.talent_statustag,
                                    Action = result.Action,
                                    Action_date = result.Action_date
                                };

                                var json = JsonConvert.SerializeObject(engagementDetails);
                                ATSCall aTSCall = new(configuration, db);
                                aTSCall.SendTalentEngagementDetails(json, LoggedInUserId, result.HiringRequest_ID);
                            }
                        }
                    }
                    #endregion

                }

                if (onBoardTalents.IsHrmanaged == false)
                {
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime, null);
                }

                #region SaveReplacementDetails

                // UTS-7389: If replacement is Yes then save replacement details
                try
                {
                    bool isReplacement = Convert.ToBoolean(statusUpdate.IsReplacement);
                    await SaveUpdateReplacementDetails(statusUpdate.talentReplacement, isReplacement);
                }
                catch
                {

                }

                #endregion

                #region Email Binder class
                EmailBinder binder = new EmailBinder(configuration, db);
                #endregion

                if (statusUpdate.Action == "OnboardingClient" && statusUpdate.OnboardingClient != null)
                {
                    if (statusUpdate.OnboardingClient.ClientOnBoardingStatusID == 1)
                    {
                        if (onBoardTalents.IsHrmanaged == false)
                        {
                            IsSentEmail = binder.SendEmailNotificationToInternalTeampreonboardingisInProcess(OnboardId, talentId, hiringRequestId);
                        }
                    }
                    else if (statusUpdate.OnboardingClient.ClientOnBoardingStatusID == 2)
                    {
                        if (onBoardTalents.IsHrmanaged == false)
                        {
                            //bool IsClientOnBoardEmailSend = Convert.ToBoolean(onBoardTalents.IsClientOnBoardEmailSend);
                            //if (!IsClientOnBoardEmailSend)
                            //{
                            //    if (_Contact != null)
                            //    {
                            //        bool IsClientNotificationSent = _Contact.IsClientNotificationSend;

                            //        if (IsClientNotificationSent)
                            //        {
                            //            IsSentEmail = binder.SendEmailToClientpreonboardingDone(OnboardId, talentId, hiringRequestId);
                            //        }
                            //    }
                            //}

                            //if (_Talent != null)
                            //{
                            //    bool IsTalentNotificationSent = _Talent.IsTalentNotificationSend ?? false;
                            //    if (IsTalentNotificationSent)
                            //    {
                            //        IsSentEmail = binder.SendEmailToTalentpreonboardingisDone(OnboardId, talentId, hiringRequestId);
                            //    }
                            //}
                        }
                    }
                }
                else if (statusUpdate.Action == "KickOff" && statusUpdate.KickOff != null && statusUpdate.KickOff.KickoffStatusID == 4)
                {
                    if (meeting != null)
                    {
                        if (onBoardTalents.IsHrmanaged == false)
                        {
                            onBoardTalents = db.GenOnBoardTalents.FirstOrDefault(m => m.Id == statusUpdate.OnboardID);
                            db.Entry(onBoardTalents).Reload();

                            if (onBoardTalents != null)
                            {
                                DateTime scheduleTime = onBoardTalents.ZoomMeetingscheduledOn ?? DateTime.Now;
                                DateTime scheduleTillTime = onBoardTalents.ZoomMeetingscheduledOn ?? DateTime.Now;

                                // For Talent
                                if (onBoardTalents.IstZoomStartTime != null && onBoardTalents.ZoomMeetingscheduledOn != null)
                                {
                                    scheduleTime = onBoardTalents.ZoomMeetingscheduledOn.Value.Add(onBoardTalents.IstZoomStartTime.Value);
                                }

                                if (onBoardTalents.IstZoomEndTime != null && onBoardTalents.ZoomMeetingscheduledOn != null)
                                {
                                    scheduleTillTime = onBoardTalents.ZoomMeetingscheduledOn.Value.Add(onBoardTalents.IstZoomEndTime.Value);
                                }

                                var timezonevalue = Convert.ToInt32(OnBoardClientContractDetails.TalentWorkingTimeZone);
                                PrgContactTimeZone? Talent_timeZone = db.PrgContactTimeZones.Where(y => y.Id == timezonevalue).FirstOrDefault();
                                string? timezone_shortName = string.Empty;
                                if (Talent_timeZone != null)
                                {
                                    timezone_shortName = Talent_timeZone.ShortName;
                                }

                                DateTime Client_scheduleTime = onBoardTalents.ZoomMeetingscheduledOn ?? DateTime.Now;
                                DateTime Client_scheduleTillTime = onBoardTalents.ZoomMeetingscheduledOn ?? DateTime.Now;

                                // For client
                                if (onBoardTalents.ZoomStartTime != null && onBoardTalents.ZoomMeetingscheduledOn != null)
                                {
                                    Client_scheduleTime = onBoardTalents.ZoomMeetingscheduledOn.Value.Add(onBoardTalents.ZoomStartTime.Value);
                                }

                                if (onBoardTalents.ZoomEndTime != null && onBoardTalents.ZoomMeetingscheduledOn != null)
                                {
                                    Client_scheduleTillTime = onBoardTalents.ZoomMeetingscheduledOn.Value.Add(onBoardTalents.ZoomEndTime.Value);
                                }

                                var Client_timezonevalue = statusUpdate.KickOff.KickoffTimezonePreferenceId;
                                PrgContactTimeZone? Client_timeZone = db.PrgContactTimeZones.FirstOrDefault(y => y.Id == Client_timezonevalue);
                                string? Client_timezone_shortName = string.Empty;
                                if (Client_timeZone != null)
                                {
                                    Client_timezone_shortName = Client_timeZone.ShortName;
                                }

                                //if (_Contact != null)
                                //{
                                //    bool IsClientNotificationSent = _Contact.IsClientNotificationSend;
                                //    if (IsClientNotificationSent)
                                //    {
                                //        IsSentEmail = binder.SendEmailToClientForKickOff(OnboardId, talentId, hiringRequestId, meeting, Client_scheduleTime, Client_timezone_shortName, Client_scheduleTillTime);
                                //    }
                                //}

                                //if (_Talent != null)
                                //{
                                //    bool IsTalentNotificationSent = _Talent.IsTalentNotificationSend ?? false;
                                //    if (IsTalentNotificationSent)
                                //    {
                                //        IsSentEmail = binder.SendEmailToTalentForKickOff(OnboardId, talentId, hiringRequestId, meeting, scheduleTime, timezone_shortName, scheduleTillTime);
                                //    }
                                //}
                                //Send mail to team
                                IsSentEmail = binder.SendEmailToSalesTeamForKickOff(OnboardId, talentId, hiringRequestId, meeting, scheduleTime, scheduleTillTime, statusUpdate.KickOff.KickoffDate, Talent_timeZone);
                            }
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Zoom meeting is not initialized, Kindly contact Administrator" });
                    }
                }

                #region ATS call



                if (configuration["HRDataSendSwitchForPHP"].ToLower() != "local" && hiringRequestId > 0)
                {
                    var varHRdata = db.GenSalesHiringRequests.AsNoTracking().Where(t => t.Id == hiringRequestId).FirstOrDefault();
                    if (varHRdata != null)
                    {
                        //var varTalentList = db.GenContactTalentPriorities.AsNoTracking().Where(x => x.HiringRequestId == hiringRequestId && x.TalentId == statusUpdate.TalentID).ToList();
                        var varTalentList = db.GenContactTalentPriorities.AsNoTracking().Where(x => x.HiringRequestId == hiringRequestId && x.TalentId == statusUpdate.TalentID).FirstOrDefault();
                        if (varTalentList != null)
                        {
                            ContactTalentPriorityModel objCTPModel = new();
                            talentId = varTalentList.TalentId ?? 0;
                            objCTPModel.HRStatusID = varHRdata.StatusId ?? 0;
                            string HiringRequestStatus = "";
                            var varHRStatusData = db.PrgHiringRequestStatuses.AsNoTracking().Where(x => x.Id == objCTPModel.HRStatusID).FirstOrDefault();
                            if (varHRStatusData != null)
                                HiringRequestStatus = varHRStatusData.HiringRequestStatus;

                            objCTPModel.HRStatus = HiringRequestStatus;
                            objCTPModel.HRID = hiringRequestId;
                            //CallAtsAPIToSendTalentAndHRStatus api call
                            objCTPModel.TalentDetails = new();
                            //foreach (var talentID in varTalentList)
                            //{
                            var varGenTalent = db.GenTalents.AsNoTracking().Where(x => x.Id == talentId).FirstOrDefault();

                            TalentDetail talentDetail = new();
                            talentDetail.TalentStatus = "";

                            talentDetail.ATS_TalentID = Convert.ToInt64(varGenTalent.AtsTalentId);
                            //var TalentCTP_Details = varTalentList.Where(x => x.TalentId == varGenTalent.Id && x.HiringRequestId == hiringRequestId).FirstOrDefault();
                            var TalentCTP_Details = varTalentList;
                            if (TalentCTP_Details != null)
                            {
                                var TalStatusClientSelectionDetail = db.PrgTalentStatusAfterClientSelections.AsNoTracking().Where(x => x.Id == TalentCTP_Details.TalentStatusIdBasedOnHr).FirstOrDefault();
                                if (TalStatusClientSelectionDetail != null)
                                {
                                    talentDetail.TalentStatus = TalStatusClientSelectionDetail.TalentStatus;
                                }
                                //talentDetail.MatchMakingDateTime = Convert.ToDateTime(TalentCTP_Details.CreatedByDatetime).ToString("dd-MM-yyyy hh:mm:ss");

                            }
                            talentDetail.UTS_TalentID = varGenTalent.Id;
                            talentDetail.Talent_USDCost = varGenTalent.FinalCost ?? 0;

                            object[] objParam = new object[] { hiringRequestId, varGenTalent.Id };
                            string strParamas = CommonLogic.ConvertToParamString(objParam);
                            var varTalent_RejectReason = commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas).ActualReason;

                            talentDetail.Talent_RejectReason = varTalent_RejectReason;
                            var varUsrUserById = commonInterface.TalentStatus.GetUsrUserById(SessionValues.LoginUserId);

                            try
                            {
                                if (varUsrUserById != null)
                                {
                                    talentDetail.RejectedBy = varUsrUserById.EmployeeId;
                                    talentDetail.ActionUserName = varUsrUserById.FullName;
                                    talentDetail.ActionUserEmail = varUsrUserById.EmailId;
                                    talentDetail.ActionBy = Convert.ToString(StatusChangeAction.Sales);
                                }
                            }
                            catch
                            {

                            }


                            // UTS-7093: Fetch the round details and send to ATS.
                            try
                            {
                                // UTS-7093: Fetch the round details and send to ATS.
                                object[] param = new object[] { hiringRequestId, 0, varGenTalent.Id };
                                string paramString = CommonLogic.ConvertToParamString(param);

                                ATSCall aTSCallforRound = new ATSCall(configuration, db);

                                sproc_Get_InterviewRoundDetails_Result roundDetails = aTSCallforRound.sproc_Get_InterviewRoundDetails(paramString);
                                if (roundDetails != null)
                                {
                                    string? talentStatusRoundDetails = roundDetails.StrInterviewRound;
                                    if (!string.IsNullOrEmpty(talentStatusRoundDetails))
                                    {
                                        talentDetail.TalentStatusRoundDetails = talentStatusRoundDetails;
                                    }
                                }
                            }
                            catch
                            {

                            }

                            objCTPModel.TalentDetails.Add(talentDetail);
                            //}
                            try
                            {
                                var json = JsonConvert.SerializeObject(objCTPModel);
                                ATSCall aTSCall = new ATSCall(configuration, db);
                                aTSCall.SaveContactTalentPriority(json, LoggedInUserId, hiringRequestId);
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully" });
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully" });
                            }
                        }
                    }
                }
                #endregion

                #region ATS Call
                if (onBoardTalents.Id != null)
                {
                    if (!configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                    {

                        Sproc_talent_engagement_Details_For_PHP_API_Result result = await _iEngagement.TalentEngagementDetails(onBoardTalents.Id, "OnBoard Status Update");
                        if (result != null)
                        {
                            TalentEngagementDetailsViewModel engagementDetails = new()
                            {
                                HiringRequest_ID = result.HiringRequest_ID,
                                ATSTalentId = result.ATS_Talent_ID,
                                engagement_id = result.EngagemenID,
                                engagement_start_date = result.ContractStartDate,
                                engagement_end_date = result.ContractEndDate,
                                engagement_status = result.EngagementStatus,
                                talent_status = result.Talent_status,
                                joining_date = result.joining_date,
                                lost_date = result.Lost_date,
                                last_working_date = result.Last_working_date,
                                talent_statustag = result.talent_statustag,
                                Action = result.Action,
                                Action_date = result.Action_date
                            };

                            var json = JsonConvert.SerializeObject(engagementDetails);
                            ATSCall aTSCall = new(configuration, db);
                            aTSCall.SendTalentEngagementDetails(json, LoggedInUserId, result.HiringRequest_ID);
                        }
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = statusUpdate.Action + " Successfully" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("ViewInDetail")]
        public ObjectResult ViewOnBoardDetail(long onBoardId)
        {

            #region PreValidation
            OnboardContractDetailViewModel onBoardUpdateviewModel = new OnboardContractDetailViewModel();

            if (onBoardId == null || onBoardId == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide propoer onBoardID" });
            }
            #endregion

            object[] param = new object[] { onBoardId };

            Sproc_UTS_Get_OnBoardContract_Details_Result onBoardContract_Details_Result = _iOnboard.Sproc_UTS_Get_OnBoardContract_Details(CommonLogic.ConvertToParamString(param));
            List<Sproc_UTS_Get_OnBoardClientTeamMemberDeatils_Result> OnBoardClientTeamMemberDeatils = _iOnboard.GetOnBoardClientTeamMemberDeatils(CommonLogic.ConvertToParamString(param));
            if (onBoardContract_Details_Result != null)
            {
                onBoardUpdateviewModel.onboardContractDetails = onBoardContract_Details_Result;
            }
            if (OnBoardClientTeamMemberDeatils != null)
            {
                onBoardUpdateviewModel.onBoardClientTeamMembers = OnBoardClientTeamMemberDeatils;
            }
            if (onBoardUpdateviewModel != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Details = onBoardUpdateviewModel });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "OnBoard is not found" });
            }

        }

        [HttpGet("SyncEngagementDetailToATS")]
        public async Task<ObjectResult> SyncEngagementDetailToATS(long onBoardId)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;

                #region PreValidation
                if (onBoardId == null || onBoardId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide propoer onBoardID" });
                }
                #endregion

                #region ATS Call
                if (!configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    Sproc_talent_engagement_Details_For_PHP_API_Result ObjResult = await _iEngagement.TalentEngagementDetails(onBoardId, "Sync Call");
                    if (ObjResult != null)
                    {
                        TalentEngagementDetailsViewModel engagementDetails = new()
                        {
                            HiringRequest_ID = ObjResult.HiringRequest_ID,
                            ATSTalentId = ObjResult.ATS_Talent_ID,
                            engagement_id = ObjResult.EngagemenID,
                            engagement_start_date = ObjResult.ContractStartDate,
                            engagement_end_date = ObjResult.ContractEndDate,
                            engagement_status = ObjResult.EngagementStatus,
                            talent_status = ObjResult.Talent_status,
                            joining_date = ObjResult.joining_date,
                            lost_date = ObjResult.Lost_date,
                            last_working_date = ObjResult.Last_working_date,
                            talent_statustag = ObjResult.talent_statustag,
                            Action = ObjResult.Action,
                            Action_date = ObjResult.Action_date
                        };

                        var json = JsonConvert.SerializeObject(engagementDetails);
                        ATSCall aTSCall = new(configuration, db);
                        aTSCall.SendTalentEngagementDetails(json, LoggedInUserId, ObjResult.HiringRequest_ID);
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully" });
            }
        }

        [HttpPost("SaveRenewalInitiatedDetail")]
        public ObjectResult SaveRenewalInitiatedDetail(long onBoardId, string IsRenewalInitiated)
        {

            #region PreValidation

            if (onBoardId == null || onBoardId == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide propoer onBoardID" });
            }
            #endregion

            object[] param = new object[] { onBoardId, IsRenewalInitiated };
            string paramString = CommonLogic.ConvertToParamStringWithNull(param);
            _iOnboard.UpdateRenewalInitiated(paramString);


            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Record Updated Successfully." });


        }

        [HttpGet("FetchClientLegalInfo")]
        public ObjectResult FetchClientLegalInfo(long hrId)
        {
            try
            {
                #region PreValidation
                if (hrId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide proper HR" });
                }
                #endregion

                GenSalesHiringRequest? _SalesHiringRequest = new GenSalesHiringRequest();
                string? msaSignedDate = string.Empty;

                _SalesHiringRequest = db.GenSalesHiringRequests.Where(x => x.Id == hrId).FirstOrDefault();
                if (_SalesHiringRequest == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Invalid HR." });
                }

                var contactId = _SalesHiringRequest.ContactId;
                long companyID = 0;
                GenContact? contact = db.GenContacts.Where(x => x.Id == contactId).FirstOrDefault();
                if (contact != null)
                {
                    GenCompany? company = db.GenCompanies.Where(x => x.Id == contact.CompanyId).FirstOrDefault();
                    if (company != null)
                    {
                        companyID = company.Id;
                    }
                }

                if (companyID > 0)
                {
                    var companyLegalInfos = db.GenCompanyLegalInfos.FirstOrDefault(x => x.CompanyId == companyID && x.DocumentName == "MSA Document");
                    if (companyLegalInfos != null)
                    {
                        if (companyLegalInfos.SignedDate.HasValue)
                        {
                            msaSignedDate = companyLegalInfos.SignedDate.Value.ToString("yyyy-MM-dd");
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success.", Details = msaSignedDate });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("FetchTalentOnBoardInfo")]
        public async Task<ObjectResult> FetchTalentOnBoardInfo(long onBoardId)
        {
            try
            {
                #region PreValidation             

                if (onBoardId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide proper onBoardID" });
                }
                #endregion

                OnBoardStatusDetails onBoardStatusDetails = new OnBoardStatusDetails();

                GenOnBoardTalent? genOnBoardTalent = db.GenOnBoardTalents.FirstOrDefault(x => x.Id == onBoardId);

                if (genOnBoardTalent != null)
                {
                    onBoardStatusDetails.GenOnBoardTalent = genOnBoardTalent;

                    #region SetTabEnableDisable

                    if (genOnBoardTalent != null)
                    {
                        onBoardStatusDetails.IsThirdTabReadOnly = genOnBoardTalent.ClientLegalStatusId == 2;
                    }

                    if (genOnBoardTalent != null)
                    {
                        onBoardStatusDetails.IsFourthTabReadOnly = genOnBoardTalent.KickoffStatusId == 4;
                    }

                    #endregion

                    #region GetReplacementDetails

                    // UTS-7389: If replacement is Yes then save replacement details
                    try
                    {
                        var replacementDetails = GetReplacementDetails(onBoardId);
                        if (replacementDetails != null)
                        {
                            onBoardStatusDetails.ReplacementDetail = replacementDetails;
                        }

                        var replacementEngHRs = await ListofHRAndEngs(onBoardId);
                        if (replacementEngHRs != null)
                        {
                            onBoardStatusDetails.ReplacementEngAndHR = replacementEngHRs;
                        }

                    }
                    catch
                    {

                    }
                    #endregion

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success.", Details = onBoardStatusDetails });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Not found" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("OnboardList")]
        public async Task<IActionResult> Onboard([FromBody] OnBoardListViewModel onBoardListViewModel)
        {
            try
            {
                #region PreValidation
                if (onBoardListViewModel == null || (onBoardListViewModel.pagenumber == 0 || onBoardListViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion


                string Sortdatafield = "CreatedbyDatetime";
                string Sortorder = "DESC";
                string search = "";

                if (!string.IsNullOrEmpty(onBoardListViewModel.filterFields_OnBoard.search))
                {
                    search = onBoardListViewModel.filterFields_OnBoard.search.Replace("'", "''");
                }

                object[] param = new object[] {
                onBoardListViewModel.pagenumber, onBoardListViewModel.totalrecord, Sortdatafield, Sortorder,
                onBoardListViewModel.filterFields_OnBoard.Engagement_Id,
                onBoardListViewModel.filterFields_OnBoard.Position,
                onBoardListViewModel.filterFields_OnBoard.Client,
                onBoardListViewModel.filterFields_OnBoard.Talent,
                onBoardListViewModel.filterFields_OnBoard.HR_Number,
                onBoardListViewModel.filterFields_OnBoard.Company,
                onBoardListViewModel.filterFields_OnBoard.Final_HR_Cost,
                onBoardListViewModel.filterFields_OnBoard.Talent_Cost,
                onBoardListViewModel.filterFields_OnBoard.NRPercentage,
                onBoardListViewModel.filterFields_OnBoard.SalesPerson,
                onBoardListViewModel.filterFields_OnBoard.AMAssignmentuser,
                onBoardListViewModel.filterFields_OnBoard.ContractStatus,
                onBoardListViewModel.filterFields_OnBoard.IsLost,
                onBoardListViewModel.filterFields_OnBoard.DPAmount,
                onBoardListViewModel.filterFields_OnBoard.DP_Percentage,
                onBoardListViewModel.filterFields_OnBoard.TypeOfHR,
                search
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_OnBoardTalents_Result> OnboardList = await _iOnboard.GetOnBoards(paramasString).ConfigureAwait(false);

                if (OnboardList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = OnboardList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Onboard Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private methods
        private void UpdateClientClosureDate(OnBoardingStatusUpdate statusUpdate)
        {
            GenOnBoardTalent OnBoardTalents = db.GenOnBoardTalents.Where(x => x.Id == statusUpdate.OnboardID).FirstOrDefault();

            if (OnBoardTalents.TalentLegalStatusId == 2 && OnBoardTalents.ClientLegalStatusId == 2)
            {
                var Client_ClosureDate = (OnBoardTalents.ClientLegalDate > OnBoardTalents.TalentLegalDate ? OnBoardTalents.ClientLegalDate : OnBoardTalents.TalentLegalDate);
                OnBoardTalents.ClientClosureDate = Client_ClosureDate;
                db.Entry(OnBoardTalents).State = EntityState.Modified;
                db.SaveChanges();

                var genContactTalentPriority =
                    db.GenContactTalentPriorities.
                    Where(x => x.HiringRequestId == statusUpdate.HiringRequestID &&
                    x.TalentId == statusUpdate.TalentID).FirstOrDefault();

                if (genContactTalentPriority != null)
                {
                    genContactTalentPriority.TalentStatusIdBasedOnHr = 10;//Hire
                    db.Entry(genContactTalentPriority).State = EntityState.Modified;
                    db.SaveChanges();
                }

                GenOnBoardTalentsReplacementDetail Replacement_OnBoardTalent = db.GenOnBoardTalentsReplacementDetails.Where(x => x.NewOnBoardId == statusUpdate.OnboardID).FirstOrDefault();
                if (Replacement_OnBoardTalent != null)
                {
                    Replacement_OnBoardTalent.ReplaceTalentClosureDate = Client_ClosureDate;
                    db.Entry(Replacement_OnBoardTalent).State = EntityState.Modified;
                    db.SaveChanges();
                }

                object[] param = new object[]
                {
                    statusUpdate.OnboardID, statusUpdate.TalentID, statusUpdate.HiringRequestID, statusUpdate.ContactID, null
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                _iOnboard.sproc_UTS_Update_LastWorkingDay_For_TalentinReplacement(paramasString);
            }
        }

        private async Task SaveUpdateReplacementDetails(TalentReplacement talentReplacement, bool IsReplacement)
        {
            long CreatedByID = SessionValues.LoginUserId;

            if (Convert.ToBoolean(IsReplacement) && talentReplacement != null)
            {
                talentReplacement = await _iTalentReplacement.SaveTalentReplacementData(talentReplacement, _universalProcRunner).ConfigureAwait(false);

                talentReplacement.HiringRequestID = talentReplacement.HiringRequestID;
                int talentStatusID = (short)prg_TalentStatus_AfterClientSelection.InReplacement;

                if (!configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    ContactTalentPriorityResponseModel contactTalentPriorityResponseModel = new ContactTalentPriorityResponseModel();
                    ATSCall aTSCall = new ATSCall(configuration, db);
                    GenTalent _Talents = await _iTalentReplacement.GetGenTalentsById(talentReplacement.TalentId.Value).ConfigureAwait(false);
                    db.Entry(_Talents).Reload();
                    if (_Talents != null)
                    {
                        var HiringRequestData = await _iTalentReplacement.GetGenSalesHiringRequestById(talentReplacement.HiringRequestID.Value).ConfigureAwait(false);
                        if (HiringRequestData != null)
                        {
                            #region Save Data in model to send reponse to PHP team after serialize   

                            contactTalentPriorityResponseModel.HRID = talentReplacement.HiringRequestID.Value;
                            contactTalentPriorityResponseModel.HRStatusID = HiringRequestData.StatusId ?? 0;

                            string TalentStatus = string.Empty;
                            if (talentStatusID > 0)
                            {
                                TalentStatus = db.PrgTalentStatusAfterClientSelections.Where(x => x.Id == talentStatusID && x.IsActive == true).FirstOrDefault()?.TalentStatus;
                            }

                            var HRStatusData = await _iTalentReplacement.GetPrgHiringRequestStatusById(contactTalentPriorityResponseModel.HRStatusID).ConfigureAwait(false);
                            if (HRStatusData != null)
                                contactTalentPriorityResponseModel.HRStatus = HRStatusData.HiringRequestStatus;

                            var genTalentBinded = await _iTalentReplacement.GenContactTalentPriorityByTalentIDorHiringRequestID(_Talents.Id, talentReplacement.HiringRequestID.Value).ConfigureAwait(false);

                            outTalentDetail talentDetail = new outTalentDetail();
                            talentDetail.ATS_TalentID = _Talents.AtsTalentId ?? 0;
                            talentDetail.TalentStatus = TalentStatus;
                            talentDetail.UTS_TalentID = _Talents.Id;
                            talentDetail.Talent_USDCost = _Talents.FinalCost ?? 0;
                            talentDetail.availibility = HiringRequestData.Availability;
                            talentDetail.noticeperiod = talentReplacement.Noticeperiod.ToString();
                            talentDetail.MatchMakingDateTime = Convert.ToDateTime(genTalentBinded.CreatedByDatetime).ToString("dd-MM-yyyy hh:mm:ss");

                            object[] objParam = new object[] { contactTalentPriorityResponseModel.HRID, _Talents.Id };
                            string strParamas = CommonLogic.ConvertToParamString(objParam);
                            var varTalent_RejectReason = commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas).ActualReason;
                            talentDetail.Talent_RejectReason = varTalent_RejectReason;
                            var varUsrUserById = commonInterface.TalentStatus.GetUsrUserById(SessionValues.LoginUserId);

                            try
                            {
                                if (varUsrUserById != null)
                                {
                                    talentDetail.RejectedBy = varUsrUserById.EmployeeId;
                                    talentDetail.ActionUserEmail = varUsrUserById.EmailId;
                                    talentDetail.ActionUserName = varUsrUserById.FullName;
                                    talentDetail.ActionBy = Convert.ToString(StatusChangeAction.Sales);
                                }
                            }
                            catch
                            {

                            }

                            // UTS-7093: Fetch the round details and send to ATS.
                            try
                            {
                                // UTS-7093: Fetch the round details and send to ATS.
                                object[] atsParam = new object[] { contactTalentPriorityResponseModel.HRID, 0, _Talents.Id };
                                string paramString = CommonLogic.ConvertToParamString(atsParam);

                                ATSCall aTSCallforRound = new(configuration, db);

                                sproc_Get_InterviewRoundDetails_Result roundDetails = aTSCallforRound.sproc_Get_InterviewRoundDetails(paramString);
                                if (roundDetails != null)
                                {
                                    string? talentStatusRoundDetails = roundDetails.StrInterviewRound;
                                    if (!string.IsNullOrEmpty(talentStatusRoundDetails))
                                    {
                                        talentDetail.TalentStatusRoundDetails = talentStatusRoundDetails;
                                    }
                                }
                            }
                            catch
                            {

                            }


                            contactTalentPriorityResponseModel.TalentDetails.Add(talentDetail);
                            #endregion
                            try
                            {
                                var json = JsonConvert.SerializeObject(contactTalentPriorityResponseModel);
                                aTSCall.SaveContactTalentPriority(json, CreatedByID, talentReplacement.HiringRequestID.Value);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }

                    Sproc_talent_engagement_Details_For_PHP_API_Result ObjResult = await _iEngagement.TalentEngagementDetails(talentReplacement.ReplacementID, "Replacement");
                    if (ObjResult != null)
                    {
                        TalentEngagementDetailsViewModel engagementDetails = new()
                        {
                            HiringRequest_ID = ObjResult.HiringRequest_ID,
                            ATSTalentId = ObjResult.ATS_Talent_ID,
                            engagement_id = ObjResult.EngagemenID,
                            engagement_start_date = ObjResult.ContractStartDate,
                            engagement_end_date = ObjResult.ContractEndDate,
                            engagement_status = ObjResult.EngagementStatus,
                            talent_status = ObjResult.Talent_status,
                            joining_date = ObjResult.joining_date,
                            lost_date = ObjResult.Lost_date,
                            last_working_date = ObjResult.Last_working_date,
                            talent_statustag = ObjResult.talent_statustag,
                            Action = ObjResult.Action,
                            Action_date = ObjResult.Action_date
                        };

                        var json = JsonConvert.SerializeObject(engagementDetails);
                        aTSCall = new ATSCall(configuration, db);
                        aTSCall.SendTalentEngagementDetails(json, CreatedByID, talentReplacement.HiringRequestID);
                    }
                }
            }

        }

        private GenOnBoardTalentsReplacementDetail? GetReplacementDetails(long? onBoardId)
        {
            var replacementDetails = db.GenOnBoardTalentsReplacementDetails.Where(x => x.OnboardId == onBoardId).FirstOrDefault();
            return replacementDetails;
        }

        //UTS-7389: Fetch the Engagement Values to be replaced.
        private async Task<List<MastersResponseModel>> ListofHRAndEngs(long? onBoardId)
        {
            List<MastersResponseModel> data = new List<MastersResponseModel>();
            List<Sproc_Get_HRIDEngagementID_ForReplacement_Result> Sproc_Get_HRIDEngagementID_ForReplacement = await _iTalentReplacement.Sproc_Get_HRIDEngagementID_ForReplacement(onBoardId.ToString());
            if (Sproc_Get_HRIDEngagementID_ForReplacement != null)
            {
                data = Sproc_Get_HRIDEngagementID_ForReplacement.ToList()
                    .Select(x => new MastersResponseModel
                    {
                        StringIdValue = x.IDvalue,
                        Value = x.Dropdowntext ?? "",
                        Id = x.ID ?? 0,
                        seletected = Convert.ToBoolean(x.IsHR) // If the value is of HR then true
                    }).ToList();
            }

            return data;
        }

        #endregion

        #region Client OnBoarding First & Second Tab API ----- backup

        //
        //[HttpGet]
        //[Route("GetPreOnBoardingDetailForAMAssignment")]
        //public async Task<ObjectResult> GetPreOnBoardingDetailForAMAssignment(long? OnBoardId, long? HR_ID, string actionName)
        //{
        //    try
        //    {
        //        string[] actionList = { "AMAssignment", "GotoOnboard" };

        //        AMAssignmentPreOnBoardingDetailsViewModel AMAssignmentViewModel = new();
        //        GenSalesHiringRequest? _SalesHiringRequest = new GenSalesHiringRequest();

        //        #region PreValidation

        //        if (string.IsNullOrEmpty(actionName) || !actionList.Contains(actionName))
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide proper action name." });
        //        }

        //        if (HR_ID == 0 || HR_ID == null)
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
        //        else
        //        {
        //            _SalesHiringRequest = db.GenSalesHiringRequests.Where(x => x.Id == HR_ID).FirstOrDefault();
        //            if (_SalesHiringRequest == null)
        //            {
        //                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
        //            }

        //            // Added by Riya
        //            AMAssignmentViewModel.IsTransparentPricing = _SalesHiringRequest.IsTransparentPricing;
        //        }

        //        if (OnBoardId == 0 || OnBoardId == null)
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide OnBoard ID." });
        //        }

        //        #endregion

        //        #region Fetch Pre-Onboarding details

        //        object[] param = new object[] { OnBoardId, HR_ID };
        //        string paramasString = CommonLogic.ConvertToParamString(param);

        //        sproc_Get_PreOnboarding_Details_For_AMAssignment_Result AMAssignmentPreOnBoardingDetails = _iOnboard.sproc_Get_PreOnboarding_Details_For_AMAssignment(paramasString);
        //        if (AMAssignmentPreOnBoardingDetails != null)
        //        {
        //            AMAssignmentViewModel.PreOnboardingDetailsForAMAssignment = AMAssignmentPreOnBoardingDetails;
        //        }

        //        #endregion

        //        #region Fetch CurrentHR's

        //        //Fetch the open/inprocess HR's.
        //        object[] paramHR = new object[] { Convert.ToInt64(_SalesHiringRequest.ContactId), HR_ID };
        //        string paramasStringHR = CommonLogic.ConvertToParamString(paramHR);
        //        List<sproc_UTS_GetAllOpenInprocessHR_BasedOnContact_Result> currentHRs = _iOnboard.sproc_UTS_GetAllOpenInprocessHR_BasedOnContact(paramasStringHR);
        //        AMAssignmentViewModel.CurrentHRs = currentHRs;

        //        #endregion

        //        #region FetchDropdown values
        //        //Fetch the Netpaymentdays dropdown values.
        //        var drpNetPaymentDays = new List<SelectListItem>();

        //        drpNetPaymentDays.Insert(0, new SelectListItem { Value = "7", Text = "7" });
        //        drpNetPaymentDays.Insert(1, new SelectListItem { Value = "15", Text = "15" });
        //        drpNetPaymentDays.Insert(2, new SelectListItem { Value = "30", Text = "30" });
        //        drpNetPaymentDays.Insert(3, new SelectListItem { Value = "45", Text = "45" });
        //        drpNetPaymentDays.Insert(4, new SelectListItem { Value = "60", Text = "60" });

        //        AMAssignmentViewModel.DRPNetPaymentDays = drpNetPaymentDays;

        //        //Add the Lead types dropdown values.
        //        List<SelectListItem> bindLeadTypes = new List<SelectListItem>();

        //        SelectListItem bindLeadType1 = new SelectListItem();
        //        bindLeadType1.Text = "1";
        //        bindLeadType1.Value = "InBound";
        //        bindLeadTypes.Add(bindLeadType1);

        //        SelectListItem bindLeadType2 = new SelectListItem();
        //        bindLeadType2.Text = "2";
        //        bindLeadType2.Value = "OutBound";
        //        bindLeadTypes.Add(bindLeadType2);

        //        SelectListItem bindLeadType3 = new SelectListItem();
        //        bindLeadType3.Text = "3";
        //        bindLeadType3.Value = "Partnership";
        //        bindLeadTypes.Add(bindLeadType3);

        //        AMAssignmentViewModel.DRPLeadTypes = bindLeadTypes;

        //        var leadType = Convert.ToString(AMAssignmentViewModel.PreOnboardingDetailsForAMAssignment?.InBoundType);

        //        if (string.IsNullOrEmpty(leadType))
        //        {
        //            leadType = "InBound";
        //        }
        //        else if (leadType.ToLower().Contains("InBound".ToLower()))
        //        {
        //            leadType = "InBound";
        //        }
        //        else if (leadType.ToLower().Contains("OutBound".ToLower()))
        //        {
        //            leadType = "OutBound";
        //        }
        //        else if (leadType.ToLower().Contains("Partnership".ToLower()))
        //        {
        //            leadType = "Partnership";
        //        }
        //        else
        //        {
        //            leadType = "InBound";
        //        }

        //        object[] param2 = new object[] { leadType, HR_ID };
        //        string paramstring = CommonLogic.ConvertToParamString(param2);

        //        //Fetch the default LeadUsers.
        //        var inboundLeadTypeList = _iUsers.Sproc_GetUserBy_LeadType(paramstring);

        //        var LeadTypeList = inboundLeadTypeList.Select(x => new SelectListItem
        //        {
        //            Value = x.Value.ToString(),
        //            Text = x.Text.ToString()
        //        }).ToList();

        //        LeadTypeList.Insert(0, new SelectListItem() { Value = "0", Text = "--Select--" });

        //        AMAssignmentViewModel.DRPLeadUsers = LeadTypeList;

        //        //Fetch the start and end times.
        //        var drpStartTime = new List<SelectListItem>();

        //        DateTime start = new DateTime(2019, 12, 17, 0, 0, 0);
        //        DateTime end = new DateTime(2019, 12, 17, 23, 59, 59);
        //        int j = 0;
        //        List<SelectListItem> list = new List<SelectListItem>();
        //        //while (start.AddMinutes(30) <= end)
        //        while (start <= end)
        //        {
        //            list.Add(new SelectListItem() { Text = start.ToString("t"), Value = start.ToString("t") });
        //            start = start.AddMinutes(30);
        //            j += 1;
        //        }
        //        AMAssignmentViewModel.DRPStartTime = list;

        //        var drpEndTime = new List<SelectListItem>();

        //        j = 0;
        //        //while (start.AddMinutes(30) <= end)
        //        while (start <= end)
        //        {
        //            list.Add(new SelectListItem() { Text = start.ToString("t"), Value = start.ToString("t") });
        //            start = start.AddMinutes(30);
        //            j += 1;
        //        }
        //        AMAssignmentViewModel.DRPEndTime = list;

        //        //Fetch HRAcceptedByUserList
        //        var HRAcceptedByUserList = db.UsrUsers.Where(x => x.UserTypeId == 5 && x.UserTypeId == 10 && x.IsActive == true).Select(x => new SelectListItem
        //        {
        //            Text = x.Id.ToString(),
        //            Value = Convert.ToString(x.FullName)
        //        }).ToList();
        //        AMAssignmentViewModel.DRPHRAcceptedByUserList = HRAcceptedByUserList;
        //        #endregion

        //        #region SetDynamicCTA

        //        DynamicOnBoardCTA dynamicOnBoardCTA = new DynamicOnBoardCTA();
        //        if (actionName == "AMAssignment")
        //        {
        //            dynamicOnBoardCTA.AMAssignment = new CTAInfo(OnBoardCTA.AMAssignment, "Complete AM Assignment", true);
        //            AMAssignmentViewModel.AssignAM = true;
        //        }

        //        if (actionName == "GotoOnboard")
        //        {
        //            dynamicOnBoardCTA.GotoOnboard = new CTAInfo(OnBoardCTA.GotoOnBoard, "Next", true);
        //            AMAssignmentViewModel.AssignAM = false;
        //        }

        //        AMAssignmentViewModel.dynamicOnBoardCTA = dynamicOnBoardCTA;

        //        #endregion

        //        #region SetTabEnableDisable

        //        GenOnBoardTalent? genOnBoardTalent = db.GenOnBoardTalents.FirstOrDefault(x => x.Id == OnBoardId);
        //        if (genOnBoardTalent != null)
        //        {
        //            AMAssignmentViewModel.IsFirstTabReadOnly = genOnBoardTalent.ClientOnBoardingStatusId != 1;
        //        }

        //        #endregion

        //        #region GetReplacementDetails

        //        // UTS-7389: If replacement is Yes then save replacement details
        //        try
        //        {
        //            var replacementDetails = GetReplacementDetails(OnBoardId);
        //            if (replacementDetails != null)
        //            {
        //                AMAssignmentViewModel.ReplacementDetail = replacementDetails;
        //            }

        //            var replacementEngHRs = await ListofHRAndEngs(OnBoardId);
        //            if (replacementEngHRs != null)
        //            {
        //                AMAssignmentViewModel.ReplacementEngAndHR = replacementEngHRs;
        //            }

        //        }
        //        catch
        //        {

        //        }
        //        #endregion

        //        if (AMAssignmentViewModel != null)
        //            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = AMAssignmentViewModel });
        //        else
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Not found" });
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //
        //[HttpPost]
        //[Route("UpdatePreOnBoardingDetailForAMAssignment")]
        //public async Task<ObjectResult> UpdatePreOnBoardingDetailForAMAssignment(UpdatePreOnBoardingDetailsForAMAssignment updatePreOnBoardingDetailsForAMAssignment)
        //{
        //    try
        //    {
        //        long CreatedByID = SessionValues.LoginUserId;
        //        AMAssignmentPreOnBoardingDetailsViewModel AMAssignmentViewModel = new();
        //        GenSalesHiringRequest? _SalesHiringRequest = new GenSalesHiringRequest();
        //        string message = string.Empty;
        //        string colorCode = string.Empty;

        //        bool isAmAssigned = false;

        //        #region PreValidation
        //        if (updatePreOnBoardingDetailsForAMAssignment == null)
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
        //        }
        //        if (updatePreOnBoardingDetailsForAMAssignment.HR_ID == 0 || updatePreOnBoardingDetailsForAMAssignment.HR_ID == null)
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
        //        else
        //        {
        //            _SalesHiringRequest = db.GenSalesHiringRequests.Where(x => x.Id == updatePreOnBoardingDetailsForAMAssignment.HR_ID).FirstOrDefault();
        //            if (_SalesHiringRequest == null)
        //            {
        //                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
        //            }
        //        }
        //        if (updatePreOnBoardingDetailsForAMAssignment.CompanyID == 0 || updatePreOnBoardingDetailsForAMAssignment.CompanyID == null)
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Company ID." });
        //        }

        //        #endregion

        //        #region AM Assigment

        //        if (Convert.ToBoolean(updatePreOnBoardingDetailsForAMAssignment.AssignAM))
        //        {
        //            AMAssignmentController aMAssignmentController = new AMAssignmentController(commonInterface, configuration, _universalProcRunner, db, _iAMAssignment);

        //            long Onboard_ID = Convert.ToInt64(updatePreOnBoardingDetailsForAMAssignment.Onboard_ID);
        //            string EngagemenID = Convert.ToString(updatePreOnBoardingDetailsForAMAssignment.EngagemenID);

        //            ObjectResult? isSucess = aMAssignmentController.AMDataSend(Onboard_ID, EngagemenID);
        //            if (isSucess != null)
        //            {
        //                ResponseObject? responseObject = (ResponseObject)isSucess.Value;
        //                message = responseObject?.Message;
        //                if (responseObject?.statusCode == StatusCodes.Status200OK)
        //                {
        //                    isAmAssigned = true;
        //                    colorCode = "#D3E8D9";
        //                }
        //                else
        //                {
        //                    isAmAssigned = false;
        //                    colorCode = "#FFF4F4";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            isAmAssigned = true;
        //        }

        //        #endregion

        //        if (isAmAssigned)
        //        {
        //            #region Update Company/Talent Info

        //            object[] param = new object[] {
        //            updatePreOnBoardingDetailsForAMAssignment.HR_ID,
        //            updatePreOnBoardingDetailsForAMAssignment.CompanyID,
        //            CreatedByID, updatePreOnBoardingDetailsForAMAssignment.Lead_Type,
        //            updatePreOnBoardingDetailsForAMAssignment.Deal_Source,
        //            updatePreOnBoardingDetailsForAMAssignment.Deal_Owner,
        //            updatePreOnBoardingDetailsForAMAssignment.Industry_Type,
        //            updatePreOnBoardingDetailsForAMAssignment.Onboard_ID,
        //            updatePreOnBoardingDetailsForAMAssignment.TalentShiftStartTime,
        //            updatePreOnBoardingDetailsForAMAssignment.TalentShiftEndTime,
        //            updatePreOnBoardingDetailsForAMAssignment.NetPaymentDays,
        //            updatePreOnBoardingDetailsForAMAssignment.PayRate ?? 0,
        //            updatePreOnBoardingDetailsForAMAssignment.BillRate?? 0,
        //            updatePreOnBoardingDetailsForAMAssignment.TalentID,
        //            updatePreOnBoardingDetailsForAMAssignment.NRMargin ?? 0,
        //            };
        //            string paramasString = CommonLogic.ConvertToParamString(param);

        //            _iOnboard.sproc_Update_PreOnBoardingDetails_for_AMAssignment(paramasString);

        //            #region Insert HR History

        //            string action = Action_Of_History.OnboardingClient_In_Process.ToString();
        //            if (!string.IsNullOrEmpty(action))
        //            {
        //                object[] args = new object[] { action, updatePreOnBoardingDetailsForAMAssignment.HR_ID, updatePreOnBoardingDetailsForAMAssignment.TalentID, false, CreatedByID, 0, 0, "", updatePreOnBoardingDetailsForAMAssignment.Onboard_ID, false, false, 0, 0, (short)AppActionDoneBy.UTS };
        //                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
        //            }
        //            #endregion
        //            #endregion
        //        }

        //        #region SaveReplacementDetails

        //        // UTS-7389: If replacement is Yes then save replacement details
        //        try
        //        {
        //            bool isReplacement = Convert.ToBoolean(updatePreOnBoardingDetailsForAMAssignment.IsReplacement);
        //            await SaveUpdateReplacementDetails(updatePreOnBoardingDetailsForAMAssignment.talentReplacement, isReplacement);
        //        }
        //        catch
        //        {

        //        }

        //        #endregion

        //        dynamic result = new ExpandoObject();
        //        result.Message = message;
        //        result.ColorCode = colorCode;
        //        result.IsAMAssigned = isAmAssigned;

        //        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //
        //[HttpGet]
        //[Route("GetOnBoardingDetailForSecondTabAMAssignment")]
        //public async Task<ObjectResult> GetOnBoardingDetailForSecondTabAMAssignment(long? OnBoardId, long? HR_ID)
        //{
        //    try
        //    {
        //        AMAssignmentPreOnBoardingDetailsViewModel AMAssignmentViewModel = new();
        //        GenSalesHiringRequest? _SalesHiringRequest = new GenSalesHiringRequest();

        //        #region PreValidation

        //        if (HR_ID == 0 || HR_ID == null)
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
        //        else
        //        {
        //            _SalesHiringRequest = db.GenSalesHiringRequests.Where(x => x.Id == HR_ID).FirstOrDefault();
        //            if (_SalesHiringRequest == null)
        //            {
        //                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
        //            }
        //        }

        //        if (OnBoardId == 0 || OnBoardId == null)
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide OnBoard ID." });
        //        }

        //        #endregion

        //        object[] param = new object[] { OnBoardId, HR_ID };
        //        string paramasString = CommonLogic.ConvertToParamString(param);

        //        sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment_Result SecondTabAMAssignmentOnBoardingDetails = _iOnboard.sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment(paramasString);
        //        if (SecondTabAMAssignmentOnBoardingDetails != null)
        //        {
        //            AMAssignmentViewModel.SecondTabAMAssignmentOnBoardingDetails = SecondTabAMAssignmentOnBoardingDetails;
        //        }

        //        #region DeviceMaster

        //        List<PrgOnBoardPolicyDeviceMaster> deviceMaster = db.PrgOnBoardPolicyDeviceMasters.ToList();
        //        if (deviceMaster != null)
        //        {
        //            AMAssignmentViewModel.deviceMaster = deviceMaster;
        //        }

        //        #endregion

        //        #region Leave Policy Master
        //        List<SelectListItem> bindLeavePolicyDrp = new List<SelectListItem>();

        //        SelectListItem LeavePolicy1 = new SelectListItem();
        //        LeavePolicy1.Text = "1";
        //        LeavePolicy1.Value = "Proceed with Uplers Policies";
        //        bindLeavePolicyDrp.Add(LeavePolicy1);

        //        SelectListItem LeavePolicy2 = new SelectListItem();
        //        LeavePolicy2.Text = "1";
        //        LeavePolicy2.Value = "Upload Your Policies";
        //        bindLeavePolicyDrp.Add(LeavePolicy2);

        //        AMAssignmentViewModel.bindLeavePolicyDrp = bindLeavePolicyDrp;
        //        #endregion

        //        #region Add Exit Policy and FeedBack Process and Uplers Leave Policy
        //        AMAssignmentViewModel.UplersLeavePolicy = "https://www.uplers.com/talent/leave-policy/";
        //        AMAssignmentViewModel.Exit_Policy = "First Month - 7 Days <br/>  Second Month Onwards - 30 Days";
        //        AMAssignmentViewModel.Feedback_Process = "Weekly during the first 2 weeks | Fortnightly for the next 2 months | Monthly / Quarterly feedback thereafter";

        //        #endregion

        //        #region Get Team Member Details

        //        List<GenOnBoardClientTeam> onBoardClientTeam = db.GenOnBoardClientTeams.Where(x => x.OnBoardId == OnBoardId).ToList();
        //        if (onBoardClientTeam != null)
        //        {
        //            AMAssignmentViewModel.onBoardClientTeam = onBoardClientTeam;
        //        }

        //        #endregion

        //        #region SetTabEnableDisable

        //        GenOnBoardTalent? genOnBoardTalent = db.GenOnBoardTalents.FirstOrDefault(x => x.Id == OnBoardId);
        //        if (genOnBoardTalent != null)
        //        {
        //            AMAssignmentViewModel.IsSecondTabReadOnly = genOnBoardTalent.ClientOnBoardingStatusId == 2;
        //        }

        //        #endregion

        //        #region GetReplacementDetails

        //        // UTS-7389: If replacement is Yes then save replacement details
        //        try
        //        {
        //            var replacementDetails = GetReplacementDetails(OnBoardId);
        //            if (replacementDetails != null)
        //            {
        //                AMAssignmentViewModel.ReplacementDetail = replacementDetails;
        //            }

        //            var replacementEngHRs = await ListofHRAndEngs(OnBoardId);
        //            if (replacementEngHRs != null)
        //            {
        //                AMAssignmentViewModel.ReplacementEngAndHR = replacementEngHRs;
        //            }

        //        }
        //        catch
        //        {

        //        }
        //        #endregion

        //        if (AMAssignmentViewModel != null)
        //            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = AMAssignmentViewModel });
        //        else
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Not found" });
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //
        //[HttpPost]
        //[Route("UpdateOnBoardingDetailForSecondTabAMAssignment")]
        //public async Task<ObjectResult> UpdateOnBoardingDetailForSecondTabAMAssignment(UpdatePreOnBoardingDetailsForSecondTabAMAssignment updateClientOnBoardingDetails)
        //{
        //    try
        //    {
        //        long? CreatedByID = SessionValues.LoginUserId;
        //        AMAssignmentPreOnBoardingDetailsViewModel AMAssignmentViewModel = new();
        //        GenSalesHiringRequest? _SalesHiringRequest = new GenSalesHiringRequest();

        //        #region PreValidation

        //        if (updateClientOnBoardingDetails == null)
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
        //        }
        //        if (updateClientOnBoardingDetails.HR_ID == 0 || updateClientOnBoardingDetails.HR_ID == null)
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
        //        else
        //        {
        //            _SalesHiringRequest = db.GenSalesHiringRequests.Where(x => x.Id == updateClientOnBoardingDetails.HR_ID).FirstOrDefault();
        //            if (_SalesHiringRequest == null)
        //            {
        //                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
        //            }
        //        }
        //        if (updateClientOnBoardingDetails.CompanyID == 0 || updateClientOnBoardingDetails.CompanyID == null)
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Company ID." });
        //        }

        //        #endregion

        //        object[] param = new object[]
        //        {
        //            updateClientOnBoardingDetails.HR_ID,
        //            updateClientOnBoardingDetails.CompanyID,
        //            updateClientOnBoardingDetails.SigningAuthorityName,
        //            updateClientOnBoardingDetails.SigningAuthorityEmail,
        //            updateClientOnBoardingDetails.ContractDuration,
        //            updateClientOnBoardingDetails.OnBoardID,
        //            updateClientOnBoardingDetails.about_Company_desc,
        //            updateClientOnBoardingDetails.Talent_FirstWeek,
        //            updateClientOnBoardingDetails.Talent_FirstMonth,
        //            updateClientOnBoardingDetails.SoftwareToolsRequired,
        //            CreatedByID
        //        };
        //        string paramasString = CommonLogic.ConvertToParamString(param);

        //        _iOnboard.sproc_Update_Onboarding_Details_For_Second_Tab_AMAssignment(paramasString);

        //        #region Save & Update Device Policy and Leave Policy

        //        #region Device Policy

        //        GenOnBoardClientContractDetail onBoardClientContractDetails = new GenOnBoardClientContractDetail();
        //        onBoardClientContractDetails = db.GenOnBoardClientContractDetails.Where(x => x.OnBoardId == updateClientOnBoardingDetails.OnBoardID).FirstOrDefault();
        //        onBoardClientContractDetails.DevicesPoliciesOption = updateClientOnBoardingDetails.hdnRadioDevicesPolicies;

        //        if (updateClientOnBoardingDetails.hdnRadioDevicesPolicies == "Client to buy a device and Uplers to Facilitate")
        //        {
        //            updateClientOnBoardingDetails.TalentDeviceDetails = string.Empty;
        //            var InsertedDevices = db.GenOnBoardClientDevicePolicyDetails.Where(p => p.OnBoardId == updateClientOnBoardingDetails.OnBoardID).ToList();
        //            db.GenOnBoardClientDevicePolicyDetails.RemoveRange(InsertedDevices);

        //            updateClientOnBoardingDetails.AdditionalCostPerMonth_RDPSecurity = 0;
        //            updateClientOnBoardingDetails.IsRecurring = false;
        //            //OtherDeviceDescription
        //            DeviceOptionVM optionvmdata = new DeviceOptionVM();

        //            GenOnBoardClientDevicePolicyDetail devicepolicydetails = new GenOnBoardClientDevicePolicyDetail();
        //            if (updateClientOnBoardingDetails.Device_Radio_Option == "Windows Laptop")
        //            {
        //                devicepolicydetails.OnBoardId = updateClientOnBoardingDetails.OnBoardID;
        //                devicepolicydetails.DeviceId = updateClientOnBoardingDetails.DeviceID;
        //                devicepolicydetails.ClientDeviceDescription = updateClientOnBoardingDetails.Client_DeviceDescription;
        //                devicepolicydetails.Qty = 1;
        //                devicepolicydetails.TotalCost = updateClientOnBoardingDetails.TotalCost;
        //                db.GenOnBoardClientDevicePolicyDetails.Add(devicepolicydetails);
        //                db.SaveChanges();
        //            }

        //            else if (updateClientOnBoardingDetails.Device_Radio_Option == "Apple Mac Laptop")
        //            {
        //                devicepolicydetails.OnBoardId = updateClientOnBoardingDetails.OnBoardID ?? 0;
        //                devicepolicydetails.DeviceId = updateClientOnBoardingDetails.DeviceID;
        //                devicepolicydetails.ClientDeviceDescription = updateClientOnBoardingDetails.Client_DeviceDescription;
        //                devicepolicydetails.Qty = 1;
        //                devicepolicydetails.TotalCost = updateClientOnBoardingDetails.TotalCost;
        //                db.GenOnBoardClientDevicePolicyDetails.Add(devicepolicydetails);
        //                db.SaveChanges();
        //            }
        //            else if (updateClientOnBoardingDetails.Device_Radio_Option == "Other")
        //            {
        //                devicepolicydetails.OnBoardId = updateClientOnBoardingDetails.OnBoardID ?? 0;
        //                devicepolicydetails.DeviceId = updateClientOnBoardingDetails.DeviceID;
        //                devicepolicydetails.ClientDeviceDescription = updateClientOnBoardingDetails.Client_DeviceDescription;
        //                devicepolicydetails.Qty = 1;
        //                db.GenOnBoardClientDevicePolicyDetails.Add(devicepolicydetails);
        //                db.SaveChanges();
        //            }

        //        }
        //        else if (updateClientOnBoardingDetails.hdnRadioDevicesPolicies == "Talent to bring his own devices")
        //        {
        //            onBoardClientContractDetails.TalentDeviceDetails = updateClientOnBoardingDetails.TalentDeviceDetails;
        //        }
        //        else if (updateClientOnBoardingDetails.hdnRadioDevicesPolicies == "Client can use remote desktop security option facilitated by Uplers (At additional cost of $100 per month)")
        //        {
        //            onBoardClientContractDetails.TalentDeviceDetails = string.Empty;
        //            var InsertedDevices = db.GenOnBoardClientDevicePolicyDetails.Where(p => p.OnBoardId == updateClientOnBoardingDetails.OnBoardID).ToList();
        //            db.GenOnBoardClientDevicePolicyDetails.RemoveRange(InsertedDevices);

        //            onBoardClientContractDetails.AdditionalCostPerMonthRdpsecurity = 100;
        //            onBoardClientContractDetails.IsRecurring = true;

        //        }
        //        else if (updateClientOnBoardingDetails.hdnRadioDevicesPolicies == "Add This Later")
        //        {
        //            onBoardClientContractDetails.TalentDeviceDetails = string.Empty;
        //            var InsertedDevices = db.GenOnBoardClientDevicePolicyDetails.Where(p => p.OnBoardId == updateClientOnBoardingDetails.OnBoardID).ToList();
        //            db.GenOnBoardClientDevicePolicyDetails.RemoveRange(InsertedDevices);
        //        }

        //        #endregion

        //        #region Leave Policy
        //        onBoardClientContractDetails.ProceedWithUplersLeavePolicyOption = updateClientOnBoardingDetails.Radio_LeavePolicies;
        //        if (updateClientOnBoardingDetails.Radio_LeavePolicies == "Proceed with Uplers Policies")
        //        {
        //            //   onBoardClientContractDetails.ProceedWithClientLeavePolicyOption = "Leave Policy";
        //            if (!string.IsNullOrEmpty(updateClientOnBoardingDetails.LeavePolicyPasteLinkName))
        //            {
        //                //onBoardClientContractDetails.ProceedWithClientLeavePolicyOption = "Paste Link";
        //                onBoardClientContractDetails.ProceedWithClientLeavePolicyLink = updateClientOnBoardingDetails.LeavePolicyPasteLinkName;
        //            }
        //            else { onBoardClientContractDetails.ProceedWithClientLeavePolicyLink = string.Empty; }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(updateClientOnBoardingDetails.LeavePolicyFileName))
        //            {
        //                // onBoardClientContractDetails.ProceedWithClientLeavePolicyOption = "Upload Client Leave Policy";
        //                onBoardClientContractDetails.ProceedWithClientLeavePolicyFileUpload = updateClientOnBoardingDetails.LeavePolicyFileName;
        //            }
        //            else
        //            {
        //                onBoardClientContractDetails.ProceedWithClientLeavePolicyFileUpload = string.Empty;
        //            }
        //        }
        //        #endregion

        //        #region ExitPolicy

        //        onBoardClientContractDetails.ProceedWithUplersExitPolicyOption = updateClientOnBoardingDetails.Exit_Policy;

        //        #endregion

        //        db.Entry(onBoardClientContractDetails).State = EntityState.Modified;
        //        db.SaveChanges();

        //        #endregion

        //        #region Adding TeamMembers 

        //        if (updateClientOnBoardingDetails.TeamMembers != null)
        //        {
        //            if (updateClientOnBoardingDetails.TeamMembers.Count > 0)
        //            {
        //                foreach (var team in updateClientOnBoardingDetails.TeamMembers)
        //                {
        //                    GenOnBoardClientTeam OnBoardClientTeamdata = new GenOnBoardClientTeam();
        //                    OnBoardClientTeamdata.OnBoardId = updateClientOnBoardingDetails.OnBoardID;
        //                    OnBoardClientTeamdata.Name = team.Name;
        //                    OnBoardClientTeamdata.Designation = team.Designation;
        //                    OnBoardClientTeamdata.Email = team.Email;
        //                    OnBoardClientTeamdata.Linkedin = team.Linkedin;
        //                    OnBoardClientTeamdata.ReportingTo = team.ReportingTo;
        //                    if (team.Buddy == "1" || team.Buddy == "Buddy")
        //                    {
        //                        OnBoardClientTeamdata.Buddy = "Buddy";
        //                    }
        //                    else
        //                    {
        //                        OnBoardClientTeamdata.Buddy = "Select Later";
        //                    }
        //                    OnBoardClientTeamdata.IsActive = true;
        //                    db.GenOnBoardClientTeams.Add(OnBoardClientTeamdata);
        //                    db.SaveChanges();
        //                }
        //            }
        //        }

        //        #endregion

        //        #region update Client OnBoarding Status and Insert History

        //        GenOnBoardTalent? onBoardTalents = db.GenOnBoardTalents.FirstOrDefault(x => x.Id == updateClientOnBoardingDetails.OnBoardID);
        //        if (onBoardTalents != null)
        //        {
        //            onBoardTalents.ClientOnBoardingStatusId = 2;
        //            onBoardTalents.LastModifiedDatetime = DateTime.Now;
        //            onBoardTalents.LastModifiedById = (int)CreatedByID;
        //            onBoardTalents.ClientOnBoardingDate = DateTime.Now;
        //            db.Entry(onBoardTalents).State = EntityState.Modified;
        //            db.SaveChanges();

        //            #region Insert HR History
        //            string action = Action_Of_History.OnboardingClient_Done.ToString();
        //            if (!string.IsNullOrEmpty(action))
        //            {
        //                object[] args = new object[] { action, updateClientOnBoardingDetails.HR_ID, onBoardTalents.TalentId, false, CreatedByID, 0, 0, "", updateClientOnBoardingDetails.OnBoardID, false, false, 0, 0, (short)AppActionDoneBy.UTS };
        //                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
        //            }
        //            #endregion
        //        }

        //        #endregion

        //        #region SaveReplacementDetails

        //        // UTS-7389: If replacement is Yes then save replacement details
        //        try
        //        {
        //            bool isReplacement = Convert.ToBoolean(updateClientOnBoardingDetails.IsReplacement);
        //            await SaveUpdateReplacementDetails(updateClientOnBoardingDetails.talentReplacement, isReplacement);
        //        }
        //        catch
        //        {

        //        }

        //        #endregion

        //        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //
        //[HttpPost]
        //[Route("UploadSOWDocument")]
        //public async Task<ObjectResult> UploadSOWDocument([FromForm] IFormFile file, [FromQuery] long OnBoardId, [FromQuery] bool isDelete)
        //{
        //    try
        //    {
        //        #region Validation

        //        if (OnBoardId == 0)
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide OnBoard ID." });
        //        }

        //        if (!isDelete)
        //        {
        //            if (file == null)
        //            {
        //                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File Required", Details = null });
        //            }
        //            else if (file.Length == 0)
        //            {
        //                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "You are uploading corrupt file", Details = null });
        //            }

        //            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
        //            string[] allowedFileExtension = { ".pdf", ".doc", ".docx", ".txt", ".rtf", ".jpg", ".jpeg", ".png" };

        //            if (!allowedFileExtension.Contains(fileExtension))
        //            {
        //                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Your file format is incorrect.", Details = null });
        //            }

        //            var fileSize = (file.Length / 1024) / 1024;
        //            if (fileSize >= 0.5)
        //            {
        //                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File size must be less than 500 KB", Details = null });
        //            }
        //        }

        //        #endregion

        //        string DocfileName = file.FileName;
        //        string folderPath = System.IO.Path.Combine("Media/OnBoarding/SOWDocument", OnBoardId.ToString());
        //        string filePath = System.IO.Path.Combine(folderPath, DocfileName);

        //        if (isDelete)
        //        {
        //            //If file path exists then delete the path.
        //            if (Directory.Exists(folderPath))
        //            {
        //                System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

        //                foreach (FileInfo doc in di.GetFiles())
        //                {
        //                    doc.Delete();
        //                }

        //                Directory.Delete(folderPath);
        //            }

        //            return StatusCode(StatusCodes.Status200OK, new ResponseObject()
        //            {
        //                statusCode = StatusCodes.Status200OK,
        //                Message = "File removed successfully"
        //            });
        //        }

        //        //If file path does not exists then create the path.
        //        if (!Directory.Exists(folderPath))
        //        {
        //            Directory.CreateDirectory(folderPath);
        //        }

        //        if (file.Length > 0)
        //        {
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(fileStream);
        //            }
        //        }

        //        return StatusCode(StatusCodes.Status200OK, new ResponseObject()
        //        {
        //            statusCode = StatusCodes.Status200OK,
        //            Message = "Success",
        //            Details = DocfileName
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //
        //[HttpPost]
        //[Route("UploadLeavePolicy")]
        //public async Task<ObjectResult> UploadLeavePolicy([FromForm] IFormFile file, [FromQuery] long OnBoardId, [FromQuery] bool isDelete)
        //{
        //    try
        //    {
        //        #region Validation

        //        if (OnBoardId == 0)
        //        {
        //            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide OnBoard ID." });
        //        }

        //        if (!isDelete)
        //        {
        //            if (file == null)
        //            {
        //                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File Required", Details = null });
        //            }
        //            else if (file.Length == 0)
        //            {
        //                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "You are uploading corrupt file", Details = null });
        //            }

        //            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
        //            string[] allowedFileExtension = { ".pdf", ".doc", ".docx", ".txt", ".rtf", ".jpg", ".jpeg", ".png" };

        //            if (!allowedFileExtension.Contains(fileExtension))
        //            {
        //                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Your file format is incorrect.", Details = null });
        //            }

        //            var fileSize = (file.Length / 1024) / 1024;
        //            if (fileSize >= 0.5)
        //            {
        //                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File size must be less than 500 KB", Details = null });
        //            }
        //        }

        //        #endregion

        //        string DocfileName = file.FileName;
        //        string folderPath = System.IO.Path.Combine("Media/OnBoarding/LeavePolicy", OnBoardId.ToString());
        //        string filePath = System.IO.Path.Combine(folderPath, DocfileName);

        //        if (isDelete)
        //        {
        //            //If file path exists then delete the path.
        //            if (Directory.Exists(folderPath))
        //            {
        //                System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

        //                foreach (FileInfo doc in di.GetFiles())
        //                {
        //                    doc.Delete();
        //                }

        //                Directory.Delete(folderPath);
        //            }

        //            return StatusCode(StatusCodes.Status200OK, new ResponseObject()
        //            {
        //                statusCode = StatusCodes.Status200OK,
        //                Message = "File removed successfully"
        //            });
        //        }

        //        //If file path does not exists then create the path.
        //        if (!Directory.Exists(folderPath))
        //        {
        //            Directory.CreateDirectory(folderPath);
        //        }

        //        if (file.Length > 0)
        //        {
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(fileStream);
        //            }
        //        }

        //        return StatusCode(StatusCodes.Status200OK, new ResponseObject()
        //        {
        //            statusCode = StatusCodes.Status200OK,
        //            Message = "Success",
        //            Details = DocfileName
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        #endregion

        //
        //[HttpGet("ViewInDetail")]
        //public ObjectResult ViewOnBoardDetail1(long onBoardId)
        //{

        //    #region PreValidation
        //    OnBoardUpdate onBoardUpdateviewModel = new OnBoardUpdate();

        //    if (onBoardId == null || onBoardId == 0)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide propoer onBoardID" });
        //    }
        //    #endregion

        //    GenOnBoardClientContractDetail onBoardClientContractDetails = db.GenOnBoardClientContractDetails.FirstOrDefault(x => x.OnBoardId == onBoardId);
        //    GenOnBoardTalent onBoardTalent = db.GenOnBoardTalents.FirstOrDefault(x => x.Id == onBoardId);
        //    if (onBoardClientContractDetails != null)
        //    {
        //        onBoardUpdateviewModel.drpNetPaymentDays = new List<SelectListItem>();

        //        onBoardUpdateviewModel.drpNetPaymentDays.Insert(0, new SelectListItem { Value = "7", Text = "7" });
        //        onBoardUpdateviewModel.drpNetPaymentDays.Insert(1, new SelectListItem { Value = "15", Text = "15" });
        //        onBoardUpdateviewModel.drpNetPaymentDays.Insert(2, new SelectListItem { Value = "30", Text = "30" });
        //        onBoardUpdateviewModel.drpNetPaymentDays.Insert(3, new SelectListItem { Value = "45", Text = "45" });
        //        onBoardUpdateviewModel.drpNetPaymentDays.Insert(4, new SelectListItem { Value = "60", Text = "60" });

        //        onBoardUpdateviewModel.drpContractRenewal = new List<SelectListItem>();
        //        onBoardUpdateviewModel.devicepolicies = db.GenOnBoardClientDevicePolicyDetails.Where(x => x.OnBoardId == onBoardId).ToList();
        //        onBoardUpdateviewModel.leavepolicies = db.GenOnBoardClientLeavePolicies.Where(x => x.OnBoardId == onBoardId).ToList();
        //        onBoardUpdateviewModel.drpContractRenewal.Insert(0, new SelectListItem { Value = "0", Text = "0" });
        //        onBoardUpdateviewModel.drpContractRenewal.Insert(1, new SelectListItem { Value = "1", Text = "1" });
        //        onBoardUpdateviewModel.drpContractRenewal.Insert(2, new SelectListItem { Value = "2", Text = "2" });
        //        onBoardUpdateviewModel.drpContractRenewal.Insert(3, new SelectListItem { Value = "3", Text = "3" });
        //        onBoardUpdateviewModel.drpContractRenewal.Insert(4, new SelectListItem { Value = "4", Text = "4" });
        //        onBoardUpdateviewModel.drpContractRenewal.Insert(5, new SelectListItem { Value = "5", Text = "5" });
        //        onBoardUpdateviewModel.ContractDutation = onBoardClientContractDetails.ContractDuration;
        //        onBoardUpdateviewModel.ContractType = onBoardClientContractDetails.ContractType;
        //        onBoardUpdateviewModel.WorkingTimeZone = onBoardClientContractDetails.TalentWorkingTimeZone;
        //        onBoardUpdateviewModel.contractStartDate = onBoardClientContractDetails.ContractStartDate.HasValue ? onBoardClientContractDetails.ContractStartDate.Value.ToString("yyyy-MM-dd") : "";
        //        onBoardUpdateviewModel.contractEndDate = onBoardClientContractDetails.ContractEndDate.HasValue ? onBoardClientContractDetails.ContractEndDate.Value.ToString("yyyy-MM-dd") : "";
        //        onBoardUpdateviewModel.listOnBoardClientTeam = db.GenOnBoardClientTeams.Where(x => x.OnBoardId == onBoardId).ToList();

        //        if (onBoardTalent != null)
        //        {
        //            GenSalesHiringRequest genSalesHiringRequest = db.GenSalesHiringRequests.FirstOrDefault(x => x.Id == onBoardTalent.HiringRequestId);
        //            if (genSalesHiringRequest != null)
        //            {
        //                onBoardUpdateviewModel.HRID = genSalesHiringRequest.HrNumber;
        //                GenContact contact = db.GenContacts.FirstOrDefault(x => x.Id == genSalesHiringRequest.ContactId);
        //                if (contact != null)
        //                {
        //                    GenCompany company = db.GenCompanies.FirstOrDefault(x => x.Id == contact.CompanyId);
        //                    if (company != null)
        //                    {
        //                        onBoardUpdateviewModel.CompnayName = company.Company;
        //                    }
        //                }
        //            }
        //            onBoardUpdateviewModel.BillRate = onBoardTalent.FinalHrCost;
        //            onBoardUpdateviewModel.PayRate = onBoardTalent.TalentCost;
        //            Added the dynamic currency code for the onboarding.
        //            string ? talentCurrencyCode = string.IsNullOrEmpty(onBoardTalent.TalentCurrencyCode) ? "USD" : onBoardTalent.TalentCurrencyCode;
        //            onBoardUpdateviewModel.TalentCurrencyCode = $"{talentCurrencyCode} / Month";
        //            onBoardUpdateviewModel.TalentPayRate = onBoardClientContractDetails.TalentMonthlyCost;
        //            UsrUser usrUser = db.UsrUsers.FirstOrDefault(x => x.Id == onBoardTalent.AmSalesPersonId);
        //            if (usrUser != null)
        //            {
        //                onBoardUpdateviewModel.AMUser = usrUser.FullName;
        //            }
        //            UsrUser usrUserNbd = db.UsrUsers.FirstOrDefault(x => x.Id == onBoardTalent.NbdSalesPersonId);
        //            if (usrUserNbd != null)
        //            {
        //                onBoardUpdateviewModel.BDUser = usrUserNbd.FullName;
        //            }
        //        }
        //        onBoardUpdateviewModel.StartTimeAMPM = onBoardClientContractDetails.PunchStartTime;
        //        onBoardUpdateviewModel.EndTimeAMPM = onBoardClientContractDetails.PunchEndTime;
        //        PrgTimeZonePreference prgTimeZonePreference = db.PrgTimeZonePreferences.FirstOrDefault(x => x.Id == onBoardClientContractDetails.TimezonePreferenceId);
        //        if (prgTimeZonePreference != null)
        //            onBoardUpdateviewModel.TimeZone = prgTimeZonePreference.WorkingTimePreference;

        //        onBoardUpdateviewModel.drpTalentWorkingTime = db.PrgTalentTimeZones.Where(y => y.IsActive == true).Select(y => new SelectListItem
        //        {
        //            Text = y.TalentTimeZone,
        //            Value = y.Id.ToString()

        //        }).ToList();


        //        onBoardUpdateviewModel.drpTalentPrefWorkingTime = db.PrgTimeZonePreferences.ToList().Where(x => x.WorkingTimePreference != "").Select(x => new SelectListItem
        //        {
        //            Value = x.Id.ToString(),
        //            Text = x.WorkingTimePreference
        //        }).ToList();

        //        if (onBoardClientContractDetails.TimezonePreferenceId.HasValue)
        //            onBoardUpdateviewModel.TalentWorkingPrefTimeZone = Convert.ToInt32(onBoardClientContractDetails.TimezonePreferenceId);
        //        else
        //            onBoardUpdateviewModel.TalentWorkingPrefTimeZone = 0;

        //        onBoardUpdateviewModel.drpStartTime = new List<SelectListItem>();


        //        DateTime start = new DateTime(2019, 12, 17, 0, 0, 0);
        //        DateTime end = new DateTime(2019, 12, 17, 23, 59, 59);
        //        int j = 0;
        //        List<SelectListItem> list = new List<SelectListItem>();
        //        while (start.AddMinutes(30) <= end)
        //            while (start <= end)
        //            {
        //                list.Add(new SelectListItem() { Text = start.ToString("t"), Value = start.ToString("t") });
        //                start = start.AddMinutes(30);
        //                j += 1;
        //            }
        //        onBoardUpdateviewModel.drpStartTime = list;

        //        onBoardUpdateviewModel.drpEndTime = new List<SelectListItem>();

        //        j = 0;
        //        while (start.AddMinutes(30) <= end)
        //            while (start <= end)
        //            {
        //                list.Add(new SelectListItem() { Text = start.ToString("t"), Value = start.ToString("t") });
        //                start = start.AddMinutes(30);
        //                j += 1;
        //            }
        //        onBoardUpdateviewModel.drpEndTime = list;


        //        onBoardUpdateviewModel.drpStartDay = new List<SelectListItem>();

        //        onBoardUpdateviewModel.drpStartDay.Insert(0, new SelectListItem { Value = "0", Text = "Start Day" });
        //        onBoardUpdateviewModel.drpStartDay.Insert(1, new SelectListItem { Value = "Sunday", Text = "Sunday" });
        //        onBoardUpdateviewModel.drpStartDay.Insert(2, new SelectListItem { Value = "Monday", Text = "Monday" });
        //        onBoardUpdateviewModel.drpStartDay.Insert(3, new SelectListItem { Value = "Tuesday", Text = "Tuesday" });
        //        onBoardUpdateviewModel.drpStartDay.Insert(4, new SelectListItem { Value = "Wednesday", Text = "Wednesday" });
        //        onBoardUpdateviewModel.drpStartDay.Insert(5, new SelectListItem { Value = "Thursday", Text = "Thursday" });
        //        onBoardUpdateviewModel.drpStartDay.Insert(6, new SelectListItem { Value = "Friday", Text = "Friday" });
        //        onBoardUpdateviewModel.drpStartDay.Insert(7, new SelectListItem { Value = "Saturday", Text = "Saturday" });

        //        onBoardUpdateviewModel.drpEndDay = new List<SelectListItem>();

        //        onBoardUpdateviewModel.drpEndDay.Insert(0, new SelectListItem { Value = "0", Text = "End Day" });
        //        onBoardUpdateviewModel.drpEndDay.Insert(1, new SelectListItem { Value = "Sunday", Text = "Sunday" });
        //        onBoardUpdateviewModel.drpEndDay.Insert(2, new SelectListItem { Value = "Monday", Text = "Monday" });
        //        onBoardUpdateviewModel.drpEndDay.Insert(3, new SelectListItem { Value = "Tuesday", Text = "Tuesday" });
        //        onBoardUpdateviewModel.drpEndDay.Insert(4, new SelectListItem { Value = "Wednesday", Text = "Wednesday" });
        //        onBoardUpdateviewModel.drpEndDay.Insert(5, new SelectListItem { Value = "Thursday", Text = "Thursday" });
        //        onBoardUpdateviewModel.drpEndDay.Insert(6, new SelectListItem { Value = "Friday", Text = "Friday" });
        //        onBoardUpdateviewModel.drpEndDay.Insert(7, new SelectListItem { Value = "Saturday", Text = "Saturday" });

        //        onBoardUpdateviewModel.drpStartTimeAMPM = new List<SelectListItem>();

        //        onBoardUpdateviewModel.drpStartTimeAMPM.Insert(0, new SelectListItem { Value = "0", Text = "Select" });
        //        onBoardUpdateviewModel.drpStartTimeAMPM.Insert(1, new SelectListItem { Value = "AM", Text = "AM" });
        //        onBoardUpdateviewModel.drpStartTimeAMPM.Insert(2, new SelectListItem { Value = "PM", Text = "PM" });

        //        onBoardUpdateviewModel.drpEndTimeAMPM = new List<SelectListItem>();

        //        onBoardUpdateviewModel.drpEndTimeAMPM.Insert(0, new SelectListItem { Value = "0", Text = "Select" });
        //        onBoardUpdateviewModel.drpEndTimeAMPM.Insert(1, new SelectListItem { Value = "AM", Text = "AM" });
        //        onBoardUpdateviewModel.drpEndTimeAMPM.Insert(2, new SelectListItem { Value = "PM", Text = "PM" });

        //        object[] param = new object[] { onBoardId };

        //        Sproc_Get_OnBoardContract_Details_Result onBoardContract_Details_Result = _iOnboard.Sproc_Get_OnBoardContract_Details(CommonLogic.ConvertToParamString(param));

        //        if (onBoardContract_Details_Result != null)
        //        {
        //            onBoardUpdateviewModel.OnboardID = onBoardId;
        //            onBoardUpdateviewModel.onboardDetails = onBoardContract_Details_Result;
        //            onBoardUpdateviewModel.TalentWorkingTimeZone = Convert.ToInt32(onBoardContract_Details_Result.TimeZone);

        //            string punchtimestr = onBoardContract_Details_Result.PunchTime;
        //            string[] punchTime = punchtimestr.Split(new string[] { " to " }, StringSplitOptions.None);

        //            string TimeZonestr = onBoardContract_Details_Result.WorkingDay;
        //            string[] TimeZone = TimeZonestr.Split(new string[] { " To " }, StringSplitOptions.None);

        //            for (int i = 0; i < punchTime.Count(); i++)
        //            {
        //                if (i == 0)
        //                {
        //                    onBoardUpdateviewModel.StartTime = punchTime[i];
        //                }
        //                if (i == 1)
        //                {
        //                    if (punchTime[i].Contains(" ("))
        //                    {
        //                        string str = punchTime[i];
        //                        string value = str.Substring(0, str.IndexOf(" ("));
        //                        onBoardUpdateviewModel.EndTime = value;
        //                    }
        //                    else
        //                    {
        //                        onBoardUpdateviewModel.EndTime = punchTime[i];
        //                    }
        //                }
        //            }

        //            for (int i = 0; i < TimeZone.Count(); i++)
        //            {
        //                if (i == 0)
        //                {
        //                    onBoardUpdateviewModel.StartDay = TimeZone[i];
        //                }
        //                if (i == 1)
        //                {
        //                    onBoardUpdateviewModel.EndDay = TimeZone[i];
        //                }
        //            }


        //            onBoardUpdateviewModel.NetPaymentDays = onBoardContract_Details_Result.NetPaymentDays;
        //            onBoardUpdateviewModel.AutoRenewContract = onBoardContract_Details_Result.ContractRenewalSlot;
        //            onBoardUpdateviewModel.TalentOnBoardingDate = onBoardContract_Details_Result.TalentOnBoardDate;
        //            onBoardUpdateviewModel.TalentOnBoardingTime = onBoardContract_Details_Result.TalentOnBoardTime;

        //            convert the date to specific format.
        //            if (!string.IsNullOrEmpty(onBoardContract_Details_Result.ClientFirstDate))
        //            {
        //                onBoardUpdateviewModel.onboardDetails.ClientFirstDate = CommonLogic.ConvertString2DateTime(onBoardContract_Details_Result.ClientFirstDate).ToString("yyyy-MM-dd");
        //            }
        //            if (!string.IsNullOrEmpty(onBoardContract_Details_Result.TalentOnBoardDate))
        //            {
        //                onBoardUpdateviewModel.onboardDetails.TalentOnBoardDate = CommonLogic.ConvertString2DateTime(onBoardContract_Details_Result.TalentOnBoardDate).ToString("yyyy-MM-dd");
        //            }

        //            List<GenOnBoardClientTeam> onBoardClientTeam = db.GenOnBoardClientTeams.Where(x => x.OnBoardId == onBoardId).ToList();
        //            if (onBoardClientTeam != null)
        //            {
        //                List<GenOnBoardClientTeam> OnBoardClientTeams = new List<GenOnBoardClientTeam>();
        //                OnBoardClientTeams = onBoardClientTeam;
        //                Session["OnBoardClientTeams"] = OnBoardClientTeams;
        //            }

        //            onBoardUpdateviewModel.DevicesPoliciesOption = onBoardContract_Details_Result.DevicesPoliciesOption;
        //            onBoardUpdateviewModel.TalentBringOwnRemark = onBoardContract_Details_Result.TalentDeviceDetails;

        //            onBoardUpdateviewModel.ExpectationFirstWeek = onBoardContract_Details_Result.ExpectationFromTalent_FirstWeek;
        //            onBoardUpdateviewModel.ExpectationFirstMonth = onBoardContract_Details_Result.ExpectationFromTalent_FirstMonth;
        //            onBoardUpdateviewModel.ExpectationAdditionalInformation = onBoardContract_Details_Result.Client_Remark;

        //            onBoardUpdateviewModel.ProceedWithUplers_ExitPolicyOption = onBoardContract_Details_Result.ProceedWithUplers_ExitPolicyOption;
        //            onBoardUpdateviewModel.ProceedWithUplers_LeavePolicyOption = onBoardContract_Details_Result.ProceedWithUplers_LeavePolicyOption;
        //            onBoardUpdateviewModel.LeavePolicyPasteLinkName = onBoardContract_Details_Result.ProceedWithClient_LeavePolicyLink;
        //            onBoardUpdateviewModel.LeavePolicyFileName = onBoardContract_Details_Result.ProceedWithClient_LeavePolicyFileUpload;
        //            onBoardUpdateviewModel.ProceedWithClient_LeavePolicyOption = onBoardContract_Details_Result.ProceedWithClient_LeavePolicyOption;

        //            List<GenOnBoardClientLeavePolicy> OnBoardClientLeavePolicydata = db.GenOnBoardClientLeavePolicies.Where(x => x.OnBoardId == onBoardId).ToList();
        //            if (OnBoardClientLeavePolicydata != null)
        //            {
        //                List<GenOnBoardClientLeavePolicy> OnBoardClientLeavePolicy = new List<GenOnBoardClientLeavePolicy>();
        //                OnBoardClientLeavePolicy = OnBoardClientLeavePolicydata;
        //                Session["OnBoardClientLeavePolicy"] = OnBoardClientLeavePolicy;
        //            }
        //            object[] param2 = new object[] { onBoardId };

        //            onBoardUpdateviewModel.Devicemulticheckbox = new List<DeviceOptionVM>();
        //            onBoardUpdateviewModel.Devicemulticheckbox = (from a in commonInterface.Onboard.Sproc_OnBoardPolicy_DeviceMaster_Update(CommonLogic.ConvertToParamString(param2)) select new DeviceOptionVM { ID = a.ID.ToString(), IsSelected = Convert.ToBoolean(a.IsActive), Text = a.DeviceName, Qty = a.Qty, Cost = a.DeviceCost, OtherDetails = a.Client_DeviceDescription, DeviceDescription = a.DeviceDescription }).ToList();
        //        }
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "OnBoard is not found" });
        //    }
        //    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Details = onBoardUpdateviewModel });

        //}
    }
}
