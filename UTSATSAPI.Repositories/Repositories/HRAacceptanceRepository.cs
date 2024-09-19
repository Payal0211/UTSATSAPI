using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class HRAacceptanceRepository : IHRAcceptance
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public HRAacceptanceRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }
        #endregion

        #region public Methods
        public List<Sproc_GET_PostAcceptance_detail_Result> GetPostAcceptanceDetail(long? PostId)
        {
            return db.Set<Sproc_GET_PostAcceptance_detail_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GET_PostAcceptance_detail, PostId)).ToList();
        }

        public List<Sproc_GET_PostAcceptance_detail_Availability_Result> GetPostAcceptanceDetailAvailability(long? PostId)
        {
            return db.Set<Sproc_GET_PostAcceptance_detail_Availability_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GET_PostAcceptance_detail_Availability, PostId)).ToList();
        }

        public List<Sproc_GET_PostAcceptance_detail_HowSoon_Result> GetPostAcceptanceDetailHowSoon(long? PostId)
        {
            return db.Set<Sproc_GET_PostAcceptance_detail_HowSoon_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GET_PostAcceptance_detail_HowSoon, PostId)).ToList();
        }

        public void UpdateTalentConfirmHRAcceptance(long? HiringRequestDetailId, int TalentID, long? ContactId, bool Radio_OptionMatch, long? PrimaryKey, string TableName)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}, {2}, {3}, {4}, {5}, {6}", Constants.ProcConstant.sproc_update_Talent_ConfirmPostAcceptance, HiringRequestDetailId, TalentID, ContactId, Radio_OptionMatch, PrimaryKey, TableName));
        }

        public void ShortlistedTalentsUpdate(long HRDetailId)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_ShortlistedTalents_Update, HRDetailId));
        }

        public List<Sproc_Add_Confirm_Interview_Slot_Result> AddConfirmInterviewSlotResult(long ShortListedId, long? HiringRequestId, long? HiringRequestDetailId, long TalentId, long? ContactId, int CreatedById)
        {
            return db.Set<Sproc_Add_Confirm_Interview_Slot_Result>().FromSqlRaw(string.Format("{0} {1}, {2}, {3}, {4}, {5}, {6}", Constants.ProcConstant.Sproc_Add_Confirm_Interview_Slot, ShortListedId, HiringRequestId, HiringRequestDetailId, TalentId, ContactId, CreatedById)).ToList();
        }
        #endregion
    }
}
