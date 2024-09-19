namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class ClientPortalTrackingDetailFilter
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public long? ClientID { get; set; }
    }
    public class ClientPortalTrackingDetail_Popup_Filter
    {
        public int? ActionID { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public long? ClientID { get; set; }
    }
}
