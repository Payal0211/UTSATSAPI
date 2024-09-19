namespace UTSATSAPI.Repositories.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Repositories.Interfaces;
    using static UTSATSAPI.Helpers.Enum;

    public class MastersRepositories : IMasters
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion


        #region Constructor
        public MastersRepositories(TalentConnectAdminDBContext _db)
        {
            this.db = _db;
        }        
        #endregion

        public List<Sproc_Get_PrgSkill_NotusedinMappingtables_Result> GetSkills(string param)
        {
            return db.Set<Sproc_Get_PrgSkill_NotusedinMappingtables_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Get_PrgSkill_NotusedinMappingtables, param)).ToList();
        }
        public Sproc_prg_TempSkills_Insert AddSkills(string param)
        {
            return db.Set<Sproc_prg_TempSkills_Insert>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_prg_TempSkills_Insert, param)).AsEnumerable().FirstOrDefault();

        }
        public async Task<List<SROC_GET_HIERARCHY_Result>> GetHierarchy(long parentid, bool IsOpsUser)
        {
            int value = 0;
            if (IsOpsUser)
                value = 1;

            object[] param = { parentid, value };
            string paramterstring = CommonLogic.ConvertToParamString(param);
            return await db.Set<SROC_GET_HIERARCHY_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sroc_Get_Hierarchy, paramterstring)).ToListAsync();
        }

        public async Task<List<Sproc_UTS_Update_Role_Result>> UpdateRole(int ID, int UpdatedRoleID, long LoggedInUserId)
        {
            object[] param = { ID, UpdatedRoleID, (int)LoggedInUserId , (int)AppActionDoneBy.UTS };
            string paramterstring = CommonLogic.ConvertToParamString(param);
            return await db.Set<Sproc_UTS_Update_Role_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Update_Role, paramterstring)).ToListAsync();

        }
        public async Task<List<Sproc_GetContactTimeZone_Result>> GetTimeZone(string param)
        {
            return await db.Set<Sproc_GetContactTimeZone_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_GetContactTimeZone, param)).ToListAsync();
        }

        public List<PrgCountryRegion> GetCountryRegions()
        {
            return db.PrgCountryRegions.ToList();
        }

        public async Task<List<Sproc_Get_AM_User_Result>> GetAMUser()
        {
            return await db.Set<Sproc_Get_AM_User_Result>().FromSqlRaw(String.Format("{0}", Constants.ProcConstant.Sproc_Get_AM_User)).ToListAsync();
        }
        public async Task<List<Sproc_UTS_GetUserByType_Result>> GetUserByType(int userType)
        {
            return await db.Set<Sproc_UTS_GetUserByType_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_GetUserByType, userType)).ToListAsync();
        }
        public async Task<List<Sproc_UTS_GetSalesPerson_Result>> GetSalesPerson()
        {
            return await db.Set<Sproc_UTS_GetSalesPerson_Result>().FromSqlRaw(String.Format("{0}", Constants.ProcConstant.Sproc_UTS_GetSalesPerson)).ToListAsync();
        }
    }
}
