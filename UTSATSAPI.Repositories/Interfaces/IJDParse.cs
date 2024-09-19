using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModel;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Repositories.Interfaces
{
    /// <summary>
    /// IJDParse
    /// </summary>
    public interface IJDParse
    {
        List<Sproc_GetExtractedSkills_FromJD_Result> Sproc_GetExtractedSkills_FromJD(string param);

        Sproc_DumpJDDetailsintoTempTable_Result Sproc_DumpJDDetailsintoTempTable(string param);

        Sproc_Get_JDskillDataByJDDumpID_Result Sproc_Get_JDskillDataByJDDumpID(string param);
        Task<GenGptJdresponse?> GetGenGptJdResponseByUrl(string strJdURL);
        Task UpdateGptJdresponse(long id, int count);
        Task SaveGptJdresponse(GenGptJdresponse jdresponse);
        Sproc_CreateJDFromPrompt_ClientPortal_Result Sproc_CreateJDFromPrompt_ClientPortal_Result(string param);
        List<Sproc_Get_UpdateHR_Details_Result> Sproc_Get_UpdateHR_Details_Result(string param);
        Task<GenRoleAndHiringTypeClientPortal?> GetGenRoleAndHiringTypeClientPortalByGptJdId(long gptJdId);

        Task<GenSkillAndBudgetClientPortal?> GetGenSkillAndBudgetClientPortalByGptJdId(long gptJdId);
        Task<GenJobPostEmploymentDetailsClientPortal?> FetchEmploymentDetailsByGptJdIdAsync(long gptJdId);
        Task<GenJobPostRolesResponsibilitiesClientPortal?> FetchRolesResponsibilitiesByGptJdIdAsync(long gptJdId);
        Task<GenClientJob?> GetGenClientJobInfo(string guiId);

        Task<GenContact?> GetGenContact(string EmailId);
        Sproc_CreateJDFromPrompt_UTSAdmin_Result Sproc_CreateJDFromPrompt_UTSAdmin_Result(string param);
        void SaveStepInfoWithUnicode(string guid, string jobDescription);
        void SaveperquisitesWithUnicode(string guid, string perquisites);
    }
}
