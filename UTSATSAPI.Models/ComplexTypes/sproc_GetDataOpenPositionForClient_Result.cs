using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.ComplexTypes
{
    [Keyless]
    public class sproc_GetDataOpenPositionForClient_Result
    {
        public string RequestForTalent { get; set; }
        public string TalentRole { get; set; }
        public string Discription { get; set; }
        public int NoofEmployee { get; set; }
        public string Duration { get; set; }
        public string DurationType { get; set; }
        public string Cost { get; set; }
        public string Availability { get; set; }
        public string Timezone_Preference { get; set; }
        public string HRJoining { get; set; }
        public string HR_Number { get; set; }
        public string HR_Status { get; set; }
        public string FrontIconImage { get; set; }
    }
}
