using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class SP_UTS_ESales_Get_Client_AM_Details_Result
    {
        public string Managed_SelfManagedHR { get; set; }
        public string GEO { get; set; }
        public string HR_Number { get; set; }
        public string EngagemenID { get; set; }
        public string CompanyName { get; set; }
        public string EngagementModel { get; set; }
        public string NBD_Lead_EmployeeID { get; set; }
        public string SalesPerson_EmployeeID { get; set; }
        public string CreatedByEmployeeId { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public int CompanySize { get; set; }
        public string phone { get; set; }
        public string industry { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string Zip { get; set; }
        public string CompanySource { get; set; }
        public string LinkedInProfile { get; set; }
        public string TalentName { get; set; }
    }
}
