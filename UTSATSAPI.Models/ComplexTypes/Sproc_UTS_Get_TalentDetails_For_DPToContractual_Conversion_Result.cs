using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion_Result
    {
        public string Talentname { get; set; }
        public long TalentID { get; set; }
        public string ExpectedCTC { get; set; }
        public decimal NRPercentage { get; set; }
        public decimal BRAmount { get; set; }
        public long ContactTalentPriorityID { get; set; }
        public long OnBoardID { get; set; }
        public long HRID { get; set; }
    }
}
