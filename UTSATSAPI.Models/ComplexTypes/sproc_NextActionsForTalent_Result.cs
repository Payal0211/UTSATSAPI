using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_NextActionsForTalent_Result
    {
        public string? NextActionText { get; set; }
    }
}
