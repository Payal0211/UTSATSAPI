
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UTSATSAPI.ATSCalls;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Models.ViewModels.Request_ResponseModels;
using UTSATSAPI.Repositories.Interfaces;
using static UTSATSAPI.Helpers.Enum;

namespace UTSATSAPI.Controllers
{
    [Route("TalentReplacement/", Name = "TalentReplacement")]
    [ApiController]
    public class TalentReplacementController : ControllerBase
    {
        #region
        private readonly TalentConnectAdminDBContext db;
        private readonly IUniversalProcRunner universalProcRunner;
        private readonly IConfiguration iConfiguration;
        private readonly ITalentReplacement _iTalentReplacement;
        private readonly ICommonInterface _commonInterface;
        private readonly IHiringRequest _iHiringRequest;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEngagement _iEngagement;
        #endregion

        public TalentReplacementController(TalentConnectAdminDBContext _talentConnectAdminDBContext, IUniversalProcRunner _universalProcRunner, IConfiguration _iConfiguration,
            ITalentReplacement iTalentReplacement, ICommonInterface commonInterface, IHiringRequest iHiringRequest, IWebHostEnvironment webHostEnvironment, IEngagement iEngagement)
        {
            db = _talentConnectAdminDBContext;
            universalProcRunner = _universalProcRunner;
            iConfiguration = _iConfiguration;
            _iTalentReplacement = iTalentReplacement;
            _commonInterface = commonInterface;
            _iHiringRequest = iHiringRequest;
            _webHostEnvironment = webHostEnvironment;
            _iEngagement = iEngagement;
        }


        [Authorize]
        [HttpGet]
        [Route("ReplaceTalent")]
        public async Task<ObjectResult> ReplaceTalent(int ID)
        {
            if (ID == 0)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide OnBoard Id" });

            var TalentName = "";
            var ClientName = "";
            var Company = "";
            var ReplacementHandledName = "";
            long ReplacementHandledByID = 0;
            var AMNBD = "";

            var onBoardTalents = await _iTalentReplacement.GetGenOnBoardTalentById(ID).ConfigureAwait(false);

            DateTime CurrentDAte = System.DateTime.Now;

            var resonForReplacement = new Dictionary<string, string>
            {
                { "0", "--Select--" },
                { "Technical Skills of Talent", "Technical Skills of Talent" },
                { "Functional Skills of Talent", "Functional Skills of Talent" },
                { "Talent Resigned", "Talent Resigned" },
                { "Talent Backout", "Talent Backout" },
                { "Behavioral","Behavioral"}
            };
            var ReasonforReplacement = resonForReplacement.ToList().Select(x => new SelectListItem
            {
                Value = x.Key.ToString(),
                Text = x.Value
            }); ;

            if (onBoardTalents != null)
            {

                var talent = await _iTalentReplacement.GteGenTalentById(onBoardTalents.TalentId).ConfigureAwait(false);
                if (talent != null)
                {
                    TalentName = talent.Name;
                }
                var contact = await _iTalentReplacement.GteGenContactById(onBoardTalents.ContactId).ConfigureAwait(false);
                if (contact != null)
                {
                    ClientName = contact.FullName;

                    var obj_company = await _iTalentReplacement.GteGenCompanyById(contact.CompanyId).ConfigureAwait(false);
                    if (obj_company != null)
                    {
                        Company = obj_company.Company;
                    }

                }
                var EngagementId = onBoardTalents.EngagemenId;

                var AMNBDData = await _iTalentReplacement.GetAMNBDForReplacement(ID).ConfigureAwait(false);
                if (AMNBDData.Count > 0)
                {
                    ReplacementHandledName = AMNBDData.First().Fullname;
                    ReplacementHandledByID = AMNBDData.First().ID;
                    var objUser = await _iTalentReplacement.GetUsrUserById(AMNBDData.First().ID).ConfigureAwait(false);
                    if (objUser != null)
                    {
                        if ((objUser.UserTypeId == 4 || objUser.UserTypeId == 9) && objUser.IsNewUser == true)
                            AMNBD = "NBD";
                        else if ((objUser.UserTypeId == 4 || objUser.UserTypeId == 9) && objUser.IsNewUser == false)
                            AMNBD = "AM";
                    }
                }
                var ResponseModel = new
                {
                    onBoardTalents = onBoardTalents,
                    CurrentDate = CurrentDAte,
                    ReasonforReplacement = ReasonforReplacement,
                    TalentName = TalentName,
                    ClientName = ClientName,
                    Company = Company,
                    EngagementId = EngagementId,
                    ReplacementHandledName = ReplacementHandledName,
                    ReplacementHandledByID = ReplacementHandledByID,
                    AMNBD = AMNBD
                };

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ResponseModel });

            }
            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data not found" });

        }

        [Authorize]
        [HttpPost]
        [Route("SaveReplaceTalent")]
        public async Task<ObjectResult> SaveReplaceTalent(TalentReplacement talentReplacement)
        {
            try
            {
                var LoggedInUserID = SessionValues.LoginUserId;

                var varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(LoggedInUserID);

                if (talentReplacement == null || LoggedInUserID <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide data for save operation" });

                talentReplacement = await _iTalentReplacement.SaveTalentReplacementData(talentReplacement, universalProcRunner).ConfigureAwait(false);

                talentReplacement.HiringRequestID = talentReplacement.HiringRequestID;
                int TalentStatusID = (short)prg_TalentStatus_AfterClientSelection.InReplacement;

                #region ATS call

                if (!iConfiguration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    ContactTalentPriorityResponseModel contactTalentPriorityResponseModel = new ContactTalentPriorityResponseModel();
                    ATSCall aTSCall = new ATSCall(iConfiguration, db);
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
                            if (TalentStatusID > 0)
                            {
                                TalentStatus = db.PrgTalentStatusAfterClientSelections.Where(x => x.Id == TalentStatusID && x.IsActive == true).FirstOrDefault()?.TalentStatus;
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

                                ATSCall aTSCallforRound = new ATSCall(iConfiguration, db);

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
                                aTSCall.SaveContactTalentPriority(json, LoggedInUserID, talentReplacement.HiringRequestID.Value);
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data save successfully" });
                            }
                        }

                        #region ATS Call
                        if (talentReplacement.OnboardId != null)
                        {
                            if (!iConfiguration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                            {

                                Sproc_talent_engagement_Details_For_PHP_API_Result result = await _iEngagement.TalentEngagementDetails(talentReplacement.OnboardId,"Talent Replacement");
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
                                    aTSCall.SendTalentEngagementDetails(json, LoggedInUserID, result.HiringRequest_ID);
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data save successfully" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data not found" });
                throw;
            }

        }
        [Authorize]
        [HttpPost]
        [Route("CreateReplaceHR")]
        public async Task<ObjectResult> CreateReplaceHR(long HrID, long OnBoardID)
        {
            try
            {
                var LoggedInUserID = SessionValues.LoginUserId;

                if (HrID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide OnBoard Id" });
                }
                else
                {
                    #region CloneHR
                    object[] paramClone = new object[] { HrID, LoggedInUserID, OnBoardID, false, false, false, false, false, false };
                    sproc_CloneHRFromExistHR_Result obj_sproc_CloneHRFromExistHR_Result = new();
                    obj_sproc_CloneHRFromExistHR_Result = await _commonInterface.hiringRequest.sproc_CloneHRFromExistHR(CommonLogic.ConvertToParamString(paramClone));
                    #endregion

                    #region
                    //deberif 
                    object[] paramDeberif = new object[]
                        {
                            Action_Of_History.Update_HR, obj_sproc_CloneHRFromExistHR_Result.CloneHRID, 0, false, LoggedInUserID, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                    universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, paramDeberif);
                    #endregion

                    #region HRAccept

                    object[] paramAcceptHR = new object[] { obj_sproc_CloneHRFromExistHR_Result.CloneHRID, 1, LoggedInUserID, "" };

                    universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Accept_HR, paramAcceptHR);

                    var varHrDetails = db.GenSalesHiringRequests.Where(x => x.Id == obj_sproc_CloneHRFromExistHR_Result.CloneHRID).FirstOrDefault();

                    var varOldHrDetails = db.GenSalesHiringRequests.Where(x => x.Id == HrID).FirstOrDefault();

                    var NoofTalents = varOldHrDetails.NoofTalents - 1 ;

                    #region update TR in oldHR
                    object[] param = new object[] { varOldHrDetails.Id, LoggedInUserID, NoofTalents, "Talent-replacement", "Talent-replacement", (int)AppActionDoneBy.UTS };
                    string paramasString = CommonLogic.ConvertToParamString(param);
                    universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_UpdateTR, param);
                    #endregion


                    string EmailMsg = "";

                    EmailBinder emailBinder = new(iConfiguration, db);
                    EmailMsg = emailBinder.SendEmailForHRAcceptanceToInternalTeam(obj_sproc_CloneHRFromExistHR_Result.CloneHRID, 1, "");

                    if (1 == 1)
                    {
                        string EmailForTRAcceptance = "";
                        EmailForTRAcceptance = emailBinder.SendEmailForTRAcceptanceToInternalTeam(_webHostEnvironment.WebRootPath, obj_sproc_CloneHRFromExistHR_Result.CloneHRID, 1, 0);
                    }
                    #endregion
                    
                    if (EmailMsg.ToLower().Trim() == "success")
                    {
                            #region ATS Call to Send data
                            if (!iConfiguration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                            {
                                var HRData_Json = await _iHiringRequest.GetAllHRDataForAdmin(obj_sproc_CloneHRFromExistHR_Result.CloneHRID).ConfigureAwait(false);
                                string HRJsonData = Convert.ToString(HRData_Json);

                                if (!string.IsNullOrEmpty(HRJsonData))
                                {
                                    bool isAPIResponseSuccess = true;

                                    ATSCall aTSCall = new(iConfiguration, db);
                                    if (HRJsonData != "")
                                        isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRData_Json.ToString(), obj_sproc_CloneHRFromExistHR_Result.CloneHRID);
                                }
                            }
                        #endregion

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "successfully", Details = new { HR_Id = varHrDetails.Id, HR_Number = varHrDetails.HrNumber } });
                    }
                    
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetEngagemetnsForReplacementBasedOnLWDOption")]
        public async Task<ObjectResult> GetEngagemetnsForReplacementBasedOnLWDOption(long OnBoardId, int LastWorkingDayOption)
        {
            try
            {
                if (OnBoardId == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide OnBoard Id" });
                if (LastWorkingDayOption == null || LastWorkingDayOption == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide Last Working Day Option" });

                object[] param = new object[] { OnBoardId, LastWorkingDayOption };
                string paramasString = CommonLogic.ConvertToParamString(param);

                var EngagementList = await _iTalentReplacement.GetEngagemetnsForReplacementBasedOnLWDOption(paramasString).ConfigureAwait(false);
                if (EngagementList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = EngagementList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("ReasonForReplacements")]
        public async Task<ObjectResult> ReasonForReplacements()
        {

            var resonForReplacement = new Dictionary<string, string>
            {
                { "0", "--Select--" },
                { "Technical Skills of Talent", "Technical Skills of Talent" },
                { "Functional Skills of Talent", "Functional Skills of Talent" },
                { "Talent Resigned", "Talent Resigned" },
                { "Talent Backout", "Talent Backout" }
            };
            var ReasonforReplacement = resonForReplacement.ToList().Select(x => new SelectListItem
            {
                Value = x.Key.ToString(),
                Text = x.Value
            });

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ReasonforReplacement });

        }
    }
}
