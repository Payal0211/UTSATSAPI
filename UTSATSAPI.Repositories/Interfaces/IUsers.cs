using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ViewModel;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IUsers
    {
        List<sproc_UTS_GetUsers_Result> GetUsers(string strParams);
        IEnumerable<SelectListItem> GetListOfRequiredNewUser(int type);
        IEnumerable<SelectListItem> GetUserTypes();
        List<sproc_Inc_GetUsers_RoleDetails_Result> GetUserRoleList(string paramasString);
        List<sproc_UTS_GetBDR_Marketingusers> sproc_UTS_GetBDR_Marketingusers(string paramasString);
        SPROC_UTS_UserGeoValues_Result SPROC_UTS_UserGeoValues(long userId);
        List<Sproc_Get_HRDetails_For_UserPriority_Email_Result> GetHRDetailsForUserPriorityEmail(long UserId);
        List<sproc_GetUserHierarchy_Result> sproc_GetUserHierarchy_Results(long userID, string employeeID);
        CheckValidationMessage CheckvalidationMessageForHeadPriorityCount(string param);
        List<Sproc_GetUserBy_LeadType_Result> Sproc_GetUserBy_LeadType(string param);
        Task<List<MastersResponseModel>> Get_UserRoleList(int userType);
        Task<GiveUserDetailsToATS> GiveUserDetailsToATS(string EmployeeID);
        Sproc_InsertTicket_Result SaveUTSFeedBack(string paramString);
    }
}