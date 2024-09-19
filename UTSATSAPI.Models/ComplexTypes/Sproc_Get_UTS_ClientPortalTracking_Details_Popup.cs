using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_UTS_ClientPortalTracking_Details_Popup_Result
    {
        public long TrackingID { get; set; }
        public long? ClientID { get; set; }
        public string? Client { get; set; }
        public long? HRID { get; set; }
        public string? HRNumber { get; set; }
        public long? TalentID { get; set; }
        public string? TalentName { get; set; }
        public string? TalentStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Location { get; set; }
        public string? Device { get; set; }
        public string? IPAddress { get; set; }
        public string? Browser { get; set; }
        public string? Actions { get; set; }
        public string? RejectionReason { get; set; }
        public string? RejectionStage { get; set; }
    }
}
