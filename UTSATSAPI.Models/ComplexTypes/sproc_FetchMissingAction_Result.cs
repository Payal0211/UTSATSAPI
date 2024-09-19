using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_FetchMissingAction_Result
    {
        public long ActionID { get; set; }
        public string? ActionName { get; set; }
        public int ActionSequence { get; set; }
        public long NextActionID { get; set; }
        public string? NextActionName { get; set; }
        public Nullable<int> NextActionSequence { get; set; }
        public bool IsTalentAction { get; set; }
        public string? CTAName { get; set; }
    }
}
