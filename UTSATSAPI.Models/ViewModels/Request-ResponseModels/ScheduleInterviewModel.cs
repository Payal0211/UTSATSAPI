namespace UTSATSAPI.Models.ViewModels.Request_ResponseModels
{
    public class ScheduleInterviewModel
    {
        public int SlotType { get; set; }
        public List<RescheduleSlot> RecheduleSlots { get; set; }
        public int HiringRequest_ID { get; set; }
        public int HiringRequest_Detail_ID { get; set; }
        public int ContactID { get; set; }
        public int Talent_ID { get; set; }
        public int InterviewStatus { get; set; }
        public int InterviewMasterID { get; set; }
        public string HiringRequestNumber { get; set; }
        public int WorkingTimeZoneID { get; set; }
        public string ShortListedID { get; set; }
        public string Additional_Notes { get; set; }
        public string InterviewCallLink { get; set; }
        //public string BearerToken { get; set; }
    }

    public class RescheduleSlot
    {
        public int SlotID { get; set; }
        public string? SlotDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }

        public string STRSlotDate { get; set; }
        public string STRStartTime { get; set; }
        public string STREndTime { get; set; }
        public string ID_As_ShortListedID { get; set; }
    }
}
