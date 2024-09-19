namespace UTSATSAPI.Repositories.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using UTSATSAPI.ComplexTypes;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Repositories.Interfaces;
    using UTSATSAPI.Models.ViewModels;

    public class TalentStatusRepository : ITalentStatus
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructor
        public TalentStatusRepository(TalentConnectAdminDBContext _db)
        {
            this.db = _db;
        }
        #endregion

        #region Public Methods
        public void UpdateTalentStatus(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_UpdateHrDetailRoleStatusWithTalentStatus, param));
        }

        public void sproc_UTS_UpdateTalentRejectReason(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_UpdateTalentRejectReason, param));
        }

        public sp_UTS_get_HRTalentProfileReason_Result sproc_UTS_get_HRTalentProfileReason(string param)
        {
            return db.Set<sp_UTS_get_HRTalentProfileReason_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sp_UTS_get_HRTalentProfileReason, param)).AsEnumerable().FirstOrDefault();
        }
        public UsrUser GetUsrUserById(long ID)
        {
            return db.UsrUsers.Where(x=> x.Id==ID).FirstOrDefault() ;
        }

        public void RemoveOnHoldStatus(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Update_OnHold_Status_To_PreviousStatus, param));

        }
        public void CreditBased_UpdateTalentStatus(string paramString)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Talent_Status_Change_ClientPortal, paramString));
        }

        public List<PrgTalentStatusClientPortal> CreditBased_GetPrgTalentStatus()
        {
            return db.PrgTalentStatusClientPortals.ToList();
        }
        public List<PrgTalentRejectReason> CreditBased_PrgTalentRejectReason()
        {
            return db.PrgTalentRejectReasons.Where(x=> x.IsActive == true && x.IsDisplay == true).ToList();
        }

        public Sproc_Get_RejectionReasonForTalent_Result Sproc_Get_RejectionReasonForTalent(string param)
        {
            return db.Set<Sproc_Get_RejectionReasonForTalent_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_RejectionReasonForTalent, param)).AsEnumerable().FirstOrDefault();

        }

        #endregion
    }
}
