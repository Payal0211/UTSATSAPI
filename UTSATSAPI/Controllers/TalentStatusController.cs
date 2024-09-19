using Microsoft.AspNetCore.Mvc;
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
using UTSATSAPI.Repositories.Interfaces;
using static UTSATSAPI.Helpers.Enum;

namespace UTSATSAPI.Controllers
{
    [Authorize]
    [Route("TalentStatus/", Name = "TalentStatus")]
    [ApiController]
    public class TalentStatusController : ControllerBase
    {
        #region Variables
        private readonly ICommonInterface _commonInterface;
        private TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors
        public TalentStatusController(ICommonInterface commonInterface, TalentConnectAdminDBContext talentConnectAdminDBContext, IUniversalProcRunner universalProcRunner, IConfiguration configuration)
        {
            _commonInterface = commonInterface;
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
            _universalProcRunner = universalProcRunner;
            _configuration = configuration;
        }
        #endregion

        #region Public API

        [HttpGet("GetStatusDetail")]
        public ObjectResult GetTalentStatusDetail(long HrID, long TalentID, int TalentStatusID = 0, string TalentStatus = "")
        {
            try
            {
                if (HrID == 0 || TalentID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HrId or TalentId is in valid." });
                }
                dynamic dObject = new ExpandoObject();

                if (TalentStatusID != 0)
                {
                    dObject.TalentStatusAfterClientSelections = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections
                        .Where(x => x.IsActive == true && x.Id > 4 && x.IsDisplay == true && x.Id != TalentStatusID)
                        .Select(x => new MastersResponseModel
                        {
                            Value = x.TalentStatus,
                            Id = x.Id
                        }).ToList();
                }
                else
                {
                    dObject.TalentStatusAfterClientSelections = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections
                       .Where(x => x.IsActive == true && x.Id > 4 && x.IsDisplay == true && x.TalentStatus != TalentStatus)
                       .Select(x => new MastersResponseModel
                       {
                           Value = x.TalentStatus,
                           Id = x.Id
                       }).ToList();
                }

                dObject.ContactTalentPriorityID = 0;

                if (HrID != 0 && TalentID != 0)
                {
                    GenContactTalentPriority genContactTalentPriority = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == HrID && x.TalentId == TalentID).FirstOrDefault();
                    if (genContactTalentPriority != null)
                    {
                        dObject.ContactTalentPriorityID = genContactTalentPriority.Id;
                    }
                }

                //Get Reject Reason
                dObject.TalentRejectReason = _commonInterface.TalentStatus.CreditBased_PrgTalentRejectReason();
                
                dObject.TalentStatus = string.Empty;
                if (!string.IsNullOrEmpty(TalentStatus))
                    dObject.TalentStatus = TalentStatus;
                else
                {
                    PrgTalentStatusAfterClientSelection prgTalentStatusAfterClientSelection = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == TalentStatusID).FirstOrDefault();
                    if (prgTalentStatusAfterClientSelection != null)
                        dObject.TalentStatus = prgTalentStatusAfterClientSelection.TalentStatus;
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("UpdateTalentStatus")]
        public ObjectResult UpdateTalentStatus(UpdateTalentStatusModel updateTalentStatusModel)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;

                var varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(SessionValues.LoginUserId);

                #region Prevalidation

                GenSalesHiringRequestDetail _SalesHiringRequestDetail = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(x => x.Id == updateTalentStatusModel.HRDetailID).FirstOrDefault();
                if (_SalesHiringRequestDetail == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR detail does not exists." });

                if (updateTalentStatusModel.TalentStatusID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Select Talent Status." });
                }

                if (updateTalentStatusModel.TalentStatusID == (short)Talent_Status.Rejected)
                {
                    if (updateTalentStatusModel.RejectReasonID == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Select Reject Reason." });
                    }
                    if (string.IsNullOrEmpty(updateTalentStatusModel.Remark))
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Loss Remark." });
                    }
                    if (updateTalentStatusModel.RejectReasonID == -1 && string.IsNullOrEmpty(updateTalentStatusModel.OtherReason))
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Other Reason." });
                    }
                }
                if (updateTalentStatusModel.TalentStatusID == (short)Talent_Status.On_Hold)
                {
                    if (updateTalentStatusModel.OnHoldReasonID == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Select Onhold Reason." });
                    }
                    if (string.IsNullOrEmpty(updateTalentStatusModel.Remark))
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter onHold Remark." });
                    }
                    if (updateTalentStatusModel.OnHoldReasonID == -1 && string.IsNullOrEmpty(updateTalentStatusModel.OtherReason))
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Other Reason." });
                    }
                }
                if (updateTalentStatusModel.TalentStatusID == (short)Talent_Status.Cancelled)
                {
                    if (updateTalentStatusModel.CancelReasonID == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Select Cancel Reason." });
                    }
                    if (updateTalentStatusModel.CancelReasonID == -1 && string.IsNullOrEmpty(updateTalentStatusModel.OtherReason))
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Other Reason." });
                    }
                }
                #endregion

                #region Update talent status

                object[] param = new object[]
                {
                    updateTalentStatusModel.HRDetailID, updateTalentStatusModel.TalentStatusID, updateTalentStatusModel.TalentID, 0
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                _commonInterface.TalentStatus.UpdateTalentStatus(paramasString);

                #endregion

                #region Rejected
                if (updateTalentStatusModel.TalentStatusID == (short)Talent_Status.Rejected)
                {
                    param = new object[]
                    {
                        updateTalentStatusModel.HRDetailID, 
                        updateTalentStatusModel.TalentID, 
                        updateTalentStatusModel.RejectReasonID, 
                        updateTalentStatusModel.OtherReason, 
                        string.Empty , 
                        updateTalentStatusModel.Remark, 
                        LoggedInUserId, 
                        updateTalentStatusModel.HRID, 
                        (short)AppActionDoneBy.UTS
                    };
                    paramasString = CommonLogic.ConvertToParamString(param);
                    _commonInterface.TalentStatus.sproc_UTS_UpdateTalentRejectReason(paramasString);

                    #region Insert HR History 
                    param = new object[]
                    {
                            Action_Of_History.Talent_Status_Rejected, 
                        updateTalentStatusModel.HRID, 
                        updateTalentStatusModel.TalentID, 
                        false, 
                        LoggedInUserId, 
                        updateTalentStatusModel.ContactTalentPriorityID, 
                        0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                     };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                    #endregion

                    #region SendEmail
                    EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                    emailBinder.SendEmailToInternalTeamWhenTalentIsRejected(updateTalentStatusModel.TalentID, updateTalentStatusModel.RejectReasonID ?? 0, 
                                                                            updateTalentStatusModel.HRID, updateTalentStatusModel.Remark ?? "", 
                                                                            updateTalentStatusModel.OtherReason ?? "", varUsrUserById?.FullName ?? "");
                    #endregion
                }
                #endregion               

                #region Create model for passing json : Added by Divya for 3rd API       

                string? talentStatus = string.Empty;
                if (updateTalentStatusModel.TalentStatusID != 0)
                {
                    talentStatus = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == updateTalentStatusModel.TalentStatusID && x.IsActive == true).FirstOrDefault()?.TalentStatus;
                }

                GenTalent? _Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == updateTalentStatusModel.TalentID).FirstOrDefault();
                if (_Talent != null)
                {
                    #region Save Data in model to send reponse to PHP team after serialize     

                    if (_configuration["HRDataSendSwitchForPHP"].ToLower() != "local")
                    {
                        ContactTalentPriorityModel contactTalentPriorityModel = new ContactTalentPriorityModel();
                        var HiringRequestData = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == updateTalentStatusModel.HRID).FirstOrDefault();
                        if (HiringRequestData != null)
                        {
                            contactTalentPriorityModel.HRID = updateTalentStatusModel.HRID;
                            contactTalentPriorityModel.HRStatusID = HiringRequestData.StatusId ?? 0;

                            var HRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == contactTalentPriorityModel.HRStatusID).FirstOrDefault();
                            if (HRStatusData != null)
                                contactTalentPriorityModel.HRStatus = HRStatusData.HiringRequestStatus ?? string.Empty;

                            TalentDetail talentDetail = new TalentDetail();
                            talentDetail.ATS_TalentID = _Talent.AtsTalentId ?? 0;
                            talentDetail.TalentStatus = talentStatus ?? string.Empty;
                            talentDetail.UTS_TalentID = _Talent.Id;
                            talentDetail.Talent_USDCost = _Talent.FinalCost ?? 0;

                            object[] objParam = new object[] { contactTalentPriorityModel.HRID, _Talent.Id };
                            string strParamas = CommonLogic.ConvertToParamString(objParam);
                            var varTalent_RejectReason = _commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas);
                            if (varTalent_RejectReason != null)
                            {
                                talentDetail.Talent_RejectReason = varTalent_RejectReason.ActualReason;
                                talentDetail.RejectionComments = varTalent_RejectReason.RejectionComments;
                                talentDetail.RejectionMessageForTalent = varTalent_RejectReason.RejectionMessageToTalents;
                            }

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
                                object[] atsParam = new object[] { contactTalentPriorityModel.HRID, 0, _Talent.Id };
                                string paramString = CommonLogic.ConvertToParamString(atsParam);

                                ATSCall aTSCallforRound = new ATSCall(_configuration, _talentConnectAdminDBContext);

                                var roundDetails = aTSCallforRound.sproc_Get_InterviewRoundDetails(paramString);
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

                            contactTalentPriorityModel.TalentDetails.Add(talentDetail);

                            try
                            {
                                var json = JsonConvert.SerializeObject(contactTalentPriorityModel);
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                aTSCall.SaveContactTalentPriority(json, LoggedInUserId, updateTalentStatusModel.HRID);

                                #region HR Status updates to ATS 

                                //New Status Change
                                object[] ats_param = new object[] { HiringRequestData.Id, 0, 0, LoggedInUserId, (short)AppActionDoneBy.UTS, false };
                                string strParam = CommonLogic.ConvertToParamString(param);
                                var HRStatus_Json = _commonInterface.hiringRequest.GetUpdateHrStatus(strParam);
                                if (HRStatus_Json != null)
                                {
                                    //string JsonData = Convert.ToString(HRStatus_Json);
                                    var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                    if (!string.IsNullOrEmpty(JsonData))
                                    {
                                        aTSCall.SendHrStatusToATS(JsonData, LoggedInUserId, HiringRequestData.Id);
                                    }
                                }

                                #endregion
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
                            }
                        }
                    }

                    #endregion
                }

                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Unexpected error occurs" });
            }
        }

        [HttpGet("RemoveOnHoldStatus")]
        public async Task<ObjectResult> RemoveOnHold(long HrId, long ContactTalentPriorityID)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;

                var varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(SessionValues.LoginUserId);

                GenSalesHiringRequest _SalesHiringRequest = new();
                ContactTalentPriorityModel contactTalentPriorityResponseModel = new ContactTalentPriorityModel();

                #region PreValidation
                if (HrId == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = await _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HrId).FirstOrDefaultAsync().ConfigureAwait(false);
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }
                #endregion

                long? TalentId = 0;
                GenContactTalentPriority genContactTalentPriority = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == HrId && x.Id == ContactTalentPriorityID).FirstOrDefault();
                if (genContactTalentPriority != null)
                {
                    TalentId = genContactTalentPriority.TalentId;
                }

                object[] param = new object[] { HrId, ContactTalentPriorityID };
                string paramasString = CommonLogic.ConvertToParamString(param);
                _commonInterface.TalentStatus.RemoveOnHoldStatus(paramasString);

                #region Insert HR History 
                param = new object[]
                {
                     Action_Of_History.Update_On_Hold, HrId, TalentId, false, LoggedInUserId, ContactTalentPriorityID, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                 };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Remove on hold successfully" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Credit based status changes

        [HttpGet("CreditBased/GetStatusDetail")]
        public ObjectResult GetCreditHRTalentStatus(long HrID, long TalentID)
        {
            try
            {
                if (HrID == 0 || TalentID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HrId or TalentId is in valid." });
                }

                dynamic dObject = new ExpandoObject();
                dObject.TalentStatusIdClientPortal = 0;
                dObject.OtherRejectReason = "";
                dObject.ContactTalentPriorityID = 0;
                dObject.RejectReasonId = 0;

                GenContactTalentPriority genContactTalentPriority = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == HrID && x.TalentId == TalentID).FirstOrDefault();
                if (genContactTalentPriority != null)
                {
                    dObject.TalentStatusIdClientPortal = genContactTalentPriority.TalentStatusIdClientPortal;
                    dObject.OtherRejectReason = genContactTalentPriority.OtherRejectReason;
                    dObject.RejectReasonId = genContactTalentPriority.RejectReasonId;
                    dObject.ContactTalentPriorityID = genContactTalentPriority.Id;
                }

                dObject.CreditBased_PrgTalentStatus = _commonInterface.TalentStatus.CreditBased_GetPrgTalentStatus().Where(x => x.IsActive == true && x.ActionName != null).Select(x => new MastersResponseModel { Value = x.ActionName, text = x.ActionName, Id = x.Id }).ToList();

                dObject.CreditBased_RejectReason = _commonInterface.TalentStatus.CreditBased_PrgTalentRejectReason();
                //.Select(x => new MastersResponseModel { Value = x.Reason, text = x.Reason, Id = x.Id }).ToList();
                //dObject.CreditBased_RejectReason.Add(new MastersResponseModel { Id = -1, Value = "Other" });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Talent Status List", Details = dObject });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("CreditBased/UpdateTalentStatus")]
        public ObjectResult UpdateTalentStatus(UpdateTalentStatus updateTalentStatus)
        {

            var LoginUserID = SessionValues.LoginUserId;

            #region Pre-Validation 
            if (updateTalentStatus == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Request object is empty" });
            }
            if (updateTalentStatus.CtpId == null || updateTalentStatus.CtpId == 0 ||
                updateTalentStatus.HiringRequestId == null || updateTalentStatus.HiringRequestId == 0 ||
                updateTalentStatus.TalentId == null || updateTalentStatus.TalentId == 0 ||
                updateTalentStatus.StatusId == null || updateTalentStatus.StatusId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data is not proper" });
            }
            #endregion

            object[] param = new object[]
            {
                updateTalentStatus.CtpId,
                updateTalentStatus.TalentId,
                LoginUserID,
                updateTalentStatus.HiringRequestId,
                updateTalentStatus.StatusId,
                (short)AppActionDoneBy.UTS,
                LoginUserID,
                updateTalentStatus.ProfileRejectionStage ?? "", // Reject message to the talent
                updateTalentStatus.RejectReasonID ?? 0,
                updateTalentStatus.Remark // Additional remarks
            };

            string paramString = CommonLogic.ConvertToParamString(param);

            _commonInterface.TalentStatus.CreditBased_UpdateTalentStatus(paramString);
            var varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(SessionValues.LoginUserId);
            string RejectedBy = string.Empty;
            if (varUsrUserById != null)
            {
                RejectedBy = varUsrUserById.EmployeeId;
            }

            #region SendEmail
            EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
            emailBinder.SendEmailToInternalTeamWhenTalentIsRejected(updateTalentStatus.TalentId, updateTalentStatus.RejectReasonID ?? 0, 
                updateTalentStatus.HiringRequestId, updateTalentStatus.Remark, updateTalentStatus.ProfileRejectionStage ?? "", varUsrUserById?.FullName ?? "");
            #endregion

            try
            {
                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                aTSCall.CallAtsAPIToSendTalentAndHRStatus(updateTalentStatus.TalentId, SessionValues.LoginUserId, updateTalentStatus.HiringRequestId, RejectedBy);

                #region HR Status updates to ATS 

                //New Status Change
                object[] ats_param = new object[] { updateTalentStatus.HiringRequestId, 0, 0, LoginUserID, (short)AppActionDoneBy.UTS, false };
                string strParam = CommonLogic.ConvertToParamString(ats_param);
                var HRStatus_Json = _commonInterface.hiringRequest.GetUpdateHrStatus(strParam);
                if (HRStatus_Json != null)
                {
                    //string JsonData = Convert.ToString(HRStatus_Json);
                    var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                    if (!string.IsNullOrEmpty(JsonData))
                    {
                        aTSCall.SendHrStatusToATS(JsonData, LoginUserID, updateTalentStatus.HiringRequestId);
                    }
                }

                #endregion
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Action success" });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Action success" });
        }

        #endregion


        #region RejectionReasonMessageforTalent
        [HttpGet("GetRejectionReasonForTalent")]
        public ActionResult GetRejectionReasonForTalent(long hrId, long rejectionReasonID, long atsTalentID)
        {
            if (hrId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please pass HR ID" });
            }

            if (rejectionReasonID == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please pass Rejection ID" });
            }

            if (atsTalentID == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please pass Talent ID" });
            }

            object[] param = new object[] { hrId, rejectionReasonID, atsTalentID };
            string paramString = CommonLogic.ConvertToParamString(param);

            Sproc_Get_RejectionReasonForTalent_Result messageDetails = _commonInterface.TalentStatus.Sproc_Get_RejectionReasonForTalent(paramString);
            if (messageDetails != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = messageDetails });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "" });
            }
        }
        #endregion

        #endregion
    }
}