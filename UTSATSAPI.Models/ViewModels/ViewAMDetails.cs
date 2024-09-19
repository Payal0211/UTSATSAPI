using Microsoft.AspNetCore.Mvc.Rendering;

namespace UTSATSAPI.Models.ViewModels
{
    public class ViewAMDetails
    {
        public long? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public long? ClientID { get; set; }
        public string? ClientName { get; set; }
        public string? GEO { get; set; }
        public int? OldAM_SalesPersonID { get; set; }
        public string? OldAM_UserName { get; set; }
        public IEnumerable<SelectListItem> AMList { get; set; }
    }

    public class UpdateAMDetails
    {
        public long? CompanyID { get; set; }
        public int? OldAM_SalesPersonID { get; set; }
        public int? NewAM_SalesPersonID { get; set; }
        public string? Comment { get; set; }
    }
}
