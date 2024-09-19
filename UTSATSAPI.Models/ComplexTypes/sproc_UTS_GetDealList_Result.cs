
namespace UTSATSAPI.Models.ComplexTypes
{
    using Microsoft.EntityFrameworkCore;

    [Keyless]
    public class sproc_UTS_GetDealList_Result
    {
        public string? DealDate { get; set; }
        public string? Deal_Id { get; set; }
        public string? Lead_Type { get;set;}
        public string? Pipeline { get; set; }
        public string? DealStage { get; set; }
        public string? DealStageColorCode { get; set; }
        public string? Company { get; set; }
        public string? GEO { get; set; }
        public string? BDR { get; set; }
        public string? Sales_Consultant { get; set; }
        public int? TotalRecords { get; set; }
        public long? HR_ID { get; set; }
        public Int64? DealID { get; set; }
        public string? DealName { get; set; }
    }
}
