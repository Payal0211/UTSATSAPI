namespace UTSATSAPI.Models.ViewModels
{
    public class TRAcceptanceViewModel
    {
        public Nullable<long> HiringRequest_ID { get; set; }
        public string HR_Number { get; set; }
        public Nullable<int> NoOfTalent { get; set; }
        public Nullable<int> TR_Accepted { get; set; }
        public Nullable<int> TR_Parked { get; set; }
    }
}
