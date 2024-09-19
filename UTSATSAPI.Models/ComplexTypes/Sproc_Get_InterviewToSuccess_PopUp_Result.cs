using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_InterviewToSuccess_PopUp_Result
    {
        public string? HR_Number { get; set; }
        public string? SalesUser { get; set; }
        public string? Company { get; set; }
        public string? TalentRole { get; set; }
        public string? IsManaged { get; set; }
        public string? Name { get; set; }
        public string? Availability { get; set; }
    }
}
