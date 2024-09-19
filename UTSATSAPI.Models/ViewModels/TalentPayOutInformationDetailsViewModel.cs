namespace UTSATSAPI.Models.ViewModels
{
    public class TalentPayOutInformationDetailsViewModel
    {
        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
        public int ContractDetailID { get; set; }
        public List<MastersResponseModel> ReplacementEngAndHR { get; set; } = new List<MastersResponseModel>();
        public bool IsConvertToHireApplicable { get; set; }
        public decimal DPNRPercentage { get; set; }
        public string Currency { get; set; }
    }
}
