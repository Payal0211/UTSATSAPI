namespace UTSATSAPI.Models.Models
{
    public class GetHRCost
    {
        public long HR_ID { get; set; }
        public long ContactPriorityID { get; set; }
        public string? HrCost { get; set; }
        public string Talent_Fees { get; set; }
        public string Currency_Sign { get; set; }
        public decimal HR_Percentage { get; set; }
        public decimal FinalHRCost { get; set; }
    }
}
