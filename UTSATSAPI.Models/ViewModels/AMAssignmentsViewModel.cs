using Microsoft.AspNetCore.Mvc;

namespace UTSATSAPI.Models.ViewModels
{
    public class AMAssignmentsViewModel
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public string? Sortdatafield { get; set; }
        public string? Sortorder { get; set; }
        public FilterFields_AMAssignments? FilterFields_AMAssignments { get; set; }
    }
    public class FilterFields_AMAssignments
    {
        public string? GEOName { get; set; }
        public string? UserName { get; set; }
        public int? Priority { get; set; }
    }
}
