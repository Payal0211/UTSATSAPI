using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Company_Funding_Details_Result
    {
        public long? FundingId { get; set; }
        public string? FundingAmount { get; set; }
        public string? FundingRound {  get; set; }
        public string? Series {  get; set; }
        public string? FundingMonth { get; set; }
        public string? FundingYear { get; set; }
        public string? Investors { get; set; }
        public int? TotalRounds { get; set; }
        public string? LastFundingRound { get; set; }
        public string? AllInvestors { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
