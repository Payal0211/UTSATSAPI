namespace UTSATSAPI.Helpers
{
    using Aspose.Words;
    using iTextSharp.text.pdf.codec.wmf;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;
    using Org.BouncyCastle.Asn1.Crmf;
    using RestSharp;
    using System;
    using System.Globalization;
    using System.Net.Mail;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Web;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;

    public class EmailBinder
    {
        //#region Variables 
        //IConfiguration _configuration;
        //TalentConnectAdminDBContext _talentConnectAdminDBContext;
        //EmailDatabaseContentProvider emailDatabaseContentProvider;

        //string ccEmail = string.Empty;
        //string ccEmailName = string.Empty;
        //string clientEmailName = string.Empty;
        //#endregion

        //#region Constructor
        //public EmailBinder(IConfiguration configuration, TalentConnectAdminDBContext talentConnectAdminDBContext)
        //{
        //    _configuration = configuration;
        //    _talentConnectAdminDBContext = talentConnectAdminDBContext;
        //    emailDatabaseContentProvider = new EmailDatabaseContentProvider(_talentConnectAdminDBContext);
        //    ccEmail = _configuration["app_settings:CCEmailId"] + "," + _configuration["app_settings:CC1EmailId"] + "," + _configuration["app_settings:TSCEmailId"];
        //    ccEmailName = _configuration["app_settings:CCEmailName"] + ',' + _configuration["app_settings:CC1EmailName"] + ',' + _configuration["app_settings:TSCEmailName"];
        //    clientEmailName = _configuration["ClientEmailName"].ToString();
        //}
        //#endregion

        //#region Internal Team Emails
        ////when add new client
        //public bool SetPasswordSendEmail(GenContact varContactDetail, string invitingUserName, string invitingIserEmailId, string Designation, long PocID = 0)
        //{
        //    try
        //    {
        //        var reactClientPortalURL = _configuration["ReactClientPortalURL"];
        //        var varSMTPEmailName = _configuration["app_settings:SMTPEmailName"];
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        string Subject = "", POCContactNo = "";

        //        List<string> toEmail = new List<string>
        //        {
        //            varContactDetail.EmailId
        //        };

        //        List<string> toEmailName = new List<string>
        //        {
        //            varContactDetail.FullName
        //        };

        //        string strBccEmail = "", strBccEamilName = "";

        //        object[] param = new object[]
        //        {
        //            varContactDetail.Id
        //        };

        //        string paramString = CommonLogic.ConvertToParamStringWithNull(param);
        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";
        //        if (PocID != 0)
        //        {
        //            var varGetUser = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == PocID).FirstOrDefault();
        //            if (varGetUser != null)
        //            {
        //                strBccEmail = varGetUser.EmailId;
        //                strBccEamilName = varGetUser.FullName;
        //            }
        //            var POCUserHierarchy = GetHierarchyForEmail(PocID.ToString());

        //            CCEMAIL = string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.EmailId)).Select(x => x.EmailId));
        //            CCEMAILNAME = string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Select(x => x.UserName));

        //        }
        //        else
        //        {
        //            var obj = sproc_Get_ContactPointofContact(paramString); //_talentConnectAdminDBContext.Set<sproc_Get_ContactPointofContact_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Get_ContactPointofContact, paramString)).AsEnumerable().FirstOrDefault();
        //            if (obj != null)
        //            {
        //                PocID = obj.User_ID ?? 0;
        //                var varGetUser = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == obj.User_ID).FirstOrDefault();
        //                if (varGetUser != null)
        //                {
        //                    strBccEmail = varGetUser.EmailId;
        //                    strBccEamilName = varGetUser.FullName;

        //                }
        //                var POCUserHierarchy = GetHierarchyForEmail(obj.User_ID.ToString());

        //                CCEMAIL = string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.EmailId)).Select(x => x.EmailId));
        //                CCEMAILNAME = string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Select(x => x.UserName));
        //            }
        //        }

        //        if (PocID > 0)
        //        {
        //            var usr_user = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == PocID).FirstOrDefault();
        //            if (usr_user != null)
        //            {
        //                POCContactNo = usr_user.ContactNumber;
        //                invitingIserEmailId = usr_user.EmailId;
        //                invitingUserName = usr_user.FullName;
        //                Designation = usr_user.Designation;
        //            }
        //        }

        //        StringBuilder sbBody = new();

        //        Subject = "Welcome Aboard! Let's Get Your First Job Posted on Uplers";
        //        sbBody.Append("<div style='width:100%'>");

        //        if (varContactDetail != null)
        //        {
        //            sbBody.Append("Hello " + varContactDetail.FullName + ",");
        //        }

        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Welcome to Uplers! We're thrilled you're here to discover top talent from India.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Please login to your account to start posting jobs, exploring candidates, and managing your recruitment with ease.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (varContactDetail != null)
        //        {
        //            sbBody.Append($"Username : " + varContactDetail.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Password : " + varContactDetail.Password);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //        }
        //        if (varContactDetail != null)
        //        {
        //            //sbBody.Append("Kindly click <a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' class='link' href='" + reactClientPortalURL + "login?type=" + MyExtensions.Encrypt("IC") + "&emailid=" + varContactDetail.Username + "&password=" + varContactDetail.Password + "' target='_blank'>here</a> to login.");
        //            sbBody.Append("Kindly click <a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' class='link' href='" + reactClientPortalURL + "login?type=" + MyExtensions.Encrypt("IC") + "&contactId=" + MyExtensions.Encrypt(varContactDetail.Id.ToString()) + "' target='_blank'>here</a> to login.");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Why post a job on Uplers?");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<ul><li>Pre-vetted talent matched accurately as per your job requirement </li><li>Save your time and concentrate on the right talents.</li><li>Receive comprehensive reports and video screened responses for your job.</li><li>Dedicated support to assist you every step of the way.</li></ul>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Ready to find the perfect fit for your team? Don't wait any longer! Start by posting your job and get matched with the top talent on Uplers.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (PocID != 0)
        //        {
        //            sbBody.Append($"If you need help or have questions, please reach out to our executive {invitingUserName} at {invitingIserEmailId}");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //        }
        //        else
        //        {
        //            sbBody.Append($"Need assistance or have questions? We're just an email away.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //        }
        //        sbBody.Append("We look forward to supporting your hiring journey!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for choosing Uplers for your recruiting needs.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Warm regards,");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (PocID != 0)
        //        {
        //            sbBody.Append($"{invitingUserName}");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"{Designation}");
        //            sbBody.Append("<br/>");
        //            if (!string.IsNullOrEmpty(POCContactNo))
        //            {
        //                sbBody.Append($"{POCContactNo}");
        //                sbBody.Append("<br/>");
        //            }
        //            sbBody.Append("Website: https://www.uplers.com/");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"{invitingIserEmailId}");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<hr>");
        //        }
        //        else
        //        {
        //            sbBody.Append("Uplers");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<hr>");
        //        }
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");


        //        List<string> bccEmail = new List<string>
        //        {
        //            strBccEmail
        //        };

        //        List<string> bccEmailName = new List<string>
        //        {
        //            strBccEamilName
        //        };


        //        if (!string.IsNullOrEmpty(strBccEmail) && !string.IsNullOrEmpty(strBccEamilName))
        //        {
        //            CCEMAIL = CCEMAIL + "," + strBccEmail;
        //            CCEMAILNAME = CCEMAILNAME + "," + strBccEamilName;
        //        }

        //        if (CCEMAIL != "" && CCEMAILNAME != "")
        //        {
        //            CCEMAIL = CCEMAIL + "," + ccEmail;
        //            CCEMAILNAME = CCEMAILNAME + "," + ccEmailName;
        //        }
        //        else
        //        {
        //            CCEMAIL = ccEmail;
        //            CCEMAILNAME = ccEmailName;
        //        }


        //        #region SetParam for client email
        //        //emailOperator.SetToEmail(toEmail);
        //        //if (!string.IsNullOrEmpty(CCEMAIL) && !string.IsNullOrEmpty(CCEMAILNAME))
        //        //    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        //emailOperator.SetToEmailName(toEmailName);
        //        //emailOperator.SetSubject(Subject);
        //        //emailOperator.SetBody(sbBody.ToString());
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmailAWSSES(toEmail, Subject, sbBody.ToString());
        //        #endregion


        //        #region SetParam for Internal Team
        //        StringBuilder sbBodyCopiedText = new();
        //        sbBodyCopiedText.Append(_configuration["InternalEmailsAppendText"]);

        //        List<string> InternaltoEmail = new List<string>
        //        {
        //           invitingIserEmailId
        //        };

        //        List<string> InternaltoEmailName = new List<string>
        //        {
        //            invitingUserName
        //        };
        //        emailOperator.SetToEmail(InternaltoEmail);
        //        emailOperator.SetToEmailName(InternaltoEmailName);

        //        if (!string.IsNullOrEmpty(CCEMAIL) && !string.IsNullOrEmpty(CCEMAILNAME))
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);

        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBodyCopiedText.ToString() + " " + sbBody.ToString());

        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail(false, false, true);
        //        #endregion


        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        ////when Create NEw HR
        //public bool SendEmailForHRCreation(long hrid, string hrType)
        //{
        //    #region Variable
        //    bool emailsent = false;
        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(0, 0, 0, false, hrid, 0, 0);
        //    #endregion

        //    #region email content
        //    Subject = "New Hiring Request has been added to UTS - " + emailForBookTimeSlotModel.HR_Number;
        //    BodyCustom = "Hello Team,";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("New Hiring Request has been added to UTS. Please proceed with Debriefing details.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Below are the few details added about this Hiring Request.");
        //    sbBody.Append("<br/>");
        //    if (hrType == "New")
        //    {
        //        sbBody.Append("This is a new Hiring Request from a new prospect.");
        //    }
        //    else
        //    {
        //        sbBody.Append("This is a new Hiring Request from our existing client.");
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Company Name: " + emailForBookTimeSlotModel.CompanyName + "<br/>");
        //    sbBody.Append("Client Name: " + emailForBookTimeSlotModel.clientName + "<br/>");
        //    sbBody.Append("Client Email: " + emailForBookTimeSlotModel.ClientEmail + "<br/>");
        //    sbBody.Append("Submitted by Sales: " + emailForBookTimeSlotModel.salesName + "<br/>");
        //    sbBody.Append("HR ID: " + emailForBookTimeSlotModel.HR_Number + "<br/>");
        //    sbBody.Append("Position: " + emailForBookTimeSlotModel.position);
        //    sbBody.Append("<br/>");
        //    if (emailForBookTimeSlotModel.IsManaged)
        //    {
        //        sbBody.Append("Manage: Yes");
        //    }
        //    else
        //    {
        //        sbBody.Append("Self Manage: Yes");
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Thanks");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    #endregion

        //    #region Commened Code

        //    //string AdHocEMAIL = "";
        //    //string AdHocEMAILNAME = "";

        //    //string ManagedEMAIL = "";
        //    //string ManagedEMAILNAME = "";

        //    //AdHocEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("AdhocCCEmailIds");
        //    //AdHocEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("AdhocCCEmailName");

        //    //CCEMAIL += AdHocEMAIL;
        //    //CCEMAILNAME += AdHocEMAILNAME;

        //    //ManagedEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("ManagedCCEmailIds");
        //    //ManagedEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("ManagedCCEmailNames");

        //    //CCEMAIL += ManagedEMAIL;
        //    //CCEMAILNAME += ManagedEMAILNAME;

        //    #endregion

        //    #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //    string TOEMAIL = emailForBookTimeSlotModel.salesemailid;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.salesName;

        //    List<string> toemail = new List<string>() { TOEMAIL };
        //    List<string> toemailname = new List<string>() { TOEMAILNAME };

        //    string CCEMAIL = "";
        //    string CCEMAILNAME = "";

        //    MakeCCDetail ccDetails = MakeCCEmailDetails(emailForBookTimeSlotModel.HRSalesPersonID, false);
        //    if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //    {
        //        CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //        CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //    }
        //    #endregion

        //    #region SendEmail
        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail();
        //    emailsent = true;
        //    #endregion

        //    return emailsent;

        //}

        //public void SendEmailNotificationToInternalTeam_SlotGiven(long? TalentID, long? Contact_Id, long? HiringRequestID, PrgContactTimeZone prg_ContactTimeZone, List<RescheduleSlot> RecheduleSlots)
        //{
        //    #region Variables
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    #endregion

        //    #region GetDBData
        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID.Value, 0, 0, false, HiringRequestID.Value, Contact_Id.Value, 0);
        //    #endregion

        //    #region ContentBindingForInternalEmail
        //    string Subject = "Interview slots are shared - " + emailForBookTimeSlotModel.HR_Number;
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    BodyCustom = "Hello Team,";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Interview slots are shared, here is the details: ");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("HRID:  " + emailForBookTimeSlotModel.HR_Number);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Position: " + emailForBookTimeSlotModel.position);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Talent Name: " + string.Format("{0} {1}", emailForBookTimeSlotModel.TalentFirstName, emailForBookTimeSlotModel.TalentLastName));
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Client Email: " + emailForBookTimeSlotModel.clientName);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Interview date & time: ");

        //    if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //    {
        //        sbBody.Append("<br/>");
        //        int count = 0;
        //        foreach (var item in RecheduleSlots)
        //        {
        //            DateTime dateStartTime = DateTime.ParseExact(item.STRStartTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);
        //            DateTime dateEndTime = DateTime.ParseExact(item.STREndTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);

        //            count = count + 1;
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Slot " + count + " : " +
        //            dateStartTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            dateStartTime.ToString("hh:mm tt ") +
        //            dateEndTime.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //    }
        //    else
        //    {
        //        sbBody.Append("<br/>");
        //        int count = 0;
        //        foreach (var item in RecheduleSlots)
        //        {
        //            DateTime dateStartTime = DateTime.ParseExact(item.STRStartTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);
        //            DateTime dateEndTime = DateTime.ParseExact(item.STREndTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);

        //            count = count + 1;
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Slot " + count + " : " +
        //            dateStartTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            dateStartTime.ToString("hh:mm tt ") +
        //            dateEndTime.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }

        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview date & time (IST): ");
        //        sbBody.Append("<br/>");

        //        count = 0;

        //        foreach (var item in RecheduleSlots)
        //        {
        //            DateTime dateStartTime = DateTime.ParseExact(item.STRStartTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);
        //            DateTime dateEndTime = DateTime.ParseExact(item.STREndTime, "MM/dd/yyyy HH:mm", CultureInfo.CurrentCulture);

        //            count = count + 1;
        //            sbBody.Append("<br/>");

        //            sbBody.Append("Slot " + count + " : " +
        //            dateStartTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            dateStartTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateEndTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }
        //    }

        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Let's connect" + " <br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //    sbBody.Append("</div>");
        //    #endregion

        //    #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //    string TOEMAIL = emailForBookTimeSlotModel.salesemailid;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.salesName;

        //    List<string> toemail = new List<string>() { TOEMAIL };
        //    List<string> toemailname = new List<string>() { TOEMAILNAME };

        //    string CCEMAIL = "";
        //    string CCEMAILNAME = "";

        //    MakeCCDetail ccDetails = MakeCCEmailDetails(emailForBookTimeSlotModel.HRSalesPersonID, false, true, HiringRequestID.Value, TalentID.Value);
        //    if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //    {
        //        CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //        CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //    }

        //    #endregion

        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail();
        //    #endregion
        //}
        //public void SendEmailNotificationToInternalTeamwithZoomDetails_Schedule(Meeting zoomMeeting, int SlotType, string InterviewCallLink, long? TalentID, long? Contact_Id, DateTime scheduleTime, string TimeZone, long? HiringRequestID, DateTime dateTimeEnd, PrgContactTimeZone prg_ContactTimeZone, long ShortlistedId = 0)
        //{
        //    #region Variables
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    #endregion

        //    #region GetDBData
        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID.Value, 0, 0, false, HiringRequestID.Value, Contact_Id.Value, ShortlistedId);
        //    #endregion

        //    #region ContentBindingForInternalEmail
        //    string Subject = "Interview Confirmed - " + emailForBookTimeSlotModel.HR_Number;
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    BodyCustom = "Hello Team,";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("The Interview has been confirmed by talent and here is the details: ");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("HRID:  " + emailForBookTimeSlotModel.HR_Number);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Position: " + emailForBookTimeSlotModel.position);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Talent Name: " + string.Format("{0} {1}", emailForBookTimeSlotModel.TalentFirstName, emailForBookTimeSlotModel.TalentLastName));
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Client Email: " + emailForBookTimeSlotModel.clientName);
        //    sbBody.Append("<br/>");

        //    if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //    {
        //        sbBody.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //    }
        //    else
        //    {
        //        sbBody.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //        sbBody.Append("<br/>");

        //        sbBody.Append("Interview date & time (IST): " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //        dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //    }

        //    //sbBody.Append("Interview date: " + scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
        //    //sbBody.Append("<br/>");
        //    //sbBody.Append("Interview Time: " + scheduleTime.ToString("HH:mm ") + " " + TimeZone);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Position: " + emailForBookTimeSlotModel.position);
        //    sbBody.Append("<br/>");

        //    if (SlotType == 4)
        //    {
        //        sbBody.Append("Interview Link: " + InterviewCallLink);
        //    }
        //    else
        //    {
        //        sbBody.Append("Join Interview: " + zoomMeeting.join_url);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Kit: " + zoomMeeting.host_email);
        //        //sbBody.Append("<br/>");
        //        //sbBody.Append("Pass Code: " + zoomMeeting.password);
        //    }

        //    sbBody.Append("<br/>");
        //    if (emailForBookTimeSlotModel.IsManaged)
        //    {
        //        sbBody.Append("Manage: Yes");
        //    }
        //    else
        //    {
        //        sbBody.Append("Self Manage: Yes");
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Let's connect" + " <br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //    sbBody.Append("</div>");
        //    #endregion

        //    #region Commented code

        //    //string AdHocEMAIL = "";
        //    //string AdHocEMAILNAME = "";

        //    //string ManagedEMAIL = "";
        //    //string ManagedEMAILNAME = "";

        //    //string SMEMAIL = "";
        //    //string SMEMAILNAME = "";

        //    //if (emailForBookTimeSlotModel.IsAdHoc)
        //    //{
        //    //    AdHocEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("AdhocCCEmailIds");
        //    //    AdHocEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("AdhocCCEmailName");

        //    //    ccEmailId = InternalCCEmailID + AdHocEMAIL;
        //    //    ccEmailName = InternalCCEmailName + AdHocEMAILNAME;

        //    //}
        //    //else
        //    //{
        //    //    ccEmailId = InternalCCEmailID;
        //    //    ccEmailName = InternalCCEmailName;

        //    //}

        //    //if (emailForBookTimeSlotModel.IsManaged)
        //    //{
        //    //    ManagedEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("ManagedCCEmailIds");
        //    //    ManagedEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("ManagedCCEmailNames");

        //    //    ccEmailId = ccEmailId += ManagedEMAIL;
        //    //    ccEmailName = ccEmailName += ManagedEMAILNAME;
        //    //}
        //    //if (!emailForBookTimeSlotModel.IsManaged)
        //    //{
        //    //    SMEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("SMCCEmailIds");
        //    //    SMEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("SMCCEmailNames");

        //    //    ccEmailId = ccEmailId += SMEMAIL;
        //    //    ccEmailName = ccEmailName += SMEMAILNAME;
        //    //}

        //    #endregion

        //    var internal_descriptionCalender = new StringBuilder();

        //    if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //    {
        //        internal_descriptionCalender.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //    }
        //    else
        //    {
        //        internal_descriptionCalender.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //        internal_descriptionCalender.Append("<br/>");

        //        internal_descriptionCalender.Append("Interview date & time (IST): " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //        dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //    }

        //    //internal_descriptionCalender.Append("Interview date & time: " + scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString() + " " + TimeZone + ".");
        //    internal_descriptionCalender.Append("<br/>");

        //    if (SlotType == 4)
        //    {
        //        internal_descriptionCalender.Append("Interview Link: " + InterviewCallLink);
        //    }
        //    else
        //    {
        //        internal_descriptionCalender.Append("Interview Link: " + zoomMeeting.join_url);
        //        //internal_descriptionCalender.Append("<br/>");
        //        //internal_descriptionCalender.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //    }

        //    StringBuilder internal_str = new StringBuilder();
        //    internal_str.AppendLine("BEGIN:VCALENDAR");
        //    internal_str.AppendLine("PRODID:-//Schedule a Meeting");
        //    internal_str.AppendLine("VERSION:2.0");
        //    internal_str.AppendLine("METHOD:REQUEST");

        //    internal_str.AppendLine("BEGIN:VEVENT");
        //    internal_str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //    internal_str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //    internal_str.AppendLine("LOCATION: " + "Remote");
        //    internal_str.AppendLine("ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:talentconnect@uplers.com");
        //    internal_str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //    internal_str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", internal_descriptionCalender.ToString()));
        //    internal_str.AppendLine(string.Format("SUMMARY:{0}", "Interview with " + emailForBookTimeSlotModel.CompanyName + " | " + emailForBookTimeSlotModel.position));
        //    internal_str.AppendLine("BEGIN:VALARM");
        //    internal_str.AppendLine("TRIGGER:-PT15M");
        //    internal_str.AppendLine("ACTION:DISPLAY");
        //    internal_str.AppendLine("DESCRIPTION:Reminder");
        //    internal_str.AppendLine("END:VALARM");
        //    internal_str.AppendLine("END:VEVENT");
        //    internal_str.AppendLine("END:VCALENDAR");

        //    byte[] internal_byteArray = Encoding.ASCII.GetBytes(internal_str.ToString());
        //    MemoryStream internal_stream = new MemoryStream(internal_byteArray);

        //    Attachment internal_attach = new Attachment(internal_stream, "invite.ics");

        //    #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //    string TOEMAIL = emailForBookTimeSlotModel.salesemailid;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.salesName;

        //    List<string> toemail = new List<string>() { TOEMAIL };
        //    List<string> toemailname = new List<string>() { TOEMAILNAME };

        //    string CCEMAIL = "";
        //    string CCEMAILNAME = "";

        //    MakeCCDetail ccDetails = MakeCCEmailDetails(emailForBookTimeSlotModel.HRSalesPersonID, false, true, HiringRequestID.Value, TalentID.Value);
        //    if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //    {
        //        CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //        CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //    }

        //    #endregion

        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());
        //    emailOperator.SetAttachment(new List<Attachment>() { internal_attach });

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail();
        //    #endregion
        //}
        //public bool SendEmailNotificationForInternalForSecondRound(long hiringRequestId, Meeting zoomMeeting, long TalentID, long? Contact_Id, DateTime scheduleTime, string TimeZone, DateTime dateTimeEnd, PrgContactTimeZone prg_ContactTimeZone)
        //{
        //    try
        //    {
        //        bool emailsent = false;

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, hiringRequestId, 0, 0);
        //        #endregion

        //        int round = 1; int nextRound = 2;
        //        string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //        string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };

        //        GenTalentSelectedInterviewDetail talentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestId == hiringRequestId).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_InterviewDetails).Reload();
        //        if (talentSelected_InterviewDetails != null)
        //        {
        //            round = Convert.ToInt32(talentSelected_InterviewDetails.InterviewRound);
        //            nextRound = round + 1;
        //        }

        //        #region email Content

        //        string Subject = "Interview Confirmed for round " + nextRound.ToString() + " - " + emailDetails.HR_Number;
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        BodyCustom = "Hello Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The Interview has been confirmed for round " + nextRound.ToString() + " by talent and here is the details: ");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("HRID:  " + emailDetails.HR_Number);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Position: " + emailDetails.position);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Name: " + string.Format("{0} {1}", emailDetails.TalentFirstName, emailDetails.TalentLastName));
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Email: " + emailDetails.clientName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Company Name: " + emailDetails.CompanyName);
        //        sbBody.Append("<br/>");


        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            sbBody.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            sbBody.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            sbBody.Append("<br/>");

        //            sbBody.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }

        //        //sbBody.Append("Interview date: " + scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Time: " + scheduleTime.ToString("HH:mm ") + " " + TimeZone);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Join Interview: " + zoomMeeting.join_url);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Kit: " + zoomMeeting.host_email);
        //        //sbBody.Append("<br/>");
        //        //sbBody.Append("Pass Code: " + zoomMeeting.password);
        //        sbBody.Append("<br/>");
        //        if (emailDetails.IsManaged)
        //        {
        //            sbBody.Append("Manage: Yes");
        //        }
        //        else
        //        {
        //            sbBody.Append("Self Manage: Yes");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        #endregion



        //        var descriptionCalender = new StringBuilder();

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            descriptionCalender.Append("<br/>");

        //            descriptionCalender.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }

        //        //descriptionCalender.Append("Interview date & time: " + scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString() + " " + TimeZone + ".");
        //        descriptionCalender.Append("<br/>");
        //        descriptionCalender.Append("Interview Link: " + zoomMeeting.join_url + ".");

        //        //if (!string.IsNullOrEmpty(zoomMeeting.password))
        //        //{
        //        //    descriptionCalender.Append("<br/>");
        //        //    descriptionCalender.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //        //}

        //        StringBuilder str = new StringBuilder();
        //        str.AppendLine("BEGIN:VCALENDAR");
        //        str.AppendLine("PRODID:-//Schedule a Meeting");
        //        str.AppendLine("VERSION:2.0");
        //        str.AppendLine("METHOD:REQUEST");

        //        str.AppendLine("BEGIN:VEVENT");
        //        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine("LOCATION: " + "Remote");
        //        str.AppendLine($"ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:{clientEmailName}");
        //        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //        str.AppendLine(string.Format("SUMMARY:{0}", "Interview with " + emailDetails.CompanyName + " | " + emailDetails.position));
        //        str.AppendLine("BEGIN:VALARM");
        //        str.AppendLine("TRIGGER:-PT15M");
        //        str.AppendLine("ACTION:DISPLAY");
        //        str.AppendLine("DESCRIPTION:Reminder");
        //        str.AppendLine("END:VALARM");
        //        str.AppendLine("END:VEVENT");
        //        str.AppendLine("END:VCALENDAR");

        //        byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //        MemoryStream stream = new MemoryStream(byteArray);

        //        Attachment attach = new Attachment(stream, "invite.ics");

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested
        //        string TOEMAIL = emailDetails.salesemailid;
        //        string TOEMAILNAME = emailDetails.salesName;

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        MakeCCDetail ccDetails = MakeCCEmailDetails(emailDetails.HRSalesPersonID, false, true, hiringRequestId, TalentID);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }

        //        #endregion

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());
        //            emailOperator.SetAttachment(new List<Attachment>() { attach });

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        ////Send Email to InternalTeam When Client Selects any option for the interviewers and opts for giving slots right away For another round Later
        //public bool SendEmailNotificationToInternalTeamForSecondRoundLater(long TalentID, long? Contact_Id, long HiringRequestID, long? HiringRequestDetailID)
        //{
        //    try
        //    {
        //        bool emailsent = false;

        //        int round = 1; int nextRound = 2; string NextRoundoption = string.Empty;

        //        string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //        string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, 0);
        //        #endregion

        //        GenTalentSelectedInterviewDetail talentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_InterviewDetails).Reload();
        //        if (talentSelected_InterviewDetails != null)
        //        {
        //            round = Convert.ToInt32(talentSelected_InterviewDetails.InterviewRound);
        //            nextRound = round + 1;
        //        }

        //        GenTalentSelectedNextRoundInterviewDetail talentSelected_NextRound_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedNextRoundInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestDetailId == HiringRequestDetailID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_NextRound_InterviewDetails).Reload();

        //        if (talentSelected_NextRound_InterviewDetails != null)
        //        {
        //            if (talentSelected_NextRound_InterviewDetails.NextRoundOption == "Append")
        //            {
        //                NextRoundoption = "More";
        //            }
        //            else if (talentSelected_NextRound_InterviewDetails.NextRoundOption == "Yes")
        //            {
        //                NextRoundoption = "Existing";
        //            }
        //            else
        //            {
        //                NextRoundoption = "New";
        //            }
        //        }

        //        #region Email content

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "Client have requested for a " + digits_wordsRoman[nextRound] + " round of interview with " + emailDetails.TalentName + " - " + emailDetails.HR_Number;
        //        BodyCustom = "Hello Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Quick Update!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The Client has requested for a " + digits_wordsRoman[nextRound] + " round of interview with " + emailDetails.TalentName + ", the shortlisted candidate for the " + emailDetails.position + " position.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Also client has selected " + NextRoundoption + " Interviewer Penalist, however slots are yet to be provided.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Below are the details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Name: " + emailDetails.TalentName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hiring Position: " + emailDetails.position);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Shortlisted for Client: " + emailDetails.clientName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please look into the further course of action associated with this round " + digits_words[nextRound] + " interview.");
        //        sbBody.Append("<br/>");
        //        if (emailDetails.IsManaged)
        //        {
        //            sbBody.Append("Manage: Yes");
        //        }
        //        else
        //        {
        //            sbBody.Append("Self Manage: Yes");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        #endregion

        //        #region Commented code
        //        //string ManagedEMAIL = "";
        //        //string ManagedEMAILNAME = "";

        //        //string SMEMAIL = "";
        //        //string SMEMAILNAME = "";

        //        //if (emailDetails.IsAdHoc)
        //        //{
        //        //    string AdHocEMAIL = "";
        //        //    string AdHocEMAILNAME = "";
        //        //    AdHocEMAIL = EmailDatabaseContentProvider.GetAdHocCCEmailIdValues();
        //        //    AdHocEMAILNAME = EmailDatabaseContentProvider.GetAdHocCCEmailNameValues();

        //        //    CCEMAIL = ccEmail + InternalCCEmailID + AdHocEMAIL;
        //        //    CCEMAILNAME = ccEmailName + InternalCCEmailName + AdHocEMAILNAME;
        //        //}
        //        //else
        //        //{
        //        //    CCEMAIL = ccEmail + InternalCCEmailID;
        //        //    CCEMAILNAME = ccEmailName + InternalCCEmailName;
        //        //}

        //        //if (emailDetails.IsManaged)
        //        //{
        //        //    ManagedEMAIL = EmailDatabaseContentProvider.GetManagedCCEmailIdValues();
        //        //    ManagedEMAILNAME = EmailDatabaseContentProvider.GetManagedCCEmailNameValues();

        //        //    CCEMAIL = CCEMAIL + ManagedEMAIL;
        //        //    CCEMAILNAME = CCEMAILNAME + ManagedEMAILNAME;
        //        //}
        //        //else
        //        //{
        //        //    SMEMAIL = EmailDatabaseContentProvider.GetSMCCEmailIdValues();
        //        //    SMEMAILNAME = EmailDatabaseContentProvider.GetSMCCEmailNameValues();

        //        //    CCEMAIL = CCEMAIL + SMEMAIL;
        //        //    CCEMAILNAME = CCEMAILNAME + SMEMAILNAME;
        //        //}
        //        #endregion

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //        string TOEMAIL = emailDetails.salesemailid;
        //        string TOEMAILNAME = emailDetails.salesName;

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        MakeCCDetail ccDetails = MakeCCEmailDetails(emailDetails.HRSalesPersonID, true, true, HiringRequestID, TalentID);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }

        //        #endregion

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception e) { return false; }
        //}
        ////Send Email to InternalTeam When Client Selects any option for the interviewers and opts for giving slots right away For another round
        //public bool SendEmailNotificationToInternalTeamForSecondRound(long TalentID, long? Contact_Id, long HiringRequestID, long? HiringRequestDetailID)
        //{
        //    try
        //    {
        //        bool emailsent = false;

        //        int round = 1; int nextRound = 2; string NextRoundoption = string.Empty;

        //        string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //        string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, 0);
        //        #endregion

        //        GenTalentSelectedInterviewDetail talentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_InterviewDetails).Reload();
        //        if (talentSelected_InterviewDetails != null)
        //        {
        //            round = Convert.ToInt32(talentSelected_InterviewDetails.InterviewRound);
        //            nextRound = round + 1;
        //        }

        //        GenTalentSelectedNextRoundInterviewDetail talentSelected_NextRound_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedNextRoundInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestDetailId == HiringRequestDetailID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_NextRound_InterviewDetails).Reload();
        //        if (talentSelected_NextRound_InterviewDetails != null)
        //        {
        //            if (talentSelected_NextRound_InterviewDetails.NextRoundOption == "Append")
        //            {
        //                NextRoundoption = "More";
        //            }
        //            else if (talentSelected_NextRound_InterviewDetails.NextRoundOption == "Yes")
        //            {
        //                NextRoundoption = "Existing";
        //            }
        //            else
        //            {
        //                NextRoundoption = "New";
        //            }
        //        }

        //        #region Email content

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "Client have requested for a " + digits_wordsRoman[nextRound] + " round of interview with " + emailDetails.TalentName + " - " + emailDetails.HR_Number;
        //        BodyCustom = "Hello Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Quick Update!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The Client has requested for a " + digits_wordsRoman[nextRound] + " round of interview with " + emailDetails.TalentName + ", the shortlisted candidate for the " + emailDetails.position + " position.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Also client has selected " + NextRoundoption + " Interviewer Penalist along with interview slots for round " + nextRound.ToString() + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Below are the details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Name: " + emailDetails.TalentName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hiring Position: " + emailDetails.position);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Shortlisted for Client: " + emailDetails.clientName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please look into the further course of action associated with this round " + digits_words[nextRound] + " interview.");
        //        sbBody.Append("<br/>");
        //        if (emailDetails.IsManaged)
        //        {
        //            sbBody.Append("Manage: Yes");
        //        }
        //        else
        //        {
        //            sbBody.Append("Self Manage: Yes");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        #endregion

        //        #region commented code

        //        //string ManagedEMAIL = "";
        //        //string ManagedEMAILNAME = "";

        //        //string SMEMAIL = "";
        //        //string SMEMAILNAME = "";

        //        //if (emailDetails.IsAdHoc)
        //        //{
        //        //    string AdHocEMAIL = "";
        //        //    string AdHocEMAILNAME = "";
        //        //    AdHocEMAIL = EmailDatabaseContentProvider.GetAdHocCCEmailIdValues();
        //        //    AdHocEMAILNAME = EmailDatabaseContentProvider.GetAdHocCCEmailNameValues();

        //        //    CCEMAIL = ccEmail + InternalCCEmailID + AdHocEMAIL;
        //        //    CCEMAILNAME = ccEmailName + InternalCCEmailName + AdHocEMAILNAME;
        //        //}
        //        //else
        //        //{
        //        //    CCEMAIL = ccEmail + InternalCCEmailID;
        //        //    CCEMAILNAME = ccEmailName + InternalCCEmailName;
        //        //}

        //        //if (emailDetails.IsManaged)
        //        //{
        //        //    ManagedEMAIL = EmailDatabaseContentProvider.GetManagedCCEmailIdValues();
        //        //    ManagedEMAILNAME = EmailDatabaseContentProvider.GetManagedCCEmailNameValues();

        //        //    CCEMAIL = CCEMAIL + ManagedEMAIL;
        //        //    CCEMAILNAME = CCEMAILNAME + ManagedEMAILNAME;
        //        //}
        //        //else
        //        //{
        //        //    SMEMAIL = EmailDatabaseContentProvider.GetSMCCEmailIdValues();
        //        //    SMEMAILNAME = EmailDatabaseContentProvider.GetSMCCEmailNameValues();

        //        //    CCEMAIL = CCEMAIL + SMEMAIL;
        //        //    CCEMAILNAME = CCEMAILNAME + SMEMAILNAME;
        //        //}

        //        #endregion

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested
        //        string TOEMAIL = emailDetails.salesemailid;
        //        string TOEMAILNAME = emailDetails.salesName;

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        MakeCCDetail ccDetails = MakeCCEmailDetails(emailDetails.HRSalesPersonID, true, true, HiringRequestID, TalentID);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }
        //        #endregion

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception e) { return false; }
        //}


        //public void SendRescheduleInterviewSlotCommonEmailWithContent(Int64 User_ID, long TalentID, string RescheduleRequestBy, int statusID, int slotType, GenTalent gen_Talent, GenContact gen_Contact, Dictionary<string, string> dataContent, int HR_ID, PrgContactTimeZone prg_ContactTimeZone)
        //{

        //    #region Variables
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    #region GetDBData
        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HR_ID, 0, 0);
        //    #endregion

        //    int roundNumber = Convert.ToInt32(dataContent.Where(x => x.Key == "RoundNo").FirstOrDefault().Value);
        //    string round = dataContent.Where(x => x.Key == "Round").FirstOrDefault().Value;

        //    #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //    string TOEMAIL = emailForBookTimeSlotModel.salesemailid;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.salesName;

        //    string CCEMAIL = "";
        //    string CCEMAILNAME = "";

        //    MakeCCDetail ccDetails = MakeCCEmailDetails(emailForBookTimeSlotModel.HRSalesPersonID, true, true, HR_ID, TalentID);
        //    if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //    {
        //        CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //        CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //    }

        //    List<string> toemail = new List<string>() { TOEMAIL };
        //    List<string> toemailname = new List<string>() { TOEMAILNAME };
        //    #endregion

        //    #region commented code

        //    //string InternalCCEmailID = "";
        //    //string InternalCCEmailName = "";
        //    //string AdHocEMAIL = "";
        //    //string AdHocEMAILNAME = "";
        //    //string ManagedEMAIL = "";
        //    //string ManagedEMAILNAME = "";
        //    //string SMEMAIL = "";
        //    //string SMEMAILNAME = "";

        //    //InternalCCEmailID = emailDatabaseContentProvider.GetCCEmailIdValues();
        //    //InternalCCEmailName = emailDatabaseContentProvider.GetCCEmailNameValues();

        //    //if (emailForBookTimeSlotModel.IsAdHoc)
        //    //{
        //    //    AdHocEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("AdhocCCEmailIds");
        //    //    AdHocEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("AdhocCCEmailName");

        //    //    CCMEMAIL += AdHocEMAIL;
        //    //    CCMEMAILNAME += AdHocEMAILNAME;
        //    //}
        //    //if (emailForBookTimeSlotModel.IsManaged)
        //    //{
        //    //    ManagedEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("ManagedCCEmailIds");
        //    //    ManagedEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("ManagedCCEmailNames");

        //    //    CCMEMAIL += ManagedEMAIL;
        //    //    CCMEMAILNAME += ManagedEMAILNAME;
        //    //}
        //    //if (!emailForBookTimeSlotModel.IsManaged)
        //    //{
        //    //    SMEMAIL += emailDatabaseContentProvider.GetCCEmailIdValues("SMCCEmailIds");
        //    //    SMEMAILNAME += emailDatabaseContentProvider.GetCCEmailNameValues("SMCCEmailNames");

        //    //    CCMEMAIL += SMEMAIL;
        //    //    CCMEMAILNAME += SMEMAILNAME;
        //    //}
        //    //if (emailForBookTimeSlotModel.IsClientNotificationSend)
        //    //{
        //    //    clientEmailContent = GetEmailContentForClient(slotType, statusID, RescheduleRequestBy, roundNumber, emailForBookTimeSlotModel, prg_ContactTimeZone.ShortName);
        //    //}
        //    //if (emailForBookTimeSlotModel.IsTalentNotificationSend)
        //    //{
        //    //    talentEmailContent = GetEmailContentForTalent(slotType, statusID, RescheduleRequestBy, roundNumber, emailForBookTimeSlotModel, prg_ContactTimeZone.ShortName);
        //    //}
        //    #endregion

        //    var clientEmailContent = new RescheduleInterviewEmailContent();
        //    var talentEmailContent = new RescheduleInterviewEmailContent();
        //    var teamEmailContent = new RescheduleInterviewEmailContent();
        //    teamEmailContent = GetEmailContentForInternalTeam(slotType, statusID, RescheduleRequestBy, roundNumber, prg_ContactTimeZone.ShortName);


        //    #region SlotType=Add Final Interview Slot and Status = Interview Scheduled
        //    //if slotype 2= Add and Confrim redio checked and status = interview rescheduled then
        //    // Generate Content for 
        //    StringBuilder str = new StringBuilder();
        //    if (slotType == 2 && statusID == 4)
        //    {
        //        //get timezone from Dictionary
        //        string TimeZone = dataContent.Where(x => x.Key == "timeZone").FirstOrDefault().Value;

        //        string Description = emailDatabaseContentProvider.GetPrgContactTimeZone(TimeZone);
        //        string TimeZoneTitleValue = string.Empty;

        //        //get Start time and end time from Dictionary
        //        DateTime scheduleTime = CommonLogic.ConvertString2DateTime(dataContent.Where(x => x.Key == "StartTime").FirstOrDefault().Value);
        //        DateTime dateTimeEnd = CommonLogic.ConvertString2DateTime(dataContent.Where(x => x.Key == "EndTime").FirstOrDefault().Value);
        //        var UTCStartTime = scheduleTime.ToUniversalTime();
        //        var UTCEndTime = dateTimeEnd.ToUniversalTime();

        //        DateTime NewStartTime;
        //        DateTime NewEndTime;

        //        //get position from Dictionary
        //        string Position = dataContent.Where(x => x.Key == "Position").FirstOrDefault().Value;
        //        if (!string.IsNullOrEmpty(Description))
        //        {
        //            TimeZoneTitleValue = Description.Split(')')[1].Trim();
        //            NewStartTime = TimeZoneInfo.ConvertTimeToUtc(scheduleTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneTitleValue));
        //            NewEndTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeEnd, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneTitleValue));
        //        }
        //        else
        //        {
        //            NewStartTime = scheduleTime;
        //            NewEndTime = dateTimeEnd;
        //        }

        //        var descriptionCalender = new StringBuilder();

        //        //Get zoomMeeting_join_url and zoomMeeting_password from Dictionary
        //        string zoomMeeting_join_url = dataContent.Where(x => x.Key == "InterviewLink").FirstOrDefault().Value;
        //        //string zoomMeeting_password = dataContent.Where(x => x.Key == "PassCode").FirstOrDefault().Value;

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            descriptionCalender.Append("<br/>");

        //            descriptionCalender.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }


        //        //descriptionCalender.Append("Interview date & time: " + scheduleTime.ToString() + " " + TimeZone);
        //        descriptionCalender.Append("<br/>");
        //        descriptionCalender.Append("Interview Link: " + zoomMeeting_join_url);
        //        descriptionCalender.Append("<br/>");
        //        //descriptionCalender.Append("Pass Code : " + zoomMeeting_password + " (Optional)");

        //        //if (!string.IsNullOrEmpty(zoomMeeting_password))
        //        //{
        //        //    descriptionCalender.Append("<br/>");
        //        //    descriptionCalender.Append("Pass Code : " + zoomMeeting_password + " (Optional)");
        //        //}

        //        str.AppendLine("BEGIN:VCALENDAR");
        //        str.AppendLine("PRODID:-//Schedule a Meeting");
        //        str.AppendLine("VERSION:2.0");
        //        str.AppendLine("METHOD:REQUEST");

        //        str.AppendLine("BEGIN:VEVENT");

        //        //str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", NewStartTime));
        //        //str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", NewEndTime));
        //        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine("LOCATION: " + "Remote");
        //        str.AppendLine($"ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:{clientEmailName}");
        //        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //        str.AppendLine(string.Format("SUMMARY:{0}", "Interview with " + emailForBookTimeSlotModel.TalentName + " | " + Position));
        //        str.AppendLine("BEGIN:VALARM");
        //        str.AppendLine("TRIGGER:-PT15M");
        //        str.AppendLine("ACTION:DISPLAY");
        //        str.AppendLine("DESCRIPTION:Reminder");
        //        str.AppendLine("END:VALARM");
        //        str.AppendLine("END:VEVENT");
        //        str.AppendLine("END:VCALENDAR");
        //    }
        //    #endregion

        //    #region Replace Value with Matched key with Content from Dictionary
        //    string subject = "";
        //    string content = "";
        //    foreach (var item in dataContent)
        //    {
        //        if (slotType == 3)
        //        {
        //            //For Client Content
        //            //if (emailForBookTimeSlotModel.IsClientNotificationSend &&
        //            //    clientEmailContent != null &&
        //            //    !string.IsNullOrEmpty(clientEmailContent.Subject) &&
        //            //    !string.IsNullOrEmpty(clientEmailContent.Content))
        //            //{
        //            //    subject = clientEmailContent.Subject;
        //            //    content = clientEmailContent.Content;
        //            //    subject = subject.Replace("##" + item.Key + "##", item.Value);
        //            //    content = content.Replace("##" + item.Key + "##", item.Value);
        //            //    clientEmailContent.Subject = subject;
        //            //    clientEmailContent.Content = content;
        //            //}
        //        }
        //        else
        //        {
        //            //For Client Content
        //            //if (emailForBookTimeSlotModel.IsClientNotificationSend &&
        //            //    clientEmailContent != null &&
        //            //    !string.IsNullOrEmpty(clientEmailContent.Subject) &&
        //            //    !string.IsNullOrEmpty(clientEmailContent.Content))
        //            //{
        //            //    subject = clientEmailContent.Subject;
        //            //    content = clientEmailContent.Content;
        //            //    subject = subject.Replace("##" + item.Key + "##", !string.IsNullOrEmpty(item.Value) ? item.Value : "");
        //            //    content = content.Replace("##" + item.Key + "##", !string.IsNullOrEmpty(item.Value) ? item.Value : "");
        //            //    clientEmailContent.Subject = subject;
        //            //    clientEmailContent.Content = content;
        //            //}

        //            //For Talent Content
        //            //if (emailForBookTimeSlotModel.IsTalentNotificationSend &&
        //            //    talentEmailContent != null &&
        //            //    !string.IsNullOrEmpty(talentEmailContent.Subject) &&
        //            //    !string.IsNullOrEmpty(talentEmailContent.Content))
        //            //{
        //            //    subject = talentEmailContent.Subject;
        //            //    content = talentEmailContent.Content;
        //            //    subject = subject.Replace("##" + item.Key + "##", !string.IsNullOrEmpty(item.Value) ? item.Value : "");
        //            //    content = content.Replace("##" + item.Key + "##", !string.IsNullOrEmpty(item.Value) ? item.Value : "");
        //            //    talentEmailContent.Subject = subject;
        //            //    talentEmailContent.Content = content;
        //            //}

        //            //For Team Content
        //            if (teamEmailContent != null &&
        //                !string.IsNullOrEmpty(teamEmailContent.Subject) &&
        //                !string.IsNullOrEmpty(teamEmailContent.Content))
        //            {
        //                subject = teamEmailContent.Subject;
        //                content = teamEmailContent.Content;
        //                subject = subject.Replace("##" + item.Key + "##", !string.IsNullOrEmpty(item.Value) ? item.Value : "");
        //                if (content != null)
        //                {
        //                    content = content.Replace("##" + item.Key + "##", !string.IsNullOrEmpty(item.Value) ? item.Value : "");
        //                }
        //                teamEmailContent.Subject = subject;
        //                teamEmailContent.Content = content;
        //            }
        //        }
        //    }
        //    #endregion

        //    #region SendEmail

        //    //List<string> emailClient = new List<string>() { emailForBookTimeSlotModel.ClientEmail };
        //    //List<string> nameClient = new List<string>() { emailForBookTimeSlotModel.clientName };

        //    //List<string> emailTalent = new List<string>() { emailForBookTimeSlotModel.TalentEmail };
        //    //List<string> nameTalent = new List<string>() { emailForBookTimeSlotModel.TalentName };
        //    if (slotType == 2 && statusID == 4)
        //    {
        //        byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //        MemoryStream stream = new MemoryStream(byteArray);

        //        Attachment attach = new Attachment(stream, "invite.ics");

        //        //Mail to Client
        //        //if (emailForBookTimeSlotModel.IsClientNotificationSend &&
        //        //    clientEmailContent != null &&
        //        //    !string.IsNullOrEmpty(clientEmailContent.Subject) &&
        //        //    !string.IsNullOrEmpty(clientEmailContent.Content))
        //        //{
        //        //    #region SetParam
        //        //    emailOperator.SetToEmail(emailClient);
        //        //    emailOperator.SetToEmailName(nameClient);
        //        //    emailOperator.SetCCEmail(CCMEMAIL, CCMEMAILNAME);
        //        //    emailOperator.SetSubject(clientEmailContent.Subject);
        //        //    emailOperator.SetBody(clientEmailContent.Content.ToString());
        //        //    emailOperator.SetAttachment(new List<Attachment>() { attach });
        //        //    #endregion
        //        //    emailOperator.SendEmail(false, true);
        //        //}

        //        //Mail to Talent
        //        //if (emailForBookTimeSlotModel.IsTalentNotificationSend &&
        //        //    talentEmailContent != null &&
        //        //    !string.IsNullOrEmpty(talentEmailContent.Subject) &&
        //        //    !string.IsNullOrEmpty(talentEmailContent.Content))
        //        //{
        //        //    #region SetParam
        //        //    emailOperator.SetToEmail(emailTalent);
        //        //    emailOperator.SetToEmailName(nameTalent);
        //        //    emailOperator.SetCCEmail(CCMEMAIL, CCMEMAILNAME);
        //        //    emailOperator.SetSubject(talentEmailContent.Subject);
        //        //    emailOperator.SetBody(talentEmailContent.Content.ToString());
        //        //    emailOperator.SetAttachment(new List<Attachment>() { attach });
        //        //    #endregion
        //        //    emailOperator.SendEmail();
        //        //}

        //        //Mail to Team
        //        if (teamEmailContent != null &&
        //            !string.IsNullOrEmpty(teamEmailContent.Subject) &&
        //            !string.IsNullOrEmpty(teamEmailContent.Content))
        //        {
        //            #region SetParam
        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            emailOperator.SetSubject(teamEmailContent.Subject);
        //            emailOperator.SetBody(teamEmailContent.Content.ToString());
        //            emailOperator.SetAttachment(new List<Attachment>() { attach });
        //            #endregion
        //            emailOperator.SendEmail();
        //        }
        //    }
        //    else if (slotType == 3)
        //    {
        //        //if (clientEmailContent != null &&
        //        //    !string.IsNullOrEmpty(clientEmailContent.Subject) &&
        //        //    !string.IsNullOrEmpty(clientEmailContent.Content))
        //        //{
        //        //    #region SetParam
        //        //    emailOperator.SetToEmail(emailClient);
        //        //    emailOperator.SetToEmailName(nameClient);
        //        //    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        //    emailOperator.SetSubject(clientEmailContent.Subject);
        //        //    emailOperator.SetBody(clientEmailContent.Content.ToString());
        //        //    #endregion
        //        //    emailOperator.SendEmail();
        //        //}
        //    }
        //    else
        //    {
        //        //Mail to Client
        //        //if (emailForBookTimeSlotModel.IsClientNotificationSend &&
        //        //    clientEmailContent != null &&
        //        //    !string.IsNullOrEmpty(clientEmailContent.Subject) &&
        //        //    !string.IsNullOrEmpty(clientEmailContent.Content))
        //        //{
        //        //    #region SetParam
        //        //    emailOperator.SetToEmail(emailClient);
        //        //    emailOperator.SetToEmailName(nameClient);
        //        //    emailOperator.SetCCEmail(CCMEMAIL, CCMEMAILNAME);
        //        //    emailOperator.SetSubject(clientEmailContent.Subject);
        //        //    emailOperator.SetBody(clientEmailContent.Content.ToString());
        //        //    #endregion
        //        //    emailOperator.SendEmail(false, true);
        //        //}
        //        //Mail to Talent
        //        //if (emailForBookTimeSlotModel.IsTalentNotificationSend &&
        //        //    talentEmailContent != null &&
        //        //    !string.IsNullOrEmpty(talentEmailContent.Subject) &&
        //        //    !string.IsNullOrEmpty(talentEmailContent.Content))
        //        //{
        //        //    #region SetParam
        //        //    emailOperator.SetToEmail(emailTalent);
        //        //    emailOperator.SetToEmailName(nameTalent);
        //        //    emailOperator.SetCCEmail(CCMEMAIL, CCMEMAILNAME);
        //        //    emailOperator.SetSubject(talentEmailContent.Subject);
        //        //    emailOperator.SetBody(talentEmailContent.Content.ToString());
        //        //    #endregion
        //        //    emailOperator.SendEmail();
        //        //}

        //        //Mail to Team
        //        if (teamEmailContent != null &&
        //            !string.IsNullOrEmpty(teamEmailContent.Subject) &&
        //            !string.IsNullOrEmpty(teamEmailContent.Content))
        //        {
        //            #region SetParam
        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            emailOperator.SetSubject(teamEmailContent.Subject);
        //            emailOperator.SetBody(teamEmailContent.Content.ToString());
        //            #endregion
        //            emailOperator.SendEmail();
        //        }
        //    }
        //    #endregion
        //}
        //public void SendEmailOnDeleteHRAfterOnBoard(Int64 OnBoardId, string Deletereason, sproc_GetOnBoardData_Result sproc_GetOnBoardData_Result)
        //{
        //    #region Variables
        //    string Engagement_Number = "";
        //    string HR_Number = "";
        //    string ClientName = "", TalentName = "", Currency = "", ReasonForLoss = "";
        //    decimal Total_Invoice_Amount = 0, Total_Paid_Amount = 0, Estimated_Remaining_payment_Amount = 0;
        //    DateTime CancelledDate = DateTime.Now;
        //    #endregion

        //    if (sproc_GetOnBoardData_Result != null)
        //    {
        //        Engagement_Number = sproc_GetOnBoardData_Result.Engagement_Number;
        //        HR_Number = sproc_GetOnBoardData_Result.HR_ID;
        //        ClientName = sproc_GetOnBoardData_Result.Client_Name;
        //        TalentName = sproc_GetOnBoardData_Result.Talent_Name;
        //        Total_Invoice_Amount = sproc_GetOnBoardData_Result.Total_Invoice_Amount;
        //        Total_Paid_Amount = sproc_GetOnBoardData_Result.Total_Paid_Amount;
        //        Estimated_Remaining_payment_Amount = sproc_GetOnBoardData_Result.Estimated_Remaining_payment_Amount;
        //        Currency = sproc_GetOnBoardData_Result.Currency;
        //        CancelledDate = sproc_GetOnBoardData_Result.CancelledDate;
        //        ReasonForLoss = Deletereason;
        //    }
        //    else
        //    {
        //        return;
        //    }


        //    #region Binding
        //    string Subject = "";
        //    string BodyCustom = "";

        //    StringBuilder sbBody = new StringBuilder();

        //    Subject = "Delete HR After On Board";
        //    BodyCustom = "Hello Team,";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Greetings for the day!");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("This is to inform you that the below HR has been deleted/Cancelled. Please find below details for your reference:");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("HR ID: " + HR_Number + "<br/>");
        //    if (!string.IsNullOrEmpty(ClientName))
        //    {
        //        sbBody.Append("Client Name: " + ClientName + "<br/>");
        //    }
        //    if (!string.IsNullOrEmpty(TalentName))
        //    {
        //        sbBody.Append("Talent Name: " + TalentName + "<br/>");
        //    }
        //    sbBody.Append("Cancelled/ Deleted Date: " + CancelledDate + "<br/>");
        //    sbBody.Append("Reason for Loss: " + ReasonForLoss + "<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    //sbBody.Append("Also, this is to update you that HR has been moved to “Completed” Status." + "<br/>");
        //    sbBody.Append("Also, this is to update you that HR has been moved to “Won” Status." + "<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Regards,");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("</div>");
        //    #endregion

        //    EmailOperator emailOperator = new EmailOperator(_configuration);

        //    #region SetParam
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());
        //    #endregion

        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail();
        //}
        //public void SendEmailOnArchiveInvoice(long InvoiceId, long OnBoardId, long TalentID, Boolean IsReplacement)
        //{
        //    #region Variable
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, InvoiceId, OnBoardId, IsReplacement, 0, 0, 0);

        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Email content

        //    Subject = "Archive Invoice Details.";
        //    BodyCustom = "Hello Team,";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Greetings for the day!");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Please be informed that " + emailForBookTimeSlotModel.InvoiceNumber + " has been archived from the invoice application and the Zoho (if it was sent).");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Below are the details for your reference:");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Invoice Number: " + emailForBookTimeSlotModel.InvoiceNumber + "<br/>");
        //    sbBody.Append("Amount: " + emailForBookTimeSlotModel.InvoiceAmount + "<br/>");
        //    sbBody.Append("Currency: " + emailForBookTimeSlotModel.Currancy + "<br/>");
        //    if (IsReplacement)
        //        sbBody.Append("Reason: " + emailForBookTimeSlotModel.Reason + "<br/>");
        //    sbBody.Append("Invoice Creation date: " + emailForBookTimeSlotModel.InvoiceCreationDate + "<br/>");
        //    sbBody.Append("<br/>");
        //    if (emailForBookTimeSlotModel.IsManaged)
        //    {
        //        sbBody.Append("Manage: Yes");
        //    }
        //    else
        //    {
        //        sbBody.Append("Self Manage: Yes");
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Thanks");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    #endregion

        //    #region Commented Code

        //    //string TOEMAIL = emailForBookTimeSlotModel.SalesUseEMail;
        //    //string TOEMAILNAME = emailForBookTimeSlotModel.SalesUserName;

        //    //string AdHocEMAIL = "";
        //    //string AdHocEMAILNAME = "";

        //    //string ManagedEMAIL = "";
        //    //string ManagedEMAILNAME = "";

        //    //string SMEMAIL = "";
        //    //string SMEMAILNAME = "";

        //    //string CCEMAIL = "";
        //    //string CCEMAILNAME = "";

        //    //CCEMAIL = Config.CCEmailId + ',' + Config.CC1EmailId + ',' + Config.TSCEmailId;
        //    //CCEMAILNAME = Config.CCEmailName + ',' + Config.CC1EmailName + ',' + Config.TSCEmailName;

        //    //AdHocEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("AdhocCCEmailIds");
        //    //AdHocEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("AdhocCCEmailName");

        //    //CCEMAIL += AdHocEMAIL;
        //    //CCEMAILNAME += AdHocEMAILNAME;

        //    //ManagedEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("ManagedCCEmailIds");
        //    //ManagedEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("ManagedCCEmailNames");

        //    //CCEMAIL += ManagedEMAIL;
        //    //CCEMAILNAME += ManagedEMAILNAME;

        //    //SMEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("SMCCEmailIds");
        //    //SMEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("SMCCEmailNames");

        //    //CCEMAIL += SMEMAIL;
        //    //CCEMAILNAME += SMEMAILNAME;

        //    #endregion

        //    #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //    string TOEMAIL = emailForBookTimeSlotModel.salesemailid;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.salesName;

        //    List<string> toemail = new List<string>() { TOEMAIL };
        //    List<string> toemailname = new List<string>() { TOEMAILNAME };

        //    string CCEMAIL = "";
        //    string CCEMAILNAME = "";

        //    MakeCCDetail ccDetails = MakeCCEmailDetails(emailForBookTimeSlotModel.HRSalesPersonID, false);
        //    if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //    {
        //        CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //        CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //    }
        //    #endregion

        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail();
        //    #endregion

        //}
        //public void BindEmailForError(List<string> toEmail, List<string> toEmailName, string subject, string body = null)
        //{
        //    EmailOperator emailOperator = new EmailOperator(_configuration);

        //    #region SetParam
        //    emailOperator.SetToEmail(toEmail);
        //    emailOperator.SetToEmailName(toEmailName);
        //    emailOperator.SetSubject(subject);
        //    emailOperator.SetBody(body);
        //    #endregion

        //    if (!string.IsNullOrEmpty(subject))
        //        emailOperator.SendEmail(true);
        //}
        //public void SendEmailNotificationToSalesTeam(Int64 TalentID, long HiringRequest_ID)
        //{
        //    #region Variable
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequest_ID, 0, 0);

        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Email Content
        //    Subject = "Talent has been selected for Hiring Request - " + emailForBookTimeSlotModel.HR_Number;
        //    BodyCustom = "Hello Team,";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/> New talent has been selected for the hiring request " + emailForBookTimeSlotModel.HR_Number + " for the client " + emailForBookTimeSlotModel.CompanyName + " requested by " + emailForBookTimeSlotModel.ClientEmail + ".<br/><br/>");
        //    sbBody.Append("<b>Talent Selected:</b>" + ". <br/>");
        //    sbBody.Append("Talent Name: " + emailForBookTimeSlotModel.TalentName + ". <br/>");
        //    if (emailForBookTimeSlotModel.yearsofExperience > 0)
        //    {
        //        sbBody.Append("Years Of Experience: " + emailForBookTimeSlotModel.yearsofExperience + " Years. <br/>");
        //    }
        //    else
        //    {
        //        sbBody.Append($"<li>Experience: Freshers</li>");
        //    }
        //    sbBody.Append("Priority: " + emailForBookTimeSlotModel.priority + ". <br/>");
        //    sbBody.Append("<a href='" + _configuration["NewAdminProjectURL"] + "allhiringrequest'>Click to View Hiring Request details.</a>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/> Thanks" + " <br/>");
        //    sbBody.Append("Uplers Talent Solutions Team" + " <br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    #endregion

        //    string TOEMAIL = emailForBookTimeSlotModel.salesemailid;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.salesName;

        //    List<string> toemail = new List<string>() { TOEMAIL };
        //    List<string> toemailname = new List<string>() { TOEMAILNAME };

        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail();
        //    #endregion
        //}
        //public void SendEmailtoSalesDirectPlacement(long TalentID, long HiringRequest_ID)
        //{

        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Variable
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequest_ID, 0, 0);

        //    #region Email Content
        //    Subject = "The Talent " + emailForBookTimeSlotModel.TalentName + " has accepted the Hiring Request - " + emailForBookTimeSlotModel.HR_Number + " of Hiring Request for Direct Placement";
        //    BodyCustom = "Hello Team,";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/> A new talent has been matched for the " + emailForBookTimeSlotModel.TalentRole + " role for client " + emailForBookTimeSlotModel.CompanyName + ".<br/><br/>");
        //    sbBody.Append("HRID: " + emailForBookTimeSlotModel.HR_Number + ". <br/>");
        //    sbBody.Append("HR Type: Direct Placement. <br/>");
        //    if (emailForBookTimeSlotModel.finalCost != null)
        //    {
        //        sbBody.Append("Talent Expected CTC: " + emailForBookTimeSlotModel.finalCost + ". <br/>");
        //    }
        //    else
        //    {
        //        sbBody.Append("Talent Expected CTC: NA. <br/>");
        //    }
        //    if (emailForBookTimeSlotModel.CurrentCTC != null)
        //    {
        //        sbBody.Append("Talent current CTC: " + emailForBookTimeSlotModel.CurrentCTC + ". <br/>");
        //    }
        //    else
        //    {
        //        sbBody.Append("Talent current CTC: NA. <br/>");
        //    }
        //    if (emailForBookTimeSlotModel.DPPercentage != null)
        //    {
        //        sbBody.Append("DP percentage: " + emailForBookTimeSlotModel.DPPercentage + ". <br/>");
        //    }
        //    else
        //    {
        //        sbBody.Append("DP percentage: NA. <br/>");
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/> Thanks" + " <br/>");
        //    sbBody.Append("Uplers Talent Solutions Team" + " <br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    #endregion

        //    string TOEMAIL = emailForBookTimeSlotModel.salesemailid;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.salesName;
        //    if (!string.IsNullOrEmpty(TOEMAIL))
        //    {
        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion
        //    }
        //}
        //public void SendEmailForTalentAcceptedHR(long TalentID, long HiringRequestID)
        //{
        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Variable
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, 0);

        //    Subject = "The Talent " + emailForBookTimeSlotModel.TalentName + " has accepted the Hiring Request - " + emailForBookTimeSlotModel.HR_Number + " of Hiring Request";

        //    BodyCustom = "Hello Team,";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Please proceed to change the status of Talent to Show to Client, he has accepted the Hiring Request.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("HRID: " + emailForBookTimeSlotModel.HR_Number + "<br/>");
        //    sbBody.Append("Talent Name: " + emailForBookTimeSlotModel.TalentName + "<br/>");
        //    sbBody.Append("Priority: " + emailForBookTimeSlotModel.priority);
        //    sbBody.Append("<br/>");
        //    if (emailForBookTimeSlotModel.IsManaged)
        //    {
        //        sbBody.Append("Manage: Yes");
        //    }
        //    else
        //    {
        //        sbBody.Append("Self Manage: Yes");
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Thanks");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    string TOEMAIL = emailForBookTimeSlotModel.salesemailid;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.salesName;
        //    if (!string.IsNullOrEmpty(TOEMAIL))
        //    {
        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion
        //    }
        //}
        //public void SendEmailNotificationForPassToAdhoc(long HiringRequest_ID)
        //{
        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Variable
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(0, 0, 0, false, HiringRequest_ID, 0, 0);

        //    Subject = "HR " + emailForBookTimeSlotModel.HR_Number + " Passed to the ODR team";

        //    BodyCustom = "Hi,";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("The operation team has passed this HR to the ODR team. Kindly review the HR and proceed to source the ODR talents.");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Company Name: " + emailForBookTimeSlotModel.CompanyName);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Client Name: " + emailForBookTimeSlotModel.clientName);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Client Email: " + emailForBookTimeSlotModel.ClientEmail);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("HRID:  " + emailForBookTimeSlotModel.HR_Number);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Position: " + emailForBookTimeSlotModel.position);

        //    sbBody.Append("<br/>");


        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Thanks");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //    sbBody.Append("</div>");


        //    string AdHocEMAIL = "";
        //    string AdHocEMAILNAME = "";

        //    AdHocEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("AdhocCCEmailIds");
        //    AdHocEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("AdhocCCEmailName");

        //    List<string> toemail = new List<string>() { emailForBookTimeSlotModel.ClientEmail };
        //    List<string> toemailname = new List<string>() { emailForBookTimeSlotModel.clientName };

        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetCCEmail(AdHocEMAIL, AdHocEMAILNAME);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail(false, true);
        //    #endregion

        //}

        //public bool SendEmailNotificationToInternalTeampreonboardingisInProcess(long onBoardID, long TalentID, long HiringRequestID)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, onBoardID, false, HiringRequestID, 0, 0);

        //        #region Email Content
        //        Subject = "Client Pre Onboarding Form submitted - " + emailDetail.HR_Number;
        //        BodyCustom = "Hi Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client pre-request onboarding details are submitted.. Kindly vet the details,  and proceed with the Talent pre-onboarding process.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("HRID: " + emailDetail.HR_Number);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Engagement ID: " + emailDetail.EngagementID);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Position: " + emailDetail.position);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Name: " + emailDetail.TalentName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Name: " + emailDetail.clientName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Email: " + emailDetail.ClientEmail);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Company Name: " + emailDetail.CompanyName);
        //        sbBody.Append("<br/>");
        //        if (emailDetail.IsManaged)
        //        {
        //            sbBody.Append("Manage: Yes");
        //        }
        //        else
        //        {
        //            sbBody.Append("Self Manage: Yes");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");
        //        #endregion

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        MakeCCDetail ccDetails = MakeCCEmailDetails(emailDetail.HRSalesPersonID, false, true, HiringRequestID, TalentID);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }
        //        #endregion

        //        string TOEMAIL = emailDetail.salesemailid;
        //        string TOEMAILNAME = emailDetail.salesName;

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailToSalesTeamForKickOff(long onBoardID, long TalentID, long HiringRequestID, Meeting meeting, DateTime scheduleTime, DateTime dateTimeEnd, DateTime? KickOffDatetime, PrgContactTimeZone prg_ContactTimeZone)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, onBoardID, false, HiringRequestID, 0, 0);

        //        #region Email Content
        //        Subject = "Kick off Scheduled for " + emailDetail.clientName + " and " + emailDetail.TalentName + " - " + emailDetail.HR_Number;
        //        BodyCustom = "Hello Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The kick off has been confirmed and here is the details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("HRID: " + emailDetail.HR_Number);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Engagement ID: " + emailDetail.EngagementID);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Position: " + emailDetail.position);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Name: " + emailDetail.TalentName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Name: " + emailDetail.clientName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Email: " + emailDetail.ClientEmail);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Company Name: " + emailDetail.CompanyName);
        //        sbBody.Append("<br/>");
        //        //sbBody.Append("Kick off Date & Time: " + KickOffDatetime.Value.ToString("dd/MM/yyyy HH:mm"));

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            sbBody.Append("Kickoff date & time: " +
        //            KickOffDatetime.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            KickOffDatetime.Value.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            sbBody.Append("Kickoff date & time: " +
        //            KickOffDatetime.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            KickOffDatetime.Value.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            sbBody.Append("<br/>");

        //            sbBody.Append("Kickoff date & time (IST): " +
        //            KickOffDatetime.Value.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            KickOffDatetime.Value.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }

        //        sbBody.Append("<br/>");
        //        sbBody.Append("Link: " + meeting.join_url);
        //        sbBody.Append("<br/>");
        //        //sbBody.Append("Pass Code: " + meeting.password + " (Optional)");
        //        //sbBody.Append("<br/>");
        //        if (emailDetail.IsManaged)
        //        {
        //            sbBody.Append("Manage: Yes");
        //        }
        //        else
        //        {
        //            sbBody.Append("Self Manage: Yes");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");
        //        #endregion



        //        StringBuilder str = new StringBuilder();
        //        var descriptionCalender = new StringBuilder();

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            descriptionCalender.Append("Kickoff date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            descriptionCalender.Append("Kickoff date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            descriptionCalender.Append("<br/>");

        //            descriptionCalender.Append("Kickoff date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }


        //        //descriptionCalender.Append("Interview date & time: " + scheduleTime.ToString() + " " + prg_ContactTimeZone.ShortName);
        //        descriptionCalender.Append("<br/>");
        //        descriptionCalender.Append("Interview Link: " + meeting.join_url);
        //        descriptionCalender.Append("<br/>");
        //        //descriptionCalender.Append("Pass Code : " + meeting.password + " (Optional)");

        //        //if (!string.IsNullOrEmpty(meeting.password))
        //        //{
        //        //    descriptionCalender.Append("<br/>");
        //        //    descriptionCalender.Append("Pass Code : " + meeting.password + " (Optional)");
        //        //}

        //        str.AppendLine("BEGIN:VCALENDAR");
        //        str.AppendLine("PRODID:-//Schedule a KickOff");
        //        str.AppendLine("VERSION:2.0");
        //        str.AppendLine("METHOD:REQUEST");

        //        str.AppendLine("BEGIN:VEVENT");
        //        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine("LOCATION: " + "Remote");
        //        str.AppendLine($"ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:{clientEmailName}");
        //        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //        str.AppendLine(string.Format("SUMMARY:{0}", "Kickoff Schedule : " + emailDetail.TalentName + " | " + emailDetail.position));
        //        str.AppendLine("BEGIN:VALARM");
        //        str.AppendLine("TRIGGER:-PT15M");
        //        str.AppendLine("ACTION:DISPLAY");
        //        str.AppendLine("DESCRIPTION:Reminder");
        //        str.AppendLine("END:VALARM");
        //        str.AppendLine("END:VEVENT");
        //        str.AppendLine("END:VCALENDAR");

        //        byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //        MemoryStream stream = new MemoryStream(byteArray);

        //        Attachment attach = new Attachment(stream, "invite.ics");

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        MakeCCDetail ccDetails = MakeCCEmailDetails(emailDetail.HRSalesPersonID, false);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }
        //        #endregion

        //        string TOEMAIL = emailDetail.salesemailid;
        //        string TOEMAILNAME = emailDetail.salesName;

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());
        //            emailOperator.SetAttachment(new List<Attachment>() { attach });

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailForAMAssignment(long HRID, long TalentID)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        #region Email Content
        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HRID, 0, 0);

        //        Subject = "AM Assignment | Client " + emailDetail.clientName;
        //        BodyCustom = "Hello " + emailDetail.clientName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hope you are doing well.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("This is to inform you that you have been assigned as AM for the Client " + emailDetail.clientName + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please find below details for your reference:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hiring Request ID: " + emailDetail.HR_Number + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Name: " + emailDetail.clientName + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Role: " + emailDetail.position);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Regards");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //        #endregion

        //        List<string> toemail = new List<string>();
        //        List<string> toemailname = new List<string>();

        //        if (!string.IsNullOrEmpty(emailDetail.AM_SalesPersonEmailID))
        //        {
        //            toemail.Add(emailDetail.AM_SalesPersonEmailID);
        //            toemailname.Add(emailDetail.AM_SalesPersonName);
        //        }
        //        if (!string.IsNullOrEmpty(emailDetail.NBD_SalesPersonEmailID))
        //        {
        //            toemail.Add(emailDetail.NBD_SalesPersonEmailID);
        //            toemailname.Add(emailDetail.NBD_SalesPersonName);
        //        }
        //        if (!string.IsNullOrEmpty(emailDetail.AM_AssignedSalesManagerEmailID))
        //        {
        //            toemail.Add(emailDetail.AM_AssignedSalesManagerEmailID);
        //            toemailname.Add(emailDetail.AM_AssignedSalesManagerName);
        //        }

        //        if (toemail.Count > 0)
        //        {
        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail(false, true);
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendAMAssignmentEmailToAM(long HRID, long TalentID)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string HRNo = "";
        //        int? NoOfTalent = 0;
        //        string Role = "";
        //        string HRStatus = "";

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HRID, 0, 0);

        //        List<GenAmnbdAssignmentEmailsendDetail> AMNBD_AssignmentList = new List<GenAmnbdAssignmentEmailsendDetail>();
        //        if (emailDetail.AM_SalesPersonID != 0)
        //        {
        //            AMNBD_AssignmentList = _talentConnectAdminDBContext.GenAmnbdAssignmentEmailsendDetails.Where(x => x.NeedEmailSend == 1 && x.AmSalesPersonId == emailDetail.AM_SalesPersonID).ToList();
        //        }

        //        #region Email Content
        //        Subject = "AM Assignment | Company Name " + emailDetail.CompanyName;
        //        BodyCustom = "Hello " + emailDetail.AM_SalesPersonName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Greetings for the day!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We would like to inform you that you have been assigned as AM to the new company.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please find below required details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Company Name - " + emailDetail.CompanyName + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Name - " + emailDetail.clientName + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Email - " + emailDetail.ClientEmail + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Engagement ID - " + emailDetail.EngagementID + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Name - " + emailDetail.TalentName + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Role - " + emailDetail.TalentRole + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Type - " + emailDetail.TalentType + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Discovery Role Link - " + emailDetail.Discovery_Call);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (AMNBD_AssignmentList != null && AMNBD_AssignmentList.Count != 0)
        //        {
        //            sbBody.Append(string.Format("<p>List of HR</p>"));
        //            sbBody.Append("<br/>");
        //            sbBody.Append(string.Format("<table style='border-style: solid; width:100%'>"));
        //            sbBody.Append(string.Format("<tr><th style='text-align: center;'>HR #</th><th style='text-align: center;'>TR</th><th style='text-align: center;'>Role</th><th style='text-align: center;'>Status</th></tr>"));
        //            foreach (var item in AMNBD_AssignmentList)
        //            {
        //                GenSalesHiringRequest hiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == item.HiringRequestId).FirstOrDefault();
        //                //_talentConnectAdminDBContext.Entry(hiringRequest).Reload();
        //                if (hiringRequest != null)
        //                {
        //                    HRNo = hiringRequest.HrNumber;
        //                    NoOfTalent = hiringRequest.NoofTalents;
        //                    Role = hiringRequest.RequestForTalent;
        //                    //HRStatus = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == hiringRequest.StatusId).FirstOrDefault().HiringRequestStatus;

        //                    if (string.IsNullOrEmpty(emailDetail.HRStatus))
        //                    {
        //                        HRStatus = _talentConnectAdminDBContext.PrgJobStatusClientPortals.Where(x => x.Id == hiringRequest.JobStatusId).FirstOrDefault().JobStatus;
        //                    }
        //                    else
        //                    {
        //                        HRStatus = emailDetail.HRStatus;
        //                    }

        //                    sbBody.Append(string.Format("<tr><td style='text-align: center;'>" + HRNo + "</td><td style='text-align: center;'>" + NoOfTalent + "</td><td style='text-align: center;'>" + Role + "</td><td style='text-align: center;'>" + HRStatus + "</td></tr>"));
        //                }
        //            }
        //            sbBody.Append(string.Format("</table>"));
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Request you to proceed with further process. Also, Please connect with your respective NBD - " + emailDetail.NBD_SalesPersonName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Regards");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //        #endregion

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        List<string> toemail = new List<string>();
        //        List<string> toemailname = new List<string>();

        //        if (!string.IsNullOrEmpty(emailDetail.AM_SalesPersonEmailID))
        //        {
        //            toemail.Add(emailDetail.AM_SalesPersonEmailID);
        //            toemailname.Add(emailDetail.AM_SalesPersonName);
        //        }
        //        if (!string.IsNullOrEmpty(emailDetail.NBD_SalesPersonEmailID))
        //        {
        //            CCEMAIL = emailDetail.NBD_SalesPersonEmailID;
        //            CCEMAILNAME = emailDetail.NBD_SalesPersonName;
        //        }

        //        if (toemail.Count > 0)
        //        {
        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendAMAssignmentEmailToNBD(long HRID, long TalentID)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string HRNo = "";
        //        int? NoOfTalent = 0;
        //        string Role = "";
        //        string HRStatus = "";

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HRID, 0, 0);
        //        List<GenAmnbdAssignmentEmailsendDetail> AMNBD_AssignmentList = new List<GenAmnbdAssignmentEmailsendDetail>();
        //        if (emailDetail.NBD_SalesPersonID != 0)
        //        {
        //            AMNBD_AssignmentList = _talentConnectAdminDBContext.GenAmnbdAssignmentEmailsendDetails.Where(x => x.NeedEmailSend == 1 && x.NbdSalesPersonId == emailDetail.NBD_SalesPersonID).ToList();
        //        }

        //        #region Email Content
        //        Subject = "AM Re-Assignment | " + emailDetail.CompanyName;
        //        BodyCustom = "Hello " + emailDetail.NBD_SalesPersonName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Greetings for the Day!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We would like to inform you that below HR will be reassigned to " + emailDetail.AM_SalesPersonName + ". Please find below details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please find below required details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Company Name - " + emailDetail.CompanyName + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Name - " + emailDetail.clientName + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Email - " + emailDetail.ClientEmail + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Engagement ID - " + emailDetail.EngagementID + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Name - " + emailDetail.TalentName + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Role - " + emailDetail.TalentRole + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Type - " + emailDetail.TalentType + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Discovery Role Link - " + emailDetail.Discovery_Call);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");

        //        if (AMNBD_AssignmentList != null && AMNBD_AssignmentList.Count != 0)
        //        {
        //            sbBody.Append(string.Format("<p>List of HR</p>"));
        //            sbBody.Append("<br/>");
        //            sbBody.Append(string.Format("<table style='border-style: solid; width:100%'>"));
        //            sbBody.Append(string.Format("<tr><th style='text-align: center;'>HR #</th><th style='text-align: center;'>TR</th><th style='text-align: center;'>Role</th><th style='text-align: center;'>Status</th></tr>"));
        //            foreach (var item in AMNBD_AssignmentList)
        //            {
        //                GenSalesHiringRequest hiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == item.HiringRequestId).FirstOrDefault();
        //                //_talentConnectAdminDBContext.Entry(hiringRequest).Reload();
        //                if (hiringRequest != null)
        //                {
        //                    HRNo = hiringRequest.HrNumber;
        //                    NoOfTalent = hiringRequest.NoofTalents;
        //                    Role = hiringRequest.RequestForTalent;
        //                    //HRStatus = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == hiringRequest.StatusId).FirstOrDefault().HiringRequestStatus;
        //                    //HRStatus = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == hiringRequest.StatusId).FirstOrDefault().HiringRequestStatus;

        //                    if (string.IsNullOrEmpty(emailDetail.HRStatus))
        //                    {
        //                        HRStatus = _talentConnectAdminDBContext.PrgJobStatusClientPortals.Where(x => x.Id == hiringRequest.JobStatusId).FirstOrDefault().JobStatus;
        //                    }
        //                    else
        //                    {
        //                        HRStatus = emailDetail.HRStatus;
        //                    }

        //                    sbBody.Append(string.Format("<tr><td style='text-align: center;'>" + HRNo + "</td><td style='text-align: center;'>" + NoOfTalent + "</td><td style='text-align: center;'>" + Role + "</td><td style='text-align: center;'>" + HRStatus + "</td></tr>"));
        //                }
        //            }
        //            sbBody.Append(string.Format("</table>"));
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("For any further update or changes, please contact " + emailDetail.AM_SalesPersonName + " or you can also reach out to  <a class='link' href='#' target='_blank'>" + emailDetail.AM_SalesPersonEmailID + "</a>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Regards");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //        #endregion

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        List<string> toemail = new List<string>();
        //        List<string> toemailname = new List<string>();

        //        if (!string.IsNullOrEmpty(emailDetail.NBD_SalesPersonEmailID))
        //        {
        //            toemail.Add(emailDetail.NBD_SalesPersonEmailID);
        //            toemailname.Add(emailDetail.NBD_SalesPersonName);
        //        }

        //        if (!string.IsNullOrEmpty(emailDetail.AM_SalesPersonEmailID))
        //        {
        //            CCEMAIL = emailDetail.AM_SalesPersonEmailID;
        //            CCEMAILNAME = emailDetail.AM_SalesPersonName;
        //        }

        //        if (toemail.Count > 0)
        //        {
        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public string SendEmailForChangeAMForCompany(UpdateAMDetails updateAM)
        //{
        //    string CompanyName = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == updateAM.CompanyID).Select(x => x.Company).FirstOrDefault();

        //    int[] Ids = { (int)updateAM.OldAM_SalesPersonID, (int)updateAM.NewAM_SalesPersonID };
        //    string NewAMUserName = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == updateAM.NewAM_SalesPersonID).Select(x => x.FullName).FirstOrDefault();

        //    int Count = 0;
        //    foreach (int id in Ids)
        //    {
        //        Count++;
        //        var userdetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == id).Select(x => new
        //        {
        //            EmailId = x.EmailId,
        //            Name = x.FullName
        //        }).FirstOrDefault();

        //        if (userdetails != null && !string.IsNullOrEmpty(userdetails.EmailId) && !string.IsNullOrEmpty(userdetails.Name))
        //        {
        //            string Subject = "", BodyCustom = "";
        //            StringBuilder sbBody = new StringBuilder();

        //            Subject = $"AM Changed | Client {CompanyName}";
        //            BodyCustom = $"Hello {userdetails.Name},";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            if (Count == 1)
        //            {
        //                sbBody.Append($"Please be informed that AM of Client <b>{CompanyName}</b> has been changed from you to <b>{NewAMUserName}</b>.");
        //                sbBody.Append("<br/>");
        //                sbBody.Append($"All your existing HR’s for this client will be transferred to <b>{NewAMUserName}</b>.");
        //                sbBody.Append("<br/>");
        //            }
        //            else
        //            {
        //                sbBody.Append($"Please be informed that AM of Client <b>{CompanyName}</b> has been updated to you.");
        //                sbBody.Append("<br/>");
        //                sbBody.Append($"All existing HR’s for this client will be transferred to <b>{userdetails.Name}</b>.");
        //                sbBody.Append("<br/>");
        //            }
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Regards,");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");
        //            var dstring = sbBody.ToString();

        //            string TOEMAIL = userdetails.EmailId;
        //            string TOEMAILNAME = userdetails.Name;

        //            string CCEMAIL = "";
        //            string CCEMAILNAME = "";

        //            long oldAMUSerID = (long)updateAM.OldAM_SalesPersonID;
        //            long newAMUSerID = (long)updateAM.NewAM_SalesPersonID;

        //            // old AM user 
        //            if (Count == 1 && oldAMUSerID > 0)
        //            {
        //                MakeCCDetail ccDetails = MakeCCEmailDetails(oldAMUSerID, true);
        //                if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //                {
        //                    CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                    CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //                }
        //            }

        //            // New AM User
        //            if (Count == 2 && newAMUSerID > 0)
        //            {
        //                MakeCCDetail ccDetails = MakeCCEmailDetails(newAMUSerID, true);
        //                if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //                {
        //                    CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                    CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(TOEMAIL))
        //            {
        //                try
        //                {
        //                    List<string> toemail = new List<string>() { TOEMAIL };
        //                    List<string> toemailname = new List<string>() { TOEMAILNAME };

        //                    EmailOperator emailOperator = new EmailOperator(_configuration);
        //                    emailOperator.SetToEmail(toemail);
        //                    emailOperator.SetToEmailName(toemailname);
        //                    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //                    emailOperator.SetSubject(Subject);
        //                    emailOperator.SetBody(sbBody.ToString());

        //                    #region SendEmail
        //                    if (!string.IsNullOrEmpty(Subject))
        //                        emailOperator.SendEmail();
        //                    #endregion
        //                }
        //                catch (Exception ex)
        //                {
        //                    return ex.Message;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return "error";
        //        }
        //    }
        //    return "success";
        //}
        //public string SendEmailForChangeAMForPayOut(UpdateAM_Payout updateAM, Sproc_UTS_Get_PayOut_Basic_Information_Result payOutDetails, List<Sproc_Get_Hierarchy_For_Email_Result> oldAMUserHierarchy, List<Sproc_Get_Hierarchy_For_Email_Result> newAMUserHierarchy)
        //{
        //    int[] Ids = { (int)updateAM.OldAMPersonID, (int)updateAM.NewAMPersonID };
        //    string NewAMUserName = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == updateAM.NewAMPersonID).Select(x => x.FullName).FirstOrDefault();

        //    int Count = 0;
        //    foreach (int id in Ids)
        //    {
        //        Count++;
        //        var userdetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == id).Select(x => new
        //        {
        //            EmailId = x.EmailId,
        //            Name = x.FullName
        //        }).FirstOrDefault();

        //        if (userdetails != null && !string.IsNullOrEmpty(userdetails.EmailId) && !string.IsNullOrEmpty(userdetails.Name))
        //        {
        //            string Subject = "", BodyCustom = "";
        //            StringBuilder sbBody = new StringBuilder();
        //            updateAM.EngagementId_HRID = updateAM.EngagementId_HRID.Replace("\n", "").Replace("/", "|");
        //            Subject = $"AM Changed | {updateAM.EngagementId_HRID}";
        //            BodyCustom = $"Hello {userdetails.Name},";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            if (Count == 1)//old AM mail
        //            {
        //                sbBody.Append($"Please be informed that AM of Engagement <b>{updateAM.EngagementId_HRID}</b> has been changed from you to <b>{NewAMUserName}</b>.");
        //                sbBody.Append("<br/>");
        //                sbBody.Append($"All your ongoing contract will be transferred to <b>{NewAMUserName}</b>.");
        //                sbBody.Append($"For this Talent : {payOutDetails.TalentName}, Now Onwards Month/Year : {updateAM.Month}/{updateAM.Year} ");
        //                sbBody.Append("<br/>");
        //            }
        //            else
        //            {
        //                sbBody.Append($"Please be informed that AM of Engagement <b>{updateAM.EngagementId_HRID}</b> has been updated to you.");
        //                sbBody.Append("<br/>");
        //                sbBody.Append($"All your ongoing contract will be transferred to <b>{NewAMUserName}</b>.");
        //                sbBody.Append($"For this Talent : {payOutDetails.TalentName}, Now Onwards Month/Year : {updateAM.Month}/{updateAM.Year} ");
        //                sbBody.Append("<br/>");
        //            }
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Regards,");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");
        //            var dstring = sbBody.ToString();

        //            string TOEMAIL = userdetails.EmailId;
        //            string TOEMAILNAME = userdetails.Name;

        //            string CCEMAIL = "";
        //            string CCEMAILNAME = "";

        //            long oldAMUSerID = (long)updateAM.OldAMPersonID;
        //            long newAMUSerID = (long)updateAM.NewAMPersonID;

        //            // old AM user 
        //            if (Count == 1 && oldAMUSerID > 0)
        //            {
        //                MakeCCDetail ccDetails = MakeCCEmailDetails(oldAMUSerID, true);
        //                if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //                {
        //                    CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                    CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //                }
        //            }

        //            // New AM User
        //            if (Count == 2 && newAMUSerID > 0)
        //            {
        //                MakeCCDetail ccDetails = MakeCCEmailDetails(newAMUSerID, true);
        //                if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //                {
        //                    CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                    CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(TOEMAIL))
        //            {
        //                try
        //                {
        //                    List<string> toemail = new List<string>() { TOEMAIL };
        //                    List<string> toemailname = new List<string>() { TOEMAILNAME };

        //                    EmailOperator emailOperator = new EmailOperator(_configuration);
        //                    emailOperator.SetToEmail(toemail);
        //                    emailOperator.SetToEmailName(toemailname);
        //                    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //                    emailOperator.SetSubject(Subject);
        //                    emailOperator.SetBody(sbBody.ToString());

        //                    #region SendEmail
        //                    if (!string.IsNullOrEmpty(Subject))
        //                        emailOperator.SendEmail();
        //                    #endregion
        //                }
        //                catch (Exception ex)
        //                {
        //                    return ex.Message;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return "error";
        //        }
        //    }
        //    return "success";
        //}
        //public string SendEmailForChangePayOutDeatils(Sproc_Get_engagement_Edit_All_BR_PR_Result payOutDetails)
        //{
        //    if (payOutDetails != null)
        //    {
        //        string Subject = "", BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();
        //        payOutDetails.EngagementId_HRID = payOutDetails.EngagementId_HRID.Replace("\n", "").Replace("/", "|");
        //        Subject = $"Contract Details Changed | {payOutDetails.EngagementId_HRID}";
        //        BodyCustom = $"Hello Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Greetings for the day!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Please be informed that, Current Contract details for {payOutDetails.EngagementId_HRID}, are below");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Month : {payOutDetails.MonthNames}({payOutDetails.Years})");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Contract Type : {payOutDetails.ContractType}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Bill Rate : {payOutDetails.BR} {payOutDetails.Currency}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Pay Rate : {payOutDetails.PR} {payOutDetails.Currency}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Regards,");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        var dstring = sbBody.ToString();

        //        //get AM Details
        //        var AM_userdetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == payOutDetails.AM_SalesPersonID).Select(x => new
        //        {
        //            EmailId = x.EmailId,
        //            Name = x.FullName,
        //            ID = x.Id
        //        }).FirstOrDefault();

        //        //Get TSC Details 
        //        var TSC_userdetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == payOutDetails.TSC_PersonID).Select(x => new
        //        {
        //            EmailId = x.EmailId,
        //            Name = x.FullName,
        //            ID = x.Id
        //        }).FirstOrDefault();

        //        #region commneted code

        //        //string BuddyMAIL = "";
        //        //string BuddyMAILNAME = "";

        //        //BuddyMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("HRBuddyEmail");
        //        //BuddyMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("HRBuddyName");

        //        //if (TSC_userdetails != null && !string.IsNullOrEmpty(TSC_userdetails.EmailId))
        //        //{
        //        //    BuddyMAIL = BuddyMAIL + ',' + TSC_userdetails.EmailId;
        //        //    BuddyMAILNAME = BuddyMAILNAME + ',' + TSC_userdetails.Name;
        //        //}

        //        //string InternalCCEmailID = emailDatabaseContentProvider.GetCCEmailIdValues();
        //        //string InternalCCEmailName = emailDatabaseContentProvider.GetCCEmailNameValues();

        //        //CCEMAIL = _configuration["app_settings:CCEmailId"] + ','
        //        //           + _configuration["app_settings:CC1EmailId"] + ','
        //        //           + _configuration["app_settings:TSCEmailId"] + InternalCCEmailID + BuddyMAIL;

        //        //CCEMAILNAME = _configuration["app_settings:CCEmailName"] + ','
        //        //               + _configuration["app_settings:CC1EmailName"] + ','
        //        //               + _configuration["app_settings:TSCEmailName"] + InternalCCEmailName + BuddyMAILNAME;

        //        #endregion

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //        List<string> toemail = new List<string>();
        //        List<string> toemailname = new List<string>();

        //        if (AM_userdetails != null && !string.IsNullOrEmpty(AM_userdetails.EmailId))
        //        {
        //            toemail.Add(AM_userdetails.EmailId);
        //            toemailname.Add(AM_userdetails.Name);
        //        }

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        if (TSC_userdetails != null && !string.IsNullOrEmpty(TSC_userdetails.EmailId))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? TSC_userdetails.EmailId : "," + TSC_userdetails.EmailId;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? TSC_userdetails.Name : "," + TSC_userdetails.Name;
        //        }

        //        MakeCCDetail ccDetails = MakeCCEmailDetails(AM_userdetails.ID, true);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }
        //        #endregion

        //        if (CCEMAIL != "" && toemail.Any())
        //        {
        //            try
        //            {
        //                EmailOperator emailOperator = new EmailOperator(_configuration);
        //                emailOperator.SetToEmail(toemail);
        //                emailOperator.SetToEmailName(toemailname);
        //                emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //                emailOperator.SetSubject(Subject);
        //                emailOperator.SetBody(sbBody.ToString());

        //                #region SendEmail
        //                if (!string.IsNullOrEmpty(Subject))
        //                    emailOperator.SendEmail();
        //                #endregion
        //            }
        //            catch (Exception ex)
        //            {
        //                return ex.Message;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return "error";
        //    }

        //    return "success";
        //}
        //public bool SendSalesPersonEmailHRTypeChanged(string PreviousType, string CurrentType, long? HrID, long TalentId)  //, long? HrID, long TalentId
        //{

        //    string Subject = "";
        //    string BodyCustom = "";
        //    bool emailsent = false;
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Variable
        //    //EmailForBookTimeSlotModel emailDetail = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    object[] param1 = new object[]
        //                {
        //                    HrID,
        //                    TalentId
        //                };
        //    string paramasString1 = CommonLogic.ConvertToParamString(param1);
        //    Sproc_EmailHRTypeChanged_Result emailDetail = new Sproc_EmailHRTypeChanged_Result();
        //    emailDetail = _talentConnectAdminDBContext.Set<Sproc_EmailHRTypeChanged_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_EmailHRTypeChanged, paramasString1)).AsEnumerable().FirstOrDefault();


        //    //emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentId, 0, 0, false, HrID.Value, 0, 0);

        //    Subject = "Type of HR Changed to " + CurrentType + emailDetail.HR_Number;
        //    BodyCustom = "Hello " + emailDetail.ClientName + ",";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>Hope you are doing well." + " <br/><br/>");
        //    sbBody.Append("We would like to inform you that type of the HR has been changed from " + PreviousType + " to " + CurrentType + "<br/>");
        //    sbBody.Append("Please find below details for your reference:<br/>");
        //    sbBody.Append("HR Number: " + emailDetail.HR_Number + " <br/>");
        //    sbBody.Append("Client Name: " + emailDetail.ClientName + " <br/>");
        //    sbBody.Append("Talent Name: " + emailDetail.TalentName + " <br/>");
        //    if (emailDetail.FinalCost != null)
        //    {
        //        sbBody.Append("Expected CTC/Talent Cost: " + emailDetail.FinalCost + " <br/>");
        //    }
        //    else
        //    {
        //        sbBody.Append("Expected CTC/Talent Cost: NA <br/>");
        //    }
        //    if (emailDetail.DPPercentage != null)
        //    {
        //        sbBody.Append("NR Percentage/DP Percentage: " + emailDetail.DPPercentage + " <br/>");

        //    }
        //    else
        //    {
        //        sbBody.Append("NR Percentage/DP Percentage: NA <br/>");
        //    }
        //    sbBody.Append("Role: " + emailDetail.TalentRole + " <br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/> Thanks" + " <br/>");
        //    sbBody.Append("Uplers Talent Solutions Team" + " <br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //    string TOEMAIL = emailDetail.SalesEmailId;
        //    string TOEMAILNAME = emailDetail.SalesEmailName;

        //    string CCEMAIL = "";
        //    string CCEMAILNAME = "";

        //    MakeCCDetail ccDetails = MakeCCEmailDetails(emailDetail.HRSalesPersonID ?? 0, false);
        //    if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //    {
        //        if (string.IsNullOrEmpty(CCEMAIL))
        //            CCEMAIL = ccDetails.CCEmail;
        //        else
        //            CCEMAIL = CCEMAIL + "," + ccDetails.CCEmail;

        //        if (string.IsNullOrEmpty(CCEMAILNAME))
        //            CCEMAILNAME = ccDetails.CCEmailName;
        //        else
        //            CCEMAILNAME = CCEMAILNAME + "," + ccDetails.CCEmailName;
        //    }
        //    #endregion

        //    if (!string.IsNullOrEmpty(TOEMAIL))
        //    {
        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail(false, true);
        //        #endregion
        //    }
        //    emailsent = true;
        //    return emailsent;
        //}
        //public string SendEmailForReplacementToInternalTeam(string webrootpath, string HR_Number, string EngagementID, string TalentName, string ClientName, string Last_Working_Day, string Reason_For_Replacement, long? SalesUserId, string SalesPersonName, string SalesPersonEmail, long HiringRequestID, long TalentId)
        //{
        //    try
        //    {
        //        #region Email Content
        //        string Subject = "", BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        Subject = "Talent Replacement | " + HR_Number + " | " + EngagementID;
        //        BodyCustom = "Hi Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("This is to inform you that replacement has been raised for the Talent " + TalentName + ". Please find below required details.");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("HR Number: " + HR_Number);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Engagement ID: " + EngagementID);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Name: " + TalentName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Name: " + ClientName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Last Working Date: " + Last_Working_Day);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Reason for Replacement: " + Reason_For_Replacement);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //        #endregion

        //        emailDatabaseContentProvider.SalesUserId = Convert.ToInt64(SalesUserId);

        //        #region Commented Code
        //        //string InternalCCEmailID = emailDatabaseContentProvider.GetCCEmailIdValues();
        //        //string InternalCCEmailName = emailDatabaseContentProvider.GetCCEmailNameValues();

        //        //List<string> internal_toEmail = new List<string>() { SalesPersonEmail };
        //        //List<string> internal_toEmailName = new List<string>() { SalesPersonName };

        //        //string CCEMAIL = _configuration["app_settings:CCEmailId"] + ','
        //        //               + _configuration["app_settings:CC1EmailId"] + ','
        //        //               + _configuration["app_settings:TSCEmailId"] + InternalCCEmailID;

        //        //string CCEMAILNAME = _configuration["app_settings:CCEmailName"] + ','
        //        //               + _configuration["app_settings:CC1EmailName"] + ','
        //        //               + _configuration["app_settings:TSCEmailName"] + InternalCCEmailName;
        //        #endregion

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //        string TOEMAIL = SalesPersonEmail;
        //        string TOEMAILNAME = SalesPersonName;

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        MakeCCDetail ccDetails = MakeCCEmailDetails(emailDatabaseContentProvider.SalesUserId, true, true, HiringRequestID, TalentId);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }
        //        #endregion

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        return "success";

        //    }
        //    catch (Exception e) { return e.Message; }
        //}
        //public string SendEmailforTalentReplacementDirectPlacement(string webRootPath, long HiringRequestID, long TalentID)
        //{
        //    try
        //    {
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);

        //        GenSalesHiringRequest salesHiringREquest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(salesHiringREquest).Reload();
        //        string TalentName = "", priority = "";
        //        string HR_Number = "", EngagementID = "", ClientName = "", ReplacetimePeriod = "", salesemailid = "", salesName = "";
        //        if (salesHiringREquest != null)
        //        {
        //            HR_Number = salesHiringREquest.HrNumber;
        //            var userDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == salesHiringREquest.SalesUserId)
        //                .Select(x => new
        //                {
        //                    FullName = x.FullName,
        //                    EmailId = x.EmailId
        //                }).FirstOrDefault();

        //            salesName = userDetails.FullName;
        //            salesemailid = userDetails.EmailId;

        //            GenContact contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == salesHiringREquest.ContactId).FirstOrDefault();
        //            //_talentConnectAdminDBContext.Entry(contact).Reload();
        //            if (contact != null)
        //            {
        //                GenCompany company = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == contact.CompanyId).FirstOrDefault();
        //                //_talentConnectAdminDBContext.Entry(company).Reload();
        //                if (company != null)
        //                {
        //                    ClientName = company.Company;
        //                }
        //            }
        //        }

        //        GenTalent talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == TalentID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talent).Reload();
        //        GenOnBoardTalent onBoardTalents = _talentConnectAdminDBContext.GenOnBoardTalents.Where(x => x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(onBoardTalents).Reload();
        //        if (talent != null)
        //        {
        //            TalentName = talent.FirstName;
        //            GenContactTalentPriority ContactTalentPriority = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == talent.Id && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //            //_talentConnectAdminDBContext.Entry(ContactTalentPriority).Reload();
        //            if (ContactTalentPriority != null)
        //            {
        //                priority = ContactTalentPriority.TalentPriority;
        //            }
        //        }
        //        if (onBoardTalents != null)
        //        {
        //            EngagementID = onBoardTalents.EngagemenId;
        //        }

        //        Subject = "Replacement | DP HR | " + HR_Number + ".";
        //        BodyCustom = "Hello Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/> The talent replacement process has been initiated. Please find below details for your reference:<br/><br/>");
        //        sbBody.Append("Talent Name: " + TalentName + ". <br/>");
        //        sbBody.Append("Engagement ID: " + EngagementID + ". <br/>");
        //        sbBody.Append("Client Name: " + ClientName + ". <br/>");
        //        sbBody.Append("Replacement Time Period: Within 3 months/After 3 Months. <br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/> Thanks" + " <br/>");
        //        sbBody.Append("Uplers Talent Solutions Team" + " <br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //        string TOEMAIL = salesemailid;
        //        string TOEMAILNAME = salesName;

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        MakeCCDetail ccDetails = MakeCCEmailDetails((long)salesHiringREquest.SalesUserId, true, true, HiringRequestID, TalentID);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }
        //        #endregion

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        return "success";
        //    }
        //    catch (Exception e) { return e.Message; }
        //}
        //public string SendEmailForHRAcceptanceToInternalTeam(long HRID, int AcceptanceValue, string? Reason)
        //{
        //    #region Variables
        //    EmailOperator emailOperator = new(_configuration);
        //    EmailForBookTimeSlotModel emailDetails = new();
        //    bool IsAdhoc = false, IsManaged = false;
        //    long ContactID = 0;
        //    string Subject = "", BodyCustom = "", ClientName = "", ClientEmail = "", CompanyName = "",
        //            SalesPersonName = "", SalesPersonEmail = "", InternalCCEmailID = "", InternalCCEmailName = "",
        //            ToEmail = "", ToEmailName = "", CCEMAIL = "", CCEMAILNAME = "", AdHocEMAIL = "", AdHocEMAILNAME = "",
        //            SMEMAIL = "", SMEMAILNAME = "";
        //    StringBuilder sbBody = new();
        //    GenSalesHiringRequest HiringRequestData = new();
        //    GenContact ContactData = new();
        //    GenCompany CompanyData = new();
        //    UsrUser UserData = new();
        //    #endregion

        //    #region Data Validations
        //    HiringRequestData = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(x => x.Id == HRID);
        //    //_talentConnectAdminDBContext.Entry(HiringRequestData).Reload();
        //    if (HiringRequestData == null)
        //        return "Hiring request data not found.";

        //    ContactID = HiringRequestData.ContactId ?? 0;
        //    IsAdhoc = HiringRequestData.IsAdHocHr;
        //    IsManaged = HiringRequestData.IsManaged;

        //    ContactData = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == ContactID);
        //    //_talentConnectAdminDBContext.Entry(ContactData).Reload();

        //    if (ContactID == 0 || ContactData == null)
        //        return "Contact data not found.";

        //    ClientName = ContactData.FullName;
        //    ClientEmail = ContactData.EmailId;

        //    CompanyData = _talentConnectAdminDBContext.GenCompanies.FirstOrDefault(x => x.Id == ContactData.CompanyId);
        //    //_talentConnectAdminDBContext.Entry(CompanyData).Reload();

        //    if (CompanyData == null)
        //        return "Company data not found.";

        //    CompanyName = CompanyData.Company;

        //    UserData = _talentConnectAdminDBContext.UsrUsers.FirstOrDefault(x => x.Id == HiringRequestData.SalesUserId);
        //    _talentConnectAdminDBContext.Entry(UserData).Reload();

        //    if (UserData == null)
        //        return "Sales user data not found.";

        //    SalesPersonName = UserData.FullName;
        //    SalesPersonEmail = UserData.EmailId;

        //    #endregion

        //    #region Email Body
        //    if (AcceptanceValue == 1)
        //    {
        //        Subject = "Operation team has accepted the HR for further process";
        //        BodyCustom = "Hi,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Ops team has accepted the HR added by you and have started searching for the right talent.");
        //    }
        //    else if (AcceptanceValue == 2)
        //    {
        //        Subject = "Operation team need more clarity";
        //        BodyCustom = "Hi,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The details shared by you in this HR is not enough to start searching for the talent. So, kindly coordinate with the Ops team and shared the missing details to them.");
        //    }


        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Below are the HR details:");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Company Name: " + CompanyName);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Client Name: " + ClientName);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Client Email: " + ClientEmail);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("HR ID: " + HiringRequestData.HrNumber);
        //    sbBody.Append("<br/>");
        //    if (AcceptanceValue == 2)
        //    {
        //        sbBody.Append("Add details which are missing to or need more clarity : " + Reason);
        //        sbBody.Append("<br/>");
        //    }
        //    if (IsManaged)
        //    {
        //        sbBody.Append("Manage: Yes");
        //    }
        //    else
        //    {
        //        sbBody.Append("Self Manage: Yes");
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Thanks");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    #endregion

        //    #region Participants For Email

        //    InternalCCEmailID = emailDatabaseContentProvider.GetCCEmailIdValues();
        //    InternalCCEmailName = emailDatabaseContentProvider.GetCCEmailNameValues();

        //    ToEmail = SalesPersonEmail;
        //    ToEmailName = SalesPersonName;

        //    if (IsAdhoc)
        //    {
        //        AdHocEMAIL = EmailDatabaseContentProvider.GetAdHocCCEmailIdValues();
        //        AdHocEMAILNAME = EmailDatabaseContentProvider.GetAdHocCCEmailNameValues();

        //        CCEMAIL = ccEmail + InternalCCEmailID + AdHocEMAIL;
        //        CCEMAILNAME = ccEmailName + InternalCCEmailName + AdHocEMAILNAME;

        //    }
        //    else
        //    {

        //        CCEMAIL = ccEmail + InternalCCEmailID;
        //        CCEMAILNAME = ccEmailName + InternalCCEmailName;

        //    }

        //    SMEMAIL = EmailDatabaseContentProvider.GetSMCCEmailIdValues();
        //    SMEMAILNAME = EmailDatabaseContentProvider.GetSMCCEmailNameValues();

        //    CCEMAIL += SMEMAIL;
        //    CCEMAILNAME += SMEMAILNAME;

        //    #endregion

        //    if (!string.IsNullOrEmpty(ToEmail) && !string.IsNullOrEmpty(CCEMAIL))
        //    {
        //        List<string> toemail = new() { ToEmail };
        //        List<string> toemailname = new() { ToEmailName };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        return "Success";
        //    }
        //    else
        //        return "Error";
        //}
        //public bool SendEmailForRenewal(long ContactID, long TalentID, long HRID, long OnBoardID, DateTime? ContractStartDate, DateTime? ContractEndDate, List<Sproc_Get_Hierarchy_For_Email_Result> sproc_Get_Hierarchy_For_Email_SalesUser)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, OnBoardID, false, HRID, ContactID, 0);

        //        #region Email Content
        //        Subject = "Renewal | " + emailDetail.EngagementID;
        //        BodyCustom = "Hi Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hope you are doing well!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We would like to inform you that <b>" + emailDetail.EngagementID + "</b> has been renewed by the client.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please find below required details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Company : " + emailDetail.CompanyName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client : " + emailDetail.clientName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Engagement ID : " + emailDetail.EngagementID);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("HR ID : " + emailDetail.HR_Number);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Contract Start : " + ContractStartDate);
        //        sbBody.Append("<br/>");
        //        if (ContractEndDate != null)
        //        {
        //            sbBody.Append("Contract End : " + ContractEndDate);
        //            sbBody.Append("<br/>");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //        #endregion

        //        #region Commented code

        //        //List<string> internal_toEmail = new List<string>() { emailDetail.salesemailid };
        //        //List<string> internal_toEmailName = new List<string>() { emailDetail.salesName };

        //        //string InternalCCEmailID = emailDatabaseContentProvider.GetCCEmailIdValues();
        //        //string InternalCCEmailName = emailDatabaseContentProvider.GetCCEmailNameValues();

        //        //string AdHocEMAIL = "";
        //        //string AdHocEMAILNAME = "";

        //        //string ccEmailId = "";
        //        //string ccEmailName = "";

        //        //string ManagedEMAIL = "";
        //        //string ManagedEMAILNAME = "";

        //        //string SMEMAIL = "";
        //        //string SMEMAILNAME = "";

        //        //if (emailDetail.IsAdHoc)
        //        //{
        //        //    AdHocEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("AdhocCCEmailIds");
        //        //    AdHocEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("AdhocCCEmailName");

        //        //    ccEmailId = InternalCCEmailID + AdHocEMAIL;
        //        //    ccEmailName = InternalCCEmailName + AdHocEMAILNAME;

        //        //}
        //        //else
        //        //{
        //        //    ccEmailId = InternalCCEmailID;
        //        //    ccEmailName = InternalCCEmailName;

        //        //}

        //        //if (emailDetail.IsManaged)
        //        //{
        //        //    ManagedEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("ManagedCCEmailIds");
        //        //    ManagedEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("ManagedCCEmailNames");

        //        //    ccEmailId = ccEmailId += ManagedEMAIL;
        //        //    ccEmailName = ccEmailName += ManagedEMAILNAME;
        //        //}
        //        //if (!emailDetail.IsManaged)
        //        //{
        //        //    SMEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("SMCCEmailIds");
        //        //    SMEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("SMCCEmailNames");

        //        //    ccEmailId = ccEmailId += SMEMAIL;
        //        //    ccEmailName = ccEmailName += SMEMAILNAME;
        //        //}

        //        //if (sproc_Get_Hierarchy_For_Email_SalesUser != null)
        //        //{
        //        //    foreach (var item in sproc_Get_Hierarchy_For_Email_SalesUser)
        //        //    {
        //        //        ccEmailId += "," + item.EmailId;
        //        //        ccEmailName += "," + item.UserName;
        //        //    }
        //        //}
        //        #endregion

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //        string TOEMAIL = emailDetail.salesemailid;
        //        string TOEMAILNAME = emailDetail.salesName;

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        MakeCCDetail ccDetails = MakeCCEmailDetails(emailDetail.HRSalesPersonID, true);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }
        //        #endregion

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }

        //}
        //public bool SendEmailForPriorityCountChange(long UserId, int oldPriorityCount, int newPriorityCount, List<Sproc_Get_HRDetails_For_UserPriority_Email_Result> sproc_Get_HRDetails_For_UserPriority_Email_Results)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        var userdetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == UserId).Select(x => new
        //        {
        //            EmailId = x.EmailId,
        //            Name = x.FullName,
        //            ID = x.Id
        //        }).FirstOrDefault();

        //        #region Variable
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        if (userdetails != null)
        //        {
        //            string Subject = "", BodyCustom = "";
        //            StringBuilder sbBody = new StringBuilder();

        //            Subject = $"Head Priority Change";
        //            BodyCustom = $"Hello <b>{userdetails.Name}</b>,";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"Your priority count on basis of target has been revised to <b>{newPriorityCount}</b> for this month. ");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"You already have  <b>{oldPriorityCount}</b> opportunities into priorities.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            if (sproc_Get_HRDetails_For_UserPriority_Email_Results.Any())
        //            {
        //                sbBody.Append(string.Format("Please rearrange priorities for this month if you wish to make any changes."));
        //                sbBody.Append("<br/>");
        //                sbBody.Append("<br/>");
        //                sbBody.Append(string.Format("List of HR"));
        //                sbBody.Append("<br/>");
        //                sbBody.Append("<br/>");
        //                sbBody.Append(string.Format("<table style='border-style:solid;width:100%;'>"));
        //                sbBody.Append(string.Format("<tr><th style='text-align: left;'>HR #</th><th style='text-align: left;'>Company</th><th style='text-align: left;'>Role</th><th style='text-align: left;'>HR status</th><th style='text-align: left;'>Talent</th></tr>"));
        //                foreach (var user in sproc_Get_HRDetails_For_UserPriority_Email_Results)
        //                {
        //                    sbBody.Append(string.Format("<tr><td style='text-align:left;'>" + user.HR_Number + "</td><td style='text-align: left;'>" + user.Company + "</td><td style='text-align: left;'>" + user.HRRole + "</td><td style='text-align: left;'>" + user.HRStatus + "</td><td style='text-align: left;'>" + user.Talent + "</td></tr>"));

        //                }
        //                sbBody.Append(string.Format("</table>"));
        //            }
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Thank you,");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");

        //            #region Commented code

        //            //List<string> internal_toEmail = new List<string>() { userdetails.EmailId };
        //            //List<string> internal_toEmailName = new List<string>() { userdetails.Name };

        //            //string ccEmailId = "";
        //            //string ccEmailName = "";

        //            //string HRBuddyEmailName = emailDatabaseContentProvider.GetCCHRBuddyNameValues();
        //            //string HRBuddyEmailId = emailDatabaseContentProvider.GetCCHRBuddyEmailValues();
        //            //ccEmailId = _configuration["app_settings:CCEmailId"] + "," + _configuration["app_settings:CC1EmailId"] + "," + _configuration["app_settings:TSCEmailId"] + HRBuddyEmailId; ;
        //            //ccEmailName = _configuration["app_settings:CCEmailName"] + ',' + _configuration["app_settings:CC1EmailName"] + ',' + _configuration["app_settings:TSCEmailName"] + HRBuddyEmailName;
        //            #endregion

        //            #region need to send person & the Hierarchy - New Changes as per Mehul sir suggested

        //            string TOEMAIL = userdetails.EmailId;
        //            string TOEMAILNAME = userdetails.Name;

        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            string CCEMAIL = "";
        //            string CCEMAILNAME = "";

        //            MakeCCDetail ccDetails = MakeCCEmailDetails(userdetails.ID, false);
        //            if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //            {
        //                CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //            }
        //            #endregion

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion

        //            emailsent = true;
        //            return emailsent;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        ////public bool SetPasswordSendEmail(GenContact varContactDetail, string invitingUserName, string invitingIserEmailId, string Designation, long PocID = 0)
        ////{
        ////    try
        ////    {
        ////        var reactClientPortalURL = _configuration["ReactClientPortalURL"];
        ////        var varSMTPEmailName = _configuration["app_settings:SMTPEmailName"];
        ////        EmailOperator emailOperator = new EmailOperator(_configuration);
        ////        string Subject = "";

        ////        StringBuilder sbBody = new();
        ////        Subject = "Welcome Aboard! Let's Get Your First Job Posted on Uplers";
        ////        sbBody.Append("<div style='width:100%'>");

        ////        if (varContactDetail != null)
        ////        {
        ////            sbBody.Append("Hello " + varContactDetail.FullName + ",");
        ////        }

        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append($"Welcome to Uplers! We're thrilled you're here to discover top talent from India.");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append($"Please login to your account to start posting jobs, exploring candidates, and managing your recruitment with ease.");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("<br/>");
        ////        if (varContactDetail != null)
        ////        {
        ////            sbBody.Append($"Username : " + varContactDetail.Username);
        ////            sbBody.Append("<br/>");
        ////            sbBody.Append("Password : " + varContactDetail.Password);
        ////            sbBody.Append("<br/>");
        ////            sbBody.Append("<br/>");
        ////        }
        ////        if (varContactDetail != null)
        ////        {
        ////            //sbBody.Append("Kindly click <a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' class='link' href='" + reactClientPortalURL + "login?type=" + MyExtensions.Encrypt("IC") + "&emailid=" + varContactDetail.Username + "&password=" + varContactDetail.Password + "' target='_blank'>here</a> to login.");
        ////            sbBody.Append("Kindly click <a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' class='link' href='" + reactClientPortalURL + "login?type=" + MyExtensions.Encrypt("IC") + "' target='_blank'>here</a> to login.");
        ////        }
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("Why post a job on Uplers?");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("<ul><li>Pre-vetted talent matched accurately as per your job requirement </li><li>Save your time and concentrate on the right talents.</li><li>Receive comprehensive reports and video screened responses for your job.</li><li>Dedicated support to assist you every step of the way.</li></ul>");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("Ready to find the perfect fit for your team? Don't wait any longer! Start by posting your job and get matched with the top talent on Uplers.");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("If you need help or have questions, please reach out to us on client@uplers.com");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("We look forward to supporting your hiring journey!");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("Uplers");
        ////        sbBody.Append("<br/>");
        ////        sbBody.Append("<hr>");
        ////        sbBody.Append("We are committed to your privacy. ");
        ////        sbBody.Append("You may unsubscribe from these communications at any time. ");
        ////        sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.</br><br/>");
        ////        sbBody.Append("</div>");

        ////        List<string> toEmail = new List<string>
        ////        {
        ////            varContactDetail.EmailId
        ////        };

        ////        List<string> toEmailName = new List<string>
        ////        {
        ////            varContactDetail.FullName
        ////        };

        ////        string strBccEmail = "", strBccEamilName = "";
        ////        //long PocID = 0;
        ////        //var obj = _talentConnectAdminDBContext.sproc_Get_ContactPointofContact_Result.FirstOrDefault();

        ////        object[] param = new object[]
        ////        {
        ////            varContactDetail.Id
        ////        };

        ////        string paramString = CommonLogic.ConvertToParamStringWithNull(param);
        ////        string CCEMAIL = "";
        ////        string CCEMAILNAME = "";
        ////        if (PocID != 0)
        ////        {
        ////            var varGetUser = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == PocID).FirstOrDefault();
        ////            if (varGetUser != null)
        ////            {
        ////                strBccEmail = varGetUser.EmailId;
        ////                strBccEamilName = varGetUser.FullName;
        ////            }
        ////            var POCUserHierarchy = GetHierarchyForEmail(PocID.ToString());

        ////            CCEMAIL = string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.EmailId)).Select(x => x.EmailId));
        ////            CCEMAILNAME = string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Select(x => x.UserName));

        ////        }
        ////        else
        ////        {
        ////            var obj = sproc_Get_ContactPointofContact(paramString); //_talentConnectAdminDBContext.Set<sproc_Get_ContactPointofContact_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Get_ContactPointofContact, paramString)).AsEnumerable().FirstOrDefault();
        ////            if (obj != null)
        ////            {
        ////                var varGetUser = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == obj.User_ID).FirstOrDefault();
        ////                if (varGetUser != null)
        ////                {
        ////                    strBccEmail = varGetUser.EmailId;
        ////                    strBccEamilName = varGetUser.FullName;

        ////                }
        ////                var POCUserHierarchy = GetHierarchyForEmail(obj.User_ID.ToString());

        ////                CCEMAIL = string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.EmailId)).Select(x => x.EmailId));
        ////                CCEMAILNAME = string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Select(x => x.UserName));
        ////            }
        ////        }
        ////        List<string> bccEmail = new List<string>
        ////        {
        ////            strBccEmail
        ////        };

        ////        List<string> bccEmailName = new List<string>
        ////        {
        ////            strBccEamilName
        ////        };


        ////        if (!string.IsNullOrEmpty(strBccEmail) && !string.IsNullOrEmpty(strBccEamilName))
        ////        {
        ////            CCEMAIL = CCEMAIL + "," + strBccEmail;
        ////            CCEMAILNAME = CCEMAILNAME + "," + strBccEamilName;
        ////        }


        ////        #region SetParam
        ////        emailOperator.SetToEmail(toEmail);
        ////        if (!string.IsNullOrEmpty(CCEMAIL) && !string.IsNullOrEmpty(CCEMAILNAME))
        ////            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        ////        emailOperator.SetToEmailName(toEmailName);
        ////        emailOperator.SetSubject(Subject);
        ////        emailOperator.SetBody(sbBody.ToString());
        ////        #endregion

        ////        if (!string.IsNullOrEmpty(Subject))
        ////            emailOperator.SendEmailClientPortal(false, true);

        ////        return true;
        ////    }
        ////    catch
        ////    {
        ////        return false;
        ////    }
        ////}
        //public string SendEmailForTRAcceptanceToInternalTeam(string webrootpath, long HRID, int AcceptanceValue, int? TRParked)
        //{
        //    try
        //    {
        //        #region Variable
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "", companyName = "", ClientName = "", ClientEmail = "";
        //        StringBuilder sbBody = new StringBuilder();
        //        bool IsAdHoc, IsManaged = false;
        //        GenSalesHiringRequest _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == HRID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(_SalesHiringRequest).Reload();
        //        if (_SalesHiringRequest != null)
        //        {
        //            var ContactId = _SalesHiringRequest.ContactId;
        //            GenContact contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ContactId).FirstOrDefault();
        //            //_talentConnectAdminDBContext.Entry(contact).Reload();
        //            if (contact != null)
        //            {
        //                ClientName = contact.FullName;
        //                ClientEmail = contact.EmailId;
        //                GenCompany company = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == contact.CompanyId).FirstOrDefault();
        //                //_talentConnectAdminDBContext.Entry(company).Reload();
        //                if (company != null)
        //                {
        //                    companyName = company.Company;
        //                }
        //            }

        //            SalesPersonName = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.FullName).FirstOrDefault();
        //            salesPersonEmail = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.EmailId).FirstOrDefault();

        //            IsAdHoc = _SalesHiringRequest.IsAdHocHr;
        //            IsManaged = _SalesHiringRequest.IsManaged;

        //            Subject = "Operation team has accepted the TR for further process";
        //            BodyCustom = "Hi,";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Ops team has accepted the TR added by you and have started searching for the right talent.");

        //            sbBody.Append("<div style='width:100%'>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Below are the TR details:");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Company Name: " + companyName);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Client Name: " + ClientName);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Client Email: " + ClientEmail);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("HR ID: " + _SalesHiringRequest.HrNumber);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("TR Accepted: " + AcceptanceValue);
        //            //sbBody.Append("<br/>");
        //            //sbBody.Append("TR Parked: " + TRParked);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Thanks");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<hr>");
        //            sbBody.Append("We are committed to your privacy. ");
        //            sbBody.Append("You may unsubscribe from these communications at any time. ");
        //            sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //            sbBody.Append("</div>");

        //            emailDatabaseContentProvider.SalesUserId = Convert.ToInt64(_SalesHiringRequest.SalesUserId);

        //            #region Commneted code
        //            //string InternalCCEmailID = emailDatabaseContentProvider.GetCCEmailIdValues();
        //            //string InternalCCEmailName = emailDatabaseContentProvider.GetCCEmailNameValues();

        //            //List<string> internal_toEmail = new List<string>() { salesPersonEmail };
        //            //List<string> internal_toEmailName = new List<string>() { SalesPersonName };

        //            //string CCEMAIL = "";
        //            //string CCEMAILNAME = "";

        //            //string AdHocEMAIL = "";
        //            //string AdHocEMAILNAME = "";

        //            //string ManagedEMAIL = "";
        //            //string ManagedEMAILNAME = "";

        //            //string SMEMAIL = "";
        //            //string SMEMAILNAME = "";

        //            //if (IsAdHoc)
        //            //{
        //            //    AdHocEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("AdhocCCEmailIds");
        //            //    AdHocEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("AdhocCCEmailName");


        //            //    CCEMAIL = ccEmail + ',' + InternalCCEmailID + AdHocEMAIL;
        //            //    CCEMAILNAME = ccEmailName + ',' + InternalCCEmailName + AdHocEMAILNAME;
        //            //}
        //            //else
        //            //{
        //            //    CCEMAIL = ccEmail + ',' + InternalCCEmailID;
        //            //    CCEMAILNAME = ccEmailName + ',' + InternalCCEmailName;
        //            //}

        //            //if (IsManaged)
        //            //{
        //            //    ManagedEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("ManagedCCEmailIds");
        //            //    ManagedEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("ManagedCCEmailNames");

        //            //    CCEMAIL = CCEMAIL + ManagedEMAIL;
        //            //    CCEMAILNAME = CCEMAILNAME + ManagedEMAILNAME;
        //            //}
        //            //if (!IsManaged)
        //            //{
        //            //    SMEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("SMCCEmailIds");
        //            //    SMEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("SMCCEmailNames");

        //            //    CCEMAIL = CCEMAIL + SMEMAIL;
        //            //    CCEMAILNAME = CCEMAILNAME + SMEMAILNAME;
        //            //}
        //            #endregion

        //            #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //            string TOEMAIL = salesPersonEmail;
        //            string TOEMAILNAME = SalesPersonName;

        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            string CCEMAIL = "";
        //            string CCEMAILNAME = "";

        //            MakeCCDetail ccDetails = MakeCCEmailDetails(emailDatabaseContentProvider.SalesUserId, true);
        //            if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //            {
        //                CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //            }
        //            #endregion

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion

        //            return "success";
        //        }
        //        else
        //            return "error";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public string SendClientFeedbackFromEngagementToInternalTeam(long AMID, string AMName, string AMEmail, DateTime FeedbackDate, string FeedbackType, string FeedbackComments, string ActionToTake, string TscName, string TscEmail)
        //{
        //    try
        //    {
        //        #region Variable
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        string Subject = "", BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();
        //        bool IsAdHoc, IsManaged = false;

        //        Subject = $"{AMName} - Submitted Client Feedback";
        //        BodyCustom = "Hi Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"{AMName} - Submitted Client Feedback");

        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Below are the Feedback details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Feedback Date: " + FeedbackDate);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Feedback Type:  " + FeedbackType);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Feedback Comments: " + FeedbackComments);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Action to Take: " + ActionToTake);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        List<string> toEmail = new List<string> { AMEmail };
        //        List<string> toEmailName = new List<string> { AMName };

        //        object[] param = new object[] { AMID };
        //        string paramString = CommonLogic.ConvertToParamStringWithNull(param);

        //        var obj = sproc_Get_ContactPointofContact(paramString);
        //        if (obj != null && obj.User_ID > 0)
        //        {
        //            MakeCCDetail ccDetails = MakeCCEmailDetails((long)obj.User_ID, false);
        //            if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //            {
        //                CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(TscEmail) && !string.IsNullOrEmpty(TscName))
        //        {
        //            if (!string.IsNullOrEmpty(CCEMAIL) && !string.IsNullOrEmpty(CCEMAILNAME))
        //            {
        //                CCEMAIL = CCEMAIL + ',' + TscEmail;
        //                CCEMAILNAME = CCEMAILNAME + ',' + TscName;
        //            }
        //            else
        //            {
        //                CCEMAIL = TscEmail;
        //                CCEMAILNAME = TscName;
        //            }
        //        }

        //        #region SetParam
        //        emailOperator.SetToEmail(toEmail);
        //        emailOperator.SetToEmailName(toEmailName);
        //        if (!string.IsNullOrEmpty(CCEMAIL) && !string.IsNullOrEmpty(CCEMAILNAME))
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());
        //        #endregion

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        return "success";
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ////UTS-3117: Edit TR email notifications should be sent to the internal team members.
        //#region Send Emails when TR is Edited
        //public string SendEmailForHRStatusUpdateToInternalTeam(long HRID, string Reason, string lostCancelledTR = "", bool isHRCloseCall = false)
        //{
        //    try
        //    {
        //        string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "", companyName = "", ClientName = "", ClientEmail = "";
        //        StringBuilder sbBody = new StringBuilder();
        //        bool IsAdHoc, IsManaged = false;
        //        GenSalesHiringRequest _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == HRID).FirstOrDefault();
        //        _talentConnectAdminDBContext.Entry(_SalesHiringRequest).Reload();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        #endregion

        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(0, 0, 0, false, HRID, 0, 0);

        //        if (_SalesHiringRequest != null)
        //        {
        //            //get the status of the HR
        //            int hrStatusId = Convert.ToInt32(_SalesHiringRequest.StatusId);
        //            //PrgHiringRequestStatus prg_HiringRequestStatus = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == hrStatusId).FirstOrDefault();
        //            //string hrStatus = prg_HiringRequestStatus.HiringRequestStatus;

        //            string hrStatus = string.Empty;
        //            if (string.IsNullOrEmpty(emailDetail.HRStatus))
        //            {
        //                hrStatus = _talentConnectAdminDBContext.PrgJobStatusClientPortals.Where(x => x.Id == _SalesHiringRequest.JobStatusId).FirstOrDefault().JobStatus;
        //            }
        //            else
        //            {
        //                hrStatus = emailDetail.HRStatus;
        //            }


        //            //get the number of hired profiles.
        //            int hiredTalentCount = 0;

        //            if (!isHRCloseCall)
        //            {
        //                hiredTalentCount = _talentConnectAdminDBContext.GenOnBoardTalents.Where(t => t.HiringRequestId == HRID && t.StatusId == 1).Count();
        //            }

        //            var ContactId = _SalesHiringRequest.ContactId;
        //            GenContact contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ContactId).FirstOrDefault();
        //            //_talentConnectAdminDBContext.Entry(contact).Reload();
        //            if (contact != null)
        //            {
        //                ClientName = contact.FullName;
        //                ClientEmail = contact.EmailId;
        //                GenCompany company = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == contact.CompanyId).FirstOrDefault();
        //                //_talentConnectAdminDBContext.Entry(company).Reload();
        //                if (company != null)
        //                {
        //                    companyName = company.Company;
        //                }
        //            }

        //            var salesPersonDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == _SalesHiringRequest.SalesUserId).FirstOrDefault();

        //            SalesPersonName = salesPersonDetails.FullName;
        //            salesPersonEmail = salesPersonDetails.EmailId;
        //            string role = _SalesHiringRequest.RequestForTalent;

        //            IsAdHoc = _SalesHiringRequest.IsAdHocHr;
        //            IsManaged = _SalesHiringRequest.IsManaged;

        //            IsAdHoc = _SalesHiringRequest.IsAdHocHr;
        //            IsManaged = _SalesHiringRequest.IsManaged;
        //            string typeOfHR = _SalesHiringRequest.IsHrtypeDp ? "Direct Placement" : "Contractual";

        //            BodyCustom = "Hello Team,";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/><br/>");
        //            sbBody.Append("Greetings for the day!");
        //            sbBody.Append("<br/><br/>");

        //            Subject = $"HR {hrStatus} : {_SalesHiringRequest.HrNumber} | {companyName} | {role}";
        //            sbBody.Append($"Please be informed that {_SalesHiringRequest.HrNumber} has been marked as {hrStatus}.");

        //            //if the action is called from close HR.
        //            if (isHRCloseCall)
        //            {
        //                Subject = $"HR {hrStatus}";
        //                if (!string.IsNullOrEmpty(lostCancelledTR))
        //                {
        //                    sbBody.Append($"&nbsp;{lostCancelledTR}.");
        //                }
        //            }

        //            sbBody.Append("&nbsp;Below are the required details:");
        //            sbBody.Append("<div style='width:100%'>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("HR number: " + _SalesHiringRequest.HrNumber);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Company: " + companyName);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Sales person: " + SalesPersonName);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Reason: " + Reason);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Role: " + role);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Type of HR: " + typeOfHR);
        //            sbBody.Append("<br/>");

        //            //if (hiredTalentCount > 0 && (hrStatusId == 1 || hrStatusId == 2))
        //            if (hiredTalentCount > 0 && (hrStatusId == 1 || hrStatusId == 4))
        //            {
        //                sbBody.Append("<br/>");
        //                sbBody.Append($"Please note that {hiredTalentCount} out {_SalesHiringRequest.NoofTalents} TR has been achieved for HR.");
        //                sbBody.Append("<br/>");
        //            }

        //            sbBody.Append("<br/>");

        //            sbBody.Append("Thanks");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<hr>");
        //            sbBody.Append("We are committed to your privacy. ");
        //            sbBody.Append("You may unsubscribe from these communications at any time. ");
        //            sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.");
        //            sbBody.Append("</div>");

        //            emailDatabaseContentProvider.SalesUserId = Convert.ToInt64(_SalesHiringRequest.SalesUserId);

        //            #region Need to send person & the Hierarchy - New Changes as per Mehul sir suggested

        //            string TOEMAIL = salesPersonEmail;
        //            string TOEMAILNAME = SalesPersonName;

        //            string CCEMAIL = "";
        //            string CCEMAILNAME = "";

        //            MakeCCDetail ccDetails = MakeCCEmailDetails(emailDatabaseContentProvider.SalesUserId, true);
        //            if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //            {
        //                CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //            }
        //            #endregion

        //            if (!string.IsNullOrEmpty(TOEMAIL) && !string.IsNullOrEmpty(CCEMAIL))
        //            {
        //                List<string> toemail = new() { TOEMAIL };
        //                List<string> toemailname = new() { TOEMAILNAME };

        //                #region Variable
        //                EmailOperator emailOperator = new EmailOperator(_configuration);
        //                #endregion

        //                emailOperator.SetToEmail(toemail);
        //                emailOperator.SetToEmailName(toemailname);
        //                emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //                emailOperator.SetSubject(Subject);
        //                emailOperator.SetBody(sbBody.ToString());

        //                #region SendEmail
        //                if (!string.IsNullOrEmpty(Subject))
        //                    emailOperator.SendEmail();
        //                #endregion

        //                return "Success";
        //            }
        //            else
        //                return "Error";
        //        }
        //        else
        //            return "error";
        //    }
        //    catch (Exception e) { return e.Message; }
        //}
        //#endregion

        ////UTS-3641: Send email notification to the internal team when HR is reopened.
        //#region Send Email on HR Reopening
        //public string SendEmailForHRReopeningToInternalTeam(long hrId, decimal originalTR, int originalHRStatus, ref bool sendTRUpdate)
        //{
        //    try
        //    {
        //        string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "", companyName = "", ClientName = "", ClientEmail = "";
        //        StringBuilder sbBody = new StringBuilder();
        //        bool IsAdHoc, IsManaged = false;
        //        GenSalesHiringRequest _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == hrId).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(_SalesHiringRequest).Reload();

        //        int hrStatusId = Convert.ToInt32(_SalesHiringRequest.JobStatusId);


        //        if (_SalesHiringRequest != null)
        //        {
        //            var ContactId = _SalesHiringRequest.ContactId;
        //            GenContact contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ContactId).FirstOrDefault();
        //            //_talentConnectAdminDBContext.Entry(contact).Reload();
        //            if (contact != null)
        //            {
        //                ClientName = contact.FullName;
        //                ClientEmail = contact.EmailId;
        //                GenCompany company = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == contact.CompanyId).FirstOrDefault();
        //                //_talentConnectAdminDBContext.Entry(company).Reload();
        //                if (company != null)
        //                {
        //                    companyName = company.Company;
        //                }
        //            }

        //            var salesPersonDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == _SalesHiringRequest.SalesUserId).FirstOrDefault();

        //            if (salesPersonDetails != null)
        //            {
        //                SalesPersonName = salesPersonDetails.FullName;
        //                salesPersonEmail = salesPersonDetails.EmailId;
        //            }
        //            string role = _SalesHiringRequest.RequestForTalent;

        //            IsAdHoc = _SalesHiringRequest.IsAdHocHr;
        //            IsManaged = _SalesHiringRequest.IsManaged;

        //            IsAdHoc = _SalesHiringRequest.IsAdHocHr;
        //            IsManaged = _SalesHiringRequest.IsManaged;

        //            int emailCase = 0;
        //            int updatedTR = _SalesHiringRequest.NoofTalents.Value;

        //            //if (originalHRStatus == 3 && updatedTR > originalTR)
        //            //{
        //            //    emailCase = 1;
        //            //}

        //            //if (originalHRStatus == 4 && !sendTRUpdate)
        //            //{
        //            //    emailCase = 2;
        //            //}

        //            //if (originalHRStatus == 6 && !sendTRUpdate)
        //            //{
        //            //    emailCase = 3;
        //            //}

        //            //if ((originalHRStatus == 4 || originalHRStatus == 6) && updatedTR != originalTR)
        //            //{
        //            //    if (sendTRUpdate)
        //            //    {
        //            //        emailCase = 4;
        //            //    }
        //            //    else
        //            //    {
        //            //        sendTRUpdate = true;
        //            //    }
        //            //}

        //            if (hrStatusId == 2 && updatedTR > originalTR) //Closed but Updated TR > Original TR
        //            {
        //                emailCase = 1;
        //            }

        //            if (hrStatusId == 2 && !sendTRUpdate)   // Closed But not Sent Update
        //            {
        //                emailCase = 2;
        //            }

        //            if (hrStatusId == 2 && updatedTR != originalTR) // Closed But not matched count
        //            {
        //                if (sendTRUpdate)
        //                {
        //                    emailCase = 4;
        //                }
        //                else
        //                {
        //                    sendTRUpdate = true;
        //                }
        //            }



        //            sbBody = SetHRReopeningMailContent(_SalesHiringRequest.HrNumber, SalesPersonName, emailCase, ref Subject);

        //            sbBody.Append("<br/><br/>");
        //            sbBody.Append("Thanks");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<hr>");
        //            sbBody.Append("We are committed to your privacy. ");
        //            sbBody.Append("You may unsubscribe from these communications at any time. ");
        //            sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.");
        //            sbBody.Append("</div>");

        //            emailDatabaseContentProvider.SalesUserId = Convert.ToInt64(_SalesHiringRequest.SalesUserId);

        //            #region Need to send person & the Hierarchy - New Changes as per Mehul sir suggested

        //            string TOEMAIL = salesPersonEmail;
        //            string TOEMAILNAME = SalesPersonName;

        //            string CCEMAIL = "";
        //            string CCEMAILNAME = "";

        //            MakeCCDetail ccDetails = MakeCCEmailDetails(emailDatabaseContentProvider.SalesUserId, true);
        //            if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //            {
        //                CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //            }
        //            #endregion

        //            if (!string.IsNullOrEmpty(TOEMAIL) && !string.IsNullOrEmpty(CCEMAIL))
        //            {
        //                List<string> toemail = new() { TOEMAIL };
        //                List<string> toemailname = new() { TOEMAILNAME };

        //                #region Variable
        //                EmailOperator emailOperator = new EmailOperator(_configuration);
        //                #endregion

        //                emailOperator.SetToEmail(toemail);
        //                emailOperator.SetToEmailName(toemailname);
        //                emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //                emailOperator.SetSubject(Subject);
        //                emailOperator.SetBody(sbBody.ToString());

        //                #region SendEmail
        //                if (!string.IsNullOrEmpty(Subject))
        //                    emailOperator.SendEmail();
        //                #endregion

        //                return "Success";
        //            }
        //            else
        //                return "Error";
        //        }
        //        else
        //            return "error";
        //    }
        //    catch (Exception e) { return e.Message; }
        //}
        //private StringBuilder SetHRReopeningMailContent(string hrNumber, string salesPersonName, int emailCase, ref string subject)
        //{
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Case 1 when completed(Closed - Won) HR is reopened 
        //    if (emailCase == 1)
        //    {
        //        subject = $"Urgent: Increased Talent Requirement - Reopening Job Position {hrNumber}";
        //        sbBody.Append($"Dear {salesPersonName}");
        //        sbBody.Append("<br/><br/>");
        //        sbBody.Append($"We hope this message finds you well. We are writing to bring your attention to the job position under <strong>{hrNumber}</strong>. Due to an increase in the requirement for talent in this particular area, we are reopening this position.");
        //        sbBody.Append("<br/><br/>");
        //        sbBody.Append($"We kindly request you to review the details of the <strong>{hrNumber}</strong> and take necessary action to facilitate the recruitment process. Should you need any additional information or support, please do not hesitate to reach out.");
        //        sbBody.Append("<br/><br/>");
        //        sbBody.Append("We appreciate your immediate attention and assistance in this matter.");
        //    }
        //    #endregion

        //    #region Case 2 When cancelled HR is reopened with same TR
        //    if (emailCase == 2)
        //    {
        //        subject = $"Alert: Reopening of Closed {hrNumber}";
        //        sbBody.Append("Dear Team");
        //        sbBody.Append("<br/><br/>");
        //        sbBody.Append($"We hope this message finds you well. We would like to inform you that the <strong>{hrNumber}</strong> position, which was previously cancelled, has now been reopened.");
        //        sbBody.Append("<br/><br/>");
        //        sbBody.Append("We kindly request all team members to note this update and adjust your actions accordingly. Once the status change is confirmed, we will proceed with the same Talent Requirement (TR), with the option to update the TR as per the current needs.");
        //        sbBody.Append("<br/><br/>");
        //        sbBody.Append("We appreciate your immediate attention to this matter. Please feel free to reach out if you have any questions or concerns.");
        //    }
        //    #endregion

        //    //#region Case 3 When Lost TR is reopened with same TR
        //    //if (emailCase == 3)
        //    //{
        //    //    subject = $"Update: Previously Lost {hrNumber} Now In Process";
        //    //    sbBody.Append("Dear Team");
        //    //    sbBody.Append("<br/><br/>");
        //    //    sbBody.Append($"We trust this message finds you in good spirits. We wanted to update you that the <strong>{hrNumber}</strong> position, which was previously lost, is now in the process of being reopened.");
        //    //    sbBody.Append("<br/><br/>");
        //    //    sbBody.Append("Please align your activities accordingly during this transition period. As we progress, we will maintain the same Talent Requirement (TR) and introduce the feature to update the TR based on the dynamic organisational needs.");
        //    //    sbBody.Append("<br/><br/>");
        //    //    sbBody.Append("Your attention and cooperation in this matter are highly appreciated. Feel free to reach out with any questions or concerns.");
        //    //}
        //    //#endregion

        //    #region Case 4 When Cancelled/Lost HR is reopened with new TR values
        //    if (emailCase == 4)
        //    {
        //        subject = $"Update: Talent Requirement Adjustment for {hrNumber}";
        //        sbBody.Append("Dear Team");
        //        sbBody.Append("<br/><br/>");
        //        sbBody.Append($"We are reaching out to notify you that we have made necessary adjustments to the Talent Requirement (TR) for the <strong>{hrNumber}</strong> position in response to the client’s evolving organizational needs.");
        //        sbBody.Append("<br/><br/>");
        //        sbBody.Append("Please review these changes carefully and align your recruitment efforts accordingly. We believe these updates will enable us to attract and select the most suitable talent for our team.");
        //        sbBody.Append("<br/><br/>");
        //        sbBody.Append("As always, please feel free to contact us with any questions or need for clarification.");
        //    }
        //    #endregion

        //    return sbBody;
        //}
        //#endregion
        //public string SendEmailForAddingClientHappinessSurvey(long? ID, string Email, string Link, string client, long? LoggedInUserId)
        //{

        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Variable
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion


        //    Subject = "Your Input Matters: Complete Our Happiness Survey";
        //    BodyCustom = "";
        //    sbBody.Append("Dear " + client + ",");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("At Uplers, we prioritize not just meeting but exceeding your expectations. Your satisfaction drives us! Help us serve you better by completing our quick Happiness Survey.");
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("This brief survey will only take 2 minutes of your time and covers various aspects of your experience working with us. Your responses will remain confidential and will be used solely to improve our services to better serve you.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<center>");
        //    sbBody.Append("<div class='container' style='position: relative;width:50%;margin:0 auto;'>");
        //    sbBody.Append("<div class='container' style='margin: 0;position: absolute;top: 50%;left: 50%;-ms-transform: translate(-50%, -50%);transform: translate(-50%, -50%);'>");
        //    sbBody.Append("<a class='button' style='background-color: black;color: white;padding: 10px;text-decoration: none;display:inline-block;border-radius: 2px;' href = '" + Link + "' target='_blank'> Take the Survey </a>");
        //    sbBody.Append("</div>");
        //    sbBody.Append("</div>");
        //    sbBody.Append("</center>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Thanks");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");


        //    string TOEMAIL = Email;
        //    string TOEMAILNAME = client;

        //    string HRBuddyEmailName = emailDatabaseContentProvider.GetCCHRBuddyNameValues();
        //    string HRBuddyEmailId = emailDatabaseContentProvider.GetCCHRBuddyEmailValues();


        //    if (!string.IsNullOrEmpty(TOEMAIL))
        //    {
        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(HRBuddyEmailId, HRBuddyEmailName);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail(false, true);
        //        #endregion
        //    }

        //    return "success";
        //}
        //public bool SendTSCAssignmentEmailToOLDTSC(Sproc_GetEmailDetailForTSCAssignment_Result result)
        //{
        //    try
        //    {
        //        bool emailsent = false;

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "TSC Assignment | " + result.ClientName + " | " + result.HRID;
        //        BodyCustom = "Hello " + result.OldTSCName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hope you are doing well.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("This is to inform you that " + result.NewTSCName + " will be the new TSC for the " + result.ClientName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please find below details for your reference:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hiring Request ID: " + result.HRID + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Engagement ID: " + result.EngagementID + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Name: " + result.ClientName + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Role: " + result.Role + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Regards");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("</div>");

        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        List<string> toEmail = new List<string>
        //        {
        //          result.OldTSCEmail
        //        };

        //        List<string> toEmailName = new List<string>
        //        {
        //           result.OldTSCName
        //        };
        //        emailOperator.SetToEmail(toEmail);
        //        emailOperator.SetToEmailName(toEmailName);
        //        //emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendTSCAssignmentEmailToNewTSC(Sproc_GetEmailDetailForTSCAssignment_Result result)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        //string? NewTSCUser = "";
        //        //string? NewTSCUserEmail = "";
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        //#region GetNewTSC
        //        //if (TSCUserId != 0)
        //        // {
        //        //     UsrUser usr = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == TSCUserId).Select(x=>x).FirstOrDefault();
        //        //    if (usr != null)
        //        //    {
        //        //        NewTSCUser = usr.FullName;
        //        //        NewTSCUserEmail = usr.EmailId;
        //        //    }
        //        //}
        //        //#endregion
        //        Subject = "TSC Assignment | " + result.ClientName + " | " + result.HRID;
        //        ;
        //        BodyCustom = "Hello " + result.NewTSCName ?? "" + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hope you are doing well.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("This is to inform you that you have been assigned as TSC for the " + result.ClientName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please find below details for your reference:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hiring Request ID: " + result.HRID + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Engagement ID: " + result.EngagementID + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Name: " + result.ClientName + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Role: " + result.Role + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Regards");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("</div>");

        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        List<string> toEmail = new List<string>
        //        {
        //           result.NewTSCEmail
        //        };

        //        List<string> toEmailName = new List<string>
        //        {
        //            result.NewTSCName
        //        };
        //        emailOperator.SetToEmail(toEmail);
        //        emailOperator.SetToEmailName(toEmailName);
        //        //emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendTSCAutoAssignmentEmail(Sproc_GetEmailDetailForTSCAssignment_Result result)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();


        //        Subject = "TSC Assignment | " + result.ClientName + " | " + result.HRID;
        //        BodyCustom = "Hello " + result.OldTSCName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hope you are doing well.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("This is to inform you that you have been assigned as TSC for the " + result.ClientName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please find below details for your reference:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hiring Request ID: " + result.HRID + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Engagement ID: " + result.EngagementID + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Name: " + result.ClientName + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Role: " + result.Role + "<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Regards");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("</div>");

        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        List<string> toEmail = new List<string>
        //        {
        //          result.OldTSCEmail
        //        };

        //        List<string> toEmailName = new List<string>
        //        {
        //            result.OldTSCName
        //        };
        //        emailOperator.SetToEmail(toEmail);
        //        emailOperator.SetToEmailName(toEmailName);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public void SendSLAUpdateEmailToInternalTeam(Sproc_Get_SLAUpdateDetails_ForEmail_Result sLAUpdateEmailViewModel)
        //{
        //    string Subject = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    EmailOperator emailOperator = new EmailOperator(_configuration);

        //    Subject = $"Important SLA Update for {sLAUpdateEmailViewModel.ClientName}";

        //    sbBody.Append("Dear Team,");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"Please be advised that there has been an update in the SLA for the project with {sLAUpdateEmailViewModel.ClientName}. Here are the new details:");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"Old SLA Details: {sLAUpdateEmailViewModel.OldSlaDetails}");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"New SLA Details: {sLAUpdateEmailViewModel.UpdatedSlaDetails}");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"Reason for Update: {sLAUpdateEmailViewModel.UpdatedSlaReason}");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("It's imperative that we all review and adapt to these new terms as quickly as possible. Your dedication to adhering to these updates will ensure a seamless experience for our client and uphold the standards Uplers is known for.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Should you have any questions or need clarification on the new SLA, please don't hesitate to reach out.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Thank you for your immediate attention to this matter.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");

        //    sbBody.Append("Best regards,");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"{sLAUpdateEmailViewModel.PocName}");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"{sLAUpdateEmailViewModel.PocDesignation}");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers");
        //    sbBody.Append("<br/>");

        //    emailDatabaseContentProvider.SalesUserId = Convert.ToInt64(sLAUpdateEmailViewModel.SalesUserID);

        //    #region Need to send person & the Hierarchy - New Changes as per Mehul sir suggested

        //    string TOEMAIL = "";
        //    string TOEMAILNAME = "";

        //    var salesPersonDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == emailDatabaseContentProvider.SalesUserId).FirstOrDefault();

        //    if (salesPersonDetails != null)
        //    {
        //        TOEMAIL = salesPersonDetails.FullName;
        //        TOEMAILNAME = salesPersonDetails.EmailId;
        //    }

        //    string CCEMAIL = "";
        //    string CCEMAILNAME = "";

        //    MakeCCDetail ccDetails = MakeCCEmailDetails(emailDatabaseContentProvider.SalesUserId, true);
        //    if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //    {
        //        CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //        CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //    }
        //    #endregion

        //    if (!string.IsNullOrEmpty(TOEMAIL) && !string.IsNullOrEmpty(TOEMAILNAME))
        //    {
        //        List<string> toemail = new() { TOEMAIL };
        //        List<string> toemailname = new() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());
        //        emailOperator.SendEmail();
        //    }
        //}
        ////ATS-UTS Exception Email
        //public async Task<bool> sendErrorEmail(string API_Type, string End_Point, string Payloads, string ex)
        //{
        //    #region Variable
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    bool IsSendErrorEmail = false;

        //    bool.TryParse(_configuration["SendEmailException_UTSToATS"], out IsSendErrorEmail);

        //    if (IsSendErrorEmail)
        //    {
        //        string EmailSubject = "Talent Solutions Admin API :: Error Occurred";
        //        StringBuilder EmailBody = new StringBuilder();

        //        EmailBody.Append("Hi Admin,");
        //        EmailBody.Append("<br/><br/>");
        //        EmailBody.Append("<table border='1' style='font-size:13px;' cellpadding='2' cellspacing='2' width='50%'>");
        //        EmailBody.Append("<tr><td><b>API Type</b></td><td>" + API_Type + "</td></tr>");
        //        EmailBody.Append("<tr><td><b>End Point</b></td><td>" + End_Point + "</td></tr>");
        //        EmailBody.Append("<tr><td><b>Payloads</b></td><td>" + Payloads + "</td></tr>");
        //        EmailBody.Append("<tr><td><b>Exception</b></td><td>" + ex + "</td></tr>");
        //        EmailBody.Append("</table>");
        //        EmailBody.Append("<br/><br/>");
        //        EmailBody.Append("<br/><br/>");
        //        EmailBody.Append("Thank You,<br />Talent Solutions Admin @ Uplers");

        //        string EmailTo = _configuration["app_settings:ExceptionEmailId"];
        //        string EmailToName = _configuration["app_settings:ExceptionEmailName"];

        //        if (!string.IsNullOrEmpty(EmailTo) && !string.IsNullOrEmpty(EmailToName) && !string.IsNullOrEmpty(EmailSubject))
        //        {
        //            List<string> toemail = new List<string>();
        //            List<string> toemailname = new List<string>();

        //            toemail.AddRange(EmailTo.Split(','));
        //            toemailname.AddRange(EmailToName.Split(','));

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(EmailSubject);
        //            emailOperator.SetBody(EmailBody.ToString());

        //            #region SendEmail
        //            emailOperator.SendEmail(true);
        //            #endregion
        //        }
        //    }
        //    return true;
        //}
        ////SendEmailforHRDelete
        //public string SendEmailForHRDeleteToInternalTeam(GenSalesHiringRequest _SalesHiringRequest, UsrUser usr)
        //{
        //    try
        //    {
        //        string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "", companyName = "", ClientName = "", ClientEmail = "",
        //            userName = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        if (_SalesHiringRequest != null)
        //        {
        //            var ContactId = _SalesHiringRequest.ContactId;
        //            GenContact contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ContactId).FirstOrDefault();

        //            if (contact != null)
        //            {
        //                ClientName = contact.FullName;
        //                ClientEmail = contact.EmailId;
        //                GenCompany company = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == contact.CompanyId).FirstOrDefault();

        //                if (company != null)
        //                {
        //                    companyName = company.Company;
        //                }
        //            }

        //            if (usr != null)
        //            {
        //                userName = usr.FullName;
        //            }

        //            var salesPersonDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == _SalesHiringRequest.SalesUserId).FirstOrDefault();

        //            SalesPersonName = salesPersonDetails.FullName;
        //            salesPersonEmail = salesPersonDetails.EmailId;
        //            string role = _SalesHiringRequest.RequestForTalent;

        //            BodyCustom = "Hello Team,";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/><br/>");
        //            sbBody.Append("Greetings for the day!");
        //            sbBody.Append("<br/><br/>");

        //            Subject = $"{_SalesHiringRequest.HrNumber} Deleted";
        //            sbBody.Append($"This HR is deleted by {userName}.");

        //            sbBody.Append("&nbsp;Below are the required details:");
        //            sbBody.Append("<div style='width:100%'>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("HR number: " + _SalesHiringRequest.HrNumber);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Job Title: " + _SalesHiringRequest.RequestForTalent);
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"Client: {ClientName} ({ClientEmail})");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Company: " + companyName);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Thanks");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("</div>");

        //            emailDatabaseContentProvider.SalesUserId = Convert.ToInt64(_SalesHiringRequest.SalesUserId);
        //            string InternalCCEmailID = emailDatabaseContentProvider.GetCCEmailIdValues();
        //            string InternalCCEmailName = emailDatabaseContentProvider.GetCCEmailNameValues();

        //            string TOEMAIL = salesPersonEmail;
        //            string TOEMAILNAME = SalesPersonName;

        //            if (!string.IsNullOrEmpty(TOEMAIL) && !string.IsNullOrEmpty(InternalCCEmailID))
        //            {
        //                List<string> toemail = new() { TOEMAIL };
        //                List<string> toemailname = new() { TOEMAILNAME };

        //                #region Variable
        //                EmailOperator emailOperator = new EmailOperator(_configuration);
        //                #endregion

        //                emailOperator.SetToEmail(toemail);
        //                emailOperator.SetToEmailName(toemailname);
        //                emailOperator.SetCCEmail(InternalCCEmailID, InternalCCEmailName);
        //                emailOperator.SetSubject(Subject);
        //                emailOperator.SetBody(sbBody.ToString());

        //                #region SendEmail
        //                if (!string.IsNullOrEmpty(Subject))
        //                    emailOperator.SendEmail();
        //                #endregion

        //                return "Success";
        //            }
        //            else
        //                return "Error";
        //        }
        //        else
        //            return "error";
        //    }
        //    catch (Exception e) { return e.Message; }
        //}
        ////Send Email When Company Logo Scrap To Internal Team
        //public async Task<bool> SendEmailToInternalTeamWhenCompanyLogoScrap(string ClientName, string Company, bool IsLogoFound, string CompanyLogoName, string CompanyURL, long? POCUserID)
        //{
        //    try
        //    {
        //        EmailOperator emailOperator = new EmailOperator(_configuration);

        //        string subject = "";
        //        string bodyCustom = "";
        //        System.Text.StringBuilder sbBody = new System.Text.StringBuilder();

        //        subject = "Company Logo Scrapping Info";
        //        bodyCustom = $"Hello Team,";
        //        sbBody.Append(bodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Tried to Register using following Details and It's Logo scrapping Status is as Follow:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (!string.IsNullOrEmpty(ClientName))
        //        {
        //            sbBody.Append($"Client : {ClientName}");
        //            sbBody.Append("<br/>");
        //        }

        //        if (IsLogoFound)
        //        {
        //            sbBody.Append($"Company Logo Found.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Logo : <img align='center' src='" + _configuration["UTSAdminContactLogo"].ToString() + CompanyLogoName + "'/>");
        //        }
        //        else
        //        {
        //            sbBody.Append($"Company Logo Not Found.");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Company : {Company}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Company URL : {CompanyURL}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("For any queries or further assistance, our support team is here to help. Please get in touch with us on client@uplers.com");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Best regards,");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers");

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //        string TOEMAIL = "";
        //        string TOEMAILNAME = "";

        //        var UserData = _talentConnectAdminDBContext.UsrUsers.FirstOrDefault(x => x.Id == POCUserID);
        //        if (UserData != null)
        //        {
        //            TOEMAIL = UserData.EmailId;
        //            TOEMAILNAME = UserData.FullName;
        //        }

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        emailDatabaseContentProvider.SalesUserId = Convert.ToInt64(POCUserID);

        //        MakeCCDetail ccDetails = MakeCCEmailDetails(emailDatabaseContentProvider.SalesUserId, true);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }
        //        #endregion

        //        #region SetParam
        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(subject);
        //        emailOperator.SetBody(sbBody.ToString());
        //        #endregion

        //        if (!string.IsNullOrEmpty(subject))
        //            emailOperator.SendEmail(false, false);

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        ////Send email to the person while adding HR to demo account
        //public bool SendEmailWhileAddingHRToDemoAccount(string email, string name, List<string> includedHRs, List<string> excludedHrs)
        //{
        //    try
        //    {
        //        EmailOperator emailOperator = new EmailOperator(_configuration);

        //        string subject = "";
        //        string bodyCustom = "";
        //        System.Text.StringBuilder sbBody = new System.Text.StringBuilder();

        //        subject = "HR Clone Updates for Demo Account";
        //        bodyCustom = $"Hello {name},";
        //        sbBody.Append(bodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Below mentioned the details of the HR(s) which are added/excluded while cloning to the demo account.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("List of HR's added");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<strong>Note: This HR's are added in the demo account, we are finding best talents for the added roles, please check after sometime.</strong>");
        //        if (includedHRs.Count > 0)
        //        {
        //            sbBody.Append("<ul>");
        //            foreach (var i in includedHRs)
        //            {
        //                sbBody.Append("<li>");
        //                sbBody.Append(i);
        //                sbBody.Append("</li>");
        //            }
        //            sbBody.Append("</ul>");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append("No HR added.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //        }


        //        sbBody.Append("List of HR's excluded due to lack of appropriate In Interview Talent Profiles");
        //        if (excludedHrs.Count > 0)
        //        {
        //            sbBody.Append("<ul>");
        //            foreach (var i in excludedHrs)
        //            {
        //                sbBody.Append("<li>");
        //                sbBody.Append(i);
        //                sbBody.Append("</li>");
        //            }
        //            sbBody.Append("</ul>");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append("No HR excluded.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //        }


        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = email;
        //        string TOEMAILNAME = name;

        //        //string InternalCCEmailID = emailDatabaseContentProvider.GetCCEmailIdValues();
        //        //string InternalCCEmailName = emailDatabaseContentProvider.GetCCEmailNameValues();

        //        string CCEMAIL = "mehul@uplers.com,bhuvan@uplers.com,payal.p@uplers.in,riya.a@uplers.in,mohitkumar.m@uplers.in";
        //        string CCEMAILNAME = "Mehul,Bhuvan,Payal,Riya,Mohit";

        //        //string SMEMAIL = "";
        //        //string SMEMAILNAME = "";

        //        //CCEMAIL = ccEmail + InternalCCEmailID;
        //        //CCEMAILNAME = ccEmailName + InternalCCEmailName;

        //        //SMEMAIL = EmailDatabaseContentProvider.GetSMCCEmailIdValues();
        //        //SMEMAILNAME = EmailDatabaseContentProvider.GetSMCCEmailNameValues();

        //        //CCEMAIL = CCEMAIL + SMEMAIL;
        //        //CCEMAILNAME = CCEMAILNAME + SMEMAILNAME;

        //        List<string> TOEMAIL_Client = new List<string>() { TOEMAIL };
        //        List<string> TOEMAILNAME_Client = new List<string>() { TOEMAILNAME };

        //        #region SetParam
        //        emailOperator.SetToEmail(TOEMAIL_Client);
        //        emailOperator.SetToEmailName(TOEMAILNAME_Client);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(subject);
        //        emailOperator.SetBody(sbBody.ToString());
        //        #endregion

        //        if (!string.IsNullOrEmpty(subject))
        //            emailOperator.SendEmail(false, false, true);

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        //#region SendEmail when Talent is rejected.

        //public bool SendEmailToInternalTeamWhenTalentIsRejected(long talentId, int rejectedReasonId, long hrId, string comments = "",
        //                                                        string rejectionMsgToTalents = "", string rejectedBy = "")
        //{
        //    try
        //    {
        //        string? ClientName = "", ClientEmail = "", CompanyName = "",
        //                hrNumber = "", talentName = "", rejectedReason = "",
        //                talentDesignation = "", talentYearsOfExp = "", talentEmail = "";

        //        long SalesUserid = 0;

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //        #endregion

        //        #region GetDBData
        //        emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(talentId, 0, 0, false, hrId, 0, 0);
        //        #endregion

        //        CompanyName = emailForBookTimeSlotModel.CompanyName;
        //        ClientName = emailForBookTimeSlotModel.clientName;
        //        ClientEmail = emailForBookTimeSlotModel.ClientEmail;
        //        hrNumber = emailForBookTimeSlotModel.HR_Number;
        //        SalesUserid = emailForBookTimeSlotModel.HRSalesPersonID;

        //        talentName = emailForBookTimeSlotModel.TalentName;
        //        talentDesignation = emailForBookTimeSlotModel.Designation;
        //        talentYearsOfExp = emailForBookTimeSlotModel.yearsofExperience.ToString();
        //        talentEmail = emailForBookTimeSlotModel.TalentEmail;

        //        PrgTalentRejectReason reason = emailDatabaseContentProvider.GetPrgTalentRejectReason(rejectedReasonId);
        //        if (reason != null)
        //        {
        //            if (!string.IsNullOrEmpty(reason.ParentName))
        //            {
        //                rejectedReason = string.Format("{0} - {1}", reason.ParentName, reason.Reason);
        //            }
        //            else
        //            {
        //                rejectedReason = reason.Reason;
        //            }
        //        }

        //        string subject = "";
        //        string bodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();


        //        subject = $"Talent Rejected from Sales portal by {rejectedBy}";
        //        bodyCustom = $"Hello Team,";
        //        sbBody.Append(bodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"The Talent <strong> {talentName} </strong> is rejected from Sales portal by {rejectedBy}.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");

        //        sbBody.Append("<strong>Talent Details:</strong>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Name : {talentName}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Email : {talentEmail}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Profile : {talentDesignation}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Years of Experience : {talentYearsOfExp}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<strong>Other Details:</strong>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Company : {CompanyName}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Client : {ClientName} ({ClientEmail})");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"HR Number : {hrNumber}");
        //        sbBody.Append("<br/>");
        //        if (!string.IsNullOrEmpty(rejectedReason))
        //        {
        //            sbBody.Append($"Rejection Reason : {rejectedReason}");
        //            sbBody.Append("<br/>");
        //        }
        //        if (!string.IsNullOrEmpty(rejectionMsgToTalents))
        //        {
        //            sbBody.Append($"Rejection message for the candidate: {rejectionMsgToTalents}");
        //            sbBody.Append("<br/>");
        //        }
        //        if (!string.IsNullOrEmpty(comments))
        //        {
        //            sbBody.Append($"Comments : {comments}");
        //            sbBody.Append("<br/>");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");

        //        #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //        string TOEMAIL = emailForBookTimeSlotModel.salesemailid;
        //        string TOEMAILNAME = emailForBookTimeSlotModel.salesName;

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        string CCEMAIL = "";
        //        string CCEMAILNAME = "";

        //        MakeCCDetail ccDetails = MakeCCEmailDetails(emailForBookTimeSlotModel.HRSalesPersonID, true, true, hrId, talentId);
        //        if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //        }

        //        #endregion

        //        #region SetParam
        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //        emailOperator.SetSubject(subject);
        //        emailOperator.SetBody(sbBody.ToString());
        //        #endregion

        //        if (!string.IsNullOrEmpty(subject))
        //            emailOperator.SendEmail(false, false);

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //#endregion


        //#region Schedualr Emails
        //public void SendEmailOnNewCandidatesAdded_PayPerView(long HRID, string RequestForTalent, string ClientName, string EmailId, int VettedCount, long SalesUserID, int AITalentCount, int NoOfTalent,long PocID,string PocName,string PocEmail,string PocDesignation,string PocPhone)
        //{
        //    string Subject = "", InternalToEmail = "", InternalToEmailName = "";
        //    StringBuilder sbBody = new StringBuilder();
        //    int AIVettedTalentCounts = VettedCount;
        //    var reactClientPortalURL = _configuration["ReactClientPortalURL"];
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    int count = 1;
        //    if (NoOfTalent > 1)
        //    {
        //        Subject = $"New Candidates Added For Your {RequestForTalent} Role!";
        //    }
        //    else
        //    {
        //        Subject = $"New Candidate Added For Your {RequestForTalent} Role!";
        //    }
        //    sbBody.Append($"Hello {ClientName},");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    if (NoOfTalent > 1)
        //    {
        //        sbBody.Append($"Great news! New Profiles have been added to your job of {RequestForTalent} role.");
        //    }
        //    else
        //    {
        //        sbBody.Append($"Great news! New Profile has been added to your job of {RequestForTalent} role.");
        //    }
        //    object[] param = new object[] { HRID };
        //    string paramString = CommonLogic.ConvertToParamStringWithNull(param);

        //    var candidateList = _talentConnectAdminDBContext.Set<Sproc_Get_Candidate_Details_For_Job_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Candidate_Details_For_Job, paramString)).AsEnumerable().ToList();

        //    foreach (var j in candidateList)
        //    {
        //        if (count <= 3)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"<b>Candidate {count}:</b>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"{j.TalentName}");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"{j.Designation}");
        //            sbBody.Append("<br/>");
        //            if (!string.IsNullOrEmpty(j.AISummary))
        //            {
        //                sbBody.Append($"{j.AISummary}");
        //                sbBody.Append("<br/>");
        //            }
        //            if (j.IsVideoResume.ToLower() == "yes")
        //            {
        //                sbBody.Append($"Video Resume Available");
        //                sbBody.Append("<br/>");
        //            }
        //            if (!string.IsNullOrEmpty(j.VideoVetting))
        //            {
        //                sbBody.Append($"AI Video Vetting Available");
        //                sbBody.Append("<br/>");
        //            }
        //            if (!string.IsNullOrEmpty(j.TalentResume))
        //            {
        //                sbBody.Append($"<a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:30px;text-decoration:none;' href='{reactClientPortalURL}creditbase-shortlist?HRID={HRID}'>Review {j.TalentName}'s CV</a>");
        //                sbBody.Append("<br/>");
        //            }
        //        }
        //        count += 1;

        //    }
        //    if (NoOfTalent > 3)
        //    {
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("There are a few more candidates added, review them all.");
        //    }
        //    if (NoOfTalent > 1)
        //    {
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"<a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' href='{reactClientPortalURL}creditbase-shortlist?HRID={HRID}'>Review Candidates</a>");
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    //sbBody.Append("To review these candidates:");
        //    //sbBody.Append("<br/>");
        //    //sbBody.Append("<ul>");
        //    //sbBody.Append("<li>Log into your Uplers account.</li>");
        //    //sbBody.Append("<li>Navigate to your job posting.</li>");
        //    //sbBody.Append("<li>Review the candidates' shares.</li>");
        //    //sbBody.Append("<li>Click on the profiles to view their information.</li>");
        //    //if (AITalentCount == 1)
        //    //{
        //    //    sbBody.Append("<li>Leverage our AI screening reports of candidates for the best match.</li>");
        //    //}
        //    //sbBody.Append("</ul>");
        //    //sbBody.Append("<br/>");
        //    //sbBody.Append("<br/>");
        //    sbBody.Append("Our intelligent AI algorithms have expertly matched these talents to your position's requirements; our support team is here to help if you have any questions or need assistance.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    if (PocID > 0)
        //    {
        //        sbBody.Append($"If you need help or have questions, please reach out to our executive {PocName} at {PocEmail}.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //    }
        //    else
        //    {
        //        sbBody.Append("Need assistance or have questions? We're just an email away.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //    }
        //    sbBody.Append("We look forward to supporting your hiring journey!");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Thank you for choosing Uplers for your recruiting needs.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Warm regards,");
        //    sbBody.Append("<br/>");
        //    if (PocID > 0)
        //    {
        //        sbBody.Append($"{PocName}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"{PocDesignation}");
        //        sbBody.Append("<br/>");
        //        if (!string.IsNullOrEmpty(PocPhone))
        //        {
        //            sbBody.Append($"{PocPhone}");
        //            sbBody.Append("<br/>");
        //        }
        //        sbBody.Append("Website: https://www.uplers.com/");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"{PocEmail}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //    }
        //    sbBody.Append("Uplers");


        //    string TOEMAIL = EmailId;
        //    string TOEMAILNAME = ClientName;

        //    string CCEMAIL = ccEmail;
        //    string CCEMAILNAME = ccEmailName;

        //    if (SalesUserID > 0)
        //    {

        //        var varGetUser = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == SalesUserID).FirstOrDefault();
        //        if (varGetUser != null)
        //        {
        //            InternalToEmail = varGetUser.EmailId;
        //            InternalToEmailName = varGetUser.FullName;
        //        }

        //        var POCUserHierarchy = GetHierarchyForEmail(SalesUserID.ToString());
        //        if (POCUserHierarchy != null && POCUserHierarchy.Any())
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? "" : ",";
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? "" : ",";

        //            CCEMAIL += string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.EmailId)).Select(x => x.EmailId));
        //            CCEMAILNAME += string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Select(x => x.UserName));
        //        }
        //    }

        //    List<string> toEmail = new List<string>
        //        {
        //            EmailId
        //        };

        //    List<string> toEmailName = new List<string>
        //        {
        //            ClientName
        //        };

        //    if (CCEMAIL != "" && TOEMAIL != "")
        //    {
        //        try
        //        {
        //            #region SetParam for client Email
        //            //emailOperator.SetToEmail(toEmail);
        //            //emailOperator.SetToEmailName(toEmailName);
        //            //if (!string.IsNullOrEmpty(CCEMAIL) && !string.IsNullOrEmpty(CCEMAILNAME))
        //            //    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);

        //            //emailOperator.SetSubject(Subject);
        //            //emailOperator.SetBody(sbBody.ToString());
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmailAWSSES(toEmail, Subject, sbBody.ToString());
        //            #endregion


        //            #region SetParam for Internal Team
        //            StringBuilder sbBodyCopiedText = new();
        //            sbBodyCopiedText.Append(_configuration["InternalEmailsAppendText"]);

        //            List<string> InternaltoEmail = new List<string>
        //            {
        //               InternalToEmail
        //            };

        //            List<string> InternaltoEmailName = new List<string>
        //            {
        //                InternalToEmailName
        //            };
        //            emailOperator.SetToEmail(InternaltoEmail);
        //            emailOperator.SetToEmailName(InternaltoEmailName);

        //            if (!string.IsNullOrEmpty(CCEMAIL) && !string.IsNullOrEmpty(CCEMAILNAME))
        //                emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);

        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBodyCopiedText.ToString() + " " + sbBody.ToString());

        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail(false, false, true);
        //            #endregion

        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //public void SendEmailJobExpiryOnsomedays(long pocID, string PocName, string PocEmail, string PocDesignation, string PocPhone, string ClientName, string EmailId, Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal_Result clientsWelcomeEmailViewModel, int pendingdays, int VettedCount)
        //{
        //    string Subject = "";
        //    StringBuilder sbBody = new StringBuilder();
        //    long ContactID = clientsWelcomeEmailViewModel.ContactID;
        //    int JobPostCount = clientsWelcomeEmailViewModel.JobPostCount;
        //    int JobPostCountPayPerHire = clientsWelcomeEmailViewModel.JobPostCountpayperHire;
        //    int ProfileSharedCount = clientsWelcomeEmailViewModel.ProfileSharedCount;
        //    int CurrentCreditBalance = clientsWelcomeEmailViewModel.CreditBalance;
        //    int UsedCredit = clientsWelcomeEmailViewModel.UsedCredit;
        //    string HRNumber = clientsWelcomeEmailViewModel.HR_Number;
        //    int AIVettedTalentCounts = VettedCount;
        //    string RequestForTalent = clientsWelcomeEmailViewModel.RequestForTalent;
        //    long HRID = clientsWelcomeEmailViewModel.HRID;
        //    var reactClientPortalURL = _configuration["ReactClientPortalURL"];
        //    EmailOperator emailOperator = new EmailOperator(_configuration);



        //    if (pendingdays == 5)
        //    {
        //        Subject = $"Stay Ahead: Your Job Post for {RequestForTalent} Expires Soon - Take Action Now!";
        //        sbBody.Append($"Hello {ClientName},");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hope you're doing great! We noticed your job post is nearing its expiry date. To continue attracting top talent without interruption, consider reposting now to refresh your presence and remain visible to potential candidates");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<ul>");
        //        sbBody.Append($"<li>Job Title: {RequestForTalent}</li>");
        //        sbBody.Append($"<li>Job ID: {HRNumber}</li>");
        //        if (ProfileSharedCount > 0)
        //        {
        //            sbBody.Append($"<li>Number of active profiles: {ProfileSharedCount}</li>");
        //        }
        //        sbBody.Append("</ul>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"<a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' href='{reactClientPortalURL}creditbase-shortlist?HRID={HRID}'>Repost Job</a>"); sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Here's a quick glance at your account:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Credit Balance");
        //        sbBody.Append("<ul>");
        //        sbBody.Append($"<li>Current Credits: {CurrentCreditBalance}</li>");
        //        sbBody.Append($"<li>Used Credits: {UsedCredit}</li>");
        //        sbBody.Append($"<li>Number of Active Job Posts: {JobPostCount}</li>");
        //        sbBody.Append("</ul>");
        //        if (JobPostCountPayPerHire > 0)
        //        {
        //            sbBody.Append($"Beyond this, there are {JobPostCountPayPerHire} Jobs where Uplers would help you on a pay per hire model.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //        }
        //        if (CurrentCreditBalance > 0)
        //            sbBody.Append("You can still use your free credits to attract amazing profiles.");
        //        else
        //        {
        //            sbBody.Append("Keep your recruitment drive going strong by recharging your credits today.");
        //            sbBody.Append("<br/>"); sbBody.Append("<br/>");
        //            sbBody.Append($"<a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' href='{reactClientPortalURL + "subscriptionPlan"}'>Recharge Your Credits</a>");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Need assistance or have questions? We're just an email away.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Your dedicated POC {PocEmail} is just a click away if you have questions or require further assistance. We will make sure your experience with Uplers is exceptional.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for choosing Uplers for your recruiting needs. ");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Warm regards,");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers");
        //        sbBody.Append("<br/>");
        //        if (pocID > 0)
        //        {
        //            sbBody.Append($"{PocName}");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"{PocDesignation}");
        //            sbBody.Append("<br/>");
        //            if (!string.IsNullOrEmpty(PocPhone))
        //            {
        //                sbBody.Append($"{PocPhone}");
        //                sbBody.Append("<br/>");
        //            }
        //            sbBody.Append("Website: https://www.uplers.com/");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"{PocEmail}");
        //        }


        //    }
        //    if (pendingdays == 3)
        //    {
        //        Subject = $"Important: Your Job Post for {RequestForTalent} Expires in 3 Days - Immediate Action Required!";
        //        sbBody.Append($"Hello {ClientName},");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (ProfileSharedCount > 0)
        //            sbBody.Append("Here is a friendly reminder for you. Your job post expires in 3 days. Repost now to maintain visibility and attract top talent exploring opportunities. Keep your post active to capture the interest of the best candidates.");
        //        else
        //            sbBody.Append("Here is a friendly reminder for you. Your job post expires in 3 days.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<ul>");
        //        sbBody.Append($"<li>Job Title: {RequestForTalent}</li>");
        //        sbBody.Append($"<li>Job ID: {HRNumber}</li>");
        //        if (ProfileSharedCount > 0)
        //        {
        //            sbBody.Append($"<li>Number of active profiles: {ProfileSharedCount}</li>");
        //        }
        //        sbBody.Append("</ul>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"<a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' href='{reactClientPortalURL}creditbase-shortlist?HRID={HRID}'>Repost Job</a>"); sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Here's a quick glance at your account:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Credit Balance");
        //        sbBody.Append("<ul>");
        //        sbBody.Append($"<li>Current Credits: {CurrentCreditBalance}</li>");
        //        sbBody.Append($"<li>Used Credits: {UsedCredit}</li>");
        //        sbBody.Append($"<li>Number of Active Job Posts: {JobPostCount}</li>");
        //        sbBody.Append("</ul>");
        //        if (JobPostCountPayPerHire > 0)
        //        {
        //            sbBody.Append($"Beyond this, there are {JobPostCountPayPerHire} Jobs where Uplers would help you on a pay per hire model.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //        }
        //        if (CurrentCreditBalance > 0)
        //            sbBody.Append("You can still use your free credits to attract amazing profiles.");
        //        else
        //        {
        //            sbBody.Append("Keep your recruitment drive going strong by recharging your credits today.");
        //            sbBody.Append("<br/>"); sbBody.Append("<br/>");
        //            sbBody.Append($"<a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' href='{reactClientPortalURL + "subscriptionPlan"}'>Recharge Your Credits</a>");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Need assistance or have questions? We're just an email away.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Your dedicated POC {PocEmail} is just a click away if you have questions or require further assistance. We will make sure your experience with Uplers is exceptional.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for choosing Uplers for your recruiting needs. ");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Warm regards,");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers");
        //        sbBody.Append("<br/>");
        //        if (pocID > 0)
        //        {
        //            sbBody.Append($"{PocName}");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"{PocDesignation}");
        //            sbBody.Append("<br/>");
        //            if (!string.IsNullOrEmpty(PocPhone))
        //            {
        //                sbBody.Append($"{PocPhone}");
        //                sbBody.Append("<br/>");
        //            }
        //            sbBody.Append("Website: https://www.uplers.com/");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"{PocEmail}");
        //        }
        //    }
        //    if (pendingdays == 1)
        //    {
        //        Subject = $"Tomorrow is the last day for {RequestForTalent} - Repost Job now!";
        //        sbBody.Append($"Hello {ClientName},");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Time is ticking, and this is your last call: your job post will expire tomorrow. Repost your job today to ensure it stays visible and accessible to talented candidates.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<ul>");
        //        sbBody.Append($"<li>Job Title: {RequestForTalent}</li>");
        //        sbBody.Append($"<li>Job ID: {HRNumber}</li>");
        //        sbBody.Append("</ul>");
        //        sbBody.Append($"<a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' href='{reactClientPortalURL}creditbase-shortlist?HRID={HRID}'>Repost Job</a>"); sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Here's a quick glance at your account:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<ul>");
        //        sbBody.Append($"<li>Current Credits: {CurrentCreditBalance}</li>");
        //        sbBody.Append($"<li>Used Credits: {UsedCredit}</li>");
        //        sbBody.Append($"<li>Number of Active Job Posts: {JobPostCount}</li>");
        //        sbBody.Append("</ul>");
        //        if (JobPostCountPayPerHire > 0)
        //        {
        //            sbBody.Append($"Beyond this, there are {JobPostCountPayPerHire} Jobs where Uplers would help you on a pay per hire model.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //        }
        //        if (CurrentCreditBalance > 0)
        //            sbBody.Append("You can still use your free credits to attract amazing profiles.");
        //        else
        //        {
        //            sbBody.Append("To keep your recruitment drive uninterrupted, it is advised that you recharge your credit soon.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"<a style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' href='{reactClientPortalURL + "subscriptionPlan"}'>Recharge Your Credits</a>");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Need assistance or have questions? We're just an email away.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Your dedicated POC {PocEmail} is just a click away if you have questions or require further assistance. We will make sure your experience with Uplers is exceptional.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for choosing Uplers for your recruiting needs. ");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Warm regards,");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers");
        //        sbBody.Append("<br/>");
        //        if (pocID > 0)
        //        {
        //            sbBody.Append($"{PocName}");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"{PocDesignation}");
        //            sbBody.Append("<br/>");
        //            if (!string.IsNullOrEmpty(PocPhone))
        //            {
        //                sbBody.Append($"{PocPhone}");
        //                sbBody.Append("<br/>");
        //            }
        //            sbBody.Append("Website: https://www.uplers.com/");
        //            sbBody.Append("<br/>");
        //            sbBody.Append($"{PocEmail}");
        //        }
        //    }

        //    string TOEMAIL = EmailId;
        //    string TOEMAILNAME = ClientName;

        //    string CCEMAIL = ccEmail;
        //    string CCEMAILNAME = ccEmailName;

        //    if (pocID > 0)
        //    {
        //        var POCUserHierarchy = GetHierarchyForEmail(pocID.ToString());
        //        if (POCUserHierarchy != null && POCUserHierarchy.Any())
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? "" : ",";
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? "" : ",";

        //            CCEMAIL += string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.EmailId)).Select(x => x.EmailId));
        //            CCEMAILNAME += string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Select(x => x.UserName));
        //        }
        //    }

        //    List<string> toEmail = new List<string>
        //        {
        //            EmailId
        //        };

        //    List<string> toEmailName = new List<string>
        //        {
        //            ClientName
        //        };

        //    if (CCEMAIL != "" && TOEMAIL != "")
        //    {
        //        try
        //        {
        //            #region SetParam
        //            //emailOperator.SetToEmail(toEmail);
        //            //if (!string.IsNullOrEmpty(CCEMAIL) && !string.IsNullOrEmpty(CCEMAILNAME))
        //            //    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            //emailOperator.SetToEmailName(toEmailName);
        //            //emailOperator.SetSubject(Subject);
        //            //emailOperator.SetBody(sbBody.ToString());
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmailAWSSES(toEmail, Subject, sbBody.ToString());

        //            #endregion

        //            #region SetParam for Internal Team
        //            StringBuilder sbBodyCopiedText = new();
        //            sbBodyCopiedText.Append(_configuration["InternalEmailsAppendText"]);

        //            List<string> InternaltoEmail = new List<string>
        //            {
        //               PocEmail
        //            };

        //            List<string> InternaltoEmailName = new List<string>
        //            {
        //                PocName
        //            };
        //            emailOperator.SetToEmail(InternaltoEmail);
        //            emailOperator.SetToEmailName(InternaltoEmailName);

        //            if (!string.IsNullOrEmpty(CCEMAIL) && !string.IsNullOrEmpty(CCEMAILNAME))
        //                emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);

        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBodyCopiedText.ToString() + " " + sbBody.ToString());

        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail(false, false, true);
        //            #endregion


        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //public string SendEmailForNotesAddedToClient(List<Sproc_Fetch_TalentNotesEmailsLog_HRWise_Result> notesViewModel, Sproc_Fetch_TalentNotesEmailsLog_Result result)
        //{
        //    try
        //    {
        //        string? Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "",
        //            CompanyName = result.Company,
        //            ClientName = result.FullName, hrNumber = result.HR_Number,
        //            ClientEmail = result.EmailID,
        //            POCContactNo = "", invitingIserEmailId = "", invitingUserName = "", Designation = "";


        //        StringBuilder sbBody = new StringBuilder();

        //        long PocID = 0;
        //        long? clientID = result.ContactID;
        //        long hrId = Convert.ToInt64(result.HRID);

        //        #region GetPOCDetails
        //        object[] param = new object[] { clientID };

        //        string paramString = CommonLogic.ConvertToParamStringWithNull(param);

        //        var obj = sproc_Get_ContactPointofContact(paramString); //_talentConnectAdminDBContext.Set<sproc_Get_ContactPointofContact_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Get_ContactPointofContact, paramString)).AsEnumerable().FirstOrDefault();
        //        if (obj != null)
        //        {
        //            PocID = obj.User_ID ?? 0;
        //            if (PocID > 0)
        //            {
        //                var usr_user = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == PocID).FirstOrDefault();
        //                if (usr_user != null)
        //                {
        //                    POCContactNo = usr_user.ContactNumber;
        //                    invitingIserEmailId = usr_user.EmailId;
        //                    invitingUserName = usr_user.FullName;
        //                    Designation = usr_user.Designation;
        //                }
        //            }
        //        }
        //        #endregion

        //        GenSalesHiringRequest? _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == hrId).FirstOrDefault();

        //        if (_SalesHiringRequest != null)
        //        {
        //            var salesPersonDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == _SalesHiringRequest.SalesUserId).FirstOrDefault();

        //            SalesPersonName = salesPersonDetails.FullName;
        //            salesPersonEmail = salesPersonDetails.EmailId;
        //            string role = _SalesHiringRequest.RequestForTalent;

        //            BodyCustom = $"Dear {ClientName},";
        //            Subject = $"Notes for {hrNumber} - {_SalesHiringRequest.RequestForTalent}";

        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/><br/>");
        //            sbBody.Append($"Below are the notes exchanged for the {_SalesHiringRequest.RequestForTalent} position at {CompanyName} (Job ID: {hrNumber}):");

        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");

        //            string CCEMAIL = "";
        //            string CCEMAILNAME = "";

        //            foreach (var i in notesViewModel)
        //            {

        //                sbBody.Append($"<strong>Talent: {i.TalentName}</strong>");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("<br/>");

        //                param = new object[] { hrId, i.ATSTalentID };
        //                paramString = CommonLogic.ConvertToParamStringWithNull(param);

        //                var talentNotes = _talentConnectAdminDBContext.Set<Sproc_Fetch_TalentNotesEmailsLog_HRWise_TalentWise_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Fetch_TalentNotesEmailsLog_HRWise_TalentWise, paramString)).AsEnumerable().ToList();

        //                foreach (var j in talentNotes)
        //                {
        //                    if (!string.IsNullOrEmpty(j.Note))
        //                    {
        //                        sbBody.Append($"Content: {j.Note} - from {j.NotesAddedby}");
        //                        sbBody.Append("<br/>");
        //                        sbBody.Append("<br/>");
        //                    }

        //                    param = new object[] { Convert.ToString(j.ATSNoteID) };
        //                    paramString = CommonLogic.ConvertToParamStringWithNull(param);

        //                    _talentConnectAdminDBContext.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_AddUpdate_TalentNotesEmailsLog, paramString));
        //                }

        //                MakeCCDetail ccDetailsTalents = MakeCCEmailDetails(0, false, true, hrId, i.UTSTalentID.Value, false);
        //                if (ccDetailsTalents != null && !string.IsNullOrEmpty(ccDetailsTalents.CCEmail) && !string.IsNullOrEmpty(ccDetailsTalents.CCEmailName))
        //                {
        //                    CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetailsTalents.CCEmail : "," + ccDetailsTalents.CCEmail;
        //                    CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetailsTalents.CCEmailName : "," + ccDetailsTalents.CCEmailName;
        //                }
        //            }

        //            sbBody.Append("If you have any additional feedback or would like to discuss these notes further, please feel free to reach out.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Best Regards");

        //            if (PocID != 0)
        //            {
        //                sbBody.Append("<br/>");
        //                sbBody.Append($"{invitingUserName}");
        //                sbBody.Append("<br/>");
        //                sbBody.Append($"{Designation}");
        //                sbBody.Append("<br/>");
        //                if (!string.IsNullOrEmpty(POCContactNo))
        //                {
        //                    sbBody.Append($"{POCContactNo}");
        //                    sbBody.Append("<br/>");
        //                }
        //                sbBody.Append("Website: https://www.uplers.com/");
        //                sbBody.Append("<br/>");
        //                sbBody.Append($"{invitingIserEmailId}");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("<hr>");
        //            }
        //            else
        //            {
        //                sbBody.Append("<br/>");
        //                sbBody.Append("Uplers Product Engineering Team");
        //            }

        //            #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //            List<string> toemail = new List<string>() { ClientEmail };
        //            List<string> toemailname = new List<string>() { ClientName };

        //            MakeCCDetail ccDetails = MakeCCEmailDetails(_SalesHiringRequest.SalesUserId.Value, true, false, hrId, 0);
        //            if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //            {
        //                CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //            }

        //            #endregion

        //            EmailOperator emailOperator = new EmailOperator(_configuration);

        //            #region SetParam for client email
        //            //emailOperator.SetToEmail(toemail);
        //            //emailOperator.SetToEmailName(toemailname);
        //            //emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            //emailOperator.SetSubject(Subject);
        //            //emailOperator.SetBody(sbBody.ToString());
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmailAWSSES(toemail, Subject, sbBody.ToString());

        //            #endregion

        //            #region SetParam for Internal Team
        //            StringBuilder sbBodyCopiedText = new();
        //            sbBodyCopiedText.Append(_configuration["InternalEmailsAppendText"]);


        //            List<string> InternaltoEmail = new List<string>
        //            {
        //               invitingIserEmailId
        //            };

        //            List<string> InternaltoEmailName = new List<string>
        //            {
        //                invitingUserName
        //            };
        //            emailOperator.SetToEmail(InternaltoEmail);
        //            emailOperator.SetToEmailName(InternaltoEmailName);

        //            if (!string.IsNullOrEmpty(CCEMAIL) && !string.IsNullOrEmpty(CCEMAILNAME))
        //                emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);

        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBodyCopiedText.ToString() + " " + sbBody.ToString());

        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail(false, false, true);
        //            #endregion


        //            return "success";

        //        }
        //        else
        //            return "error";
        //    }
        //    catch (Exception e) { return e.Message; }
        //}

        //#region Send Engagement Renewal Emails
        //public async Task<string> SendEmailForEngagementRenewalAsync(Sproc_Get_Engagement_Renewal_Emails_Details_Result result)
        //{
        //    try
        //    {
        //        EmailOperator emailOperator = new EmailOperator(_configuration);

        //        StringBuilder sbBodyCopiedText = new();
        //        sbBodyCopiedText.Append(_configuration["InternalEmailsAppendText"]);


        //        List<string> toEmails = new List<string>();
        //        List<string> toEmailNames = new List<string>();

        //        toEmails.AddRange(result.ToEmailIds.Split(','));
        //        toEmailNames.AddRange(result.ToEmailNames.Split(','));

        //        emailOperator.SetToEmail(toEmails);
        //        emailOperator.SetToEmailName(toEmailNames);
        //        emailOperator.SetCCEmail(result.CCEmailIds, result.CCEmailNames);
        //        emailOperator.SetSubject(result.subject);
        //        emailOperator.SetBody(sbBodyCopiedText.ToString() + " " + result.Body.ToString());

        //        if (!string.IsNullOrEmpty(result.subject))
        //            emailOperator.SendEmailClientPortal(false, true);


        //        return "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
        //#endregion

        //#region SendEmailToInternalTeamWhenClientAddNotes 
        //public string SendEmailForNotesAddedToInternalTeam(HRTalentNotesViewModel notesViewModel, long loggedInUserId)
        //{
        //    try
        //    {
        //        string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "",
        //            CompanyName = "",
        //            ClientName = "", hrNumber = "",
        //            ClientEmail = "", talentName = "", rejectedReason = "", talentDesignation = "", talentYearsOfExp = "", talentEmail = "",
        //            POCContactNo = "", invitingIserEmailId = "", invitingUserName = "", Designation = "", notesAddedBy = "";

        //        string noteAction = "", noteActionSubject = "", noteActionText = "";


        //        if (notesViewModel.Flag == "Add")
        //        {
        //            noteAction = noteActionSubject = "Added";
        //            noteActionText = "Client's";
        //        }
        //        else if (notesViewModel.Flag == "Edit")
        //        {
        //            noteAction = noteActionText = "Updated";
        //            noteActionSubject = "Edited";
        //        }
        //        else if (notesViewModel.Flag == "Delete")
        //        {
        //            noteAction = noteActionText = noteActionSubject = "Deleted";
        //        }

        //        StringBuilder sbBody = new StringBuilder();
        //        bool IsAdHoc, IsManaged = false;

        //        long hrId = Convert.ToInt64(notesViewModel.HRID);

        //        long clientID = 0;


        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //        #endregion

        //        long talentId = Convert.ToInt64(notesViewModel.UTSTalentID);
        //        long PocID = 0;

        //        #region GetPOCDetails
        //        object[] param = new object[]
        //            {
        //                clientID
        //            };

        //        string paramString = CommonLogic.ConvertToParamStringWithNull(param);

        //        var obj = sproc_Get_ContactPointofContact(paramString); //_talentConnectAdminDBContext.Set<sproc_Get_ContactPointofContact_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Get_ContactPointofContact, paramString)).AsEnumerable().FirstOrDefault();
        //        if (obj != null)
        //        {
        //            PocID = obj.User_ID ?? 0;
        //            if (PocID > 0)
        //            {
        //                var usr_user = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == PocID).FirstOrDefault();
        //                if (usr_user != null)
        //                {
        //                    POCContactNo = usr_user.ContactNumber;
        //                    invitingIserEmailId = usr_user.EmailId;
        //                    invitingUserName = usr_user.FullName;
        //                    Designation = usr_user.Designation;
        //                }
        //            }
        //        }
        //        #endregion

        //        #region GetDBData
        //        emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(talentId, 0, 0, false, hrId, 0, 0);
        //        #endregion

        //        if (emailForBookTimeSlotModel != null)
        //        {
        //            CompanyName = emailForBookTimeSlotModel.CompanyName;
        //            ClientName = emailForBookTimeSlotModel.clientName;
        //            ClientEmail = emailForBookTimeSlotModel.ClientEmail;
        //            hrNumber = emailForBookTimeSlotModel.HR_Number;

        //            talentName = emailForBookTimeSlotModel.TalentName;
        //            talentDesignation = emailForBookTimeSlotModel.Designation;
        //            talentYearsOfExp = emailForBookTimeSlotModel.yearsofExperience.ToString();
        //            talentEmail = emailForBookTimeSlotModel.TalentEmail;
        //        }

        //        #region GetLoggedinUserDetails
        //        var loggedInUserDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == loggedInUserId).FirstOrDefault();
        //        if (loggedInUserDetails != null)
        //        {
        //            notesAddedBy = loggedInUserDetails.FullName;
        //        }
        //        #endregion

        //        GenSalesHiringRequest? _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == hrId).FirstOrDefault();

        //        if (_SalesHiringRequest != null)
        //        {
        //            var salesPersonDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == _SalesHiringRequest.SalesUserId).FirstOrDefault();

        //            SalesPersonName = salesPersonDetails.FullName;
        //            salesPersonEmail = salesPersonDetails.EmailId;
        //            string role = _SalesHiringRequest.RequestForTalent;
        //            clientID = Convert.ToInt64(_SalesHiringRequest.ContactId);
        //            hrNumber = _SalesHiringRequest.HrNumber;


        //            BodyCustom = $"Dear {ClientName},";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/><br/>");

        //            if (noteAction == "Added")
        //            {
        //                sbBody.Append($"Below are the notes exchanged for the {_SalesHiringRequest.RequestForTalent} position at {CompanyName} (Job ID: {hrNumber}):");
        //                Subject = $"Notes for {hrNumber} - {_SalesHiringRequest.RequestForTalent}";

        //                if (!string.IsNullOrEmpty(notesViewModel.Notes))
        //                {
        //                    sbBody.Append($"Content: {notesViewModel.Notes} - from {notesAddedBy}");

        //                }
        //            }
        //            else
        //            {
        //                sbBody.Append($"A note regarding talent {talentName} for Job ID {hrNumber} has been updated by {notesAddedBy}.");
        //                Subject = $"Notes Updated for {hrNumber} - {talentName}";

        //                if (!string.IsNullOrEmpty(notesViewModel.Notes))
        //                {
        //                    sbBody.Append($"Updated Note: {notesViewModel.Notes}");

        //                }
        //            }
        //            sbBody.Append("<br/><br/>");

        //            sbBody.Append("If you have any additional feedback or would like to discuss these notes further, please feel free to reach out.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Best Regards");
        //            if (PocID != 0)
        //            {
        //                sbBody.Append($"{invitingUserName}");
        //                sbBody.Append("<br/>");
        //                sbBody.Append($"{Designation}");
        //                sbBody.Append("<br/>");
        //                if (!string.IsNullOrEmpty(POCContactNo))
        //                {
        //                    sbBody.Append($"{POCContactNo}");
        //                    sbBody.Append("<br/>");
        //                }
        //                sbBody.Append("Website: https://www.uplers.com/");
        //                sbBody.Append("<br/>");
        //                sbBody.Append($"{invitingIserEmailId}");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("<hr>");
        //            }
        //            else
        //            {
        //                sbBody.Append("<br/>");
        //                sbBody.Append("Uplers Product Engineering Team");
        //            }

        //            #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //            List<string> toemail = new List<string>() { ClientEmail };
        //            List<string> toemailname = new List<string>() { ClientName };

        //            string CCEMAIL = "";
        //            string CCEMAILNAME = "";

        //            MakeCCDetail ccDetails = MakeCCEmailDetails(emailForBookTimeSlotModel.HRSalesPersonID, true, true, hrId, talentId);
        //            if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //            {
        //                CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //                CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //            }

        //            #endregion
        //            #region SetParam for client email
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmailAWSSES(toemail, Subject, sbBody.ToString());

        //            #endregion

        //            #region SetParam
        //            StringBuilder sbBodyCopiedText = new();
        //            sbBodyCopiedText.Append(_configuration["InternalEmailsAppendText"]);


        //            List<string> InternaltoEmail = new List<string>
        //            {
        //               invitingIserEmailId
        //            };

        //            List<string> InternaltoEmailName = new List<string>
        //            {
        //                invitingUserName
        //            };
        //            emailOperator.SetToEmail(InternaltoEmail);
        //            emailOperator.SetToEmailName(InternaltoEmailName);
        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBodyCopiedText.ToString() + " " + sbBody.ToString());
        //            #endregion

        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail(false, false);

        //            return "success";

        //        }
        //        else
        //            return "error";
        //    }
        //    catch (Exception e) { return e.Message; }
        //}

        //#endregion

        //#region Send Email For Nurture
        //public async Task<string> SendEmailForNurture(Sproc_Get_Summary_Emails_Result result)
        //{
        //    try
        //    {
        //        EmailOperator emailOperator = new EmailOperator(_configuration);

        //        List<string> toEmails = new List<string>();
        //        List<string> toEmailNames = new List<string>();


        //        if (result != null)
        //        {
        //            toEmails.AddRange(result.ToEmailIds.Split(','));
        //            toEmailNames.AddRange(result.ToEmailNames.Split(','));

        //            #region SetParam for client Email                
        //            if (!string.IsNullOrEmpty(result.subject))
        //                emailOperator.SendEmailAWSSES(toEmails, result.subject, result.Body.ToString());
        //            #endregion

        //            #region SetParam for Internal Team
        //            StringBuilder sbBodyCopiedText = new();
        //            sbBodyCopiedText.Append(_configuration["InternalEmailsAppendText"]);

        //            List<string> InternaltoEmail = new List<string>
        //            {
        //               result.POCEmail
        //            };

        //            List<string> InternaltoEmailName = new List<string>
        //            {
        //                result.POCName
        //            };
        //            emailOperator.SetToEmail(InternaltoEmail);
        //            emailOperator.SetToEmailName(InternaltoEmailName);

        //            string CCEMAIL = result.CCEmailIds ?? "", CCEMAILNAME = result.CCEmailNames ?? "";

        //            if (CCEMAIL != "" && CCEMAILNAME != "")
        //            {
        //                CCEMAIL = CCEMAIL + "," + ccEmail;
        //                CCEMAILNAME = CCEMAILNAME + "," + ccEmailName;
        //            }
        //            else
        //            {
        //                CCEMAIL = ccEmail;
        //                CCEMAILNAME = ccEmailName;
        //            }

        //            emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);

        //            emailOperator.SetSubject(result.subject);
        //            emailOperator.SetBody(sbBodyCopiedText.ToString() + " " + result.Body.ToString());

        //            if (!string.IsNullOrEmpty(result.subject))
        //                emailOperator.SendEmail(false, false, true);
        //            #endregion
        //        }
        //        return "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
        //#endregion

        //#endregion

        //#endregion

        //#region Unused Emails
        //public void SendEmailForBookTimeSlot(Int64 TalentID, Int64 HRID, string encryptedHRID, string encryptedHRDetailID, string encryptedContactID, string encryptedAnotherRoundID, bool forSecondRound = false)
        //{
        //    #region Variables
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();
        //    string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //    string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };
        //    int round = 1; int nextRound = 2;
        //    #endregion

        //    #region GetDBData
        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HRID, 0, 0);
        //    #endregion

        //    #region Binding
        //    if (!forSecondRound)
        //    {
        //        Subject = "Congratulations " + emailForBookTimeSlotModel.TalentName + " you are one step closer - " + emailForBookTimeSlotModel.HR_Number;

        //        BodyCustom = "Hello " + emailForBookTimeSlotModel.TalentName + ", ";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Congratulations! The client showed interest in your profile and is ready to interview you.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please log into the app and check out slots provided by client in next 12 - 18 hrs and confirm the slot you can be present for interview.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Remember, there are a couple of other profiles along with yours that client has shortlisted. So, the faster you book, higher are the chances of you getting hired. ");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString().Trim() + "ConfirmInterviewSlot/ConfirmInterviewSlot?HiringRequestDetailID=" + encryptedHRDetailID + "&HiringRequestId=" + encryptedHRID + "&ContactId=" + encryptedContactID + "'' target='_blank'>Click here to review the slots and book your interview.</a>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any questions, feel free to reach your TSC (" + emailForBookTimeSlotModel.TalentSuccessEmail + ").");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }
        //    else
        //    {
        //        Subject = "Congratulations! interview slots are shared by " + emailForBookTimeSlotModel.clientName + ", please confirm slot for the " + digits_wordsRoman[nextRound] + " round of interview - " + emailForBookTimeSlotModel.HR_Number;

        //        BodyCustom = "Hello " + emailForBookTimeSlotModel.TalentName + ", ";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Warm wishes!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The client has provided slots for scheduling the " + digits_wordsRoman[nextRound] + " round of interview with you. Please confirm your availability for the interview with " + emailForBookTimeSlotModel.CompanyName + " for " + emailForBookTimeSlotModel.position + " position within 24 hrs. You can confirm a slot for the round " + digits_words[nextRound] + " interview with the selected client from <a class='link' href='" + _configuration["FrontOfficeUrl"] + "ConfirmInterviewSlot/ConfirmInterviewSlot?HiringRequestDetailID=" + encryptedHRDetailID + "&HiringRequestId=" + encryptedHRID + "&ContactId=" + encryptedContactID + "&AnotherRound_Id=" + encryptedAnotherRoundID + "'' target='_blank'>here</a>.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("" + emailForBookTimeSlotModel.TalentName + ", making client wait, would not be a good option. So, the faster you confirm and proceed, the higher are the chances of you getting hired.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("In case of any concerns or queries, feel free to reach out to your Talent Success Coach (" + emailForBookTimeSlotModel.TalentSuccessEmail + ").");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "PointOfContact/PointOfContact' target='_blank'>Click here to contact TSC</a>.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }
        //    #endregion

        //    #region SetGetEmailOperator
        //    List<string> toEmail = new List<string>() { emailForBookTimeSlotModel.TalentEmail };
        //    List<string> toEmailName = new List<string>() { emailForBookTimeSlotModel.TalentName };
        //    emailOperator.SetToEmail(toEmail);
        //    emailOperator.SetToEmailName(toEmailName);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());
        //    #endregion

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail();
        //    #endregion
        //}
        //public void SendEmailNotificationToTalentwithZoomDetails_Schedule(Meeting zoomMeeting, int SlotType, string InterviewCallLink, long? TalentID, long? Contact_Id, DateTime scheduleTime, string TimeZone, long? HiringRequestID, DateTime dateTimeEnd, PrgContactTimeZone prg_ContactTimeZone, sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact_Result, long ShortlistedId = 0)
        //{
        //    #region Variables
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    #endregion

        //    #region GetDBData
        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID.Value, 0, 0, false, HiringRequestID.Value, Contact_Id.Value, ShortlistedId);
        //    #endregion

        //    if (sproc_Get_ContactPointofContact_Result != null)
        //    {
        //        emailForBookTimeSlotModel.ManagerEmail = sproc_Get_ContactPointofContact_Result.EmailID;
        //    }

        //    #region ContentBinding 
        //    string talent_Subject = string.Empty;
        //    string talent_BodyCustom = string.Empty;
        //    StringBuilder talent_sbBody = new StringBuilder();

        //    talent_Subject = "[Interview Scheduled] Congratulations " + emailForBookTimeSlotModel.TalentName + " you got an interview Scheduled - " + emailForBookTimeSlotModel.HR_Number;
        //    talent_BodyCustom = "Hello " + emailForBookTimeSlotModel.TalentName + ",";
        //    talent_sbBody.Append(talent_BodyCustom);
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("<div style='width:100%'>");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("Thank you for confirming the new slot for an interview. Below are the details of the interview, please add them to your calendar.<br/>");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("Here is the detail:");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("HRID: " + emailForBookTimeSlotModel.HR_Number);
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("Position: " + emailForBookTimeSlotModel.position);
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("Interviewer: " + emailForBookTimeSlotModel.Interviewer);
        //    talent_sbBody.Append("<br/>");

        //    if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //    {
        //        talent_sbBody.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //    }
        //    else
        //    {
        //        talent_sbBody.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //        talent_sbBody.Append("<br/>");

        //        talent_sbBody.Append("Interview date & time (IST): " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //        dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //    }

        //    //talent_sbBody.Append("Interview date & time: " + scheduleTime.ToString("dd/MM/yyyy HH:mm") + " " + TimeZone);
        //    talent_sbBody.Append("<br/>");

        //    if (SlotType == 4)
        //    {
        //        talent_sbBody.Append("Interview Link: " + InterviewCallLink);
        //    }
        //    else
        //    {
        //        talent_sbBody.Append("Interview Link: " + zoomMeeting.join_url);
        //        //talent_sbBody.Append("<br/>");
        //        //talent_sbBody.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //    }

        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("Please review the details about the company and interviewer in the app, before the interview.");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("There are few recommendations also provided in the app.");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("Looking forward to seeing you 5 mins before the interview. If you have any questions related to the interview, feel free to reach your Talent Success Coach.");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("Thank you");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("Uplers Talent Solutions Team");
        //    talent_sbBody.Append("<br/>");
        //    talent_sbBody.Append("<hr>");
        //    talent_sbBody.Append("We are committed to your privacy. ");
        //    talent_sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    talent_sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //    talent_sbBody.Append("</div>");
        //    #endregion

        //    #region AttachmentBinding
        //    var descriptionCalender = new StringBuilder();

        //    if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //    {
        //        descriptionCalender.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //    }
        //    else
        //    {
        //        descriptionCalender.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //        descriptionCalender.Append("<br/>");

        //        descriptionCalender.Append("Interview date & time (IST): " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //        dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //    }

        //    //descriptionCalender.Append("Interview date & time: " + scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString() + " IST.");
        //    descriptionCalender.Append("<br/>");

        //    if (SlotType == 4)
        //    {
        //        descriptionCalender.Append("Interview Link: " + InterviewCallLink);
        //    }
        //    else
        //    {
        //        descriptionCalender.Append("Interview Link: " + zoomMeeting.join_url);
        //        //descriptionCalender.Append("<br/>");
        //        //descriptionCalender.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //    }

        //    StringBuilder str = new StringBuilder();
        //    str.AppendLine("BEGIN:VCALENDAR");
        //    str.AppendLine("PRODID:-//Schedule a Meeting");
        //    str.AppendLine("VERSION:2.0");
        //    str.AppendLine("METHOD:REQUEST");

        //    str.AppendLine("BEGIN:VEVENT");
        //    str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //    str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //    str.AppendLine("LOCATION: " + "Remote");
        //    str.AppendLine("ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:talentconnect@uplers.com");
        //    str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //    str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //    str.AppendLine(string.Format("SUMMARY:{0}", "Interview with " + emailForBookTimeSlotModel.CompanyName + " | " + emailForBookTimeSlotModel.position));
        //    str.AppendLine("BEGIN:VALARM");
        //    str.AppendLine("TRIGGER:-PT15M");
        //    str.AppendLine("ACTION:DISPLAY");
        //    str.AppendLine("DESCRIPTION:Reminder");
        //    str.AppendLine("END:VALARM");
        //    str.AppendLine("END:VEVENT");
        //    str.AppendLine("END:VCALENDAR");

        //    byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //    MemoryStream stream = new MemoryStream(byteArray);

        //    Attachment attach = new Attachment(stream, "invite.ics");
        //    #endregion

        //    #region SetGetEmailOperator
        //    List<string> toEmail = new List<string>() { emailForBookTimeSlotModel.TalentEmail };
        //    List<string> toEmailName = new List<string>() { emailForBookTimeSlotModel.TalentName };
        //    emailOperator.SetToEmail(toEmail);
        //    emailOperator.SetToEmailName(toEmailName);
        //    emailOperator.SetSubject(talent_Subject);
        //    emailOperator.SetBody(talent_sbBody.ToString());
        //    emailOperator.SetAttachment(new List<Attachment>() { attach });
        //    #endregion

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(talent_Subject))
        //        emailOperator.SendEmail();
        //    #endregion

        //}
        //public void SendEmailNotificationToClientwithZoomDetails_Schedule(Meeting zoomMeeting, int SlotType, string InterviewCallLink, long? TalentID, long? Contact_Id, DateTime scheduleTime, string TimeZone, long? HiringRequestID, DateTime dateTimeEnd, PrgContactTimeZone prg_ContactTimeZone, sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact_Result, long ShortlistedId = 0)
        //{
        //    #region Variables
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    #endregion

        //    #region GetDBData
        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID.Value, 0, 0, false, HiringRequestID.Value, Contact_Id.Value, ShortlistedId);
        //    #endregion

        //    if (sproc_Get_ContactPointofContact_Result != null)
        //    {
        //        emailForBookTimeSlotModel.ManagerEmail = sproc_Get_ContactPointofContact_Result.EmailID;
        //    }

        //    #region ContentBinding 
        //    string client_Subject = "";
        //    string client_BodyCustom = "";
        //    StringBuilder client_sbBody = new StringBuilder();

        //    client_Subject = "[Interview Scheduled] Congratulations " + emailForBookTimeSlotModel.clientName + " you got an interview Scheduled - " + emailForBookTimeSlotModel.HR_Number;
        //    client_BodyCustom = "Hello " + emailForBookTimeSlotModel.clientName + ",";
        //    client_sbBody.Append(client_BodyCustom);
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("<div style='width:100%'>");
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("Thank you for providing 3 slots, the talent " + emailForBookTimeSlotModel.TalentName + " has confirmed the slot for an interview.<br/>");
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("Here is the detail:");
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("HRID: " + emailForBookTimeSlotModel.HR_Number);
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("Position: " + emailForBookTimeSlotModel.position);
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("Talent Name: " + emailForBookTimeSlotModel.TalentName);
        //    client_sbBody.Append("<br/>");
        //    //client_sbBody.Append("Interview date & time: " + scheduleTime.ToString("dd/MM/yyyy HH:mm") + " " + TimeZone);

        //    if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //    {
        //        client_sbBody.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //    }
        //    else
        //    {
        //        client_sbBody.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //        client_sbBody.Append("<br/>");

        //        client_sbBody.Append("Interview date & time (IST): " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //        dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //    }

        //    client_sbBody.Append("<br/>");

        //    if (SlotType == 4)
        //    {
        //        client_sbBody.Append("Interview Link: " + InterviewCallLink);
        //    }
        //    else
        //    {
        //        client_sbBody.Append("Interview Link: " + zoomMeeting.join_url);
        //        //client_sbBody.Append("<br/>");
        //        //client_sbBody.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //    }

        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("Please review the full profile before the interview in the app.");
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("Looking forward to seeing you 5 mins before the interview. If you have any questions related to the interview, feel free to reach your Account Manager for any help " + emailForBookTimeSlotModel.ManagerEmail);
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("<br/>");
        //    if (emailForBookTimeSlotModel.IsResetPassword)
        //    {
        //        client_sbBody.Append("<br/>");
        //        //client_sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //        client_sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //        client_sbBody.Append("<br/>");
        //        client_sbBody.Append("LoginID : " + emailForBookTimeSlotModel.Username);
        //        client_sbBody.Append("<br/>");
        //        client_sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //    }
        //    else
        //    {
        //        client_sbBody.Append("<br/>");
        //        //client_sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailForBookTimeSlotModel.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailForBookTimeSlotModel.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //        client_sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailForBookTimeSlotModel.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailForBookTimeSlotModel.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //        client_sbBody.Append("<br/>");
        //        client_sbBody.Append("LoginID : " + emailForBookTimeSlotModel.Username);
        //    }
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("Thank you");
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("Uplers Talent Solutions Team");
        //    client_sbBody.Append("<br/>");
        //    client_sbBody.Append("<hr>");
        //    client_sbBody.Append("We are committed to your privacy. ");
        //    client_sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    client_sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //    client_sbBody.Append("</div>");
        //    #endregion

        //    #region AttachmentBinding 
        //    StringBuilder str = new StringBuilder();
        //    string Description = emailDatabaseContentProvider.GetPrgContactTimeZone(TimeZone);
        //    string TimeZoneTitleValue = string.Empty;

        //    var UTCStartTime = scheduleTime.ToUniversalTime();
        //    var UTCEndTime = dateTimeEnd.ToUniversalTime();

        //    DateTime NewStartTime;
        //    DateTime NewEndTime;
        //    if (!string.IsNullOrEmpty(Description))
        //    {
        //        TimeZoneTitleValue = Description.Split(')')[1].Trim();
        //        NewStartTime = TimeZoneInfo.ConvertTimeToUtc(scheduleTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneTitleValue));
        //        NewEndTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeEnd, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneTitleValue));
        //    }
        //    else
        //    {
        //        NewStartTime = scheduleTime;
        //        NewEndTime = dateTimeEnd;
        //    }


        //    var descriptionCalender = new StringBuilder();

        //    //descriptionCalender.Append("Interview date & time: " + scheduleTime.ToString() + " " + TimeZone);

        //    if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //    {
        //        descriptionCalender.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //    }
        //    else
        //    {
        //        descriptionCalender.Append("Interview date & time: " +
        //        scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //        scheduleTime.ToString("hh:mm tt ") +
        //        dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //        descriptionCalender.Append("<br/>");

        //        descriptionCalender.Append("Interview date & time (IST): " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //        scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //        dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //    }


        //    descriptionCalender.Append("<br/>");

        //    if (SlotType == 4)
        //    {
        //        descriptionCalender.Append("Interview Link: " + InterviewCallLink);
        //    }
        //    else
        //    {
        //        descriptionCalender.Append("Interview Link: " + zoomMeeting.join_url);
        //        //descriptionCalender.Append("<br/>");
        //        //descriptionCalender.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //    }

        //    str.AppendLine("BEGIN:VCALENDAR");
        //    str.AppendLine("PRODID:-//Schedule a Meeting");
        //    str.AppendLine("VERSION:2.0");
        //    str.AppendLine("METHOD:REQUEST");

        //    str.AppendLine("BEGIN:VEVENT");
        //    //str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", NewStartTime));
        //    //str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", NewEndTime));
        //    str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //    str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //    str.AppendLine("LOCATION: " + "Remote");
        //    str.AppendLine("ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:talentconnect@uplers.com");
        //    str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //    str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //    str.AppendLine(string.Format("SUMMARY:{0}", "Interview with " + emailForBookTimeSlotModel.TalentName + " | " + emailForBookTimeSlotModel.position));
        //    str.AppendLine("BEGIN:VALARM");
        //    str.AppendLine("TRIGGER:-PT15M");
        //    str.AppendLine("ACTION:DISPLAY");
        //    str.AppendLine("DESCRIPTION:Reminder");
        //    str.AppendLine("END:VALARM");
        //    str.AppendLine("END:VEVENT");
        //    str.AppendLine("END:VCALENDAR");

        //    byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //    MemoryStream stream = new MemoryStream(byteArray);

        //    Attachment attach = new Attachment(stream, "invite.ics");
        //    #endregion

        //    #region SetGetEmailOperator
        //    List<string> toEmail = new List<string>() { emailForBookTimeSlotModel.ClientEmail };
        //    List<string> toEmailName = new List<string>() { emailForBookTimeSlotModel.clientName };
        //    emailOperator.SetToEmail(toEmail);
        //    emailOperator.SetToEmailName(toEmailName);
        //    emailOperator.SetSubject(client_Subject);
        //    emailOperator.SetBody(client_sbBody.ToString());
        //    emailOperator.SetAttachment(new List<Attachment>() { attach });
        //    #endregion

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(client_Subject))
        //        emailOperator.SendEmail(false, true);
        //    #endregion
        //}
        //public bool SendEmailNotificationwithZoomDetails_Cancel(long TalentId, long HiringRequestID, long TalentSelected_InterviewDetails_ID, DateTime scheduleTime, long ShortlistedId = 0)
        //{
        //    try
        //    {
        //        bool sentEmail = false;

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentId, 0, 0, false, HiringRequestID, 0, ShortlistedId);
        //        #endregion

        //        GenTalentSelectedInterviewDetail InterviewDetails = new GenTalentSelectedInterviewDetail();
        //        InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(xy => xy.Id == TalentSelected_InterviewDetails_ID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(InterviewDetails).Reload();

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        if (ShortlistedId > 0)
        //        {
        //            #region Cancel mail to Talent

        //            Subject = "[Interview Scheduled] Unfortunately this interview has been cancelled - " + emailDetails.HR_Number;
        //            BodyCustom = "Hello " + emailDetails.TalentName + ",";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<div style='width:100%'>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Due to unavoidable circumstances this interview has been cancelled. Below are the details for the same. You will shortly get another message for further course of action.<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Interview Cancelled");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("HRID: " + emailDetails.HR_Number + "");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Position: " + emailDetails.position + "");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Interviewer: " + emailDetails.Interviewer + "");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Interview date & time: " + scheduleTime.ToString("dd/MM/yyyy HH:mm"));
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Interview Link: " + InterviewDetails.ZoomInterviewLink + "");
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("Pass Code: " + InterviewDetails.ZoomInterviewKitPassword + " (Optional)");
        //            //sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            if (emailDetails.IsResetPassword)
        //            {
        //                sbBody.Append("<br/>");
        //                //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //                sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("LoginID : " + emailDetails.Username);
        //                sbBody.Append("<br/>");
        //                sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //            }
        //            else
        //            {
        //                sbBody.Append("<br/>");
        //                //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetails.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //                sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("LoginID : " + emailDetails.Username);
        //            }
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Thank you");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<hr>");
        //            sbBody.Append("We are committed to your privacy. ");
        //            sbBody.Append("You may unsubscribe from these communications at any time. ");
        //            sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //            sbBody.Append("</div>");

        //            string TOEMAIL = emailDetails.TalentEmail;
        //            string TOEMAILNAME = emailDetails.TalentName;

        //            if (!string.IsNullOrEmpty(TOEMAIL))
        //            {
        //                List<string> toemail = new List<string>() { TOEMAIL };
        //                List<string> toemailname = new List<string>() { TOEMAILNAME };

        //                emailOperator.SetToEmail(toemail);
        //                emailOperator.SetToEmailName(toemailname);
        //                emailOperator.SetSubject(Subject);
        //                emailOperator.SetBody(sbBody.ToString());

        //                #region SendEmail
        //                if (!string.IsNullOrEmpty(Subject))
        //                    emailOperator.SendEmail();
        //                #endregion
        //            }
        //            #endregion

        //        }
        //        else
        //        {
        //            #region Cancel mail to client

        //            Subject = "[Interview Scheduled] Unfortunately this interview has been cancelled - " + emailDetails.HR_Number;
        //            BodyCustom = "Hello " + emailDetails.clientName + ",";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<div style='width:100%'>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Due to unavoidable circumstances this interview has been cancelled. Below are the details for the same. You will shortly get another message for further course of action.<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Interview Cancelled");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("HRID: " + emailDetails.HR_Number + "");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Position: " + emailDetails.position + "");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Interviewer: " + emailDetails.Interviewer + "");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Interview date & time: " + scheduleTime.ToString("dd/MM/yyyy HH:mm"));
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Interview Link: " + InterviewDetails.ZoomInterviewLink + "");
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("Pass Code: " + InterviewDetails.ZoomInterviewKitPassword + " (Optional)");
        //            //sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            if (emailDetails.IsResetPassword)
        //            {
        //                sbBody.Append("<br/>");
        //                //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //                sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("LoginID : " + emailDetails.Username);
        //                sbBody.Append("<br/>");
        //                sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //            }
        //            else
        //            {
        //                sbBody.Append("<br/>");
        //                //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetails.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //                sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("LoginID : " + emailDetails.Username);
        //            }
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Thank you");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<hr>");
        //            sbBody.Append("We are committed to your privacy. ");
        //            sbBody.Append("You may unsubscribe from these communications at any time. ");
        //            sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //            sbBody.Append("</div>");

        //            string TOEMAIL = emailDetails.ClientEmail;
        //            string TOEMAILNAME = emailDetails.clientName;

        //            if (!string.IsNullOrEmpty(TOEMAIL))
        //            {
        //                List<string> toemail = new List<string>() { TOEMAIL };
        //                List<string> toemailname = new List<string>() { TOEMAILNAME };

        //                emailOperator.SetToEmail(toemail);
        //                emailOperator.SetToEmailName(toemailname);
        //                emailOperator.SetSubject(Subject);
        //                emailOperator.SetBody(sbBody.ToString());

        //                #region SendEmail
        //                if (!string.IsNullOrEmpty(Subject))
        //                    emailOperator.SendEmail(false, true);
        //                #endregion
        //            }
        //            #endregion
        //        }
        //        sentEmail = true;
        //        return sentEmail;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailAfterInterview_Completed(long TalentID, long HRID, bool IsForClient)
        //{
        //    try
        //    {
        //        bool sentEmail = false;

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HRID, 0, 0);
        //        #endregion

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        if (IsForClient)
        //        {
        //            Subject = "You have a feedback pending for the interview - " + emailDetails.HR_Number;
        //            BodyCustom = "Hello " + emailDetails.clientName + ",";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<div style='width:100%'>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("We hope the experience of an interview was fantastic. You are yet to submit feedback on that interview.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Please help us and Talent know about your experience.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("HRID: " + emailDetails.HR_Number + "<br/>");
        //            sbBody.Append("Position: " + emailDetails.position + "<br/>");
        //            sbBody.Append("Talent Name: " + emailDetails.TalentName + "<br/>");
        //            sbBody.Append("<br/>");
        //            if (emailDetails.IsResetPassword)
        //            {
        //                sbBody.Append("<br/>");
        //                sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "ClientFeedbackList/ClientFeedbackList' target='_blank'>Submit Your Feedback</a>.");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("LoginID : " + emailDetails.Username);
        //                sbBody.Append("<br/>");
        //                sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //            }
        //            else
        //            {
        //                sbBody.Append("<br/>");
        //                //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetails.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //                sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("LoginID : " + emailDetails.Username);
        //            }
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("The talent shall be available only for 5 days under this hiring request, hire <him/her> quickly. In case if you haven't liked him, don't worry, submit a feedback and we can help you find a better talent.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Also if you have any questions, feel free to contact your Account Manager " + emailDetails.ManagerEmail + ".");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Looking forward to receiving your feedback.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Thanks");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<hr>");
        //            sbBody.Append("We are committed to your privacy. ");
        //            sbBody.Append("You may unsubscribe from these communications at any time. ");
        //            sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //            sbBody.Append("</div>");

        //            string TOEMAIL = emailDetails.ClientEmail;
        //            string TOEMAILNAME = emailDetails.clientName;

        //            if (!string.IsNullOrEmpty(TOEMAIL))
        //            {
        //                List<string> toemail = new List<string>() { TOEMAIL };
        //                List<string> toemailname = new List<string>() { TOEMAILNAME };

        //                emailOperator.SetToEmail(toemail);
        //                emailOperator.SetToEmailName(toemailname);
        //                emailOperator.SetSubject(Subject);
        //                emailOperator.SetBody(sbBody.ToString());

        //                #region SendEmail
        //                if (!string.IsNullOrEmpty(Subject))
        //                    emailOperator.SendEmail(false, true);
        //                #endregion
        //            }
        //        }
        //        else
        //        {
        //            Subject = "You have a feedback pending for the interview - " + emailDetails.HR_Number;
        //            BodyCustom = "Hello " + emailDetails.TalentName + ",";
        //            sbBody.Append(BodyCustom);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<div style='width:100%'>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("We hope the experience of an interview was fantastic. You are yet to submit feedback on that interview.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Please help us and clients know about your experience and feedback.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("HRID: " + emailDetails.HR_Number + "<br/>");
        //            sbBody.Append("Position: " + emailDetails.position + "<br/>");
        //            sbBody.Append("Company Name: " + emailDetails.CompanyName + "<br/>");
        //            sbBody.Append("<br/>");
        //            if (emailDetails.IsResetPassword)
        //            {
        //                sbBody.Append("<br/>");
        //                sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "TalentFeedback/TalentFeedbackList' target='_blank'>Submit Your Feedback</a>.");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("LoginID : " + emailDetails.Username);
        //                sbBody.Append("<br/>");
        //                sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //            }
        //            else
        //            {
        //                sbBody.Append("<br/>");
        //                //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetails.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //                sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //                sbBody.Append("<br/>");
        //                sbBody.Append("LoginID : " + emailDetails.Username);
        //            }
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("This hiring request shall be available only for 5 days, quickly submit the feedback and so that when you get selected you don't have to lose this opportunity.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have any questions, feel free to contact your TSC (" + emailDetails.TalentSuccessEmail + ").");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Looking forward to receiving your feedback.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<br/>");

        //            sbBody.Append("Thanks");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("Uplers Talent Solutions Team");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<hr>");
        //            sbBody.Append("We are committed to your privacy. ");
        //            sbBody.Append("You may unsubscribe from these communications at any time. ");
        //            sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //            sbBody.Append("</div>");

        //            string TOEMAIL = emailDetails.TalentEmail;
        //            string TOEMAILNAME = emailDetails.TalentName;

        //            if (!string.IsNullOrEmpty(TOEMAIL))
        //            {
        //                List<string> toemail = new List<string>() { TOEMAIL };
        //                List<string> toemailname = new List<string>() { TOEMAILNAME };

        //                emailOperator.SetToEmail(toemail);
        //                emailOperator.SetToEmailName(toemailname);
        //                emailOperator.SetSubject(Subject);
        //                emailOperator.SetBody(sbBody.ToString());

        //                #region SendEmail
        //                if (!string.IsNullOrEmpty(Subject))
        //                    emailOperator.SendEmail();
        //                #endregion
        //            }
        //        }

        //        sentEmail = true;
        //        return sentEmail;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailNotificationToInterviewerwithZoomDetails(Meeting zoomMeeting, long TalentID, long? Contact_Id, string InterviewerEmail, string InterviewerName, DateTime scheduleTime, string TimeZone, long HiringRequestID, DateTime dateTimeEnd, PrgContactTimeZone prg_ContactTimeZone, long ShortlistedId = 0)
        //{
        //    try
        //    {
        //        bool emailsent = false;

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, ShortlistedId);
        //        #endregion

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "[Interview Scheduled] Congratulations " + InterviewerName + " you got an interview Scheduled - " + emailDetails.HR_Number;
        //        BodyCustom = "Hi " + InterviewerName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Warm wishes from Uplers Talent Solutions Team!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("" + emailDetails.clientName + " have scheduled an interview with " + emailDetails.TalentName + " for " + emailDetails.position + ". To know more about the talent, kindly connect with " + emailDetails.clientName + ". Below are the interview details for you to refer.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Details");
        //        sbBody.Append("<br/>");

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            sbBody.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + TimeZone);
        //        }
        //        else
        //        {
        //            sbBody.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + TimeZone);

        //            sbBody.Append("<br/>");

        //            sbBody.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }

        //        //sbBody.Append("Interview date & time: " + scheduleTime.ToString("dd/MM/yyyy HH:mm") + " " + TimeZone);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Link: " + zoomMeeting.join_url);
        //        //sbBody.Append("<br/>");
        //        //sbBody.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interviewer: " + emailDetails.Interviewer);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Note: Kindly add and save the above interview details to your calendar.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please be available on the mentioned date and time for assessing the talent further.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any questions, you can reply to this email " + emailDetails.ManagerEmail + ". Alternatively, feel free to contact our customer success team talent@uplers.com anytime. We’ll ensure to respond to you at the earliest.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");


        //        string TOEMAIL = InterviewerEmail;
        //        string TOEMAILNAME = InterviewerName;

        //        StringBuilder str = new StringBuilder();
        //        var descriptionCalender = new StringBuilder();

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            descriptionCalender.Append("<br/>");

        //            descriptionCalender.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }


        //        //descriptionCalender.Append("Interview date & time: " + scheduleTime.ToString() + " " + TimeZone);
        //        descriptionCalender.Append("<br/>");
        //        descriptionCalender.Append("Interview Link: " + zoomMeeting.join_url);

        //        //if (!string.IsNullOrEmpty(zoomMeeting.password))
        //        //{
        //        //    descriptionCalender.Append("<br/>");
        //        //    descriptionCalender.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //        //}

        //        str.AppendLine("BEGIN:VCALENDAR");
        //        str.AppendLine("PRODID:-//Schedule a Meeting");
        //        str.AppendLine("VERSION:2.0");
        //        str.AppendLine("METHOD:REQUEST");

        //        str.AppendLine("BEGIN:VEVENT");
        //        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine("LOCATION: " + "Remote");
        //        str.AppendLine($"ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:{clientEmailName}");
        //        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //        str.AppendLine(string.Format("SUMMARY:{0}", "Interview with " + emailDetails.TalentName + " | " + emailDetails.position));
        //        str.AppendLine("BEGIN:VALARM");
        //        str.AppendLine("TRIGGER:-PT15M");
        //        str.AppendLine("ACTION:DISPLAY");
        //        str.AppendLine("DESCRIPTION:Reminder");
        //        str.AppendLine("END:VALARM");
        //        str.AppendLine("END:VEVENT");
        //        str.AppendLine("END:VCALENDAR");

        //        byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //        MemoryStream stream = new MemoryStream(byteArray);

        //        Attachment attach = new Attachment(stream, "invite.ics");

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());
        //            emailOperator.SetAttachment(new List<Attachment>() { attach });

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail(false, true);
        //            #endregion
        //        }

        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailNotificationToClientwithZoomDetailsForSecondRound(Meeting zoomMeeting, long TalentID, long? Contact_Id, DateTime scheduleTime, string TimeZone, long HiringRequestID, DateTime dateTimeEnd, PrgContactTimeZone prg_ContactTimeZone)
        //{
        //    try
        //    {
        //        bool emailsent = false;

        //        int round = 1; int nextRound = 2;

        //        string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //        string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, 0);
        //        #endregion

        //        GenTalentSelectedInterviewDetail talentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_InterviewDetails).Reload();
        //        if (talentSelected_InterviewDetails != null)
        //        {
        //            round = Convert.ToInt32(talentSelected_InterviewDetails.InterviewRound);
        //            nextRound = round + 1;
        //        }

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "Congratulations!, Interview has been scheduled for round " + nextRound.ToString() + " with " + emailDetails.TalentName + " - " + emailDetails.HR_Number;
        //        BodyCustom = "Hi " + emailDetails.clientName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Congratulations!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The " + emailDetails.TalentName + " has confirmed the round " + digits_words[nextRound] + " interview slot from the list of three available dates and timings shared by you. Below are the details of the interview for you to refer.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Details:");
        //        sbBody.Append("<br/>");

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            sbBody.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            sbBody.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            sbBody.Append("<br/>");

        //            sbBody.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }

        //        //sbBody.Append("Interview date & time: "+ scheduleTime.ToString("dd/MM/yyyy HH:mm") + " " + TimeZone);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Link: " + zoomMeeting.join_url);
        //        //sbBody.Append("<br/>");
        //        //sbBody.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");

        //        if (emailDetails.IsResetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append("You can view details of the interview and the talent from <a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "ClientYourUpcomingInterview/ClientYourUpcomingInterview' target='_blank'>here</a> as well.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetails.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetails.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetails.Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Kindly add and save these above details to your calendar.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any questions, you can reply to this email (" + emailDetails.ManagerEmail + "). Alternatively, feel free to contact our customer success team talent@uplers.com anytime. We’ll ensure to respond to you at the earliest.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetails.ClientEmail;
        //        string TOEMAILNAME = emailDetails.clientName;

        //        StringBuilder str = new StringBuilder();

        //        string Description = emailDatabaseContentProvider.GetPrgContactTimeZone(TimeZone);
        //        string TimeZoneTitleValue = string.Empty;

        //        var UTCStartTime = scheduleTime.ToUniversalTime();
        //        var UTCEndTime = dateTimeEnd.ToUniversalTime();

        //        DateTime NewStartTime;
        //        DateTime NewEndTime;
        //        if (!string.IsNullOrEmpty(Description))
        //        {
        //            TimeZoneTitleValue = Description.Split(')')[1].Trim();
        //            NewStartTime = TimeZoneInfo.ConvertTimeToUtc(scheduleTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneTitleValue));
        //            NewEndTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeEnd, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneTitleValue));
        //        }
        //        else
        //        {
        //            NewStartTime = scheduleTime;
        //            NewEndTime = dateTimeEnd;
        //        }

        //        var descriptionCalender = new StringBuilder();

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            descriptionCalender.Append("<br/>");

        //            descriptionCalender.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }

        //        //descriptionCalender.Append("Interview date & time: " + scheduleTime.ToString() + " " + TimeZone);
        //        descriptionCalender.Append("<br/>");
        //        descriptionCalender.Append("Interview Link: " + zoomMeeting.join_url);
        //        //descriptionCalender.Append("<br/>");
        //        //descriptionCalender.Append("Pass Code : " + zoomMeeting.password + " (Optional)");

        //        str.AppendLine("BEGIN:VCALENDAR");
        //        str.AppendLine("PRODID:-//Schedule a Meeting");
        //        str.AppendLine("VERSION:2.0");
        //        str.AppendLine("METHOD:REQUEST");

        //        str.AppendLine("BEGIN:VEVENT");
        //        //str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", NewStartTime));
        //        //str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", NewEndTime));
        //        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine("LOCATION: " + "Remote");
        //        str.AppendLine("ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:talentconnect@uplers.com");
        //        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //        str.AppendLine(string.Format("SUMMARY:{0}", "Interview with " + emailDetails.TalentName + " | " + emailDetails.position));
        //        str.AppendLine("BEGIN:VALARM");
        //        str.AppendLine("TRIGGER:-PT15M");
        //        str.AppendLine("ACTION:DISPLAY");
        //        str.AppendLine("DESCRIPTION:Reminder");
        //        str.AppendLine("END:VALARM");
        //        str.AppendLine("END:VEVENT");
        //        str.AppendLine("END:VCALENDAR");

        //        byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //        MemoryStream stream = new MemoryStream(byteArray);

        //        Attachment attach = new Attachment(stream, "invite.ics");
        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());
        //            emailOperator.SetAttachment(new List<Attachment>() { attach });

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail(false, true);
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailNotificationToTalentwithZoomDetailsForSecondRound(Meeting zoomMeeting, long TalentID, long? Contact_Id, DateTime scheduleTime, string TimeZone, long HiringRequestID, DateTime dateTimeEnd, PrgContactTimeZone prg_ContactTimeZone)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        int round = 1; int nextRound = 2;
        //        string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //        string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, 0);
        //        #endregion

        //        GenTalentSelectedInterviewDetail talentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_InterviewDetails).Reload();
        //        if (talentSelected_InterviewDetails != null)
        //        {
        //            round = Convert.ToInt32(talentSelected_InterviewDetails.InterviewRound);
        //            nextRound = round + 1;
        //        }

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "Congratulations!, Interview has been scheduled for round " + nextRound + " with " + emailDetails.CompanyName + " - " + emailDetails.HR_Number;
        //        BodyCustom = "Hi " + emailDetails.TalentName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for confirming the " + digits_wordsRoman[nextRound] + " round interview slot and congratulations on getting your round " + digits_words[nextRound] + " screening scheduled with " + emailDetails.CompanyName + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Below are the details of the interview, please add and save them to your calendar.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Details:");
        //        sbBody.Append("<br/>");
        //        //sbBody.Append("Interview date & time: " + scheduleTime.ToString("dd/MM/yyyy HH:mm") + " " + TimeZone);

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            sbBody.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            sbBody.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            sbBody.Append("<br/>");

        //            sbBody.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }


        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Link: " + zoomMeeting.join_url);
        //        //sbBody.Append("<br/>");
        //        //sbBody.Append("Pass Code: " + zoomMeeting.password + " (Optional)");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interviewer: " + emailDetails.Interviewer);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Remember, this " + digits_wordsRoman[nextRound] + " round of screening might be the ultimate gateway for you to secure your position for this open hiring request. So, give your best shot in this round and impress the interviewers.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any questions, feel free to reach your TSC at " + emailDetails.TalentSuccessEmail + "");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Looking forward to seeing you 5 mins before the interview. If you have any questions related to the interview, feel free to reach your Talent Success Coach.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetails.TalentEmail;
        //        string TOEMAILNAME = emailDetails.TalentName;

        //        var descriptionCalender = new StringBuilder();

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            descriptionCalender.Append("<br/>");

        //            descriptionCalender.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }

        //        //descriptionCalender.Append("Interview date & time: " + scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString() + " " + TimeZone + ".");
        //        descriptionCalender.Append("<br/>");
        //        descriptionCalender.Append("Interview Link: " + zoomMeeting.join_url + ".");
        //        //descriptionCalender.Append("<br/>");
        //        //descriptionCalender.Append("Pass Code : " + zoomMeeting.password + " (Optional)");

        //        StringBuilder str = new StringBuilder();
        //        str.AppendLine("BEGIN:VCALENDAR");
        //        str.AppendLine("PRODID:-//Schedule a Meeting");
        //        str.AppendLine("VERSION:2.0");
        //        str.AppendLine("METHOD:REQUEST");

        //        str.AppendLine("BEGIN:VEVENT");
        //        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine("LOCATION: " + "Remote");
        //        str.AppendLine("ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:talentconnect@uplers.com");
        //        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //        str.AppendLine(string.Format("SUMMARY:{0}", "Interview with " + emailDetails.CompanyName + " | " + emailDetails.position));
        //        str.AppendLine("BEGIN:VALARM");
        //        str.AppendLine("TRIGGER:-PT15M");
        //        str.AppendLine("ACTION:DISPLAY");
        //        str.AppendLine("DESCRIPTION:Reminder");
        //        str.AppendLine("END:VALARM");
        //        str.AppendLine("END:VEVENT");
        //        str.AppendLine("END:VCALENDAR");

        //        byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //        MemoryStream stream = new MemoryStream(byteArray);

        //        Attachment attach = new Attachment(stream, "invite.ics");

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());
        //            emailOperator.SetAttachment(new List<Attachment>() { attach });

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailNotificationToInterviewerwithZoomDetailsForSecondRound(Meeting zoomMeeting, long TalentID, long? Contact_Id, string InterviewerEmail, string InterviewerName, DateTime scheduleTime, string TimeZone, long HiringRequestID, DateTime dateTimeEnd, PrgContactTimeZone prg_ContactTimeZone)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        int round = 1; int nextRound = 2;
        //        string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //        string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, 0);
        //        #endregion

        //        GenTalentSelectedInterviewDetail talentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_InterviewDetails).Reload();
        //        if (talentSelected_InterviewDetails != null)
        //        {
        //            round = Convert.ToInt32(talentSelected_InterviewDetails.InterviewRound);
        //            nextRound = round + 1;
        //        }

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "" + emailDetails.clientName + " would love to invite you and be a part of interview for a talent " + emailDetails.TalentName + " - " + emailDetails.HR_Number;
        //        BodyCustom = "Hi " + InterviewerName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Warm wishes from Uplers Talent Solutions Team!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("" + emailDetails.clientName + " from " + emailDetails.CompanyName + " have scheduled a " + digits_wordsRoman[nextRound] + " round of interview with " + emailDetails.TalentName + " for " + emailDetails.position + ". To know more about the talent, kindly connect with " + emailDetails.clientName + ". Below are the interview details for you to refer.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Details");
        //        sbBody.Append("<br/>");

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            sbBody.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            sbBody.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            sbBody.Append("<br/>");

        //            sbBody.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }

        //        //sbBody.Append("Interview date & time: " + scheduleTime.ToString("dd/MM/yyyy HH:mm") + " " + TimeZone);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interview Link: " + zoomMeeting.join_url);
        //        //sbBody.Append("<br/>");
        //        //sbBody.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Interviewer: " + emailDetails.Interviewer);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Note: Kindly add and save the above interview details to your calendar.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please be available on the mentioned date and time for assessing the talent further.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any questions, you can reply to this email " + emailDetails.ManagerEmail + ". Alternatively, feel free to contact our customer success team talent@uplers.com anytime. We’ll ensure to respond to you at the earliest.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = InterviewerEmail;
        //        string TOEMAILNAME = InterviewerName;

        //        StringBuilder str = new StringBuilder();
        //        var descriptionCalender = new StringBuilder();

        //        if (prg_ContactTimeZone.ShortName.ToLower() == "ist")
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);
        //        }
        //        else
        //        {
        //            descriptionCalender.Append("Interview date & time: " +
        //            scheduleTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " +
        //            scheduleTime.ToString("hh:mm tt ") +
        //            dateTimeEnd.ToString("hh:mm tt ") + prg_ContactTimeZone.ShortName);

        //            descriptionCalender.Append("<br/>");

        //            descriptionCalender.Append("Interview date & time (IST): " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("dd/MM/yyyy") + " " +
        //            scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") +
        //            dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value).ToString("hh:mm tt ") + "IST");
        //        }

        //        //descriptionCalender.Append("Interview date & time: " + scheduleTime.ToString() + " " + TimeZone);
        //        descriptionCalender.Append("<br/>");
        //        descriptionCalender.Append("Interview Link: " + zoomMeeting.join_url);
        //        //if (!string.IsNullOrEmpty(zoomMeeting.password))
        //        //{
        //        //    descriptionCalender.Append("<br/>");
        //        //    descriptionCalender.Append("Pass Code : " + zoomMeeting.password + " (Optional)");
        //        //}

        //        str.AppendLine("BEGIN:VCALENDAR");
        //        str.AppendLine("PRODID:-//Schedule a Meeting");
        //        str.AppendLine("VERSION:2.0");
        //        str.AppendLine("METHOD:REQUEST");

        //        str.AppendLine("BEGIN:VEVENT");
        //        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd.AddMinutes(prg_ContactTimeZone.IsttimeDiffMin.Value)));
        //        str.AppendLine("LOCATION: " + "Remote");
        //        str.AppendLine($"ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:{clientEmailName}");
        //        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //        str.AppendLine(string.Format("SUMMARY:{0}", "Interview with " + emailDetails.TalentName + " | " + emailDetails.position));
        //        str.AppendLine("BEGIN:VALARM");
        //        str.AppendLine("TRIGGER:-PT15M");
        //        str.AppendLine("ACTION:DISPLAY");
        //        str.AppendLine("DESCRIPTION:Reminder");
        //        str.AppendLine("END:VALARM");
        //        str.AppendLine("END:VEVENT");
        //        str.AppendLine("END:VCALENDAR");

        //        byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //        MemoryStream stream = new MemoryStream(byteArray);

        //        Attachment attach = new Attachment(stream, "invite.ics");

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());
        //            emailOperator.SetAttachment(new List<Attachment>() { attach });

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail(false, true);
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}

        //#region Send Email to Client,Talent and Internal Team When Client Selects any option for the interviewers and opts for giving slots right away For another round
        ////Send Email to Client When Client Selects any option for the interviewers and opts for giving slots right away For another round
        //public bool SendEmailNotificationToClientForSecondRound(long TalentID, long? Contact_Id, long HiringRequestID, sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact)
        //{
        //    try
        //    {
        //        bool emailsent = false;

        //        int round = 1; int nextRound = 2;

        //        string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //        string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, 0);
        //        #endregion

        //        GenTalentSelectedInterviewDetail talentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_InterviewDetails).Reload();
        //        if (talentSelected_InterviewDetails != null)
        //        {
        //            round = Convert.ToInt32(talentSelected_InterviewDetails.InterviewRound);
        //            nextRound = round + 1;
        //        }
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "Thank you for submitting your round " + round.ToString() + " feedback and moving " + emailDetails.TalentName + " to round " + nextRound.ToString() + " - " + emailDetails.HR_Number;
        //        BodyCustom = "Hi " + emailDetails.clientName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks for providing us with the interview slots and interviewer details for the round " + digits_words[nextRound] + " screening of " + emailDetails.TalentName + " for the " + emailDetails.position + " role.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We have sent the slots to the selected talent to confirm their availability. You will shortly receive a notification on the scheduled timing for the " + digits_wordsRoman[nextRound] + " round of interview with " + emailDetails.TalentName + " for the " + emailDetails.position + " role.");
        //        sbBody.Append("<br/>");
        //        if (emailDetails.IsResetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append("You can also keep track of the status of this hiring request and round " + nextRound.ToString() + " interview from <a class='link' href='" + _configuration["FrontOfficeUrl"] + "OpenPosition/OpenPosition' target='_blank'>here</a>.");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetails.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("T") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt((emailDetails.Username == null ? String.Empty : emailDetails.Username))) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt((emailDetails.Password == null ? String.Empty : emailDetails.Password))) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Username == null ? String.Empty : emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Password == null ? String.Empty : emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetails.Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any questions, you can reply to this email " + sproc_Get_ContactPointofContact.EmailID + ". Alternatively, feel free to contact our customer success team talent@uplers.com anytime.We’ll ensure to respond to you at the earliest.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetails.ClientEmail;
        //        string TOEMAILNAME = emailDetails.clientName;

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };
        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(ccEmail, ccEmailName);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail(false, true);
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception e) { return false; }
        //}
        ////Send Email to Talent When Client Selects any option for the interviewers and opts for giving slots right away For another round
        //public bool SendEmailNotificationToTalentForSecondRound(long TalentID, long? Contact_Id, long HiringRequestID, long? HiringRequestDetailID, string encryptedHRID, string encryptedHRDetailID, string encryptedContactID, string encryptedAnotherRound_Id)
        //{
        //    try
        //    {
        //        bool emailsent = false;

        //        int round = 1; int nextRound = 2;

        //        string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //        string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, 0);
        //        #endregion

        //        GenTalentSelectedInterviewDetail talentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_InterviewDetails).Reload();
        //        if (talentSelected_InterviewDetails != null)
        //        {
        //            round = Convert.ToInt32(talentSelected_InterviewDetails.InterviewRound);
        //            nextRound = round + 1;
        //        }

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "Congratulations, you have been moved to " + nextRound.ToString() + " round of interview - " + emailDetails.HR_Number;
        //        BodyCustom = "Hi " + emailDetails.TalentName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Warm Wishes! You were excellent in your " + digits_wordsRoman[round] + " round of interview, client would love to have another round of interview with you.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The client has provided slots for scheduling the " + digits_wordsRoman[nextRound] + " round of interview with you. Please confirm your availability for the interview with " + emailDetails.CompanyName + " for " + emailDetails.position + " position within 24 hours. You can confirm a slot for the round " + digits_words[nextRound] + " interview with the selected client from <a class='link' href='" + _configuration["FrontOfficeUrl"] + "ConfirmInterviewSlot/ConfirmInterviewSlot?HiringRequestDetailID=" + encryptedHRDetailID + "&HiringRequestId=" + encryptedHRID + "&ContactId=" + encryptedContactID + "&AnotherRound_Id=" + encryptedAnotherRound_Id + "'' target='_blank'>here</a>.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Remember, there are a couple of other profiles along with yours that the client has shortlisted. So, the faster you confirm and proceed, the higher are the chances of you getting hired.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any questions, feel free to reach your TSC at " + emailDetails.TalentSuccessEmail + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (emailDetails.IsResetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetails.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetails.Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetails.TalentEmail;
        //        string TOEMAILNAME = emailDetails.TalentName;

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };
        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(ccEmail, ccEmailName);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //#endregion

        //#region Send Email to Client,Talent and Internal Team When Client Selects any option for the interviewers and opts for giving slots right away For another round
        ////Send Email to Client When Client Selects any option for the interviewers and opts for giving slots right away For another round Later
        //public bool SendEmailNotificationToClientForSecondRoundLater(long TalentID, long? Contact_Id, long HiringRequestID, long? HiringRequestDetailID, string encryptedHRID, string encryptedRoleID, sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact)
        //{
        //    try
        //    {
        //        bool emailsent = false;

        //        int round = 1; int nextRound = 2;

        //        string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //        string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, 0);
        //        #endregion

        //        GenTalentSelectedInterviewDetail talentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_InterviewDetails).Reload();
        //        if (talentSelected_InterviewDetails != null)
        //        {
        //            round = Convert.ToInt32(talentSelected_InterviewDetails.InterviewRound);
        //            nextRound = round + 1;
        //        }

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "Thank you for submitting your round " + round.ToString() + " feedback and moving " + emailDetails.TalentName + " to round " + nextRound.ToString() + " - " + emailDetails.HR_Number;
        //        BodyCustom = "Hi " + emailDetails.clientName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks for confirming us with the details of the interviewers to be present for the round " + digits_words[nextRound] + " screening of " + emailDetails.TalentName + " for the " + emailDetails.position + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please log into the app and provide interview slots in the next 24 - 48 hrs, so  the talent can also  confirm the slot for the interview.");
        //        sbBody.Append("<br/>");
        //        if (emailDetails.IsResetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ShortListed/ShortListedTalent?HiringRequest_ID=" + encryptedHRID + "&Role_ID=" + encryptedRoleID + "' target='_blank'>Click here to view the talent details in brief and to book your interview slots.</a>.");
        //            //sbBody.Append("<a class='link' href='" + Config.FrontOfficeUrl + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetails.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetails.Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Meanwhile, if you have anything to discuss or ask, then reach out to our support representatives on talent@uplers.com.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetails.ClientEmail;
        //        string TOEMAILNAME = emailDetails.clientName;

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };
        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(ccEmail, ccEmailName);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail(false, true);
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception e) { return false; }
        //}
        ////Send Email to Talent When Client Selects any option for the interviewers and opts for giving slots right away For another round Later
        //public bool SendEmailNotificationToTalentForSecondRoundLater(long TalentID, long? Contact_Id, long HiringRequestID, long? HiringRequestDetailID)
        //{
        //    try
        //    {
        //        bool emailsent = false;

        //        int round = 1; int nextRound = 2;

        //        string[] digits_words = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        //        string[] digits_wordsRoman = { "0th", "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };

        //        #region Variables
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        EmailForBookTimeSlotModel emailDetails = new();
        //        #endregion

        //        #region GetDBData
        //        emailDetails = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequestID, 0, 0);
        //        #endregion

        //        GenTalentSelectedInterviewDetail talentSelected_InterviewDetails = _talentConnectAdminDBContext.GenTalentSelectedInterviewDetails.Where(x => x.ContactId == Contact_Id && x.TalentId == TalentID && x.HiringRequestId == HiringRequestID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentSelected_InterviewDetails).Reload();
        //        if (talentSelected_InterviewDetails != null)
        //        {
        //            round = Convert.ToInt32(talentSelected_InterviewDetails.InterviewRound);
        //            nextRound = round + 1;
        //        }

        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        Subject = "Congratulations! You have been shortlisted for round " + nextRound.ToString() + " - " + emailDetails.HR_Number;
        //        BodyCustom = "Hi " + emailDetails.TalentName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Here is some quick update for you!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("" + emailDetails.CompanyName + " have shortlisted you for the next round of interview. Client is yet to provide the interview slots, once the slots are provided you will get the email notification for the same.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("You can also keep track of the above by logging in to your Uplers Talent Solutions account.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (emailDetails.IsResetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetails.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetails.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetails.Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any questions, feel free to reach your TSC at " + emailDetails.TalentSuccessEmail + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetails.TalentEmail;
        //        string TOEMAILNAME = emailDetails.TalentName;

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };
        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetCCEmail(ccEmail, ccEmailName);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //#endregion

        //public void SendEmailForClientFeedbackSubmitToInternalTeam(Int64 ClientID, Int64 TalentID, Int64 HRID, string FeedbackType)
        //{

        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);


        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HRID, ClientID, 0);

        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Binding
        //    if (FeedbackType == "Hire")
        //    {
        //        Subject = "Client has selected the candidate for the Hiring Request - " + emailForBookTimeSlotModel.HR_Number;
        //        BodyCustom = "Hello Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The Client has selected the " + emailForBookTimeSlotModel.TalentName + " for this " + emailForBookTimeSlotModel.HR_Number);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Below are the details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Client Name: " + emailForBookTimeSlotModel.clientName + "<br/>");
        //        sbBody.Append("HRID: " + emailForBookTimeSlotModel.HR_Number + "<br/>");
        //        sbBody.Append("Position: " + emailForBookTimeSlotModel.position + "<br/>");
        //        sbBody.Append("<a class='link' href='" + _configuration["NewAdminProjectURL"] + "allhiringrequest' target='_blank'>View Hiring Request</a>. <br/><br/>");
        //        sbBody.Append("Kindly, proceed with the further course of action.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }
        //    else if (FeedbackType == "NoHire")
        //    {
        //        Subject = "Client has Rejected the Shortlisted Talent - " + emailForBookTimeSlotModel.HR_Number;
        //        BodyCustom = "Hello Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The Client has rejected the shortlisted Talent " + emailForBookTimeSlotModel.TalentName + " for this Hiring Request " + emailForBookTimeSlotModel.HR_Number);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Below are the details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Name: " + emailForBookTimeSlotModel.TalentName + "<br/>");
        //        sbBody.Append("Title of Talent: " + emailForBookTimeSlotModel.TypeofDeveloper + "<br/>");
        //        sbBody.Append("Status: " + emailForBookTimeSlotModel.talentStatusAfterClientSelection + "<br/>");
        //        sbBody.Append("Kindly, proceed with the further course of action.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }
        //    else if (FeedbackType == "OnHold")
        //    {
        //        Subject = "Client has put the Shortlisted Talent on hold";
        //        BodyCustom = "Hello Team,";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The Client has put the shortlisted Talent " + emailForBookTimeSlotModel.TalentName + " on hold for this Hiring Request " + emailForBookTimeSlotModel.HR_Number);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Below are the details:");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Talent Name: " + emailForBookTimeSlotModel.TalentName + "<br/>");
        //        sbBody.Append("Title of Talent: " + emailForBookTimeSlotModel.TypeofDeveloper + "<br/>");
        //        sbBody.Append("Status: " + emailForBookTimeSlotModel.talentStatusAfterClientSelection + "<br/>");
        //        sbBody.Append("Kindly, proceed with the further course of action.");
        //        sbBody.Append("<br/>");
        //        if (emailForBookTimeSlotModel.IsManaged)
        //        {
        //            sbBody.Append("Manage: Yes");
        //        }
        //        else
        //        {
        //            sbBody.Append("Self Manage: Yes");
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }
        //    #endregion


        //    string TOEMAIL = emailForBookTimeSlotModel.salesemailid;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.salesName;

        //    string ManagedEMAIL = "";
        //    string ManagedEMAILNAME = "";

        //    string SMEMAIL = "";
        //    string SMEMAILNAME = "";

        //    string CCEMAIL = "";
        //    string CCEMAILNAME = "";


        //    if (emailForBookTimeSlotModel.IsAdHoc)
        //    {
        //        string AdHocEMAIL = "";
        //        string AdHocEMAILNAME = "";

        //        AdHocEMAIL = emailDatabaseContentProvider.GetCCEmailIdValues("AdhocCCEmailIds");
        //        AdHocEMAILNAME = emailDatabaseContentProvider.GetCCEmailNameValues("AdhocCCEmailName");

        //        CCEMAIL += AdHocEMAIL;
        //        CCEMAILNAME += AdHocEMAILNAME;
        //    }

        //    MakeCCDetail ccDetails = MakeCCEmailDetails(emailForBookTimeSlotModel.HRSalesPersonID, false, true, HRID, TalentID);
        //    if (ccDetails != null && !string.IsNullOrEmpty(ccDetails.CCEmail) && !string.IsNullOrEmpty(ccDetails.CCEmailName))
        //    {
        //        CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccDetails.CCEmail : "," + ccDetails.CCEmail;
        //        CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccDetails.CCEmailName : "," + ccDetails.CCEmailName;
        //    }



        //    List<string> toemail = new List<string>() { TOEMAIL };
        //    List<string> toemailname = new List<string>() { TOEMAILNAME };
        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail();
        //    #endregion

        //}
        //public void SendEmailForClientFeedbackSubmit(Int64 ClientID, Int64 TalentID, Int64 HRID, string FeedbackType)
        //{
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HRID, ClientID, 0);
        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    Subject = "Thank you for submitting the feedback - " + emailForBookTimeSlotModel.HR_Number;
        //    if (FeedbackType == "Hire")
        //    {
        //        BodyCustom = "Hello " + emailForBookTimeSlotModel.clientName + ", ";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for submitting the feedback.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We are glad, you liked " + emailForBookTimeSlotModel.TalentName + " for this hiring request.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Your Account Manager will contact you in the next 24 hrs to discuss the further course of action.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Meanwhile you can share feedback on overall experience with Uplers Talent Solutions.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }
        //    else if (FeedbackType == "NoHire")
        //    {
        //        BodyCustom = "Hello " + emailForBookTimeSlotModel.clientName + ", ";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for submitting the feedback.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We are so sorry, our first find did not fit your hiring request. We would like to understand more about your requirements in order to meet your expectations.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We will analyse your requirements along with feedback and get back to you within the next 24 hrs.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Mean while you can contact your account manager on " + emailForBookTimeSlotModel.ManagerEmail + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }
        //    else if (FeedbackType == "OnHold")
        //    {
        //        BodyCustom = "Hello " + emailForBookTimeSlotModel.clientName + ", ";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for submitting the feedback.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We are so sorry, you are not confident about our first choice for you. But don't worry, we appreciate you giving us detailed feedback and we will analyse it to meet your expectations.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Our account manager will contact you within the next 24 hrs to understand it further.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }

        //    string TOEMAIL = emailForBookTimeSlotModel.ClientEmail;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.clientName;

        //    List<string> toemail = new List<string>() { TOEMAIL };
        //    List<string> toemailname = new List<string>() { TOEMAILNAME };
        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail(false, true);
        //    #endregion
        //}
        //public void SendEmailForClientFeedbackSubmitToTalent(Int64 ClientID, Int64 TalentID, Int64 HRID, string FeedbackType)
        //{
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HRID, ClientID, 0);
        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();


        //    if (FeedbackType == "Hire")
        //    {
        //        Subject = "Congratulations " + emailForBookTimeSlotModel.TalentName + " your profile is selected - " + emailForBookTimeSlotModel.HR_Number;
        //        BodyCustom = "Hello " + emailForBookTimeSlotModel.TalentName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Congratulations! The client has shared his feedback, selected your profile and is ready to hire you.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Your Talent Success Coach will contact you in the next 24 hrs to discuss the further course of action.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Meanwhile you can share feedback on overall experience with Uplers Talent Solutions.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }
        //    else if (FeedbackType == "NoHire")
        //    {
        //        Subject = "Sorry " + emailForBookTimeSlotModel.TalentName + " your profile didn’t match client’s requirements - " + emailForBookTimeSlotModel.HR_Number;
        //        BodyCustom = "Hello " + emailForBookTimeSlotModel.TalentName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for applying to the Hiring Request " + emailForBookTimeSlotModel.HR_Number + " placed by our client " + emailForBookTimeSlotModel.clientName + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We are sorry to inform you that your profile did not fit the client’s hiring request. But this doesn’t mean you stop applying!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Do check the incoming notifications for new Hiring Requests and keep applying for the ones which interest you.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("In case you need guidance on how to apply for the hiring requests, crack interviews and increase your hiring chances - contact the Talent Success Coach at " + emailForBookTimeSlotModel.TalentSuccessEmail + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }
        //    else if (FeedbackType == "OnHold")
        //    {
        //        Subject = "Hey " + emailForBookTimeSlotModel.TalentName + " you are in queue";
        //        BodyCustom = "Hello " + emailForBookTimeSlotModel.TalentName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for applying to the Hiring Request - " + emailForBookTimeSlotModel.HR_Number + " placed by our Client - " + emailForBookTimeSlotModel.clientName + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("The client has shared a positive response about your profile. And because the client is currently reviewing a few more candidates along with yours, we request you to wait for a response within 5 business days. In case of any delays in the response time, we’ll update you.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("So, hold on, we’ll keep you posted on the same.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Meanwhile, do check incoming notifications for new Hiring Requests and keep applying for the ones that interest you.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("In case you need guidance on how to apply for new hiring requests, crack interviews and increase your hiring chances - contact the Talent Success Coach at " + emailForBookTimeSlotModel.TalentSuccessEmail + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");
        //    }

        //    string TOEMAIL = emailForBookTimeSlotModel.TalentEmail;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.TalentName;

        //    List<string> toemail = new List<string>() { TOEMAIL };
        //    List<string> toemailname = new List<string>() { TOEMAILNAME };

        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail();
        //    #endregion
        //}
        //public void SendEmailNotificationForAssociateclient(Int64 TalentID, long HiringRequest_ID)
        //{
        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();


        //    #region Variable
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HiringRequest_ID, 0, 0);


        //    Subject = "Congratulations, Your Profile has been selected - " + emailForBookTimeSlotModel.HR_Number;
        //    BodyCustom = "Hello " + emailForBookTimeSlotModel.TalentName + ",";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/> Congratulations! Uplers expert team has selected your profile for a new hiring request." + " <br/><br/>");
        //    sbBody.Append("Please click below link and login to view the hiring request and accept the same to move ahead into the cadre." + " <br/>");
        //    if (emailForBookTimeSlotModel.IsResetPassword)
        //    {
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<a href='" + _configuration["FrontOfficeUrl"].ToString().Trim() + "OpenPositionHiringRequest/OpenPositionHiringRequest'>Click to View Hiring Request details.</a>" + " <br/><br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("LoginID : " + emailForBookTimeSlotModel.Username);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //    }
        //    else
        //    {
        //        sbBody.Append("<br/>");
        //        //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("T") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt((emailForBookTimeSlotModel.Username == null ? String.Empty : emailForBookTimeSlotModel.Username))) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt((emailForBookTimeSlotModel.Password == null ? String.Empty : emailForBookTimeSlotModel.Password))) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //        sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailForBookTimeSlotModel.Username == null ? String.Empty : emailForBookTimeSlotModel.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailForBookTimeSlotModel.Password == null ? String.Empty : emailForBookTimeSlotModel.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("LoginID : " + emailForBookTimeSlotModel.Username);
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Once you accept this request, your profile will be shared with the client and he/she would review the same. If you get shortlisted, an interview will be scheduled with the client." + " <br/><br/>");
        //    sbBody.Append("If you have any questions, feel free to reach your TSC (" + emailForBookTimeSlotModel.TalentSuccessEmail + ").");
        //    //sbBody.Append("Please email us <a href='mailto:bhuvan@uplers.com'>bhuvan@uplers.com</a>&nbsp;And&nbsp;<a href='mailto:soumya.s@uplers.in'>soumya.s@uplers.in</a></br>");
        //    sbBody.Append("<br/>");

        //    sbBody.Append("<br/> Thanks" + " <br/>");
        //    sbBody.Append("Uplers Talent Solutions Team" + " <br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    string TOEMAIL = "";
        //    string TOEMAILNAME = emailForBookTimeSlotModel.TalentName;
        //    if (!string.IsNullOrEmpty(TOEMAIL))
        //    {
        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion
        //    }
        //}
        //public void SendEmailtoClientDirectPlacement(long? contactid, long hrid, long talentid)
        //{

        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Variable
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(talentid, 0, 0, false, hrid, contactid.Value, 0);

        //    List<GenContactPointofContact> contactPointofContact = emailDatabaseContentProvider.getPointOfContact(contactid);

        //    Subject = "Congratulations, we found a right match for your hiring request - " + emailForBookTimeSlotModel.HR_Number;
        //    BodyCustom = "Hello " + emailForBookTimeSlotModel.clientName + ",";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/> Thank you for your patience in allowing us to find the right talent for you." + " <br/><br/>");
        //    sbBody.Append("Here is a shortlisted talent for you " + " <br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Please review the talent profile and provide some slots to book an interview." + " <br/><br/>");
        //    sbBody.Append("Here is a shortlisted talent for you <a class='link' href='" + _configuration["FrontOfficeUrl"] + "ShortListed/ShortListedTalent?HiringRequest_ID=" + CommonLogic.Encrypt(Convert.ToString(hrid)) + "&Role_ID=" + CommonLogic.Encrypt(emailForBookTimeSlotModel.RoleID.ToString()) + "' target='_blank'>Review the Talent Profile.</a><br/><br/>");
        //    string pointofcontact = "";

        //    if (contactPointofContact.Count() != 0)
        //    {
        //        for (int i = 0; i < contactPointofContact.Count(); i++)
        //        {
        //            int? id = contactPointofContact[i].UserId;
        //            UsrUser Userdetail = emailDatabaseContentProvider.getUserObject(id.Value);
        //            if (Userdetail != null)
        //            {
        //                pointofcontact += "{" + Userdetail.FullName + ", " + Userdetail.EmailId + "}";
        //            }
        //        }
        //    }
        //    sbBody.Append("If you need any help, please feel free to contact your point of contact (" + pointofcontact + ").");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/> Thanks" + " <br/>");
        //    sbBody.Append("Uplers Talent Solutions Team" + " <br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    string TOEMAIL = emailForBookTimeSlotModel.ClientEmail;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.clientName;
        //    if (!string.IsNullOrEmpty(TOEMAIL))
        //    {
        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail(false, true);
        //        #endregion
        //    }
        //}
        //public void SendEmailForTalentShowtoclientbySalesTeam(long TalentID, long HRID)
        //{
        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Variable
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, 0, false, HRID, 0, 0);

        //    Subject = "Congratulations, we found a right match for your hiring request - " + emailForBookTimeSlotModel.HR_Number;

        //    BodyCustom = "Hello " + emailForBookTimeSlotModel.clientName + ",";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Thank you for your patience in allowing us to find the right talent for you.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Here is a shortlisted talent for you.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("HRID: " + emailForBookTimeSlotModel.HR_Number + "<br/>");
        //    sbBody.Append("Position: " + emailForBookTimeSlotModel.position + "<br/>");
        //    sbBody.Append("Talent Name: " + emailForBookTimeSlotModel.TalentName + "<br/>");
        //    sbBody.Append("<br/>");
        //    if (emailForBookTimeSlotModel.IsResetPassword)
        //    {
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "ShortListed/ShortListedTalent?HiringRequest_ID=" + CommonLogic.Encrypt(Convert.ToString(HRID)) + "&Role_ID=" + CommonLogic.Encrypt(emailForBookTimeSlotModel.RoleID.ToString()) + "' target='_blank'>Review the Talent Profile.</a>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("LoginID : " + emailForBookTimeSlotModel.Username);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //    }
        //    else
        //    {
        //        sbBody.Append("<br/>");
        //        //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailForBookTimeSlotModel.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailForBookTimeSlotModel.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //        sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailForBookTimeSlotModel.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailForBookTimeSlotModel.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("LoginID : " + emailForBookTimeSlotModel.Username);
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Please review the talent profile and provide some slots to book an interview. This would allow us to schedule an interview with the selected talent.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("If you need any help, please feel free to contact your point of contact on " + emailForBookTimeSlotModel.TalentSuccessEmail + ".");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");

        //    sbBody.Append("Thanks");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    string TOEMAIL = emailForBookTimeSlotModel.ClientEmail;
        //    string TOEMAILNAME = emailForBookTimeSlotModel.clientName;

        //    if (!string.IsNullOrEmpty(TOEMAIL))
        //    {
        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail(false, true);
        //        #endregion
        //    }
        //}
        //public bool SendEmailNotificationForLogIn(string UserName, string Password, string ClientName, string RoleNames, Int64 ClientID)
        //{
        //    bool emailsent = false;

        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();
        //    string SalesPerson = "Sales Person";
        //    string SalesPersonEmail = "Sales Person";


        //    #region Variable
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(0, 0, 0, false, 0, ClientID, 0);


        //    Subject = "Welcome To Uplers Talent Solutions";
        //    BodyCustom = "Hey " + ClientName + ",";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("" + SalesPerson + " has invited you to Join Uplers Talent Solutions and shortlist the right fit " + RoleNames + " from the pool of 100+ vetted talent.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("All you need to do is set a new password to get started.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Login ID: " + UserName);
        //    sbBody.Append("<br/>");
        //    //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt(UserName)) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt(Password)) + "' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //    sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailForBookTimeSlotModel.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailForBookTimeSlotModel.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    if (emailForBookTimeSlotModel.IsResetPassword)
        //    {
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Once you have set your password, you can <a class='link' href='" + _configuration["FrontOfficeUrl"] + "' target='_blank'>log in</a> here anytime.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("LoginID : " + UserName);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //    }
        //    else
        //    {
        //        sbBody.Append("<br/>");
        //        //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt(UserName)) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //        sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailForBookTimeSlotModel.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailForBookTimeSlotModel.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("LoginID : " + UserName);
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("If you have any questions for " + SalesPerson + ", you can reply on this email " + SalesPersonEmail + ". ");
        //    sbBody.Append(" Alternatively, feel free to contact our customer success team <u>talent@uplers.com</u> anytime.");
        //    sbBody.Append(" We’ll ensure to respond to you at the earliest.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Let's connect");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    List<string> toemail = new List<string>() { UserName };
        //    List<string> toemailname = new List<string>() { ClientName };

        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail(false, true);
        //    emailsent = true;
        //    #endregion


        //    return emailsent;

        //}
        //public bool SendEmailForHRCreationtoclient(long hrid, sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact_Result)
        //{
        //    bool emailsent = false;

        //    string Subject = "";
        //    string BodyCustom = "";
        //    StringBuilder sbBody = new StringBuilder();


        //    #region Variable
        //    EmailForBookTimeSlotModel emailForBookTimeSlotModel = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailForBookTimeSlotModel = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(0, 0, 0, false, hrid, 0, 0);

        //    if (sproc_Get_ContactPointofContact_Result != null)
        //    {
        //        emailForBookTimeSlotModel.ManagerEmail = sproc_Get_ContactPointofContact_Result.EmailID;
        //    }

        //    Subject = "New Hiring Request has been added on your behalf - " + emailForBookTimeSlotModel.HR_Number;
        //    BodyCustom = "Hello " + emailForBookTimeSlotModel.clientName + ",";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Your point of contact with UTS, " + emailForBookTimeSlotModel.ManagerEmail + " has added a new hiring request on your behalf into the system.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Feel free to review the same, while we are debriefing it further and finding a right talent for you.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Please click below link and login to view the hiring request: ");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("HR ID: " + emailForBookTimeSlotModel.HR_Number + "<br/>");
        //    sbBody.Append("Position: " + emailForBookTimeSlotModel.position);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<a href='" + _configuration["FrontOfficeUrl"] + "OpenPosition/OpenPosition'>Link of hiring request.</a>" + " <br/>");
        //    //if (IsresetPassword)
        //    //{
        //    //    sbBody.Append("<br/>");
        //    //    sbBody.Append("<a href='" + _configuration["FrontOfficeUrl"] + "OpenPosition/OpenPosition'>Link of hiring request.</a>" + " <br/>");
        //    //    //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //    //    sbBody.Append("<br/>");
        //    //    sbBody.Append("LoginID : " + Username);
        //    //    sbBody.Append("<br/>");
        //    //    sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //    //}
        //    //else
        //    //{
        //    //    sbBody.Append("<br/>");
        //    //    sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //    //    sbBody.Append("<br/>");
        //    //    sbBody.Append("LoginID : " + Username);
        //    //}
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("If you have any questions, feel free to reach your Sales Representative " + emailForBookTimeSlotModel.salesName + " on " + emailForBookTimeSlotModel.salesemailid);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Thanks");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers Talent Solutions Team");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");


        //    List<string> toemail = new List<string>() { emailForBookTimeSlotModel.ClientEmail };
        //    List<string> toemailname = new List<string>() { emailForBookTimeSlotModel.clientName };

        //    emailOperator.SetToEmail(toemail);
        //    emailOperator.SetToEmailName(toemailname);
        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    #region SendEmail
        //    if (!string.IsNullOrEmpty(Subject))
        //        emailOperator.SendEmail(false, true);
        //    emailsent = true;
        //    #endregion

        //    return emailsent;
        //}
        //public bool SendEmailAfterHRStatusToShortlisted(long TalentId, long HRID)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentId, 0, 0, false, HRID, 0, 0);

        //        Subject = "Congratulations, we found a right match for your hiring request - " + emailDetail.HR_Number;
        //        BodyCustom = "Hello " + emailDetail.TalentName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thank you for your patience in allowing us to find the right talent for you. ");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Here is a shortlisted talent for you.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("HR ID: " + emailDetail.HR_Number + "<br/>");
        //        sbBody.Append("Position: " + emailDetail.position + "<br/>");
        //        sbBody.Append("Talent Name: " + emailDetail.TalentName + "<br/>");
        //        sbBody.Append("<br/>");
        //        if (emailDetail.IsResetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "ShortListed/ShortListedTalent?HiringRequest_ID=" + CommonLogic.Encrypt(Convert.ToString(HRID)) + "&Role_ID=" + CommonLogic.Encrypt(emailDetail.RoleID.ToString()) + "' target='_blank'>Review the Talent Profile.</a>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetail.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetail.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetail.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetail.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetail.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetail.Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please review the talent profile and provide some slots to book an interview. This would allow us to schedule an interview with the selected talent.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you need any help, please feel free to contact your point of contact on " + emailDetail.TalentSuccessEmail + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetail.ClientEmail;
        //        string TOEMAILNAME = emailDetail.clientName;

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailToClientpreonboardingDone(long onBoardID, long TalentID, long HiringRequestID)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, onBoardID, false, HiringRequestID, 0, 0);

        //        Subject = "Thank you for initiating hiring process for talent " + emailDetail.TalentName + " and review pre onboarding details - " + emailDetail.HR_Number;
        //        BodyCustom = "Hi " + emailDetail.clientName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (emailDetail.IsResetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append("We are glad that you found  your preferred talent " + emailDetail.position + " role. Kindly <a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "OnBoardAddUpdate/ClientOnBoardView?OnBoard_ID=" + CommonLogic.Encrypt(onBoardID.ToString()) + "' target='_blank'>review</a> pre boarding details filled by the associated Account Manager on your behalf.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetail.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetail.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetail.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetail.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetail.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetail.Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Your Account Manager will contact you in the next 24-48 hrs to discuss tripartite agreement.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any questions, you can reply to this email. Alternatively, feel free to contact our support representative team at talentconnect@uplers.com anytime. We’ll ensure to respond to you at the earliest and resolve things concerning you.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetail.ClientEmail;
        //        string TOEMAILNAME = emailDetail.clientName;

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail(false, true);
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailToTalentpreonboardingisDone(long onBoardID, long TalentID, long HiringRequestID)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion
        //        string viewflagEncrypted = CommonLogic.Encrypt("0");
        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, onBoardID, false, HiringRequestID, 0, 0);

        //        Subject = "Congratulations!  " + emailDetail.CompanyName + " has initiated  pre onboarding process - " + emailDetail.HR_Number;
        //        BodyCustom = "Hi " + emailDetail.TalentName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We are glad to see you getting hired for  " + emailDetail.position + " hiring request.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (emailDetail.IsResetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append("" + emailDetail.CompanyName + " onboarding process has been initiated, <a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "TalentOnBoard/TalentOnBoards?viewflag=" + viewflagEncrypted + "&OnboardID=" + CommonLogic.Encrypt(onBoardID.ToString()) + "' target='_blank'>Click here</a> to review and share your consent on onboarding details.");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetail.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetail.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetail.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetail.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetail.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetail.Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("In case of any doubts or queries, feel free to connect with us. You can simply reply to this email or can also email us at talent@uplers.com. Our team will make sure to respond to you in the shortest possible time and resolve your queries.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");

        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetail.TalentEmail;
        //        string TOEMAILNAME = emailDetail.TalentName;

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailToClientForKickOff(long onBoardID, long TalentID, long HiringRequestID, Meeting meeting, DateTime scheduleTime, string TimeZone, DateTime dateTimeEnd)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, onBoardID, false, HiringRequestID, 0, 0);

        //        Subject = "Congratulations! You have successfully onboarded " + emailDetail.TalentName + " to your team - " + emailDetail.HR_Number;
        //        BodyCustom = "Hi " + emailDetail.clientName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Congratulations on the successful onboarding of " + emailDetail.TalentName + " in your team.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We are glad that you could find the right match for your " + emailDetail.position + " hiring request.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We have scheduled the kick-off meeting with " + emailDetail.TalentName + ". Below are the details of the kick-off meeting with the talent.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Kick-off Meeting Details");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Date & Time: " + scheduleTime.ToString() + " To " + dateTimeEnd.ToString());
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Link: " + meeting.join_url);
        //        //sbBody.Append("<br/>");
        //        //sbBody.Append("Pass Code: " + meeting.password + " (Optional)");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any concerns with the above, feel free to reach out to us by replying to this email. Alternatively, you can also email us at talentconnect@uplers.com. Our support team will reach out to you at the earliest and resolve any issues concerning you.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (emailDetail.IsResetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetail.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetail.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetail.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetail.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetail.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetail.Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetail.ClientEmail;
        //        string TOEMAILNAME = emailDetail.clientName;

        //        string Description = emailDatabaseContentProvider.GetPrgContactTimeZone(TimeZone);
        //        string TimeZoneTitleValue = string.Empty;

        //        var UTCStartTime = scheduleTime.ToUniversalTime();
        //        var UTCEndTime = dateTimeEnd.ToUniversalTime();

        //        DateTime NewStartTime;
        //        DateTime NewEndTime;
        //        if (!string.IsNullOrEmpty(Description))
        //        {
        //            TimeZoneTitleValue = Description.Split(')')[1].Trim();
        //            NewStartTime = TimeZoneInfo.ConvertTimeToUtc(scheduleTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneTitleValue));
        //            NewEndTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeEnd, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneTitleValue));
        //        }
        //        else
        //        {
        //            NewStartTime = scheduleTime;
        //            NewEndTime = dateTimeEnd;
        //        }

        //        StringBuilder str = new StringBuilder();
        //        var descriptionCalender = new StringBuilder();

        //        descriptionCalender.Append("Interview date & time: " + scheduleTime.ToString() + " " + TimeZone);
        //        descriptionCalender.Append("<br/>");
        //        descriptionCalender.Append("Interview Link: " + meeting.join_url);

        //        //if (!string.IsNullOrEmpty(meeting.password))
        //        //{
        //        //    descriptionCalender.Append("<br/>");
        //        //    descriptionCalender.Append("Pass Code : " + meeting.password + " (Optional)");
        //        //}

        //        str.AppendLine("BEGIN:VCALENDAR");
        //        str.AppendLine("PRODID:-//Schedule a KickOff");
        //        str.AppendLine("VERSION:2.0");
        //        str.AppendLine("METHOD:REQUEST");

        //        str.AppendLine("BEGIN:VEVENT");
        //        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", NewStartTime));
        //        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", NewEndTime));
        //        str.AppendLine("LOCATION: " + "Remote");
        //        str.AppendLine($"ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:{clientEmailName}");
        //        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //        str.AppendLine(string.Format("SUMMARY:{0}", "Kickoff Schedule : " + emailDetail.TalentName + " | " + emailDetail.position));
        //        str.AppendLine("BEGIN:VALARM");
        //        str.AppendLine("TRIGGER:-PT15M");
        //        str.AppendLine("ACTION:DISPLAY");
        //        str.AppendLine("DESCRIPTION:Reminder");
        //        str.AppendLine("END:VALARM");
        //        str.AppendLine("END:VEVENT");
        //        str.AppendLine("END:VCALENDAR");

        //        byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //        MemoryStream stream = new MemoryStream(byteArray);

        //        Attachment attach = new Attachment(stream, "invite.ics");

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());
        //            emailOperator.SetAttachment(new List<Attachment>() { attach });

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail(false, true);
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendEmailToTalentForKickOff(long onBoardID, long TalentID, long HiringRequestID, Meeting meeting, DateTime scheduleTime, string TimeZone, DateTime dateTimeEnd)
        //{
        //    try
        //    {
        //        bool emailsent = false;
        //        string Subject = "";
        //        string BodyCustom = "";
        //        StringBuilder sbBody = new StringBuilder();

        //        #region Variable
        //        EmailForBookTimeSlotModel emailDetail = new();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        #endregion

        //        emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentID, 0, onBoardID, false, HiringRequestID, 0, 0);

        //        Subject = "Congratulations! You are successfully on boarded with " + emailDetail.CompanyName + " - " + emailDetail.HR_Number;
        //        BodyCustom = "Hi " + emailDetail.TalentName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Congratulations on successfully completing the hiring process and getting on boarded to " + emailDetail.CompanyName + " for " + emailDetail.position + " hiring request.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We are glad to see you getting this opportunity. Your kick-off meeting with " + emailDetail.CompanyName + " is scheduled. Below are the details for your reference.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We have scheduled the kick-off meeting with " + emailDetail.TalentName + ". Below are the details of the kick-off meeting with the talent.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Date & Time: " + scheduleTime.ToString() + " To " + dateTimeEnd.ToString());
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Link: " + meeting.join_url);
        //        //sbBody.Append("<br/>");
        //        //sbBody.Append("Pass Code: " + meeting.password + " (Optional)");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Be ready for this kick-off and get started with your job at " + emailDetail.CompanyName + ".");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (emailDetail.IsResetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetail.Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetail.Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(emailDetail.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetail.Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(emailDetail.Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + emailDetail.Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any concerns with the above, feel free to reach out to us. You can simply reply to this email or can also email us at talent@uplers.com anytime. Our support team will get to you in the shortest time possible and resolve your queries.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");

        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = emailDetail.TalentEmail;
        //        string TOEMAILNAME = emailDetail.TalentName;

        //        StringBuilder str = new StringBuilder();
        //        var descriptionCalender = new StringBuilder();

        //        descriptionCalender.Append("Interview date & time: " + scheduleTime.ToString() + " " + TimeZone);
        //        descriptionCalender.Append("<br/>");
        //        descriptionCalender.Append("Interview Link: " + meeting.join_url);
        //        descriptionCalender.Append("<br/>");
        //        //descriptionCalender.Append("Pass Code : " + meeting.password + " (Optional)");

        //        //if (!string.IsNullOrEmpty(meeting.password))
        //        //{
        //        //    descriptionCalender.Append("<br/>");
        //        //    descriptionCalender.Append("Pass Code : " + meeting.password + " (Optional)");
        //        //}

        //        str.AppendLine("BEGIN:VCALENDAR");
        //        str.AppendLine("PRODID:-//Schedule a KickOff");
        //        str.AppendLine("VERSION:2.0");
        //        str.AppendLine("METHOD:REQUEST");

        //        str.AppendLine("BEGIN:VEVENT");
        //        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", scheduleTime));
        //        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", dateTimeEnd));
        //        str.AppendLine("LOCATION: " + "Remote");
        //        str.AppendLine($"ORGANIZER;CN=\"Uplers Talent Solutions\":mailto:{clientEmailName}");
        //        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));

        //        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", descriptionCalender.ToString()));
        //        str.AppendLine(string.Format("SUMMARY:{0}", "Kickoff Schedule : " + emailDetail.TalentName + " | " + emailDetail.position));
        //        str.AppendLine("BEGIN:VALARM");
        //        str.AppendLine("TRIGGER:-PT15M");
        //        str.AppendLine("ACTION:DISPLAY");
        //        str.AppendLine("DESCRIPTION:Reminder");
        //        str.AppendLine("END:VALARM");
        //        str.AppendLine("END:VEVENT");
        //        str.AppendLine("END:VCALENDAR");

        //        byte[] byteArray = Encoding.ASCII.GetBytes(str.ToString());
        //        MemoryStream stream = new MemoryStream(byteArray);

        //        Attachment attach = new Attachment(stream, "invite.ics");

        //        if (!string.IsNullOrEmpty(TOEMAIL))
        //        {
        //            List<string> toemail = new List<string>() { TOEMAIL };
        //            List<string> toemailname = new List<string>() { TOEMAILNAME };

        //            emailOperator.SetToEmail(toemail);
        //            emailOperator.SetToEmailName(toemailname);
        //            emailOperator.SetSubject(Subject);
        //            emailOperator.SetBody(sbBody.ToString());
        //            emailOperator.SetAttachment(new List<Attachment>() { attach });

        //            #region SendEmail
        //            if (!string.IsNullOrEmpty(Subject))
        //                emailOperator.SendEmail();
        //            #endregion
        //        }
        //        emailsent = true;
        //        return emailsent;
        //    }
        //    catch (Exception e) { return false; }
        //}
        //public bool SendClientEmailHRTypeChanged(string PreviousType, string CurrentType, long? HrID, long TalentId)
        //{

        //    string Subject = "";
        //    string BodyCustom = "";
        //    bool emailsent = false;
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Variable
        //    EmailForBookTimeSlotModel emailDetail = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentId, 0, 0, false, HrID.Value, 0, 0);


        //    Subject = "Type of HR Changed to " + CurrentType + emailDetail.HR_Number;
        //    BodyCustom = "Hello " + emailDetail.clientName + ",";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>Hope you are doing well." + " <br/><br/>");
        //    sbBody.Append("We would like to inform you that type of the HR has been changed from " + PreviousType + " to " + CurrentType + "<br/>");
        //    sbBody.Append("Please find below details for your reference:<br/>");
        //    sbBody.Append("HR Number: " + emailDetail.HR_Number + ". <br/>");
        //    sbBody.Append("Role: " + emailDetail.TalentRole + ". <br/>");
        //    sbBody.Append("Talent Name: " + emailDetail.TalentFirstName + ". <br/>");
        //    if (emailDetail.finalCost != null)
        //    {
        //        sbBody.Append("Talent Cost/expected CTC: " + emailDetail.finalCost + ". <br/>");
        //    }
        //    else
        //    {
        //        sbBody.Append("Talent Cost/expected CTC: NA. <br/>");
        //    }
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("If you need any help, please feel free to contact your point of contact.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/> Thanks" + " <br/>");
        //    sbBody.Append("Uplers Talent Solutions Team" + " <br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    string TOEMAIL = emailDetail.ClientEmail;
        //    string TOEMAILNAME = emailDetail.clientName;

        //    if (!string.IsNullOrEmpty(TOEMAIL))
        //    {
        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail(false, true);
        //        #endregion
        //    }
        //    emailsent = true;
        //    return emailsent;
        //}
        //public bool SendTalentEmailHRTypeChanged(string PreviousType, string CurrentType, long? HrID, long TalentId)
        //{

        //    string Subject = "";
        //    string BodyCustom = "";
        //    bool emailsent = false;
        //    StringBuilder sbBody = new StringBuilder();

        //    #region Variable
        //    EmailForBookTimeSlotModel emailDetail = new();
        //    EmailOperator emailOperator = new EmailOperator(_configuration);
        //    #endregion

        //    emailDetail = emailDatabaseContentProvider.GetEmailForBookTimeSlotModel(TalentId, 0, 0, false, HrID.Value, 0, 0);

        //    Subject = "Type of HR Changed to " + CurrentType + emailDetail.HR_Number;
        //    BodyCustom = "Hello Talent,";
        //    sbBody.Append(BodyCustom);
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<div style='width:100%'>");
        //    sbBody.Append("<br/>Hope you are doing well." + " <br/><br/>");
        //    sbBody.Append("We would like to inform you that type of the HR has been changed from " + PreviousType + " to " + CurrentType + "<br/>");
        //    sbBody.Append("Please find below details for your reference:<br/>");
        //    sbBody.Append("HR Number: " + emailDetail.HR_Number + ". <br/>");
        //    sbBody.Append("Client Name: " + emailDetail.clientName + ". <br/>");
        //    sbBody.Append("Role: " + emailDetail.TalentRole + ". <br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("If you need any help, please feel free to contact your point of contact.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/> Thanks" + " <br/>");
        //    sbBody.Append("Uplers Talent Solutions Team" + " <br/>");
        //    sbBody.Append("<hr>");
        //    sbBody.Append("We are committed to your privacy. ");
        //    sbBody.Append("You may unsubscribe from these communications at any time. ");
        //    sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //    sbBody.Append("</div>");

        //    string TOEMAIL = emailDetail.TalentEmail;
        //    string TOEMAILNAME = emailDetail.TalentName;

        //    if (!string.IsNullOrEmpty(TOEMAIL))
        //    {
        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion
        //    }
        //    emailsent = true;
        //    return emailsent;
        //}
        //public string SendEmailForReplacementToClient(long? ReplacementID, long TalentID, long ClientID, long HRID)
        //{
        //    try
        //    {
        //        string Subject = "", BodyCustom = "", EngagementID = "", Last_Working_Day = "", talentEmail = "", SalesPersonName = "", salesPersonEmail = "", companyName = "", ClientName = "", ClientEmail = "", TalentName = "";
        //        StringBuilder sbBody = new StringBuilder();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        bool IsresetPassword = false;
        //        string Username = "", Password = "";

        //        GenTalent talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == TalentID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talent).Reload();
        //        if (talent != null)
        //        {
        //            talentEmail = talent.EmailId;
        //            TalentName = talent.Name;
        //        }

        //        GenContact contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ClientID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(contact).Reload();
        //        if (contact != null)
        //        {
        //            ClientEmail = contact.EmailId;
        //            ClientName = contact.FullName;
        //            IsresetPassword = Convert.ToBoolean(contact.IsResetPassword);
        //            Username = contact.EmailId;
        //            //Password = contact.FirstName.ToLower() + "@123";
        //            Password = "Uplers@123";

        //        }

        //        GenOnBoardTalentsReplacementDetail talents_ReplacementDetails = _talentConnectAdminDBContext.GenOnBoardTalentsReplacementDetails.Where(x => x.Id == ReplacementID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talents_ReplacementDetails).Reload();
        //        if (talents_ReplacementDetails != null)
        //        {
        //            var OnboardId = talents_ReplacementDetails.OnboardId;
        //            Last_Working_Day = Convert.ToString(talents_ReplacementDetails.LastWorkingDay);
        //            GenOnBoardTalent onBoardTalents = _talentConnectAdminDBContext.GenOnBoardTalents.Where(x => x.Id == OnboardId).FirstOrDefault();
        //            if (onBoardTalents != null)
        //            {
        //                EngagementID = onBoardTalents.EngagemenId;
        //            }
        //        }
        //        GenSalesHiringRequest _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == HRID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(_SalesHiringRequest).Reload();
        //        if (_SalesHiringRequest != null)
        //        {
        //            var UsersDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => new
        //            {
        //                FullName = x.FullName,
        //                EmailId = x.EmailId
        //            }).FirstOrDefault();

        //            SalesPersonName = UsersDetails.FullName;
        //            salesPersonEmail = UsersDetails.EmailId;
        //        }

        //        Subject = "Talent Replacement process has initiated.";
        //        BodyCustom = "Hi " + ClientName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Our Operation team has initiated the replacement process for talent " + TalentName + ", and the last working day of talent will be " + Last_Working_Day + ".");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Soon our ops team will share the new talent profiles with you as per your requirement.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("If you have any questions, you can reply to this email " + salesPersonEmail + ". Alternatively, feel free to contact our customer success team at talent@uplers.com anytime. We’ll ensure to respond to you at the earliest.");
        //        if (IsresetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = ClientEmail;
        //        string TOEMAILNAME = ClientName;

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail(false, true);
        //        #endregion
        //        return "success";

        //    }
        //    catch (Exception e) { return e.Message; }
        //}
        //public string SendEmailForReplacementToTalent(long? ReplacementID, long TalentID, long ClientID, long HRID)
        //{
        //    try
        //    {
        //        string Subject = "", BodyCustom = "", TalentSuccessEmail = "", EngagementID = "", Last_Working_Day = "", talentEmail = "",
        //            SalesPersonName = "", salesPersonEmail = "", companyName = "", ClientName = "", ClientEmail = "", TalentName = "",
        //            HR_Number = "";
        //        StringBuilder sbBody = new StringBuilder();
        //        EmailOperator emailOperator = new EmailOperator(_configuration);

        //        bool IsresetPassword = false;
        //        string Username = "", Password = "";

        //        GenTalent talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == TalentID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talent).Reload();
        //        GenContact contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ClientID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(contact).Reload();

        //        GenCompany comany = _talentConnectAdminDBContext.GenCompanies.Where(y => y.Id == contact.CompanyId).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(comany).Reload();
        //        if (talent != null)
        //        {
        //            talentEmail = talent.EmailId;
        //            TalentName = talent.Name;
        //        }


        //        if (contact != null)
        //        {
        //            ClientEmail = contact.EmailId;
        //            ClientName = contact.FullName;
        //            IsresetPassword = Convert.ToBoolean(contact.IsResetPassword);
        //            Username = contact.EmailId;
        //            //Password = contact.FirstName.ToLower() + "@123";
        //            Password = "Uplers@123";


        //        }

        //        GenOnBoardTalentsReplacementDetail talents_ReplacementDetails = _talentConnectAdminDBContext.GenOnBoardTalentsReplacementDetails.Where(x => x.Id == ReplacementID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talents_ReplacementDetails).Reload();
        //        if (talents_ReplacementDetails != null)
        //        {
        //            var OnboardId = talents_ReplacementDetails.OnboardId;
        //            Last_Working_Day = Convert.ToString(talents_ReplacementDetails.LastWorkingDay);
        //            GenOnBoardTalent onBoardTalents = _talentConnectAdminDBContext.GenOnBoardTalents.Where(x => x.Id == OnboardId).FirstOrDefault();
        //            //_talentConnectAdminDBContext.Entry(onBoardTalents).Reload();
        //            if (onBoardTalents != null)
        //            {
        //                EngagementID = onBoardTalents.EngagemenId;
        //            }
        //        }
        //        GenSalesHiringRequest _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == HRID).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(_SalesHiringRequest).Reload();
        //        if (_SalesHiringRequest != null)
        //        {
        //            var UserDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => new
        //            {
        //                FullName = x.FullName,
        //                EmailId = x.EmailId
        //            }).FirstOrDefault();

        //            SalesPersonName = UserDetails.FullName;
        //            salesPersonEmail = UserDetails.EmailId;
        //            HR_Number = _SalesHiringRequest.HrNumber;
        //        }

        //        GenTalentPointofContact talentPointofContact = _talentConnectAdminDBContext.GenTalentPointofContacts.Where(x => x.TalentId == TalentID).OrderBy(x => x.Id).FirstOrDefault();
        //        //_talentConnectAdminDBContext.Entry(talentPointofContact).Reload();
        //        if (talentPointofContact != null)
        //        {
        //            UsrUser Userdetail = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == talentPointofContact.UserId).FirstOrDefault();
        //            //_talentConnectAdminDBContext.Entry(Userdetail).Reload();
        //            if (Userdetail != null)
        //            {

        //                TalentSuccessEmail = Userdetail.EmailId;
        //            }
        //        }

        //        Subject = "Talent Replacement " + HR_Number + "";
        //        BodyCustom = "Hi " + TalentName + ",";
        //        sbBody.Append(BodyCustom);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Hope you are doing well.");
        //        sbBody.Append("<div style='width:100%'>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please be informed that you have been replaced for the below mentioned HR ID.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("HR Number: " + HR_Number);
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We Would also like to inform you that we will still consider your profile for any upcoming requirements.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Please feel free to connect with your POC in case of any concerns.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Thanks");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Uplers Talent Solutions Team");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='" + _configuration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
        //        sbBody.Append("</div>");

        //        string TOEMAIL = talentEmail;
        //        string TOEMAILNAME = TalentName;

        //        List<string> toemail = new List<string>() { TOEMAIL };
        //        List<string> toemailname = new List<string>() { TOEMAILNAME };

        //        emailOperator.SetToEmail(toemail);
        //        emailOperator.SetToEmailName(toemailname);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());

        //        #region SendEmail
        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail();
        //        #endregion
        //        return "success";

        //    }
        //    catch (Exception e) { return e.Message; }
        //}
        ///*public bool SetPasswordSendEmail( string invitingUserName, string invitingIserEmailId, string Designation)
        //{
        //    try
        //    {
        //        var reactClientPortalURL = _configuration["ReactClientPortalURL"];
        //        var varSMTPEmailName = _configuration["app_settings:SMTPEmailName"];
        //        EmailOperator emailOperator = new EmailOperator(_configuration);
        //        string Subject = "";

        //        StringBuilder sbBody = new();
        //        Subject = "Welcome to Uplers! Let's Get Your First Job Posted on Uplers";
        //        sbBody.Append("<div style='width:100%'>");

        //        if (varContactDetail != null)
        //        {
        //            sbBody.Append("Hello " + varContactDetail.FullName + ",");
        //        }

        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Welcome to Uplers! We're thrilled you're here to discover top talent from India.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Please set up your password to start posting jobs, exploring candidates, and managing your recruitment with ease.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        if (varContactDetail != null)
        //        {
        //            sbBody.Append("<a class='link' href='" + reactClientPortalURL + "setPassword?type=" + MyExtensions.Encrypt("IC") + "&Username=" + Uri.EscapeDataString(MyExtensions.Encrypt(varContactDetail.Username)) + "&expDate=" + Uri.EscapeDataString(MyExtensions.Encrypt(Convert.ToString(DateTime.Now))) + "' target='_blank' style='color:#232323;font-style:normal;font-weight:700;text-transform:uppercase;border:0;background:#FFDA30;padding:0 20px;font-size:14px;display:inline-block;text-align:center;border-radius:27px;line-height:40px;text-decoration:none;' >Click Here</a>");
        //        }

        //        sbBody.Append("<br/>");
        //        sbBody.Append("Why post a job on Uplers?");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<ul><li>Pre-vetted talent matched accurately as per your job requirement </li><li>Save your time and concentrate on the right talents.</li><li>Receive comprehensive reports and video screened responses for your job.</li><li>Dedicated support to assist you every step of the way.</li></ul>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Ready to find the perfect fit for your team? Don't wait any longer! Start by posting your job and get matched with the top talent on Uplers.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"If you need help or have questions, please reach out to our executive {invitingUserName} at {invitingIserEmailId}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("We look forward to supporting your hiring journey!");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Need assistance or have questions? We're just an email away.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Your dedicated POC is just a click away if you have questions or require further assistance. [Contact us] (client@uplers.com), and we'll make sure your experience with Uplers is exceptional.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Thank you for choosing Uplers for your recruiting needs.");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"Warm regards,");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"{invitingUserName}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"{Designation}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("Website: https://www.uplers.com/");
        //        sbBody.Append("<br/>");
        //        sbBody.Append($"{invitingIserEmailId}");
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<hr>");
        //        sbBody.Append("We are committed to your privacy. ");
        //        sbBody.Append("You may unsubscribe from these communications at any time. ");
        //        sbBody.Append("For more information, check out our <a class='link' href='https://talent.uplers.com/privacy-policy/' target='_blank'>Privacy Policy</a>.</br><br/>");
        //        sbBody.Append("</div>");

        //        List<string> toEmail = new List<string>
        //        {
        //            varContactDetail.EmailId
        //        };

        //        List<string> toEmailName = new List<string>
        //        {
        //            varContactDetail.EmailId
        //        };

        //        //string subject = "Uplers Talent Solutions Account Setup Request";

        //        #region SetParam
        //        emailOperator.SetToEmail(toEmail);
        //        emailOperator.SetToEmailName(toEmailName);
        //        emailOperator.SetSubject(Subject);
        //        emailOperator.SetBody(sbBody.ToString());
        //        #endregion

        //        if (!string.IsNullOrEmpty(Subject))
        //            emailOperator.SendEmail(false, true);

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}*/
        //#region Emails for Client Journey
        //#region Send SLA Update emails to Client & Internal Team
        //public void SendSLAUpdateEmailToClient(Sproc_Get_SLAUpdateDetails_ForEmail_Result sLAUpdateEmailViewModel)
        //{
        //    string Subject = "";
        //    StringBuilder sbBody = new StringBuilder();

        //    EmailOperator emailOperator = new EmailOperator(_configuration);

        //    Subject = "Important Update: Changes to Your Service Level Agreement with Uplers";

        //    sbBody.Append($"Dear {sLAUpdateEmailViewModel.ClientName},");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("I'm reaching out to inform you of an important update to the Service Level Agreement (SLA) for your current project/contract. We have made the following changes to align more closely with your project's unique requirements and to ensure an optimal experience.");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Here's a summary of the updates:");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"<b>Old SLA Details: {sLAUpdateEmailViewModel.OldSlaDetails} </b>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"<b>New SLA Details: {sLAUpdateEmailViewModel.UpdatedSlaDetails} </b>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"Our priority is to make your hiring journey with Uplers as efficient and effective as possible.<b> Please review the updated SLA at <a class='link' href='{_configuration["ClientPortalHomeURL"]}' target='_blank'>View on dashboard</a> and feel free to reach out to your POC ({sLAUpdateEmailViewModel.PocEmail}) with any questions or concerns. </b>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("<br/>");

        //    sbBody.Append("Best regards,");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"{sLAUpdateEmailViewModel.PocName}");
        //    sbBody.Append("<br/>");
        //    sbBody.Append($"{sLAUpdateEmailViewModel.PocDesignation}");
        //    sbBody.Append("<br/>");
        //    sbBody.Append("Uplers");
        //    sbBody.Append("<br/>");

        //    emailOperator.SetToEmail(new List<string> { sLAUpdateEmailViewModel.ToClientEmail });
        //    emailOperator.SetToEmailName(new List<string> { sLAUpdateEmailViewModel.ToClientEmailName });

        //    emailOperator.SetSubject(Subject);
        //    emailOperator.SetBody(sbBody.ToString());

        //    emailOperator.SendEmail(false, true);
        //}
        //#endregion
        //#endregion
        //#endregion

        //#region Private Methods
        ////private void GatherCCEmail(bool isCCEmail)
        ////{
        ////    if (isCCEmail)
        ////    {
        ////        CCEMAIL = ccEmail.Split(",").ToList();
        ////    }
        ////    else
        ////    {
        ////        CCEMAILNAME = ccEmailName.Split(",").ToList();
        ////    }
        ////}
        //private RescheduleInterviewEmailContent GetEmailContentForClient(int type, int status, string RequestedBy, int roundNumber, EmailForBookTimeSlotModel emailForBookTimeSlotModel, string timezone)
        //{
        //    StringBuilder sbBody = new StringBuilder();
        //    RescheduleInterviewEmailContent emailContent = new RescheduleInterviewEmailContent();

        //    bool IsresetPassword = false;
        //    string Username = "", Password = "";

        //    IsresetPassword = Convert.ToBoolean(emailForBookTimeSlotModel.IsResetPassword);
        //    Username = emailForBookTimeSlotModel.clientName;
        //    Password = emailForBookTimeSlotModel.Password;

        //    #region Case1: When ops team have confirmed interview slot
        //    //Mail to Client
        //    if (type == 2 && (status == 4 || status == 8))
        //    {
        //        sbBody.Append(string.Format("<p>Hello <span>##ClientName##,</span></p>"));
        //        sbBody.Append(string.Format("<p>Your interview is rescheduled for below Hiring Request.</p>"));
        //        sbBody.Append(string.Format("<strong>Here is the detail:</strong>"));
        //        sbBody.Append(string.Format("<table>"));
        //        sbBody.Append(string.Format("<tr><td>HRID:</td><td>##HRID##</td></tr>"));
        //        if (roundNumber > 1)
        //        {
        //            sbBody.Append(string.Format("<tr><td>Round:</td><td>##RoundNo##</td></tr>"));
        //        }
        //        sbBody.Append(string.Format("<tr><td>scheduleTime.ToString(\"dd/MM/yyyy\", CultureInfo.InvariantCulture) + \" \" +:</td><td>##Position##</td></tr>"));
        //        sbBody.Append(string.Format("<tr><td>Talent Name:</td><td>##TalentName##</td></tr>"));
        //        //sbBody.Append(string.Format("<tr><td>Interview date & time:</td><td>##Interviewdatetime##</td></tr>"));
        //        //sbBody.Append(string.Format("<tr><td>Interview date:</td><td>##Interviewdate##</td></tr>"));
        //        if (timezone.ToLower() == "ist")
        //        {
        //            sbBody.Append(string.Format("<tr><td>Interview date & time: </td><td>##Interviewdatetimeist##</td></tr>"));
        //        }
        //        else
        //        {
        //            sbBody.Append(string.Format("<tr><td>Interview date & time: </td><td>##Interviewdatetime##</td></tr>"));
        //            sbBody.Append(string.Format("<tr><td>Interview date & time (IST):</td><td>##Interviewdatetimeist##</td></tr>"));
        //        }

        //        sbBody.Append(string.Format("<tr><td>Interview Link:</td><td>##InterviewLink##</td></tr>"));
        //        //sbBody.Append(string.Format("<tr><td>Pass Code:</td><td>##PassCode##</td></tr>"));
        //        sbBody.Append(string.Format("</table>"));
        //        sbBody.Append(string.Format("<p>Please review the full profile before the interview in the app.</p>"));
        //        sbBody.Append(string.Format("Looking forward to seeing you 5 mins before the interview.If you have any questions related to the interview, feel free to reach your Account Manager for any help AM ##AMID##(##AMIDEmail##)"));
        //        sbBody.Append(string.Format("<br/>"));
        //        if (IsresetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString().Trim() + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString().Trim() + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(Username)) + "&Password=" + UrlEncoder.Default.Encode(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"].ToString() + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));
        //        emailContent.Subject = "Interview Re-Scheduled for ##TalentName## - ##HRID##";
        //    }
        //    #endregion

        //    #region Case2.2.1 or 2.2.2: When ops edit given slots(Requested by Talent)
        //    //Mail to client
        //    if (type == 1 && status == 1 && (RequestedBy == "Talent" || RequestedBy == "Client"))
        //    {
        //        sbBody.Append(string.Format("<p>Hi <span>##ClientName##,</span></p>"));
        //        sbBody.Append(string.Format("<p>Uplers Operation team has updated the interview slots and shared it with the Talent to confirm.</p>"));
        //        if (IsresetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append(string.Format("<p><a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientYourUpcomingInterview/ClientYourUpcomingInterview' target='_blank'>Click here</a>  to review the new slots for the interview.</p>"));
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt(Username)) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>If you have any questions, feel free to reach your AM ##AMID##(##AMIDEmail##).</p>"));
        //        sbBody.Append(string.Format("<br/>"));
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));
        //        emailContent.Subject = "Interview Slots Updated for ##TalentName## - ##HRID##";
        //    }
        //    #endregion

        //    #region Case3.3.1: When ops add fresh slots on behalf of client(Requested by Client Post talent acceptance of any slot)
        //    //Mail to Client
        //    if (type == 1 && status == 4 && RequestedBy == "Client")
        //    {
        //        sbBody.Append(string.Format("<p>Hello <span>##ClientName##,</span></p>"));
        //        sbBody.Append(string.Format("<p>On behalf of you, our Ops team has canceled your scheduled interview and shared the new interview slots with the Talent to confirm."));
        //        if (IsresetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append(string.Format("<p><a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientYourUpcomingInterview/ClientYourUpcomingInterview' target='_blank'>Click here</a> to review the new slots shared by our team for the interview."));
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt(Username)) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>If you have any questions, feel free to reach your AM ##AMID##(##AMIDEmail##).</p>"));
        //        sbBody.Append(string.Format("<br/>"));
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));
        //        emailContent.Subject = "Interview Slots Re Shared for ##TalentName## - ##HRID##";
        //    }
        //    #endregion

        //    #region Case3.3.2: When ops add fresh slots on behalf of client(Requested by Talent Post talent acceptance of any slot)
        //    //Mail to Client
        //    if (type == 1 && status == 4 && RequestedBy == "Talent")
        //    {
        //        sbBody.Append(string.Format("<p>Hello <span>##ClientName##,</span></p>"));
        //        sbBody.Append(string.Format("<p>Unfortunately, due to unavoidable circumstances, the ##TalentName## is not available on the scheduled interview slot and our Ops team has canceled your scheduled interview. Talent has shared some  new interview slots. So kindly login into the application to select your preferred interview slot & schedule the interview."));
        //        if (IsresetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append(string.Format("<p><a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientYourUpcomingInterview/ClientYourUpcomingInterview' target='_blank'>Click here</a> to review the new slots shared by Talent for the interview."));
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords ?type=" + CommonLogic.Encrypt("C") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt(Username)) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>If you have any questions, feel free to reach your AM ##AMID##(##AMIDEmail##).</p>"));
        //        sbBody.Append(string.Format("<br/>"));
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));
        //        emailContent.Subject = "Interview Reschedule Request by ##TalentName## - ##HRID##";
        //    }
        //    #endregion

        //    #region Case4: When ops team wants, that client to add new slots from frontend
        //    if (type == 3)
        //    {
        //        sbBody.Append(string.Format("<p>Hello <span>##ClientName##,</span></p>"));
        //        sbBody.Append(string.Format("<p>Your interview has been canceled, kindly login into the app and share new interview slots for talent to schedule an interview."));
        //        if (IsresetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            sbBody.Append(string.Format("<p><a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientYourUpcomingInterview/ClientYourUpcomingInterview' target='_blank'>Click here</a> to add new slots for the ##TalentName##.</p>"));
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt(Username)) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>If you have any questions, feel free to reach your AM ##AMID##(##AMIDEmail##).</p>"));
        //        sbBody.Append(string.Format("<br/>"));
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));
        //        emailContent.Subject = "Add New Interview Slots for ##TalentName## - ##HRID##";
        //    }
        //    #endregion

        //    emailContent.Content = sbBody.ToString();
        //    return emailContent;

        //}
        //private RescheduleInterviewEmailContent GetEmailContentForTalent(int type, int status, string RequestedBy, int roundNumber, EmailForBookTimeSlotModel emailForBookTimeSlotModel, string timezone)
        //{
        //    StringBuilder sbBody = new StringBuilder();
        //    RescheduleInterviewEmailContent emailContent = new RescheduleInterviewEmailContent();

        //    bool IsresetPassword = false;
        //    string Username = "", Password = "";

        //    IsresetPassword = Convert.ToBoolean(emailForBookTimeSlotModel.IsResetPasswordForTalent);
        //    Username = emailForBookTimeSlotModel.TalentEmail;
        //    Password = "Uplers@123";


        //    #region Case1: When ops team have confirmed interview slot
        //    //Mail to Talent
        //    if (type == 2 && (status == 4 || status == 8))
        //    {
        //        sbBody.Append(string.Format("<p>Hello <span>##TalentName##,</span></p>"));
        //        sbBody.Append(string.Format("<p>Your interview is rescheduled for below Hiring Request.</p>"));
        //        sbBody.Append(string.Format("<strong>Here is the detail:</strong>"));
        //        sbBody.Append(string.Format("<table>"));
        //        sbBody.Append(string.Format("<tr><td>HRID:</td><td>##HRID##</td></tr>"));
        //        if (roundNumber > 1)
        //        {
        //            sbBody.Append(string.Format("<tr><td>Round:</td><td>##RoundNo##</td></tr>"));
        //        }
        //        sbBody.Append(string.Format("<tr><td>Position:</td><td>##Position##</td></tr>"));
        //        sbBody.Append(string.Format("<tr><td>Talent Name:</td><td>##TalentName##</td></tr>"));
        //        //sbBody.Append(string.Format("<tr><td>Interview date & time:</td><td>##Interviewdatetime##</td></tr>"));
        //        //sbBody.Append(string.Format("<tr><td>Interview date:</td><td>##Interviewdate##</td></tr>"));
        //        if (timezone.ToLower() == "ist")
        //        {
        //            sbBody.Append(string.Format("<tr><td>Interview date & time: </td><td>##Interviewdatetimeist##</td></tr>"));
        //        }
        //        else
        //        {
        //            sbBody.Append(string.Format("<tr><td>Interview date & time: </td><td>##Interviewdatetime##</td></tr>"));
        //            sbBody.Append(string.Format("<tr><td>Interview date & time (IST):</td><td>##Interviewdatetimeist##</td></tr>"));
        //        }
        //        sbBody.Append(string.Format("<tr><td>Interview Link:</td><td>##InterviewLink##</td></tr>"));
        //        //sbBody.Append(string.Format("<tr><td>Pass Code:</td><td>##PassCode##</td></tr>"));
        //        sbBody.Append(string.Format("</table>"));
        //        sbBody.Append(string.Format("<p>Please review the full profile before the interview in the app.</p>"));
        //        sbBody.Append(string.Format("Looking forward to seeing you 5 mins before the interview.If you have any questions related to the interview, feel free to reach your Account Manager for any help AM ##AMID##(##AMIDEmail##)"));
        //        sbBody.Append(string.Format("<br/>"));
        //        if (IsresetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords ?type=" + CommonLogic.Encrypt("T") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt(Username)) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));
        //        emailContent.Subject = "Interview Re-Scheduled for ##ClientName## - ##HRID##";
        //    }
        //    #endregion

        //    #region Case2.2.1 or 2.2.2: When ops edit given slots(Requested by Talent)
        //    //Mail to Talent
        //    if (type == 1 && status == 1 && (RequestedBy == "Talent" || RequestedBy == "Client"))
        //    {
        //        sbBody.Append(string.Format("<p>Hello <span>##TalentName##,</span></p>"));
        //        sbBody.Append(string.Format("<p>Your interview slots have been changed, kindly login into the application to select your preferred interview slot & schedule the interview."));
        //        sbBody.Append(string.Format("<p><a class='link' href='" + _configuration["FrontOfficeUrl"] + "TalentYourupcomingInterview/TalentYourupcomingInterview' target='_blank'>Click here</a> to review the slots and book your interview.</p>"));
        //        if (IsresetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("T") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt(Username)) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>If you have any questions, feel free to reach your TSC ##PointOfContactName##(##PointOfContactEmail##).</p>"));
        //        sbBody.Append(string.Format("<br/>"));
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));
        //        emailContent.Subject = "Interview Slots Updated for ##ClientName## - ##HRID##";
        //    }
        //    #endregion

        //    #region Case3.3.1: When ops add fresh slots on behalf of client(Requested by Client Post talent acceptance of any slot)
        //    //Mail to Talent
        //    if (type == 1 && status == 4 && RequestedBy == "Client")
        //    {
        //        sbBody.Append(string.Format("<p>Hello <span>##TalentName##,</span></p>"));
        //        sbBody.Append(string.Format("<p>Client wants to reschedule the interview and has shared new interview slots, kindly login into the application to select your preferred interview slot & schedule the interview."));
        //        sbBody.Append(string.Format("<p><a class='link' href='" + _configuration["FrontOfficeUrl"] + "TalentYourupcomingInterview/TalentYourupcomingInterview' target='_blank'>Click here</a> to review the slots and book your interview."));
        //        if (IsresetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("T") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt(Username)) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>If you have any questions, feel free to reach your TSC ##PointOfContactName##(##PointOfContactEmail##).</p>"));
        //        sbBody.Append(string.Format("<br/>"));
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));
        //        emailContent.Subject = "Interview Re-Scheduled for ##ClientName## - ##HRID##";
        //    }
        //    #endregion

        //    #region Case3.3.2: When ops add fresh slots on behalf of client(Requested by Talent Post talent acceptance of any slot)
        //    //Mail to Talent
        //    if (type == 1 && status == 4 && RequestedBy == "Talent")
        //    {
        //        sbBody.Append(string.Format("<p>Hello <span>##TalentName##,</span></p>"));
        //        sbBody.Append(string.Format("<p>On behalf of you, our Ops team has canceled your scheduled interview and shared the new interview slots with the Client to confirm."));
        //        sbBody.Append(string.Format("<p><a class='link' href='" + _configuration["FrontOfficeUrl"] + "TalentYourupcomingInterview/TalentYourupcomingInterview' target='_blank'>Click here</a> to review the new slots shared by our team for the interview."));
        //        if (IsresetPassword)
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "login' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //            sbBody.Append("<br/>");
        //            sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
        //        }
        //        else
        //        {
        //            sbBody.Append("<br/>");
        //            //sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("T") + "&Username=" + CommonLogic.Encoder(CommonLogic.Encrypt(Username)) + "&Password=" + CommonLogic.Encoder(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<a class='link' href='" + _configuration["FrontOfficeUrl"] + "SetPasswords?type=" + MyExtensions.Encrypt("T") + "&Username=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(MyExtensions.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
        //            sbBody.Append("<br/>");
        //            sbBody.Append("LoginID : " + Username);
        //        }
        //        sbBody.Append("<br/>");
        //        sbBody.Append("<br/>");
        //        sbBody.Append(string.Format("<p>If you have any questions, feel free to reach your TSC ##PointOfContactName##(##PointOfContactEmail##).</p>"));
        //        sbBody.Append(string.Format("<br/>"));
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));
        //        emailContent.Subject = "Interview Rescheduled Request shared with ##ClientName## - ##HRID##";
        //    }
        //    #endregion

        //    emailContent.Content = sbBody.ToString();
        //    return emailContent;
        //}
        //private RescheduleInterviewEmailContent GetEmailContentForInternalTeam(int type, int status, string RequestedBy, int roundNumber, string timezone)
        //{
        //    StringBuilder sbBody = new StringBuilder();
        //    RescheduleInterviewEmailContent emailContent = new RescheduleInterviewEmailContent();

        //    #region Case2.2.2: Mail to Internal Team
        //    if (type == 1 && (status == 1 || status == 4) && (RequestedBy == "Client" || RequestedBy == "Talent"))
        //    {
        //        sbBody.Append(string.Format("<p>Hello Team, </p>"));
        //        if (RequestedBy == "Client")
        //        {
        //            sbBody.Append(string.Format("<p>The Interview slots has been updated on behalf of ##ClientName##.</p>"));
        //        }
        //        if (RequestedBy == "Talent")
        //        {
        //            sbBody.Append(string.Format("<p>The Interview slots has been updated on behalf of ##TalentName##.</p>"));
        //        }
        //        sbBody.Append(string.Format("<strong>Here is the detail:</strong>"));
        //        sbBody.Append(string.Format("<table style='width:100%;'>"));
        //        sbBody.Append(string.Format("<tr><td>HRID:</td><td>##HRID##</td></tr>"));
        //        if (roundNumber > 1)
        //        {
        //            sbBody.Append(string.Format("<tr><td>Round:</td><td>##RoundNo##</td></tr>"));
        //        }
        //        sbBody.Append(string.Format("<tr><td>Position:</td><td>##Position##</td></tr>"));
        //        sbBody.Append(string.Format("<tr><td>Talent Name:</td><td>##TalentName##</td></tr>"));
        //        sbBody.Append(string.Format("<tr><td>Client Email:</td><td>##ClientEmail##</td></tr>"));
        //        sbBody.Append(string.Format("<tr><td>Company Name:</td><td>##CompanyName##</td></tr>"));
        //        sbBody.Append(string.Format("<tr><td>Slots Detail:</td><td>##AllSlots##</td></tr>"));
        //        //sbBody.Append(string.Format("<tr><td>Interview Link:</td><td>##InterviewLink##</td></tr>"));
        //        //sbBody.Append(string.Format("<tr><td>Pass Code:</td><td>##PassCode##</td></tr>"));
        //        sbBody.Append(string.Format("</table>"));
        //        sbBody.Append(string.Format("<br/>"));
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));

        //        emailContent.Subject = "Interview Slots Re Shared for ##ClientName## - ##HRID##";
        //        emailContent.Content = sbBody.ToString();
        //    }
        //    #endregion

        //    #region Case1: Mail to Internal Team
        //    if (type == 2 && (status == 4 || status == 8))
        //    {
        //        //Mail to Internal Team
        //        sbBody.Clear();
        //        sbBody.Append(string.Format("<p>Hello Team, </p>"));
        //        if (RequestedBy == "Client")
        //        {
        //            sbBody.Append(string.Format("<p>The Interview slots has been updated on behalf of ##ClientName##.</p>"));
        //        }
        //        if (RequestedBy == "Talent")
        //        {
        //            sbBody.Append(string.Format("<p>The Interview slots has been updated on behalf of ##TalentName##.</p>"));
        //        }
        //        sbBody.Append(string.Format("<strong>Here is the detail:</strong>"));
        //        sbBody.Append(string.Format("<table>"));
        //        sbBody.Append(string.Format("<tr><td>HRID:</td><td>##HRID##</td></tr>"));
        //        if (roundNumber > 1)
        //        {
        //            sbBody.Append(string.Format("<tr><td>Round:</td><td>##RoundNo##</td></tr>"));
        //        }
        //        sbBody.Append(string.Format("<tr><td>Position:</td><td>##Position##</td></tr>"));
        //        sbBody.Append(string.Format("<tr><td>Talent Name:</td><td>##TalentName##</td></tr>"));
        //        sbBody.Append(string.Format("<tr><td>Client Email:</td><td>##ClientEmail##</td></tr>"));
        //        sbBody.Append(string.Format("<tr><td>Company Name:</td><td>##CompanyName##</td></tr>"));
        //        //sbBody.Append(string.Format("<tr><td>Interview date:</td><td>##Interviewdate##</td></tr>"));
        //        if (timezone.ToLower() == "ist")
        //        {
        //            sbBody.Append(string.Format("<tr><td>Interview date & time: </td><td>##Interviewdatetimeist##</td></tr>"));
        //        }
        //        else
        //        {
        //            sbBody.Append(string.Format("<tr><td>Interview date & time: </td><td>##Interviewdatetime##</td></tr>"));
        //            sbBody.Append(string.Format("<tr><td>Interview date & time (IST): </td><td>##Interviewdatetimeist##</td></tr>"));
        //        }
        //        sbBody.Append(string.Format("<tr><td>Interview Link:</td><td>##InterviewLink##</td></tr>"));
        //        //sbBody.Append(string.Format("<tr><td>Pass Code:</td><td>##PassCode##</td></tr>"));
        //        sbBody.Append(string.Format("</table>"));
        //        sbBody.Append(string.Format("<br/>"));
        //        sbBody.Append(string.Format("<p>Thank you</p>"));
        //        sbBody.Append(string.Format("<p>Uplers Talent Solutions Team</p>"));
        //        emailContent.Subject = "Interview Slots updated for ##Round## - ##HRID##";
        //        emailContent.Content = sbBody.ToString();
        //    }
        //    #endregion

        //    return emailContent;
        //}
        //private sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact(string param)
        //{
        //    return _talentConnectAdminDBContext.Set<sproc_Get_ContactPointofContact_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Get_ContactPointofContact, param)).AsEnumerable().FirstOrDefault();
        //}
        //#endregion
        //public class RescheduleInterviewEmailContent
        //{
        //    public string? Subject { get; set; }
        //    public string? Content { get; set; }
        //}

        //public class MakeCCDetail
        //{
        //    public string CCEmail { get; set; }
        //    public string CCEmailName { get; set; }
        //}
        //private List<Sproc_Get_Hierarchy_For_Email_Result> GetHierarchyForEmail(string param)
        //{
        //    return _talentConnectAdminDBContext.Set<Sproc_Get_Hierarchy_For_Email_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Hierarchy_For_Email, param)).ToList();
        //}

        //private MakeCCDetail MakeCCEmailDetails(long UserID, bool IsInternalCCNeeded = false, bool IsMatcherNeeded = false,
        //                                        long HRID = 0, long TalentID = 0, bool isAppSettingCCNeeded = true)
        //{
        //    MakeCCDetail makeCC = new MakeCCDetail();

        //    string CCEMAIL = "";
        //    string CCEMAILNAME = "";

        //    if (IsInternalCCNeeded)
        //    {
        //        string InternalCCEmailID = emailDatabaseContentProvider.GetCCEmailIdValues();
        //        string InternalCCEmailName = emailDatabaseContentProvider.GetCCEmailNameValues();

        //        CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? InternalCCEmailID : "," + InternalCCEmailID;
        //        CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? InternalCCEmailName : "," + InternalCCEmailName;
        //    }

        //    if (IsMatcherNeeded && HRID > 0 && TalentID > 0)
        //    {
        //        #region Get Matcher List From ATS
        //        string MatcherCCEMAIL = "", MatcherCCEMAILNAME = "";

        //        try
        //        {
        //            GetMatcherDataFromATS getMatcherDataFromATS = new GetMatcherDataFromATS();
        //            MatcherUserLists matcherUserLists = new MatcherUserLists();
        //            var genTalent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == TalentID).FirstOrDefault();
        //            if (genTalent != null)
        //            {
        //                getMatcherDataFromATS.HRID = HRID;
        //                getMatcherDataFromATS.ATS_TalentID = genTalent.AtsTalentId;
        //                matcherUserLists = SendATSData(getMatcherDataFromATS, HRID);
        //                List<User> users = new List<User>();
        //                if (matcherUserLists != null && matcherUserLists.users != null && matcherUserLists.users.Any())
        //                {
        //                    users = matcherUserLists.users;
        //                    if (users != null)
        //                    {
        //                        MatcherCCEMAIL = string.Join(",", users.Where(x => !string.IsNullOrWhiteSpace(x.email)).Select(x => x.email));
        //                        MatcherCCEMAILNAME = string.Join(",", users.Where(x => !string.IsNullOrWhiteSpace(x.name)).Select(x => x.name));
        //                    }
        //                }
        //                if (MatcherCCEMAIL != "" && MatcherCCEMAILNAME != "")
        //                {
        //                    CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? MatcherCCEMAIL : "," + MatcherCCEMAIL;
        //                    CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? MatcherCCEMAILNAME : "," + MatcherCCEMAILNAME;
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        #endregion
        //    }

        //    #region need to send sales person & the Hierarchy - New Changes as per Mehul sir suggested

        //    if (isAppSettingCCNeeded)
        //    {
        //        CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? ccEmail : "," + ccEmail;
        //        CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? ccEmailName : "," + ccEmailName;
        //    }

        //    if (UserID > 0)
        //    {
        //        var POCUserHierarchy = GetHierarchyForEmail(UserID.ToString());
        //        if (POCUserHierarchy != null && POCUserHierarchy.Any())
        //        {
        //            CCEMAIL += string.IsNullOrEmpty(CCEMAIL) ? "" : ",";
        //            CCEMAILNAME += string.IsNullOrEmpty(CCEMAILNAME) ? "" : ",";

        //            CCEMAIL += string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.EmailId)).Select(x => x.EmailId));
        //            CCEMAILNAME += string.Join(",", POCUserHierarchy.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Select(x => x.UserName));
        //        }
        //    }
        //    #endregion

        //    makeCC.CCEmail = CCEMAIL;
        //    makeCC.CCEmailName = CCEMAILNAME;
        //    return makeCC;
        //}

        //#region Function to send Data to ATS
        //public MatcherUserLists SendATSData(GetMatcherDataFromATS obj, long HRID)
        //{
        //    MatcherUserLists matcherUserLists = new MatcherUserLists();
        //    string Token = "";
        //    long loggedInUser_ID = SessionValues.LoginUserId;
        //    long APIRecordInsertedID = 0;
        //    string json = JsonConvert.SerializeObject(obj);
        //    if (obj != null)
        //    {
        //        var ATSAPIURL = _configuration["ATS_API_URL"].ToString();
        //        var client = new RestClient(ATSAPIURL); // Replace with your API endpoint
        //        var request = new RestRequest("hr/get-users", Method.Post); // Replace with your specific endpoint and HTTP method          

        //        // Convert the Person object to JSON and add it as the request body
        //        request.AddJsonBody(json);
        //        request.AddHeader("Content-Type", "application/json");
        //        request.AddHeader("Authorization", "y8sxutshp4gm2u4lsmsmlg"); // Replace with your actual access token

        //        // Execute the request
        //        var response = client.Execute(request);
        //        dynamic JSonResponseData = "";

        //        // Check the response
        //        if (response.IsSuccessful)
        //        {
        //            //JSonResponseData = response.Content;

        //            #region Add record in gen_UtsAts_Records
        //            string EditHRJsonData = JsonConvert.SerializeObject(response.Content);
        //            GenUtsAtsApiRecord utsAtsApi_Records = new()
        //            {
        //                FromApiUrl = _configuration["ProjectURL"].ToString() + "GetMatcherLeadsThroughATS",
        //                ToApiUrl = ATSAPIURL + "hr/get-users",//ATS Get Matcher Leads
        //                PayloadToSend = json,
        //                ResponseReceived = EditHRJsonData,
        //                CreatedById = loggedInUser_ID,
        //                CreatedByDateTime = DateTime.Now,
        //                HrId = HRID
        //            };

        //            APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);
        //            #endregion


        //            matcherUserLists = JsonConvert.DeserializeObject<MatcherUserLists>(response.Content);
        //            return matcherUserLists;
        //        }
        //    }

        //    return matcherUserLists;
        //}

        //private long InsertUtsAtsApiDetails(GenUtsAtsApiRecord gen_UtsAtsApi_Records)
        //{
        //    GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();

        //    utsAtsApi_Records.FromApiUrl = gen_UtsAtsApi_Records.FromApiUrl;
        //    utsAtsApi_Records.ToApiUrl = gen_UtsAtsApi_Records.ToApiUrl;    //Here API URL of ATS will come.
        //    utsAtsApi_Records.PayloadToSend = gen_UtsAtsApi_Records.PayloadToSend;
        //    utsAtsApi_Records.CreatedById = gen_UtsAtsApi_Records.CreatedById;
        //    utsAtsApi_Records.CreatedByDateTime = DateTime.Now;
        //    utsAtsApi_Records.HrId = gen_UtsAtsApi_Records.HrId;
        //    utsAtsApi_Records.ResponseReceived = gen_UtsAtsApi_Records.ResponseReceived;
        //    _talentConnectAdminDBContext.GenUtsAtsApiRecords.Add(utsAtsApi_Records);
        //    _talentConnectAdminDBContext.SaveChanges();

        //    return utsAtsApi_Records.Id;
        //}
        //#endregion
    }
}
