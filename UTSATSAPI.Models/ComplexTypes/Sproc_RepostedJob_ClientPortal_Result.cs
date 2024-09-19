using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_RepostedJob_ClientPortal_Result
    {
        public string? Message { get; set; }
        public byte? Status { get; set; }
        public bool? IsReopen { get; set; }
    }
}
