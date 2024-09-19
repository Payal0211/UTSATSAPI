namespace UTSATSAPI.Models.ViewModels
{
    public class GiveUserDetailsToATS
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public string? contact_number { get; set; }
        public string? skype { get; set; }
        public string? designation { get; set; }
        public string? department { get; set; }
        public string? employee_id { get; set; }
        public ReportingUserDetailsToATS reporting_to { get; set; }
        public bool? IsActive { get; set; }
    }
    public class ReportingUserDetailsToATS
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public string? contact_number { get; set; }
        public string? skype { get; set; }
        public string? designation { get; set; }
        public string? department { get; set; }
        public string? employee_id { get; set; }
    }
}
