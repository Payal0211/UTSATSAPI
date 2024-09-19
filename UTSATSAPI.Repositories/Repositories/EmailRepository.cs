using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Web;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class EmailRepository : ISendEmailNotes
    {
        #region Variables

        private TalentConnectAdminDBContext db;
        private readonly IConfiguration iConfiguration;
        private IWebHostEnvironment webHostEnvironment;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailRepository"/> class.
        /// </summary>
        /// <param name="_db">The database.</param>
        /// <param name="_iConfiguration">The i configuration.</param>
        /// <param name="_webHostEnvironment">The web host environment.</param>
        public EmailRepository(TalentConnectAdminDBContext _db, IConfiguration _iConfiguration, IWebHostEnvironment _webHostEnvironment)
        {
            db = _db;
            iConfiguration = _iConfiguration;
            webHostEnvironment = _webHostEnvironment;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends the email for adding notes to internal team and assigned users.
        /// </summary>
        /// <param name="hrID">The hr identifier.</param>
        /// <param name="assignedUsers">The assigned users.</param>
        /// <param name="notes">The notes.</param>
        /// <param name="userEmployeeID">The user employee identifier.</param>
        /// <returns></returns>
        public async Task<string> SendEmailForAddingNotes(long? hrID, string assignedUsers, string notes, string userEmployeeID)
        {
            try
            {
                string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "", LoggedInUsername = "", AssignedUserEmailId = "", AssignedUserEmailName = "";
                StringBuilder sbBody = new StringBuilder();
                bool IsAdHoc, IsManaged = false;
                LoggedInUsername = db.UsrUsers.AsNoTracking().Where(x => x.EmployeeId == userEmployeeID).Select(x => x.FullName).FirstOrDefault();
                GenSalesHiringRequest _SalesHiringRequest = db.GenSalesHiringRequests.AsNoTracking().Where(t => t.Id == hrID).FirstOrDefault();
                if (_SalesHiringRequest != null)
                {
                    SalesPersonName = db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.FullName).FirstOrDefault();
                    salesPersonEmail = db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.EmailId).FirstOrDefault();

                    IsAdHoc = _SalesHiringRequest.IsAdHocHr;
                    IsManaged = _SalesHiringRequest.IsManaged;

                    Subject = "New Note has been added - " + _SalesHiringRequest.HrNumber;
                    BodyCustom = "Hi,";
                    sbBody.Append(BodyCustom);
                    sbBody.Append("<br/>");
                    sbBody.Append(LoggedInUsername + " has added a note for you.");

                    sbBody.Append("<div style='width:100%'>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Note: <a class='link' href = '" + iConfiguration["NewAdminProjectURL"].ToString().Trim() + "allhiringrequest/" + hrID + "' target='_blank' >" + notes + " </a>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Click here to view or add any new note for this HR");
                    sbBody.Append("<br/>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Thanks");
                    sbBody.Append("<br/>");
                    sbBody.Append("Uplers Talent Solutions Team");
                    sbBody.Append("<br/>");
                    sbBody.Append("<hr>");
                    sbBody.Append("We are committed to your privacy. ");
                    sbBody.Append("You may unsubscribe from these communications at any time. ");
                    sbBody.Append("For more information, check out our <a class='link' href='" + iConfiguration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
                    sbBody.Append("</div>");

                    string InternalCCEmailID = GetCCEmailIdValues(_SalesHiringRequest.SalesUserId);
                    string InternalCCEmailName = GetCCEmailNameValues(_SalesHiringRequest.SalesUserId);
                    if (assignedUsers != null && assignedUsers != "")
                    {
                        AssignedUserEmailId = GetAssignedUsersCCEmailIdValues(assignedUsers);
                        AssignedUserEmailName = GetAssignedUsersCCEmailNameValues(assignedUsers);
                    }
                    string TOEMAIL = "";
                    string TOEMAILNAME = "";

                    if (assignedUsers != null && assignedUsers != "")
                    {
                        if (!string.IsNullOrEmpty(AssignedUserEmailId))
                        {
                            TOEMAIL += "," + AssignedUserEmailId;
                            TOEMAILNAME += "," + AssignedUserEmailName;
                        }
                    }
                    TOEMAIL = TOEMAIL.TrimStart(',');
                    TOEMAILNAME = TOEMAILNAME.TrimStart(',');

                    string CCEMAIL = "";
                    string CCEMAILNAME = "";

                    if (!string.IsNullOrEmpty(salesPersonEmail))
                    {
                        CCEMAIL = iConfiguration["app_settings:CCEmailId"] + ',' + iConfiguration["app_settings:CC1EmailId"] + ',' + iConfiguration["app_settings:TSCEmailId"] + ',' + salesPersonEmail;
                        CCEMAILNAME = iConfiguration["app_settings:CCEmailName"] + ',' + iConfiguration["app_settings:CC1EmailName"] + ',' + iConfiguration["app_settings:TSCEmailName"] + ',' + SalesPersonName;
                    }
                    else
                    {
                        CCEMAIL = iConfiguration["app_settings:CCEmailId"] + ',' + iConfiguration["app_settings:CC1EmailId"] + ',' + iConfiguration["app_settings:TSCEmailId"] + InternalCCEmailID;
                        CCEMAILNAME = iConfiguration["app_settings:CCEmailName"] + ',' + iConfiguration["app_settings:CC1EmailName"] + ',' + iConfiguration["app_settings:TSCEmailName"] + InternalCCEmailName;
                    }

                    if (!string.IsNullOrEmpty(TOEMAIL) && !string.IsNullOrEmpty(CCEMAIL))
                    {
                        EmailOperator emailOperator = new EmailOperator(iConfiguration);
                        //List<string> toemail = new List<string>() { TOEMAIL };
                        //List<string> toemailname = new List<string>() { TOEMAILNAME };

                        emailOperator.SetToEmail(TOEMAIL.Split(',').ToList());
                        emailOperator.SetToEmailName(TOEMAILNAME.Split(',').ToList());
                        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
                        emailOperator.SetSubject(Subject);
                        emailOperator.SetBody(sbBody.ToString());

                        #region SendEmail
                        if (!string.IsNullOrEmpty(Subject))
                            emailOperator.SendEmail();
                        #endregion
                    }
                    return "success";
                }
                else
                    return "error";
            }
            catch (Exception e) { return e.Message; }
        }

        /// <summary>
        /// Gets the cc email identifier values.
        /// </summary>
        /// <param name="SalesUSerID">The sales u ser identifier.</param>
        /// <returns></returns>
        public string GetCCEmailIdValues(long? salesUSerID)
        {
            string CCEmailIds = "";
            string SalesLeadEmailId = "";

            if (salesUSerID > 0)
            {
                UsrUserHierarchy _objUserHierarchy = db.UsrUserHierarchies.AsNoTracking().Where(x => x.UserId == salesUSerID).FirstOrDefault();
                if (_objUserHierarchy != null)
                {
                    SalesLeadEmailId = db.UsrUsers.AsNoTracking().Where(x => x.Id == _objUserHierarchy.ParentId).Select(x => x.EmailId).FirstOrDefault();
                }
            }

            GenSystemConfiguration _objSystemConfigurationName = db.GenSystemConfigurations.AsNoTracking().Where(x => x.Key == "EmailId" && x.IsActive == true).FirstOrDefault();
            if (_objSystemConfigurationName != null)
            {
                if (SalesLeadEmailId != "")
                    CCEmailIds = "," + _objSystemConfigurationName.Value + "," + SalesLeadEmailId;
                else
                    CCEmailIds = "," + _objSystemConfigurationName.Value;
            }

            return CCEmailIds;
        }

        /// <summary>
        /// Gets the cc email name values.
        /// </summary>
        /// <param name="salesUSerID">The sales u ser identifier.</param>
        /// <returns></returns>
        public string GetCCEmailNameValues(long? salesUSerID)
        {
            string CCEmailNames = "";
            string SalesLeadName = "";

            if (salesUSerID > 0)
            {
                UsrUserHierarchy _objUserHierarchy = db.UsrUserHierarchies.AsNoTracking().Where(x => x.UserId == salesUSerID).FirstOrDefault();
                if (_objUserHierarchy != null)
                {
                    SalesLeadName = db.UsrUsers.AsNoTracking().Where(x => x.Id == _objUserHierarchy.ParentId).Select(x => x.FullName).FirstOrDefault();
                }
            }


            GenSystemConfiguration _objSystemConfigurationName = db.GenSystemConfigurations.AsNoTracking().Where(x => x.Key == "Name" && x.IsActive == true).FirstOrDefault();
            if (_objSystemConfigurationName != null)
            {
                if (SalesLeadName != "")
                    CCEmailNames = "," + _objSystemConfigurationName.Value + "," + SalesLeadName;
                else
                    CCEmailNames = "," + _objSystemConfigurationName.Value;
            }

            return CCEmailNames;
        }

        public string GetAssignedUsersCCEmailIdValues(string assignedUsersIds)
        {
            assignedUsersIds = assignedUsersIds.TrimEnd(',');
            string CCEmailIds = "";

            if (assignedUsersIds != null && assignedUsersIds != "")
            {
                List<UserOptionVM> Userdata = new List<UserOptionVM>();
                for (int i = 0; i < assignedUsersIds.Split(',').Count(); i++)
                {
                    UserOptionVM user = new UserOptionVM();
                    user.ID = assignedUsersIds.Split(',')[i];
                    if (user.ID != "" || !string.IsNullOrEmpty(user.ID))
                    {
                        long TaggedUserID = 0;
                        bool isValidUserID = long.TryParse(user.ID, out TaggedUserID);
                        if (isValidUserID == true && TaggedUserID > 0)
                        {
                            var CheckValidUser = db.UsrUsers.AsNoTracking().FirstOrDefault(x => x.Id == TaggedUserID);
                            if (CheckValidUser != null)
                                CCEmailIds += CheckValidUser.EmailId + ",";
                        }
                    }
                }
            }
            if (CCEmailIds != "")
            {
                CCEmailIds = CCEmailIds.TrimEnd(',');
            }
            return CCEmailIds;
        }

        public string GetAssignedUsersCCEmailNameValues(string assignedUsersIds)
        {
            string CCEmailNames = "";
            if (assignedUsersIds != null && assignedUsersIds != "")
            {
                List<UserOptionVM> Userdata = new List<UserOptionVM>();
                for (int i = 0; i < assignedUsersIds.Split(',').Count(); i++)
                {
                    UserOptionVM user = new UserOptionVM();
                    user.ID = assignedUsersIds.Split(',')[i];
                    if (user.ID != "" || !string.IsNullOrEmpty(user.ID))
                    {
                        if (user.ID != "" || !string.IsNullOrEmpty(user.ID))
                        {
                            long TaggedUserID = 0;
                            bool isValidUserID = long.TryParse(user.ID, out TaggedUserID);
                            if (isValidUserID == true && TaggedUserID > 0)
                            {
                                var CheckValidUser = db.UsrUsers.AsNoTracking().FirstOrDefault(x => x.Id == TaggedUserID);
                                if (CheckValidUser != null)
                                    CCEmailNames += CheckValidUser.FullName + ",";
                            }
                        }
                    }
                }
            }
            if (CCEmailNames != "")
            {
                CCEmailNames = CCEmailNames.TrimEnd(',');
            }
            return CCEmailNames;
        }

        /// <summary>
        /// Gets the ad hoc cc email identifier values.
        /// </summary>
        /// <returns></returns>
        public string GetAdHocCCEmailIdValues()
        {
            string CCEmailIds = "";
            GenSystemConfiguration _objSystemConfigurationName = db.GenSystemConfigurations.AsNoTracking().Where(x => x.Key == "AdhocCCEmailIds" && x.IsActive == true).FirstOrDefault();
            if (_objSystemConfigurationName != null)
            {
                CCEmailIds = "," + _objSystemConfigurationName.Value;
            }

            return CCEmailIds;
        }

        /// <summary>
        /// Gets the ad hoc cc email name values.
        /// </summary>
        /// <returns></returns>
        public string GetAdHocCCEmailNameValues()
        {
            string CCEmailNames = "";
            GenSystemConfiguration _objSystemConfigurationName = db.GenSystemConfigurations.AsNoTracking().Where(x => x.Key == "AdhocCCEmailName" && x.IsActive == true).FirstOrDefault();
            if (_objSystemConfigurationName != null)
            {
                CCEmailNames = "," + _objSystemConfigurationName.Value;
            }

            return CCEmailNames;
        }

        /// <summary>
        /// Gets the managed cc email identifier values.
        /// </summary>
        /// <returns></returns>
        public string GetManagedCCEmailIdValues()
        {
            string CCEmailIds = "";
            GenSystemConfiguration _objSystemConfigurationName = db.GenSystemConfigurations.AsNoTracking().Where(x => x.Key == "ManagedCCEmailIds" && x.IsActive == true).FirstOrDefault();
            if (_objSystemConfigurationName != null)
            {
                CCEmailIds = "," + _objSystemConfigurationName.Value;
            }

            return CCEmailIds;
        }

        /// <summary>
        /// Gets the managed cc email name values.
        /// </summary>
        /// <returns></returns>
        public string GetManagedCCEmailNameValues()
        {
            string CCEmailNames = "";
            GenSystemConfiguration _objSystemConfigurationName = db.GenSystemConfigurations.AsNoTracking().Where(x => x.Key == "ManagedCCEmailNames" && x.IsActive == true).FirstOrDefault();
            if (_objSystemConfigurationName != null)
            {
                CCEmailNames = "," + _objSystemConfigurationName.Value;
            }
            return CCEmailNames;
        }

        /// <summary>
        /// Gets the SMCC email identifier values.
        /// </summary>
        /// <returns></returns>
        public string GetSMCCEmailIdValues()
        {
            string CCEmailIds = "";
            GenSystemConfiguration _objSystemConfigurationName = db.GenSystemConfigurations.AsNoTracking().Where(x => x.Key == "SMCCEmailIds" && x.IsActive == true).FirstOrDefault();
            if (_objSystemConfigurationName != null)
            {
                CCEmailIds = "," + _objSystemConfigurationName.Value;
            }
            return CCEmailIds;
        }

        /// <summary>
        /// Gets the SMCC email name values.
        /// </summary>
        /// <returns></returns>
        public string GetSMCCEmailNameValues()
        {
            string CCEmailNames = "";
            GenSystemConfiguration _objSystemConfigurationName = db.GenSystemConfigurations.AsNoTracking().Where(x => x.Key == "SMCCEmailNames" && x.IsActive == true).FirstOrDefault();
            if (_objSystemConfigurationName != null)
            {
                CCEmailNames = "," + _objSystemConfigurationName.Value;
            }
            return CCEmailNames;
        }

        /// <summary>
        /// Sends the email for hr acceptance to internal team.
        /// </summary>
        /// <param name="HRID">The hrid.</param>
        /// <param name="AcceptanceValue">The acceptance value.</param>
        /// <param name="Reason">The reason.</param>
        /// <returns></returns>
        public string SendEmailForHRAcceptanceToInternalTeam(long HRID, int AcceptanceValue, string Reason)
        {
            try
            {
                string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "", companyName = "", ClientName = "", ClientEmail = "";
                StringBuilder sbBody = new StringBuilder();
                bool IsAdHoc, IsManaged = false;
                GenSalesHiringRequest _SalesHiringRequest = db.GenSalesHiringRequests.AsNoTracking().Where(t => t.Id == HRID).FirstOrDefault();
                if (_SalesHiringRequest != null)
                {
                    var ContactId = _SalesHiringRequest.ContactId;
                    GenContact contact = db.GenContacts.AsNoTracking().Where(x => x.Id == ContactId).FirstOrDefault();
                    if (contact != null)
                    {
                        ClientName = contact.FullName;
                        ClientEmail = contact.EmailId;
                        GenCompany company = db.GenCompanies.AsNoTracking().Where(x => x.Id == contact.CompanyId).FirstOrDefault();
                        if (company != null)
                        {
                            companyName = company.Company;
                        }
                    }

                    SalesPersonName = db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.FullName).FirstOrDefault();
                    salesPersonEmail = db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.EmailId).FirstOrDefault();

                    IsAdHoc = _SalesHiringRequest.IsAdHocHr;
                    IsManaged = _SalesHiringRequest.IsManaged;

                    if (AcceptanceValue == 1)
                    {
                        Subject = "Operation team has accepted the HR for further process";
                        BodyCustom = "Hi,";
                        sbBody.Append(BodyCustom);
                        sbBody.Append("<br/>");
                        sbBody.Append("Ops team has accepted the HR added by you and have started searching for the right talent.");
                    }
                    else if (AcceptanceValue == 2)
                    {
                        Subject = "Operation team need more clarity";
                        BodyCustom = "Hi,";
                        sbBody.Append(BodyCustom);
                        sbBody.Append("<br/>");
                        sbBody.Append("The details shared by you in this HR is not enough to start searching for the talent. So, kindly coordinate with the Ops team and shared the missing details to them.");
                    }

                    sbBody.Append("<div style='width:100%'>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Below are the HR details:");
                    sbBody.Append("<br/>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Company Name: " + companyName);
                    sbBody.Append("<br/>");
                    sbBody.Append("Client Name: " + ClientName);
                    sbBody.Append("<br/>");
                    sbBody.Append("Client Email: " + ClientEmail);
                    sbBody.Append("<br/>");
                    sbBody.Append("HR ID: " + _SalesHiringRequest.HrNumber);
                    sbBody.Append("<br/>");
                    if (AcceptanceValue == 2)
                    {
                        sbBody.Append("Add details which are missing to or need more clarity : " + Reason);
                        sbBody.Append("<br/>");
                    }
                    if (IsManaged)
                    {
                        sbBody.Append("Manage: Yes");
                    }
                    else
                    {
                        sbBody.Append("Self Manage: Yes");
                    }
                    sbBody.Append("<br/>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Thanks");
                    sbBody.Append("<br/>");
                    sbBody.Append("Uplers Talent Solutions Team");
                    sbBody.Append("<br/>");
                    sbBody.Append("<hr>");
                    sbBody.Append("We are committed to your privacy. ");
                    sbBody.Append("You may unsubscribe from these communications at any time. ");
                    sbBody.Append("For more information, check out our <a class='link' href='" + iConfiguration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
                    sbBody.Append("</div>");

                    string InternalCCEmailID = GetCCEmailIdValues(_SalesHiringRequest.SalesUserId);
                    string InternalCCEmailName = GetCCEmailNameValues(_SalesHiringRequest.SalesUserId);

                    string TOEMAIL = salesPersonEmail;
                    string TOEMAILNAME = SalesPersonName;

                    string CCEMAIL = "";
                    string CCEMAILNAME = "";

                    string AdHocEMAIL = "";
                    string AdHocEMAILNAME = "";

                    string ManagedEMAIL = "";
                    string ManagedEMAILNAME = "";

                    string SMEMAIL = "";
                    string SMEMAILNAME = "";

                    if (IsAdHoc)
                    {
                        AdHocEMAIL = GetAdHocCCEmailIdValues();
                        AdHocEMAILNAME = GetAdHocCCEmailNameValues();

                        CCEMAIL = iConfiguration["app_settings:CCEmailId"] + ',' + iConfiguration["app_settings:CC1EmailId"] + ',' + iConfiguration["app_settings:TSCEmailId"] + ',' + InternalCCEmailID + AdHocEMAIL;
                        CCEMAILNAME = iConfiguration["app_settings:CCEmailName"] + ',' + iConfiguration["app_settings:CC1EmailName"] + ',' + iConfiguration["app_settings:TSCEmailName"] + ',' + string.IsNullOrEmpty(InternalCCEmailName) + string.IsNullOrEmpty(AdHocEMAILNAME);
                    }
                    else
                    {
                        CCEMAIL = iConfiguration["app_settings:CCEmailId"] + ',' + iConfiguration["app_settings:CC1EmailId"] + ',' + iConfiguration["app_settings:TSCEmailId"] + string.IsNullOrEmpty(InternalCCEmailID);
                        CCEMAILNAME = iConfiguration["app_settings:CCEmailName"] + ',' + iConfiguration["app_settings:CC1EmailName"] + ',' + iConfiguration["app_settings:TSCEmailName"] + string.IsNullOrEmpty(InternalCCEmailName);
                    }

                    if (IsManaged)
                    {
                        ManagedEMAIL = GetManagedCCEmailIdValues();
                        ManagedEMAILNAME = GetManagedCCEmailNameValues();

                        CCEMAIL = CCEMAIL + ManagedEMAIL;
                        CCEMAILNAME = CCEMAILNAME + ManagedEMAILNAME;
                    }
                    if (!IsManaged)
                    {
                        SMEMAIL = GetSMCCEmailIdValues();
                        SMEMAILNAME = GetSMCCEmailNameValues();

                        CCEMAIL = CCEMAIL + SMEMAIL;
                        CCEMAILNAME = CCEMAILNAME + SMEMAILNAME;
                    }

                    if (!string.IsNullOrEmpty(TOEMAIL) && !string.IsNullOrEmpty(CCEMAIL))
                    {
                        EmailOperator emailOperator = new EmailOperator(iConfiguration);
                        List<string> toemail = new List<string>() { TOEMAIL };
                        List<string> toemailname = new List<string>() { TOEMAILNAME };

                        emailOperator.SetToEmail(toemail);
                        emailOperator.SetToEmailName(toemailname);
                        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
                        emailOperator.SetSubject(Subject);
                        emailOperator.SetBody(sbBody.ToString());

                        #region SendEmail
                        if (!string.IsNullOrEmpty(Subject))
                            emailOperator.SendEmail();
                        #endregion
                    }
                    return "success";
                }
                else
                    return "error";
            }
            catch (Exception e) { return e.Message; }
        }

        public string SendEmailForTRAcceptanceToInternalTeam(long HRID, int AcceptanceValue, int? TRParked)
        {
            try
            {
                string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "", companyName = "", ClientName = "", ClientEmail = "";
                StringBuilder sbBody = new StringBuilder();
                bool IsAdHoc, IsManaged = false;
                GenSalesHiringRequest _SalesHiringRequest = db.GenSalesHiringRequests.AsNoTracking().Where(t => t.Id == HRID).FirstOrDefault();
                if (_SalesHiringRequest != null)
                {
                    var ContactId = _SalesHiringRequest.ContactId;
                    GenContact contact = db.GenContacts.AsNoTracking().Where(x => x.Id == ContactId).FirstOrDefault();
                    if (contact != null)
                    {
                        ClientName = contact.FullName;
                        ClientEmail = contact.EmailId;
                        GenCompany company = db.GenCompanies.AsNoTracking().Where(x => x.Id == contact.CompanyId).FirstOrDefault();
                        if (company != null)
                        {
                            companyName = company.Company;
                        }
                    }

                    SalesPersonName = db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.FullName).FirstOrDefault();
                    salesPersonEmail = db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.EmailId).FirstOrDefault();

                    IsAdHoc = _SalesHiringRequest.IsAdHocHr;
                    IsManaged = _SalesHiringRequest.IsManaged;

                    Subject = "Operation team has accepted the TR for further process";
                    BodyCustom = "Hi,";
                    sbBody.Append(BodyCustom);
                    sbBody.Append("<br/>");
                    sbBody.Append("Ops team has accepted the TR added by you and have started searching for the right talent.");

                    sbBody.Append("<div style='width:100%'>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Below are the TR details:");
                    sbBody.Append("<br/>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Company Name: " + companyName);
                    sbBody.Append("<br/>");
                    sbBody.Append("Client Name: " + ClientName);
                    sbBody.Append("<br/>");
                    sbBody.Append("Client Email: " + ClientEmail);
                    sbBody.Append("<br/>");
                    sbBody.Append("HR ID: " + _SalesHiringRequest.HrNumber);
                    sbBody.Append("<br/>");
                    sbBody.Append("TR Accepted: " + AcceptanceValue);
                    //sbBody.Append("<br/>");
                    //sbBody.Append("TR Parked: " + TRParked);
                    sbBody.Append("<br/>");
                    sbBody.Append("<br/>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Thanks");
                    sbBody.Append("<br/>");
                    sbBody.Append("Uplers Talent Solutions Team");
                    sbBody.Append("<br/>");
                    sbBody.Append("<hr>");
                    sbBody.Append("We are committed to your privacy. ");
                    sbBody.Append("You may unsubscribe from these communications at any time. ");
                    sbBody.Append("For more information, check out our <a class='link' href='" + iConfiguration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
                    sbBody.Append("</div>");

                    string InternalCCEmailID = GetCCEmailIdValues(_SalesHiringRequest.SalesUserId);
                    string InternalCCEmailName = GetCCEmailNameValues(_SalesHiringRequest.SalesUserId);

                    string TOEMAIL = salesPersonEmail;
                    string TOEMAILNAME = SalesPersonName;

                    string CCEMAIL = "";
                    string CCEMAILNAME = "";

                    string AdHocEMAIL = "";
                    string AdHocEMAILNAME = "";

                    string ManagedEMAIL = "";
                    string ManagedEMAILNAME = "";

                    string SMEMAIL = "";
                    string SMEMAILNAME = "";

                    if (IsAdHoc)
                    {
                        AdHocEMAIL = GetAdHocCCEmailIdValues();
                        AdHocEMAILNAME = GetAdHocCCEmailNameValues();


                        CCEMAIL = iConfiguration["app_settings:CCEmailId"] + ',' + iConfiguration["app_settings:CC1EmailId"] + ',' + iConfiguration["app_settings:TSCEmailId"] + ',' + InternalCCEmailID + AdHocEMAIL;
                        CCEMAILNAME = iConfiguration["app_settings:CCEmailName"] + ',' + iConfiguration["app_settings:CC1EmailName"] + ',' + iConfiguration["app_settings:TSCEmailName"] + ',' + InternalCCEmailName + AdHocEMAILNAME;
                    }
                    else
                    {
                        CCEMAIL = iConfiguration["app_settings:CCEmailId"] + ',' + iConfiguration["app_settings:CC1EmailId"] + ',' + iConfiguration["app_settings:TSCEmailId"] + ',' + InternalCCEmailID;
                        CCEMAILNAME = iConfiguration["app_settings:CCEmailName"] + ',' + iConfiguration["app_settings:CC1EmailName"] + ',' + iConfiguration["app_settings:TSCEmailName"] + ',' + InternalCCEmailName;
                    }

                    if (IsManaged)
                    {
                        ManagedEMAIL = GetManagedCCEmailIdValues();
                        ManagedEMAILNAME = GetManagedCCEmailNameValues();

                        CCEMAIL = CCEMAIL + ManagedEMAIL;
                        CCEMAILNAME = CCEMAILNAME + ManagedEMAILNAME;
                    }
                    if (!IsManaged)
                    {
                        SMEMAIL = GetSMCCEmailIdValues();
                        SMEMAILNAME = GetSMCCEmailNameValues();

                        CCEMAIL = CCEMAIL + SMEMAIL;
                        CCEMAILNAME = CCEMAILNAME + SMEMAILNAME;
                    }

                    if (!string.IsNullOrEmpty(TOEMAIL) && !string.IsNullOrEmpty(CCEMAIL))
                    {
                        EmailOperator emailOperator = new EmailOperator(iConfiguration);
                        List<string> toemail = new List<string>() { TOEMAIL };
                        List<string> toemailname = new List<string>() { TOEMAILNAME };

                        emailOperator.SetToEmail(toemail);
                        emailOperator.SetToEmailName(toemailname);
                        emailOperator.SetCCEmail(CCEMAIL, CCEMAILNAME);
                        emailOperator.SetSubject(Subject);
                        emailOperator.SetBody(sbBody.ToString());

                        #region SendEmail
                        if (!string.IsNullOrEmpty(Subject))
                            emailOperator.SendEmail();
                        #endregion
                    }
                    return "success";
                }
                else
                    return "error";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //After Shortlisted Talent by HR email notification to Sales Team (Internal)
        /// <summary>
        /// Sends the email for talent accepted hr.
        /// </summary>
        /// <param name="TalentID">The talent identifier.</param>
        /// <param name="HRID">The hrid.</param>
        /// <returns></returns>
        [Authorize]
        public string SendEmailForTalentAcceptedHR(Int64 TalentID, Int64? HRID)
        {
            try
            {

                string TalentName = "", HR_Number = "", SalesEmail = "", priority = "", SalesName = "";
                GenTalent talent = db.GenTalents.AsNoTracking().Where(x => x.Id == TalentID).FirstOrDefault();

                bool IsAdHoc = false;
                bool IsManaged = false;


                if (talent != null)
                {
                    TalentName = talent.Name;


                    GenContactTalentPriority ContactTalentPriority = db.GenContactTalentPriorities.AsNoTracking().Where(x => x.TalentId == talent.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (ContactTalentPriority != null)
                    {
                        priority = ContactTalentPriority.TalentPriority;
                    }
                }
                GenSalesHiringRequest salesHiringRequest = db.GenSalesHiringRequests.AsNoTracking().Where(x => x.Id == HRID).FirstOrDefault();
                if (salesHiringRequest != null)
                {
                    HR_Number = salesHiringRequest.HrNumber;
                    IsAdHoc = salesHiringRequest.IsAdHocHr;
                    IsManaged = salesHiringRequest.IsManaged;
                    UsrUser objSalesPerson = db.UsrUsers.AsNoTracking().Where(x => x.Id == salesHiringRequest.SalesUserId).FirstOrDefault();
                    if (objSalesPerson != null)
                    {
                        SalesEmail = objSalesPerson.EmailId;
                        SalesName = objSalesPerson.FullName;
                    }
                }

                string Subject = "";
                string BodyCustom = "";
                System.Text.StringBuilder sbBody = new System.Text.StringBuilder();

                Subject = "The Talent " + TalentName + " has accepted the Hiring Request - " + HR_Number + " of Hiring Request";

                BodyCustom = "Hello Team,";
                sbBody.Append(BodyCustom);
                sbBody.Append("<br/>");
                sbBody.Append("<div style='width:100%'>");
                sbBody.Append("<br/>");
                sbBody.Append("Please proceed to change the status of Talent to Show to Client, he has accepted the Hiring Request.");
                sbBody.Append("<br/>");
                sbBody.Append("HRID: " + HR_Number + "<br/>");
                sbBody.Append("Talent Name: " + TalentName + "<br/>");
                sbBody.Append("Priority: " + priority);
                sbBody.Append("<br/>");
                if (IsManaged)
                {
                    sbBody.Append("Manage: Yes");
                }
                else
                {
                    sbBody.Append("Self Manage: Yes");
                }
                sbBody.Append("<br/>");
                sbBody.Append("<br/>");
                sbBody.Append("Thanks");
                sbBody.Append("<br/>");
                sbBody.Append("Uplers Talent Solutions Team");
                sbBody.Append("<br/>");
                sbBody.Append("<hr>");
                sbBody.Append("We are committed to your privacy. ");
                sbBody.Append("You may unsubscribe from these communications at any time. ");
                sbBody.Append("For more information, check out our <a class='link' href='" + iConfiguration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
                sbBody.Append("</div>");

                string TOEMAIL = SalesEmail;
                string TOEMAILNAME = SalesName;

                string InternalCCEmailID = GetCCEmailIdValues(salesHiringRequest.SalesUserId);
                string InternalCCEmailName = GetCCEmailNameValues(salesHiringRequest.SalesUserId);

                string AdHocEMAIL = "";
                string AdHocEMAILNAME = "";

                string CCEMAIL = "";
                string CCEMAILNAME = "";

                string ManagedEMAIL = "";
                string ManagedEMAILNAME = "";

                string SMEMAIL = "";
                string SMEMAILNAME = "";

                if (IsAdHoc)
                {
                    AdHocEMAIL = GetAdHocCCEmailIdValues();
                    AdHocEMAILNAME = GetAdHocCCEmailNameValues();

                    CCEMAIL = iConfiguration["app_settings:CCEmailId"] + ',' + iConfiguration["app_settings:CC1EmailId"] + ',' + iConfiguration["app_settings:TSCEmailId"] + ',' + InternalCCEmailID + AdHocEMAIL;
                    CCEMAILNAME = iConfiguration["app_settings:CCEmailName"] + ',' + iConfiguration["app_settings:CC1EmailName"] + ',' + iConfiguration["app_settings:TSCEmailName"] + ',' + InternalCCEmailName + AdHocEMAILNAME;

                }
                else
                {
                    CCEMAIL = iConfiguration["app_settings:CCEmailId"] + ',' + iConfiguration["app_settings:CC1EmailId"] + ',' + iConfiguration["app_settings:TSCEmailId"] + ',' + InternalCCEmailID;
                    CCEMAILNAME = iConfiguration["app_settings:CCEmailName"] + ',' + iConfiguration["app_settings:CC1EmailName"] + ',' + iConfiguration["app_settings:TSCEmailName"] + ',' + InternalCCEmailName;
                }

                if (IsManaged)
                {
                    ManagedEMAIL = GetManagedCCEmailIdValues();
                    ManagedEMAILNAME = GetManagedCCEmailNameValues();

                    CCEMAIL = CCEMAIL + ManagedEMAIL;
                    CCEMAILNAME = CCEMAILNAME + ManagedEMAILNAME;
                }
                if (!IsManaged)
                {
                    SMEMAIL = GetSMCCEmailIdValues();
                    SMEMAILNAME = GetSMCCEmailNameValues();

                    CCEMAIL = CCEMAIL + SMEMAIL;
                    CCEMAILNAME = CCEMAILNAME + SMEMAILNAME;
                }

                if (CCEMAIL != "" && TOEMAIL != "")
                {
                    try
                    {
                        Helpers.SendEmails.SendEmailsWithCC_WithOutBcc(webHostEnvironment.WebRootPath, iConfiguration["app_settings:TSCEmailId"], "", TOEMAIL, TOEMAILNAME, CCEMAILNAME, CCEMAIL, Subject, sbBody.ToString(), "", "", AppSettingProvider.SMTPEmailName, AppSettingProvider.SMTPPasswordName, AppSettingProvider.SMTPClientName, AppSettingProvider.SMTPSSLName, AppSettingProvider.SMTPPortName);
                    }

                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                return "success";
            }
            catch (Exception e) { return e.Message; }
        }

        /// <summary>
        /// Sends the emailto sales direct placement.
        /// </summary>
        /// <param name="TalentID">The talent identifier.</param>
        /// <param name="HiringRequest_ID">The hiring request identifier.</param>
        /// <returns></returns>
        [Authorize]
        public string SendEmailtoSalesDirectPlacement(Int64 TalentID, long? HiringRequest_ID)
        {
            try
            {
                string Subject = "";
                string BodyCustom = "";
                System.Text.StringBuilder sbBody = new System.Text.StringBuilder();

                GenSalesHiringRequest salesHiringREquest = db.GenSalesHiringRequests.AsNoTracking().Where(x => x.Id == HiringRequest_ID).FirstOrDefault();
                string TalentName = "", priority = ""; 
                decimal? yearsofExperience = 0, CurrentCTC = 0, ExpectedCTC = 0;
                string HR_Number = "", Company_Name = "", Contact_Email = "", EmailID = "", salesName = "", salesEmailId = "";
                if (salesHiringREquest != null)
                {
                    HR_Number = salesHiringREquest.HrNumber;
                    var salesDetails = db.UsrUsers.AsNoTracking().Where(x => x.Id == salesHiringREquest.SalesUserId).Select(x => new
                    {
                        Fullname = x.FullName,
                        EmailID = x.EmailId
                    }).FirstOrDefault();

                    salesName = salesDetails.Fullname;
                    salesEmailId = salesDetails.EmailID;
                    GenContact contact = db.GenContacts.AsNoTracking().Where(x => x.Id == salesHiringREquest.ContactId).FirstOrDefault();
                    if (contact != null)
                    {
                        Contact_Email = contact.EmailId;
                        GenCompany company = db.GenCompanies.AsNoTracking().Where(x => x.Id == contact.CompanyId).FirstOrDefault();
                        if (company != null)
                        {
                            Company_Name = company.Company;
                        }
                    }
                }

                GenTalent talent = db.GenTalents.AsNoTracking().Where(x => x.Id == TalentID).FirstOrDefault();
                PrgTalentRole roles = db.PrgTalentRoles.AsNoTracking().Where(x => x.Id == talent.RoleId).FirstOrDefault();

                if (talent != null)
                {
                    TalentName = talent.FirstName;
                    EmailID = talent.EmailId;
                    yearsofExperience = talent.TotalExpYears;
                    GenContactTalentPriority ContactTalentPriority = db.GenContactTalentPriorities.AsNoTracking().Where(x => x.TalentId == talent.Id && x.HiringRequestId == HiringRequest_ID).FirstOrDefault();
                    if (ContactTalentPriority != null)
                    {
                        priority = ContactTalentPriority.TalentPriority;
                        CurrentCTC = ContactTalentPriority.CurrentCtc;
                        ExpectedCTC = ContactTalentPriority.TalentCost;
                    }
                }

                Subject = "The Talent " + TalentName + " has accepted the Hiring Request - " + HR_Number + " of Hiring Request for Direct Placement";
                BodyCustom = "Hello Team,";
                sbBody.Append(BodyCustom);
                sbBody.Append("<br/>");
                sbBody.Append("<div style='width:100%'>");
                sbBody.Append("<br/> A new talent has been matched for the " + roles.TalentRole + " role for client " + Company_Name + ".<br/><br/>");
                sbBody.Append("HRID: " + HR_Number + ". <br/>");
                sbBody.Append("HR Type: Direct Placement. <br/>");
                sbBody.Append("Talent Expected CTC: " + ExpectedCTC + ". <br/>");
                sbBody.Append("Talent current CTC: " + CurrentCTC + ". <br/>");
                sbBody.Append("DP percentage: " + salesHiringREquest.Dppercentage + ". <br/>");
                sbBody.Append("<br/>");
                sbBody.Append("<br/>");
                sbBody.Append("<br/>");
                sbBody.Append("<br/> Thanks" + " <br/>");
                sbBody.Append("Uplers Talent Solutions Team" + " <br/>");
                sbBody.Append("<hr>");
                sbBody.Append("We are committed to your privacy. ");
                sbBody.Append("You may unsubscribe from these communications at any time. ");
                sbBody.Append("For more information, check out our <a class='link' href='" + iConfiguration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
                sbBody.Append("</div>");

                string TOEMAIL = salesEmailId;
                string TOEMAILNAME = salesName;

                string CCEMAIL = iConfiguration["app_settings:CCEmailId"] + ',' + iConfiguration["app_settings:CC1EmailId"] + ',' + iConfiguration["app_settings:TSCEmailId"];
                string CCEMAILNAME = iConfiguration["app_settings:CCEmailName"] + ',' + iConfiguration["app_settings:CC1EmailName"] + ',' + iConfiguration["app_settings:TSCEmailName"];

                if (TOEMAIL != "")
                {
                    try
                    {
                        Helpers.SendEmails.SendEmailsWithCC_WithOutBcc(webHostEnvironment.WebRootPath, iConfiguration["app_settings:TSCEmailId"], "", TOEMAIL, TOEMAILNAME, CCEMAILNAME, CCEMAIL, Subject, sbBody.ToString(), "", "", AppSettingProvider.SMTPEmailName, AppSettingProvider.SMTPPasswordName, AppSettingProvider.SMTPClientName, AppSettingProvider.SMTPSSLName, AppSettingProvider.SMTPPortName);
                    }

                    catch (Exception ex)
                    {

                    }
                }
                return "success";
            }
            catch (Exception e) { return e.Message; }
        }

        /// <summary>
        /// Sends the email for talent showtoclientby sales team.
        /// </summary>
        /// <param name="TalentID">The talent identifier.</param>
        /// <param name="HRID">The hrid.</param>
        /// <param name="encryptedHRID">The encrypted hrid.</param>
        /// <param name="encryptedRole_ID">The encrypted role identifier.</param>
        /// <returns></returns>
        public string SendEmailForTalentShowtoclientbySalesTeam(Int64 TalentID, Int64? HRID, string encryptedHRID, string encryptedRole_ID)
        {
            try
            {


                string TalentName = "", HR_Number = "", ClientEmail = "", ClientName = "", Position = "", pointofContactEmail = "";
                Int32 Role_Id = 0;
                bool IsresetPassword = false;
                string Username = "", Password = "";


                GenTalent talent = db.GenTalents.AsNoTracking().Where(x => x.Id == TalentID).FirstOrDefault();

                if (talent != null)
                {
                    TalentName = talent.Name;

                }
                GenSalesHiringRequest salesHiringRequest = db.GenSalesHiringRequests.AsNoTracking().Where(x => x.Id == HRID).FirstOrDefault();
                if (salesHiringRequest != null)
                {
                    HR_Number = salesHiringRequest.HrNumber;
                    Position = salesHiringRequest.RequestForTalent;
                    GenContact contact = db.GenContacts.AsNoTracking().Where(x => x.Id == salesHiringRequest.ContactId).FirstOrDefault();
                    if (contact != null)
                    {
                        ClientEmail = contact.EmailId;
                        ClientName = contact.FullName;

                        IsresetPassword = Convert.ToBoolean(contact.IsResetPassword);
                        Username = contact.EmailId;
                        Password = contact.Password;

                    }

                }

                GenTalentPointofContact pointofContact = db.GenTalentPointofContacts.AsNoTracking().Where(x => x.TalentId == TalentID).OrderByDescending(x => x.Id).FirstOrDefault();
                if (pointofContact != null)
                {
                    UsrUser objSalesPerson = db.UsrUsers.AsNoTracking().Where(x => x.Id == pointofContact.UserId).FirstOrDefault();
                    if (objSalesPerson != null)
                    {
                        pointofContactEmail = objSalesPerson.EmailId;
                    }
                }

                string Subject = "";
                string BodyCustom = "";
                System.Text.StringBuilder sbBody = new System.Text.StringBuilder();


                Subject = "Congratulations, we found a right match for your hiring request - " + HR_Number;

                BodyCustom = "Hello " + ClientName + ",";
                sbBody.Append(BodyCustom);
                sbBody.Append("<br/>");
                sbBody.Append("<div style='width:100%'>");
                sbBody.Append("<br/>");
                sbBody.Append("Thank you for your patience in allowing us to find the right talent for you.");
                sbBody.Append("<br/>");
                sbBody.Append("Here is a shortlisted talent for you.");
                sbBody.Append("<br/>");
                sbBody.Append("HRID: " + HR_Number + "<br/>");
                sbBody.Append("Position: " + Position + "<br/>");
                sbBody.Append("Talent Name: " + TalentName + "<br/>");
                sbBody.Append("<br/>");
                if (IsresetPassword)
                {
                    sbBody.Append("<br/>");
                    sbBody.Append("<a class='link' href='" + iConfiguration["FrontOfficeUrl"] + "ShortListed/ShortListedTalent?HiringRequest_ID=" + encryptedHRID + "&Role_ID=" + encryptedRole_ID + "' target='_blank'>Review the Talent Profile.</a>");
                    //sbBody.Append("<a class='link' href='" + Config.FrontOfficeUrl + "ClientLogin' target='_blank'>Join Uplers Talent Solutions</a>." + " <br/>");
                    sbBody.Append("<br/>");
                    sbBody.Append("LoginID : " + Username);
                    sbBody.Append("<br/>");
                    sbBody.Append("If you have misplaced your password, then you can set up a new password using forgot password option, or you can log in via OTP.");
                }
                else
                {
                    sbBody.Append("<br/>");
                    sbBody.Append("<a class='link' href='" + iConfiguration["FrontOfficeUrl"] + "SetPasswords?type=" + CommonLogic.Encrypt("C") + "&Username=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(Username)) + "&Password=" + HttpUtility.UrlEncode(CommonLogic.Encrypt(Password)) + "' target='_blank'>Click here</a> to setup your password and review the complete profile." + " <br/>");
                    sbBody.Append("<br/>");
                    sbBody.Append("LoginID : " + Username);
                }
                sbBody.Append("<br/>");
                sbBody.Append("<br/>");
                sbBody.Append("Please review the talent profile and provide some slots to book an interview. This would allow us to schedule an interview with the selected talent.");
                sbBody.Append("<br/>");
                sbBody.Append("If you need any help, please feel free to contact your point of contact on " + pointofContactEmail + ".");
                sbBody.Append("<br/>");
                sbBody.Append("<br/>");

                sbBody.Append("Thanks");
                sbBody.Append("<br/>");
                sbBody.Append("Uplers Talent Solutions Team");
                sbBody.Append("<br/>");
                sbBody.Append("<hr>");
                sbBody.Append("We are committed to your privacy. ");
                sbBody.Append("You may unsubscribe from these communications at any time. ");
                sbBody.Append("For more information, check out our <a class='link' href='" + iConfiguration["UplersPrivacyPolicyURL"] + "' target='_blank'>Privacy Policy</a>.");
                sbBody.Append("</div>");

                string TOEMAIL = ClientEmail;
                string TOEMAILNAME = ClientName;

                string InternalCCEmailID = GetCCEmailIdValues(salesHiringRequest.SalesUserId);
                string InternalCCEmailName = GetCCEmailNameValues(salesHiringRequest.SalesUserId);

                string CCEMAIL = iConfiguration["app_settings:CCEmailId"] + InternalCCEmailID;
                string CCEMAILNAME = iConfiguration["app_settings:CCEmailName"] + InternalCCEmailName;

                if (CCEMAIL != "" && TOEMAIL != "")
                {
                    try
                    {
                        Helpers.SendEmails.SendEmail(iConfiguration["app_settings:TSCEmailId"], "", TOEMAIL, TOEMAILNAME, Subject, sbBody.ToString(), AppSettingProvider.SMTPEmailName, AppSettingProvider.SMTPPasswordName, AppSettingProvider.SMTPClientName, AppSettingProvider.SMTPSSLName, AppSettingProvider.SMTPPortName, null, CCEMAILNAME, CCEMAIL, "", "", webHostEnvironment.WebRootPath);
                    }

                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                return "success";
            }
            catch (Exception e) { return e.Message; }
        }
        #endregion
    }
}