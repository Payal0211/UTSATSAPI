using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IDeals
    {
        Task<List<sproc_UTS_GetDealList_Result>> GetDealList(string paramasString);
        Task<List<sproc_UTS_GetDealCompanyDetails_Result>> GetDealCompanydetails(Nullable<long> DealID);
        Task<List<sproc_UTS_GetDealLeadDetails_Result>> GetDealLeadDetails(Nullable<long> DealID);
        Task<List<sproc_UTS_GetDealPrimary_SecondaryClient_Result>> GetDealPrimaryClient(Nullable<long> DealID);
        Task<List<sproc_UTS_GetDealPrimary_SecondaryClient_Result>> GetDealSecondaryClient(Nullable<long> DealID);
        Task<List<sproc_UTS_GetDealActivity_Result>> GetDealActivity(Nullable<long> DealID);
        Task<List<sp_get_UTS_GetFilterTypeForDeals_Result>> sp_get_UTS_GetFilterTypeForDeals_Result(string param);

        Task<GenDeal> GetGenDealByDealNumber(long dealNumber);
        Task<PrgDealStage> GetPrgDealStageById(int? id);
        Task<PrgDealType> GetPrgDealTypeById(int? id);

        Task<IEnumerable<PrgDealStage>> GetPrgDealStageList();
        Task<IEnumerable<GenDeal>> GetGenDealsList();
        Task<IEnumerable<PrgPipeline>> GetPrgPipelineList();
        Task<IEnumerable<PrgGeo>> GetPrgGeosList();
        Task<IEnumerable<GenCompany>> GetGenCompanyList();
    }
}
