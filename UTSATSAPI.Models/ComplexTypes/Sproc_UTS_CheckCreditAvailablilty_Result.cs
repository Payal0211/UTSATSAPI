using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_CheckCreditAvailablilty_Result
    {
        public long? CompanyID { get; set; } 
        public string? CompanyName { get; set; }
        public decimal? JPCreditBalance { get; set; }
        public int? JobPostCredit { get; set; }
        public bool? IsPostaJob { get; set; }
        public bool? IsProfileView { get; set; }
        public bool? IsCreditAvailable { get; set; }
    }
}
