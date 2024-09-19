using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes.UpChat
{
    [Keyless]
    public class Sproc_GetChannels_Result
    {
        public long? HRID { get; set; }
        public string? CompanyInitial { get; set; }
        public string? CompanyName { get; set; }
        public string? Role { get; set; }
        public string? HRNumber { get; set; }
        public string? HRStatus { get; set; }
        public int? HRStatusID { get; set; }
        public bool? IsPinned { get; set; }
        public bool? IsSnoozed { get; set; }
    }
}
