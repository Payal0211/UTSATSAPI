namespace UTSATSAPI.Models.ViewModels
{
    public class SaveBillPayRateViewModel
    {
        public long OnboardId { get; set; }
        public decimal BillRate { get; set; }
        public decimal PayRate { get; set; }
        public decimal NR { get; set; }
        public string BillRateComment { get; set; }
        public string payRateComment { get; set; }
        public string BillrateCurrency { get; set; }
        public int month { get; set; }
        public int Year { get; set; }
        public string BillRateReason { get; set; }
        public string payrateReason { get; set; }
        public bool isEditBillRate { get; set; }
        public string? ContractType { get; set; }
        public bool? isFromMultiPopup { get; set; }
    }
}
