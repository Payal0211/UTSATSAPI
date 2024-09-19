namespace UTSATSAPI.Controllers
{
    using ClosedXML.Excel;
    using FluentValidation;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
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
    using static UTSATSAPI.Helpers.Enum;

    [Authorize]
    [Route("Engagement/")]
    [ApiController]
    public class EngagementController : ControllerBase
    {

        #region Variables
        private readonly ICommonInterface _commonInterface;
        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private readonly IConfiguration _configuration;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly IEngagement _iEngagement;
        private readonly ITalentReplacement _iTalentReplacement;
        #endregion

        #region Constructor
        public EngagementController(ICommonInterface commonInterface, IConfiguration iConfiguration,
            TalentConnectAdminDBContext talentConnectAdminDBContext, IUniversalProcRunner universalProcRunner, IEngagement iEngagement, ITalentReplacement iTalentReplacement)
        {
            _commonInterface = commonInterface;
            _configuration = iConfiguration;
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
            _universalProcRunner = universalProcRunner;
            _iEngagement = iEngagement;
            _iTalentReplacement = iTalentReplacement;
        }
        #endregion

        #region Public APIs
        [HttpGet("Filters")]
        public async Task<ObjectResult> Filters()
        {
            try
            {
                EngagementFiltersModel engagementFiltersModel = new EngagementFiltersModel();

                engagementFiltersModel.ClientFeedback = new List<SelectListItem>();
                engagementFiltersModel.ClientFeedback.Add(new SelectListItem { Value = "Green", Text = "Green" });
                engagementFiltersModel.ClientFeedback.Add(new SelectListItem { Value = "Red", Text = "Red" });
                engagementFiltersModel.ClientFeedback.Add(new SelectListItem { Value = "Orange", Text = "Orange" });
                engagementFiltersModel.ClientFeedback.Add(new SelectListItem { Value = "No Feedback", Text = "No Feedback" });

                engagementFiltersModel.TypeOfHiring = new List<SelectListItem>();
                engagementFiltersModel.TypeOfHiring.Add(new SelectListItem { Value = "Contractual", Text = "Contractual" });
                engagementFiltersModel.TypeOfHiring.Add(new SelectListItem { Value = "DP", Text = "DP" });

                engagementFiltersModel.CurrentStatus = new List<SelectListItem>();
                engagementFiltersModel.CurrentStatus.Add(new SelectListItem { Value = "In Replacement", Text = "In Replacement" });
                engagementFiltersModel.CurrentStatus.Add(new SelectListItem { Value = "Ongoing", Text = "Ongoing" });
                engagementFiltersModel.CurrentStatus.Add(new SelectListItem { Value = "Engagement Completed", Text = "Engagement Completed" });

                engagementFiltersModel.TSCName = await _iEngagement.GetTSCNameList();

                // Commented by Ashwin on 26 jul,2023
                // because of large data it is taking time to load on REACT side.
                //engagementFiltersModel.Company = _talentConnectAdminDBContext.GenCompanies.ToList().Select(x => new SelectListItem
                //{
                //    Value = x.Id.ToString(),
                //    Text = x.Company
                //}).OrderBy(y => y.Text);

                engagementFiltersModel.OnBoardingLostReasons = _talentConnectAdminDBContext.PrgOnboardLostReasons.ToList().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Reason
                }).OrderBy(y => y.Text);

                engagementFiltersModel.GEO = _talentConnectAdminDBContext.PrgGeos.ToList().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Geo
                }).OrderBy(y => y.Text);

                engagementFiltersModel.Postion = _talentConnectAdminDBContext.PrgTalentRoles.Where(x => x.IsActive == true).OrderBy(x => x.TalentRole).ToList().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.TalentRole
                }).OrderBy(y => y.Text);

                engagementFiltersModel.NBDName = _talentConnectAdminDBContext.UsrUsers.Where(x => x.IsActive == true && x.IsNewUser == true && (x.UserTypeId == 4 || x.UserTypeId == 9)).ToList().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FullName
                }).OrderBy(y => y.Text);

                engagementFiltersModel.AMName = _talentConnectAdminDBContext.UsrUsers.Where(x => x.IsActive == true && x.IsNewUser == false && (x.UserTypeId == 4 || x.UserTypeId == 9)).ToList().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FullName
                }).OrderBy(y => y.Text);

                engagementFiltersModel.Pending = new List<SelectListItem>();
                engagementFiltersModel.Pending.Add(new SelectListItem { Value = "yes", Text = "yes" });
                engagementFiltersModel.Pending.Add(new SelectListItem { Value = "No", Text = "No" });

                engagementFiltersModel.Lost = new List<SelectListItem>();
                engagementFiltersModel.Lost.Add(new SelectListItem { Value = "1", Text = "yes" });
                engagementFiltersModel.Lost.Add(new SelectListItem { Value = "0", Text = "No" });

                engagementFiltersModel.Years = new List<SelectListItem>();
                for (int i = 2000; i <= (DateTime.Now.Year + 20); i++)
                {
                    engagementFiltersModel.Years.Add(new SelectListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    });
                }

                engagementFiltersModel.EngagementTenure = new List<SelectListItem>();
                for (int i = 1; i <= 99; i++)
                {
                    engagementFiltersModel.EngagementTenure.Add(new SelectListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    });
                }

                engagementFiltersModel.SearchType = new List<SelectListItem>();
                engagementFiltersModel.SearchType.Add(new SelectListItem { Value = "All Time", Text = "All Time", Selected = true });

                engagementFiltersModel.DeployedSource = new List<SelectListItem>();
                engagementFiltersModel.DeployedSource.Add(new SelectListItem { Value = "POOL", Text = "POOL" });
                engagementFiltersModel.DeployedSource.Add(new SelectListItem { Value = "ODR", Text = "ODR" });
                engagementFiltersModel.DeployedSource.Add(new SelectListItem { Value = "POOL + ODR", Text = "POOL + ODR" });


                /* -====================Commented by Ashwin on 26 jul,2023=========================
                 * because of large data it is taking time to load on REACT side.
                 * 
                 * 
                engagementFiltersModel.EngagementIds = _talentConnectAdminDBContext.GenOnBoardTalents.ToList().Where(x => !string.IsNullOrWhiteSpace(x.EngagemenId)).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.EngagemenId
                }).OrderBy(y => y.Text);
                engagementFiltersModel.HRIds = _talentConnectAdminDBContext.GenSalesHiringRequests.ToList().Where(x => !string.IsNullOrWhiteSpace(x.HrNumber)).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.HrNumber
                }).OrderBy(y => y.Text);

                engagementFiltersModel.Clients = _talentConnectAdminDBContext.GenContacts.ToList().Where(x => !string.IsNullOrWhiteSpace(x.FullName)).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FullName
                }).OrderBy(y => y.Text);

                engagementFiltersModel.Talents = _talentConnectAdminDBContext.GenTalents.ToList().Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).OrderBy(y => y.Text);
                *
                */

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Filter List", Details = engagementFiltersModel });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("List")]
        public async Task<IActionResult> EngagementList(ListEngagementFilterModel listEngagementFilterModel)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                #region Pre-Validation
                if (listEngagementFilterModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide filters" });
                #endregion

                string Sortdatafield = "CreatedbyDatetime";
                string Sortorder = "desc";

                //UTS-3768: Apply table search for EngagementID, HRID, Client, Company, Talent
                //Trim the blank spaces from the end of the search text.
                string searchText = string.Empty;
                if (!string.IsNullOrEmpty(listEngagementFilterModel.searchText))
                {
                    searchText = listEngagementFilterModel.searchText.TrimStart().TrimEnd();
                }

                listEngagementFilterModel.totalrecord = (listEngagementFilterModel.IsExport) ? 0 : listEngagementFilterModel.totalrecord;
                object[] param = new object[] {
                    listEngagementFilterModel.pagenumber, listEngagementFilterModel.totalrecord,
                    Sortdatafield,
                    Sortorder,
                    listEngagementFilterModel.filterFieldsEngagement.ClientFeedback,
                    listEngagementFilterModel.filterFieldsEngagement.TypeOfHiring,
                    listEngagementFilterModel.filterFieldsEngagement.CurrentStatus,
                    listEngagementFilterModel.filterFieldsEngagement.TSCName,
                    listEngagementFilterModel.filterFieldsEngagement.Company,
                    listEngagementFilterModel.filterFieldsEngagement.GEO,
                    listEngagementFilterModel.filterFieldsEngagement.Position,
                    listEngagementFilterModel.filterFieldsEngagement.EngagementTenure,
                    listEngagementFilterModel.filterFieldsEngagement.NBDName,
                    listEngagementFilterModel.filterFieldsEngagement.AMName,
                    listEngagementFilterModel.filterFieldsEngagement.Pending,
                    listEngagementFilterModel.filterFieldsEngagement.SearchMonth,
                    listEngagementFilterModel.filterFieldsEngagement.SearchYear,
                    listEngagementFilterModel.filterFieldsEngagement.SearchType,
                    listEngagementFilterModel.filterFieldsEngagement.Islost,
                    listEngagementFilterModel.filterFieldsEngagement.EngagementId,
                    listEngagementFilterModel.filterFieldsEngagement.HRId,
                    listEngagementFilterModel.filterFieldsEngagement.ClientIds,
                    listEngagementFilterModel.filterFieldsEngagement.TalentIds,
                    listEngagementFilterModel.filterFieldsEngagement.DeployedSource,
                    LoggedInUserId,
                    searchText,
                    listEngagementFilterModel.filterFieldsEngagement.OnBoardLostReasons
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_BusinessDashboard_Result> listEngagement = await _iEngagement.ListEngagement(paramasString);
                if (listEngagement.Any())
                {
                    if (listEngagementFilterModel.IsExport)
                    {
                        return ExportEngagementDetails(listEngagement);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(listEngagement, listEngagement[0].TotalRecords, listEngagementFilterModel.totalrecord, listEngagementFilterModel.pagenumber) });
                    }
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Engagement" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Edit AM
        [HttpGet("GetAMDetails")]
        public async Task<ObjectResult> GetAMDetails(long PayOutID)
        {
            try
            {
                #region Prevalidation
                if (PayOutID <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide PayOutID" });
                }
                #endregion

                dynamic dObject = new ExpandoObject();

                Sproc_UTS_Get_PayOut_Basic_Information_Result Detail = _iEngagement.PayOut_Basic_Informtion(PayOutID);
                dObject.PayOut_Basic_Informtion = Detail;

                var UserList = await _iEngagement.GetUserListForAMChange((long)Detail.AM_SalesPersonID);

                dObject.AMList = UserList.ToList().Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.Id.ToString()
                });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = dObject });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data Not Available" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("UpdateAMForPayOut")]
        public async Task<ObjectResult> UpdateAMForPayOut(UpdateAM_Payout updateAM)
        {
            try
            {
                #region Prevalidation
                if (updateAM != null)
                {
                    if (updateAM.ContactID == null || updateAM.ContactID <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide ContactID" });
                    }
                    if (updateAM.HiringRequestID == null || updateAM.HiringRequestID <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide HiringRequestID" });
                    }
                    if (updateAM.OnBoardID == null || updateAM.OnBoardID <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide OnBoardID" });
                    }
                    if (updateAM.PayOutID == null || updateAM.PayOutID <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide PayOutID" });
                    }
                    if (updateAM.Month == null || updateAM.Month <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Month" });
                    }
                    if (updateAM.Year == null || updateAM.Year <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Year" });
                    }
                    if (updateAM.OldAMPersonID == null || updateAM.OldAMPersonID <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide OldAM_SalesPersonID" });
                    }
                    if (updateAM.NewAMPersonID == null || updateAM.NewAMPersonID <= 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide NewAM_SalesPersonID" });
                    }
                    if (string.IsNullOrEmpty(updateAM.Comment))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Comment" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper details, object must not be null" });
                }
                #endregion

                #region update AM details

                long LoggedInUserId = SessionValues.LoginUserId;

                object[] param = new object[]
                {
                    updateAM.HiringRequestID,
                    updateAM.OnBoardID,
                    updateAM.TalentID,
                    updateAM.PayOutID,
                    updateAM.Year,
                    updateAM.Month,
                    updateAM.NewAMPersonID,
                    LoggedInUserId
                };
                string paramasString = CommonLogic.ConvertToParamString(param);

                _iEngagement.Update_AMDetails_In_PayOut(paramasString);

                #endregion

                #region SendEmail
                var oldAMUserHierarchy = await _iEngagement.GetHierarchyForEmail(updateAM.OldAMPersonID.ToString());
                var newAMUserHierarchy = await _iEngagement.GetHierarchyForEmail(updateAM.NewAMPersonID.ToString());

                Sproc_UTS_Get_PayOut_Basic_Information_Result PayOutDetails = _iEngagement.PayOut_Basic_Informtion(updateAM.PayOutID);

                EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                emailBinder.SendEmailForChangeAMForPayOut(updateAM, PayOutDetails, oldAMUserHierarchy, newAMUserHierarchy);
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "AM change Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = ex.Message });
            }
        }
        #endregion

        #region Add/Edit TSC
        [HttpGet("GetTSCUsersDetail")]
        public ObjectResult GetTSCUsersDetail(long? OnBoardId)
        {
            try
            {
                TSCEditDetailViewModel TSCEditDetail = new TSCEditDetailViewModel();

                #region PreValidation

                if (OnBoardId == 0 || OnBoardId == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide OnBoard ID." });
                }

                #endregion

                #region Fetch TSC Edit details

                object[] param = new object[] { OnBoardId };
                string paramasString = CommonLogic.ConvertToParamString(param);

                Sproc_GetTSCUserEditDetail_Result TSCUserEditDetail = _iEngagement.Sproc_GetTSCUserEditDetail(paramasString);
                if (TSCUserEditDetail != null)
                {
                    TSCEditDetail.EngagementID = TSCUserEditDetail.EngagementID;
                    TSCEditDetail.TalentName = TSCUserEditDetail.TalentName;
                    TSCEditDetail.CurrentTSCName = TSCUserEditDetail.CurrentTSCName;
                    TSCEditDetail.EditTSCReason = TSCUserEditDetail.EditTSCReason;
                    TSCEditDetail.TSCPersonID = TSCUserEditDetail.TSCPersonID;

                }
                #endregion

                #region FetchDropdown values
                //Fetch the TscUsers dropdown values.
                var drpTSCUsers = new List<SelectListItem>();
                drpTSCUsers = _talentConnectAdminDBContext.UsrUsers.Join(_talentConnectAdminDBContext.GenTscusers, u => u.Id, tsc => tsc.UserId,
                                                          (u, tsc) => new { u, tsc }).Select(m => new SelectListItem
                                                          {
                                                              Value = m.u.Id.ToString(),
                                                              Text = m.u.FullName
                                                          }).ToList();
                TSCEditDetail.DrpTSCUserList = drpTSCUsers;
                #endregion

                if (TSCEditDetail != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = TSCEditDetail });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Not found" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPost("UpdateTSCName")]
        public ObjectResult UpdateTSCName(UpdateTSCNameViewModel updateTSCNameViewModel)
        {
            try
            {
                long? LoggedInUserID = SessionValues.LoginUserId;
                GenOnBoardTalent? onBoardTalents = null;
                #region PreValidation

                if (updateTSCNameViewModel == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                if (updateTSCNameViewModel.OnBoardID == 0 || updateTSCNameViewModel.OnBoardID == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide OnBoardID." });
                }
                else
                {
                    onBoardTalents = _talentConnectAdminDBContext.GenOnBoardTalents.Where(m => m.Id == updateTSCNameViewModel.OnBoardID).Select(m => m).FirstOrDefault();
                    if (onBoardTalents == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Onboard data not exist." });
                    }
                }
                #endregion

                object[] param = new object[]
                {
                    updateTSCNameViewModel.OnBoardID,
                    updateTSCNameViewModel.TSCUserId,
                    updateTSCNameViewModel.TSCEditReason,
                    LoggedInUserID,
                    updateTSCNameViewModel.OldTSC_PersonID,
                    updateTSCNameViewModel.Month,
                    updateTSCNameViewModel.Year
                };
                string paramasString = CommonLogic.ConvertToParamString(param);
                _iEngagement.Sproc_UpdateTSCUser(paramasString);

                #region SendEmail
                EmailBinder binder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                object[] param1 = new object[]
                {
                    updateTSCNameViewModel.OnBoardID
                };
                string paramasString1 = CommonLogic.ConvertToParamString(param1);
                Sproc_GetEmailDetailForTSCAssignment_Result result = _iEngagement.GetEmailDetailForTSCAssignment(paramasString1);
                if (result != null)
                {
                    if (result.OldTSCEmail != "" && result.OldTSCEmail != null)
                    {
                        bool SendEmailToOldTSC = binder.SendTSCAssignmentEmailToOLDTSC(result);
                    }
                    if (result.NewTSCEmail != "" && result.NewTSCEmail != null)
                    {
                        bool SendEmailToNewTSC = binder.SendTSCAssignmentEmailToNewTSC(result);
                    }
                }

                #endregion

                #region Insert TSCEdit History 
                if (updateTSCNameViewModel.OldTSC_PersonID != 0)
                {
                    param = new object[]
                    {
                         Action_Of_History.Edit_TSC, onBoardTalents.HiringRequestId, onBoardTalents.TalentId??0, false, LoggedInUserID, 0, 0, "", updateTSCNameViewModel.OnBoardID, false, false, 0, 0, (short)AppActionDoneBy.UTS
                    };
                }
                else
                {
                    param = new object[]
                    {
                        Action_Of_History.Add_TSC, onBoardTalents.HiringRequestId, onBoardTalents.TalentId??0, false, LoggedInUserID, 0, 0, "", updateTSCNameViewModel.OnBoardID, false, false, 0, 0, (short)AppActionDoneBy.UTS
                    };
                }

                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                #endregion
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPost("TSCAutoAssignment")]
        public ObjectResult TSCAutoAssignment(long? OnBoardId)
        {
            try
            {
                long? LoggedInUserID = SessionValues.LoginUserId;
                GenOnBoardTalent? onBoardTalents = null;
                #region PreValidation
                if (OnBoardId == 0 || OnBoardId == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide OnBoardID." });
                }
                else
                {
                    onBoardTalents = _talentConnectAdminDBContext.GenOnBoardTalents.FirstOrDefault(m => m.Id == OnBoardId);
                    if (onBoardTalents == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Onboard data not exist." });
                    }
                }

                #endregion

                object[] param = new object[]
                {
                    OnBoardId,
                    LoggedInUserID
                };
                string paramasString = CommonLogic.ConvertToParamString(param);

                _iEngagement.Sproc_Get_User_For_TSCAutoAssignment_BasedOnRoundRobin(paramasString);

                #region SendEmail
                EmailBinder binder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                object[] param1 = new object[]
                {
                    OnBoardId
                };
                string paramasString1 = CommonLogic.ConvertToParamString(param1);
                Sproc_GetEmailDetailForTSCAssignment_Result result = _iEngagement.GetEmailDetailForTSCAssignment(paramasString1);
                if (result != null)
                {
                    bool SendEmailToNewTSC = binder.SendTSCAutoAssignmentEmail(result);

                }

                #endregion

                #region Insert TSCAssignment History 
                param = new object[]
                {
                 Action_Of_History.TSC_AutoAssignment, onBoardTalents.HiringRequestId, onBoardTalents.TalentId??0, false, LoggedInUserID, 0, 0, "", OnBoardId, false, false, 0, 0, (short)AppActionDoneBy.UTS
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        
        [HttpGet("GetFeedbackFormContent")]
        public async Task<ObjectResult> GetFeedbackFormContent(long? HR_ID, long? OnBoardID)
        {
            try
            {
                #region PreValidation
                if (OnBoardID == null || OnBoardID == 0 || HR_ID == null || HR_ID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide HR Id and OnBoardID" });

                }
                #endregion

                OnBoardTalents_ClientFeedback OnBoard = new OnBoardTalents_ClientFeedback();


                OnBoard.OnBoardID = OnBoardID ?? 0;
                OnBoard.HiringRequest_ID = HR_ID ?? 0;
                var OnBoardData = _talentConnectAdminDBContext.GenOnBoardTalents.FirstOrDefault(x => x.Id == OnBoardID);
                if (OnBoardData != null)
                {
                    OnBoard.ContactID = OnBoardData.ContactId ?? 0;
                    OnBoard.EngagemenID = OnBoardData.EngagemenId ?? "";
                    if (OnBoardData.TalentId != null && OnBoardData.TalentId != 0)
                    {
                        OnBoard.TalentName = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == OnBoardData.TalentId).Name ?? "";
                        OnBoard.TalentID = OnBoardData.TalentId;
                    }

                }
                OnBoard.HRNumber = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(x => x.Id == HR_ID).HrNumber ?? "";

                OnBoard.drpFeedbackType = new List<SelectListItem>();
                OnBoard.drpFeedbackType.Add(new SelectListItem { Value = "0", Text = "--Select--" });
                OnBoard.drpFeedbackType.Add(new SelectListItem { Value = "Green", Text = "Green" });
                OnBoard.drpFeedbackType.Add(new SelectListItem { Value = "Red", Text = "Red" });
                OnBoard.drpFeedbackType.Add(new SelectListItem { Value = "Orange", Text = "Orange" });

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Onboard Popup form ", Details = OnBoard });

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet("Calculate_ActualNR_From_BRPR")]
        public async Task<ObjectResult> Calculate_ActualNR_From_BRPR(decimal BR, decimal PR, string Currency)
        {
            try
            {
                #region PreValidation
                if (BR == 0 || BR == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide BR" });
                }
                else if (PR == 0 || PR == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide PR" });
                }
                else if (BR < PR)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Bill rate should be greater than pay rate" });
                }
                else if (string.IsNullOrEmpty(Currency))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Currency" });
                }
                #endregion

                object[] param2 = new object[] { Convert.ToDecimal(BR), Convert.ToDecimal(PR), Currency };

                NRPerCentageValue nRPerCentageValue = _iEngagement.Sproc_Calculate_ActualNR_From_BR_PR(CommonLogic.ConvertToParamString(param2));
                if (nRPerCentageValue != null && nRPerCentageValue.NR_Percentage > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Calculated Value", Details = nRPerCentageValue.NR_Percentage });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Invalid Amount" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet("Get_engagement_Edit_All_BR_PR")]
        public async Task<ObjectResult> Get_engagement_Edit_All_BR_PR(int OnboardID)
        {
            try
            {
                #region PreValidation
                if (OnboardID == 0 || OnboardID == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide BR" });
                }
                #endregion

                object[] param2 = new object[] { OnboardID, null, null };
                string paramString = CommonLogic.ConvertToParamString(param2);

                List<Sproc_Get_engagement_Edit_All_BR_PR_Result> result = await _iEngagement.IGet_engagement_Edit_All_BR_PR(paramString);

                if (result.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Calculated Value", Details = result });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet("EditBillRatePayRate")]
        public async Task<ObjectResult> EditBillRatePayRate(long? HR_ID, long? OnBoardID, int Month, int Year)
        {
            try
            {
                #region PreValidation
                if (OnBoardID == null || OnBoardID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide OnBoardID" });

                }
                if (HR_ID == null || HR_ID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide HR Id" });
                }
                #endregion

                #region check login User have special rights or not

                long LoggedInUserId = SessionValues.LoginUserId;
                var SpecialRights_UserIDs = await _talentConnectAdminDBContext.GenSystemConfigurations.Where(x => x.Key == "BRPR_UserAddReason" && x.IsActive == true).FirstOrDefaultAsync();
                bool IsSpecialRights = false;

                if (SpecialRights_UserIDs != null && SpecialRights_UserIDs.Value != null)
                {
                    string[] UserIDs = SpecialRights_UserIDs.Value.Split(",", StringSplitOptions.TrimEntries);
                    IsSpecialRights = UserIDs.Contains(LoggedInUserId.ToString());
                }

                #endregion

                EditBillRatePayRateViewModel editBillRatePayRateViewModel = new EditBillRatePayRateViewModel();

                if (IsSpecialRights)
                {
                    editBillRatePayRateViewModel.ReasonDrp = _talentConnectAdminDBContext.PrgEditBrprReasons.ToList().Select(x => new SelectListItem
                    {
                        Text = x.Reason,
                        Value = x.Id.ToString()
                    });
                }
                else
                {
                    editBillRatePayRateViewModel.ReasonDrp = _talentConnectAdminDBContext.PrgEditBrprReasons.Where(x => x.IsActive == true).ToList().Select(x => new SelectListItem
                    {
                        Text = x.Reason,
                        Value = x.Id.ToString()
                    });
                }

                editBillRatePayRateViewModel.CurrencyDrp = _talentConnectAdminDBContext.PrgCurrencyExchangeRates.ToList().Select(x => new SelectListItem
                {
                    Text = x.CurrencyCode,
                    Value = x.Id.ToString()
                });

                editBillRatePayRateViewModel.OnBoardID = OnBoardID.Value;
                GenOnBoardTalent gen_OnBoardTalents = _talentConnectAdminDBContext.GenOnBoardTalents.FirstOrDefault(x => x.Id == OnBoardID);
                GenPayoutInformation genPayoutInformation = _talentConnectAdminDBContext.GenPayoutInformations.FirstOrDefault(x => x.OnBoardId == OnBoardID.Value && x.ClientInvoiceDate.Value.Month == Month && x.ClientInvoiceDate.Value.Year == Year);

                if (genPayoutInformation != null)
                {
                    editBillRatePayRateViewModel.OnBoardID = genPayoutInformation.OnBoardId;
                    editBillRatePayRateViewModel.BillRate = genPayoutInformation.ClientFinalPayOutAmount ?? 0;
                    editBillRatePayRateViewModel.PayRate = genPayoutInformation.PayOutAmount ?? 0;

                    if (genPayoutInformation.ActualNrPercentage != null)
                    {
                        editBillRatePayRateViewModel.BillRate_NR = genPayoutInformation.ActualNrPercentage ?? 0;
                        editBillRatePayRateViewModel.PayRate_NR = genPayoutInformation.ActualNrPercentage ?? 0;
                    }
                    else
                    {
                        if (gen_OnBoardTalents != null && gen_OnBoardTalents.Nrpercentage > 0)
                        {
                            editBillRatePayRateViewModel.BillRate_NR = gen_OnBoardTalents.Nrpercentage ?? 0;
                            editBillRatePayRateViewModel.PayRate_NR = gen_OnBoardTalents.Nrpercentage ?? 0;
                        }

                    }

                    editBillRatePayRateViewModel.FinalBillRate = (decimal)(genPayoutInformation.ActualBillRate != null ? genPayoutInformation.ActualBillRate : genPayoutInformation.ClientFinalPayOutAmount.Value);
                    editBillRatePayRateViewModel.FinalPayRate = (decimal)(genPayoutInformation.ActualPayRate != null ? genPayoutInformation.ActualPayRate : genPayoutInformation.PayOutAmount.Value);

                    if (!string.IsNullOrEmpty(genPayoutInformation.ReasonBr))
                    {
                        var BRPRReason = _talentConnectAdminDBContext.PrgEditBrprReasons.Where(x => x.Reason.ToLower() == genPayoutInformation.ReasonBr.ToLower()).Select(x => x.Reason).FirstOrDefault();

                        if (string.IsNullOrEmpty(BRPRReason))
                        {
                            editBillRatePayRateViewModel.BillRateOtherReason = genPayoutInformation.ReasonBr;
                            editBillRatePayRateViewModel.BillRateReason = "Others";
                        }
                        else
                            editBillRatePayRateViewModel.BillRateReason = BRPRReason;
                    }
                    if (!string.IsNullOrEmpty(genPayoutInformation.ReasonPr))
                    {
                        var BRPRReason = _talentConnectAdminDBContext.PrgEditBrprReasons.Where(x => x.Reason.ToLower() == genPayoutInformation.ReasonPr.ToLower()).Select(x => x.Reason).FirstOrDefault();

                        if (string.IsNullOrEmpty(BRPRReason))
                        {
                            editBillRatePayRateViewModel.PayRateOtherReason = genPayoutInformation.ReasonPr;
                            editBillRatePayRateViewModel.PayRateReason = "Others";
                        }
                        else
                            editBillRatePayRateViewModel.PayRateReason = BRPRReason;
                    }

                    editBillRatePayRateViewModel.Currency = genPayoutInformation.CurrencyExchangeCode != null ? genPayoutInformation.CurrencyExchangeCode : "USD";
                    var prgCurrencyExchangeRates = _talentConnectAdminDBContext.PrgCurrencyExchangeRates
                        .FirstOrDefault(x => x.CurrencyCode == genPayoutInformation.CurrencyExchangeCode);
                    if (prgCurrencyExchangeRates != null)
                    {
                        editBillRatePayRateViewModel.CurrencyId = prgCurrencyExchangeRates.Id;
                    }
                    editBillRatePayRateViewModel.PayRateCurrencyId = editBillRatePayRateViewModel.CurrencyId;
                }

                //GenSalesHiringRequestDetail gen_SalesHiringRequest_Details = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.FirstOrDefault(x => x.Id == HR_ID);
                //if (gen_SalesHiringRequest_Details != null)
                //{
                //    = !string.IsNullOrEmpty(gen_SalesHiringRequest_Details.Currency) ? gen_SalesHiringRequest_Details.Currency : "";
                //}



                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Edit Bill/Pay Rate Popup form ", Details = editBillRatePayRateViewModel });

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPost("SaveBillRatePayRate")]
        public async Task<ObjectResult> SaveBillRatePayRate(SaveBillPayRateViewModel saveBillPayRateViewModel)
        {
            try
            {
                #region PreValidation
                if (saveBillPayRateViewModel == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request Object is empty" });
                }
                #endregion

                #region Validation
                if (saveBillPayRateViewModel.isEditBillRate)
                {
                    SaveBillPayRateViewModel_BillRateValidator validationRules = new SaveBillPayRateViewModel_BillRateValidator();
                    ValidationResult validationResult = validationRules.Validate(saveBillPayRateViewModel);
                    if (!validationResult.IsValid)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "Bill Rate") });
                    }
                }
                else
                {
                    SaveBillPayRateViewModel_PayRateValidator validationRules = new SaveBillPayRateViewModel_PayRateValidator();
                    ValidationResult validationResult = validationRules.Validate(saveBillPayRateViewModel);
                    if (!validationResult.IsValid)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "Pay Rate") });
                    }
                }
                #endregion

                #region SP Call
                saveBillPayRateViewModel.isFromMultiPopup = saveBillPayRateViewModel.isFromMultiPopup ?? false;
                //DEfault comment msg for Edit BR/PR
                if ((bool)saveBillPayRateViewModel.isFromMultiPopup)
                {
                    if (saveBillPayRateViewModel.isEditBillRate)
                        saveBillPayRateViewModel.BillRateComment = "BR updated via Edit All BR PR Action";
                    else
                        saveBillPayRateViewModel.payRateComment = "PR updated via Edit All BR PR Action";
                }

                saveBillPayRateViewModel.month = saveBillPayRateViewModel.month == 0 ? DateTime.Now.Month : saveBillPayRateViewModel.month;
                saveBillPayRateViewModel.Year = saveBillPayRateViewModel.Year == 0 ? DateTime.Now.Year : saveBillPayRateViewModel.Year;

                object[] param = new object[]
                {
                    saveBillPayRateViewModel.OnboardId,
                    saveBillPayRateViewModel.BillRate,
                    saveBillPayRateViewModel.PayRate,
                    saveBillPayRateViewModel.NR,
                    saveBillPayRateViewModel.BillRateComment,
                    saveBillPayRateViewModel.payRateComment,
                    saveBillPayRateViewModel.BillrateCurrency,
                    saveBillPayRateViewModel.month,
                    saveBillPayRateViewModel.Year,
                    saveBillPayRateViewModel.BillRateReason,
                    saveBillPayRateViewModel.payrateReason,
                    saveBillPayRateViewModel.ContractType
                };

                _universalProcRunner.ManipulationWithNULL(Constants.ProcConstant.Sproc_Add_Actual_BR_PR_NR, param);

                #endregion

                #region Email to buddy users, AM, TSC, Internal team

                object[] param2 = new object[] { saveBillPayRateViewModel.OnboardId, saveBillPayRateViewModel.month, saveBillPayRateViewModel.Year };

                string paramString = CommonLogic.ConvertToParamString(param2);
                List<Sproc_Get_engagement_Edit_All_BR_PR_Result> result = await _iEngagement.IGet_engagement_Edit_All_BR_PR(paramString);
                if (result != null && result.Any())
                {
                    EmailBinder binder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                    binder.SendEmailForChangePayOutDeatils(result.FirstOrDefault());
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Bill/Pay Rate Saved" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPost("SaveFeedbackClientOnBoard")]
        public async Task<ObjectResult> SaveClientFeedbacck(OnBoardTalents_ClientFeedback onBoardTalents_ClientFeedback)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                #region Variables
                GenOnBoardTalentsClientFeedback genOnBoardTalentsClientFeedback = new GenOnBoardTalentsClientFeedback();
                #endregion

                #region PreValidation
                if (onBoardTalents_ClientFeedback == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request Object is empty" });
                }
                #endregion

                #region Validation
                OnBoardTalents_ClientFeedbackValidator onBoardTalents_ClientFeedbackValidator = new OnBoardTalents_ClientFeedbackValidator();
                ValidationResult validationResult = onBoardTalents_ClientFeedbackValidator.Validate(onBoardTalents_ClientFeedback);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "Client Feedback") });
                }
                #endregion

                genOnBoardTalentsClientFeedback.HiringRequestId = onBoardTalents_ClientFeedback.HiringRequest_ID;
                genOnBoardTalentsClientFeedback.OnBoardId = onBoardTalents_ClientFeedback.OnBoardID;
                genOnBoardTalentsClientFeedback.ContactId = onBoardTalents_ClientFeedback.ContactID;
                genOnBoardTalentsClientFeedback.FeedbackType = onBoardTalents_ClientFeedback.FeedbackType;
                genOnBoardTalentsClientFeedback.FeedbackComment = onBoardTalents_ClientFeedback.FeedbackComment;
                genOnBoardTalentsClientFeedback.FeedbackActionToTake = onBoardTalents_ClientFeedback.FeedbackActionToTake;
                genOnBoardTalentsClientFeedback.FeedbackCreatedDateTime = onBoardTalents_ClientFeedback.FeedbackCreatedDateTime.Value;
                genOnBoardTalentsClientFeedback.FileName = (onBoardTalents_ClientFeedback.SupportingFilename == null) ? string.Empty : onBoardTalents_ClientFeedback.SupportingFilename;
                genOnBoardTalentsClientFeedback.CreatedById = Convert.ToInt32(LoggedInUserId);
                _talentConnectAdminDBContext.GenOnBoardTalentsClientFeedbacks.Add(genOnBoardTalentsClientFeedback);
                _talentConnectAdminDBContext.SaveChanges();


                object[] param2 = new object[]
                      {
                            Action_Of_History.Onboard_ClientFeedback, onBoardTalents_ClientFeedback.HiringRequest_ID, onBoardTalents_ClientFeedback.TalentID ?? 0, false, LoggedInUserId, 0, 0, "", onBoardTalents_ClientFeedback.OnBoardID, false, false, 0, 0, (short)AppActionDoneBy.UTS
                      };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param2);

                #region Send Email after submitted Feedback
                long OnBoardID = onBoardTalents_ClientFeedback.OnBoardID;
                long AMSalesPersonId = 0, TSCID = 0;
                string AMEmail = "", AmName = "", TscName = "", TscEmail = ""
                        , FeedbackType = onBoardTalents_ClientFeedback.FeedbackType, FeedBackComment = onBoardTalents_ClientFeedback.FeedbackComment, FeedbackActionToTake = string.IsNullOrEmpty(onBoardTalents_ClientFeedback.FeedbackActionToTake) ? "NA" : onBoardTalents_ClientFeedback.FeedbackActionToTake;
                DateTime FeedbackDate = onBoardTalents_ClientFeedback.FeedbackCreatedDateTime.Value;

                var genOnBoardTalents = _talentConnectAdminDBContext.GenOnBoardTalents.Where(x => x.Id == OnBoardID).FirstOrDefault();
                if (genOnBoardTalents != null)
                {
                    AMSalesPersonId = genOnBoardTalents.AmSalesPersonId ?? 0;
                    TSCID = genOnBoardTalents.TscPersonId ?? 0;
                    if (AMSalesPersonId != 0)
                    {
                        var usr = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == TSCID).FirstOrDefault();
                        if (usr != null)
                        {
                            TscName = usr.FullName;
                            TscEmail = usr.EmailId;
                        }
                        //var usrUser = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == AMSalesPersonId).FirstOrDefault();
                        //if (usrUser != null)
                        //{
                        //    AMEmail = usrUser.EmailId;
                        //    AmName = usrUser.FullName;

                        //    EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                        //    emailBinder.SendClientFeedbackFromEngagementToInternalTeam(AMSalesPersonId, AmName, AMEmail, FeedbackDate, FeedbackType, FeedBackComment, FeedbackActionToTake, TscName, TscEmail);
                        //}

                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Feedback Saved!", Details = genOnBoardTalentsClientFeedback });

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet("GetContentForAddInvoice")]
        public async Task<ObjectResult> GetContentForAddInvoice(long? OnBoardID)
        {
            try
            {
                #region PreValidation
                if (OnBoardID == null || OnBoardID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide HR Id and OnBoardID" });

                }
                #endregion

                EditBillRatePayRateViewModel editBillRatePayRateViewModel = new EditBillRatePayRateViewModel();

                editBillRatePayRateViewModel.OnBoardID = OnBoardID.Value;
                GenOnBoardTalent gen_OnBoardTalents = _talentConnectAdminDBContext.GenOnBoardTalents.FirstOrDefault(x => x.Id == OnBoardID);

                if (gen_OnBoardTalents != null)
                {
                    GenSalesHiringRequest gen_SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(x => x.Id == gen_OnBoardTalents.HiringRequestId);
                    if (gen_SalesHiringRequest != null)
                    {
                        editBillRatePayRateViewModel.HrNumber = gen_SalesHiringRequest.HrNumber;
                    }
                    GenTalent gen_Talent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == gen_OnBoardTalents.TalentId);
                    if (gen_Talent != null)
                    {
                        editBillRatePayRateViewModel.TalentName = gen_Talent.Name;
                    }
                }

                //editBillRatePayRateViewModel.InvoiceStatus = _talentConnectAdminDBContext.PrgTalentPayoutStatuses.Where(x => x.IsActive == true).Select(x => new SelectListItem
                //{
                //    Value = x.Id.ToString(),
                //    Text = x.PayOutStatus
                //});

                var InvoiceStatus = await (_talentConnectAdminDBContext.PrgTalentPayoutStatuses.Select(x => new InvoiceStatus { Value = x.PayOutStatus, ID = x.Id })).ToListAsync();
                editBillRatePayRateViewModel._InvoiceStatus = InvoiceStatus;
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Add Invoice Popup", Details = editBillRatePayRateViewModel });

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPost("SaveInvoiceDetails")]
        public async Task<ObjectResult> SaveInvoiceDetails(SaveInvoiceDetailsFromEngagementDashboard saveInvoiceDetailsFromEngagementDashboard)
        {
            try
            {
                #region PreValidation
                if (saveInvoiceDetailsFromEngagementDashboard == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                #endregion

                #region Validation
                SaveInvoiceDetailsValidators validationRules = new SaveInvoiceDetailsValidators();
                ValidationResult validationResult = validationRules.Validate(saveInvoiceDetailsFromEngagementDashboard);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "Invoice error") });
                }
                #endregion

                long LoggedInUserId = SessionValues.LoginUserId;


                object[] param2 = new object[]
                      {
                          saveInvoiceDetailsFromEngagementDashboard.OnBoardID,
                          saveInvoiceDetailsFromEngagementDashboard.InvoiceSentdate,
                          saveInvoiceDetailsFromEngagementDashboard.InvoiceNumber,
                          saveInvoiceDetailsFromEngagementDashboard.InvoiceStatusId,
                          saveInvoiceDetailsFromEngagementDashboard.PaymentDate,
                          saveInvoiceDetailsFromEngagementDashboard.Month ?? DateTime.Now.Month,
                          saveInvoiceDetailsFromEngagementDashboard.Year ?? DateTime.Now.Year,
                          LoggedInUserId
                      };
                _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Add_Invoice_Details_For_Engagements, param2);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Invoice Details Saved!" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet("GetContentEndEnagagement")]
        public async Task<ObjectResult> ChangeContractEndDate(long OnBoardID)
        {
            #region PreValidation
            if (OnBoardID == null || OnBoardID == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide HR Id and OnBoardID" });
            }
            #endregion

            TalentPayOutInformationDetailsViewModel talentPayOutInformationDetailsViewModel = new TalentPayOutInformationDetailsViewModel();
            var objOnBoardClientContractDetails = _talentConnectAdminDBContext.GenOnBoardClientContractDetails.FirstOrDefault(x => x.OnBoardId == OnBoardID);
            var objOnBoardDetails = _talentConnectAdminDBContext.GenOnBoardTalents.FirstOrDefault(x => x.Id == OnBoardID);

            if (objOnBoardClientContractDetails != null)
            {
                DateTime ContractStartDate, ContractEndDate;
                if (objOnBoardClientContractDetails.ContractStartDate != null && objOnBoardClientContractDetails.ContractEndDate != null)
                {
                    ContractStartDate = Convert.ToDateTime(objOnBoardClientContractDetails.ContractStartDate);
                    ContractEndDate = Convert.ToDateTime(objOnBoardClientContractDetails.ContractEndDate);

                    talentPayOutInformationDetailsViewModel.ContractEndDate = ContractEndDate.ToString("dd/MM/yyyy");
                    talentPayOutInformationDetailsViewModel.ContractStartDate = ContractStartDate.ToString("dd/MM/yyyy");
                }

                talentPayOutInformationDetailsViewModel.ContractDetailID = Convert.ToInt32(objOnBoardClientContractDetails.Id);

                //UTS-7389: Fetch the Engagement Values to be replaced.
                List<Sproc_Get_HRIDEngagementID_ForReplacement_Result> Sproc_Get_HRIDEngagementID_ForReplacement = await _iTalentReplacement.Sproc_Get_HRIDEngagementID_ForReplacement(OnBoardID.ToString());
                if (Sproc_Get_HRIDEngagementID_ForReplacement != null)
                {
                    talentPayOutInformationDetailsViewModel.ReplacementEngAndHR = Sproc_Get_HRIDEngagementID_ForReplacement.ToList()
                        .Select(x => new MastersResponseModel
                        {
                            StringIdValue = x.IDvalue,
                            Value = x.Dropdowntext ?? "",
                            Id = x.ID ?? 0,
                            seletected = Convert.ToBoolean(x.IsHR) // If the value is of HR then true
                        }).ToList();
                }

                object[] objParam = new object[] { OnBoardID };
                string strParamas = CommonLogic.ConvertToParamString(objParam);
                var varTalent_RejectReason = await _iTalentReplacement.Sproc_Get_OnBoardDetailFor_C2H(strParamas);
                if (varTalent_RejectReason != null)
                {
                    talentPayOutInformationDetailsViewModel.DPNRPercentage = varTalent_RejectReason.DPNRPercentage ?? 0;
                    talentPayOutInformationDetailsViewModel.IsConvertToHireApplicable = varTalent_RejectReason.IsConvertToHireApplicable == 1 ? true : false;
                    talentPayOutInformationDetailsViewModel.Currency = varTalent_RejectReason.Currency;
                }

            }
            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Content Date", Details = talentPayOutInformationDetailsViewModel });
        }
        
        [HttpPost("ChangeContractEndDate")]
        public async Task<ObjectResult> ChangeContractEndDate(UpdateContractEndDateViewModel updateContractEndDateViewModel)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                long C2HOnBoardID = 0;
                string message = "Contract Date change !";
                #region PreValidation
                if (updateContractEndDateViewModel == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                #endregion

                #region Validation
                UpdateContractEndDateViewModelValidator validationRules = new UpdateContractEndDateViewModelValidator();
                ValidationResult validationResult = validationRules.Validate(updateContractEndDateViewModel);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "End Enagagement error") });
                }
                #endregion

                if (updateContractEndDateViewModel.Reason.Contains("'"))
                {
                    updateContractEndDateViewModel.Reason = updateContractEndDateViewModel.Reason.Replace("'", "''");
                }

                var varOnBoardClientContractDetails = _talentConnectAdminDBContext.GenOnBoardClientContractDetails.AsNoTracking().FirstOrDefault(x => x.Id == updateContractEndDateViewModel.ContractDetailID);
                var varOnBoard = _talentConnectAdminDBContext.GenOnBoardTalents.AsNoTracking().FirstOrDefault(x => x.Id == varOnBoardClientContractDetails.OnBoardId);
                if (varOnBoard != null && varOnBoardClientContractDetails != null)
                {
                    string contractEndDate = updateContractEndDateViewModel.ContractEndDate.ToString("MM/dd/yyyy");

                    object[] param2 = new object[]
                          {
                            updateContractEndDateViewModel.ContractDetailID, contractEndDate, updateContractEndDateViewModel.Reason, updateContractEndDateViewModel.FileName.Replace("'","''"), LoggedInUserId, updateContractEndDateViewModel.LostReasonID
                          };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Update_OnBoardClientContractEndDateDetails, param2);

                    if (updateContractEndDateViewModel.fileUpload != null)
                    {
                        byte[] img = CommonLogic.UploadImageFromBase64(updateContractEndDateViewModel.fileUpload.Base64ProfilePic);
                        if (img != null && img.Length > 0)
                        {
                            string fileName = string.Format("{0}.{1}", DateTime.Now.ToFileTime(), updateContractEndDateViewModel.fileUpload.Extenstion);
                            var pathToSave = CommonLogic.GetFileUploadFolderFor(Constants.MediaConstant.ContractEndDate);
                            string modifiedFilename = string.Format("{0}/{1}", pathToSave, fileName);
                            System.IO.File.WriteAllBytes(modifiedFilename, img);
                        }
                    }

                    // UTS-7389: If replacement is Yes then save replacement details
                    try
                    {
                        #region SaveReplacementDetails
                        if (Convert.ToBoolean(updateContractEndDateViewModel.IsReplacement) && updateContractEndDateViewModel.talentReplacement != null)
                        {
                            TalentReplacement talentReplacement = updateContractEndDateViewModel.talentReplacement;
                            talentReplacement = await _iTalentReplacement.SaveTalentReplacementData(talentReplacement, _universalProcRunner).ConfigureAwait(false);

                            talentReplacement.HiringRequestID = talentReplacement.HiringRequestID;
                            int talentStatusID = (short)prg_TalentStatus_AfterClientSelection.InReplacement;

                            if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                            {
                                ContactTalentPriorityResponseModel contactTalentPriorityResponseModel = new ContactTalentPriorityResponseModel();
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                GenTalent _Talents = await _iTalentReplacement.GetGenTalentsById(talentReplacement.TalentId.Value).ConfigureAwait(false);
                                _talentConnectAdminDBContext.Entry(_Talents).Reload();
                                if (_Talents != null)
                                {
                                    var varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(LoggedInUserId);
                                    var HiringRequestData = await _iTalentReplacement.GetGenSalesHiringRequestById(talentReplacement.HiringRequestID.Value).ConfigureAwait(false);
                                    if (HiringRequestData != null)
                                    {
                                        #region Save Data in model to send reponse to PHP team after serialize   

                                        contactTalentPriorityResponseModel.HRID = talentReplacement.HiringRequestID.Value;
                                        contactTalentPriorityResponseModel.HRStatusID = HiringRequestData.StatusId ?? 0;

                                        string TalentStatus = string.Empty;
                                        if (talentStatusID > 0)
                                        {
                                            TalentStatus = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == talentStatusID && x.IsActive == true).FirstOrDefault()?.TalentStatus;
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
                                        var varTalent_RejectReason = _commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas).ActualReason;
                                        talentDetail.Talent_RejectReason = varTalent_RejectReason;

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
                                            object[] atsParam = new object[] { contactTalentPriorityResponseModel.HRID, 0, _Talents.Id };
                                            string paramString = CommonLogic.ConvertToParamString(atsParam);

                                            ATSCall aTSCallforRound = new(_configuration, _talentConnectAdminDBContext);

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
                                            aTSCall.SaveContactTalentPriority(json, LoggedInUserId, talentReplacement.HiringRequestID.Value);
                                        }
                                        catch (Exception)
                                        {
                                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data save successfully" });
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                    }

                    #region //added Jimit 27-05-24 For C2H Functionality
                    try
                    {
                        if (updateContractEndDateViewModel.DPPercentage > 0 && updateContractEndDateViewModel.LostReasonID == 3)
                        {
                            object[] param = new object[]
                                 {
                                    varOnBoardClientContractDetails.OnBoardId,
                                     updateContractEndDateViewModel.DPPercentage,
                                     updateContractEndDateViewModel.DPAmount,
                                     updateContractEndDateViewModel.ExpectedCTC,
                                     updateContractEndDateViewModel.NewContractStartDate,
                                     LoggedInUserId,
                                     (short)AppActionDoneBy.UTS
                                 };
                            string paramasString = CommonLogic.ConvertToParamString(param);

                            C2HOnBoardID = _iEngagement.Sproc_TalentEngagementConverted_C2H(paramasString).C2HOnBoardId ?? 0;


                        }
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                    }
                    #endregion

                    #region ATS Call
                    if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                    {
                        Sproc_talent_engagement_Details_For_PHP_API_Result ObjResult = await _iEngagement.TalentEngagementDetails(varOnBoardClientContractDetails.OnBoardId, "Change ContractEnd Date");
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
                            ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                            aTSCall.SendTalentEngagementDetails(json, LoggedInUserId, varOnBoard.HiringRequestId);
                        }
                    }
                    #endregion

                    #region ATS Call For C2H
                    if (C2HOnBoardID > 0)
                    {
                        if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                        {
                            Sproc_talent_engagement_Details_For_PHP_API_Result ObjResult = await _iEngagement.TalentEngagementDetails(C2HOnBoardID, "Contract To Hire");
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
                                ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                                aTSCall.SendTalentEngagementDetails(json, LoggedInUserId, varOnBoard.HiringRequestId);
                            }
                        }
                    }
                    #endregion

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = message });

                }



                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data found" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet("ViewOnBoardFeedback")]
        public async Task<ObjectResult> ViewOnBoardFeedback(long? OnBoardID)
        {
            #region PreValidation
            if (OnBoardID == null || OnBoardID == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide HR Id and OnBoardID" });
            }
            #endregion
            OnBoardTalents_ClientFeedback onBoardTalents_ClientFeedback = new OnBoardTalents_ClientFeedback();

            onBoardTalents_ClientFeedback.OnBoardID = OnBoardID ?? 0;
            var OnBoardData = _talentConnectAdminDBContext.GenOnBoardTalents.FirstOrDefault(x => x.Id == OnBoardID);
            if (OnBoardData != null)
            {
                onBoardTalents_ClientFeedback.ContactID = OnBoardData.ContactId ?? 0;
                if (OnBoardData.TalentId != null && OnBoardData.TalentId != 0)
                    onBoardTalents_ClientFeedback.TalentName = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == OnBoardData.TalentId).Name ?? "";
                onBoardTalents_ClientFeedback.HiringRequest_ID = OnBoardData.HiringRequestId ?? 0;
                if (onBoardTalents_ClientFeedback.HiringRequest_ID != 0)
                    onBoardTalents_ClientFeedback.HRNumber = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(x => x.Id == onBoardTalents_ClientFeedback.HiringRequest_ID).HrNumber ?? "";
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Feedback", Details = onBoardTalents_ClientFeedback });
        }
        
        [HttpGet("GetDashboardCountForEngagement")]
        public async Task<ObjectResult> GetDashboardCountForEngagement()
        {
            try
            {
                var dashboardCount = _iEngagement.GetDashboardCount();
                if (dashboardCount != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Count", Details = dashboardCount });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data found" });

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        [HttpGet("GetOnBordFeedBack")]
        public async Task<ObjectResult> GetOnBordFeedBack(int totalrecord, int pagenumber, long onBoardId)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                string Sortdatafield = "FeedbackCreatedDateTime";
                string Sortorder = "desc";
                #region PreValidation
                if (totalrecord == null || totalrecord == 0 || pagenumber == null || onBoardId == null || onBoardId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Totalrecord,Pagenumber and OnBoardId" });

                }
                #endregion

                object[] param = new object[] { pagenumber, totalrecord, Sortdatafield, Sortorder, onBoardId };
                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_OnBoardClientFeedBack_Result> onBordFeedBacklist = await _iEngagement.GetOnBoardFeedback(paramasString);
                if (onBordFeedBacklist.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(onBordFeedBacklist, onBordFeedBacklist[0].TotalRecords, totalrecord, pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        [HttpGet("GetRenewEngagement")]
        public async Task<ObjectResult> GetRenewEngagement(long onBoardId)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                if (onBoardId == null || onBoardId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide OnBoardId" });
                }
                RenewalDetailsModel viewmodel = new RenewalDetailsModel();
                object[] param = new object[] { onBoardId };
                string paramasString = CommonLogic.ConvertToParamString(param);
                var result = _iEngagement.Get_Renewal_Details_Result(paramasString);
                if (result != null)
                {
                    viewmodel.OnBoardId = onBoardId;
                    viewmodel.ContractStartDate = Convert.ToDateTime(result.ContractStartDate);
                    viewmodel.ContractEndDate = Convert.ToDateTime(result.ContractEndDate).AddDays(1);
                    viewmodel.BillRate = result.BR;
                    viewmodel.PayRate = result.PR;
                    viewmodel.EngagementId = result.EngagemenId;
                    viewmodel.TalentName = result.TalentName;
                    viewmodel.Company = result.Company;
                    viewmodel.ContactName = result.ContactName;
                    viewmodel.Currency = result.Currency;
                    viewmodel.NRPercentage = result.NRPercentage;
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = viewmodel });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        
        [HttpPost("SaveRenewEngagement")]
        public async Task<ObjectResult> SaveRenewEngagement([FromBody] RenewalDetailsModel renewalDetailsModel)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                #region PreValidation
                if (renewalDetailsModel == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                #endregion

                #region Validation
                RenewalDetailsModelValidator validationRules = new RenewalDetailsModelValidator();
                ValidationResult validationResult = validationRules.Validate(renewalDetailsModel);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "Renewal Engagement error") });
                }
                #endregion

                object[] param = new object[]
                {
                      renewalDetailsModel.OnBoardId, renewalDetailsModel.ContractStartDate, renewalDetailsModel.ContractEndDate, renewalDetailsModel.ContarctDuration, renewalDetailsModel.BillRate, renewalDetailsModel.PayRate, renewalDetailsModel.NRPercentage, renewalDetailsModel.ReasonForBRPRChange,LoggedInUserId,0,renewalDetailsModel.IsContractOnGoing
                };
                string paramasString = CommonLogic.ConvertToParamString(param);

                var result = _iEngagement.Insert_Contract_Renewal_Details(paramasString);
                //_universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Add_Contract_Renewal_Details, param);
                if (result.NEWOnBoardID != 0)
                {
                    GenOnBoardTalent _OnBoardTalents = _talentConnectAdminDBContext.GenOnBoardTalents.FirstOrDefault(x => x.Id == renewalDetailsModel.OnBoardId);
                    if (_OnBoardTalents != null)
                    {
                        long ContactID = _OnBoardTalents.ContactId ?? 0;
                        long HRID = _OnBoardTalents.HiringRequestId ?? 0;
                        long TalentID = _OnBoardTalents.TalentId ?? 0;

                        GenSalesHiringRequest salesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HRID).FirstOrDefault();

                        param = new object[] { salesHiringRequest.SalesUserId };
                        string paramString = CommonLogic.ConvertToParamString(param);

                        List<Sproc_Get_Hierarchy_For_Email_Result> sproc_Get_Hierarchy_For_Email_SalesUser = null;

                        sproc_Get_Hierarchy_For_Email_SalesUser = await _iEngagement.GetHierarchyForEmail(paramString);

                        #region SendEmailForRenewal
                        EmailBinder binder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                        binder.SendEmailForRenewal(ContactID, TalentID, HRID, renewalDetailsModel.OnBoardId, renewalDetailsModel.ContractStartDate, renewalDetailsModel.ContractEndDate, sproc_Get_Hierarchy_For_Email_SalesUser);
                        #endregion

                        #region Insert HR history
                        param = new object[]
                        {
                         Action_Of_History.Contract_Renewal, HRID, TalentID, false, LoggedInUserId, 0, 0, "", renewalDetailsModel.OnBoardId, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                        #endregion

                        #region ATS Call

                        if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                        {
                            var varOnBoardId = new long[] { renewalDetailsModel.OnBoardId, result.NEWOnBoardID };
                            foreach (var item in varOnBoardId)
                            {
                                Sproc_talent_engagement_Details_For_PHP_API_Result ObjResult = new Sproc_talent_engagement_Details_For_PHP_API_Result();

                                if (item == renewalDetailsModel.OnBoardId)
                                    ObjResult = await _iEngagement.TalentEngagementDetails(item, "Contract Ended");
                                else
                                    ObjResult = await _iEngagement.TalentEngagementDetails(item, "Renewal");

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
                                    ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                                    aTSCall.SendTalentEngagementDetails(json, LoggedInUserId, HRID);
                                }
                            }
                        }
                        #endregion
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Renewal engagement Details Saved!" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("UploadFile")]
        public async Task<ObjectResult> UploadFile([FromForm] IFormFile file)
        {
            try
            {
                #region Validation
                if (file == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File Required", Details = null });
                }
                else if (file.Length == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "You are uploading corrupt file", Details = null });
                }

                var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                string[] allowedFileExtension = { ".pdf", ".doc", ".docx", ".txt", ".jpg", ".jpeg", ".png" };

                if (!allowedFileExtension.Contains(fileExtension))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Your file format is incorrect.", Details = null });
                }

                var fileSize = (file.Length / 1024) / 1024;
                if (fileSize >= 0.5)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File size must be less than 500 KB", Details = null });
                }
                #endregion

                long loggedInUserID = SessionValues.LoginUserId;
                string DocfileName = file.FileName;

                string filePath = System.IO.Path.Combine("Media/ClientFeedbackDocument", DocfileName);
                string FileExtension = System.IO.Path.GetExtension(filePath);

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                dynamic objResult = new ExpandoObject();

                objResult.FileName = DocfileName;

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "File Upload Successfully",
                    Details = objResult
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Cancel Engagement
        
        [HttpGet("GetCancelEndEnagagement")]
        public async Task<ObjectResult> GetCancelEndEnagagement(long OnBoardID)
        {
            #region PreValidation
            if (OnBoardID == null || OnBoardID == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide HR Id and OnBoardID" });
            }
            #endregion

            CancelEngagementViewModel cancelEngagementViewModel = new CancelEngagementViewModel();
            var objOnBoardClientContractDetails = _talentConnectAdminDBContext.GenOnBoardClientContractDetails.FirstOrDefault(x => x.OnBoardId == OnBoardID);
            var objOnBoardDetails = _talentConnectAdminDBContext.GenOnBoardTalents.FirstOrDefault(x => x.Id == OnBoardID);

            if (objOnBoardClientContractDetails != null)
            {
                DateTime LastWorkingDate, ClosureDate;
                if (objOnBoardClientContractDetails.LastWorkingDate != null)
                {
                    LastWorkingDate = Convert.ToDateTime(objOnBoardClientContractDetails.LastWorkingDate);
                    if (objOnBoardDetails != null)
                    {
                        ClosureDate = Convert.ToDateTime(objOnBoardDetails.ClientClosureDate);
                        cancelEngagementViewModel.ClosureDate = ClosureDate.ToString("dd/MM/yyyy");
                    }

                    cancelEngagementViewModel.LastWorkingDate = LastWorkingDate.ToString("dd/MM/yyyy");

                }

                cancelEngagementViewModel.ContractDetailID = Convert.ToInt32(objOnBoardClientContractDetails.Id);


                object[] objParam = new object[] { OnBoardID };
                string strParamas = CommonLogic.ConvertToParamString(objParam);
                var varTalent_RejectReason = await _iTalentReplacement.Sproc_Get_OnBoardDetailFor_C2H(strParamas);
                if (varTalent_RejectReason != null)
                {
                    cancelEngagementViewModel.DPNRPercentage = varTalent_RejectReason.DPNRPercentage ?? 0;
                    cancelEngagementViewModel.IsConvertToHireApplicable = varTalent_RejectReason.IsConvertToHireApplicable == 1 ? true : false;
                    cancelEngagementViewModel.Currency = varTalent_RejectReason.Currency;
                }

            }
            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Content Date", Details = cancelEngagementViewModel });
        }

        
        [HttpPost("CancelEngagement")]
        public async Task<ObjectResult> CancelEngagement(UpdateCancelEngagementViewModel updateCancelEngagementViewModel)
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                long C2HOnBoardID = 0;
                string message = "Engagement Cancelled !";
                #region PreValidation
                if (updateCancelEngagementViewModel == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                #endregion

                if (updateCancelEngagementViewModel.Reason.Contains("'"))
                {
                    updateCancelEngagementViewModel.Reason = updateCancelEngagementViewModel.Reason.Replace("'", "''");
                }

                var varOnBoardClientContractDetails = _talentConnectAdminDBContext.GenOnBoardClientContractDetails.AsNoTracking().FirstOrDefault(x => x.Id == updateCancelEngagementViewModel.ContractDetailID);
                long OnBoardId = varOnBoardClientContractDetails.OnBoardId ?? 0;
                var varOnBoard = _talentConnectAdminDBContext.GenOnBoardTalents.AsNoTracking().FirstOrDefault(x => x.Id == OnBoardId);
                if (varOnBoard != null && varOnBoardClientContractDetails != null)
                {
                    string LastWorkingDate = "";
                    if (updateCancelEngagementViewModel.LastWorkingDate != null)
                    {
                        LastWorkingDate = updateCancelEngagementViewModel.LastWorkingDate.Value.ToString("MM/dd/yyyy");
                    }


                    object[] param2 = new object[]
                          {
                            updateCancelEngagementViewModel.ContractDetailID, LastWorkingDate, updateCancelEngagementViewModel.Reason, LoggedInUserId, updateCancelEngagementViewModel.LostReasonID,updateCancelEngagementViewModel.EngcancelType
                          };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Update_CancelEngagementDetails, param2);

                    #region ATS Call
                    if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                    {
                        Sproc_talent_engagement_Details_For_PHP_API_Result ObjResult = await _iEngagement.TalentEngagementDetails(varOnBoardClientContractDetails.OnBoardId, "CancelEngagement");
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
                            ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                            aTSCall.SendTalentEngagementDetails(json, LoggedInUserId, varOnBoard.HiringRequestId);


                            #region Update HR Status
                            object[] param = new object[] { ObjResult.HiringRequest_ID, 0, 0, LoggedInUserId, (short)AppActionDoneBy.UTS, false };
                            string strParam = CommonLogic.ConvertToParamString(param);
                            var HRStatus_Json = _iEngagement.GetUpdateHrStatus(strParam);
                            if (HRStatus_Json != null)
                            {
                                //string JsonData = Convert.ToString(HRStatus_Json);
                                var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                if (!string.IsNullOrEmpty(JsonData))
                                {
                                    aTSCall.SendHrStatusToATS(JsonData, LoggedInUserId, ObjResult.HiringRequest_ID);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion



                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = message });

                }



                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data found" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region Private Methods
        
        private IActionResult ExportEngagementDetails(List<Sproc_Get_BusinessDashboard_Result> DetailList)
        {
            var ExportFileName = "EngagementReport_" + DateTime.Now.ToString("yyyyMMdd") + @".xlsx";
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("EngagementReport");

            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "LastFeedbackDate";
            worksheet.Cell(currentRow, 2).Value = "EngagementCount";
            worksheet.Cell(currentRow, 3).Value = "EngagementId_HRID";
            worksheet.Cell(currentRow, 4).Value = "Company";
            worksheet.Cell(currentRow, 5).Value = "ClientName";
            worksheet.Cell(currentRow, 6).Value = "TalentName";
            worksheet.Cell(currentRow, 7).Value = "CurrentStatus";
            worksheet.Cell(currentRow, 8).Value = "DeployedSource";
            worksheet.Cell(currentRow, 9).Value = "TSCName";
            worksheet.Cell(currentRow, 10).Value = "Geo";
            worksheet.Cell(currentRow, 11).Value = "Position";
            worksheet.Cell(currentRow, 12).Value = "IsLost";
            worksheet.Cell(currentRow, 13).Value = "OldTalent";
            worksheet.Cell(currentRow, 14).Value = "ReplacementEng";
            worksheet.Cell(currentRow, 15).Value = "NoticePeriod";
            worksheet.Cell(currentRow, 16).Value = "KickOff";
            worksheet.Cell(currentRow, 17).Value = "BillRate";
            worksheet.Cell(currentRow, 18).Value = "ActualBillRate";
            worksheet.Cell(currentRow, 19).Value = "PayRate";
            worksheet.Cell(currentRow, 20).Value = "ActualPayRate";
            worksheet.Cell(currentRow, 21).Value = "ContractStartDate";
            worksheet.Cell(currentRow, 22).Value = "ContractEndDate";
            worksheet.Cell(currentRow, 23).Value = "ActualEndDate";
            worksheet.Cell(currentRow, 24).Value = "NR";
            worksheet.Cell(currentRow, 25).Value = "ActualNR";
            worksheet.Cell(currentRow, 26).Value = "DP_Percentage";
            worksheet.Cell(currentRow, 27).Value = "RenewalstartDate";
            worksheet.Cell(currentRow, 28).Value = "RenewalendDate";
            worksheet.Cell(currentRow, 29).Value = "EngagementTenure";
            worksheet.Cell(currentRow, 30).Value = "SOWSignedDate";
            worksheet.Cell(currentRow, 31).Value = "NBDName";
            worksheet.Cell(currentRow, 32).Value = "AMName";
            worksheet.Cell(currentRow, 33).Value = "InvoiceSentDate";
            worksheet.Cell(currentRow, 34).Value = "InvoiceNumber";
            worksheet.Cell(currentRow, 35).Value = "InvoiceStatus";
            worksheet.Cell(currentRow, 36).Value = "DateofPayment";
            worksheet.Cell(currentRow, 37).Value = "CreatedByDatetime";
            worksheet.Cell(currentRow, 38).Value = "TypeOfHR";
            worksheet.Cell(currentRow, 39).Value = "Availability";
            worksheet.Cell(currentRow, 40).Value = "EngagementStatus";
            worksheet.Cell(currentRow, 41).Value = "payout_BillRate";
            worksheet.Cell(currentRow, 42).Value = "payout_PayRate";

            foreach (var details in DetailList)
            {
                currentRow++;
                var currentColumn = 1;

                worksheet.Cell(currentRow, currentColumn++).Value = details.LastFeedbackDate;
                worksheet.Cell(currentRow, currentColumn++).Value = details.EngagementCount;
                worksheet.Cell(currentRow, currentColumn++).Value = details.EngagementId_HRID;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Company;
                worksheet.Cell(currentRow, currentColumn++).Value = details.ClientName;
                worksheet.Cell(currentRow, currentColumn++).Value = details.TalentName;
                worksheet.Cell(currentRow, currentColumn++).Value = details.CurrentStatus;
                worksheet.Cell(currentRow, currentColumn++).Value = details.DeployedSource;
                worksheet.Cell(currentRow, currentColumn++).Value = details.TSCName;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Geo;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Position;
                worksheet.Cell(currentRow, currentColumn++).Value = details.IsLost;
                worksheet.Cell(currentRow, currentColumn++).Value = details.OldTalent;
                worksheet.Cell(currentRow, currentColumn++).Value = details.ReplacementEng;
                worksheet.Cell(currentRow, currentColumn++).Value = details.NoticePeriod;
                worksheet.Cell(currentRow, currentColumn++).Value = details.KickOff;
                worksheet.Cell(currentRow, currentColumn++).Value = details.BillRate + " " + details.BillRateCurrency;
                worksheet.Cell(currentRow, currentColumn++).Value = details.ActualBillRate + " " + details.BillRateCurrency;
                worksheet.Cell(currentRow, currentColumn++).Value = details.PayRate + " " + details.PayRateCurrency;
                worksheet.Cell(currentRow, currentColumn++).Value = details.ActualPayRate + " " + details.PayRateCurrency; ;
                worksheet.Cell(currentRow, currentColumn++).Value = details.ContractStartDate;
                worksheet.Cell(currentRow, currentColumn++).Value = details.ContractEndDate;
                worksheet.Cell(currentRow, currentColumn++).Value = details.ActualEndDate;
                worksheet.Cell(currentRow, currentColumn++).Value = details.NR;
                worksheet.Cell(currentRow, currentColumn++).Value = details.ActualNR;
                worksheet.Cell(currentRow, currentColumn++).Value = details.DP_Percentage;
                worksheet.Cell(currentRow, currentColumn++).Value = details.RenewalstartDate;
                worksheet.Cell(currentRow, currentColumn++).Value = details.RenewalendDate;
                worksheet.Cell(currentRow, currentColumn++).Value = details.EngagementTenure;
                worksheet.Cell(currentRow, currentColumn++).Value = details.SOWSignedDate;
                worksheet.Cell(currentRow, currentColumn++).Value = details.NBDName;
                worksheet.Cell(currentRow, currentColumn++).Value = details.AMName;
                worksheet.Cell(currentRow, currentColumn++).Value = details.InvoiceSentDate;
                worksheet.Cell(currentRow, currentColumn++).Value = details.InvoiceNumber;
                worksheet.Cell(currentRow, currentColumn++).Value = details.InvoiceStatus;
                worksheet.Cell(currentRow, currentColumn++).Value = details.DateofPayment;
                worksheet.Cell(currentRow, currentColumn++).Value = details.CreatedByDatetime;
                worksheet.Cell(currentRow, currentColumn++).Value = details.TypeOfHR;
                worksheet.Cell(currentRow, currentColumn++).Value = details.H_Availability;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Status_ID != 2 ? "Active" : "In Active";
                worksheet.Cell(currentRow, currentColumn++).Value = details.Payout_BillRate;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Payout_PayRate;
            }
            worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExportFileName);
        }
        #endregion
    }
}
