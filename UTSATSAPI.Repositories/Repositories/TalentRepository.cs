using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class TalentRepository : ITalent
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public TalentRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }

        #endregion

        #region public Methods
        public List<sproc_UTS_GetTalentList_Result> GetTalentList(string paramasString)
        {
            return db.Set<sproc_UTS_GetTalentList_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetTalent, paramasString)).ToList();
        }
        public List<Sproc_UTS_ManagedTalent_Result> GetManagedTalentList(string paramasString)
        {
            return db.Set<Sproc_UTS_ManagedTalent_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_ManagedTalent, paramasString)).ToList();
        }
        public List<Sproc_UTS_GetTalentLegalInfo_Result> GetTalentLegalInfo(string paramasString)
        {
            return db.Set<Sproc_UTS_GetTalentLegalInfo_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetTalentLegalInfo, paramasString)).ToList();
        }
        #endregion
    }
}
