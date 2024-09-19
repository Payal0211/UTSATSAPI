using Aspose.Words;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces.UpChat;

namespace UTSATSAPI.Repositories.Repositories.UpChat
{
    public class UpchatEmailRepository : IEmail
    {
        #region Variables

        private TalentConnectAdminDBContext db;
        private readonly IConfiguration iConfiguration;

        #endregion

        #region Constructors

        public UpchatEmailRepository(TalentConnectAdminDBContext _db, IConfiguration _iConfiguration)
        {
            db = _db;
            iConfiguration = _iConfiguration;
        }

        #endregion

        #region public
        public async Task<string> SendEmail_UserTagInChat(long? hrID, string assignedUsers, string notes, string userEmployeeID)
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

                    Subject = "New Chat has been added - " + _SalesHiringRequest.HrNumber;
                    BodyCustom = "Hi,";
                    sbBody.Append(BodyCustom);
                    sbBody.Append("<br/>");
                    sbBody.Append(LoggedInUsername + " has added a chat for you.");

                    sbBody.Append("<div style='width:100%'>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Chat: <a class='link' href = '" + iConfiguration["NewAdminProjectURL"].ToString().Trim() + "allhiringrequest/" + hrID + "' target='_blank' >" + notes + " </a>");
                    sbBody.Append("<br/>");
                    sbBody.Append("Click here to view or add any new chat for this HR");
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
                        string removeEmail = CCEMAIL.Replace("bhuvan@uplers.com", "").Replace(",,",",");
                        string removeEmailName = CCEMAILNAME.Replace("Bhuvan", "").Replace(",,", ",");

                        emailOperator.SetCCEmail(removeEmail, removeEmailName);
                        emailOperator.SetSubject(Subject);
                        emailOperator.SetBody(sbBody.ToString());

                        #region SendEmail
                        if (!string.IsNullOrEmpty(Subject))
                            emailOperator.SendEmailForUpchat();

                        #endregion
                    }
                    return "success";
                }
                else
                    return "error";
            }
            catch (Exception e) { return e.Message; }
        }
        public async Task<string> SendEmail_AddUserInChannel(long? hrID, string userEmpId)
        {
            try
            {
                string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "", LoggedInUsername = "", AssignedUserEmailId = "", AssignedUserEmailName = "", assignedUser = "";
                StringBuilder sbBody = new StringBuilder();
                bool IsAdHoc, IsManaged = false;

                LoggedInUsername = await db.UsrUsers.AsNoTracking().Where(x => x.Id == SessionValues.LoginUserId).Select(x => x.FullName).FirstOrDefaultAsync();
                UsrUser _addedUser = await db.UsrUsers.AsNoTracking().Where(x => x.EmployeeId == userEmpId).FirstOrDefaultAsync();

                GenSalesHiringRequest _SalesHiringRequest = db.GenSalesHiringRequests.AsNoTracking().Where(t => t.Id == hrID).FirstOrDefault();
                
                if (_SalesHiringRequest != null && _addedUser != null)
                {
                    SalesPersonName = await db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.FullName).FirstOrDefaultAsync();
                    salesPersonEmail = await db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.EmailId).FirstOrDefaultAsync();
                    
                    assignedUser = _addedUser.Id.ToString();
                    
                    IsAdHoc = _SalesHiringRequest.IsAdHocHr;
                    IsManaged = _SalesHiringRequest.IsManaged;

                    Subject = "New Member has been added into channel (" + _SalesHiringRequest.HrNumber + ")";
                    BodyCustom = "Hi, " +  _addedUser.Username;
                    sbBody.Append(BodyCustom);
                    sbBody.Append("<br/>");
                    sbBody.Append(LoggedInUsername + " has added you in channel. Welcome!");

                    sbBody.Append("<div style='width:100%'>");
                    sbBody.Append("<br/>");
                    sbBody.Append("<a class='link' href = '" + iConfiguration["NewAdminProjectURL"].ToString().Trim() + "allhiringrequest/" + hrID + "' target='_blank' >Click here to view channel for this HR</a>");
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
                    if (assignedUser != null && assignedUser != "")
                    {
                        AssignedUserEmailId = GetAssignedUsersCCEmailIdValues(assignedUser);
                        AssignedUserEmailName = GetAssignedUsersCCEmailNameValues(assignedUser);
                    }
                    string TOEMAIL = "";
                    string TOEMAILNAME = "";

                    if (assignedUser != null && assignedUser != "")
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
        public async Task<string> SendEmail_RemoveUserInChannel(long? hrID, string userEmpId)
        {
            try
            {
                string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "", LoggedInUsername = "", AssignedUserEmailId = "", AssignedUserEmailName = "", assignedUser = "";
                StringBuilder sbBody = new StringBuilder();
                bool IsAdHoc, IsManaged = false;

                LoggedInUsername = db.UsrUsers.AsNoTracking().Where(x => x.Id == SessionValues.LoginUserId).Select(x => x.FullName).FirstOrDefault();
                UsrUser _removedUser = db.UsrUsers.AsNoTracking().Where(x => x.EmployeeId == userEmpId).FirstOrDefault();
                
                GenSalesHiringRequest _SalesHiringRequest = db.GenSalesHiringRequests.AsNoTracking().Where(t => t.Id == hrID).FirstOrDefault();
                
                if (_SalesHiringRequest != null && _removedUser != null)
                {
                    SalesPersonName = db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.FullName).FirstOrDefault();
                    salesPersonEmail = db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.EmailId).FirstOrDefault();

                    assignedUser = _removedUser.Id.ToString();

                    IsAdHoc = _SalesHiringRequest.IsAdHocHr;
                    IsManaged = _SalesHiringRequest.IsManaged;

                    Subject = "Member has been removed from channel (" + _SalesHiringRequest.HrNumber + ")";
                    BodyCustom = "Hi, " + _removedUser.Username;
                    sbBody.Append(BodyCustom);
                    sbBody.Append("<br/>");
                    sbBody.Append(LoggedInUsername + " has removed you from channel.");

                    sbBody.Append("<div style='width:100%'>");
                    sbBody.Append("<br/>");
                    sbBody.Append("<a class='link' href = '" + iConfiguration["NewAdminProjectURL"].ToString().Trim() + "allhiringrequest/" + hrID + "' target='_blank' >Click here to view this HR</a>");
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
                    if (assignedUser != null && assignedUser != "")
                    {
                        AssignedUserEmailId = GetAssignedUsersCCEmailIdValues(assignedUser);
                        AssignedUserEmailName = GetAssignedUsersCCEmailNameValues(assignedUser);
                    }
                    string TOEMAIL = "";
                    string TOEMAILNAME = "";

                    if (assignedUser != null && assignedUser != "")
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
        public async Task<string> SendEmail_LeftUserInChannel(long? hrID, string userEmpId)
        {
            try
            {
                string Subject = "", BodyCustom = "", SalesPersonName = "", salesPersonEmail = "", LoggedInUsername = "", AssignedUserEmailId = "", AssignedUserEmailName = "", assignedUser = "";
                StringBuilder sbBody = new StringBuilder();
                bool IsAdHoc, IsManaged = false;

                LoggedInUsername = db.UsrUsers.AsNoTracking().Where(x => x.Id == SessionValues.LoginUserId).Select(x => x.FullName).FirstOrDefault();
                UsrUser _removedUser = db.UsrUsers.AsNoTracking().Where(x => x.EmployeeId == userEmpId).FirstOrDefault();

                GenSalesHiringRequest _SalesHiringRequest = db.GenSalesHiringRequests.AsNoTracking().Where(t => t.Id == hrID).FirstOrDefault();

                if (_SalesHiringRequest != null && _removedUser != null)
                {
                    SalesPersonName = db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.FullName).FirstOrDefault();
                    salesPersonEmail = db.UsrUsers.AsNoTracking().Where(x => x.Id == _SalesHiringRequest.SalesUserId).Select(x => x.EmailId).FirstOrDefault();

                    assignedUser = _removedUser.Id.ToString();

                    IsAdHoc = _SalesHiringRequest.IsAdHocHr;
                    IsManaged = _SalesHiringRequest.IsManaged;

                    Subject = "Member has left from channel (" + _SalesHiringRequest.HrNumber + ")";
                    BodyCustom = "Hi, " + _removedUser.Username;
                    sbBody.Append(BodyCustom);
                    sbBody.Append("<br/>");
                    sbBody.Append(LoggedInUsername + " has left from channel.");

                    sbBody.Append("<div style='width:100%'>");
                    sbBody.Append("<br/>");
                    sbBody.Append("<a class='link' href = '" + iConfiguration["NewAdminProjectURL"].ToString().Trim() + "allhiringrequest/" + hrID + "' target='_blank' >Click here to view this HR</a>");
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
                    if (assignedUser != null && assignedUser != "")
                    {
                        AssignedUserEmailId = GetAssignedUsersCCEmailIdValues(assignedUser);
                        AssignedUserEmailName = GetAssignedUsersCCEmailNameValues(assignedUser);
                    }
                    string TOEMAIL = "";
                    string TOEMAILNAME = "";

                    if (assignedUser != null && assignedUser != "")
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
        #endregion

        #region Private
        private string GetCCEmailIdValues(long? salesUSerID)
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
        private string GetCCEmailNameValues(long? salesUSerID)
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
        private string GetAssignedUsersCCEmailIdValues(string assignedUsersIds)
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
        private string GetAssignedUsersCCEmailNameValues(string assignedUsersIds)
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

        #endregion
    }
}
