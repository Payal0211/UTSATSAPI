using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GetCompanyDetail_Result
    {
        public string Company { get; set; }
        public string Location { get; set; }
        public Nullable<int> CompanySize { get; set; }
        public string CompanyDomain { get; set; }
        public string LinkedInProfile { get; set; }
    }
}
