using Microsoft.AspNetCore.Mvc.Rendering;
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
    public interface IIncentiveReport
    {
        Task<List<SelectListItem>> GetUserTypeDDL();

        Task<List<Sproc_Inc_Incentive_Report_Result>> GetIncentiveReportData(string param);
        Task<ValidationMessageForIncentiveReport> GetValidationMessageForIncentiveReport(string param);
        Task<List<Sproc_GetInc_ReportDetails_Result>> GetIncentiveReportDetails(string param);
        Task<List<Sproc_Inc_Incentive_Report_AM_NR_Result>> GetIncentiveReportDataAMNR(string param);
        Task<List<Sproc_GetInc_ReportDetails_AM_NR_Result>> GetIncentiveReportDetailsAMOrNR(string param);
        Task<List<Sproc_GetInc_ReportDetails_Contract_Booster_Result>> GetIncentiveReportDetailsContractBooster(string param);
        Task<List<Sproc_Get_Users_BasedOnUserRole_Result>> GetUsersBasedOnUserRole(string param);

        Task<List<SROC_GET_HIERARCHY_Result>> GetHierarchy(long parentid, long forSalesorOps);

        Task<UsrUser> GetUsrUserById(long id);
        Task<IEnumerable< UsrUser>> GetAllUsrUserList();

        Task<bool> InsertTargetAcheivedDetails(string param);
        Task<UsrUserRoleDetail> GetUsrUserRoleDetailById(long id);
        Task<IEnumerable<UsrUserRole>> GetAllUsrUserRoleList();

    }
}
