using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class SP_NextActionsForManagedHR_Result
    {
        public string? NextActionText { get; set; }
    }
}
