namespace UTSATSAPI.Models.ViewModels
{
    public class TalentLegalInfo
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public string? Sortdatafield { get; set; }
        public string? Sortorder { get; set; }
        public FilterFields_TalentLegalInfo? FilterFields_TalentLegalInfo { get; set; }
    }

    public class FilterFields_TalentLegalInfo
    {
        public int? TalentId { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentName { get; set; }
        public string? AgreementStatus { get; set; }
        public string? Name { get; set; }
        public string? EmailId { get; set; }
    }
}
