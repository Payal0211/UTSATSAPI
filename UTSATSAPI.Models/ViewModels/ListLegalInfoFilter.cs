namespace UTSATSAPI.Models.ViewModels
{
    public class ListLegalInfoFilter
    {

        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public FilterFields_LegalInfo FilterFields_LegalInfo { get; set; }
      
    }

    public class FilterFields_LegalInfo
    {
        public long CompanyId { get; set; }
        public string DocumentType { get; set; }

        public string DocumentName { get; set; }
        public string AgreementStatus { get; set; }

        public string Company { get; set; }
    }
}
