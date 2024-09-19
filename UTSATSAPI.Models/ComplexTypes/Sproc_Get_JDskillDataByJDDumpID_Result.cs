
using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_JDskillDataByJDDumpID_Result
    {
        public string? JDSkills { get; set; }
    }
}
