using Microsoft.AspNetCore.Mvc.Rendering;

namespace UTSATSAPI.Models.ViewModels
{
    public  class IncentiveReportViewModel
    {
        public long UserId { get; set; }
        public List<SelectListItem> SalesUserDDL { get; set; }
        public int UserRoleId { get; set; }
        public int UserTypeId { get; set; }
        public List<SelectListItem> SalesUserRoleDDL { get; set; }
        public IEnumerable<SelectListItem> UserDDL { get; set; }
        public bool IsLastDayOfMonth { get; set; }
    }
}