namespace UTSATSAPI.Models.ViewModels.Request_ResponseModels
{
    public class RescheduleInterviewSlotModel
    {
        public string RescheduleRequestBy { get; set; }
        public string ReasonforReschedule { get; set; }
        public int SlotType { get; set; }
        public List<RescheduleSlot> RescheduleSlot { get; set; }
        public int HiringRequest_ID { get; set; }
        public int HiringRequest_Detail_ID { get; set; }
        public int ContactID { get; set; }
        public int Talent_ID { get; set; }
        public int InterviewStatus { get; set; }
        public int InterviewMasterID { get; set; }
        public string HiringRequestNumber { get; set; }
        public int WorkingTimeZoneID { get; set; }
        public Int64 NextRound_InterviewDetailsID { get; set; }
        public string Additional_Notes { get; set; }
        public string InterviewCallLink { get; set; }
        public bool? IsAnotherRoundInterview { get; set; }
        //public string BearerToken { get; set; }
    }
}
