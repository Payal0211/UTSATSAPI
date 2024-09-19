using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.Models
{
    [Keyless]
    public class sproc_ViewAllUnAssignedHRs_Result
    {
        public long ID { get; set; }
        public long HRID { get; set; }
        public long? CompanyID { get; set; }
        public bool StarMarked { get; set; }
        public int StarMarkedStatusCode { get; set; }
        public string? AdHocHR { get; set; }
        public string? CreatedDateTime { get; set; }
        public string? HR { get; set; }
        public double TR { get; set; }
        public string? Position { get; set; }
        public string? Company { get; set; }
        public string? TimeZone { get; set; }
        public string? TypeOfEmployee { get; set; }

        public long UserId { get; set; }
        public string? SalesRep { get; set; }
        public string? HRStatus { get; set; }
        public int HRStatusCode { get; set; }
        public int TotalRecords { get; set; }
        public int ReopenHR { get; set; } //property to handle if HR can be reopened or not.
        public string? CompanyCategory { get; set; } //Represents Company Category.
        public string? Cost { get; set; } //Represents Budget 
        public string? HowSoon { get; set; } //Represents Notice 
        public bool IsHRFocused { get; set; } //Represents whether the HR is focused or not. 
        public string? TypeOfHR { get; set; } //Represents whether the HR is DP or Contractual. 
        public int? HRTypeId { get; set; } //For HR Type ID: Sales/Direct/ClientPortal(3)
        public string? HRTypeName { get; set; } //For HR Type Name: Sales/Direct/ClientPortal(3)
        public string? CompanyModel { get; set; }
        public bool? IsDisplayReopenOrCloseIcon { get; set; } //UTS-6409 Pay per credit based HR : Closed & Reopen CTA will not be available in UTS admin hr list
        public bool? IsHybrid { get; set; } //UTS-6616 Clone HR changes
        public bool? ShowCloneToDemoAccount { get; set; } // UTS-7517 : Show check for pay per hire HR to add in Demo account
        public string? HRAcceptedSince { get; set; }
        public string? HREngagementType { get; set; }
        public string? SalaryBudget { get; set; }
    }
}
