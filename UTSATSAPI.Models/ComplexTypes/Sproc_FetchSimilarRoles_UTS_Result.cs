using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_FetchSimilarRoles_UTS_Result
    {
        public Int64? RoleID { get; set; }
    }
}
