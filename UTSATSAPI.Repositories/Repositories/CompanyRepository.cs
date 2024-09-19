using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class CompanyRepository : ICompany
    {

        #region Variables
        private TalentConnectAdminDBContext _db;
        #endregion

        #region Constructors
        public CompanyRepository(TalentConnectAdminDBContext db)
        {
            _db = db;
        }
        #endregion

        #region Public Methods
        public async Task<List<Sproc_UTS_GetCompanyList_Result>> GetCompanyList(string strparams)
        {
            return await _db.Set<Sproc_UTS_GetCompanyList_Result>().FromSqlRaw(string.Format("{0} {1}",
                Constants.ProcConstant.sproc_UTS_GetCompany, strparams)).ToListAsync();
        }

        public async Task<List<sproc_GetCompanyLegalInfo_Result>> GetLegalInfoList(string strparams)
        {
            return await _db.Set<sproc_GetCompanyLegalInfo_Result>().FromSqlRaw(string.Format("{0} {1}",
                Constants.ProcConstant.sproc_UTS_GetCompanyLegalInfo, strparams)).ToListAsync();
        }

        public async Task<List<Sproc_UTS_GetHubSpotCompanyList_Result>> GetHubSpotCompanyList(string strparams)
        {
            return await _db.Set<Sproc_UTS_GetHubSpotCompanyList_Result>().FromSqlRaw(string.Format("{0} {1}",
                Constants.ProcConstant.Sproc_UTS_GetHubSpotCompanyList, strparams)).ToListAsync();
        }

        /// <summary>
        /// Gets the company by hub spot company identifier.
        /// </summary>
        /// <param name="HubSpotCompanyId">The hub spot company identifier.</param>
        /// <returns></returns>
        public async Task<GenCompany?> GetCompanyByHubSpotCompanyId(long HubSpotCompanyId)
        {
            return await _db.GenCompanies.FirstOrDefaultAsync(x => x.HubSpotCompany == HubSpotCompanyId);
        }

        /// <summary>
        /// Gets the company company name and by hub spot company identifier.
        /// </summary>
        /// <param name="CompanyName">Name of the company.</param>
        /// <param name="HubSpotCompanyId">The hub spot company identifier.</param>
        /// <returns></returns>
        public async Task<GenCompany?> GetCompanyCompanyNameAndByHubSpotCompanyId(string CompanyName, long HubSpotCompanyId)
        {
            return await _db.GenCompanies.FirstOrDefaultAsync(x => x.Company.ToLower().Trim() == CompanyName.ToLower().Trim() && x.HubSpotCompany == HubSpotCompanyId);
        }

        /// <summary>
        /// Saves the company.
        /// </summary>
        /// <param name="genCompany">The gen company.</param>
        /// <returns></returns>
        public async Task<GenCompany> SaveCompany(GenCompany genCompany)
        {
            if (genCompany.Id > 0)
            {
                _db.GenCompanies.Update(genCompany);
                await _db.SaveChangesAsync();
            }
            else
            {
                await _db.GenCompanies.AddAsync(genCompany);
                await _db.SaveChangesAsync();
            }

            return genCompany;
        }

        public sproc_UTS_GetCompanyDetails_Result GetCompanyDetails(long? CompanyId)
        {
            return _db.Set<sproc_UTS_GetCompanyDetails_Result>().FromSqlRaw(String.Format("{0} {1}",Constants.ProcConstant.sproc_UTS_GetCompanyDetails, CompanyId)).ToList().AsEnumerable().FirstOrDefault();

        }
        public sproc_UTS_GetCompanyDetailsForEdit_Result GetCompanyDetailsForEdit(long? CompanyId)
        {
            return _db.Set<sproc_UTS_GetCompanyDetailsForEdit_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetCompanyDetailsForEdit, CompanyId)).ToList().AsEnumerable().FirstOrDefault();

        }

        #region New Company profile save calls
        public void Sproc_Add_Company_Funding_Details_Result(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_Company_Funding_Details, paramstring));
        }

        public void Sproc_Add_Company_CultureandPerksDetails_Result(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_Company_CultureandPerksDetails, paramstring));
        }

        public void Sproc_Add_Company_PerksDetails_Result(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_Company_PerksDetails, paramstring));
        }

        public Sproc_Update_Basic_CompanyDetails_Result UpdateCompanyBasicDetails(string paramstring)
        {
            return _db.Set<Sproc_Update_Basic_CompanyDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_Basic_CompanyDetails, paramstring)).AsEnumerable().FirstOrDefault();
        }

        public sproc_UTS_UpdateContactDetails_Result UpdateClientDetails(string paramstring)
        {
            return _db.Set<sproc_UTS_UpdateContactDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_UpdateContactDetails, paramstring)).AsEnumerable().FirstOrDefault();
        }

        public void Sproc_Add_YoutubeLink(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_YoutubeLink, paramstring));
        }

        public void UpdateCompanyEngagementDetails(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_Company_EngagementDetails, paramstring));
        }

        
        public void Delete_Company_CultureandPerksDetails(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Delete_Company_CultureandPerksDetails, paramstring));
        }

        public void Delete_Company_YoutubeDetails(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Delete_Company_YouTubeDetails, paramstring));
        }

        public void Delete_Company_PerksDetails(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Delete_Company_PerksDetails, paramstring));
        }

        public void Delete_Company_Funding_Details(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Delete_Company_Funding_Details, paramstring));
        }

        public void DeleteInsertPOCDetails(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_UpdatePOCUserIDsByCompanyID, paramstring));
        }
        #endregion

        #region New Company profile GEt calls

        public Sproc_Get_Basic_CompanyDetails_Result Sproc_Get_Basic_CompanyDetails_Result(long CompanyId)
        {
            return _db.Set<Sproc_Get_Basic_CompanyDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Basic_CompanyDetails, CompanyId)).AsEnumerable().FirstOrDefault();
        }

        public Sproc_CompanyDetail_TransferToATS_Result Sproc_CompanyDetail_TransferToATS_Result(long CompanyId)
        {
            return _db.Set<Sproc_CompanyDetail_TransferToATS_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_CompanyDetail_TransferToATS, CompanyId)).AsEnumerable().FirstOrDefault();
        }

        public List<Sproc_Get_Company_Funding_Details_Result> Sproc_Get_Company_Funding_Details_Result(long CompanyId)
        {
            return _db.Set<Sproc_Get_Company_Funding_Details_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Company_Funding_Details, CompanyId)).ToList();
        }

        public List<Sproc_Get_Company_CultureandPerksDetails_Result> Sproc_Get_Company_CultureandPerksDetails_Result(long CompanyId)
        {
            return _db.Set<Sproc_Get_Company_CultureandPerksDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Company_CultureandPerksDetails, CompanyId)).ToList();
        }

        public List<Sproc_Get_Company_PerksDetails_Result> Sproc_Get_Company_PerksDetails_Result(long CompanyId)
        {
            return _db.Set<Sproc_Get_Company_PerksDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Company_PerksDetails, CompanyId)).ToList();
        }
        public List<Sproc_Get_Company_YouTubeDetails_Result> Sproc_Get_Company_YouTubeDetails_Result(long CompanyId)
        {
            return _db.Set<Sproc_Get_Company_YouTubeDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Company_YouTubeDetails, CompanyId)).ToList();
        }
        
        public List<sp_UTS_GetPOCUserIDByCompanyID_Result> GetPOCUserIDByCompanyID(long CompanyId)
        {
            return _db.Set<sp_UTS_GetPOCUserIDByCompanyID_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sp_UTS_GetPOCUserIDByCompanyID, CompanyId)).ToList();
        }

        public sp_UTS_GetPOCUserIDByCompanyID_Edit_Result GetPOCUserIDByCompanyIDEdit(long CompanyId)
        {
            return _db.Set<sp_UTS_GetPOCUserIDByCompanyID_Edit_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sp_UTS_GetPOCUserIDByCompanyID_Edit, CompanyId)).AsEnumerable().FirstOrDefault();
        }

        public sproc_UTS_GetCompanyEngagementDetails_Result GetCompanyEngagementDetails(long CompanyId)
        {
            return _db.Set<sproc_UTS_GetCompanyEngagementDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetCompanyEngagementDetails, CompanyId)).AsEnumerable().FirstOrDefault();
        }
        #endregion

        public async Task<GenContact> ContactDetails(string emailId, long id = 0)
        {
            GenContact genContact = new GenContact();
            if (string.IsNullOrEmpty(emailId))
            {
                genContact = await _db.GenContacts.FirstOrDefaultAsync(x => x.Id == id);

            }
            else
            {
                genContact = await _db.GenContacts.FirstOrDefaultAsync(x => x.EmailId.ToLower().Equals(emailId.ToLower()));
            }
            return genContact;
        }
        public async Task<GenCompany> CompanyDetails(string companyName, long id = 0)
        {
            GenCompany genCompany = new GenCompany();
            if (string.IsNullOrEmpty(companyName))
            {
                genCompany = await _db.GenCompanies.FirstOrDefaultAsync(x => x.Id == id);

            }
            else
            {
                genCompany = await _db.GenCompanies.FirstOrDefaultAsync(x => x.Company.ToLower().Equals(companyName.ToLower()));
            }

            return genCompany;
        }
        public void sproc_Update_Company_Details_From_Scrapping_Result(string paramstring)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Update_Company_Details_From_Scrapping, paramstring));
        }

        public async Task<bool> UpdateCompanyLogo(long companyId, string logo)
        {
            bool isSuccess = false;
            var genCompany = await _db.GenCompanies.FirstOrDefaultAsync(x => x.Id == companyId);
            if (genCompany != null)
            {
                genCompany.CompanyLogo = logo;
                _db.GenCompanies.Update(genCompany);
                var result = _db.SaveChanges();
                if (result > 0)
                {
                    isSuccess = true;
                }
            }

            return isSuccess;
        }

        public async Task<Sp_UTS_PreviewJobPost_ClientPortal_Result> Sp_UTS_PreviewJobPost_ClientPortal_Result(string param)
        {
            return _db.Set<Sp_UTS_PreviewJobPost_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sp_UTS_PreviewJobPost_ClientPortal, param)).AsEnumerable().FirstOrDefault();
        }
        public Sproc_GetStandOutDetails_ClientPortal_Result GetStandOutDetailsAsync(string param)
        {
            return _db.Set<Sproc_GetStandOutDetails_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GetStandOutDetails_ClientPortal, param)).AsEnumerable().FirstOrDefault();
        }
        public void UpdatePreviewDetails(string param)
        {
            _db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sp_UTS_PreviewJobPostUpdate_ClientPortal, param));
        }

        public void UpdatePreviewDetailsUTSAdmin(string param)
        {
            _db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sp_UTS_PreviewJobPostUpdate_UTSAdmin, param));
        }


        public object GetAllHRDataForAdmin(long HiringRequest_ID)
        {
            string HRData = "";

            if (HiringRequest_ID == 0)
            {
                var table = _db.Set<Sproc_GET_ALL_HR_Details_For_PHP_API_Result>()
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
                var table = _db.Set<Sproc_GET_ALL_HR_Details_For_PHP_API_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.Sproc_GET_ALL_HR_Details_For_PHP_API} " + $"{HiringRequest_ID}, {false}")
                                .ToList().AsEnumerable().FirstOrDefault();

                if (table == null)
                {
                    return "";
                }

                //if (!string.IsNullOrEmpty(table.JDFilename))
                //{
                //    table.JDPath = System.IO.Path.Combine($"{Request.Scheme}:", Request.Host.Value, "Media", "JDParsing", "JDFiles", table.JDFilename);
                //}

                //HRData = JsonConvert.SerializeObject(table);
                HR_Data _data = new HR_Data();

                HRDetailWithSkills hRDetailWithSkills = new HRDetailWithSkills();
                hRDetailWithSkills.HR_Details = table;
                var HR_Skills_Details = _db.Set<sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI} " + $" {HiringRequest_ID} ")
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

                var HR_InterviewerDetails = _db.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == HiringRequest_ID).ToList();
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
                        InterviewerType = _db.PrgCompanyTypeofInterviewers.Where(x => x.Id == item.TypeofInterviewerId).FirstOrDefault() == null ? ""
                                                            : _db.PrgCompanyTypeofInterviewers.Where(x => x.Id == item.TypeofInterviewerId).FirstOrDefault().TypeofInterviewer,
                        InterviewerTypeID = item.TypeofInterviewerId ?? 0,
                        InterviewerYearsOfExp = item.InterviewerYearofExperience ?? 0
                    });
                }

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

                _data.HRData = hRDetailWithSkills;
                _data.Status = "200";


                return JsonConvert.SerializeObject(_data);
            }
        }

        public Sproc_Update_HrStatus_Result GetUpdateHrStatus(string param)
        {
            return _db.Set<Sproc_Update_HrStatus_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_HrStatus, param)).AsEnumerable().FirstOrDefault();
        }
        public async Task SaveGptJdresponse(GenGptJdresponse jdresponse)
        {
            var jdDumpDetails = _db.GenGptJdresponses.Add(jdresponse);
            _db.SaveChanges();

        }

        public void SaveStepInfoWithUnicode(string guid, string jobDescription)
        {
            var Guid = new SqlParameter("@Guid", SqlDbType.NVarChar) { Value = guid };
            var JobDescription = new SqlParameter("@JobDescription", SqlDbType.NVarChar) { Value = jobDescription };

            _db.Database.ExecuteSqlRaw("EXEC Sproc_Update_UnicodeValues @GUID, @JobDescription", parameters: new[] { Guid, JobDescription });

        }

        public void SaveperquisitesWithUnicode(string guid, string perquisites)
        {
            var Guid = new SqlParameter("@Guid", SqlDbType.NVarChar) { Value = guid };
            var perquisite = new SqlParameter("@Prerequisites", SqlDbType.NVarChar) { Value = perquisites };

            _db.Database.ExecuteSqlRaw("EXEC Sproc_Update_Prerequisites_UnicodeValues @GUID, @Prerequisites", parameters: new[] { Guid,  perquisite });

        }
        public void SaveCompanyDescUnicode(long CompanyID, string Aboutus,long loggedinUserId)
        {
            var Company = new SqlParameter("@CompanyID", SqlDbType.BigInt) { Value = CompanyID };
            var AboutCompany = new SqlParameter("@Aboutus", SqlDbType.NVarChar) { Value = Aboutus };
            var UserId = new SqlParameter("@LoggedInUserId", SqlDbType.BigInt) { Value = loggedinUserId };

            _db.Database.ExecuteSqlRaw("EXEC Sproc_Update_Company_UnicodeValues @CompanyID, @Aboutus, @LoggedInUserId", parameters: new[] { Company, AboutCompany, UserId });

        }

        public void SaveCultureDetailUnicode(long CompanyID,  string CultureDetail, long loggedinUserId)
        {
            var Company = new SqlParameter("@CompanyID", SqlDbType.BigInt) { Value = CompanyID };
            var Culture = new SqlParameter("@CultureDetail", SqlDbType.NVarChar) { Value = CultureDetail };
            var UserId = new SqlParameter("@LoggedInUserId", SqlDbType.BigInt) { Value = loggedinUserId };

            _db.Database.ExecuteSqlRaw("EXEC Sproc_Update_CultureDetail_UnicodeValues @CompanyID, @CultureDetail ,@LoggedInUserId", parameters: new[] { Company, Culture, UserId });

        }

        public void SaveAdditionalInfoUnicode(long CompanyID, string AdditionalInformation, long loggedinUserId)
        {
            var Company = new SqlParameter("@CompanyID", SqlDbType.BigInt) { Value = CompanyID };
            var AdditionalInfo = new SqlParameter("@AdditionalInformation", SqlDbType.NVarChar) { Value = AdditionalInformation };
            var UserId = new SqlParameter("@LoggedInUserId", SqlDbType.BigInt) { Value = loggedinUserId };

            _db.Database.ExecuteSqlRaw("EXEC Sproc_Update_AdditionalInfo_UnicodeValues @CompanyID, @AdditionalInformation,@LoggedInUserId", parameters: new[] { Company, AdditionalInfo, UserId });

        }

        public List<Sproc_HiringTypeDetails_ClientPortal_Result> GetHiringTypePricingDetails(string param)
        {
            return _db.Set<Sproc_HiringTypeDetails_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_HiringTypeDetails_ClientPortal, param)).AsEnumerable().ToList();
        }

        public List<Sproc_UTS_GetCompanyWhatsappDetails_Result> Sproc_UTS_GetCompanyWhatsappDetails(long CompanyId)
        {
            return _db.Set<Sproc_UTS_GetCompanyWhatsappDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_GetCompanyWhatsappDetails, CompanyId)).AsEnumerable().ToList();
        }

        public Sproc_UTS_SaveCompanyWhatsappDetails_Result Sproc_UTS_SaveCompanyWhatsappDetails(string param)
        {
            return _db.Set<Sproc_UTS_SaveCompanyWhatsappDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_SaveCompanyWhatsappDetails, param)).AsEnumerable().ToList().FirstOrDefault();
        }

        public void Sproc_UTS_SaveCompanyWhatsappMemberDetails(string param)
        {
            _db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_SaveCompanyWhatsappMemberDetails, param));
        }


        public List<Sproc_Get_Add_CompanyData_Send_Details_To_ATS_Result> Sproc_Get_Add_CompanyData_Send_Details_To_ATS(string param)
        {
            return _db.Set<Sproc_Get_Add_CompanyData_Send_Details_To_ATS_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Add_CompanyData_Send_Details_To_ATS, param)).ToList();
        }
        #endregion
    }
}
