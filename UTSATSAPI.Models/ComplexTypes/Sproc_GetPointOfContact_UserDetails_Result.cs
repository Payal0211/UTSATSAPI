using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GetPointOfContact_UserDetails_Result
    {
        public string UserName { get; set; }
        public long UserID { get; set; }
    }
}
