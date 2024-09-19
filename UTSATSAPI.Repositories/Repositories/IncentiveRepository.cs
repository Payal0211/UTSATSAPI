using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class IncentiveRepository : IIncentive
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructor
        public IncentiveRepository(TalentConnectAdminDBContext _db)
        {
            this.db = _db;
        }

       
        #endregion

        #region Public Methods
        public List<Sproc_UTS_Get_Inc_PlacementFees_Slab_Result> GetPlacementFees(string param)
        {
            return db.Set<Sproc_UTS_Get_Inc_PlacementFees_Slab_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Get_Inc_PlacementFees_Slab, param)).ToList();
        }
        public List<Sproc_UTS_Get_Acheived_Target_Details_Result> Get_Acheived_Target_Details(string param)
        {
            return db.Set<Sproc_UTS_Get_Acheived_Target_Details_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Get_Acheived_Target_Details, param)).ToList();
        }
        public List<Sproc_UTS_Get_Inc_AM_NRSlab_Result> Get_AM_NRSlab(string param)
        {
            return db.Set<Sproc_UTS_Get_Inc_AM_NRSlab_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Get_Inc_AM_NRSlab, param)).ToList();
        }
        public List<Sproc_Get_Inc_Contracts_Result> GetContracts(string param)
        {
            return db.Set<Sproc_Get_Inc_Contracts_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Inc_Contracts, param)).ToList();
        }
        public List<Sproc_Get_Inc_TalentDeploySlab_Result> GetAMTalentDeployment(string param)
        {
            return db.Set<Sproc_Get_Inc_TalentDeploySlab_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Inc_TalentDeploySlab, param)).ToList();
        }
        public List<Sproc_Get_Inc_SALGoal_Result> GetSALGoal(string param)
        {
            return db.Set<Sproc_Get_Inc_SALGoal_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Inc_SALGoal, param)).ToList();
        }
        #endregion
    }
}   
