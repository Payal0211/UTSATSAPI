using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_HRDetails_For_UserPriority_Email_Result
    {
        public string HR_Number { get; set; }
        public string Company { get; set; }
        public string HRRole { get; set; }
        public string HRStatus { get; set; }
        public string Talent { get; set; }
    }
}
