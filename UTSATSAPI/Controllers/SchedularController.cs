using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using System.Text;
using UTSATSAPI.ATSCalls;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModel;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [ApiController]
    [Route("Schedular/", Name = "Schedular")]
    public class SchedularController : ControllerBase
    {
        #region Variables
        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private readonly ISchedular _iSchedular;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public SchedularController(TalentConnectAdminDBContext talentConnectAdminDBContext, ISchedular iSchedular, IConfiguration configuration)
        {
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
            _iSchedular = iSchedular;
            _configuration = configuration;
        }
        #endregion


        [HttpGet("SendEmailCandidatesAddedForPosition")]
        [AllowAnonymous]
        //Send Email Job Credits Exhausted
        public async Task<ObjectResult> SendEmailCandidatesAddedForPosition()
        {
            try
            {
                var list = new Dictionary<string, int>();
                int VettedCount = 0;
                int ShortlistedCount = 0;
                string PocName = "", PocEmail = "", PocDesignation = "", PocPhone = "";
                var hrList = _iSchedular.Sproc_Get_ListOfHR_For_NewCandidate_Added_Email_ClientPortal().ToList();
                long PocID = 0;
                if (hrList != null && hrList.Count() > 0)
                {
                    foreach (var hr in hrList)
                    {
                        long HRID = hr.HRID??0;
                        long ContactID = hr.ContactID??0;
                        long SalesUserID = 0;
                        int HRTypeID = 0;
                        int AITalentCount = 0,NoOfTalent = 0;
                        string RequestForTalent = "";
                        string ClientName = "", EmailId = "";
                        var gensalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HRID).FirstOrDefault();
                        VettedCount = 0;
                        AITalentCount = hr.IsAITalent??0;
                        NoOfTalent = hr.NoOfTalents ?? 0;
                        var obj = _iSchedular.sproc_Get_ContactPointofContact(ContactID);
                        if (obj != null)
                        {
                            PocID = obj.User_ID??0;
                            var usr_User = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == obj.User_ID).FirstOrDefault();
                            if (usr_User != null)
                            {
                                PocName = usr_User.FullName;
                                PocEmail = usr_User.EmailId;
                                PocDesignation = usr_User.Designation;
                                PocPhone = usr_User.ContactNumber;
                            }
                        }

                        if (gensalesHiringRequest != null)
                        {
                            RequestForTalent = gensalesHiringRequest.RequestForTalent;
                            HRTypeID = gensalesHiringRequest.HrtypeId ?? 0;

                            SalesUserID = gensalesHiringRequest.SalesUserId ?? 0;
                            EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);

                            var gen_Contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ContactID).FirstOrDefault();
                            if (gen_Contact != null)
                            {
                                ClientName = gen_Contact.FullName;
                                EmailId = gen_Contact.EmailId;
                            }

                            emailBinder.SendEmailOnNewCandidatesAdded_PayPerView(HRID, RequestForTalent, ClientName, EmailId, ShortlistedCount, SalesUserID, AITalentCount, NoOfTalent,
                                PocID,PocName, PocEmail, PocDesignation, PocPhone); //AITalentCount

                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success"});
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("SendEmailJobExpiry")]
        [AllowAnonymous]
        //Send Email Job Expiry on 5,3 and 1 days
        public async Task<ActionResult> SendEmailJobExpiry()
        {
            try
            {
                EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                
                int[] Reminders = new int[] { 5,3,1 };  //5, 3, 
                var list = new Dictionary<string, int>();
                int VettedCount = 0;
                string ClientName = "", EmailId = "", PocName = "", PocEmail = "", PocDesignation = "", PocPhone = "";
                foreach (var item in Reminders)
                {
                    var hrList = _iSchedular.Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal(item).ToList();

                    if (hrList != null && hrList.Count() > 0)
                    {
                        foreach (var hr in hrList)
                        {
                            long ContactId = hr.ContactID;
                            var gen_Contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ContactId).FirstOrDefault();
                            if (gen_Contact != null)
                            {
                                ClientName = gen_Contact.FullName;
                                EmailId = gen_Contact.EmailId;
                            }

                            long pocID = hr.PocId;
                            var obj = _iSchedular.sproc_Get_ContactPointofContact(hr.ContactID);
                            if (pocID != 0)
                            {
                                var usr_User = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == pocID).FirstOrDefault();
                                if (usr_User != null)
                                {
                                    PocName = usr_User.FullName;
                                    PocEmail = usr_User.EmailId;
                                    PocDesignation = usr_User.Designation;
                                    PocPhone = usr_User.ContactNumber;
                                }
                            }

                            emailBinder.SendEmailJobExpiryOnsomedays(pocID,PocName, PocEmail, PocDesignation, PocPhone, ClientName, EmailId,hr, item, VettedCount);                            
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #region HRTalentNotes
        [HttpGet("SendTalentNotesEmailToClient")]
        [AllowAnonymous]
        public ObjectResult SendTalentNotesEmailToClient()
        {
            try
            {               
                // fetch all the HR's and their details
                List<Sproc_Fetch_TalentNotesEmailsLog_Result> talentHrDetails = _iSchedular.Sproc_Fetch_TalentNotesEmailsLog();
                foreach(var i in talentHrDetails)
                {
                    object[] param = new object[] { i.HRID };
                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);

                    // fetch talent details from HR
                    List<Sproc_Fetch_TalentNotesEmailsLog_HRWise_Result> talentHrNotesDetails = _iSchedular.Sproc_Fetch_TalentNotesEmailsLog_HRWise(paramString);
                    if (talentHrNotesDetails != null)
                    {
                        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                        emailBinder.SendEmailForNotesAddedToClient(talentHrNotesDetails, i);
                    }
                }                

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UpComing Reneal Emails
        [HttpGet("RenewalEngagementNotification")]
        [AllowAnonymous]
        //Send Email Job Credits Exhausted
        public async Task<ObjectResult> RenewalEngagementNotification()
        {
            try
            {
                var list = new Dictionary<string, int>();
                int VettedCount = 0;
                int ShortlistedCount = 0;

                var hrList = _iSchedular.Sproc_Get_Engagement_Renewal_Emails_EngagementList().ToList();

                if (hrList != null && hrList.Count() > 0)
                {
                    foreach (var hr in hrList)
                    {
                        long OnBoardID = hr.OnBoardID??0;
                        string EngagementID = hr.EngagemenID;

                        object[] param = new object[] { OnBoardID, EngagementID };
                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);

                        var result = _iSchedular.Sproc_Get_Engagement_Renewal_Emails_Details(paramString);
                        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                        if (result != null)
                        {
                             await emailBinder.SendEmailForEngagementRenewalAsync(result);                            
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Nurture Summary Emails
        [HttpGet("SendNurtureSummaryEmails")]
        [AllowAnonymous]
        //Send Email Job Credits Exhausted
        public async Task<ObjectResult> SendNurtureSummaryEmails()
        {
            try
            {
                var list = new Dictionary<string, int>();
                
                var hrList = _iSchedular.Sproc_Get_Nurture_Email_List().ToList();

                if (hrList != null && hrList.Count() > 0)
                {
                    foreach (var hr in hrList)
                    {
                        long HRID = hr.HRID ?? 0;
                        long ContactID = hr.ClientID ?? 0;
                        int Month = hr.MONTH??0;
                        int year = hr.YEAR??0;
                        string Environment = _configuration["Environment"];

                        object[] param = new object[] { ContactID, Environment, Month, year, HRID };
                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);


                        var EmailDetails = _iSchedular.Sproc_Get_Summary_Emails(paramString);

                        if(EmailDetails != null)
                        {
                            EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                            emailBinder.SendEmailForNurture(EmailDetails);
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Nurture Email for PPH
        [HttpGet("SendNurtureSummaryEmailsForSelfSignup")]
        [AllowAnonymous]       
        public async Task<ObjectResult> SendNurtureSummaryEmailsForSelfSignup()
        {
            try
            {
                var list = new Dictionary<string, int>();

                List<Sproc_SelfSignUpUserWithoutJobPost_ClientPortal_Result> clientList = _iSchedular.Sproc_SelfSignUpUserWithoutJobPost_ClientPortal_List("1").ToList();

                if (clientList != null && clientList.Count() > 0)
                {
                    foreach (var client in clientList)
                    {                       
                        long ContactID = client.ContactID ?? 0; 
                        string Environment = _configuration["Environment"];

                        object[] param = new object[] { ContactID, Environment, 1 };
                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);

                        var EmailDetails = _iSchedular.Sproc_Get_Nurture_Logs_Emails(paramString);

                        if (EmailDetails != null)
                        {
                            EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                            emailBinder.SendEmailForNurture(EmailDetails);

                            long companyID = client.CompanyID ?? 0;
                            param = new object[] { ContactID, companyID };
                            paramString = CommonLogic.ConvertToParamStringWithNull(param);
                            _iSchedular.UpdateNurtureLogs(paramString);
                        }
                    }
                }

                SendNurtureSummaryEmailsForSelfSignupAfter48Hrs();

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("SendNurtureSummaryEmailsForSelfSignupAfter48Hrs")]
        [AllowAnonymous]    
        public async Task<ObjectResult> SendNurtureSummaryEmailsForSelfSignupAfter48Hrs()
        {
            try
            {
                var list = new Dictionary<string, int>();

                List<Sproc_SelfSignUpUserWithoutJobPost_ClientPortal_Result> clientList = _iSchedular.Sproc_SelfSignUpUserWithoutJobPost_ClientPortal_List("2").ToList();

                if (clientList != null && clientList.Count() > 0)
                {
                    foreach (var client in clientList)
                    {
                        long ContactID = client.ContactID ?? 0;
                        string Environment = _configuration["Environment"];

                        object[] param = new object[] { ContactID, Environment, 2 };
                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);

                        var EmailDetails = _iSchedular.Sproc_Get_Nurture_Logs_Emails(paramString);

                        if (EmailDetails != null)
                        {
                            EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                            emailBinder.SendEmailForNurture(EmailDetails);

                            long companyID = client.CompanyID ?? 0;
                            param = new object[] { ContactID, companyID };
                            paramString = CommonLogic.ConvertToParamStringWithNull(param);
                            _iSchedular.UpdateNurtureLogs(paramString);
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Reset All HR Talent status for Demo client

        [AllowAnonymous]
        [HttpGet("ResetAllDemoHRTalentStatus")]
        public ObjectResult ResetAllDemoHRTalentStatus()
        {
            try
            {
                _iSchedular.Sproc_Reset_AllHR_TalentStatus();

                ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                aTSCall.SendResetAllDemoHRRequest("", SessionValues.LoginUserId, 0);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Sucess" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetUpdatesforSchedularRun

        [AllowAnonymous]
        [HttpGet("GetUpdatesforSchedularRun")]
        public void GetUpdatesforSchedularRun()
        {
            List<Sproc_Get_SchedularUpdates_Result> updatedSchedularData = _iSchedular.Sproc_Get_SchedularUpdates_Result();
            if (updatedSchedularData != null && updatedSchedularData.Count > 0)
            {
                StringBuilder sb = new();
                int index = 1;
                foreach (var i in updatedSchedularData)
                {
                    if (i.LastRunDateTime.HasValue)
                    {
                        sb.Append($"{index++}. ");
                        sb.Append($"{i.SchedularName} - {i.SPName} - {i.LastRunDateTime.Value.ToString("dd/MM/yyyy")}");
                        sb.Append("\\n");
                    }
                }

                index = 0;

                string uri = "https://chat.googleapis.com/v1/spaces/AAAACRCq2fw/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=mTeq94xU-BLhYjyVkYQuhjMSFVy7eZ_nT3q45GP2fQQ";

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.Timeout = 500000;
                    webRequest.ContentType = "application/json";
                    webRequest.Credentials = CredentialCache.DefaultCredentials;

                    using (var requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        string text = "{text:\"" + sb.ToString() + "\"}";

                        requestWriter.Write(text);
                        requestWriter.Flush();
                        requestWriter.Close();
                    }
                }

                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                Stream resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                string ResponseJson = reader.ReadToEnd();
                if ((int)response.StatusCode == 200)
                {

                }
            }
        }

        #endregion
    }
}
