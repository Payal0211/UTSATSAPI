using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class HubSpotCompanyRepository : IHubSpotCompany
    {

        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public HubSpotCompanyRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }
        #endregion

        #region Public Methods
        public List<Sproc_UTS_GetHubSpotCompanyList_Result> GetHubSpotCompanyList(string strparams)
        {
            return db.Set<Sproc_UTS_GetHubSpotCompanyList_Result>().FromSqlRaw(string.Format("{0} {1}",
                Constants.ProcConstant.Sproc_UTS_GetHubSpotCompanyList, strparams)).ToList();
        }

        #endregion
    }
}
