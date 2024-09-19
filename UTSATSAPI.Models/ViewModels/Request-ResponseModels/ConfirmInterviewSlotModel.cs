namespace UTSATSAPI.Models.ViewModels.Request_ResponseModels
{
    public class ConfirmInterviewSlotModel
    {
        public long HiringRequest_ID { get; set; }
        public long HiringRequest_Detail_ID { get; set; }
        public long ContactID { get; set; }
        public long Talent_ID { get; set; }
        public long InterviewMasterID { get; set; }
        public long ShortListedID { get; set; }
        //public string BearerToken { get; set; }
    }
}
