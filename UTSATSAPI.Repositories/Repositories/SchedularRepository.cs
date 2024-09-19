using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;
using UTSATSAPI.Helpers;
using Newtonsoft.Json;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Helpers.Common;

namespace UTSATSAPI.Repositories.Repositories
{
   
    public class SchedularRepository : ISchedular
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public SchedularRepository(TalentConnectAdminDBContext _db, IHttpContextAccessor httpContextAccessor)
        {
            this.db = _db;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        public List<Sproc_Get_ListOfHR_For_NewCandidate_Added_Email_ClientPortal_Result> Sproc_Get_ListOfHR_For_NewCandidate_Added_Email_ClientPortal()
        {
            return db.Set<Sproc_Get_ListOfHR_For_NewCandidate_Added_Email_ClientPortal_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Get_ListOfHR_For_NewCandidate_Added_Email_ClientPortal)).AsEnumerable().ToList();
        }

        public async Task<object> GetAllHRDataForAdmin(long HRID, bool? isAutogenerateQuestions = null)
        {
            string HRData = "";

            if (HRID == 0)
            {
                var table = db.Set<Sproc_GET_ALL_HR_Details_For_PHP_API_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.Sproc_GET_ALL_HR_Details_For_PHP_API} " + $"{null}, {true}")
                                .ToList();

                if (table.Count == 0)
                {
                    return string.Empty;
                }

                HRData = JsonConvert.SerializeObject(table);
                return HRData;

            }
            else
            {
                var table = db.Set<Sproc_GET_ALL_HR_Details_For_PHP_API_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.Sproc_GET_ALL_HR_Details_For_PHP_API} " + $"{HRID}, {false}")
                                .ToList().AsEnumerable().FirstOrDefault();

                if (table == null)
                {
                    return "";
                }

                if (!string.IsNullOrEmpty(table.JDFilename))
                {
                    table.JDPath = System.IO.Path.Combine($"{_httpContextAccessor.HttpContext.Request.Scheme}:", _httpContextAccessor.HttpContext.Request.Host.Value, "Media", "JDParsing", "JDFiles", table.JDFilename);
                }

                //HRData = JsonConvert.SerializeObject(table);
                HR_Data _data = new HR_Data();

                HRDetailWithSkills hRDetailWithSkills = new HRDetailWithSkills();
                hRDetailWithSkills.HR_Details = table;
                var HR_Skills_Details = db.Set<sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI} " + $" {HRID} ")
                                .ToList();

                foreach (var item in HR_Skills_Details)
                {
                    hRDetailWithSkills.skill.Add(new SkillDetail
                    {
                        Proficiency = item.Proficiency,
                        SkillName = item.Skill,
                        TempSkillName = item.TempSkill
                    });
                }

                var HR_InterviewerDetails = db.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == HRID).ToList();
                foreach (var item in HR_InterviewerDetails)
                {
                    hRDetailWithSkills.HR_InterviewerDetails.Add(new HR_InterviewerDetail
                    {
                        ID = item.Id,
                        HiringRequestID = item.HiringRequestId ?? 0,
                        InterviewerDesignation = item.InterviewerDesignation,
                        InterviewerEmailID = item.InterviewerEmailId,
                        InterviewerLinkedIn = item.InterviewLinkedin,
                        InterviewerName = item.InterviewerName,
                        InterviewerType = db.PrgCompanyTypeofInterviewers.Where(x => x.Id == item.TypeofInterviewerId).FirstOrDefault() == null ? ""
                                                            : db.PrgCompanyTypeofInterviewers.Where(x => x.Id == item.TypeofInterviewerId).FirstOrDefault().TypeofInterviewer,
                        InterviewerTypeID = item.TypeofInterviewerId ?? 0,
                        InterviewerYearsOfExp = item.InterviewerYearofExperience ?? 0
                    });
                }

                #region Vital info
                try
                {
                    VitalInfo vitalInfo = new VitalInfo();

                    string? compensationOptionStr = table.CompensationOption;
                    string[] compensationOption = new string[1000];

                    if (compensationOptionStr != null)
                    {
                        compensationOption = compensationOptionStr.Split('^');
                        vitalInfo.CompensationOption = compensationOption;
                    }

                    string? candidateIndustryStr = table.CandidateIndustry;
                    string[] candidateIndustry = new string[1000];
                    if (candidateIndustryStr != null)
                    {
                        candidateIndustry = candidateIndustryStr.Split('^');
                        vitalInfo.CandidateIndustry = candidateIndustry;
                    }

                    vitalInfo.Prerequisites = table.Prerequisites;
                    vitalInfo.HasPeopleManagementExp = table.HasPeopleManagementExp;

                    _data.VitalInformation = vitalInfo;
                }
                catch
                {

                }
                #endregion

                #region Send HRPOC to ATS.
                try
                {
                    object[] pocParam = new object[] { 0, "", HRID, "", 0, false };
                    string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                    List<Sproc_HR_POC_ClientPortal_Result> hrPOCs = db.Set<Sproc_HR_POC_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_HR_POC_ClientPortal, pocParamString)).AsEnumerable().ToList();

                    List<HRPOC> hRPOCs = new List<HRPOC>();
                    foreach (var i in hrPOCs)
                    {
                        HRPOC hRPOC = new HRPOC();
                        hRPOC.EmailID = i.EmailID;
                        hRPOC.FullName = i.FullName;
                        hRPOC.ContactNo = i.ContactNo;
                        hRPOC.ShowEmailToTalent = i.ShowEmailToTalent;
                        hRPOC.ShowContactNumberToTalent = i.ShowContactNumberToTalent;
                        hRPOCs.Add(hRPOC);
                    }
                    _data.HRPOC = hRPOCs;
                }
                catch
                {

                }
                #endregion

                _data.HRData = hRDetailWithSkills;

                _data.question_regeneration_needed = isAutogenerateQuestions;

                _data.Status = "200";


                return JsonConvert.SerializeObject(_data);
            }
        }

        public List<Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal_Result> Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal(int param)
        {
            return db.Set<Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal, param)).AsEnumerable().ToList();
        }
        public sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact(long param)
        {
            return db.Set<sproc_Get_ContactPointofContact_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Get_ContactPointofContact, param)).AsEnumerable().FirstOrDefault();
        }

        public List<Sproc_Fetch_TalentNotesEmailsLog_HRWise_Result> Sproc_Fetch_TalentNotesEmailsLog_HRWise(string param)
        {
            return db.Set<Sproc_Fetch_TalentNotesEmailsLog_HRWise_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Fetch_TalentNotesEmailsLog_HRWise, param)).AsEnumerable().ToList();
            
        }

        public List<Sproc_Fetch_TalentNotesEmailsLog_Result> Sproc_Fetch_TalentNotesEmailsLog()
        {
            return db.Set<Sproc_Fetch_TalentNotesEmailsLog_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Fetch_TalentNotesEmailsLog)).AsEnumerable().ToList();

        }

        public List<Sproc_Get_Engagement_Renewal_Emails_EngagementList_Result> Sproc_Get_Engagement_Renewal_Emails_EngagementList()
        {
            return db.Set<Sproc_Get_Engagement_Renewal_Emails_EngagementList_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Get_Engagement_Renewal_Emails_EngagementList)).AsEnumerable().ToList();
        }

        public Sproc_Get_Engagement_Renewal_Emails_Details_Result Sproc_Get_Engagement_Renewal_Emails_Details(string paramString)
        {
            return db.Set<Sproc_Get_Engagement_Renewal_Emails_Details_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Engagement_Renewal_Emails_Details, paramString)).AsEnumerable().FirstOrDefault();
        }

        public Sproc_Get_Summary_Emails_Result Sproc_Get_Summary_Emails(string paramString)
        {
            return db.Set<Sproc_Get_Summary_Emails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Summary_Emails,paramString)).AsEnumerable().FirstOrDefault();
        }

        public List<Sproc_Get_Nurture_Email_List_Result> Sproc_Get_Nurture_Email_List()
        {
            return db.Set<Sproc_Get_Nurture_Email_List_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Get_Nurture_Email_List)).AsEnumerable().ToList();
        }

        public List<Sproc_SelfSignUpUserWithoutJobPost_ClientPortal_Result> Sproc_SelfSignUpUserWithoutJobPost_ClientPortal_List(string paramString)
        {
            return db.Set<Sproc_SelfSignUpUserWithoutJobPost_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_SelfSignUpUserWithoutJobPost_ClientPortal, paramString)).AsEnumerable().ToList();
        }

        public Sproc_Get_Summary_Emails_Result Sproc_Get_Nurture_Logs_Emails(string paramString)
        {
            return db.Set<Sproc_Get_Summary_Emails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Nurture_Logs_Emails, paramString)).AsEnumerable().FirstOrDefault();
        }

        public void UpdateNurtureLogs(string paramString)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UpdateNurtureLogs, paramString));
        }

        public void Sproc_Reset_AllHR_TalentStatus()
        {
            bool isFromSchedular = true;
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Reset_AllHR_TalentStatus, isFromSchedular.ToString()));
        }

        public List<Sproc_Get_SchedularUpdates_Result> Sproc_Get_SchedularUpdates_Result()
        {
            return db.Set<Sproc_Get_SchedularUpdates_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Get_SchedularUpdates)).AsEnumerable().ToList();
        }
    }
}
