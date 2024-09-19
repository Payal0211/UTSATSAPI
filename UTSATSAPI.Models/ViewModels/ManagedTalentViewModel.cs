using Microsoft.AspNetCore.Mvc;

namespace UTSATSAPI.Models.ViewModels
{
    public class ManagedTalentViewModel
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public string? Sortdatafield { get; set; }
        public string? Sortorder { get; set; }
        public FilterFields_ManagedTalent? FilterFields_ManagedTalent { get; set; }
    }

    public class FilterFields_ManagedTalent
    {
        public string? ManagedTalentFirstName { get; set; }
        public string? ManagedTalentLastName { get; set; }
        public string? ManagedTalentEmailID { get; set; }
        public string? TalentRole { get; set; }
        public string? ManagedTalent_Level { get; set; }
        public string? Talent_Type { get; set; }
        public string? NRPercentage { get; set; }
        public string? HR_Number { get; set; }
        public string? ManagedTalentCost { get; set; }
        public string? HRCost { get; set; }
        public string? POC_FullName { get; set; }
    }
}
