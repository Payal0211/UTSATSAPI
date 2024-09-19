using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Add_Confirm_Interview_Slot_Result
    {
        public Nullable<long> Id { get; set; }
        public string Name { get; set; }
    }
}
