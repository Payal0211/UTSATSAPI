using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_OnBoardSummary_Result
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public int RowID { get; set; }
    }
}
