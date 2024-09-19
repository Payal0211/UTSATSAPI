namespace UTSATSAPI.Controllers
{
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.Text.Encodings.Web;
    using UTSATSAPI.ATSCalls;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;
    using UTSATSAPI.Models.ViewModels.Validators;
    using UTSATSAPI.Repositories.Interfaces;
    using static UTSATSAPI.Helpers.Enum;

    [ApiController]
    [Route("Interview/")]
    public class InterviewController : ControllerBase
    {
        #region Variables
        private readonly ICommonInterface _commonInterface;
        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion

        #region Constructor
        public InterviewController(ICommonInterface commonInterface, TalentConnectAdminDBContext talentConnectAdminDBContext, IUniversalProcRunner universalProcRunner, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _commonInterface = commonInterface;
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
            _universalProcRunner = universalProcRunner;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        #endregion

        #region Authorized APIs
        [Authorize]
        [HttpGet("List")]
        public ObjectResult GetInterviewList(int pagenumber, int totalrecord)
        {
            try
            {
                int totalRecords = 0;
                var interviewList = _commonInterface.interview.sproc_UTS_GetHiringInterview(string.Format("{0},{1}", pagenumber, totalrecord));
                if (interviewList.Count() > 0)
                {
                    totalRecords = interviewList.ToList().FirstOrDefault().TotalRecords;
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Interview records not found" });

                }
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)totalrecord);

                var jsonData = new
                {
                    totalPages = totalPages,
                    pagenumber = pagenumber,
                    totalrows = totalRecords,
                    rows = interviewList
                };

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Interview records found", Details = jsonData });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("Schedule")]
        public ObjectResult ScheduleInterview(ScheduleInterviewModel scheduleInterviewModel, long loggedinuserid)
        {
            try
            {
                loggedinuserid = SessionValues.LoginUserId;

                #region Variables
                int status_id = 1;
                long? anotherRoundID = 1;
                int? insertedId = 0;
                string returnMessage = string.Empty;
                #endregion

                #region PreValidation
                if (scheduleInterviewModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty, please check the datatype or naming convanation", Details = scheduleInterviewModel });
                #endregion

                #region Validation
                ScheduleInterviewModelValidator validationRulesForScheduleModel = new ScheduleInterviewModelValidator();
                ValidationResult validationResult = validationRulesForScheduleModel.Validate(scheduleInterviewModel);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "scheduleInterviewModel") });
                }

                //UTS-3441: Do not allow same start and end time for scheduling the interview.
                var fetchSameTimingData = scheduleInterviewModel.RecheduleSlots.Where(x => x.StartTime == x.EndTime).ToList();
                if (fetchSameTimingData.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Start and End time cannot be same", Details = fetchSameTimingData });
                }

                #endregion

                PrgContactTimeZone prgContactTimeZone = _talentConnectAdminDBContext.PrgContactTimeZones.FirstOrDefault(x => x.Id == scheduleInterviewModel.WorkingTimeZoneID);

                #region Scheduling
                //slot type 1
                if (scheduleInterviewModel.RecheduleSlots.Count > 1) // Slot type 1 with 3 slot option
                {
                    #region SlotsValidation
                    Dictionary<string, DateTime> slotsDates = new Dictionary<string, DateTime>();
                    Dictionary<string, string> slotsStartTimes = new Dictionary<string, string>();
                    Dictionary<string, string> slotsEndTimes = new Dictionary<string, string>();
                    int cnt = 0;
                    foreach (var slot in scheduleInterviewModel.RecheduleSlots)
                    {
                        cnt += 1;
                        RescheduleSlotValidator validationRules = new RescheduleSlotValidator();
                        ValidationResult validationResultForSlots = validationRules.Validate(slot);
                        if (!validationResultForSlots.IsValid)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResultForSlots.Errors, "slot") });
                        }
                        else
                        {
                            slotsDates.Add(string.Format("D{0}", cnt), CommonLogic.ConvertString2DateTime(slot.SlotDate));
                            slotsStartTimes.Add(string.Format("D{0}", cnt), slot.StartTime);
                            slotsEndTimes.Add(string.Format("D{0}", cnt), slot.EndTime);
                        }
                    }

                    #endregion

                    #region SaveSlot 
                    object[] param = new object[]
                    {
                        scheduleInterviewModel.ContactID,
                        scheduleInterviewModel.ContactID,
                        scheduleInterviewModel.HiringRequest_Detail_ID,
                        scheduleInterviewModel.HiringRequest_ID,
                        slotsDates["D1"].ToString("yyyy-MM-dd"),
                        slotsDates["D2"].ToString("yyyy-MM-dd"),
                        slotsDates["D3"].ToString("yyyy-MM-dd"),
                        Convert.ToString(TimeSpan.Parse(slotsStartTimes["D1"])),
                        Convert.ToString(TimeSpan.Parse(slotsStartTimes["D2"])),
                        Convert.ToString(TimeSpan.Parse(slotsStartTimes["D3"])),
                        Convert.ToString(TimeSpan.Parse(slotsEndTimes["D1"])),
                        Convert.ToString(TimeSpan.Parse(slotsEndTimes["D2"])),
                        Convert.ToString(TimeSpan.Parse(slotsEndTimes["D3"])),
                        scheduleInterviewModel.WorkingTimeZoneID,
                        scheduleInterviewModel.Talent_ID,
                        status_id,anotherRoundID, scheduleInterviewModel.Additional_Notes, string.Empty
                    };

                    string paramstring = CommonLogic.ConvertToParamString(param);

                    Sproc_InsertBookSlot_Result sproc_InsertBookSlot_Result = _commonInterface.interview.Sproc_InsertBookSlot(paramstring);
                    if (sproc_InsertBookSlot_Result != null)
                    {
                        insertedId = sproc_InsertBookSlot_Result.ID;
                        returnMessage = sproc_InsertBookSlot_Result.ReturnMessage;
                    }
                    #endregion

                    if (insertedId > 0)
                    {
                        // UTS-7093: Change talent status to Interview even if slots given
                        ChangeTalentHRStatus(scheduleInterviewModel.HiringRequest_ID, scheduleInterviewModel.Talent_ID, scheduleInterviewModel.SlotType, loggedinuserid);

                        object[] param2 = new object[]
                        {
                            Action_Of_History.Slot_Given, scheduleInterviewModel.HiringRequest_ID,
                            scheduleInterviewModel.Talent_ID, false, loggedinuserid, 0, 0, "", 0,
                            false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param2);

                        EmailBinder emailBinderForInternal = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                        emailBinderForInternal.SendEmailNotificationToInternalTeam_SlotGiven(scheduleInterviewModel.Talent_ID, scheduleInterviewModel.ContactID, scheduleInterviewModel.HiringRequest_ID, prgContactTimeZone, scheduleInterviewModel.RecheduleSlots);

                        #region commented unused code talent emails

                        //string encryptedHRID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(scheduleInterviewModel.HiringRequest_ID)));
                        //string encryptedHRDetailID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(scheduleInterviewModel.HiringRequest_Detail_ID)));
                        //string encryptedContactID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(scheduleInterviewModel.ContactID)));

                        //GenTalentSelectedInterviewDetail TalentSelected = 
                        //    _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails
                        //    .FirstOrDefault(x => x.HiringRequestId == scheduleInterviewModel.HiringRequest_ID && 
                        //    x.HiringRequestDetailId == scheduleInterviewModel.HiringRequest_Detail_ID && 
                        //    x.ContactId == scheduleInterviewModel.ContactID && 
                        //    x.TalentId == scheduleInterviewModel.Talent_ID && x.StatusId == 1);

                        //var IsTalentNotificationSent = false;

                        //GenTalent talentDetail = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == scheduleInterviewModel.Talent_ID);
                        //if (talentDetail != null)
                        //    IsTalentNotificationSent = talentDetail.IsTalentNotificationSend ?? false;

                        //if (TalentSelected != null)
                        //{
                        //    if (TalentSelected.InterviewRound == 1)
                        //    {
                        //        #region SendEmail
                        //        if (IsTalentNotificationSent)
                        //        {
                        //            EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                        //            emailBinder.SendEmailForBookTimeSlot(scheduleInterviewModel.Talent_ID, scheduleInterviewModel.HiringRequest_ID, encryptedHRID, encryptedHRDetailID, encryptedContactID, string.Empty);
                        //        }
                        //        #endregion
                        //    }
                        //    else
                        //    {
                        //        #region SendEmail
                        //        var encryptedAnotherRound_Id = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(anotherRoundID)));
                        //        if (IsTalentNotificationSent)
                        //        {
                        //            EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                        //            emailBinder.SendEmailForBookTimeSlot(scheduleInterviewModel.Talent_ID, scheduleInterviewModel.HiringRequest_ID, encryptedHRID, encryptedHRDetailID, encryptedContactID, encryptedAnotherRound_Id, true);
                        //        }
                        //        #endregion
                        //    }
                        //}
                        //else
                        //{
                        //    #region SendEmail
                        //    if (IsTalentNotificationSent)
                        //    {
                        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                        //        emailBinder.SendEmailForBookTimeSlot(scheduleInterviewModel.Talent_ID, scheduleInterviewModel.HiringRequest_ID, encryptedHRID, encryptedHRDetailID, encryptedContactID, string.Empty);
                        //    }
                        //    #endregion
                        //}
                        #endregion
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = returnMessage });
                    }
                }
                else //slot type 2 or 4
                {
                    DateTime dateStartTime = DateTime.ParseExact(scheduleInterviewModel.RecheduleSlots.FirstOrDefault().STRStartTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);
                    DateTime dateEndTime = DateTime.ParseExact(scheduleInterviewModel.RecheduleSlots.FirstOrDefault().STREndTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);

                    Meeting meeting = new Meeting();

                    if (prgContactTimeZone == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please select valid Timezone / TimeZone is not available" });
                    }
                    else
                    {
                        if (scheduleInterviewModel.SlotType == 2)
                        {
                            string googleMeetUrl = _configuration["GoogleMeetPythonURL"].ToString();
                            meeting = ExternalCalls.ToScheduleZoomMeeting(prgContactTimeZone.Description, dateStartTime, googleMeetUrl);
                            if (meeting == null)
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Meeting Link is not initialized, Kindly contact Administrator" });
                        }
                    }

                    long Id = AddNewSlots(scheduleInterviewModel, Convert.ToInt32(loggedinuserid), meeting);
                    insertedId = Convert.ToInt32(Id);

                    if (insertedId > 0)
                    {
                        GenTalentSelectedInterviewDetail TalentSelected = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.HiringRequestId == scheduleInterviewModel.HiringRequest_ID && x.HiringRequestDetailId == scheduleInterviewModel.HiringRequest_Detail_ID && x.ContactId == scheduleInterviewModel.ContactID && x.TalentId == scheduleInterviewModel.Talent_ID && x.StatusId == (short)prg_InterviewStatus.Interview_Scheduled).OrderByDescending(x => x.Id).FirstOrDefault();

                        if (TalentSelected != null && TalentSelected.InterviewRound == 1)
                        {
                            EmailBinder emailBinderForInternal = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                            emailBinderForInternal.SendEmailNotificationToInternalTeamwithZoomDetails_Schedule(meeting, scheduleInterviewModel.SlotType, scheduleInterviewModel.InterviewCallLink, scheduleInterviewModel.Talent_ID, scheduleInterviewModel.ContactID, dateStartTime, prgContactTimeZone.ShortName, scheduleInterviewModel.HiringRequest_ID, dateEndTime, prgContactTimeZone, TalentSelected.ShortlistedInterviewId.Value);

                            #region commneted unused code , unused client, talent emails

                            //var encryptedHRID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(scheduleInterviewModel.HiringRequest_ID)));
                            //var encryptedHRDetailID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(scheduleInterviewModel.HiringRequest_Detail_ID)));
                            //var encryptedContactID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(scheduleInterviewModel.ContactID)));

                            //GenContact genContact = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == scheduleInterviewModel.ContactID);

                            //if (genContact != null && genContact.IsClientNotificationSend)
                            //{
                            //    Object[] param = new object[] { scheduleInterviewModel.ContactID };

                            //    sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact = _commonInterface.interview.sproc_Get_ContactPointofContact(CommonLogic.ConvertToParamString(param));

                            //    EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                            //    emailBinder.SendEmailNotificationToClientwithZoomDetails_Schedule(meeting, scheduleInterviewModel.SlotType, scheduleInterviewModel.InterviewCallLink, scheduleInterviewModel.Talent_ID, scheduleInterviewModel.ContactID, dateStartTime, prgContactTimeZone.ShortName, scheduleInterviewModel.HiringRequest_ID, dateEndTime, prgContactTimeZone, sproc_Get_ContactPointofContact, 0);
                            //}

                            //GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == scheduleInterviewModel.Talent_ID);

                            //if (_Talent != null)
                            //{
                            //    var NotifyToTalent = _Talent.IsTalentNotificationSend ?? false;
                            //    if (NotifyToTalent)
                            //    {
                            //        Object[] param = new object[] { scheduleInterviewModel.ContactID };
                            //        sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact = _commonInterface.interview.sproc_Get_ContactPointofContact(CommonLogic.ConvertToParamString(param));

                            //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                            //        emailBinder.SendEmailNotificationToTalentwithZoomDetails_Schedule(meeting, scheduleInterviewModel.SlotType, scheduleInterviewModel.InterviewCallLink, scheduleInterviewModel.Talent_ID, scheduleInterviewModel.ContactID, dateStartTime, prgContactTimeZone.ShortName, scheduleInterviewModel.HiringRequest_ID, dateEndTime, prgContactTimeZone, sproc_Get_ContactPointofContact, TalentSelected.ShortlistedInterviewId.Value);
                            //    }
                            //}

                            #endregion
                        }
                    }
                }
                #endregion

                #region ATS Call
                if (_configuration["HRDataSendSwitchForPHP"].ToLower() != "local")
                {
                    #region Send Data to ATS via API Call.

                    string json = "";

                    ATSInterviewDetailViewModel model = new ATSInterviewDetailViewModel();
                    model.ATS_TalentID = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == scheduleInterviewModel.Talent_ID).FirstOrDefault().AtsTalentId ?? 0;
                    model.UTS_TalentID = scheduleInterviewModel.Talent_ID;
                    model.UTS_HiringRequestID = scheduleInterviewModel.HiringRequest_ID;

                    List<GenSalesHiringRequestInterviewerDetail> HR_InterviewerDetails = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == scheduleInterviewModel.HiringRequest_ID && x.HiringRequestDetailId == scheduleInterviewModel.HiringRequest_Detail_ID).ToList();
                    model.HR_InterviewerDetails = ModelBinderATS.InterviewerDetails(HR_InterviewerDetails);

                    List<GenInterviewSlotsMaster> Interview_SlotMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(x => x.HiringRequestId == scheduleInterviewModel.HiringRequest_ID &&
                                                            x.HiringRequestDetailId == scheduleInterviewModel.HiringRequest_Detail_ID &&
                                                            x.TalentId == scheduleInterviewModel.Talent_ID &&
                                                            x.ContactId == scheduleInterviewModel.ContactID).ToList();
                    model.Interview_SlotMaster = ModelBinderATS.InterviewSlotsMaster(Interview_SlotMaster);

                    List<GenTalentSelectedInterviewDetail> SelectedTalent_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.HiringRequestId == scheduleInterviewModel.HiringRequest_ID &&
                                                            x.HiringRequestDetailId == scheduleInterviewModel.HiringRequest_Detail_ID &&
                                                            x.TalentId == scheduleInterviewModel.Talent_ID &&
                                                            x.ContactId == scheduleInterviewModel.ContactID).ToList();
                    model.SelectedTalent_InterviewDetails = ModelBinderATS.TalentSelectedInterviewDetail(SelectedTalent_InterviewDetails);

                    List<GenShortlistedTalentInterviewDetail> ShortListedTalent_InterviewDetails = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.HiringRequestId == scheduleInterviewModel.HiringRequest_ID &&
                                                            x.HiringRequestDetailId == scheduleInterviewModel.HiringRequest_Detail_ID &&
                                                            x.TalentId == scheduleInterviewModel.Talent_ID &&
                                                            x.ContactId == scheduleInterviewModel.ContactID).ToList();
                    model.ShortListedTalent_InterviewDetails = ModelBinderATS.ShortlistedTalentInterviewDetail(ShortListedTalent_InterviewDetails);

                    try
                    {
                        json = JsonConvert.SerializeObject(model);
                        ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                        aTSCall.SendInterviewDetailsWithSlot(json, loggedinuserid, scheduleInterviewModel.HiringRequest_ID);
                        Thread.Sleep(1000);
                        aTSCall.CallAtsAPIToSendTalentAndHRStatus(model.UTS_TalentID, loggedinuserid, scheduleInterviewModel.HiringRequest_ID);

                    }
                    catch (Exception)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Interview has been scheduled" });
                    }

                    #endregion
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Interview has been scheduled" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("Reschedule")]
        public ObjectResult RescheduleInterview(RescheduleInterviewSlotModel rescheduleInterviewSlotModel, long loggedinuserid = 0)
        {
            try
            {
                loggedinuserid = SessionValues.LoginUserId;
                #region PreValidation
                if (rescheduleInterviewSlotModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty, please check the datatype or naming convanation", Details = rescheduleInterviewSlotModel });
                #endregion

                #region Validation
                RescheduleInterviewSlotModelValidator validationRulesForScheduleModel = new RescheduleInterviewSlotModelValidator();
                ValidationResult validationResult = validationRulesForScheduleModel.Validate(rescheduleInterviewSlotModel);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "rescheduleInterviewSlotModel") });
                }

                if (rescheduleInterviewSlotModel.IsAnotherRoundInterview == null)
                {
                    rescheduleInterviewSlotModel.IsAnotherRoundInterview = false;
                }
                if (!(bool)rescheduleInterviewSlotModel.IsAnotherRoundInterview)
                {
                    if (string.IsNullOrEmpty(rescheduleInterviewSlotModel.ReasonforReschedule) ||
                        string.IsNullOrEmpty(rescheduleInterviewSlotModel.RescheduleRequestBy))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = "ReasonforReschedule / RescheduleRequestBy can not be empty" });
                    }
                }
                #endregion

                #region Variables
                long InterviewSlotsMaster_ID = 0;
                var InterviewStatusID = Convert.ToInt64(rescheduleInterviewSlotModel.InterviewStatus);
                long? Round = 0;
                string strRound = "";
                Meeting meeting = new Meeting();
                DateTime dtStartTime = new DateTime();
                DateTime dtEndTime = new DateTime();
                DateTime dateStartTime = DateTime.Now;
                DateTime dateEndTime = DateTime.Now;
                DateTime dateSlotDate = DateTime.Now;
                #endregion

                #region Case 2.2.1 | 2.2.2
                if (rescheduleInterviewSlotModel.SlotType == 1 && rescheduleInterviewSlotModel.InterviewStatus == 1)
                {
                    UpdateSlots(rescheduleInterviewSlotModel, loggedinuserid);
                }
                #endregion

                #region Case3.3.1 && 3.3.2
                if (rescheduleInterviewSlotModel.SlotType == 1 && rescheduleInterviewSlotModel.InterviewStatus == 4)
                {
                    long Id = AddNewSlotsForRescheduling(rescheduleInterviewSlotModel, Convert.ToInt32(loggedinuserid), null);

                    //Get Round detail
                    Round = _talentConnectAdminDBContext.GenInterviewSlotsMasters.FirstOrDefault(x => x.Id == Id).InterviewRoundCount;
                }
                #endregion

                #region Case 1
                if ((rescheduleInterviewSlotModel.SlotType == 2 || rescheduleInterviewSlotModel.SlotType == 4) && rescheduleInterviewSlotModel.InterviewStatus == 4)
                {
                    InterviewSlotsMaster_ID = AddNewSlotsForRescheduling(rescheduleInterviewSlotModel, Convert.ToInt32(loggedinuserid), null);
                    Round = _talentConnectAdminDBContext.GenInterviewSlotsMasters.FirstOrDefault(x => x.Id == InterviewSlotsMaster_ID).InterviewRoundCount;
                }
                #endregion

                #region Case 4
                if (rescheduleInterviewSlotModel.SlotType == 3 && rescheduleInterviewSlotModel.InterviewStatus == 4)
                {
                    GenInterviewSlotsMaster genInterviewSlotsMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.InterviewMasterID);
                    genInterviewSlotsMaster.InterviewStatusId = (short)prg_InterviewStatus.Cancelled;
                    Round = genInterviewSlotsMaster.InterviewRoundCount;
                    strRound = genInterviewSlotsMaster.InterviewRoundStr;
                    CommonLogic.DBOperator(_talentConnectAdminDBContext, genInterviewSlotsMaster, EntityState.Modified);

                    var slotlist = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.InterviewMasterId == rescheduleInterviewSlotModel.InterviewMasterID).ToList();
                    foreach (var slot in slotlist)
                    {
                        slot.StatusId = 3;
                        slot.LastModifiedById = Convert.ToInt32(loggedinuserid);
                        slot.LastModifiedDatetime = DateTime.Now;
                        CommonLogic.DBOperator(_talentConnectAdminDBContext, slot, EntityState.Modified);
                    }

                    GenTalent genTalent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.Talent_ID);
                    genTalent.TalentStatusIdAfterClientSelection = 2;
                    //TODO : Make enum for this status id above(^)
                    genTalent.LastModifiedById = Convert.ToInt32(loggedinuserid);
                    genTalent.LastModifiedDatetime = DateTime.Now;
                    CommonLogic.DBOperator(_talentConnectAdminDBContext, genTalent, EntityState.Modified);

                    object[] param = new object[]
                    {
                    Action_Of_History.Cancelled, rescheduleInterviewSlotModel.HiringRequest_ID, rescheduleInterviewSlotModel.Talent_ID, false, loggedinuserid, 0, rescheduleInterviewSlotModel.InterviewMasterID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                    };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                }
                #endregion

                #region Another Round + later option
                var GetShortlistedInterviewId = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.FirstOrDefault(x => x.InterviewMasterId == rescheduleInterviewSlotModel.InterviewMasterID && x.StatusId == (short)prg_InterviewStatus.Feedback_Submitted);
                if (GetShortlistedInterviewId != null)
                {
                    var laterInterviewSlots = _talentConnectAdminDBContext.GenTalentSelectedNextRoundInterviewDetails.FirstOrDefault(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID && x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID && x.ShortlistedInterviewId == GetShortlistedInterviewId.Id);
                    if (laterInterviewSlots != null)
                    {
                        //if (string.IsNullOrEmpty(laterInterviewSlots.SlotGiven) || laterInterviewSlots.SlotGiven.Equals("Later"))
                        //{
                        InterviewSlotsMaster_ID = AddNewSlotsForLaterOption(rescheduleInterviewSlotModel, Convert.ToInt32(loggedinuserid), GetShortlistedInterviewId.Id, true);
                        //}
                    }
                    if (laterInterviewSlots == null && rescheduleInterviewSlotModel.NextRound_InterviewDetailsID == 0)
                    {
                        InterviewSlotsMaster_ID = AddNewSlotsForLaterOption(rescheduleInterviewSlotModel, Convert.ToInt32(loggedinuserid), GetShortlistedInterviewId.Id, false);
                    }

                    if (InterviewSlotsMaster_ID > 0)
                    {
                        GenInterviewSlotsMaster genInterviewSlotsMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.FirstOrDefault(x => x.Id == InterviewSlotsMaster_ID);
                        if (genInterviewSlotsMaster != null)
                        {
                            Round = genInterviewSlotsMaster.InterviewRoundCount;
                            rescheduleInterviewSlotModel.InterviewStatus = (int)genInterviewSlotsMaster.InterviewStatusId;
                        }
                    }

                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, new object[] { Action_Of_History.Another_Round_Slot, rescheduleInterviewSlotModel.HiringRequest_ID, rescheduleInterviewSlotModel.Talent_ID, false, loggedinuserid, 0, InterviewSlotsMaster_ID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS });
                }
                #endregion

                #region Get Talent and Client Detail
                GenTalent talent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.Talent_ID);
                GenContact client = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.ContactID);
                #endregion

                #region Create Dictionary
                Dictionary<string, string> dataContent = new Dictionary<string, string>();

                if (talent != null)
                {
                    GenTalentPointofContact talentPointofContact = new GenTalentPointofContact();
                    talentPointofContact = _talentConnectAdminDBContext.GenTalentPointofContacts.FirstOrDefault(x => x.TalentId == talent.Id);
                    if (talentPointofContact != null)
                    {
                        long userId = Convert.ToInt64(talentPointofContact.UserId);
                        UsrUser user = new UsrUser();
                        user = _talentConnectAdminDBContext.UsrUsers.FirstOrDefault(x => x.Id == userId);
                        if (user != null)
                        {
                            dataContent.Add("PointOfContactEmail", user.EmailId);
                            dataContent.Add("PointOfContactName", user.FullName);
                        }
                    }
                    dataContent.Add("TalentName", talent.Name);
                }

                if (client != null)
                {
                    dataContent.Add("ClientName", client.FullName);
                    dataContent.Add("ClientEmail", client.EmailId);
                    var CompanyName = _talentConnectAdminDBContext.GenCompanies.FirstOrDefault(x => x.Id == client.CompanyId).Company;
                    if (!String.IsNullOrEmpty(CompanyName))
                    {
                        dataContent.Add("CompanyName", CompanyName);
                    }
                    else
                        dataContent.Add("CompanyName", "");

                }

                //Get gen_SalesHiringRequest_Details for tacking Role
                var HRDetails = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.HiringRequest_Detail_ID & x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID);
                string RoleName = "";
                if (HRDetails != null)
                {
                    var RoleDetails = _talentConnectAdminDBContext.PrgTalentRoles.FirstOrDefault(x => x.Id == HRDetails.RoleId && x.IsActive == true);
                    if (RoleDetails != null)
                    {
                        RoleName = RoleDetails.TalentRole;
                    }
                }

                dataContent.Add("HRID", Convert.ToString(rescheduleInterviewSlotModel.HiringRequestNumber));
                dataContent.Add("Position", RoleName);

                //Get timezone detail 
                var timeZone = _talentConnectAdminDBContext.PrgContactTimeZones.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.WorkingTimeZoneID);

                //Create ZoomLink and Passcode
                if (rescheduleInterviewSlotModel.SlotType == 2)
                {
                    dtStartTime = DateTime.ParseExact(rescheduleInterviewSlotModel.RescheduleSlot.FirstOrDefault().STRStartTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);
                    dtEndTime = DateTime.ParseExact(rescheduleInterviewSlotModel.RescheduleSlot.FirstOrDefault().STREndTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);

                    if (timeZone != null)
                    {
                        string googleMeetUrl = _configuration["GoogleMeetPythonURL"].ToString();
                        meeting = ExternalCalls.ToScheduleZoomMeeting(timeZone.Description, dtStartTime, googleMeetUrl);
                        if (meeting == null)
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Meeting Link is not initialized, Kindly contact Administrator" });
                    }

                    string interviewLink = "";
                    string PassCode = "";
                    if (meeting != null)
                    {
                        interviewLink = meeting.join_url;
                        PassCode = meeting.password;
                    }

                    dataContent.Add("InterviewLink", interviewLink);
                    dataContent.Add("PassCode", PassCode);
                }
                else
                {
                    dataContent.Add("InterviewLink", rescheduleInterviewSlotModel.InterviewCallLink);
                    dataContent.Add("PassCode", "");
                }
                dataContent.Add("timeZone", timeZone != null ? timeZone.ShortName : "");
                dataContent.Add("RoundNo", Round != null ? Round.Value.ToString() : "0");
                dataContent.Add("Round", !string.IsNullOrEmpty(strRound) ? strRound : null);
                string allSlots = "<table style='border-style: solid; width:100%'><tr><th style='text-align: center;'>No.</th><th style='text-align: center;'>Interview Slot Date</th><th style='text-align: center;'>StartTime</th><th style='text-align: center;'>EndTime</th></tr>";
                if (rescheduleInterviewSlotModel.RescheduleSlot != null && rescheduleInterviewSlotModel.RescheduleSlot.Count > 0)
                {
                    foreach (var s in rescheduleInterviewSlotModel.RescheduleSlot)
                    {
                        dateStartTime = DateTime.ParseExact(s.STRStartTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);
                        dateEndTime = DateTime.ParseExact(s.STREndTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);
                        dateSlotDate = DateTime.ParseExact(s.STRSlotDate, "MM/dd/yyyy", CultureInfo.CurrentCulture);
                        allSlots += string.Format("<tr><td style='text-align: center;'>{0}</td><td style='text-align: center;'>{1}</td><td style='text-align: center;'>{2}</td><td style='text-align: center;'>{3}</td></tr>", s.SlotID, dateSlotDate.ToShortDateString(), dateStartTime.TimeOfDay, dateEndTime.TimeOfDay);
                    }
                    allSlots += "</table>";

                    string strInterviewdatetime =
                        dtStartTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
                        dtStartTime.ToString("hh:mm tt ") +
                        dtEndTime.ToString("hh:mm tt ") + timeZone.ShortName;

                    string strInterviewdatetimeIst =
                        dtStartTime.AddMinutes(timeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
                        dtStartTime.AddMinutes(timeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
                        dtEndTime.AddMinutes(timeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST";

                    //dataContent.Add("Interviewdatetime", rescheduleInterviewSlotModel.RescheduleSlot.FirstOrDefault().StartTime.ToString());

                    dataContent.Add("Interviewdatetime", strInterviewdatetime);
                    dataContent.Add("Interviewdatetimeist", strInterviewdatetimeIst);

                    dataContent.Add("StartTime", dtStartTime.ToString("yyyy/MM/dd HH:mm:ss tt"));
                    dataContent.Add("EndTime", dtEndTime.ToString("yyyy/MM/dd HH:mm:ss tt"));
                }
                else
                {
                    allSlots = "";
                    dataContent.Add("Interviewdatetime", "");
                    dataContent.Add("Interviewdatetimeist", "");
                    //dataContent.Add("Interviewdatetime", "");
                    dataContent.Add("StartTime", "");
                    dataContent.Add("EndTime", "");
                }
                dataContent.Add("AllSlots", allSlots);


                //Get hiringRequestDetail for Sales Person ID
                var hiringRequestDetail = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.HiringRequest_ID);
                if (hiringRequestDetail != null)
                {
                    //get sales person detail
                    UsrUser Userdetail = _talentConnectAdminDBContext.UsrUsers.FirstOrDefault(x => x.Id == hiringRequestDetail.SalesUserId);
                    if (Userdetail != null)
                    {
                        dataContent.Add("AMID", Userdetail.FullName);
                        dataContent.Add("AMIDEmail", Userdetail.EmailId);
                    }
                }
                #endregion

                #region slot 2
                if (rescheduleInterviewSlotModel.SlotType == 2 && (rescheduleInterviewSlotModel.InterviewStatus == 4))
                {
                    #region Update GenTalentSelectedInterviewDetail
                    GenTalentSelectedInterviewDetail TalentSelected = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID && x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID && x.ContactId == rescheduleInterviewSlotModel.ContactID && x.TalentId == rescheduleInterviewSlotModel.Talent_ID && x.InterviewMasterId == InterviewSlotsMaster_ID).OrderByDescending(x => x.Id).FirstOrDefault();

                    if (TalentSelected != null && meeting != null)
                    {
                        TalentSelected.ShortlistedInterviewId = TalentSelected.ShortlistedInterviewId;
                        TalentSelected.Id = TalentSelected.Id;
                        TalentSelected.ZoomInterviewLink = meeting.join_url;
                        TalentSelected.ZoomMeetingId = Convert.ToString(meeting.id);
                        TalentSelected.ZoomMeetingscheduledOn = DateTime.Now;
                        TalentSelected.ZoomInterviewKitUsername = meeting.host_email;
                        TalentSelected.ZoomInterviewKitPassword = meeting.password;
                        TalentSelected.InterviewMasterId = InterviewSlotsMaster_ID;
                        TalentSelected.AdditionalNotes = rescheduleInterviewSlotModel.Additional_Notes;
                        CommonLogic.DBOperator(_talentConnectAdminDBContext, TalentSelected, EntityState.Modified);

                        _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Update_TalentSelected, new object[] { TalentSelected.Id, loggedinuserid, TalentSelected.ShortlistedInterviewId, meeting.join_url, Convert.ToString(meeting.id), DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"), meeting.host_email, meeting.password, TalentSelected.AdditionalNotes });

                    }
                    #endregion
                }
                #endregion

                #region slot 4
                if (rescheduleInterviewSlotModel.SlotType == 4 && (rescheduleInterviewSlotModel.InterviewStatus == 4))
                {
                    #region Update GenTalentSelectedInterviewDetail
                    GenTalentSelectedInterviewDetail TalentSelected = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID && x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID && x.ContactId == rescheduleInterviewSlotModel.ContactID && x.TalentId == rescheduleInterviewSlotModel.Talent_ID && x.InterviewMasterId == InterviewSlotsMaster_ID).OrderByDescending(x => x.Id).FirstOrDefault();

                    if (TalentSelected != null)
                    {
                        TalentSelected.ShortlistedInterviewId = TalentSelected.ShortlistedInterviewId;
                        TalentSelected.Id = TalentSelected.Id;
                        TalentSelected.ZoomInterviewLink = rescheduleInterviewSlotModel.InterviewCallLink;
                        TalentSelected.InterviewMasterId = InterviewSlotsMaster_ID;
                        TalentSelected.AdditionalNotes = rescheduleInterviewSlotModel.Additional_Notes;
                        CommonLogic.DBOperator(_talentConnectAdminDBContext, TalentSelected, EntityState.Modified);

                        _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Update_TalentSelected, new object[] { TalentSelected.Id, loggedinuserid, TalentSelected.ShortlistedInterviewId, TalentSelected.ZoomInterviewLink, "", DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"), "", "", TalentSelected.AdditionalNotes });
                    }
                    #endregion
                }
                #endregion

                #region Send Email
                EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                emailBinder.SendRescheduleInterviewSlotCommonEmailWithContent(loggedinuserid, rescheduleInterviewSlotModel.Talent_ID, rescheduleInterviewSlotModel.RescheduleRequestBy ?? "", Convert.ToInt32(rescheduleInterviewSlotModel.InterviewStatus), rescheduleInterviewSlotModel.SlotType, talent, client, dataContent, rescheduleInterviewSlotModel.HiringRequest_ID, timeZone);
                #endregion

                #region ATS Call

                if (_configuration["HRDataSendSwitchForPHP"].ToLower() != "local")
                {
                    #region Send Data to ATS via API Call.

                    string json = "";

                    ATSInterviewDetailViewModel model = new ATSInterviewDetailViewModel();
                    model.ATS_TalentID = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == rescheduleInterviewSlotModel.Talent_ID).FirstOrDefault().AtsTalentId ?? 0;
                    model.UTS_TalentID = rescheduleInterviewSlotModel.Talent_ID;
                    model.UTS_HiringRequestID = rescheduleInterviewSlotModel.HiringRequest_ID;

                    List<GenSalesHiringRequestInterviewerDetail> HR_InterviewerDetails = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID && x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID).ToList();
                    model.HR_InterviewerDetails = ModelBinderATS.InterviewerDetails(HR_InterviewerDetails);

                    List<GenInterviewSlotsMaster> Interview_SlotMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID &&
                                                            x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID &&
                                                            x.TalentId == rescheduleInterviewSlotModel.Talent_ID &&
                                                            x.ContactId == rescheduleInterviewSlotModel.ContactID).ToList();
                    model.Interview_SlotMaster = ModelBinderATS.InterviewSlotsMaster(Interview_SlotMaster);

                    List<GenTalentSelectedInterviewDetail> SelectedTalent_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID &&
                                                            x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID &&
                                                            x.TalentId == rescheduleInterviewSlotModel.Talent_ID &&
                                                            x.ContactId == rescheduleInterviewSlotModel.ContactID).ToList();
                    model.SelectedTalent_InterviewDetails = ModelBinderATS.TalentSelectedInterviewDetail(SelectedTalent_InterviewDetails);


                    List<GenShortlistedTalentInterviewDetail> ShortListedTalent_InterviewDetails = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID &&
                                                            x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID &&
                                                            x.TalentId == rescheduleInterviewSlotModel.Talent_ID &&
                                                            x.ContactId == rescheduleInterviewSlotModel.ContactID).ToList();
                    model.ShortListedTalent_InterviewDetails = ModelBinderATS.ShortlistedTalentInterviewDetail(ShortListedTalent_InterviewDetails);

                    try
                    {
                        json = JsonConvert.SerializeObject(model);
                        ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                        aTSCall.SendInterviewDetailsWithSlot(json, loggedinuserid, rescheduleInterviewSlotModel.HiringRequest_ID);
                        Thread.Sleep(1000);
                        aTSCall.CallAtsAPIToSendTalentAndHRStatus(model.UTS_TalentID, loggedinuserid, rescheduleInterviewSlotModel.HiringRequest_ID);
                    }
                    catch (Exception)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Interview has been rescheduled" });
                    }

                    #endregion
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Interview has been rescheduled" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetClientFeedback")]
        public ObjectResult GetClientFeedback(long ClientFeedbackId)
        {

            try
            {

                GenContactInterviewFeedback getFeedback = null;

                #region Pre Validation
                if (ClientFeedbackId == null || ClientFeedbackId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide propoer ClientFeedbackId" });
                }
                else
                {
                    getFeedback = _talentConnectAdminDBContext.GenContactInterviewFeedbacks.FirstOrDefault(x => x.Id == ClientFeedbackId);
                    if (getFeedback == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "ClientFeedback Not exist" });
                    }
                }
                #endregion

                ClientFeedbackViewModel clientFeedbackViewModel = new ClientFeedbackViewModel();
                clientFeedbackViewModel.HiringRequestID = getFeedback.HiringRequestId ?? 0;
                clientFeedbackViewModel.HiringRequestDetailID = getFeedback.HiringRequestDetailId ?? 0;
                clientFeedbackViewModel.ShortlistedInterviewID = getFeedback.ShortlistedInterviewId ?? 0;
                clientFeedbackViewModel.ContactInterviewFeedbackId = ClientFeedbackId;
                clientFeedbackViewModel.HdnRadiovalue = getFeedback.FeedBackType;
                clientFeedbackViewModel.TechnicalSkillRating = getFeedback.TechnicalSkillRating;
                clientFeedbackViewModel.CommunicationSkillRating = getFeedback.CommunicationSkillRating;
                clientFeedbackViewModel.CognitiveSkillRating = getFeedback.CognitiveSkillRating;
                clientFeedbackViewModel.MessageToTalent = getFeedback.MessageToTalent;
                clientFeedbackViewModel.ClientsDecision = getFeedback.ClientsDecision;
                clientFeedbackViewModel.Comments = getFeedback.Comments;
                clientFeedbackViewModel.TopSkill = string.Empty;
                clientFeedbackViewModel.ImprovedSkill = string.Empty;

                if (clientFeedbackViewModel != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = clientFeedbackViewModel });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "ClientFeedback Not exist" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPost("Feedback")]
        public ObjectResult SubmitClientFeedback(ClientFeedbackViewModel clientFeedbackViewModel, long loggedinuserid = 0)
        {
            try
            {
                loggedinuserid = SessionValues.LoginUserId;
                string json = "";

                var varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(SessionValues.LoginUserId);

                #region PreValidation
                if (clientFeedbackViewModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty, please check the datatype or naming convanation", Details = clientFeedbackViewModel });
                #endregion

                #region Variable
                string TalentStatus = string.Empty;
                int TalentStatusID = 0;
                string responseMessage = "Inserted";
                //bool NotificationSendToClient = clientFeedbackViewModel.IsClientNotificationSent ?? false;
                bool NotificationSendToClient = false;
                ContactTalentPriorityModel contactTalentPriorityModel = new();
                List<outTalentDetailATS> talentCancelledResponsesList = new();
                outTalentDetailATS talentCancelledResponseModel = new();
                long InterviewMasterId = 0;
                #endregion

                #region Validation
                ClientFeedbackViewModelValidator validationRules = new ClientFeedbackViewModelValidator();
                ValidationResult validationResult = validationRules.Validate(clientFeedbackViewModel);
                if (!validationResult.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "clientFeedbackViewModel") });
                #endregion

                long ContactID = clientFeedbackViewModel.ContactIDValue;

                GenSalesHiringRequestDetail genSalesHiringRequestDetail = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.FirstOrDefault(x => x.HiringRequestId == clientFeedbackViewModel.HiringRequestID);
                var InterviewDetails_Obj = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.FirstOrDefault(x => x.ShortlistedInterviewId == clientFeedbackViewModel.ShortlistedInterviewID && x.HiringRequestId == clientFeedbackViewModel.HiringRequestID && x.TalentId == clientFeedbackViewModel.TalentIDValue);
                if (InterviewDetails_Obj != null)
                {
                    InterviewMasterId = Convert.ToInt64(InterviewDetails_Obj.InterviewMasterId);
                }
                var isAnotherTalent = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(y => y.HiringRequestId == clientFeedbackViewModel.HiringRequestID
                                 && y.HiringRequestDetailId == genSalesHiringRequestDetail.Id
                                 && y.TalentId != clientFeedbackViewModel.TalentIDValue
                                 && y.TalentStatusIdBasedOnHr == 4
                                ).Any();

                GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == clientFeedbackViewModel.TalentIDValue);

                if (clientFeedbackViewModel.FeedbackId > 0)
                {
                    //Update it
                    object[] param = new object[]
                    {
                             clientFeedbackViewModel.ShortlistedInterviewID,
                             clientFeedbackViewModel.HiringRequestID,
                             genSalesHiringRequestDetail.Id,
                             clientFeedbackViewModel.ContactIDValue,
                             clientFeedbackViewModel.Role,
                             clientFeedbackViewModel.HdnRadiovalue,
                             clientFeedbackViewModel.TalentIDValue,
                             clientFeedbackViewModel.TechnicalSkillRating,
                             clientFeedbackViewModel.CommunicationSkillRating,
                             clientFeedbackViewModel.CognitiveSkillRating,
                             clientFeedbackViewModel.TopSkill,
                             clientFeedbackViewModel.ImprovedSkill,
                             clientFeedbackViewModel.MessageToTalent,
                             clientFeedbackViewModel.ClientsDecision,
                             clientFeedbackViewModel.Comments,
                             clientFeedbackViewModel.FeedbackId,
                             loggedinuserid,
                             (short)AppActionDoneBy.UTS
                    };
                    var updatedObject = _commonInterface.interview.Sproc_Add_TalentSelected_ClientFeedback(CommonLogic.ConvertToParamString(param));
                    object[] param2 = new object[] { Action_Of_History.Update_Client_Feedback, clientFeedbackViewModel.HiringRequestID, clientFeedbackViewModel.TalentIDValue, false, loggedinuserid, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param2);
                    responseMessage = "Updated";
                }
                else
                {
                    object[] param = new object[]
                    {
                             clientFeedbackViewModel.ShortlistedInterviewID,
                             clientFeedbackViewModel.HiringRequestID,
                             genSalesHiringRequestDetail.Id,
                             clientFeedbackViewModel.ContactIDValue,
                             clientFeedbackViewModel.Role,
                             clientFeedbackViewModel.HdnRadiovalue,
                             clientFeedbackViewModel.TalentIDValue,
                             clientFeedbackViewModel.TechnicalSkillRating,
                             clientFeedbackViewModel.CommunicationSkillRating,
                             clientFeedbackViewModel.CognitiveSkillRating,
                             clientFeedbackViewModel.TopSkill,
                             clientFeedbackViewModel.ImprovedSkill,
                             clientFeedbackViewModel.MessageToTalent,
                             clientFeedbackViewModel.ClientsDecision,
                             clientFeedbackViewModel.Comments,
                             0,
                             loggedinuserid,
                             (short)AppActionDoneBy.UTS
                    };
                    var insertedObject = _commonInterface.interview.Sproc_Add_TalentSelected_ClientFeedback(CommonLogic.ConvertToParamString(param));
                    if (insertedObject != null)
                        clientFeedbackViewModel.en_Id = CommonLogic.Encrypt(insertedObject.ID.ToString());
                }

                if (clientFeedbackViewModel.HdnRadiovalue == "Hire")
                {
                    TalentStatusID = (short)prg_TalentStatus_AfterClientSelection.Offered; //Hire(Offered)
                    object[] param = new object[] { Action_Of_History.Feedback_Submitted_With_Hire, clientFeedbackViewModel.HiringRequestID, clientFeedbackViewModel.TalentIDValue, false, loggedinuserid, 0, InterviewMasterId, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                }
                if (clientFeedbackViewModel.HdnRadiovalue == "NoHire")
                {
                    object[] RejectParam = new object[]
                    {
                        clientFeedbackViewModel.HiringRequestDetailID,
                        clientFeedbackViewModel.TalentIDValue,
                        clientFeedbackViewModel.RejectReasonID,
                        clientFeedbackViewModel.OtherReason, // rejection message for candidate
                        string.Empty,
                        clientFeedbackViewModel.Remark, // comments 
                        loggedinuserid,
                        clientFeedbackViewModel.HiringRequestID,
                        (short)AppActionDoneBy.UTS
                    };
                    string rejectParamString = CommonLogic.ConvertToParamString(RejectParam);
                    _commonInterface.TalentStatus.sproc_UTS_UpdateTalentRejectReason(rejectParamString);

                    TalentStatusID = (short)prg_TalentStatus_AfterClientSelection.Rejected; //Rejected
                    object[] param = new object[] { Action_Of_History.Feedback_Submitted_With_NoHire, clientFeedbackViewModel.HiringRequestID, clientFeedbackViewModel.TalentIDValue, false, loggedinuserid, 0, InterviewMasterId, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);

                    #region SendEmail
                    EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                    emailBinder.SendEmailToInternalTeamWhenTalentIsRejected(clientFeedbackViewModel.TalentIDValue, clientFeedbackViewModel.RejectReasonID ?? 0,
                                                                            clientFeedbackViewModel.HiringRequestID, clientFeedbackViewModel.Remark ?? "",
                                                                            clientFeedbackViewModel.OtherReason ?? "", varUsrUserById?.FullName ?? "");
                    #endregion
                }
                if (clientFeedbackViewModel.HdnRadiovalue == "OnHold")
                {
                    TalentStatusID = (short)prg_TalentStatus_AfterClientSelection.OnHold; //On Hold
                    object[] param = new object[] { Action_Of_History.Feedback_Submitted_With_OnHold, clientFeedbackViewModel.HiringRequestID, clientFeedbackViewModel.TalentIDValue, false, loggedinuserid, 0, InterviewMasterId, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                }
                if (clientFeedbackViewModel.HdnRadiovalue == "AnotherRound")
                {
                    TalentStatusID = (short)prg_TalentStatus_AfterClientSelection.ProfileShared; //Shortlisted(Profile Shared)
                    object[] param = new object[] { Action_Of_History.Feedback_Submitted_With_AnotherRound, clientFeedbackViewModel.HiringRequestID, clientFeedbackViewModel.TalentIDValue, false, loggedinuserid, 0, InterviewMasterId, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                }

                //if (clientFeedbackViewModel.HdnRadiovalue != "AnotherRound")
                //{
                //    EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);

                //    if (NotificationSendToClient)
                //    {
                //        GenContact _Contact = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == ContactID);
                //        if (_Contact != null && _Contact.IsClientNotificationSend)
                //        {
                //            emailBinder.SendEmailForClientFeedbackSubmit(ContactID, clientFeedbackViewModel.TalentIDValue, clientFeedbackViewModel.HiringRequestID, clientFeedbackViewModel.HdnRadiovalue);
                //        }
                //    }

                //    if (_Talent != null)
                //    {
                //        var NotifyToTalent = _Talent.IsTalentNotificationSend ?? false;
                //        if (NotifyToTalent)
                //            emailBinder.SendEmailForClientFeedbackSubmitToTalent(ContactID, clientFeedbackViewModel.TalentIDValue, clientFeedbackViewModel.HiringRequestID, clientFeedbackViewModel.HdnRadiovalue);
                //    }
                //}

                #region ATS Call
                if (_configuration["HRDataSendSwitchForPHP"].ToLower() != "local")
                {
                    _talentConnectAdminDBContext.Entry(_Talent).Reload();
                    if (_Talent != null)
                    {
                        var HiringRequestData = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(x => x.Id == clientFeedbackViewModel.HiringRequestID);
                        if (HiringRequestData != null)
                        {
                            #region Send Interview Data to ATS via API Call.

                            ATSInterviewDetailViewModel model = new();
                            model.ATS_TalentID = _Talent.AtsTalentId ?? 0;
                            model.UTS_TalentID = _Talent.Id;
                            model.UTS_HiringRequestID = HiringRequestData.Id;

                            List<GenSalesHiringRequestInterviewerDetail> HR_InterviewerDetails = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == HiringRequestData.Id && x.HiringRequestDetailId == genSalesHiringRequestDetail.Id).ToList();

                            model.HR_InterviewerDetails = ModelBinderATS.InterviewerDetails(HR_InterviewerDetails);

                            List<GenInterviewSlotsMaster> Interview_SlotMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(x => x.HiringRequestId == HiringRequestData.Id &&
                                                                    x.HiringRequestDetailId == genSalesHiringRequestDetail.Id &&
                                                                    x.TalentId == _Talent.Id &&
                                                                    x.ContactId == HiringRequestData.ContactId).ToList();

                            model.Interview_SlotMaster = ModelBinderATS.InterviewSlotsMaster(Interview_SlotMaster);

                            List<GenTalentSelectedInterviewDetail> SelectedTalent_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.HiringRequestId == HiringRequestData.Id &&
                                                                    x.HiringRequestDetailId == genSalesHiringRequestDetail.Id &&
                                                                    x.TalentId == _Talent.Id &&
                                                                    x.ContactId == HiringRequestData.ContactId).ToList();

                            model.SelectedTalent_InterviewDetails = ModelBinderATS.TalentSelectedInterviewDetail(SelectedTalent_InterviewDetails);

                            List<GenShortlistedTalentInterviewDetail> ShortListedTalent_InterviewDetails = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.HiringRequestId == HiringRequestData.Id &&
                                                                    x.HiringRequestDetailId == genSalesHiringRequestDetail.Id &&
                                                                    x.TalentId == _Talent.Id &&
                                                                    x.ContactId == HiringRequestData.ContactId).ToList();

                            model.ShortListedTalent_InterviewDetails = ModelBinderATS.ShortlistedTalentInterviewDetail(ShortListedTalent_InterviewDetails);

                            try
                            {
                                json = JsonConvert.SerializeObject(model);
                                ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                                aTSCall.SendInterviewDetailsWithSlot(json, loggedinuserid, HiringRequestData.Id);
                                Thread.Sleep(1000);

                                #region HR Status updates to ATS 

                                //New Status Change
                                object[] param = new object[] { HiringRequestData.Id, 0, 0, loggedinuserid, (short)AppActionDoneBy.UTS, false };
                                string strParam = CommonLogic.ConvertToParamString(param);
                                var HRStatus_Json = _commonInterface.hiringRequest.GetUpdateHrStatus(strParam);
                                if (HRStatus_Json != null)
                                {
                                    //string JsonData = Convert.ToString(HRStatus_Json);
                                    var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                    if (!string.IsNullOrEmpty(JsonData))
                                    {
                                        aTSCall.SendHrStatusToATS(JsonData, loggedinuserid, HiringRequestData.Id);
                                    }
                                }

                                #endregion

                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data save successfully" });
                            }

                            #endregion

                            string noHireReason = string.Empty;
                            if (clientFeedbackViewModel.HdnRadiovalue == "NoHire")
                            {
                                noHireReason = clientFeedbackViewModel.MessageToTalent;
                            }

                            TalentStatus = string.Empty;
                            if (TalentStatusID > 0)
                            {
                                TalentStatus = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == TalentStatusID && x.IsActive == true).FirstOrDefault()?.TalentStatus;
                            }

                            contactTalentPriorityModel.HRID = clientFeedbackViewModel.HiringRequestID;
                            contactTalentPriorityModel.HRStatusID = HiringRequestData.StatusId.Value;

                            var HRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.FirstOrDefault(x => x.Id == contactTalentPriorityModel.HRStatusID);
                            if (HRStatusData != null)
                                contactTalentPriorityModel.HRStatus = HRStatusData.HiringRequestStatus;

                            TalentDetail talentDetail = new TalentDetail();
                            talentDetail.ATS_TalentID = _Talent.AtsTalentId ?? 0;
                            talentDetail.TalentStatus = TalentStatus;
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
                                object[] param = new object[] { contactTalentPriorityModel.HRID, 0, _Talent.Id };
                                string paramString = CommonLogic.ConvertToParamString(param);

                                ATSCall aTSCallforRound = new ATSCall(_configuration, _talentConnectAdminDBContext);

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

                            contactTalentPriorityModel.TalentDetails.Add(talentDetail);

                            try
                            {
                                json = JsonConvert.SerializeObject(contactTalentPriorityModel);
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                aTSCall.SaveContactTalentPriority(json, loggedinuserid, clientFeedbackViewModel.HiringRequestID);
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data save successfully" });
                            }

                        }
                    }
                }

                //if (_configuration["HRDataSendSwitchForPHP"].ToLower() != "local")
                //{

                //    #region Add call for find and send status to ATS for HR used at other place : Added by Divya
                //    var hiringRequestData = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == clientFeedbackViewModel.HiringRequestID).FirstOrDefault();
                //    if (hiringRequestData != null)
                //    {
                //        object[] param4 = new object[] { hiringRequestData.Id, 0 };
                //        string paramasString = CommonLogic.ConvertToParamString(param4);
                //        var talentListATS = _commonInterface.hiringRequest.sproc_TalentHR_CancelledHR_List(paramasString);
                //        foreach (var list in talentListATS)
                //        {
                //            talentCancelledResponseModel = new outTalentDetailATS();
                //            talentCancelledResponseModel.HRID = list.HRID;
                //            talentCancelledResponseModel.HRStatusID = list.HRStatusID;

                //            var HRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == list.HRStatusID).FirstOrDefault();
                //            if (HRStatusData != null)
                //                talentCancelledResponseModel.HRStatus = HRStatusData.HiringRequestStatus;


                //            object[] objParam = new object[] { list.HRID, list.TalentID };
                //            string strParamas = CommonLogic.ConvertToParamString(objParam);
                //            var varTalent_RejectReason = _commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas).ActualReason;

                //            talentCancelledResponseModel.ATS_TalentID = Convert.ToInt64(list.ATS_Talent_ID);
                //            talentCancelledResponseModel.TalentStatus = list.TalentStatus;
                //            talentCancelledResponseModel.UTS_TalentID = list.TalentID;
                //            talentCancelledResponseModel.Talent_USDCost = Convert.ToInt64(list.FinalCost);
                //            talentCancelledResponseModel.Reason = list.Reason;
                //            talentCancelledResponseModel.Talent_RejectReason = varTalent_RejectReason;
                //            if (varUsrUserById != null)
                //            {
                //                talentCancelledResponseModel.RejectedBy = varUsrUserById.EmployeeId;
                //            }

                //            talentCancelledResponsesList.Add(talentCancelledResponseModel);
                //        }
                //        try
                //        {
                //            if (talentCancelledResponsesList.Count > 0)
                //            {
                //                var jsonList = JsonConvert.SerializeObject(talentCancelledResponsesList);
                //                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                //                aTSCall.SendAutoCancelledTalentHRStatusToATS(jsonList, loggedinuserid, clientFeedbackViewModel.HiringRequestID);
                //            }
                //        }
                //        catch (Exception)
                //        {
                //            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Feedback has been " + responseMessage, Details = clientFeedbackViewModel.en_Id });
                //        }
                //    }
                //    #endregion
                //}

                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Feedback has been " + responseMessage, Details = clientFeedbackViewModel.en_Id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ClientCurrentDetailsForAnotherRound")]
        public ObjectResult ClientCurrentDetailsForAnotherRound(long HiringRequestID)
        {
            try
            {
                if (HiringRequestID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HiringRequestID is in valid." });
                }

                dynamic dObject = new ExpandoObject();

                string[] strArray = { "Yes", "Append", "Add" };

                dObject.AnotherRoundInterviewOption = strArray;

                string[] strArray1 = { "Now", "Later" };

                dObject.AnotherRoundTimeSlotOption = strArray1;

                List<sproc_UTS_InterviewerDetails_Result> InterviewerDetails = _commonInterface.interview.sproc_GetCurrentInterviewerDetails(HiringRequestID.ToString());

                dObject.CurrentInterviewerDetails = InterviewerDetails;

                dObject.TimeZoneData = _talentConnectAdminDBContext.PrgContactTimeZones.Where(y => y.IsActive == true).Select(y => new MastersResponseModel
                {
                    Value = y.TimeZoneTitle,
                    Id = y.Id

                }).ToList();

                dObject.TypeOfInterview = _talentConnectAdminDBContext.PrgCompanyTypeofInterviewers.ToList().Where(x => x.IsActive == true).Select(x => new MastersResponseModel
                {
                    Id = x.Id,
                    Value = x.TypeofInterviewer
                });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = dObject });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HiringRequestID is in valid." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("SaveAnotherRoundFeedback")]
        public ObjectResult SaveAnotherRoundFeedback(ClientAnotherRoundViewModel AnotherRoundViewModel)
        {
            try
            {
                long loggedinuserid = SessionValues.LoginUserId;

                #region PreValidation
                if (AnotherRoundViewModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty, please check the datatype or naming convention", Details = AnotherRoundViewModel });
                #endregion

                #region SlotsValidation

                if (string.IsNullOrEmpty(AnotherRoundViewModel.AnotherRoundTimeSlotOption) ||
                    string.IsNullOrEmpty(AnotherRoundViewModel.AnotherRoundInterviewOption))
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseObject()
                        {
                            statusCode = StatusCodes.Status400BadRequest,
                            Message = "Please provide proper Details (AnotherRoundTimeSlotOption/AnotherRoundInterviewOption)"
                        });
                }
                else
                {
                    if (AnotherRoundViewModel.AnotherRoundTimeSlotOption == "Now" &&
                        (AnotherRoundViewModel.Timeslot == null || AnotherRoundViewModel.Timeslot.Count <= 0 ||
                        AnotherRoundViewModel.TimeZoneId == null || AnotherRoundViewModel.TimeZoneId <= 0))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseObject()
                        {
                            statusCode = StatusCodes.Status400BadRequest,
                            Message = "Please provide proper Timeslot & Timezone."
                        });
                    }
                }

                Dictionary<string, DateTime> slotsDates = new Dictionary<string, DateTime>();
                Dictionary<string, string> slotsStartTimes = new Dictionary<string, string>();
                Dictionary<string, string> slotsEndTimes = new Dictionary<string, string>();
                int cnt = 0;

                //UTS-3960: Do not allow to schedule the interview with back date. 
                var fetchEarlierDateSlots = AnotherRoundViewModel?.Timeslot?.Where(x =>
                                                                DateTime.Compare(DateTime.ParseExact(x.STRStartTime, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.Now) < 0
                                                                || DateTime.Compare(DateTime.ParseExact(x.STREndTime, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.Now) < 0).
                                                                ToList();

                if (fetchEarlierDateSlots?.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Start and End date time cannot be less than today", Details = fetchEarlierDateSlots });
                }

                foreach (var slot in AnotherRoundViewModel.Timeslot)
                {
                    cnt += 1;
                    RescheduleSlotValidator validationRules = new RescheduleSlotValidator();
                    ValidationResult validationResultForSlots = validationRules.Validate(slot);
                    if (!validationResultForSlots.IsValid)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResultForSlots.Errors, "slot") });
                    }
                    else
                    {
                        slotsDates.Add(string.Format("D{0}", cnt), CommonLogic.ConvertString2DateTime(slot.SlotDate));
                        slotsStartTimes.Add(string.Format("D{0}", cnt), slot.StartTime);
                        slotsEndTimes.Add(string.Format("D{0}", cnt), slot.EndTime);
                    }
                }
                #endregion

                #region CheckSlot is already exist in db 
                if (AnotherRoundViewModel.AnotherRoundTimeSlotOption == "Now")
                {
                    object[] param = new object[]
                    {
                            AnotherRoundViewModel.ContactID,
                            AnotherRoundViewModel.ContactID,
                            AnotherRoundViewModel.HiringDetailID,
                            AnotherRoundViewModel.HiringRequestID,
                            slotsDates["D1"].ToString("yyyy-MM-dd"),
                            slotsDates["D2"].ToString("yyyy-MM-dd"),
                            slotsDates["D3"].ToString("yyyy-MM-dd"),
                            Convert.ToString(TimeSpan.Parse(slotsStartTimes["D1"])),
                            Convert.ToString(TimeSpan.Parse(slotsStartTimes["D2"])),
                            Convert.ToString(TimeSpan.Parse(slotsStartTimes["D3"])),
                            Convert.ToString(TimeSpan.Parse(slotsEndTimes["D1"])),
                            Convert.ToString(TimeSpan.Parse(slotsEndTimes["D2"])),
                            Convert.ToString(TimeSpan.Parse(slotsEndTimes["D3"])),
                            AnotherRoundViewModel.TimeZoneId,
                            AnotherRoundViewModel.TalentID,
                            1,0
                    };

                    string paramstring = CommonLogic.ConvertToParamString(param);

                    Sproc_InsertBookSlot_CHECK_Result sproc_InsertBookSlot_Result = _commonInterface.interview.Sproc_InsertBookSlot_CHECK(paramstring);
                    if (sproc_InsertBookSlot_Result != null && !string.IsNullOrEmpty(sproc_InsertBookSlot_Result.ReturnMessage))
                    {
                        dynamic dObject = new ExpandoObject();
                        dObject.IsSlotAlreadyExist = true;

                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = sproc_InsertBookSlot_Result.ReturnMessage, Details = dObject });
                    }
                }
                #endregion

                #region Fetch Next round ID
                object[] param1 = new object[]
                    {
                        AnotherRoundViewModel.ContactID,
                        AnotherRoundViewModel.HiringRequestID,
                        AnotherRoundViewModel.HiringDetailID,
                        AnotherRoundViewModel.ShortlistInterviewerID,
                        AnotherRoundViewModel.AnotherRoundInterviewOption,
                        AnotherRoundViewModel.AnotherRoundTimeSlotOption,
                        AnotherRoundViewModel.ContactID
                    };

                string paramstring1 = CommonLogic.ConvertToParamString(param1);

                long fetchNextRoundId = _commonInterface.interview.sproc_Insert_NextInterviewRoundDetails_WithFeedbackOption(paramstring1);
                #endregion

                #region Save additional Interviewer Details
                if (AnotherRoundViewModel.interviewerDetails != null)
                {
                    for (int i = 0; i < AnotherRoundViewModel.interviewerDetails.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(AnotherRoundViewModel.interviewerDetails[i].InterviewerName) &&
                            !string.IsNullOrEmpty(AnotherRoundViewModel.interviewerDetails[i].InterviewLinkedin) &&
                            (AnotherRoundViewModel.interviewerDetails[i].InterviewerYearofExperience != 0) &&
                            (AnotherRoundViewModel.interviewerDetails[i].TypeofInterviewer > 0) &&
                            !string.IsNullOrEmpty(AnotherRoundViewModel.interviewerDetails[i].InterviewerDesignation))
                        {
                            object[] args = new object[]
                            {
                                    AnotherRoundViewModel.ContactID,
                                    AnotherRoundViewModel.HiringRequestID,
                                    AnotherRoundViewModel.HiringDetailID,
                                    AnotherRoundViewModel.interviewerDetails[i].InterviewerName,
                                    AnotherRoundViewModel.interviewerDetails[i].InterviewLinkedin,
                                    AnotherRoundViewModel.interviewerDetails[i].InterviewerYearofExperience,
                                    AnotherRoundViewModel.interviewerDetails[i].TypeofInterviewer,
                                    AnotherRoundViewModel.interviewerDetails[i].InterviewerDesignation,
                                    AnotherRoundViewModel.interviewerDetails[i].InterviewerEmailID,
                                    AnotherRoundViewModel.ContactID
                            };
                            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Insert_NextRound_InterviewerDetails, args);
                        }
                    }
                }
                #endregion

                //Save AnotherRoundTimeSlot with Now option
                if (AnotherRoundViewModel.AnotherRoundTimeSlotOption == "Now")
                {
                    long? slotid = 0;
                    object[] param = new object[]
                    {
                            AnotherRoundViewModel.ContactID,
                            AnotherRoundViewModel.ContactID,
                            AnotherRoundViewModel.HiringDetailID,
                            AnotherRoundViewModel.HiringRequestID,
                            slotsDates["D1"].ToString("yyyy-MM-dd"),
                            slotsDates["D2"].ToString("yyyy-MM-dd"),
                            slotsDates["D3"].ToString("yyyy-MM-dd"),
                            Convert.ToString(TimeSpan.Parse(slotsStartTimes["D1"])),
                            Convert.ToString(TimeSpan.Parse(slotsStartTimes["D2"])),
                            Convert.ToString(TimeSpan.Parse(slotsStartTimes["D3"])),
                            Convert.ToString(TimeSpan.Parse(slotsEndTimes["D1"])),
                            Convert.ToString(TimeSpan.Parse(slotsEndTimes["D2"])),
                            Convert.ToString(TimeSpan.Parse(slotsEndTimes["D3"])),
                            AnotherRoundViewModel.TimeZoneId,
                            AnotherRoundViewModel.TalentID,
                            1,fetchNextRoundId, string.Empty, string.Empty
                    };

                    string paramstring = CommonLogic.ConvertToParamString(param);

                    Sproc_InsertBookSlot_Result sproc_InsertBookSlot_Result = _commonInterface.interview.Sproc_InsertBookSlot(paramstring);
                    if (sproc_InsertBookSlot_Result != null && sproc_InsertBookSlot_Result.ID > 0)
                    {
                        slotid = sproc_InsertBookSlot_Result.ID;
                    }
                    //Update Interview Status slot given ,Talent status and role status as in interview.
                    if (slotid > 0)
                    {
                        var encryptedHRID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(AnotherRoundViewModel.HiringRequestID)));
                        var encryptedHRDetailID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(AnotherRoundViewModel.HiringDetailID)));
                        var encryptedContactID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(AnotherRoundViewModel.ContactID)));
                        var encryptedAnotherRound_Id = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(slotid)));

                        object[] args = new object[] { slotid, "Now", AnotherRoundViewModel.ContactID };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_UpdateInterviewStatus_WithFeedback_SlotBookNow, args);

                        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);

                        #region Send email to Client
                        //GenContact _Contact = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == AnotherRoundViewModel.ContactID);

                        //if (_Contact != null && _Contact.IsClientNotificationSend)
                        //{
                        //    sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact = _commonInterface.interview.sproc_Get_ContactPointofContact(AnotherRoundViewModel.ContactID.ToString());
                        //    emailBinder.SendEmailNotificationToClientForSecondRound(AnotherRoundViewModel.TalentID, AnotherRoundViewModel.ContactID, AnotherRoundViewModel.HiringRequestID, sproc_Get_ContactPointofContact);
                        //}
                        #endregion

                        #region Send email to Talent
                        //GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == AnotherRoundViewModel.TalentID);
                        //if (_Talent != null)
                        //{
                        //    var NotifyToTalent = _Talent.IsTalentNotificationSend ?? false;
                        //    if (NotifyToTalent)
                        //        emailBinder.SendEmailNotificationToTalentForSecondRound(AnotherRoundViewModel.TalentID, AnotherRoundViewModel.ContactID, AnotherRoundViewModel.HiringRequestID, AnotherRoundViewModel.HiringDetailID, encryptedHRID, encryptedHRDetailID, encryptedContactID, encryptedAnotherRound_Id);
                        //}
                        #endregion

                        #region send email to team
                        emailBinder.SendEmailNotificationToInternalTeamForSecondRound(AnotherRoundViewModel.TalentID, AnotherRoundViewModel.ContactID, AnotherRoundViewModel.HiringRequestID, AnotherRoundViewModel.HiringDetailID);
                        #endregion

                        if (_configuration["HRDataSendSwitchForPHP"].ToLower() != "local")
                        {
                            #region Send Data to ATS via API Call.

                            string json = "";
                            ATSInterviewDetailViewModel model = new ATSInterviewDetailViewModel();

                            model.ATS_TalentID = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == AnotherRoundViewModel.TalentID).FirstOrDefault().AtsTalentId ?? 0;
                            model.UTS_TalentID = AnotherRoundViewModel.TalentID;
                            model.UTS_HiringRequestID = AnotherRoundViewModel.HiringRequestID;

                            List<GenSalesHiringRequestInterviewerDetail> HR_InterviewerDetails = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == AnotherRoundViewModel.HiringRequestID && x.HiringRequestDetailId == AnotherRoundViewModel.HiringDetailID).ToList();
                            model.HR_InterviewerDetails = ModelBinderATS.InterviewerDetails(HR_InterviewerDetails);

                            List<GenInterviewSlotsMaster> Interview_SlotMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(x => x.HiringRequestId == AnotherRoundViewModel.HiringRequestID &&
                                                                    x.HiringRequestDetailId == AnotherRoundViewModel.HiringDetailID &&
                                                                    x.TalentId == AnotherRoundViewModel.TalentID &&
                                                                    x.ContactId == AnotherRoundViewModel.ContactID).ToList();
                            model.Interview_SlotMaster = ModelBinderATS.InterviewSlotsMaster(Interview_SlotMaster);

                            List<GenTalentSelectedInterviewDetail> SelectedTalent_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.HiringRequestId == AnotherRoundViewModel.HiringRequestID &&
                                                                    x.HiringRequestDetailId == AnotherRoundViewModel.HiringDetailID &&
                                                                    x.TalentId == AnotherRoundViewModel.TalentID &&
                                                                    x.ContactId == AnotherRoundViewModel.ContactID).ToList();
                            model.SelectedTalent_InterviewDetails = ModelBinderATS.TalentSelectedInterviewDetail(SelectedTalent_InterviewDetails);


                            List<GenShortlistedTalentInterviewDetail> ShortListedTalent_InterviewDetails = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.HiringRequestId == AnotherRoundViewModel.HiringRequestID &&
                                                                    x.HiringRequestDetailId == AnotherRoundViewModel.HiringDetailID &&
                                                                    x.TalentId == AnotherRoundViewModel.TalentID &&
                                                                    x.ContactId == AnotherRoundViewModel.ContactID).ToList();
                            model.ShortListedTalent_InterviewDetails = ModelBinderATS.ShortlistedTalentInterviewDetail(ShortListedTalent_InterviewDetails);

                            try
                            {
                                json = JsonConvert.SerializeObject(model);
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                aTSCall.SendInterviewDetailsWithSlot(json, loggedinuserid, AnotherRoundViewModel.HiringRequestID);
                                Thread.Sleep(1000);
                                aTSCall.CallAtsAPIToSendTalentAndHRStatus(model.UTS_TalentID, loggedinuserid, AnotherRoundViewModel.HiringRequestID);
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = sproc_InsertBookSlot_Result.ReturnMessage });
                    }
                }
                else
                {
                    var encryptedHRID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(0)));
                    var encryptedRoleID = UrlEncoder.Default.Encode(CommonLogic.Encrypt(Convert.ToString(0)));

                    EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);

                    //GenContact _Contact = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == AnotherRoundViewModel.ContactID);

                    //if (_Contact != null && _Contact.IsClientNotificationSend)
                    //{
                    //    sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact = _commonInterface.interview.sproc_Get_ContactPointofContact(AnotherRoundViewModel.ContactID.ToString());
                    //    emailBinder.SendEmailNotificationToClientForSecondRoundLater(AnotherRoundViewModel.TalentID, AnotherRoundViewModel.ContactID, AnotherRoundViewModel.HiringRequestID, AnotherRoundViewModel.HiringDetailID, encryptedHRID, encryptedRoleID, sproc_Get_ContactPointofContact);
                    //}

                    //GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == AnotherRoundViewModel.TalentID);

                    //if (_Talent != null)
                    //{
                    //    var NotifyToTalent = _Talent.IsTalentNotificationSend ?? false;
                    //    if (NotifyToTalent)
                    //        emailBinder.SendEmailNotificationToTalentForSecondRoundLater(AnotherRoundViewModel.TalentID, AnotherRoundViewModel.ContactID, AnotherRoundViewModel.HiringRequestID, AnotherRoundViewModel.HiringDetailID);
                    //}
                    emailBinder.SendEmailNotificationToInternalTeamForSecondRoundLater(AnotherRoundViewModel.TalentID, AnotherRoundViewModel.ContactID, AnotherRoundViewModel.HiringRequestID, AnotherRoundViewModel.HiringDetailID);
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetInterviewStatusDetail")]
        public ObjectResult GetInterviewStatusDetail(int SelectedInterviewId)
        {
            try
            {
                GenTalentSelectedInterviewDetail TalentSelectedInterviewDetails = null;
                if (SelectedInterviewId == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide interview ID" });
                else
                {
                    TalentSelectedInterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(m => m.Id == SelectedInterviewId).FirstOrDefault();
                    if (TalentSelectedInterviewDetails == null)
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Interview details not exist" });
                }

                dynamic dObject = new ExpandoObject();

                var CheckSlotConfirmedOrNot = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.Id == SelectedInterviewId).Where(x => x.IsConfirmed == true && x.ShortlistedInterviewId > 0).FirstOrDefault();
                if (CheckSlotConfirmedOrNot == null)
                {
                    // If slot is not confirmed, Only "Cancelled" status will be shown in Dropdown
                    dObject.HiringStatus = _talentConnectAdminDBContext.PrgInterviewStatuses.ToList()
                        .Where(x => x.InterviewStatus == "Cancelled").Select(x => new MastersResponseModel
                        {
                            Value = x.InterviewStatus,
                            Id = x.Id
                        });
                }
                else
                {
                    // After slot Confirmation, All status will be shown in Dropdown
                    dObject.HiringStatus = _talentConnectAdminDBContext.PrgInterviewStatuses.ToList()
                        .Where(x => x.Id == 3 || x.Id == 6)//Cancelled && Interview Completed
                        .Select(x => new MastersResponseModel
                        {
                            Value = x.InterviewStatus,
                            Id = x.Id
                        });
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("UpdateInterviewStatus")]
        public ObjectResult UpdateStatusInTalentSelectedInterviewDetail(UpdateInterviewStatusModel updateInterviewStatus)
        {
            try
            {
                GenTalentSelectedInterviewDetail InterviewDetails = null;
                GenSalesHiringRequest _SalesHiringRequest = null;
                ContactTalentPriorityModel contactTalentPriorityResponseModel = new();
                GenSalesHiringRequestDetail genSalesHiringRequestDetail = new();
                string json = "";

                #region PreValidation
                if (updateInterviewStatus.HrID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide HR Id." });
                }
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == updateInterviewStatus.HrID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR does not exists." });
                    else
                    {
                        genSalesHiringRequestDetail = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.FirstOrDefault(x => x.HiringRequestId == _SalesHiringRequest.Id);
                        if (genSalesHiringRequestDetail == null)
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Details does not exists." });
                    }
                }
                if (updateInterviewStatus.InterviewStatusID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Select Interview Status." });
                }
                if (updateInterviewStatus.TalentSelectedInterviewDetailsID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Selected Interview Detail Id." });
                }
                else
                {
                    InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(xy => xy.Id == updateInterviewStatus.TalentSelectedInterviewDetailsID).FirstOrDefault();
                    if (InterviewDetails == null)
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Interview Detail does not exists." });
                }
                #endregion

                var LoggedInUserId = SessionValues.LoginUserId;
                var varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(SessionValues.LoginUserId);

                GenTalent _Talent = new GenTalent();

                //Update status Id in GenTalentSelectedInterviewDetails
                if (updateInterviewStatus.TalentSelectedInterviewDetailsID > 0)
                {
                    InterviewDetails.Id = (long)updateInterviewStatus.TalentSelectedInterviewDetailsID;
                    InterviewDetails.StatusId = updateInterviewStatus.InterviewStatusID;
                    _talentConnectAdminDBContext.Entry(InterviewDetails).State = EntityState.Modified;
                    _talentConnectAdminDBContext.SaveChanges();
                }

                var ContactId = InterviewDetails.ContactId;
                var TalentId = InterviewDetails.TalentId;

                #region commented code, email send to talent & client
                //if (updateInterviewStatus.InterviewStatusID == (short)prg_InterviewStatus.Cancelled)
                //{
                //    GenContact _Contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ContactId).FirstOrDefault();

                //    if (_Contact != null && _Contact.IsClientNotificationSend)
                //    {
                //        #region SendEmail to client
                //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                //        bool EmailSent = emailBinder.SendEmailNotificationwithZoomDetails_Cancel((long)TalentId, (long)updateInterviewStatus.HrID, (long)updateInterviewStatus.TalentSelectedInterviewDetailsID, InterviewDetails.ZoomMeetingscheduledOn ?? DateTime.MinValue, 0);
                //        #endregion
                //    }

                //    _Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == TalentId).FirstOrDefault();

                //    if (_Talent != null)
                //    {
                //        var NotifyToTalent = _Talent.IsTalentNotificationSend ?? false;
                //        if (NotifyToTalent)
                //        {
                //            #region SendEmail to Talent
                //            EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                //            bool EmailSent = emailBinder.SendEmailNotificationwithZoomDetails_Cancel((long)TalentId, (long)updateInterviewStatus.HrID, (long)updateInterviewStatus.TalentSelectedInterviewDetailsID, InterviewDetails.ZoomMeetingscheduledOn ?? DateTime.MinValue, InterviewDetails.ShortlistedInterviewId ?? 0);
                //            #endregion
                //        }
                //    }
                //}

                //Email when status changed to Interview Completed
                //if (updateInterviewStatus.InterviewStatusID == (short)prg_InterviewStatus.Interview_Completed)
                //{
                //    GenTalentSelectedInterviewDetail objtalentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.Id == updateInterviewStatus.TalentSelectedInterviewDetailsID).FirstOrDefault();
                //    if (objtalentSelected_InterviewDetails != null)
                //    {
                //        string EmailClient = "";
                //        string EmailTalent = "";
                //        //commonEmailController commonEmailController = new CommonEmailController();
                //        if (!objtalentSelected_InterviewDetails.IsClientFeedbackSubmitted ?? true)
                //        {
                //            var Contact_ID = InterviewDetails.ContactId ?? 0;
                //            GenContact _Contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == Contact_ID).FirstOrDefault();

                //            if (_Contact != null && _Contact.IsClientNotificationSend)
                //            {
                //                #region SendEmail to client
                //                EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                //                bool EmailSent = emailBinder.SendEmailAfterInterview_Completed((long)TalentId, (long)updateInterviewStatus.HrID, true);
                //                #endregion
                //            }
                //        }
                //        if (!objtalentSelected_InterviewDetails.IsFeedbackSubmitted ?? true)
                //        {
                //            _Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == TalentId).FirstOrDefault();

                //            if (_Talent != null)
                //            {
                //                var NotifyToTalent = _Talent.IsTalentNotificationSend ?? false;
                //                if (NotifyToTalent)
                //                {
                //                    #region SendEmail to Talent
                //                    EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                //                    bool EmailSent = emailBinder.SendEmailAfterInterview_Completed((long)TalentId, (long)updateInterviewStatus.HrID, false);
                //                    #endregion
                //                }
                //            }
                //        }

                //        //Removing EmailTalent Success check as we are not going to send any Emails to Talent from UTS.
                //        //So mark IsInterviewCompletedEmailSent based on EmailClient Success.
                //        //Changes by Siddharth Jain (Dtd: 31/May/2023)
                //        if (EmailClient == "success")
                //        {
                //            objtalentSelected_InterviewDetails.IsInterviewCompletedEmailSent = true;
                //            _talentConnectAdminDBContext.Entry(objtalentSelected_InterviewDetails).State = EntityState.Modified;
                //            _talentConnectAdminDBContext.SaveChanges();
                //        }
                //    }
                //}
                #endregion

                //Update Status Id in Master Interview Slot table                 
                if (updateInterviewStatus.InterviewMasterID > 0)
                {
                    GenInterviewSlotsMaster InterviewSlotsMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(xy => xy.Id == updateInterviewStatus.InterviewMasterID).FirstOrDefault();
                    if (InterviewSlotsMaster != null)
                    {

                        InterviewSlotsMaster.InterviewStatusId = updateInterviewStatus.InterviewStatusID;
                        InterviewSlotsMaster.LastModifiedById = (int)LoggedInUserId;
                        InterviewSlotsMaster.LastModifiedDatetime = DateTime.Now;
                        _talentConnectAdminDBContext.Entry(InterviewSlotsMaster).State = EntityState.Modified;
                        _talentConnectAdminDBContext.SaveChanges();

                        List<GenShortlistedTalentInterviewDetail> genShortlistedTalentInterviewDetails = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(xy => xy.InterviewMasterId == updateInterviewStatus.InterviewMasterID).ToList();
                        if (genShortlistedTalentInterviewDetails.Count > 0)
                        {
                            foreach (var std in genShortlistedTalentInterviewDetails)
                            {
                                if (InterviewSlotsMaster.InterviewStatusId == 6)
                                {
                                    std.StatusId = 6;
                                    std.LastModifiedById = (int)LoggedInUserId;
                                    std.LastModifiedDatetime = DateTime.Now;
                                    _talentConnectAdminDBContext.Entry(std).State = EntityState.Modified;
                                    _talentConnectAdminDBContext.SaveChanges();
                                }
                            }
                        }
                    }
                }

                //UTS-8568, no need to go back in funnel
                //AfterInterviewCancel_TalentAsProfileShared
                //if (updateInterviewStatus.InterviewStatusID == (short)prg_InterviewStatus.Cancelled)
                //{
                //    object[] args = new object[] { InterviewDetails.HiringRequestId, InterviewDetails.TalentId, LoggedInUserId };
                //    _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_AfterInterviewCancel_TalentAsProfileShared, args);
                //}

                #region Insert HR History
                string action = string.Empty;

                switch (updateInterviewStatus.InterviewStatusID)
                {
                    case (short)prg_InterviewStatus.Slot_Given:
                        action = Action_Of_History.Slot_Given.ToString();
                        break;
                    case (short)prg_InterviewStatus.Cancelled:
                        action = Action_Of_History.Cancelled.ToString();
                        break;
                    case (short)prg_InterviewStatus.Interview_Scheduled:
                        action = Action_Of_History.Interview_Scheduled.ToString();
                        break;
                    case (short)prg_InterviewStatus.Interview_in_Process:
                        action = Action_Of_History.Interview_in_Process.ToString();
                        break;
                    case (short)prg_InterviewStatus.Interview_Completed:
                        action = Action_Of_History.Interview_Completed.ToString();
                        break;
                    case (short)prg_InterviewStatus.Feedback_Submitted:
                        action = Action_Of_History.Feedback_Submitted_With_Hire.ToString();
                        break;
                }
                if (!string.IsNullOrEmpty(action))
                {
                    object[] args = new object[] { action, InterviewDetails.HiringRequestId, InterviewDetails.TalentId, false, LoggedInUserId, 0, InterviewDetails.InterviewMasterId, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                }
                #endregion

                #region ATS Call
                if (_configuration["HRDataSendSwitchForPHP"].ToLower() != "local")
                {
                    _talentConnectAdminDBContext.Entry(_Talent).Reload();
                    _Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == InterviewDetails.TalentId).FirstOrDefault();

                    if (_Talent != null)
                    {
                        var HiringRequestData = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == InterviewDetails.HiringRequestId).FirstOrDefault();
                        if (HiringRequestData != null)
                        {

                            #region Send Interview Data to ATS via API Call.

                            ATSInterviewDetailViewModel model = new();
                            model.ATS_TalentID = _Talent.AtsTalentId ?? 0;
                            model.UTS_TalentID = _Talent.Id;
                            model.UTS_HiringRequestID = HiringRequestData.Id;

                            List<GenSalesHiringRequestInterviewerDetail> HR_InterviewerDetails = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == HiringRequestData.Id && x.HiringRequestDetailId == genSalesHiringRequestDetail.Id).ToList();

                            model.HR_InterviewerDetails = ModelBinderATS.InterviewerDetails(HR_InterviewerDetails);

                            List<GenInterviewSlotsMaster> Interview_SlotMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(x => x.HiringRequestId == HiringRequestData.Id &&
                                                                    x.HiringRequestDetailId == genSalesHiringRequestDetail.Id &&
                                                                    x.TalentId == _Talent.Id &&
                                                                    x.ContactId == HiringRequestData.ContactId).ToList();

                            model.Interview_SlotMaster = ModelBinderATS.InterviewSlotsMaster(Interview_SlotMaster);

                            List<GenTalentSelectedInterviewDetail> SelectedTalent_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.HiringRequestId == HiringRequestData.Id &&
                                                                    x.HiringRequestDetailId == genSalesHiringRequestDetail.Id &&
                                                                    x.TalentId == _Talent.Id &&
                                                                    x.ContactId == HiringRequestData.ContactId).ToList();

                            model.SelectedTalent_InterviewDetails = ModelBinderATS.TalentSelectedInterviewDetail(SelectedTalent_InterviewDetails);

                            List<GenShortlistedTalentInterviewDetail> ShortListedTalent_InterviewDetails = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.HiringRequestId == HiringRequestData.Id &&
                                                                    x.HiringRequestDetailId == genSalesHiringRequestDetail.Id &&
                                                                    x.TalentId == _Talent.Id &&
                                                                    x.ContactId == HiringRequestData.ContactId).ToList();

                            model.ShortListedTalent_InterviewDetails = ModelBinderATS.ShortlistedTalentInterviewDetail(ShortListedTalent_InterviewDetails);

                            try
                            {
                                json = JsonConvert.SerializeObject(model);
                                ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                                aTSCall.SendInterviewDetailsWithSlot(json, LoggedInUserId, HiringRequestData.Id);
                                Thread.Sleep(1000);
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Interview Status updated Successfully." });
                            }

                            #endregion

                            contactTalentPriorityResponseModel.HRID = HiringRequestData.Id;
                            contactTalentPriorityResponseModel.HRStatusID = HiringRequestData.StatusId ?? 0;

                            //UTS-3183: Send correct talent status 
                            #region Save Data in model to send reponse to PHP team after serialize  

                            var HRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == HiringRequestData.StatusId).FirstOrDefault();
                            if (HRStatusData != null)
                            {
                                contactTalentPriorityResponseModel.HRStatus = HRStatusData.HiringRequestStatus;
                            }

                            TalentDetail talentDetail = new TalentDetail();
                            talentDetail.ATS_TalentID = _Talent.AtsTalentId ?? 0;

                            var TalentCTP_Details = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == _Talent.Id && x.HiringRequestId == HiringRequestData.Id).FirstOrDefault();
                            if (TalentCTP_Details != null)
                            {
                                var TalStatusClientSelectionDetail = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == TalentCTP_Details.TalentStatusIdBasedOnHr).FirstOrDefault();
                                if (TalStatusClientSelectionDetail != null)
                                {
                                    talentDetail.TalentStatus = TalStatusClientSelectionDetail.TalentStatus;
                                }
                                else
                                    talentDetail.TalentStatus = "";
                            }
                            else
                                talentDetail.TalentStatus = "";

                            talentDetail.UTS_TalentID = _Talent.Id;
                            talentDetail.Talent_USDCost = _Talent.FinalCost ?? 0;

                            object[] objParam = new object[] { contactTalentPriorityResponseModel.HRID, _Talent.Id };
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
                                object[] atsParam = new object[] { contactTalentPriorityResponseModel.HRID, 0, _Talent.Id };
                                string paramString = CommonLogic.ConvertToParamString(atsParam);

                                ATSCall aTSCallforRound = new ATSCall(_configuration, _talentConnectAdminDBContext);

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
                                json = JsonConvert.SerializeObject(contactTalentPriorityResponseModel);
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                aTSCall.SaveContactTalentPriority(json, LoggedInUserId, HiringRequestData.Id);
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Interview Status updated Successfully." });
                            }
                        }
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Interview Status updated Successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Unexpected error occurs" });
            }
        }

        [Authorize]
        [HttpGet("GetExistingSlotDetails")]
        public ObjectResult GetExistingSlotDetails(long? InterviewMasterID)
        {
            try
            {
                GenInterviewSlotsMaster interviewSlotMasterData = null;
                dynamic dObject = new ExpandoObject();

                #region PreValidation
                if (InterviewMasterID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Selected Interview Id." });
                }
                else
                {
                    interviewSlotMasterData = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(x => x.Id == InterviewMasterID).FirstOrDefault();
                    if (interviewSlotMasterData == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Interview not exist" });
                    }
                }
                #endregion

                GenTalent talentDetail = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == interviewSlotMasterData.TalentId).FirstOrDefault();
                dObject.TalentName = talentDetail != null ? talentDetail.Name : "";
                dObject.InterviewStatusId = interviewSlotMasterData.InterviewStatusId;
                dObject.InterviewStatusName = _talentConnectAdminDBContext.PrgInterviewStatuses.Where(x => x.Id == interviewSlotMasterData.InterviewStatusId).FirstOrDefault().InterviewStatus;
                dObject.InterviewRound = interviewSlotMasterData.InterviewRoundStr;
                dObject.InterviewRoundCount = interviewSlotMasterData.InterviewRoundCount;

                #region SlotDetails

                List<RescheduleSlot> slots = new List<RescheduleSlot>();
                var slotsData = new List<GenShortlistedTalentInterviewDetail>();

                slotsData = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.InterviewMasterId == interviewSlotMasterData.Id).ToList();

                if (slotsData != null && slotsData.Count > 0)
                {
                    dObject.timeZoneID = slotsData[0].TimeZoneId != null ? slotsData[0].TimeZoneId : 0;
                    foreach (var str in slotsData)
                    {
                        TimeSpan tsStartTime = TimeSpan.Parse(str.InterviewStartTime.Value.ToString());
                        TimeSpan tsEndTime = TimeSpan.Parse(str.InterviewEndTime.Value.ToString());
                        slots.Add(new RescheduleSlot()
                        {
                            SlotID = slots.Count() + 1,
                            STRSlotDate = str.ScheduledOn.Value.ToString("MM/dd/yyyy"),
                            STRStartTime = tsStartTime.ToString(@"hh\:mm"),
                            STREndTime = tsEndTime.ToString(@"hh\:mm"),
                            SlotDate = str.ScheduledOn.Value.ToString("MM/dd/yyyy"),
                            ID_As_ShortListedID = str.Id.ToString()
                        });
                    }
                    dObject.Slots = slots;
                }

                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("SaveConfirmInterviewSlot")]
        public ObjectResult SaveConfirmInterviewSlot(ConfirmInterviewSlotModel confirmInterviewSlot)
        {
            try
            {
                if (confirmInterviewSlot == null ||
                    confirmInterviewSlot.HiringRequest_ID == 0 ||
                    confirmInterviewSlot.HiringRequest_Detail_ID == 0 ||
                    confirmInterviewSlot.ContactID == 0 ||
                    confirmInterviewSlot.Talent_ID == 0 ||
                    confirmInterviewSlot.InterviewMasterID == 0 ||
                    confirmInterviewSlot.ShortListedID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide proper data" });
                }

                var LoggedInUserId = SessionValues.LoginUserId;

                GenTalentSelectedInterviewDetail TalentSelected =
                    _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails
                    .Where(x => x.HiringRequestId == confirmInterviewSlot.HiringRequest_ID &&
                    x.HiringRequestDetailId == confirmInterviewSlot.HiringRequest_Detail_ID &&
                    x.ContactId == confirmInterviewSlot.ContactID &&
                    x.TalentId == confirmInterviewSlot.Talent_ID &&
                    x.InterviewMasterId == confirmInterviewSlot.InterviewMasterID).FirstOrDefault();

                long shortListedId = confirmInterviewSlot.ShortListedID;

                GenShortlistedTalentInterviewDetail interviewShortlisted = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(y => y.Id == shortListedId).FirstOrDefault();

                if (interviewShortlisted != null && interviewShortlisted.TimeZoneId > 0 && TalentSelected != null)
                {
                    var scheduleTime = interviewShortlisted.ScheduledOn.Value.Add(interviewShortlisted.InterviewStartTime.Value);
                    var scheduleTillTime = interviewShortlisted.ScheduledOn.Value.Add(interviewShortlisted.InterviewEndTime.Value);

                    PrgContactTimeZone timeZone = _talentConnectAdminDBContext.PrgContactTimeZones.Where(y => y.Id == interviewShortlisted.TimeZoneId.Value).FirstOrDefault();

                    Meeting meeting = new Meeting();
                    if (timeZone != null)
                    {
                        string googleMeetUrl = _configuration["GoogleMeetPythonURL"].ToString();
                        meeting = ExternalCalls.ToScheduleZoomMeeting(timeZone.Description, scheduleTime, googleMeetUrl);
                        if (meeting == null)
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Meeting Link is not initialized, Kindly contact Administrator" });
                    }

                    if (meeting != null)
                    {
                        #region confirm slot 
                        TalentSelected.ShortlistedInterviewId = shortListedId;
                        TalentSelected.Id = TalentSelected.Id;
                        TalentSelected.StatusId = 4;
                        TalentSelected.IsConfirmed = true;

                        TalentSelected.ZoomInterviewLink = meeting.join_url ?? string.Empty;
                        TalentSelected.ZoomMeetingId = Convert.ToString(meeting.id);
                        TalentSelected.ZoomMeetingscheduledOn = DateTime.Now;
                        TalentSelected.ZoomInterviewKitUsername = meeting.host_email ?? string.Empty;
                        TalentSelected.ZoomInterviewKitPassword = meeting.password ?? string.Empty;

                        _talentConnectAdminDBContext.Entry(TalentSelected).State = EntityState.Modified;
                        _talentConnectAdminDBContext.SaveChanges();

                        confirmInterviewSlot.InterviewMasterID = Convert.ToInt32(interviewShortlisted.InterviewMasterId);

                        UpdateStatusAfterInterviewSchedule(confirmInterviewSlot, Convert.ToInt32(LoggedInUserId));

                        _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Update_TalentSelected, new object[] { TalentSelected.Id, Convert.ToInt32(LoggedInUserId), shortListedId, meeting.join_url ?? string.Empty, Convert.ToString(meeting.id), DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"), meeting.host_email ?? string.Empty, meeting.password ?? string.Empty, "" });

                        #endregion

                        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);

                        #region email send if meeting is generated
                        if (TalentSelected.InterviewRound == 1)
                        {
                            emailBinder.SendEmailNotificationToInternalTeamwithZoomDetails_Schedule(meeting, 2, meeting.join_url, confirmInterviewSlot.Talent_ID, confirmInterviewSlot.ContactID, scheduleTime, timeZone.ShortName, confirmInterviewSlot.HiringRequest_ID, scheduleTillTime, timeZone, TalentSelected.ShortlistedInterviewId.Value);

                            #region commneted - unused code talent & client emails

                            //GenContact _Contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == confirmInterviewSlot.ContactID).FirstOrDefault();
                            //var IsClientNotificationSent = false;

                            //Object[] param = new object[] { confirmInterviewSlot.ContactID };
                            //string strParam = CommonLogic.ConvertToParamString(param);

                            //sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact = _commonInterface.interview.sproc_Get_ContactPointofContact(strParam);

                            //if (_Contact != null && _Contact.IsClientNotificationSend)
                            //{
                            //    EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                            //    emailBinder.SendEmailNotificationToClientwithZoomDetails_Schedule(meeting, 0, "", confirmInterviewSlot.Talent_ID, confirmInterviewSlot.ContactID, scheduleTime, timeZone.ShortName, confirmInterviewSlot.HiringRequest_ID, scheduleTillTime, timeZone, sproc_Get_ContactPointofContact, 0);
                            //}

                            //var Talent_ID = confirmInterviewSlot.Talent_ID;
                            //GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == confirmInterviewSlot.Talent_ID).FirstOrDefault();

                            //if (_Talent != null)
                            //{
                            //    var NotifyToTalent = _Talent.IsTalentNotificationSend ?? false;
                            //    if (NotifyToTalent)
                            //    {
                            //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                            //        emailBinder.SendEmailNotificationToTalentwithZoomDetails_Schedule(meeting, 0, "", confirmInterviewSlot.Talent_ID, confirmInterviewSlot.ContactID, scheduleTime, timeZone.ShortName, confirmInterviewSlot.HiringRequest_ID, scheduleTillTime, timeZone, sproc_Get_ContactPointofContact, TalentSelected.ShortlistedInterviewId.Value);
                            //    }
                            //}

                            //Send Notification to Interviewers
                            //GenSalesHiringRequestDetail salesHiringRequest_Details = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(x => x.HiringRequestId == confirmInterviewSlot.HiringRequest_ID).FirstOrDefault();

                            //if (salesHiringRequest_Details != null)
                            //{
                            //    //Send Notification to Interviewers only when email Id is different from client Email Id.
                            //    GenContact objContact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == confirmInterviewSlot.ContactID).FirstOrDefault();

                            //    List<GenSalesHiringRequestInterviewerDetail> salesHiringRequest_InterviewerDetails = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == confirmInterviewSlot.HiringRequest_ID).ToList();
                            //    foreach (var item in salesHiringRequest_InterviewerDetails)
                            //    {
                            //        if (item.InterviewerEmailId != "")
                            //        {
                            //            if (salesHiringRequest_Details.InterviewerEmailId != objContact.EmailId)
                            //            {
                            //                EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                            //                emailBinder.SendEmailNotificationToInterviewerwithZoomDetails(meeting, confirmInterviewSlot.Talent_ID, confirmInterviewSlot.ContactID, item.InterviewerEmailId, item.InterviewerName, scheduleTime, timeZone.ShortName, confirmInterviewSlot.HiringRequest_ID, scheduleTillTime, timeZone, shortListedId);
                            //            }
                            //        }
                            //    }
                            //}

                            #endregion
                        }
                        else
                        {
                            emailBinder.SendEmailNotificationForInternalForSecondRound(confirmInterviewSlot.HiringRequest_ID, meeting, confirmInterviewSlot.Talent_ID, confirmInterviewSlot.ContactID, scheduleTime, timeZone.ShortName, scheduleTillTime, timeZone);

                            #region commneted - unused code talent & client emails

                            //GenContact _Contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == confirmInterviewSlot.ContactID).FirstOrDefault();

                            //if (_Contact != null && _Contact.IsClientNotificationSend)
                            //{
                            //    emailBinder.SendEmailNotificationToClientwithZoomDetailsForSecondRound(meeting, confirmInterviewSlot.Talent_ID, confirmInterviewSlot.ContactID, scheduleTime, timeZone.ShortName, confirmInterviewSlot.HiringRequest_ID, scheduleTillTime, timeZone);
                            //}

                            //var Talent_ID = confirmInterviewSlot.Talent_ID;
                            //GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == Talent_ID).FirstOrDefault();

                            //if (_Talent != null)
                            //{
                            //    var NotifyToTalent = _Talent.IsTalentNotificationSend ?? false;
                            //    if (NotifyToTalent)
                            //        emailBinder.SendEmailNotificationToTalentwithZoomDetailsForSecondRound(meeting, confirmInterviewSlot.Talent_ID, confirmInterviewSlot.ContactID, scheduleTime, timeZone.ShortName, confirmInterviewSlot.HiringRequest_ID, scheduleTillTime, timeZone);
                            //}

                            //Send Notification to Interviewers
                            //GenSalesHiringRequestDetail salesHiringRequest_Details = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(x => x.HiringRequestId == confirmInterviewSlot.HiringRequest_ID).FirstOrDefault();

                            //if (salesHiringRequest_Details != null)
                            //{
                            //    //Send Notification to Interviewers only when email Id is different from client Email Id.
                            //    GenContact objContact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == confirmInterviewSlot.ContactID).FirstOrDefault();

                            //    List<GenSalesHiringRequestInterviewerDetail> salesHiringRequest_InterviewerDetails = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == confirmInterviewSlot.HiringRequest_ID).ToList();
                            //    foreach (var item in salesHiringRequest_InterviewerDetails)
                            //    {
                            //        if (item.InterviewerEmailId != "")
                            //        {
                            //            if (salesHiringRequest_Details.InterviewerEmailId != objContact.EmailId)
                            //            {
                            //                emailBinder.SendEmailNotificationToInterviewerwithZoomDetailsForSecondRound(meeting, confirmInterviewSlot.Talent_ID, confirmInterviewSlot.ContactID, item.InterviewerEmailId, item.InterviewerName, scheduleTime, timeZone.ShortName, confirmInterviewSlot.HiringRequest_ID, scheduleTillTime, timeZone);
                            //            }
                            //        }
                            //    }
                            //}

                            #endregion
                        }
                        #endregion

                        #region ATS call
                        if (_configuration["HRDataSendSwitchForPHP"].ToLower() != "local")
                        {
                            #region Send Data to ATS via API Call.

                            string json = "";

                            ATSInterviewDetailViewModel model = new ATSInterviewDetailViewModel();
                            model.ATS_TalentID = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == confirmInterviewSlot.Talent_ID).FirstOrDefault().AtsTalentId ?? 0;
                            model.UTS_TalentID = confirmInterviewSlot.Talent_ID;
                            model.UTS_HiringRequestID = confirmInterviewSlot.HiringRequest_ID;

                            List<GenSalesHiringRequestInterviewerDetail> HR_InterviewerDetails = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == confirmInterviewSlot.HiringRequest_ID && x.HiringRequestDetailId == confirmInterviewSlot.HiringRequest_Detail_ID).ToList();
                            model.HR_InterviewerDetails = ModelBinderATS.InterviewerDetails(HR_InterviewerDetails);

                            List<GenInterviewSlotsMaster> Interview_SlotMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(x => x.HiringRequestId == confirmInterviewSlot.HiringRequest_ID &&
                                                                    x.HiringRequestDetailId == confirmInterviewSlot.HiringRequest_Detail_ID &&
                                                                    x.TalentId == confirmInterviewSlot.Talent_ID &&
                                                                    x.ContactId == confirmInterviewSlot.ContactID).ToList();
                            model.Interview_SlotMaster = ModelBinderATS.InterviewSlotsMaster(Interview_SlotMaster);

                            List<GenTalentSelectedInterviewDetail> SelectedTalent_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.HiringRequestId == confirmInterviewSlot.HiringRequest_ID &&
                                                                    x.HiringRequestDetailId == confirmInterviewSlot.HiringRequest_Detail_ID &&
                                                                    x.TalentId == confirmInterviewSlot.Talent_ID &&
                                                                    x.ContactId == confirmInterviewSlot.ContactID).ToList();
                            model.SelectedTalent_InterviewDetails = ModelBinderATS.TalentSelectedInterviewDetail(SelectedTalent_InterviewDetails);


                            List<GenShortlistedTalentInterviewDetail> ShortListedTalent_InterviewDetails = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.HiringRequestId == confirmInterviewSlot.HiringRequest_ID &&
                                                                    x.HiringRequestDetailId == confirmInterviewSlot.HiringRequest_Detail_ID &&
                                                                    x.TalentId == confirmInterviewSlot.Talent_ID &&
                                                                    x.ContactId == confirmInterviewSlot.ContactID).ToList();
                            model.ShortListedTalent_InterviewDetails = ModelBinderATS.ShortlistedTalentInterviewDetail(ShortListedTalent_InterviewDetails);

                            try
                            {
                                json = JsonConvert.SerializeObject(model);
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                aTSCall.SendInterviewDetailsWithSlot(json, LoggedInUserId, confirmInterviewSlot.HiringRequest_ID);
                                Thread.Sleep(1000);
                                aTSCall.CallAtsAPIToSendTalentAndHRStatus(model.UTS_TalentID, LoggedInUserId, confirmInterviewSlot.HiringRequest_ID);
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
                            }

                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Meeting Link is not initialized, Kindly contact Administrator" });
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Not able to confirm slot due to data is not proper" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("ValidateInterviewTimeSlots")]
        public ObjectResult ValidateInterviewTimeSlots(List<RescheduleSlot> timeslots)
        {
            if (timeslots == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty, please check the datatype or naming convention", Details = timeslots });
            }

            //UTS-3441: Do not allow same start and end time for scheduling the interview.
            //var fetchSameTimingData = timeslots.Where(x => x.StartTime == x.EndTime).ToList();
            //if (fetchSameTimingData.Count > 0)
            //{
            //    string wrongSlots = string.Empty;
            //    foreach (var i in fetchSameTimingData)
            //    {
            //        if (wrongSlots.Length == 0)
            //        {
            //            wrongSlots = i.SlotID.ToString();
            //        }
            //        else
            //        {
            //            wrongSlots = wrongSlots + ", " + i.SlotID.ToString();
            //        }
            //    }
            //    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = $"Start and End time cannot be same for the slots {wrongSlots}", Details = fetchSameTimingData });
            //}

            //UTS-3960: Do not allow to schedule the interview with back date.
            var fetchEarlierDateSlots = timeslots.Where(x =>
                                                                DateTime.Compare(DateTime.ParseExact(x.STRStartTime, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.Now) < 0
                                                                || DateTime.Compare(DateTime.ParseExact(x.STREndTime, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture), DateTime.Now) < 0).
                                                                ToList();

            if (fetchEarlierDateSlots.Count > 0)
            {
                string wrongSlots = string.Empty;
                foreach (var i in fetchEarlierDateSlots)
                {
                    if (wrongSlots.Length == 0)
                    {
                        wrongSlots = i.SlotID.ToString();
                    }
                    else
                    {
                        wrongSlots = wrongSlots + ", " + i.SlotID.ToString();
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = $"Start and End date time cannot be less than today for the slots {wrongSlots}", Details = fetchEarlierDateSlots });
            }

            //UTS-5115 overlap slots
            if (timeslots.Count > 1)
            {
                List<(DateTime, DateTime)> SlotList = new List<(DateTime, DateTime)>
                {
                    (Convert.ToDateTime(timeslots.ElementAt(0).STRStartTime), Convert.ToDateTime(timeslots.ElementAt(0).STREndTime)),
                    (Convert.ToDateTime(timeslots.ElementAt(1).STRStartTime), Convert.ToDateTime(timeslots.ElementAt(1).STREndTime)),
                    (Convert.ToDateTime(timeslots.ElementAt(2).STRStartTime), Convert.ToDateTime(timeslots.ElementAt(2).STREndTime)),
                };

                if (IsDateTimeRangesOverlapList(SlotList))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = $"Slots are overlap, Please change date or time" });
                }
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "All the slots are valid", Details = timeslots });
        }
        #endregion

        #region Private Methods
        private long AddNewSlots(ScheduleInterviewModel scheduleInterviewModel, int LoggedInUserId, Meeting meeting)
        {

            long InterviewMaster_ID = 0;
            long ShortlistedInterview_ID = 0;

            int? Round = CancelOldSlots(scheduleInterviewModel.InterviewMasterID, LoggedInUserId);
            var TZone = _talentConnectAdminDBContext.PrgContactTimeZones.FirstOrDefault(x => x.Id == scheduleInterviewModel.WorkingTimeZoneID);

            GenInterviewSlotsMaster genInterviewSlotsMaster = ModelBinder.BindInterViewSlotMaster(scheduleInterviewModel, LoggedInUserId, Round.Value);
            _talentConnectAdminDBContext.Add(genInterviewSlotsMaster);
            _talentConnectAdminDBContext.SaveChanges();
            InterviewMaster_ID = _talentConnectAdminDBContext.GenInterviewSlotsMasters.FirstOrDefault(x => x.Guid == genInterviewSlotsMaster.Guid).Id;

            foreach (var slot in scheduleInterviewModel.RecheduleSlots.Where(x => !string.IsNullOrEmpty(x.STRSlotDate) && !string.IsNullOrEmpty(x.STRStartTime) && !string.IsNullOrEmpty(x.STREndTime)).OrderBy(y => y.STRSlotDate))
            {
                GenShortlistedTalentInterviewDetail genShortlistedTalentInterviewDetail = ModelBinder.BindGenShortlistedTalentInterviewDetail(scheduleInterviewModel, slot, LoggedInUserId, InterviewMaster_ID);
                _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Add(genShortlistedTalentInterviewDetail);
                _talentConnectAdminDBContext.SaveChanges();

                ShortlistedInterview_ID =
                    _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.
                    Where(x => x.InterviewMasterId == InterviewMaster_ID &&
                    x.HiringRequestId == scheduleInterviewModel.HiringRequest_ID &&
                    x.HiringRequestDetailId == scheduleInterviewModel.HiringRequest_Detail_ID &&
                    x.ContactId == scheduleInterviewModel.ContactID && x.TalentId == scheduleInterviewModel.Talent_ID).
                    OrderByDescending(x => x.Id).FirstOrDefault().Id;

                object[] param = new object[]
                {
                    genShortlistedTalentInterviewDetail.Id
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime_ByID, param);
            }
            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime, null);

            GenTalentSelectedInterviewDetail genTalentSelectedInterviewDetail = ModelBinder.BindGenTalentSelectedInterviewDetail(scheduleInterviewModel, LoggedInUserId, ShortlistedInterview_ID, InterviewMaster_ID, Round, meeting);
            _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Add(genTalentSelectedInterviewDetail);
            _talentConnectAdminDBContext.SaveChanges();

            //AddInterviewerofSlot(scheduleInterviewModel.InterviewMasterID, InterviewMaster_ID, genTalentSelectedInterviewDetail.Id, genTalentSelectedInterviewDetail.ShortlistedInterviewId);

            if (scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4)
            {
                ChangeTalentHRStatus(scheduleInterviewModel.HiringRequest_ID, scheduleInterviewModel.Talent_ID, scheduleInterviewModel.SlotType, LoggedInUserId);
            }
            if (scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4)
            {
                object[] args = new object[]
                {
                    Action_Of_History.Slot_Given, scheduleInterviewModel.HiringRequest_ID, scheduleInterviewModel.Talent_ID, false, LoggedInUserId, 0, InterviewMaster_ID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                object[] args2 = new object[]
                {
                    Action_Of_History.Interview_Scheduled, scheduleInterviewModel.HiringRequest_ID, scheduleInterviewModel.Talent_ID, false, LoggedInUserId, 0, InterviewMaster_ID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args2);
            }
            else
            {
                object[] args = new object[]
                {
                    Action_Of_History.Slot_Given, scheduleInterviewModel.HiringRequest_ID, scheduleInterviewModel.Talent_ID, false, LoggedInUserId, 0, InterviewMaster_ID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
            }
            return InterviewMaster_ID;
        }
        private long AddNewSlotsForRescheduling(RescheduleInterviewSlotModel scheduleInterviewModel, int LoggedInUserId, Meeting meeting)
        {
            long InterviewMaster_ID = 0;
            long ShortlistedInterview_ID = 0;

            int? Round = CancelOldSlots(scheduleInterviewModel.InterviewMasterID, LoggedInUserId);
            var TZone = _talentConnectAdminDBContext.PrgContactTimeZones.FirstOrDefault(x => x.Id == scheduleInterviewModel.WorkingTimeZoneID);

            GenInterviewSlotsMaster genInterviewSlotsMaster = ModelBinder.BindInterViewSlotMasterForRescheduling(scheduleInterviewModel, LoggedInUserId, Round.Value);
            _talentConnectAdminDBContext.Add(genInterviewSlotsMaster);
            _talentConnectAdminDBContext.SaveChanges();
            InterviewMaster_ID = _talentConnectAdminDBContext.GenInterviewSlotsMasters.FirstOrDefault(x => x.Guid == genInterviewSlotsMaster.Guid).Id;

            foreach (var slot in scheduleInterviewModel.RescheduleSlot.Where(x => !string.IsNullOrEmpty(x.STRSlotDate) && !string.IsNullOrEmpty(x.STRStartTime) && !string.IsNullOrEmpty(x.STREndTime)).OrderBy(y => y.STRSlotDate))
            {
                GenShortlistedTalentInterviewDetail genShortlistedTalentInterviewDetail = ModelBinder.BindGenShortlistedTalentInterviewDetailForRescheduling(scheduleInterviewModel, slot, LoggedInUserId, InterviewMaster_ID);
                _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Add(genShortlistedTalentInterviewDetail);
                _talentConnectAdminDBContext.SaveChanges();
                ShortlistedInterview_ID = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.InterviewMasterId == InterviewMaster_ID && x.HiringRequestId == scheduleInterviewModel.HiringRequest_ID && x.HiringRequestDetailId == scheduleInterviewModel.HiringRequest_Detail_ID && x.ContactId == scheduleInterviewModel.ContactID && x.TalentId == scheduleInterviewModel.Talent_ID).OrderByDescending(x => x.Id).FirstOrDefault().Id;

                object[] param = new object[]
                {
                    genShortlistedTalentInterviewDetail.Id
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime_ByID, param);
            }
            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime, null);

            //Insert SelectedTalent
            GenTalentSelectedInterviewDetail genTalentSelectedInterviewDetail = ModelBinder.BindGenTalentSelectedInterviewDetailForRescheduling(scheduleInterviewModel, LoggedInUserId, ShortlistedInterview_ID, InterviewMaster_ID, Round, meeting);
            _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Add(genTalentSelectedInterviewDetail);
            _talentConnectAdminDBContext.SaveChanges();

            //AddInterviewerofSlot(scheduleInterviewModel.InterviewMasterID, InterviewMaster_ID, genTalentSelectedInterviewDetail.Id, genTalentSelectedInterviewDetail.ShortlistedInterviewId);

            if (scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4)
            {
                ChangeTalentHRStatus(scheduleInterviewModel.HiringRequest_ID, scheduleInterviewModel.Talent_ID, scheduleInterviewModel.SlotType, LoggedInUserId);
            }
            if (scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4)
            {
                object[] args = new object[]
                {
                    Action_Of_History.Slot_Given, scheduleInterviewModel.HiringRequest_ID, scheduleInterviewModel.Talent_ID, false, LoggedInUserId, 0, InterviewMaster_ID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
                object[] args2 = new object[]
                {
                    Action_Of_History.Interview_Scheduled, scheduleInterviewModel.HiringRequest_ID, scheduleInterviewModel.Talent_ID, false, LoggedInUserId, 0, InterviewMaster_ID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args2);
            }
            else
            {
                object[] args = new object[]
                {
                    Action_Of_History.Slot_Given, scheduleInterviewModel.HiringRequest_ID, scheduleInterviewModel.Talent_ID, false, LoggedInUserId, 0, InterviewMaster_ID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args);
            }
            return InterviewMaster_ID;
        }
        private void ChangeTalentHRStatus(long HiringRequest_ID, long Talent_ID, long SlotType, long LoggedInUserId)
        {
            if (HiringRequest_ID != 0 && Talent_ID != 0)
            {
                object[] args = new object[]
                {
                    SlotType, Talent_ID, HiringRequest_ID, LoggedInUserId
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_Update_TalentHRStatus, args);
            }
        }
        private void AddInterviewerofSlot(long Old_InterviewMaster_ID, long InterviewMaster_ID, long SelectedInterviewId, long? Shortlisted_InterviewID)
        {
            if (InterviewMaster_ID > 0 && Old_InterviewMaster_ID > 0)
            {

                var obj_InterviewMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.FirstOrDefault(x => x.Id == InterviewMaster_ID);

                var obj_OldInterviewerDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewerDetails.Where(x => x.InterviewMasterId == Old_InterviewMaster_ID).ToList();
                if (obj_OldInterviewerDetails.Count > 0)
                {
                    foreach (var interviwerDetail in obj_OldInterviewerDetails)
                    {
                        GenTalentSelectedInterviewerDetail genTalentSelectedInterviewerDetail = new GenTalentSelectedInterviewerDetail();
                        genTalentSelectedInterviewerDetail.HiringRequestId = obj_InterviewMaster.HiringRequestId;
                        genTalentSelectedInterviewerDetail.HiringRequestDetailId = obj_InterviewMaster.HiringRequestDetailId;
                        genTalentSelectedInterviewerDetail.ContactId = obj_InterviewMaster.ContactId;
                        genTalentSelectedInterviewerDetail.TalentId = obj_InterviewMaster.TalentId;
                        genTalentSelectedInterviewerDetail.TalentSelectedInterviewId = SelectedInterviewId;
                        genTalentSelectedInterviewerDetail.ShortlistedInterviewId = Shortlisted_InterviewID;
                        genTalentSelectedInterviewerDetail.InterviewerId = interviwerDetail.InterviewerId;
                        genTalentSelectedInterviewerDetail.InterviewMasterId = InterviewMaster_ID;
                        _talentConnectAdminDBContext.GenTalentSelectedInterviewerDetails.Add(genTalentSelectedInterviewerDetail);
                        _talentConnectAdminDBContext.SaveChanges();
                    }
                }


            }
            else if (InterviewMaster_ID > 0)
            {
                //Get interviewer Detail for this HR if we create direct final slot for talent
                var obj_InterviewMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(x => x.Id == InterviewMaster_ID).FirstOrDefault();

                var gen_SalesHiringRequest_InterviewerDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewerDetails.Where(x => x.HiringRequestId == obj_InterviewMaster.HiringRequestId).ToList();
                if (gen_SalesHiringRequest_InterviewerDetails != null)
                {
                    foreach (var interviwerDetail in gen_SalesHiringRequest_InterviewerDetails)
                    {
                        GenTalentSelectedInterviewerDetail genTalentSelectedInterviewerDetail = new GenTalentSelectedInterviewerDetail();
                        genTalentSelectedInterviewerDetail.HiringRequestId = obj_InterviewMaster.HiringRequestId;
                        genTalentSelectedInterviewerDetail.HiringRequestDetailId = obj_InterviewMaster.HiringRequestDetailId;
                        genTalentSelectedInterviewerDetail.ContactId = obj_InterviewMaster.ContactId;
                        genTalentSelectedInterviewerDetail.TalentId = obj_InterviewMaster.TalentId;
                        genTalentSelectedInterviewerDetail.TalentSelectedInterviewId = SelectedInterviewId;
                        genTalentSelectedInterviewerDetail.ShortlistedInterviewId = Shortlisted_InterviewID;
                        genTalentSelectedInterviewerDetail.InterviewerId = interviwerDetail.InterviewerId;
                        genTalentSelectedInterviewerDetail.InterviewMasterId = InterviewMaster_ID;
                        _talentConnectAdminDBContext.GenTalentSelectedInterviewerDetails.Add(genTalentSelectedInterviewerDetail);
                        _talentConnectAdminDBContext.SaveChanges();
                    }
                }
            }
        }
        private int CancelOldSlots(long SelectedInterviewId, int LoggedInUserId)
        {
            int Round = 0;
            List<long> slotIds = new List<long>();

            GenInterviewSlotsMaster genInterviewSlotsMaster = new GenInterviewSlotsMaster();

            genInterviewSlotsMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.FirstOrDefault(x => x.Id == SelectedInterviewId);

            if (genInterviewSlotsMaster != null)
            {
                genInterviewSlotsMaster.InterviewStatusId = (short)prg_InterviewStatus.Cancelled;
                genInterviewSlotsMaster.LastModifiedById = Convert.ToInt32(LoggedInUserId);
                genInterviewSlotsMaster.LastModifiedDatetime = DateTime.Now;
                Round = genInterviewSlotsMaster.InterviewRoundCount.Value;
                CommonLogic.DBOperator(_talentConnectAdminDBContext, genInterviewSlotsMaster, EntityState.Modified);



                List<GenShortlistedTalentInterviewDetail> slotsDetails = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.InterviewMasterId == SelectedInterviewId).ToList();

                foreach (var slot in slotsDetails)
                {
                    slotIds.Add(slot.Id);
                    slot.StatusId = (short)prg_InterviewStatus.Cancelled;
                    slot.LastModifiedById = Convert.ToInt32(LoggedInUserId);
                    slot.LastModifiedDatetime = DateTime.Now;
                    CommonLogic.DBOperator(_talentConnectAdminDBContext, slot, EntityState.Modified);
                }

                GenTalentSelectedInterviewDetail genTalentSelectedInterviewDetail = new GenTalentSelectedInterviewDetail();
                foreach (long id in slotIds)
                {
                    var selectedSlotdetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.FirstOrDefault(x => x.ShortlistedInterviewId == id);
                    if (selectedSlotdetails != null)
                    {
                        selectedSlotdetails.StatusId = (short)prg_InterviewStatus.Cancelled;
                        selectedSlotdetails.LastModifiedById = Convert.ToInt32(LoggedInUserId);
                        selectedSlotdetails.LastModifiedDatetime = DateTime.Now;
                        CommonLogic.DBOperator(_talentConnectAdminDBContext, selectedSlotdetails, EntityState.Modified);
                    }
                }

                object[] param = new object[]
                {
                    Action_Of_History.Cancelled, genInterviewSlotsMaster.HiringRequestId, genInterviewSlotsMaster.TalentId, false, LoggedInUserId, 0, genInterviewSlotsMaster.Id, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);

            }
            return Round;
        }
        private void UpdateSlots(RescheduleInterviewSlotModel rescheduleInterviewSlotModel, long Loggedinuserid)
        {
            GenInterviewSlotsMaster genInterviewSlotsMaster = new();
            genInterviewSlotsMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.InterviewMasterID);
            genInterviewSlotsMaster.InterviewStatusId = (short)prg_InterviewStatus.Slot_Given;
            genInterviewSlotsMaster.LastModifiedById = Convert.ToInt32(Loggedinuserid);
            genInterviewSlotsMaster.LastModifiedDatetime = DateTime.Now;
            CommonLogic.DBOperator(_talentConnectAdminDBContext, genInterviewSlotsMaster, EntityState.Modified);

            List<GenShortlistedTalentInterviewDetail> genShortlistedTalentInterviewDetails = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.InterviewMasterId == rescheduleInterviewSlotModel.InterviewMasterID).ToList();
            var timeZone = _talentConnectAdminDBContext.PrgContactTimeZones.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.WorkingTimeZoneID);


            foreach (var slot in genShortlistedTalentInterviewDetails)
            {
                int slotID = Convert.ToInt32(slot.InterviewSlot.Replace("Slot", ""));
                slot.StatusId = 1;

                string interviewDate = rescheduleInterviewSlotModel.RescheduleSlot.FirstOrDefault(x => x.SlotID == slotID).SlotDate;
                string startTime = rescheduleInterviewSlotModel.RescheduleSlot.FirstOrDefault(x => x.SlotID == slotID).STRStartTime;
                string endTime = rescheduleInterviewSlotModel.RescheduleSlot.FirstOrDefault(x => x.SlotID == slotID).STREndTime;

                slot.ScheduledOn = CommonLogic.ConvertString2DateTime(interviewDate);
                slot.InterviewStartTime = CommonLogic.ConvertString2DateTime(startTime).TimeOfDay;
                slot.InterviewEndTime = CommonLogic.ConvertString2DateTime(endTime).TimeOfDay;
                slot.TimeZoneId = rescheduleInterviewSlotModel.WorkingTimeZoneID;

                slot.LastModifiedById = Convert.ToInt32(Loggedinuserid);
                slot.LastModifiedDatetime = DateTime.Now;

                CommonLogic.DBOperator(_talentConnectAdminDBContext, slot, EntityState.Modified);

                object[] args = new object[] { slot.Id };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime_ByID, args);

            }
            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime, null);

            GenTalentSelectedInterviewDetail genTalentSelectedInterviewDetail = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.FirstOrDefault(x => x.InterviewMasterId == rescheduleInterviewSlotModel.InterviewMasterID);
            if (genTalentSelectedInterviewDetail != null)
            {
                genTalentSelectedInterviewDetail.ContactTimeZoneId = rescheduleInterviewSlotModel.WorkingTimeZoneID;
                genTalentSelectedInterviewDetail.AdditionalNotes = rescheduleInterviewSlotModel.Additional_Notes;
                genTalentSelectedInterviewDetail.LastModifiedById = Convert.ToInt32(Loggedinuserid);
                genTalentSelectedInterviewDetail.LastModifiedDatetime = DateTime.Now;

                CommonLogic.DBOperator(_talentConnectAdminDBContext, genTalentSelectedInterviewDetail, EntityState.Modified);


            }

            object[] args2 = new object[] { Action_Of_History.Update_Slots, rescheduleInterviewSlotModel.HiringRequest_ID, rescheduleInterviewSlotModel.Talent_ID, false, Loggedinuserid, 0, rescheduleInterviewSlotModel.InterviewMasterID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS };
            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, args2);
        }
        private long AddNewSlotsForLaterOption(RescheduleInterviewSlotModel rescheduleInterviewSlotModel, int LoggedInUserId, long ShortListedInterivewId, Boolean Lateroption)
        {
            #region Variables
            long InterviewMaster_ID = 0;
            long ShortlistedInterview_ID = 0;
            int? Round = 0;
            #endregion

            GenInterviewSlotsMaster gen_InterviewSlotsMasterdetails = _talentConnectAdminDBContext.GenInterviewSlotsMasters.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.InterviewMasterID);
            if (gen_InterviewSlotsMasterdetails != null) { Round = (gen_InterviewSlotsMasterdetails.InterviewRoundCount + 1); }

            var TZone = _talentConnectAdminDBContext.PrgContactTimeZones.FirstOrDefault(x => x.Id == rescheduleInterviewSlotModel.WorkingTimeZoneID);


            GenInterviewSlotsMaster gen_InterviewSlotsMaster = new GenInterviewSlotsMaster();
            gen_InterviewSlotsMaster.ContactId = rescheduleInterviewSlotModel.ContactID;
            gen_InterviewSlotsMaster.Guid = Convert.ToString(Guid.NewGuid());
            gen_InterviewSlotsMaster.HiringRequestDetailId = rescheduleInterviewSlotModel.HiringRequest_Detail_ID;
            gen_InterviewSlotsMaster.HiringRequestId = rescheduleInterviewSlotModel.HiringRequest_ID;
            gen_InterviewSlotsMaster.CreatedById = Convert.ToInt32(LoggedInUserId);
            gen_InterviewSlotsMaster.CreatedByDatetime = System.DateTime.Now;
            gen_InterviewSlotsMaster.InterviewStatusId = rescheduleInterviewSlotModel.RescheduleSlot.Count == 1 ? 4 : 1;
            gen_InterviewSlotsMaster.RescheduledTypeId = rescheduleInterviewSlotModel.RescheduleRequestBy == "Client" ? 1 : 2;
            gen_InterviewSlotsMaster.TalentId = rescheduleInterviewSlotModel.Talent_ID;
            gen_InterviewSlotsMaster.InterviewRoundCount = Round != null ? Convert.ToInt32(Round.Value) : 0;
            gen_InterviewSlotsMaster.InterviewRoundStr = Round != null ? "Round " + Round.Value.ToString() : "";
            _talentConnectAdminDBContext.GenInterviewSlotsMasters.Add(gen_InterviewSlotsMaster);
            _talentConnectAdminDBContext.SaveChanges();
            InterviewMaster_ID = gen_InterviewSlotsMaster.Id;

            foreach (var slot in rescheduleInterviewSlotModel.RescheduleSlot.Where(x => !string.IsNullOrEmpty(x.STRSlotDate) && !string.IsNullOrEmpty(x.STRStartTime) && !string.IsNullOrEmpty(x.STREndTime)).OrderBy(y => y.STRSlotDate))
            {
                GenShortlistedTalentInterviewDetail gen_ShortlistedTalent_InterviewDetails = new GenShortlistedTalentInterviewDetail();
                gen_ShortlistedTalent_InterviewDetails.ContactId = rescheduleInterviewSlotModel.ContactID;
                gen_ShortlistedTalent_InterviewDetails.CreatedById = Convert.ToInt32(LoggedInUserId);
                gen_ShortlistedTalent_InterviewDetails.CreatedByDatetime = System.DateTime.Now;
                gen_ShortlistedTalent_InterviewDetails.DurationInHours = 0.00m;
                gen_ShortlistedTalent_InterviewDetails.HiringRequestDetailId = rescheduleInterviewSlotModel.HiringRequest_Detail_ID;
                gen_ShortlistedTalent_InterviewDetails.HiringRequestId = rescheduleInterviewSlotModel.HiringRequest_ID;
                gen_ShortlistedTalent_InterviewDetails.InterviewMasterId = InterviewMaster_ID;
                gen_ShortlistedTalent_InterviewDetails.InterviewSlot = "Slot" + slot.SlotID;
                gen_ShortlistedTalent_InterviewDetails.IsTalentConfirmed = rescheduleInterviewSlotModel.SlotType == 2 || rescheduleInterviewSlotModel.SlotType == 4 ? true : false;

                gen_ShortlistedTalent_InterviewDetails.ScheduledOn = CommonLogic.ConvertString2DateTime(slot.SlotDate.ToString());
                gen_ShortlistedTalent_InterviewDetails.InterviewStartTime = CommonLogic.ConvertString2DateTime(slot.STRStartTime).TimeOfDay;
                gen_ShortlistedTalent_InterviewDetails.InterviewEndTime = CommonLogic.ConvertString2DateTime(slot.STREndTime).TimeOfDay;

                TimeSpan difference = gen_ShortlistedTalent_InterviewDetails.InterviewEndTime.Value - gen_ShortlistedTalent_InterviewDetails.InterviewStartTime.Value;
                gen_ShortlistedTalent_InterviewDetails.DurationInHours = difference.Hours;
                gen_ShortlistedTalent_InterviewDetails.StatusId = rescheduleInterviewSlotModel.RescheduleSlot.Count == 1 ? 4 : 1;
                gen_ShortlistedTalent_InterviewDetails.TalentId = rescheduleInterviewSlotModel.Talent_ID;
                gen_ShortlistedTalent_InterviewDetails.TimeZoneId = rescheduleInterviewSlotModel.WorkingTimeZoneID;
                _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Add(gen_ShortlistedTalent_InterviewDetails);
                _talentConnectAdminDBContext.SaveChanges();
                ShortlistedInterview_ID = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.InterviewMasterId == InterviewMaster_ID && x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID && x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID && x.ContactId == rescheduleInterviewSlotModel.ContactID && x.TalentId == rescheduleInterviewSlotModel.Talent_ID).OrderByDescending(x => x.Id).FirstOrDefault().Id;

                object[] param = new object[]
                {
                    gen_ShortlistedTalent_InterviewDetails.Id
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime_ByID, param);
            }
            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Update_ShortlistedTalent_InterviewDetails_ISTTime, null);


            GenTalentSelectedInterviewDetail TalentSelected = new GenTalentSelectedInterviewDetail();
            TalentSelected.HiringRequestId = rescheduleInterviewSlotModel.HiringRequest_ID;
            TalentSelected.HiringRequestDetailId = rescheduleInterviewSlotModel.HiringRequest_Detail_ID;
            TalentSelected.ContactId = rescheduleInterviewSlotModel.ContactID;
            TalentSelected.TalentId = rescheduleInterviewSlotModel.Talent_ID;
            if (rescheduleInterviewSlotModel.SlotType == 2 || rescheduleInterviewSlotModel.SlotType == 4)
            {
                TalentSelected.ShortlistedInterviewId = ShortlistedInterview_ID;
                TalentSelected.IsConfirmed = true;
            }
            else
            {
                TalentSelected.ShortlistedInterviewId = null;
                TalentSelected.IsConfirmed = false;
            }
            TalentSelected.CreatedById = LoggedInUserId;
            TalentSelected.CreatedByDatetime = DateTime.Now;
            TalentSelected.StatusId = rescheduleInterviewSlotModel.RescheduleSlot.Count == 1 ? 4 : 1;
            TalentSelected.ContactTimeZoneId = rescheduleInterviewSlotModel.WorkingTimeZoneID;
            TalentSelected.InterviewRound = Round;
            TalentSelected.InterviewRoundStr = "Round " + Round;
            TalentSelected.InterviewMasterId = InterviewMaster_ID;

            TalentSelected.AdditionalNotes = rescheduleInterviewSlotModel.Additional_Notes;

            if (rescheduleInterviewSlotModel.SlotType == 4)
            {
                TalentSelected.ZoomInterviewLink = rescheduleInterviewSlotModel.InterviewCallLink;
            }

            _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Add(TalentSelected);
            _talentConnectAdminDBContext.SaveChanges();

            //AddInterviewerofSlot(rescheduleInterviewSlotModel.InterviewMasterID, InterviewMaster_ID, TalentSelected.Id, TalentSelected.ShortlistedInterviewId);

            if (Lateroption)
            {
                GenTalentSelectedNextRoundInterviewDetail gen_nextroundinterviewdetail = new GenTalentSelectedNextRoundInterviewDetail();
                gen_nextroundinterviewdetail = _talentConnectAdminDBContext.GenTalentSelectedNextRoundInterviewDetails.FirstOrDefault(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID && x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID && x.ShortlistedInterviewId == ShortListedInterivewId);
                if (gen_nextroundinterviewdetail != null)
                {
                    _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_UpdateIsLaterSlot, new object[] { gen_nextroundinterviewdetail.Id, true, 0 });


                }
            }
            if (!Lateroption) { _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_UpdateIsLaterSlot, new object[] { 0, false, ShortListedInterivewId }); }


            if (rescheduleInterviewSlotModel.SlotType == 2 || rescheduleInterviewSlotModel.SlotType == 4)
            {
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, new object[] { Action_Of_History.Slot_Given, rescheduleInterviewSlotModel.HiringRequest_ID, rescheduleInterviewSlotModel.Talent_ID, false, LoggedInUserId, 0, InterviewMaster_ID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS });
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, new object[] { Action_Of_History.Interview_Scheduled, rescheduleInterviewSlotModel.HiringRequest_ID, rescheduleInterviewSlotModel.Talent_ID, false, LoggedInUserId, 0, InterviewMaster_ID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS });
            }
            if (rescheduleInterviewSlotModel.SlotType == 1 || rescheduleInterviewSlotModel.SlotType == 4)
            {
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, new object[] { Action_Of_History.Slot_Given, rescheduleInterviewSlotModel.HiringRequest_ID, rescheduleInterviewSlotModel.Talent_ID, false, LoggedInUserId, 0, InterviewMaster_ID, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS });
            }
            return InterviewMaster_ID;
        }
        private ATSInterviewDetailViewModel GetModelReadyForATSCall(RescheduleInterviewSlotModel rescheduleInterviewSlotModel)
        {
            ATSInterviewDetailViewModel model = new ATSInterviewDetailViewModel();
            model.ATS_TalentID = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == rescheduleInterviewSlotModel.Talent_ID).FirstOrDefault().AtsTalentId ?? 0;
            model.UTS_TalentID = rescheduleInterviewSlotModel.Talent_ID;
            model.UTS_HiringRequestID = rescheduleInterviewSlotModel.HiringRequest_ID;

            List<GenSalesHiringRequestInterviewerDetail> HR_InterviewerDetails = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID && x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID).ToList();
            model.HR_InterviewerDetails = ModelBinderATS.InterviewerDetails(HR_InterviewerDetails);

            List<GenInterviewSlotsMaster> Interview_SlotMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID &&
                                                    x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID &&
                                                    x.TalentId == rescheduleInterviewSlotModel.Talent_ID &&
                                                    x.ContactId == rescheduleInterviewSlotModel.ContactID).ToList();
            model.Interview_SlotMaster = ModelBinderATS.InterviewSlotsMaster(Interview_SlotMaster);

            List<GenTalentSelectedInterviewDetail> SelectedTalent_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID &&
                                                    x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID &&
                                                    x.TalentId == rescheduleInterviewSlotModel.Talent_ID &&
                                                    x.ContactId == rescheduleInterviewSlotModel.ContactID).ToList();
            model.SelectedTalent_InterviewDetails = ModelBinderATS.TalentSelectedInterviewDetail(SelectedTalent_InterviewDetails);


            List<GenShortlistedTalentInterviewDetail> ShortListedTalent_InterviewDetails = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.HiringRequestId == rescheduleInterviewSlotModel.HiringRequest_ID &&
                                                    x.HiringRequestDetailId == rescheduleInterviewSlotModel.HiringRequest_Detail_ID &&
                                                    x.TalentId == rescheduleInterviewSlotModel.Talent_ID &&
                                                    x.ContactId == rescheduleInterviewSlotModel.ContactID).ToList();
            model.ShortListedTalent_InterviewDetails = ModelBinderATS.ShortlistedTalentInterviewDetail(ShortListedTalent_InterviewDetails);

            return model;
        }
        private void UpdateStatusAfterInterviewSchedule(ConfirmInterviewSlotModel confirmSlot, int LoggedInUserId)
        {
            GenInterviewSlotsMaster gen_InterviewSlotsMaster = new GenInterviewSlotsMaster();
            //Status cancel for Perviuse Interview Id change 
            gen_InterviewSlotsMaster = _talentConnectAdminDBContext.GenInterviewSlotsMasters.Where(x => x.Id == confirmSlot.InterviewMasterID).FirstOrDefault();
            gen_InterviewSlotsMaster.InterviewStatusId = 4;
            gen_InterviewSlotsMaster.LastModifiedById = Convert.ToInt32(LoggedInUserId);
            //gen_InterviewSlotsMaster.LastModifiedDatetime = rescheduleInterviewSlotModel.RecheduleSlots.Where(x => x.SlotID == 1).FirstOrDefault().SlotDate;
            gen_InterviewSlotsMaster.LastModifiedDatetime = DateTime.Now;
            _talentConnectAdminDBContext.Entry(gen_InterviewSlotsMaster).State = EntityState.Modified;
            _talentConnectAdminDBContext.SaveChanges();

            #region Insert HR History 
            object[] param = new object[]
            {
                   Action_Of_History.Interview_Scheduled, gen_InterviewSlotsMaster.HiringRequestId.Value, gen_InterviewSlotsMaster.TalentId.Value, false, LoggedInUserId, 0, gen_InterviewSlotsMaster.Id, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
            };
            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
            #endregion

            var slotsDetail = _talentConnectAdminDBContext.GenShortlistedTalentInterviewDetails.Where(x => x.InterviewMasterId == confirmSlot.InterviewMasterID).ToList();
            foreach (var slot in slotsDetail)
            {
                slot.StatusId = 4;
                slot.LastModifiedById = Convert.ToInt32(LoggedInUserId);
                slot.LastModifiedDatetime = DateTime.Now;
                _talentConnectAdminDBContext.Entry(slot).State = EntityState.Modified;
                _talentConnectAdminDBContext.SaveChanges();
            }
        }
        private bool IsDateTimeRangesOverlapList(List<(DateTime, DateTime)> dateRanges)
        {
            for (int i = 0; i < dateRanges.Count; i++)
            {
                for (int j = i + 1; j < dateRanges.Count; j++)
                {
                    var (start1, end1) = dateRanges[i];
                    var (start2, end2) = dateRanges[j];

                    if (IsDateTimeRangeOverlapping(start1, end1, start2, end2))
                    {
                        return true; // Overlap detected    
                    }
                }
            }

            return false; // No overlap within the list
        }

        private bool IsDateTimeRangeOverlapping(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            // Check if either range is entirely before or after the other
            if (end1 < start2 || end2 < start1)
            {
                return false; // No overlap
            }

            return true; // Overlap detected
        }
        #endregion


    }
}
