namespace UTSATSAPI.Models.ViewModels.CompanyProfile
{
    public class CompanyFundingDetails
    {
        public long? FundingID { get; set; }
        public string? FundingAmount { get; set; }
        public string? FundingRound { get; set; }
        public string? Series { get; set; }
        public string? MONTH { get; set; }
        public string? YEAR { get; set; }
        public string? Investors { get; set; }
        public string? AdditionalInformation { get; set; }
    }

    public class DeleteFundingDetails
    {
        public long? FundingID { get; set; }
        public long? CompanyID { get; set; }
    }
}
