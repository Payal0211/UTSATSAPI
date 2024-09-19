using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface ITalentStatus
    {
        void UpdateTalentStatus(string param);
        void sproc_UTS_UpdateTalentRejectReason(string param);
        void RemoveOnHoldStatus(string param);
        sp_UTS_get_HRTalentProfileReason_Result sproc_UTS_get_HRTalentProfileReason(string param);
        UsrUser GetUsrUserById(long ID);
        void CreditBased_UpdateTalentStatus(string paramString);
        List<PrgTalentStatusClientPortal> CreditBased_GetPrgTalentStatus();
        List<PrgTalentRejectReason> CreditBased_PrgTalentRejectReason();
        Sproc_Get_RejectionReasonForTalent_Result Sproc_Get_RejectionReasonForTalent(string param);
    }
}
