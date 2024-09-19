using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_GetSupplyWiseChannelWiseFunnelReportData_For_PopUP_Result
    {
        public string HR_No { get; set; }
        public string SalesPerson { get; set; }
        public string CompnayName { get; set; }
        public string Role { get; set; }
        public string Managed_Self { get; set; }
        public string TalentName { get; set; }
        public string Availability { get; set; }
    }
}
