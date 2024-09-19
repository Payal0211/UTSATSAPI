using UTSATSAPI.Models.ViewModel;

namespace UTSATSAPI.Models.ViewModels
{
    public class UpdateContractEndDateViewModel
    {
        public int ContractDetailID { get; set; }
        public DateTime ContractEndDate { get; set; }
        public string Reason { get; set; }
        public string FileName { get; set; }
        public FileUploadModelBase64 fileUpload { get; set; }
        public int? LostReasonID { get; set; }

        // UTS-7389: Get the replacement details from UI.

        public bool? IsReplacement { get; set; }
        public TalentReplacement talentReplacement { get; set; } = new TalentReplacement();
        public decimal? DPPercentage { get; set; }        
        public decimal? ExpectedCTC { get; set; }
        public DateTime? NewContractStartDate { get; set; }
        public decimal? DPAmount { get; set; }
    }
}
