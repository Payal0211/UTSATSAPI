using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Infrastructure.Repositories;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class DealListRepository : IDeals
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public DealListRepository(TalentConnectAdminDBContext _db, IUnitOfWork unitOfWork)
        {
            db = _db;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region public Methods

        public async Task<List<sproc_UTS_GetDealList_Result>> GetDealList(string paramasString)
        {
            return await db.Set<sproc_UTS_GetDealList_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetDealList, paramasString)).ToListAsync();
        }

        public async Task<List<sproc_UTS_GetDealCompanyDetails_Result>> GetDealCompanydetails(Nullable<long> DealID)
        {
            return await db.Set<sproc_UTS_GetDealCompanyDetails_Result>().FromSqlRaw($"{Constants.ProcConstant.sproc_UTS_GetDealCompanydetails} " + $"{DealID}").ToListAsync();
        }

        public async Task<List<sproc_UTS_GetDealLeadDetails_Result>> GetDealLeadDetails(Nullable<long> DealID)
        {
            return await db.Set<sproc_UTS_GetDealLeadDetails_Result>().FromSqlRaw($"{Constants.ProcConstant.sproc_GetDealLeadDetais} " + $"{DealID}").ToListAsync();
        }

        public async Task<List<sproc_UTS_GetDealPrimary_SecondaryClient_Result>> GetDealPrimaryClient(Nullable<long> DealID)
        {
            return await db.Set<sproc_UTS_GetDealPrimary_SecondaryClient_Result>().FromSqlRaw($"{Constants.ProcConstant.sproc_GetDealprimaryClient} " + $"{DealID}").ToListAsync();
        }

        public async Task<List<sproc_UTS_GetDealPrimary_SecondaryClient_Result>> GetDealSecondaryClient(Nullable<long> DealID)
        {
            return await db.Set<sproc_UTS_GetDealPrimary_SecondaryClient_Result>().FromSqlRaw($"{Constants.ProcConstant.sproc_GetDealSecondaryClient} " + $"{DealID}").ToListAsync();
        }

        public async Task<List<sproc_UTS_GetDealActivity_Result>> GetDealActivity(Nullable<long> DealID)
        {
            return await db.Set<sproc_UTS_GetDealActivity_Result>().FromSqlRaw($"{Constants.ProcConstant.sproc_GetDealDEALActivity} " + $"{DealID}").ToListAsync();
        }

        public async Task<List<sp_get_UTS_GetFilterTypeForDeals_Result>> sp_get_UTS_GetFilterTypeForDeals_Result(string param)
        {
            return await db.Set<sp_get_UTS_GetFilterTypeForDeals_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetFilterTypeForDeals, param)).ToListAsync();
        }


        public async Task<GenDeal> GetGenDealByDealNumber(long dealId) 
        {
            return await _unitOfWork.genDeals.GetSingleByCondition(x => x.DealId == dealId);
        }
        public async Task<PrgDealStage> GetPrgDealStageById(int? id)
        {
            return await _unitOfWork.prgDealStages.GetSingleByCondition(x => x.Id == id);
        }
        public async Task<PrgDealType> GetPrgDealTypeById(int? id)
        {
            return await _unitOfWork.prgDealTypes.GetSingleByCondition(x => x.Id == id);
        }
        public async Task<IEnumerable<PrgDealStage>> GetPrgDealStageList()
        {
            return await _unitOfWork.prgDealStages.GetAllByCondition(x => x.IsDisplay == true);
        }
        public async Task<IEnumerable<GenDeal>> GetGenDealsList()
        {
            return await _unitOfWork.genDeals.GetAll();
        }
        public async Task<IEnumerable<PrgPipeline>> GetPrgPipelineList()
        {
            return await _unitOfWork.prgPipelines.GetAll();
        }
        public async Task<IEnumerable<PrgGeo>> GetPrgGeosList()
        {
            return await _unitOfWork.prgGeos.GetAll();
        }
        public async Task<IEnumerable<GenCompany>>  GetGenCompanyList()
        {
            return await _unitOfWork.genCompanys.GetAllByCondition(x => x.IsActive == true);
        }
        #endregion
    }
}
