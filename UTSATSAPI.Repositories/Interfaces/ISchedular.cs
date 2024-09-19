using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface ISchedular
    {
        List<Sproc_Get_ListOfHR_For_NewCandidate_Added_Email_ClientPortal_Result> Sproc_Get_ListOfHR_For_NewCandidate_Added_Email_ClientPortal();
        Task<object> GetAllHRDataForAdmin(long HRID, bool? isAutogenerateQuestions = null);
        List<Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal_Result> Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal(int param);
        sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact(long param);
        List<Sproc_Fetch_TalentNotesEmailsLog_Result> Sproc_Fetch_TalentNotesEmailsLog();
        List<Sproc_Fetch_TalentNotesEmailsLog_HRWise_Result> Sproc_Fetch_TalentNotesEmailsLog_HRWise(string param);
        List<Sproc_Get_Engagement_Renewal_Emails_EngagementList_Result> Sproc_Get_Engagement_Renewal_Emails_EngagementList();
        Sproc_Get_Engagement_Renewal_Emails_Details_Result Sproc_Get_Engagement_Renewal_Emails_Details(string paramString);
        Sproc_Get_Summary_Emails_Result Sproc_Get_Summary_Emails(string paramString);
        List<Sproc_Get_Nurture_Email_List_Result> Sproc_Get_Nurture_Email_List();
        List<Sproc_SelfSignUpUserWithoutJobPost_ClientPortal_Result> Sproc_SelfSignUpUserWithoutJobPost_ClientPortal_List(string paramString);
        Sproc_Get_Summary_Emails_Result Sproc_Get_Nurture_Logs_Emails(string paramString);
        void UpdateNurtureLogs(string paramString);
        void Sproc_Reset_AllHR_TalentStatus();
        List<Sproc_Get_SchedularUpdates_Result> Sproc_Get_SchedularUpdates_Result();
    }
}
