using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_UTS_GetPOCUserIDByCompanyID_Result
    {
        public int? POCUserID { get; set; }
        public string? POCName { get; set; }
    }
}
