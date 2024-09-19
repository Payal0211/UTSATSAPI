using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_InsertBookSlot_Result
    {
        public Nullable<int> ID { get; set; }
        public string ReturnMessage { get; set; }
    }
}
