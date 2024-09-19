using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_Inc_GetUsers_RoleDetails_Result
    {
        public long ID { get; set; }
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public int TotalRecords { get; set; }
        public string UserRole { get; set; }
    }
}
