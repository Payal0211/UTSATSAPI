using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes.UpChat
{
    [Keyless]
    public class Sproc_UpChat_GetHRNotes_Result
    {
        public long? NoteID { get; set; }
        public long? HiringRequest_ID { get; set; }
        public string? Notes { get; set; }
        public string? UserName { get; set; }
        public string? UserEmpID { get; set; }
        public string? UserDesignation { get; set; }
        public int? CreatedByID { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
