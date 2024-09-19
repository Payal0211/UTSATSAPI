using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IMasters
    {
        List<Sproc_Get_PrgSkill_NotusedinMappingtables_Result> GetSkills(string param);
        Sproc_prg_TempSkills_Insert AddSkills(string param);
        Task<List<SROC_GET_HIERARCHY_Result>> GetHierarchy(long parentid, bool IsOpsUser);
        Task<List<Sproc_UTS_Update_Role_Result>> UpdateRole(int ID, int UpdatedRoleID, long LoggedInUserId);
        Task<List<Sproc_GetContactTimeZone_Result>> GetTimeZone(string param);
        List<PrgCountryRegion> GetCountryRegions();
        Task<List<Sproc_Get_AM_User_Result>> GetAMUser();
        Task<List<Sproc_UTS_GetSalesPerson_Result>> GetSalesPerson();
        Task<List<Sproc_UTS_GetUserByType_Result>> GetUserByType(int userType);
    }
}
