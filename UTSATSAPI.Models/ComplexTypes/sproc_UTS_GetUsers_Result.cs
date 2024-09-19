using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetUsers_Result
    {
        public long? ID { get; set; }
        public string? CreatedOn { get; set; }
        public string? EmployeeID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Designation { get; set; }
        public string? UserType { get; set; }
        public string? NBD_AM { get; set; }
        public string? UserLevel { get; set; }
        public string? Department { get; set; }
        public string? Manager { get; set; }
        public string? Skype { get; set; }
        public string? Contact { get; set; }
        public string? UserName { get; set; }                
        public int? UserTypeID { get; set; }
        public int? TotalRecords { get; set; }                           
        public int? PriorityCount { get; set; }
        public string? OpsManager { get; set; }
        public int? RemainingPriorityCount { get; set; }
        public string? GEO { get; set; }
    }
}
