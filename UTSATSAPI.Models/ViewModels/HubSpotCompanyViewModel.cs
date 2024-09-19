namespace UTSATSAPI.Models.ViewModels
{
    public class HubSpotCompanyViewModel
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public string? Sortdatafield { get; set; }
        public string? Sortorder { get; set; }
        public FilterFields_HubSpotCompanyList? FilterFields_HubSpotCompanyList { get; set; }
    }
    public class FilterFields_HubSpotCompanyList
    {
        public string? Company { get; set; }
        public string? Website { get; set; }
        public string? Domain { get; set; }
    }
}
