using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_UTS_GetClientWiseHRDetails_Result
    {
        public long? HR_ID { get; set; }
        public bool? StarMarked { get; set; }
        public int? StarMarkedStatusCode { get; set; }
        public string? SalesUserName { get; set; }
        public DateTime? CreatedDateTime { get; set;}
        public string? HRNumber { get; set;}
        public double? TotalTR { get; set; }
        public string? Position { get; set;}
        public string? Cost { get; set;}
        public string? Notice { get; set;}
        public string? FTE_PTE { get; set;}
        public string? GUID { get; set;}
        public bool? IsFrontEndDraftHR { get; set;}
    }
}
