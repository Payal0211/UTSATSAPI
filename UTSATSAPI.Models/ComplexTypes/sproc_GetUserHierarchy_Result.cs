using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_GetUserHierarchy_Result
    {
        public Nullable<long> ID { get; set; }
        public string UserName { get; set; }
    }
}
