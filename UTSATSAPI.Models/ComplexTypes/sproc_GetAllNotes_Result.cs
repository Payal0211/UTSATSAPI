using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_GetAllNotes_Result
    {
        public string Notes { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> CreatedByDatetime { get; set; }
    }
}
