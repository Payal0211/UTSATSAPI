using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;
using UTSATSAPI.ATSCalls;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Models.ViewModels.Request_ResponseModels;
using UTSATSAPI.Repositories.Interfaces;
using static UTSATSAPI.Helpers.Enum;

namespace UTSATSAPI.Controllers
{
    [Route("HRAcceptance/", Name = "HRAcceptance")]
    [ApiController]
    public class HRAcceptanceController : ControllerBase
    {
        #region Variables
        private readonly ICommonInterface commonInterface;
        private readonly TalentConnectAdminDBContext db;
        private readonly IUniversalProcRunner universalProcRunner;
        private readonly IConfiguration iConfiguration;
        private readonly IHRAcceptance _iHRAcceptance;
        #endregion

        public HRAcceptanceController(ICommonInterface _commonInterface, IConfiguration _iConfiguration, ILogger<DealListController> _logger, TalentConnectAdminDBContext _db, IUniversalProcRunner _universalProcRunner, IHRAcceptance iHRAcceptance)
        {
            commonInterface = _commonInterface;
            db = _db;
            universalProcRunner = _universalProcRunner;
            iConfiguration = _iConfiguration;
            _iHRAcceptance = iHRAcceptance;

        }

        [Authorize]
        [HttpGet]
        [Route("OpenPostAcceptance")]
        public ObjectResult OpenPostAcceptance(long HRDetailId, int TalentId)
        {
            try
            {
                if (HRDetailId == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide Hiring Request Details Id" });
                if (TalentId == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide TalentId" });

                var genShortlistedTalents = db.GenShortlistedTalents.Where(y => y.HiringRequestDetailId == HRDetailId).FirstOrDefault();
                if (genShortlistedTalents != null)
                {
                    var InterviewSlotObject = _iHRAcceptance.AddConfirmInterviewSlotResult(genShortlistedTalents.Id, genShortlistedTalents.HiringRequestId, genShortlistedTalents.HiringRequestDetailId, TalentId, genShortlistedTalents.ContactId, Convert.ToInt32(TalentId)).FirstOrDefault();

                    _iHRAcceptance.ShortlistedTalentsUpdate(HRDetailId);

                    var ResponseModel = new
                    {
                        PostId = InterviewSlotObject != null ? InterviewSlotObject.Id : 0,
                        TalentId = TalentId
                    };

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ResponseModel });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data not found" });
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        [Authorize]
        [HttpGet]
        [Route("GetHRAcceptance")]
        public ObjectResult GetHRAcceptance(long PostID, int TalentID)
        {
            try
            {
                if (PostID == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide PostId" });
                if (TalentID == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide TalentId" });

                PostAcceptanceViewModel postAcceptanceViewModel = new PostAcceptanceViewModel();
                postAcceptanceViewModel.TalentID = TalentID;
                postAcceptanceViewModel.postAcceptanceDetail = _iHRAcceptance.GetPostAcceptanceDetail(PostID);
                postAcceptanceViewModel.postAcceptanceDetailAvailability = _iHRAcceptance.GetPostAcceptanceDetailAvailability(PostID);
                postAcceptanceViewModel.postAcceptanceDetailHowSoon = _iHRAcceptance.GetPostAcceptanceDetailHowSoon(PostID);
                if (postAcceptanceViewModel.postAcceptanceDetail != null && postAcceptanceViewModel.postAcceptanceDetail.Count > 0 && postAcceptanceViewModel.postAcceptanceDetailAvailability != null && postAcceptanceViewModel.postAcceptanceDetailAvailability.Count > 0 && postAcceptanceViewModel.postAcceptanceDetailHowSoon != null && postAcceptanceViewModel.postAcceptanceDetailHowSoon.Count > 0)
                {
                    long ContactID = postAcceptanceViewModel.postAcceptanceDetail[0].ContactID;
                    long HiringRequestId = postAcceptanceViewModel.postAcceptanceDetail[0].HiringRequest_ID;

                    //Get Company and Send Notifiation details.
                    postAcceptanceViewModel.ContactID = ContactID;
                    GenContact gen_Contact = db.GenContacts.FirstOrDefault(x => x.Id == ContactID);
                    if (gen_Contact != null)
                    {
                        GenCompany company = db.GenCompanies.FirstOrDefault(x => x.Id == gen_Contact.CompanyId);
                        if (company != null)
                        {
                            postAcceptanceViewModel.Company = company.Company;
                        }
                        //postAcceptanceViewModel.IsClientNotificationSent = gen_Contact.IsClientNotificationSend;
                        postAcceptanceViewModel.IsClientNotificationSent = false;
                    }

                    GenSalesHiringRequestPriority genSalesHiringRequestPriority = db.GenSalesHiringRequestPriorities.Where(x => x.HiringRequestId == HiringRequestId).FirstOrDefault();
                    if (genSalesHiringRequestPriority != null)
                    {
                        postAcceptanceViewModel.IsPriority = genSalesHiringRequestPriority.IsPriority;
                        postAcceptanceViewModel.IsNextWeekPriority = genSalesHiringRequestPriority.IsNextWeekPriority;
                    }
                    postAcceptanceViewModel.Role = postAcceptanceViewModel.postAcceptanceDetail[0].TalentRole;

                    var Talentdetails = db.GenTalents.Join(db.PrgTalentStatuses, t => t.Status, S => S.Id, (p, s) => new { p, s }).Where(x => x.p.Id == TalentID)
                        .Select(y => new
                        {
                            TalentStatus = y.s.TalentStatus,
                            TalentName = y.p.Name
                        }).FirstOrDefault();

                    if (Talentdetails != null)
                    {
                        postAcceptanceViewModel.TalentStatus = Talentdetails.TalentStatus;
                        postAcceptanceViewModel.TalentName = Talentdetails.TalentName;
                    }

                    postAcceptanceViewModel.HR_Number = db.GenSalesHiringRequests.FirstOrDefault(x => x.Id == HiringRequestId).HrNumber;
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = postAcceptanceViewModel });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Talent info like Role/ Availability / Joining / Working Preferences are blank which are required for Acceptance Process." });

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("AddHRAcceptance")]
        public ObjectResult AddHRAcceptance(AddHRAcceptanceRequestModel addHRAcceptanceRequestModel)
        {
            try
            {
                if (addHRAcceptanceRequestModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide data for save operation" });
                int TalentID = Convert.ToInt32(addHRAcceptanceRequestModel.TalentId);
                long LoggedInUserID = SessionValues.LoginUserId;

                var varUsrUserById = commonInterface.TalentStatus.GetUsrUserById(LoggedInUserID);

                GenShortlistedTalent genShortlistedTalent = new GenShortlistedTalent();
                ContactTalentPriorityModel contactTalentPriorityModel = new ContactTalentPriorityModel();
                if (addHRAcceptanceRequestModel.HRAcceptanceDetailList != null && addHRAcceptanceRequestModel.HRAcceptanceDetailList.Count > 0)
                {
                    genShortlistedTalent = db.GenShortlistedTalents.Where(x => x.Id == addHRAcceptanceRequestModel.HRAcceptanceDetailList[0].ID).FirstOrDefault();
                    if (genShortlistedTalent != null)
                    {
                        if (addHRAcceptanceRequestModel.TotalCount > 0)
                        {

                            for (int i = 0; i < addHRAcceptanceRequestModel.TotalCount; i++)
                            {
                                if (addHRAcceptanceRequestModel.HRAcceptanceDetailList[i].PrimaryKey == addHRAcceptanceRequestModel.HRAcceptanceDetail)
                                {

                                    long PrimaryKey = addHRAcceptanceRequestModel.HRAcceptanceDetailList[i].PrimaryKey;
                                    string TableName = addHRAcceptanceRequestModel.HRAcceptanceDetailList[i].TableName;

                                    _iHRAcceptance.UpdateTalentConfirmHRAcceptance(genShortlistedTalent.HiringRequestDetailId, TalentID, genShortlistedTalent.ContactId, true, PrimaryKey, TableName);
                                }

                            }
                        }
                        if (addHRAcceptanceRequestModel.TotalCountAvailability > 0)
                        {
                            for (int i = 0; i < addHRAcceptanceRequestModel.TotalCountAvailability; i++)
                            {
                                if (addHRAcceptanceRequestModel.HRAcceptanceDetailAvailabilityList[i].PrimaryKey == addHRAcceptanceRequestModel.HRAcceptanceDetailAvailability)
                                {

                                    long PrimaryKey = addHRAcceptanceRequestModel.HRAcceptanceDetailAvailabilityList[i].PrimaryKey;
                                    string TableName = addHRAcceptanceRequestModel.HRAcceptanceDetailAvailabilityList[i].TableName;

                                    _iHRAcceptance.UpdateTalentConfirmHRAcceptance(genShortlistedTalent.HiringRequestDetailId, TalentID, genShortlistedTalent.ContactId, true, PrimaryKey, TableName);
                                }
                            }
                        }
                        if (addHRAcceptanceRequestModel.TotalCountHowSoon > 0)
                        {
                            for (int i = 0; i < addHRAcceptanceRequestModel.TotalCountHowSoon; i++)
                            {
                                if (addHRAcceptanceRequestModel.HRAcceptanceDetailHowSoonList[i].PrimaryKey == addHRAcceptanceRequestModel.HRAcceptanceDetailHowSoon)
                                {

                                    long PrimaryKey = addHRAcceptanceRequestModel.HRAcceptanceDetailHowSoonList[i].PrimaryKey;
                                    string TableName = addHRAcceptanceRequestModel.HRAcceptanceDetailHowSoonList[i].TableName;

                                    _iHRAcceptance.UpdateTalentConfirmHRAcceptance(genShortlistedTalent.HiringRequestDetailId, TalentID, genShortlistedTalent.ContactId, true, PrimaryKey, TableName);
                                }

                            }
                        }

                        int Role_Id = 0;
                        GenSalesHiringRequestDetail objsalesHiringRequest_Details = db.GenSalesHiringRequestDetails.Where(x => x.HiringRequestId == genShortlistedTalent.HiringRequestId).FirstOrDefault();
                        if (objsalesHiringRequest_Details != null)
                        {
                            Role_Id = objsalesHiringRequest_Details.RoleId ?? 0;
                        }

                        object[] parameters = new object[] { Action_Of_History.HRPost_Acceptance, genShortlistedTalent.HiringRequestId.Value, genShortlistedTalent.TalentId.Value, false, LoggedInUserID, 0, 0, DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"), 0, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                        universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, parameters);

                        #region SendEmailForTalentAcceptedHR
                        EmailBinder binder = new(iConfiguration, db);

                        if (genShortlistedTalent.HiringRequestId != null)
                        {
                            GenSalesHiringRequest _SalesHiringRequest = db.GenSalesHiringRequests.Where(x => x.Id == genShortlistedTalent.HiringRequestId).FirstOrDefault();
                            if (_SalesHiringRequest != null)
                            {
                                if (_SalesHiringRequest.IsHrtypeDp == true)
                                {
                                    #region SendEmailtoSalesDirectPlacement
                                    binder.SendEmailtoSalesDirectPlacement(TalentID, (long)genShortlistedTalent.HiringRequestId);
                                    #endregion
                                }
                                else
                                {
                                    #region SendEmailToSalesContractualHR
                                    binder.SendEmailForTalentAcceptedHR(TalentID, (long)genShortlistedTalent.HiringRequestId);
                                    #endregion
                                }
                            }
                        }
                        #endregion

                        var Contact_Id = genShortlistedTalent.ContactId;
                        //GenContact _Contact = db.GenContacts.Where(x => x.Id == Contact_Id).FirstOrDefault();
                        //if (_Contact != null)
                        //{
                        //    if (_Contact.IsClientNotificationSend)
                        //    {
                        //        #region SendEmailForTalentShowtoclientbySalesTeam
                        //        binder.SendEmailForTalentShowtoclientbySalesTeam(TalentID, (long)genShortlistedTalent.HiringRequestId);
                        //        #endregion
                        //    }
                        //}

                        var HiringRequest_Id = genShortlistedTalent.HiringRequestId.Value;
                        var Talent_ID = genShortlistedTalent.TalentId.Value;
                        long ContactTalentPriorityID = 0;
                        GenContactTalentPriority _ContactTalentPriority = db.GenContactTalentPriorities.Where(x => x.HiringRequestId == HiringRequest_Id && x.TalentId == Talent_ID && x.TalentStatusIdBasedOnHr == 1).FirstOrDefault();
                        if (_ContactTalentPriority != null)
                        {
                            ContactTalentPriorityID = _ContactTalentPriority.Id;
                        }

                        object[] Objparameters = new object[] { Action_Of_History.Profile_Waiting_For_Feedback, HiringRequest_Id, Talent_ID, false, LoggedInUserID, ContactTalentPriorityID, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                        universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, Objparameters);

                        //if (iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                        //{
                        //    db.Entry(genShortlistedTalent).Reload();

                        //    GenTalent _Talent = db.GenTalents.Where(x => x.Id == Talent_ID).FirstOrDefault();
                        //    db.Entry(_Talent).Reload();

                        //    if (_Talent != null)
                        //    {
                        //        var HiringRequestData = db.GenSalesHiringRequests.Where(x => x.Id == HiringRequest_Id).FirstOrDefault();
                        //        if (HiringRequestData != null)
                        //        {
                        //            #region Save Data in model to send reponse to PHP team after serialize
                        //            int TalentStatusID = (short)prg_TalentStatus_AfterClientSelection.ProfileShared;//Shortlisted(Profile Shared)
                        //            string? TalentStatus = string.Empty;
                        //            if (TalentStatusID > 0)
                        //            {
                        //                TalentStatus = db.PrgTalentStatusAfterClientSelections.Where(x => x.Id == TalentStatusID && x.IsActive == true).FirstOrDefault()?.TalentStatus;
                        //            }

                        //            contactTalentPriorityModel.HRID = HiringRequest_Id;
                        //            contactTalentPriorityModel.HRStatusID = HiringRequestData.StatusId ?? 0;

                        //            var HRStatusData = db.PrgHiringRequestStatuses.Where(x => x.Id == contactTalentPriorityModel.HRStatusID).FirstOrDefault();
                        //            if (HRStatusData != null)
                        //                contactTalentPriorityModel.HRStatus = HRStatusData.HiringRequestStatus;

                        //            object[] objParam = new object[] { contactTalentPriorityModel.HRID, _Talent.Id };
                        //            string strParamas = CommonLogic.ConvertToParamString(objParam);
                        //            var varTalent_RejectReason = commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas).ActualReason;

                        //            TalentDetail talentDetail = new()
                        //            {
                        //                ATS_TalentID = _Talent.AtsTalentId ?? 0,
                        //                TalentStatus = TalentStatus,
                        //                UTS_TalentID = _Talent.Id,
                        //                Talent_USDCost = _Talent.FinalCost ?? 0,
                        //                Talent_RejectReason= varTalent_RejectReason,
                        //                RejectedBy= varUsrUserById.EmployeeId
                        //            };

                        //            contactTalentPriorityModel.TalentDetails.Add(talentDetail);
                        //            #endregion
                        //            try
                        //            {
                        //                var json = JsonConvert.SerializeObject(contactTalentPriorityModel);
                        //                ATSCall aTSCall = new(iConfiguration, db);
                        //                aTSCall.SaveContactTalentPriority(json, LoggedInUserID, HiringRequest_Id);
                        //            }
                        //            catch (Exception)
                        //            {
                        //                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data save successfully" });
                        //            }
                        //        }
                        //    }
                        //}

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data save successfully" });
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data not found" });
        }
    }
}