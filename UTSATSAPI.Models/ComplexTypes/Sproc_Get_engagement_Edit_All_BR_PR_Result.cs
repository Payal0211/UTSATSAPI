using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_engagement_Edit_All_BR_PR_Result
    {
        public long? ID { get; set; }
        public long? OnBoardID { get; set; }
        public int? Months { get; set; }
        public int? Years { get; set; }
        public string? EngagemenID { get; set; }
        public string? Currency { get; set; }
        public string? MonthNames { get; set; }
        public decimal? BR { get; set; }
        public decimal? PR { get; set; }
        public decimal? NR_DP_Value { get; set; }
        public decimal? Actual_NR_Percentage { get; set; }
        public string? ContractType { get; set; }
        public DateTime? ClientInvoiceDate { get; set; }
        public string? EngagementId_HRID { get; set; }
        public int? AM_SalesPersonID { get; set; }
        public string? AM_UserName { get; set; }
        public int? TSC_PersonID { get; set; }
        public string? TSC_UserName { get; set; }
    }
}
