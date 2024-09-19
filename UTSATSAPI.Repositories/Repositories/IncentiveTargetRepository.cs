using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class IncentiveTargetRepository : IIncentiveTarget
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public IncentiveTargetRepository(TalentConnectAdminDBContext _db, IUnitOfWork unitOfWork)
        {
            db = _db;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Public
        public async Task<List<sproc_Get_IncentiveTarget_List_New_flow>> GetIncentiveTargetList(string param)
        {
            return await db.Set<sproc_Get_IncentiveTarget_List_New_flow>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Get_IncentiveTarget_List_New_flow, param)).ToListAsync();
        }
        public async Task<List<Sproc_Get_ALL_User_HIERARCHY_SaleTarget_For_Parent_Result>> GetSalesUserHierarchy(string param)
        {
            return await db.Set<Sproc_Get_ALL_User_HIERARCHY_SaleTarget_For_Parent_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_ALL_User_HIERARCHY_SaleTarget_For_Parent, param)).ToListAsync();
        }
        public void InsertIncentiveUserTarget(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_insert_Incentive_UserTarget, param));
        }
        #endregion
    }
}
