using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_ClientList_Result
    {
        public long ClientID { get; set; }
        public string? ClientName { get; set; }
    }
}
