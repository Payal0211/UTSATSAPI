using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    /// <summary>
    /// JDParseRepository
    /// </summary>
    /// <seealso cref="UTSATSAPI.Repositories.Interfaces.IViewAllHR" />
    public class JDParseRepository : IJDParse
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public JDParseRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }
        #endregion

        #region Public Methods

        public List<Sproc_GetExtractedSkills_FromJD_Result> Sproc_GetExtractedSkills_FromJD(string param)
        {
            return db.Set<Sproc_GetExtractedSkills_FromJD_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_GetExtractedSkills_FromJD, param)).ToList();
        }

        public Sproc_DumpJDDetailsintoTempTable_Result Sproc_DumpJDDetailsintoTempTable(string param)
        {
            return db.Set<Sproc_DumpJDDetailsintoTempTable_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_DumpJDDetailsintoTempTable, param)).AsEnumerable().FirstOrDefault();
        }
        public Sproc_Get_JDskillDataByJDDumpID_Result Sproc_Get_JDskillDataByJDDumpID(string param)
        {
            return db.Set<Sproc_Get_JDskillDataByJDDumpID_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_JDskillDataByJDDumpID, param)).AsEnumerable().FirstOrDefault();
        }

        public async Task<GenGptJdresponse?> GetGenGptJdResponseByUrl(string strJdURL)
        {
            GenGptJdresponse genGptJdresponse = new GenGptJdresponse();

            genGptJdresponse = await db.GenGptJdresponses.Where(x => Convert.ToString(x.Url).ToLower() == strJdURL.ToLower()).FirstOrDefaultAsync();

            return genGptJdresponse;
        }

        public async Task UpdateGptJdresponse(long id, int count)
        {
            GenGptJdresponse genGptJdresponse = await db.GenGptJdresponses.Where(x => x.Id == id).FirstOrDefaultAsync();
            genGptJdresponse.AchievedDetails = count;
            db.GenGptJdresponses.Update(genGptJdresponse);
            db.SaveChanges();

        }
        public async Task SaveGptJdresponse(GenGptJdresponse jdresponse)
        {
            var jdDumpDetails = db.GenGptJdresponses.Add(jdresponse);
            db.SaveChanges();

        }
        public Sproc_CreateJDFromPrompt_ClientPortal_Result Sproc_CreateJDFromPrompt_ClientPortal_Result(string param)
        {
            return db.Set<Sproc_CreateJDFromPrompt_ClientPortal_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_CreateJDFromPrompt_ClientPortal, param)).AsEnumerable().FirstOrDefault();

        }

        public Sproc_CreateJDFromPrompt_UTSAdmin_Result Sproc_CreateJDFromPrompt_UTSAdmin_Result(string param)
        {
            return db.Set<Sproc_CreateJDFromPrompt_UTSAdmin_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_CreateJDFromPrompt_UTSAdmin, param)).AsEnumerable().FirstOrDefault();

        }
        public List<Sproc_Get_UpdateHR_Details_Result> Sproc_Get_UpdateHR_Details_Result(string param)
        {
            return db.Set<Sproc_Get_UpdateHR_Details_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_UpdateHR_Details, param)).ToList();

        }
        public async Task<GenRoleAndHiringTypeClientPortal?> GetGenRoleAndHiringTypeClientPortalByGptJdId(long gptJdId)
        {
            return await db.GenRoleAndHiringTypeClientPortals.Where(rh => rh.Gptjdid == gptJdId).FirstOrDefaultAsync();
        }

        public async Task<GenSkillAndBudgetClientPortal?> GetGenSkillAndBudgetClientPortalByGptJdId(long gptJdId)
        {
            return await db.GenSkillAndBudgetClientPortals.Where(rh => rh.Gptjdid == gptJdId).FirstOrDefaultAsync();
        }

        public async Task<GenJobPostEmploymentDetailsClientPortal?> FetchEmploymentDetailsByGptJdIdAsync(long gptJdId)
        {
            return await db.GenJobPostEmploymentDetailsClientPortals.Where(x => x.Gptjdid == gptJdId).FirstOrDefaultAsync();
        }

        public async Task<GenJobPostRolesResponsibilitiesClientPortal?> FetchRolesResponsibilitiesByGptJdIdAsync(long gptJdId)
        {
            return await db.GenJobPostRolesResponsibilitiesClientPortals.Where(rh => rh.Gptjdid == gptJdId).FirstOrDefaultAsync();
        }

        public async Task<GenClientJob?> GetGenClientJobInfo(string guiId)
        {
            return await db.GenClientJobs.Where(x => x.Guid == guiId).FirstOrDefaultAsync();
        }

        public async Task<GenContact> GetGenContact(string EmailId)
        {
            var genContactDetails = db.GenContacts.Where(x => x.EmailId.ToLower() == EmailId.ToLower()).FirstOrDefaultAsync();

            return await genContactDetails;

        }

        public void SaveStepInfoWithUnicode(string guid, string jobDescription)
        {
            var Guid = new SqlParameter("@Guid", SqlDbType.NVarChar) { Value = guid };
            var JobDescription = new SqlParameter("@JobDescription", SqlDbType.NVarChar) { Value = jobDescription };

            db.Database.ExecuteSqlRaw("EXEC Sproc_Update_UnicodeValues @GUID, @JobDescription", parameters: new[] { Guid, JobDescription });

        }

        public void SaveperquisitesWithUnicode(string guid, string perquisites)
        {
            var Guid = new SqlParameter("@Guid", SqlDbType.NVarChar) { Value = guid };
            var perquisite = new SqlParameter("@Prerequisites", SqlDbType.NVarChar) { Value = perquisites };

            db.Database.ExecuteSqlRaw("EXEC Sproc_Update_Prerequisites_UnicodeValues @GUID, @Prerequisites", parameters: new[] { Guid, perquisite });

        }

        #endregion
    }
}
