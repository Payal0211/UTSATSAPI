using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IHRAcceptance
    {
        List<Sproc_GET_PostAcceptance_detail_Result> GetPostAcceptanceDetail(long? PostId);
        List<Sproc_GET_PostAcceptance_detail_Availability_Result> GetPostAcceptanceDetailAvailability(long? PostId);
        List<Sproc_GET_PostAcceptance_detail_HowSoon_Result> GetPostAcceptanceDetailHowSoon(long? PostId);
        void UpdateTalentConfirmHRAcceptance(long? HiringRequestDetailId, int TalentID, long? ContactId, bool radio_OptionMatch, long? primaryKey, string tableName);
        List<Sproc_Add_Confirm_Interview_Slot_Result> AddConfirmInterviewSlotResult(long ShortListedId, long? HiringRequestId, long? HiringRequestDetailId, long TalentId, long? ContactId, int CreatedById);
        void ShortlistedTalentsUpdate(long HRDetailId);
    }
}