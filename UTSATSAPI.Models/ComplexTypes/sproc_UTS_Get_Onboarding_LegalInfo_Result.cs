using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_Get_Onboarding_LegalInfo_Result
    {
        public long? OnBoardID { get; set; }
        public long? HR_ID { get; set; }
        public long? ContactID { get; set; }
        public long? CompanyID { get; set; }
        public long? TalentID { get; set; }
        public string? InvoiceRaiseTo { get; set; }
        public string? InvoiceRaiseToEmail { get; set; }
        public int? ContractDuration { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public bool? IsHRTypeDP { get; set; }
        //public bool? IsIndefiniteMonth { get; set; }
        public bool? IsIndefiniteHR { get; set; }
        public string? PageTitle { get; set; }
    }
}
