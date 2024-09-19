using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_ClientBasedReport_WithHubSpot_PopUp_Result
    {
        public string? FullName { get; set; }
        public string? Company { get; set; }
        public string? GEO { get; set; }
        public string? SalesUser { get; set; }
        public string? Hr_Number { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
    }
}