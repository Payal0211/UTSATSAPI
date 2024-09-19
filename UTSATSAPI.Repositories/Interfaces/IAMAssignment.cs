using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IAMAssignment
    {
        List<sproc_UTS_GetAMAssignments_Result> GetAMAssignmentList(string paramasString);
        SP_UTS_ESales_Get_Client_AM_Details_Result SP_UTS_ESales_Get_Client_AM_Details(string paramasString);
        void sproc_UTS_Update_EmployeeID_FromInvoiceAPIResponse(string paramasString);
        Task<List<SelectListItem>> GetAMUser();
        Task<bool> ChangeAssignmentTeamDistributionPriority(int id, int priority, int UserId);
    }
}