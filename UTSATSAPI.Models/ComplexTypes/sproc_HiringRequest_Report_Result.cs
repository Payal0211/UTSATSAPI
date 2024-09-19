using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_HiringRequest_Report_Result
    {
        public string StageName { get; set; }
        public int StageValue { get; set; }

    }
}
