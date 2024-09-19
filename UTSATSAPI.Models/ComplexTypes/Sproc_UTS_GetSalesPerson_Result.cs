using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_GetSalesPerson_Result
    {
        public long UserID { get; set; }
        public string? UserName { get; set; }
    }
}
