using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class CompanyLegalInfoRepository : ICompanyLegalInfo
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public CompanyLegalInfoRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }
        #endregion

        #region Public Methods

        public List<sproc_GetCompanyLegalInfo_Result> GetLegalInfoList(string strparams)
        {
            return db.Set<sproc_GetCompanyLegalInfo_Result>().FromSqlRaw(string.Format("{0} {1}",
                Constants.ProcConstant.sproc_UTS_GetCompanyLegalInfo, strparams)).ToList();
        }
        #endregion
    }
}
