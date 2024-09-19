using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_DashBoard_OnBoardCount_Result
    {
        public long TotalEnagagement { get; set; }
        public long TotalActiveEngagement { get; set; }
    }
}
