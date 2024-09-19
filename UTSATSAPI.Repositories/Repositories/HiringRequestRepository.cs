namespace UTSATSAPI.Repositories.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using UTSATSAPI.ComplexTypes;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Repositories.Interfaces;
    using Microsoft.AspNetCore.Http;
    using System.Dynamic;
    using UTSATSAPI.Helpers.Common;

    public class HiringRequestRepository : IHiringRequest
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion


        #region Constructor
        public HiringRequestRepository(TalentConnectAdminDBContext _db, IHttpContextAccessor httpContextAccessor)
        {
            this.db = _db;
            _httpContextAccessor = httpContextAccessor;
        }



        #endregion


        #region Public Methods
        public void sproc_Interviewdetailsdbrief(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Interviewdetailsdbrief, param));
        }

        public Sproc_GetCompanyDetail_Result Sproc_GetCompanyDetail(string param)
        {
            return db.Set<Sproc_GetCompanyDetail_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_GetCompanyDetail, param)).AsEnumerable().FirstOrDefault();
        }

        public sproc_FetchHiringInterviewerDetails_Result sproc_FetchHiringInterviewerDetails(string param)
        {
            return db.Set<sproc_FetchHiringInterviewerDetails_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_FetchHiringInterviewerDetails, param)).FirstOrDefault();
        }

        public void sproc_Interviewdetailsdb(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_GetInterviewDetails, param));
        }

        public List<sproc_ViewTalent_Result> sproc_UTS_ViewTalent(string param)
        {
            return db.Set<sproc_ViewTalent_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_ViewTalent, param)).ToList();

        }

        public sproc_UTS_getCompanyNameByHiringRequestID sproc_UTS_getCompanyNameByHiringRequestID(string param)
        {
            return db.Set<sproc_UTS_getCompanyNameByHiringRequestID>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_getCompanyNameByHiringRequestID, param)).AsEnumerable().FirstOrDefault();
        }

        public sproc_UTS_GetTalentProfileLog_Result sproc_UTS_GetTalentProfileLog_Result(string param)
        {
            return db.Set<sproc_UTS_GetTalentProfileLog_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetTalentProfileLog, param)).AsEnumerable().FirstOrDefault();
        }

        public List<sproc_UTS_GetProfileShareDetail_Result> sproc_UTS_GetProfileShareDetail_Result(string param)
        {
            return db.Set<sproc_UTS_GetProfileShareDetail_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetProfileShareDetail, param)).ToList();
        }

        public List<sproc_UTS_GetTalentScoreCard_Result> sproc_UTS_GetTalentScoreCard_Result(string param)
        {
            return db.Set<sproc_UTS_GetTalentScoreCard_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetTalentScoreCard, param)).ToList();
        }

        public sproc_GetOnBoardData_Result sproc_GetOnBoardData_Result(string param)
        {
            return db.Set<sproc_GetOnBoardData_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_GetOnBoardData_Result, param)).AsEnumerable().FirstOrDefault();
        }
        public List<sproc_UTS_GetAutoCompleteContacts_Result> sproc_UTS_GetAutoCompleteContacts_Result(string param)
        {
            return db.Set<sproc_UTS_GetAutoCompleteContacts_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetAutoCompleteContacts, param)).AsEnumerable().ToList();
        }

        public List<sproc_UTS_GetAutoCompleteCompany_Result> sproc_UTS_GetAutoCompleteCompany_Result(string param)
        {
            return db.Set<sproc_UTS_GetAutoCompleteCompany_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetAutoCompleteCompany, param)).AsEnumerable().ToList();
        }

        public List<Sproc_GET_PostAcceptance_detail_Result> Sproc_GET_PostAcceptance_detail(string param)
        {
            return db.Set<Sproc_GET_PostAcceptance_detail_Result>().FromSqlRaw(String.Format("{0} '{1}'", Constants.ProcConstant.Sproc_GET_PostAcceptance_detail, param)).ToList();
        }

        public List<Sproc_GET_PostAcceptance_detail_Availability_Result> Sproc_GET_PostAcceptance_detail_Availability(string param)
        {
            return db.Set<Sproc_GET_PostAcceptance_detail_Availability_Result>().FromSqlRaw(String.Format("{0} '{1}'", Constants.ProcConstant.Sproc_GET_PostAcceptance_detail_Availability, param)).ToList();
        }

        public List<Sproc_GET_PostAcceptance_detail_HowSoon_Result> Sproc_GET_PostAcceptance_detail_HowSoon(string param)
        {
            return db.Set<Sproc_GET_PostAcceptance_detail_HowSoon_Result>().FromSqlRaw(String.Format("{0} '{1}'", Constants.ProcConstant.Sproc_GET_PostAcceptance_detail_HowSoon, param)).ToList();
        }
        public async Task<sproc_CloneHRFromExistHR_Result> sproc_CloneHRFromExistHR(string param)
        {
            var data = await db.Set<sproc_CloneHRFromExistHR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_CloneHRFromExistHR, param)).ToListAsync();

            return data.FirstOrDefault();
        }

        public void sproc_UTS_UpdatePartnershipDetails_ForHR(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_UpdatePartnershipDetails_ForHR, param));
        }

        public List<sproc_UTS_GetChildCompanyList_Result> sproc_UTS_GetChildCompanyList(string param)
        {
            return db.Set<sproc_UTS_GetChildCompanyList_Result>().FromSqlRaw(String.Format("{0} '{1}'", Constants.ProcConstant.sproc_UTS_GetChildCompanyList, param)).ToList();
        }

        /// <summary>
        /// Sprocs the talent hr cancelled hr list.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public List<sproc_TalentHR_CancelledHR_List_Result> sproc_TalentHR_CancelledHR_List(string param)
        {
            return db.Set<sproc_TalentHR_CancelledHR_List_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_TalentHR_CancelledHR_List, param)).ToList();
        }

        /// <summary>
        /// UTS-3256: Validates and sends the proper output for closing HR
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Sproc_Update_Status_For_Closed_HR_Result sproc_Update_Status_For_Clsoed_HR(string param)
        {
            return db.Set<Sproc_Update_Status_For_Closed_HR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Update_Status_For_Closed_HR, param)).AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// UTS-3256: Close/Cancel/loss the HR as per the condition.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Sproc_Check_Validation_Message_For_Close_HR_Result> Sproc_Check_Validation_Message_For_Close_HR(string param)
        {
            return db.Set<Sproc_Check_Validation_Message_For_Close_HR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Check_Validation_Message_For_Close_HR, param)).ToList();
        }

        /// <summary>
        /// UTS-3641: Reopen the HR.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Sproc_Update_Status_For_Reopen_HR_Result sproc_Update_Status_For_Reopen_HR(string param)
        {
            return db.Set<Sproc_Update_Status_For_Reopen_HR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Update_Status_For_Reopen_HR, param)).AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// UTS-6449 Repost feature for credit base HR in UTS Admin which like Reopen HR
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Sproc_RepostedJob_ClientPortal_Result Sproc_RepostedJob_ClientPortal(string param)
        {
            return db.Set<Sproc_RepostedJob_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_RepostedJob_ClientPortal, param)).AsEnumerable().FirstOrDefault();
        }

        public Sproc_Get_HRDetails_From_DealId_Result Sproc_Get_HRDetails_From_DealId_Result(string param)
        {
            return db.Set<Sproc_Get_HRDetails_From_DealId_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_HRDetails_From_DealId, param)).AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// UTS-3998: Update the HR details from the special users and COE team.
        /// </summary>
        /// <param name="param"></param>
        public void Sproc_Update_HRDetails_From_Special_User(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_HRDetails_From_Special_User, param));
        }  

        /// <summary>
        /// Get the latest details of the HR SLA when data updated.
        /// </summary>
        /// <returns></returns>
        public async Task<List<sproc_Get_HiringRequest_SLADetails_Result>> Get_HiringRequest_SLADetails(long HRID)
        {
            Sproc_Get_Latest_ATS_HRStatus_Details_For_Notification_Result? result = 
                db.Set<Sproc_Get_Latest_ATS_HRStatus_Details_For_Notification_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Latest_ATS_HRStatus_Details_For_Notification, HRID)).ToList().AsEnumerable().FirstOrDefault();

            if (result != null)
            {
                string param = $"{result.HRID}";
                return await db.Set<sproc_Get_HiringRequest_SLADetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Get_HiringRequest_SLADetails, param)).ToListAsync().ConfigureAwait(false);                

            }

            return null;
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

        public sp_UTS_GetStandOutDetails_Result sp_UTS_GetStandOutDetails(string param)
        {
            return db.Set<sp_UTS_GetStandOutDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sp_UTS_GetStandOutDetails, param)).AsEnumerable().FirstOrDefault();
        }

        public List<Sproc_FetchSimilarRoles_UTS_Result> Sproc_FetchSimilarRoles_UTS(string param)
        {
            return db.Set<Sproc_FetchSimilarRoles_UTS_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_FetchSimilarRoles_UTS, param)).AsEnumerable().ToList();
        }

        public Sp_UTS_GetCreditUtilization_BasedOnHR_Result GetCreditUtilization_BasedOnHR(long HRID)
        {
            return db.Set<Sp_UTS_GetCreditUtilization_BasedOnHR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sp_UTS_GetCreditUtilization_BasedOnHR, HRID)).AsEnumerable().FirstOrDefault();
        }

        public Sproc_Update_HrStatus_Result GetUpdateHrStatus(string param)
        {
            return db.Set<Sproc_Update_HrStatus_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_HrStatus, param)).AsEnumerable().FirstOrDefault();
        }

        public List<Sproc_UTS_GetCloseHRHistory_Result> Sproc_UTS_GetCloseHRHistory(string param)
        {
            return db.Set<Sproc_UTS_GetCloseHRHistory_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_GetCloseHRHistory, param)).ToList();
        }

        public Sproc_UTS_CheckCreditAvailablilty_Result CheckCreditAvailablilty(long CompanyID)
        {
            return db.Set<Sproc_UTS_CheckCreditAvailablilty_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_CheckCreditAvailablilty, CompanyID)).AsEnumerable().FirstOrDefault();
        }
        public Sproc_UTS_AddEdit_HR_Result AddEdit_HR(string param)
        {
            return db.Set<Sproc_UTS_AddEdit_HR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_AddEdit_HR, param)).AsEnumerable().FirstOrDefault();
        }
        public void Sproc_UTS_DeleteTestHR(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_DeleteTestHR, param));
        }

        public async Task<sproc_CloneHRFromExistHR_Result> sproc_CloneHRFromExistHRDemoAccount(string param)
        {
            var data = await db.Set<sproc_CloneHRFromExistHR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_CloneHRFromExistHR_DemoAccount, param)).ToListAsync();

            return data.FirstOrDefault();
        }

        public List<Sproc_SearchLocation_Result> sproc_UTS_GetAutoComplete_CityStateWise(string param)
        {
            return db.Set<Sproc_SearchLocation_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_SearchLocation, param)).AsEnumerable().ToList();
        }

        public List<Sproc_Get_NearByMAppingCities_Result> sproc_UTS_GetNearByCities(string param)
        {
            return db.Set<Sproc_Get_NearByMAppingCities_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_NearByMAppingCities, param)).AsEnumerable().ToList();
        }

        public List<Sproc_HR_POC_ClientPortal_Result> SaveandGetHRPOCDetails(string param)
        {
            return db.Set<Sproc_HR_POC_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_HR_POC_ClientPortal, param)).AsEnumerable().ToList();
        }
        
        public sproc_UTS_HRClose_CheckTalentOfferHiredwithMessage_Result GetWarningMsg(long HRID)
        {
            return db.Set<sproc_UTS_HRClose_CheckTalentOfferHiredwithMessage_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_HRClose_CheckTalentOfferHiredwithMessage, HRID)).AsEnumerable().FirstOrDefault();
        }
        public List<Sproc_Get_Frequency_Office_Visit_Result> Sproc_Get_Frequency_Office_Visit()
        {
            return db.Set<Sproc_Get_Frequency_Office_Visit_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Get_Frequency_Office_Visit)).AsEnumerable().ToList();
        }

        public Sproc_AddUpdate_TalentNotes_ClientPortal_Result Sproc_AddUpdate_TalentNotes_ClientPortal(string param)
        {            
           return db.Set<Sproc_AddUpdate_TalentNotes_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_AddUpdate_TalentNotes_ClientPortal, param)).AsEnumerable().FirstOrDefault();
        }

        public List<Sproc_SearchLocationOnly_Result> sproc_UTS_GetAutoComplete_CityWise(string param)
        {
            return db.Set<Sproc_SearchLocationOnly_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_SearchLocationOnly, param)).AsEnumerable().ToList();
        }

        public List<Sproc_Get_ShortListedDetails_ForCreditFlow_ClientPortal_Result> Get_ShortListedDetails_ForCreditFlow_ClientPortals(long hrid)
        {
            return db.Set<Sproc_Get_ShortListedDetails_ForCreditFlow_ClientPortal_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_ShortListedDetails_ForCreditFlow_ClientPortal, hrid)).AsEnumerable().ToList();
        }

        #endregion
    }
}
