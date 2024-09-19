using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IIncentiveTarget
    {
        Task<List<sproc_Get_IncentiveTarget_List_New_flow>> GetIncentiveTargetList(string param);
        Task<List<Sproc_Get_ALL_User_HIERARCHY_SaleTarget_For_Parent_Result>> GetSalesUserHierarchy(string param);
        void InsertIncentiveUserTarget(string param);
    }
}
