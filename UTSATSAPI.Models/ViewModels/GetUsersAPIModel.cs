using Microsoft.AspNetCore.Mvc;

namespace UTSATSAPI.Models.ViewModel
{
    public class GetUsersAPIModel
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public string? Sortdatafield { get; set; }
        public string? Sortorder { get; set; }
        public string? searchText { get; set; }
        public FilterFields_UserList? FilterFields_UserList { get; set; }
    }
    public class FilterFields_UserList
    {
        public string? EmployeeID { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? EmailId { get; set; }
        public string? ContactNumber { get; set; }
        public string? UserType { get; set; }
        public string? TeamManager { get; set; }
        public string? PriorityCount { get; set; }
        public string? OpsManager { get; set; }
        public string? NBD_AM { get; set; }
    }
}
