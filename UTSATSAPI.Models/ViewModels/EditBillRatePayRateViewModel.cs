using Microsoft.AspNetCore.Mvc.Rendering;

namespace UTSATSAPI.Models.ViewModels
{
    public class EditBillRatePayRateViewModel
    {
        public string HrNumber { get; set; }
        public string TalentName { get; set; }
        public DateTime InvoiceSentdate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string InvoiceNumber { get; set; }
        public List<InvoiceStatus> _InvoiceStatus { get; set; }
        public int InvoiceStatusId { get; set; }
        public long OnBoardID { get; set; }
        public IEnumerable<SelectListItem> CurrencyDrp { get; set; }
        public Dictionary<string, string> LeaveType { get; set; }
        public IEnumerable<SelectListItem> ReasonDrp { get; set; }
        public decimal BillRate { get; set; }
        public decimal PayRate { get; set; }
        public decimal PayRate_NR { get; set; }
        public decimal BillRate_NR { get; set; }
        public string BillRate_Comment { get; set; }
        public string PayRate_Comment { get; set; }
        public string Currency { get; set; }
        public int CurrencyId { get; set; }
        public int PayRateCurrencyId { get; set; }
        public decimal FinalPayRate { get; set; }
        public decimal FinalBillRate { get; set; }

        public string BillRateReason { get; set; }
        public string BillRateOtherReason { get; set; }

        public string PayRateReason { get; set; }
        public string PayRateOtherReason { get; set; }

    }
    public class InvoiceStatus
    {
        public int ID { get; set; }
        public string Value { get; set; }
    }

}
