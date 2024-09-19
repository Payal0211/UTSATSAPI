namespace UTSATSAPI.Models.ViewModels
{
    public class AMAssignmentRulesViewModel
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public string? Sortdatafield { get; set; }
        public string? Sortorder { get; set; }
        public FilterFields_AMAssignmentRules? FilterFields_AMAssignmentRules { get; set; } 
    }
    public class FilterFields_AMAssignmentRules
    {
        public string? Description { get; set; }
        public int? Priority { get; set; }
    }
}
