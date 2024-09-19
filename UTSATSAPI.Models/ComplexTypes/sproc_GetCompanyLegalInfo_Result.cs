using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_GetCompanyLegalInfo_Result
    {
        public long ID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string DocumentURL { get; set; }
        public string AgreementStatus { get; set; }
        public int TotalRecords { get; set; }
        public string SignedDate { get; set; }
        public string Validity_StartDate { get; set; }
        public string Validity_EndDate { get; set; }
        public long CompanyID { get; set; }
        public string Company { get; set; }
    }
}
