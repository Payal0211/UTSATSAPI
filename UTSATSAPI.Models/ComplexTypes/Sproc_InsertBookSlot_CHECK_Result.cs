using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_InsertBookSlot_CHECK_Result
    {
        public long ID { get; set; }
        public string ReturnMessage { get; set; }
    }
}
