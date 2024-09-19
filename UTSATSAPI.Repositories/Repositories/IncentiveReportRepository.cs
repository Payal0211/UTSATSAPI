using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Infrastructure.Repositories;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class IncentiveReportRepository : IIncentiveReport
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private IUnitOfWork _unitOfWork;
        #endregion

        public IncentiveReportRepository(TalentConnectAdminDBContext _db, IUnitOfWork unitOfWork)
        {
            db = _db;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Sproc_Inc_Incentive_Report_Result>> GetIncentiveReportData(string param)
        {
            return await db.Set<Sproc_Inc_Incentive_Report_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Inc_Incentive_Report, param)).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetUserTypeDDL()
        {
            var data = await db.Set<Sproc_Get_Managers_Result>().FromSqlRaw(Constants.ProcConstant.Sproc_Get_Managers).ToListAsync();

            return data.Select(x => new SelectListItem
            {
                Value = x.Value.ToString(),
                Text = x.Text
            }).OrderBy(x=>x.Text).ToList();
        }

        public async Task<ValidationMessageForIncentiveReport> GetValidationMessageForIncentiveReport(string param)
        {
            return db.Set<ValidationMessageForIncentiveReport>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Validation_Message_For_Incentive_Report, param)).AsEnumerable().FirstOrDefault();
        }

        public async Task<List<Sproc_GetInc_ReportDetails_Result>> GetIncentiveReportDetails(string param)
        {
            return await db.Set<Sproc_GetInc_ReportDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GetInc_ReportDetails, param)).ToListAsync();
        }

        public async Task<List<Sproc_Inc_Incentive_Report_AM_NR_Result>> GetIncentiveReportDataAMNR(string param)
        {
            return await db.Set<Sproc_Inc_Incentive_Report_AM_NR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GetInc_ReportDetails, param)).ToListAsync();
        }

        public async Task<List<Sproc_GetInc_ReportDetails_AM_NR_Result>> GetIncentiveReportDetailsAMOrNR(string param)
        {
            return await db.Set<Sproc_GetInc_ReportDetails_AM_NR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GetInc_ReportDetails_AM_NR, param)).ToListAsync();
        }
        public async Task<List<Sproc_GetInc_ReportDetails_Contract_Booster_Result>> GetIncentiveReportDetailsContractBooster(string param)
        {
            return await db.Set<Sproc_GetInc_ReportDetails_Contract_Booster_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GetInc_ReportDetails_Contract_Booster, param)).ToListAsync();
        }
        public async Task<List<Sproc_Get_Users_BasedOnUserRole_Result>> GetUsersBasedOnUserRole(string param)
        {
            return await db.Set<Sproc_Get_Users_BasedOnUserRole_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Users_BasedOnUserRole, param)).ToListAsync();
        }
        public async Task<List<SROC_GET_HIERARCHY_Result>> GetHierarchy(long parentid, long forSalesorOps)
        {
            object[] param = { parentid, forSalesorOps };
            string paramterstring = CommonLogic.ConvertToParamString(param);
            return await db.Set<SROC_GET_HIERARCHY_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sroc_Get_Hierarchy, paramterstring)).ToListAsync();
        }
        public async Task<UsrUser> GetUsrUserById(long id)
        {
            return await _unitOfWork.usrUsers.GetSingleByCondition(x => x.Id == id);
        }
        public async Task<IEnumerable<UsrUser>> GetAllUsrUserList()
        {
            return await _unitOfWork.usrUsers.GetAll();
        }
        public async Task<bool> InsertTargetAcheivedDetails(string param)
        {
            int result = await db.Database.ExecuteSqlRawAsync(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Insert_Target_Acheived_Details, param));
            return result > 0;
        }
        public async Task<UsrUserRoleDetail> GetUsrUserRoleDetailById(long id)
        {
            return await _unitOfWork.usrUserRoleDetails.GetSingleByCondition(x => x.Id == id);
        }
        public async Task<IEnumerable<UsrUserRole>> GetAllUsrUserRoleList()
        {
            return await _unitOfWork.usrUserRoles.GetAllByCondition(x=> x.IsActive == true);
        }

    }
}

