using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.ATSCalls
{
    public class ATSCommonAPI
    {
        private static UTSATSAPIDBConnection context;
        private static IConfiguration config;
        private static HttpContext _httpContext;

        public ATSCommonAPI(UTSATSAPIDBConnection _context, IConfiguration _config, HttpContext httpContext)
        {
            context = _context;
            config = _config;
            _httpContext = httpContext;
        }

        //public void MakeMatchMakingAPICallToSendToATS(long HRID, int? HRStatusID, long UtsTalentID, long? ATSTalentID, decimal? FinalCost, DateTime? MatchMakingDateTime, string strTalent_RejectReason,string employeeId)
        //{
        //    var LoggedInUserID = SessionValues.LoginUserId;

        //    ContactTalentPriorityResponseModel contactTalentPriorityResponseModel = new();

        //    contactTalentPriorityResponseModel.HRID = HRID;
        //    contactTalentPriorityResponseModel.HRStatusID = HRStatusID ?? 0;
        //    var HRStatusData = context.PrgHiringRequestStatuses.FirstOrDefault(x => x.Id == HRStatusID);
        //    if (HRStatusData != null)
        //        contactTalentPriorityResponseModel.HRStatus = HRStatusData.HiringRequestStatus ?? "";

        //    outTalentDetail talentDetail = new()
        //    {
        //        ATS_TalentID = ATSTalentID ?? 0,
        //        TalentStatus = "Selected",
        //        UTS_TalentID = UtsTalentID,
        //        Talent_USDCost = FinalCost ?? 0,
        //        Talent_RejectReason = strTalent_RejectReason,
        //        RejectedBy= employeeId,
        //        MatchMakingDateTime = Convert.ToDateTime(MatchMakingDateTime).ToString("dd-MM-yyyy hh:mm:ss")
        //    };

        //    contactTalentPriorityResponseModel.TalentDetails.Add(talentDetail);

        //    var json = JsonConvert.SerializeObject(contactTalentPriorityResponseModel);

        //    ATSCall aTSCall = new(config, context);
        //    aTSCall.SendMatchMakingRecords(json, LoggedInUserID, HRID);
        //}

        //public void SendHRDetailsToPMS(long HR_ID)
        //{
        //    if (HR_ID != 0)
        //    {
        //        var HRData_Json = GetAllHRDataForAdmin(context, HR_ID);
        //        string HRJsonData = Convert.ToString(HRData_Json);
        //        if (!string.IsNullOrEmpty(HRJsonData))
        //        {
        //            bool isAPIResponseSuccess = true;

        //            try
        //            {
        //                ATSCall aTSCall = new(config, context);
        //                if (HRJsonData != "")
        //                    isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRData_Json.ToString(), HR_ID);
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    }
        //}

        //private object GetAllHRDataForAdmin(UTSATSAPIDBConnection tcw, long HiringRequest_ID)
        //{
        //    string HRData = "";

        //    if (HiringRequest_ID == 0)
        //    {
        //        var table = tcw.Set<Sproc_GET_ALL_HR_Details_For_PHP_API_Result>()
        //                        .FromSqlRaw($"{Constants.ProcConstant.Sproc_GET_ALL_HR_Details_For_PHP_API} " + $"{null}, {true}")
        //                        .ToList();

        //        if (table.Count == 0)
        //        {
        //            return string.Empty;
        //        }

        //        HRData = JsonConvert.SerializeObject(table);
        //        return HRData;

        //    }
        //    else
        //    {
        //        var table = tcw.Set<Sproc_GET_ALL_HR_Details_For_PHP_API_Result>()
        //                        .FromSqlRaw($"{Constants.ProcConstant.Sproc_GET_ALL_HR_Details_For_PHP_API} " + $"{HiringRequest_ID}, {false}")
        //                        .ToList().AsEnumerable().FirstOrDefault();

        //        if (table == null)
        //        {
        //            return "";
        //        }

        //        if (!string.IsNullOrEmpty(table.JDFilename))
        //        {
        //            table.JDPath = System.IO.Path.Combine($"{_httpContext.Request.Scheme}:", _httpContext.Request.Host.Value, "Media", "JDParsing", "JDFiles", table.JDFilename);
        //        }

        //        //HRData = JsonConvert.SerializeObject(table);
        //        HR_Data _data = new HR_Data();

        //        HRDetailWithSkills hRDetailWithSkills = new HRDetailWithSkills();
        //        hRDetailWithSkills.HR_Details = table;
        //        var HR_Skills_Details = tcw.Set<sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI_Result>()
        //                        .FromSqlRaw($"{Constants.ProcConstant.sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI} " + $" {HiringRequest_ID} ")
        //                        .ToList();

        //        foreach (var item in HR_Skills_Details)
        //        {
        //            hRDetailWithSkills.skill.Add(new SkillDetail
        //            {
        //                Proficiency = item.Proficiency,
        //                SkillName = item.Skill,
        //                TempSkillName = item.TempSkill
        //            });
        //        }

        //        var HR_InterviewerDetails = tcw.GenSalesHiringRequestInterviewerDetails.AsNoTracking().Where(x => x.HiringRequestId == HiringRequest_ID).ToList();
        //        foreach (var item in HR_InterviewerDetails)
        //        {
        //            hRDetailWithSkills.HR_InterviewerDetails.Add(new HR_InterviewerDetail
        //            {
        //                ID = item.Id,
        //                HiringRequestID = item.HiringRequestId ?? 0,
        //                InterviewerDesignation = item.InterviewerDesignation,
        //                InterviewerEmailID = item.InterviewerEmailId,
        //                InterviewerLinkedIn = item.InterviewLinkedin,
        //                InterviewerName = item.InterviewerName,
        //                InterviewerType = tcw.PrgCompanyTypeofInterviewers.AsNoTracking().Where(x => x.Id == item.TypeofInterviewerId).FirstOrDefault() == null ? ""
        //                                                    : tcw.PrgCompanyTypeofInterviewers.AsNoTracking().Where(x => x.Id == item.TypeofInterviewerId).FirstOrDefault().TypeofInterviewer,
        //                InterviewerTypeID = item.TypeofInterviewerId ?? 0,
        //                InterviewerYearsOfExp = item.InterviewerYearofExperience ?? 0
        //            });
        //        }

        //        try
        //        {
        //            VitalInfo vitalInfo = new VitalInfo();

        //            string? compensationOptionStr = table.CompensationOption;
        //            string[] compensationOption = new string[1000];

        //            if (compensationOptionStr != null)
        //            {
        //                compensationOption = compensationOptionStr.Split('^');
        //                vitalInfo.CompensationOption = compensationOption;
        //            }

        //            string? candidateIndustryStr = table.CandidateIndustry;
        //            string[] candidateIndustry = new string[1000];
        //            if (candidateIndustryStr != null)
        //            {
        //                candidateIndustry = candidateIndustryStr.Split('^');
        //                vitalInfo.CandidateIndustry = candidateIndustry;
        //            }

        //            vitalInfo.Prerequisites = table.Prerequisites;
        //            vitalInfo.HasPeopleManagementExp = table.HasPeopleManagementExp;

        //            _data.VitalInformation = vitalInfo;
        //        }
        //        catch
        //        {

        //        }

        //        _data.HRData = hRDetailWithSkills;
        //        _data.Status = "200";


        //        return JsonConvert.SerializeObject(_data);
        //    }
        //}
    }
}
