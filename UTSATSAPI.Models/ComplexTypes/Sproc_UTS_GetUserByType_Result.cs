using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_GetUserByType_Result
    {
        public int ID { get; set; }
        public string? UserRole { get; set; }
    }
}
