using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_GetOnBoardData_Result
    {
        public string Engagement_Number { get; set; }
        public string HR_ID { get; set; }
        public string Client_Name { get; set; }
        public string Talent_Name { get; set; }
        public decimal Total_Invoice_Amount { get; set; }
        public decimal Total_Paid_Amount { get; set; }
        public int Estimated_Remaining_payment_Amount { get; set; }
        public string Currency { get; set; }
        public System.DateTime CancelledDate { get; set; }
        public string ReasonForLoss { get; set; }
    }
}
