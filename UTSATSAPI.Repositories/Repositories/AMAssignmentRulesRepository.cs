using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class AMAssigmentRulesRepository : IAMAssignmentRules
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public AMAssigmentRulesRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }

        #endregion

        #region public Methods

        public async Task<List<sproc_UTS_GetAMAssignmentRules_Result>> GetAMAssignmentRules(string paramasString)
        {
            return await db.Set<sproc_UTS_GetAMAssignmentRules_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetAMAssignmentRules, paramasString)).ToListAsync();
        }

        #endregion
    }
}
