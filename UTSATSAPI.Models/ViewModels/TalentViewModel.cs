using Microsoft.AspNetCore.Mvc;

namespace UTSATSAPI.Models.ViewModels
{
    public class TalentViewModel
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public string? Sortdatafield { get; set; }
        public string? Sortorder { get; set; }
        public FilterFields_Talent? FilterFields_Talent { get; set; }
    }

    public class FilterFields_Talent
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? EmailID { get; set; }
        public string? ContactNumber { get; set; }
        public string? Status { get; set; }
        public string? UserType { get; set; }
        public string? TalentRole { get; set; }
        public string? AccountStatus { get; set; }
        public string? AfterClientSelectionStatus { get; set; }
        public string? TalentCategory { get; set; }
        public string? Talent_Type { get; set; }
        public string? FinalCost { get; set; }
    }
}
