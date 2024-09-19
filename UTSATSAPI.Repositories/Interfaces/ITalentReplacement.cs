using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface ITalentReplacement
    {
        Task<List<Sproc_Get_AMNBD_For_Replacement_Result>> GetAMNBDForReplacement(long ID);
        Task<TalentReplacement> SaveTalentReplacementData(TalentReplacement talentReplacement,IUniversalProcRunner universalProcRunner);
        Task<List<Sproc_Get_Engagemetns_For_Replacement_BasedOn_LWD_Option_Result>> GetEngagemetnsForReplacementBasedOnLWDOption(string param);

        Task<GenTalent> GteGenTalentById(long? Id);
        Task<GenContact> GteGenContactById(long? Id);
        Task<GenCompany> GteGenCompanyById(long? Id);
        Task<UsrUser> GetUsrUserById(long Id);
        Task<GenTalent> GetGenTalentsById(long Id); 
        Task<GenSalesHiringRequest> GetGenSalesHiringRequestById(long Id); 
        Task<PrgHiringRequestStatus> GetPrgHiringRequestStatusById(long Id);
        Task<GenOnBoardTalent> GetGenOnBoardTalentById(long Id); 
        Task<GenContactTalentPriority> GenContactTalentPriorityByTalentIDorHiringRequestID(long TalentID, long HiringRequestID);
        Task<List<Sproc_Get_HRIDEngagementID_ForReplacement_Result>> Sproc_Get_HRIDEngagementID_ForReplacement(string param);
        Task<Sproc_Get_OnBoardDetailFor_C2H_Result> Sproc_Get_OnBoardDetailFor_C2H(string param);
    }
}
