using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels
{
    public class PostAcceptanceVM
    {
        public long TalentID { get; set; }
        public List<Sproc_GET_PostAcceptance_detail_Result> postAcceptanceDetail { get; set; }
        public List<Sproc_GET_PostAcceptance_detail_Availability_Result> postAcceptanceDetailAvailability { get; set; }
        public List<Sproc_GET_PostAcceptance_detail_HowSoon_Result> postAcceptanceDetailHowSoon { get; set; }
        public bool? IsClientNotificationSent { get; set; }
        public long ContactID { get; set; }
        public string PageName { get; set; }
    }
}
