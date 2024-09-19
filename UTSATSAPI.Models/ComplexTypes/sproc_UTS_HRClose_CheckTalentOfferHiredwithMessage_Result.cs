using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_HRClose_CheckTalentOfferHiredwithMessage_Result
    {
        public bool? IsWarningMsgNeedToShow {  get; set; }
        public string? WarningMsg {  get; set; }
    }
}
